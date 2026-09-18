using MeatProxy.Core.Effects;
using MeatProxy.Core.Perception;
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
}

/// <summary>What one action did to the world.</summary>
public sealed record ActionOutcome
{
    public required bool Happened { get; init; }
    public MoveRefusal Refusal { get; init; } = MoveRefusal.None;
    public int SlicesSpent { get; init; }
    public IReadOnlyList<Detection> Detections { get; init; } = [];

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
public sealed class Simulation
{
    private readonly WorldView _world;
    private readonly MovementCosts _costs;
    private readonly EffectValidator _validator;

    public Simulation(House house, WorldState state, MovementCosts? costs = null)
    {
        _world = new WorldView(house, state);
        _costs = costs ?? MovementCosts.Default;
        _validator = new EffectValidator(_world);

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

        State.PlayerRoom = destination;
        State.RoomsVisited.Add(destination);

        var detections = new List<Detection>();
        detections.AddRange(Sense(House.ZoneOf(from), Channel.Motion, Magnitude.Low));
        detections.AddRange(Sense(House.ZoneOf(destination), Channel.Motion, Magnitude.Low));

        if (opening.Kind is OpeningKind.Door or OpeningKind.Hatch)
        {
            detections.AddRange(Sense(House.ZoneOf(destination), Channel.DoorState, Magnitude.Low));
        }

        return new ActionOutcome
        {
            Happened = true,
            SlicesSpent = spent,
            Detections = detections,
            Description = $"{from} -> {destination} ({spent} slice{(spent == 1 ? string.Empty : "s")}).",
        };
    }

    /// <summary>Spend slices on something the core does not model yet.</summary>
    public ActionOutcome Wait(int slices, string what = "waiting")
    {
        if (Clock.DayIsSpent)
        {
            return Refused(MoveRefusal.DayIsSpent, "The day is gone.");
        }

        var spent = Clock.Spend(slices);
        return new ActionOutcome
        {
            Happened = true,
            SlicesSpent = spent,
            Description = $"{what} ({spent} slices).",
        };
    }

    /// <summary>
    /// End the day. Sleeping banks the overnight review and advances tier
    /// walk-back (ADR 0008); neither exists yet, so this only turns the day over.
    /// </summary>
    public ActionOutcome Sleep(int? nextBudget = null)
    {
        var day = Clock.Day;
        var unspent = Clock.SlicesRemaining;
        Clock.EndDay(nextBudget);
        State.QuotaRemaining = 3;

        return new ActionOutcome
        {
            Happened = true,
            SlicesSpent = unspent,
            Description = $"Slept. Day {day} ended with {unspent} slices unspent; day {Clock.Day} begins.",
        };
    }

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

                // Silence is a reading. A channel that has never gone quiet going
                // quiet is itself a high-magnitude event (sensors.md §4), so the
                // absence is emitted as a detection with no device behind it.
                foreach (var zone in ZonesOn(cut.Circuit))
                {
                    State.Detections.Add(new Detection
                    {
                        Channel = Channel.PowerDraw,
                        Zone = zone,
                        Magnitude = Magnitude.High,
                        At = _world.Now,
                        Device = null,
                    });
                }

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

    private IEnumerable<ZoneId> ZonesOn(CircuitId circuit) =>
        House.Rooms.Where(r => r.Circuits.Contains(circuit)).Select(r => r.Zone).Distinct();

    /// <summary>
    /// A time this many slices from now, carried into tomorrow if the day runs
    /// out underneath it. A blinding that outlasts the day is a real case: the
    /// player blinds a camera at dusk and sleeps.
    /// </summary>
    private TimePoint Later(int slices)
    {
        var day = Clock.Day;
        var slice = Clock.SlicesSpent + slices;
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
        var device = House.DevicesIn(zone).FirstOrDefault(d =>
            d.Senses.Contains(channel)
            && _world.IsPowered(d.Id)
            && !_world.IsBlinded(d.Id)
            && _world.DeviceState(d.Id) != "destroyed");

        if (device is null)
        {
            return [];
        }

        var detection = new Detection
        {
            Channel = channel,
            Zone = zone,
            Magnitude = magnitude,
            At = _world.Now,
            Device = device.Id,
        };

        State.Detections.Add(detection);
        return [detection];
    }

    private static ActionOutcome Refused(MoveRefusal refusal, string description) =>
        new() { Happened = false, Refusal = refusal, Description = description };
}
