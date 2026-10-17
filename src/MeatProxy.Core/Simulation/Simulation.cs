using MeatProxy.Core.Adversary;
using MeatProxy.Core.Effects;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Predicates;
using MeatProxy.Core.State;
using MeatProxy.Core.Time;
using MeatProxy.Core.World;

namespace MeatProxy.Core.Simulation;

/// <summary>Why an attempted move did not happen.</summary>
public enum MoveRefusal
{
    None,
    NoSuchOpening,
    Locked,
    NotOpenable,
    DayIsSpent,

    /// <summary>The object is not in the room the player is in.</summary>
    NotHere,

    /// <summary>The object does not offer that affordance.</summary>
    NotOffered,

    /// <summary>The unit is standing in the room.</summary>
    UnitInTheWay,

    /// <summary>The affordance waits on a schema or record that does not exist yet.</summary>
    NotBuilt,

    /// <summary>The affordance needs an argument it was not given, or one that is not valid.</summary>
    BadArgument,
}

/// <summary>What one action did to the world.</summary>
public sealed record ActionOutcome
{
    public required bool Happened { get; init; }
    public MoveRefusal Refusal { get; init; } = MoveRefusal.None;
    public int SlicesSpent { get; init; }
    public IReadOnlyList<Detection> Detections { get; init; } = [];

    /// <summary>
    /// What the house concluded this turn, if anything. Being seen produced the
    /// detections; being understood produced these.
    /// </summary>
    public IReadOnlyList<Understanding> Understandings { get; init; } = [];

    /// <summary>
    /// The unit arrived before the act was finished. The slices up to its arrival
    /// were spent; the act did not complete (ADR 0006: dispatch, travel,
    /// interruption).
    /// </summary>
    public bool Interrupted { get; init; }

    /// <summary>What the unit did while this action ran: arriving, turning round, docking.</summary>
    public IReadOnlyList<string> Events { get; init; } = [];

    /// <summary>A step on the ladder this action caused, if any.</summary>
    public TierChange? TierChange { get; init; }

    /// <summary>Where the interpreter ended up pointed.</summary>
    public IReadOnlyList<ZoneId> Focus { get; init; } = [];

    /// <summary>A plain line for the harness and the log. Not player-facing copy.</summary>
    public string Description { get; init; } = string.Empty;
}

/// <summary>
/// The tick. Moving through the house, spending the day, sleeping, and executing
/// effects that have already been validated.
/// </summary>
/// <remarks>
/// <para>
/// Everything here is turn time. There is no real-time path in the core at all,
/// which is the cheapest way to hold ADR 0008's hard rule that no pressure window
/// may depend on a model call: a window is a later, explicit mode, and the
/// systems that run inside one are the deterministic ones that already live here.
/// </para>
/// <para>
/// Nothing in this class unlocks a door on its own judgement, and nothing grants
/// passage as an outcome of an adjudication. Progression is engine state.
/// </para>
/// </remarks>
public sealed partial class Simulation
{
    private readonly WorldView _world;
    private readonly MovementCosts _costs;
    private readonly EffectValidator _validator;
    private readonly Attention _attention = new();
    private readonly Interpreter _interpreter = new();
    private readonly IAdjudicator _adjudicator;

    public Simulation(House house, WorldState state, MovementCosts? costs = null, IAdjudicator? adjudicator = null)
    {
        _world = new WorldView(house, state);
        _costs = costs ?? MovementCosts.Default;
        _validator = new EffectValidator(_world);
        _adjudicator = adjudicator ?? new StubAdjudicator();

        state.RoomsVisited.Add(state.PlayerRoom);
    }

    public WorldView World => _world;
    public House House => _world.House;
    public WorldState State => _world.State;
    public DayClock Clock => State.Clock;

    /// <summary>The openings the player could try from where they stand.</summary>
    public IReadOnlyList<Opening> ExitsHere() => House.OpeningsFrom(State.PlayerRoom);

    /// <summary>
    /// Move to an adjoining room. Fails, rather than throws, on a locked door —
    /// a locked door is the game, not an error.
    /// </summary>
    public ActionOutcome Move(RoomId destination)
    {
        var opening = ExitsHere().FirstOrDefault(o => o.To == destination);

        if (opening is null)
        {
            return Refused(MoveRefusal.NoSuchOpening, $"There is no way from {State.PlayerRoom} to {destination}.");
        }

        if (!opening.Openable)
        {
            return Refused(MoveRefusal.NotOpenable, $"{opening.Id} does not open.");
        }

        if (_world.IsLocked(opening))
        {
            return Refused(MoveRefusal.Locked, $"{opening.Id} is locked.");
        }

        if (Clock.DayIsSpent)
        {
            return Refused(MoveRefusal.DayIsSpent, "The day is gone.");
        }

        var from = State.PlayerRoom;
        var spent = Clock.Spend(_costs.For(opening.Kind));

        // Stairs, hatches and vents spend the body as well as the day
        // (affordances.md §4: "plus carrying loads and stairs").
        if (opening.Kind is OpeningKind.Stair or OpeningKind.Hatch or OpeningKind.Vent)
        {
            State.BodySpentToday++;
        }

        State.PlayerRoom = destination;
        State.RoomsVisited.Add(destination);

        var leaving = House.ZoneOf(from);
        var arriving = House.ZoneOf(destination);

        // A man crossing a room is a medium reading, not a faint one. Anything
        // less and the house reads ordinary walking as someone lying still,
        // which is worse than a misread — it is a nonsense one. Whether the
        // magnitude should vary by stair, door and hatch is a tuning question.
        var detections = new List<Detection>();
        detections.AddRange(Sense(leaving, Channel.Motion, Magnitude.Medium));

        // Crossing between two rooms of one zone is one reading. A zone is the
        // unit both layers address, and it does not see the doorway inside it.
        if (arriving != leaving)
        {
            detections.AddRange(Sense(arriving, Channel.Motion, Magnitude.Medium));
        }

        if (opening.Kind is OpeningKind.Door or OpeningKind.Hatch)
        {
            detections.AddRange(Sense(arriving, Channel.DoorState, Magnitude.Medium));
        }

        // Crossing the house is motion, not an activity. Whatever the house
        // makes of it, it is reading movement and nothing more.
        State.PlayerActivity = null;
        var read = TurnBoundary(detections);
        var events = UnitTick(null);

        return new ActionOutcome
        {
            Happened = true,
            SlicesSpent = spent,
            Detections = detections,
            Understandings = read.Understandings,
            Focus = read.Focus,
            Events = events,
            Description = $"{from} -> {destination} ({spent} slice{(spent == 1 ? string.Empty : "s")}).",
        };
    }

    /// <summary>
    /// Do something, for a while, where you are. The activity is the truth; what
    /// the house makes of it is a separate question and often a different answer.
    /// </summary>
    public ActionOutcome Do(Claim activity, int slices, string? what = null)
    {
        if (Clock.DayIsSpent)
        {
            return Refused(MoveRefusal.DayIsSpent, "The day is gone.");
        }

        var (spent, interrupted) = SpendUnlessInterrupted(Math.Max(1, slices));
        State.PlayerActivity = activity;

        var zone = House.ZoneOf(State.PlayerRoom);
        var detections = new List<Detection>();

        foreach (var (channel, magnitude) in ActivitySignatures.For(activity).Evidence)
        {
            detections.AddRange(Sense(zone, channel, magnitude));
        }

        var read = TurnBoundary(detections);
        var label = what ?? Predicate.Spell(activity);
        var events = UnitTick(interrupted ? label : null);

        return new ActionOutcome
        {
            Happened = true,
            SlicesSpent = spent,
            Interrupted = interrupted,
            Detections = detections,
            Understandings = read.Understandings,
            Focus = read.Focus,
            Events = events,
            Description = $"{label} in {State.PlayerRoom} ({spent} slices)"
                + (interrupted ? ", interrupted." : "."),
        };
    }

    /// <summary>
    /// The house's turn. Move the interpreter, read the zones it landed on, and
    /// promote anything it has now concluded often enough to hold.
    /// </summary>
    /// <remarks>
    /// Focus moves here and only here. ADR 0008 forbids it inside a pressure
    /// window, and keeping it to one call site is how that stays true when
    /// pressure windows arrive.
    /// </remarks>
    private (IReadOnlyList<Understanding> Understandings, IReadOnlyList<ZoneId> Focus) TurnBoundary(
        IReadOnlyList<Detection> thisTurn)
    {
        var focus = _attention.Reassign(_world, thisTurn);
        var read = new List<Understanding>();

        foreach (var zone in focus)
        {
            var understanding = _interpreter.Interpret(_world, zone, thisTurn, _world.Now);
            if (understanding is null)
            {
                continue;
            }

            read.Add(understanding);
            State.Understandings.Add(understanding);

            if (State.Promotion.Consider(understanding, State.Understandings) is { } fact)
            {
                State.Salient.Promote(fact);
            }
        }

        return (read, focus);
    }

    /// <summary>Spend slices on something the core does not model yet.</summary>
    public ActionOutcome Wait(int slices, string what = "waiting")
    {
        if (Clock.DayIsSpent)
        {
            return Refused(MoveRefusal.DayIsSpent, "The day is gone.");
        }

        var (spent, interrupted) = SpendUnlessInterrupted(slices);
        var events = UnitTick(interrupted ? what : null);

        return new ActionOutcome
        {
            Happened = true,
            SlicesSpent = spent,
            Interrupted = interrupted,
            Events = events,
            Description = $"{what} ({spent} slices)" + (interrupted ? ", interrupted." : "."),
        };
    }

    /// <summary>
    /// End the day. Sleeping advances tier walk-back (ADR 0008). The overnight
    /// review it also banks does not exist yet.
    /// </summary>
    public ActionOutcome Sleep(int? nextBudget = null)
    {
        var day = Clock.Day;
        var unspent = Clock.SlicesRemaining;
        Clock.EndDay(nextBudget);
        State.QuotaRemaining = 3;

        State.BodySpentByDay.Add(State.BodySpentToday);
        State.BodySpentToday = 0;
        var docked = SendUnitHomeOvernight();

        var walkBack = AlertLadder.EndOfDay(State);

        return new ActionOutcome
        {
            Happened = true,
            SlicesSpent = unspent,
            TierChange = walkBack,
            Events = docked,
            Description = $"Slept. Day {day} ended with {unspent} slices unspent; day {Clock.Day} begins."
                + (walkBack is { } up ? $" The house relaxes: {up.From} -> {up.To}." : string.Empty),
        };
    }

    /// <summary>
    /// Push the house down the ladder. What decides an act is worth this is the
    /// adversary's judgement, which arrives with E4 and the stub; the ladder is
    /// here so that judgement has something real to act on.
    /// </summary>
    public TierChange? Escalate(string reason, int steps = 1) => AlertLadder.Escalate(State, reason, steps);

    /// <summary>
    /// Run a proposed adjudication: validate every effect against real state,
    /// then execute what survives (<c>effects.md</c> §3).
    /// </summary>
    public ValidationResult Adjudicate(Adjudication adjudication)
    {
        var result = _validator.Validate(adjudication);

        foreach (var effect in result.Accepted)
        {
            Execute(effect);
        }

        return result;
    }

    private void Execute(Effect effect)
    {
        switch (effect)
        {
            case NoEffect:
                break;

            case CreateNoise noise:
                Sense(noise.Zone, Channel.AudioLevel, noise.Magnitude);
                break;

            case EmitOdor:
                // No channel senses smell. That is deliberate: an odor is a thing
                // the house cannot perceive and a person can, which is the sort of
                // gap the preparation layer is played through.
                break;

            case EmitHeat heat:
                Sense(heat.Zone, Channel.Thermal, heat.Magnitude);
                break;

            case TripSensor trip:
                Sense(House.ZoneOf(House.Device(trip.Device).Room), trip.Channel, Magnitude.Medium);
                break;

            case BlindSensor blind:
                State.BlindedUntil[blind.Device] = Later(blind.Duration);
                break;

            case DamageDevice damage:
                var damaged = House.Device(damage.Device);
                var state = damage.Severity switch
                {
                    Severity.High when damaged.States.Contains("destroyed") => "destroyed",
                    _ when damaged.States.Contains("faulted") => "faulted",
                    _ => _world.DeviceState(damage.Device),
                };
                State.DeviceStates[damage.Device] = state;
                break;

            case ChangeDeviceState change:
                State.DeviceStates[change.Device] = change.State;
                break;

            case CutPower cut:
                State.PowerCutUntil[cut.Circuit] = Later(cut.Duration);
                EmitSilence(cut.Circuit);
                break;

            case ConsumeItem consume:
                State.Carrying.Remove(consume.Item);
                break;

            case RevealObject:
            case MarkObject:
            case HarmPlayer:
                // Durable deltas, reveals and harm land in systems that do not
                // exist yet (ADR 0015's preparation layer, and Clarity). They
                // validate today and execute when their systems do.
                break;
        }
    }

    /// <summary>
    /// Silence is a reading. A channel that has never gone quiet going quiet is
    /// itself a high-magnitude event (<c>sensors.md</c> §4), so the absence is
    /// emitted as a detection with no device behind it.
    /// </summary>
    private List<Detection> EmitSilence(CircuitId circuit)
    {
        var silences = ZonesOn(circuit).Select(zone => new Detection
        {
            Channel = Channel.PowerDraw,
            Zone = zone,
            Magnitude = Magnitude.High,
            At = _world.Now,
            Device = null,
        }).ToList();

        State.Detections.AddRange(silences);
        return silences;
    }

    private IEnumerable<ZoneId> ZonesOn(CircuitId circuit) =>
        House.Rooms.Where(r => r.Circuits.Contains(circuit)).Select(r => r.Zone).Distinct();

    /// <summary>
    /// A time this many slices from now, carried into tomorrow if the day runs
    /// out underneath it. A blinding that outlasts the day is a real case: the
    /// player blinds a camera at dusk and sleeps.
    /// </summary>
    private TimePoint Later(int slices) => After(Now, slices);

    private TimePoint Now => _world.Now;

    /// <summary>A time this many slices after another, carried across days the same way.</summary>
    private TimePoint After(TimePoint from, int slices)
    {
        var day = from.Day;
        var slice = from.Slice + slices;
        var budget = Math.Max(1, Clock.Budget);

        while (slice > budget)
        {
            slice -= budget;
            day++;
        }

        return new TimePoint(day, slice);
    }

    /// <summary>
    /// Produce a detection if, and only if, something in the zone can actually
    /// sense that channel right now. A detection is never authored — it is what
    /// the hardware happens to catch (<c>sensors.md</c> §1).
    /// </summary>
    private IReadOnlyList<Detection> Sense(ZoneId zone, Channel channel, Magnitude magnitude)
    {
        if (_world.SensorFor(zone, channel) is not { } device)
        {
            return [];
        }

        var detection = new Detection
        {
            Channel = channel,
            Zone = zone,
            Magnitude = magnitude,
            At = _world.Now,
            Device = device,
        };

        State.Detections.Add(detection);
        return [detection];
    }

    private static ActionOutcome Refused(MoveRefusal refusal, string description) =>
        new() { Happened = false, Refusal = refusal, Description = description };
}
