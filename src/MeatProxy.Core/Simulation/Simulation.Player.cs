using MeatProxy.Core.Effects;
using MeatProxy.Core.Enforcement;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Player;
using MeatProxy.Core.Time;

namespace MeatProxy.Core.Simulation;

/// <summary>
/// The player acting on the house: surface 3's manipulate and combine
/// (<c>interaction-model.md</c> §3), through the affordance table
/// (<c>affordances.md</c>). Deterministic everywhere but <c>combine</c>, which
/// asks the adjudicator and then validates what it says.
/// </summary>
public sealed partial class Simulation
{
    /// <summary>
    /// Do something to a device in the room the player is in. The device must
    /// offer the affordance; nothing outside the closed set exists to ask for.
    /// </summary>
    /// <param name="argument">
    /// <c>operate</c>: the state to set. <c>power_toggle</c>: the circuit.
    /// <c>combine</c>: the other device. Ignored otherwise.
    /// </param>
    public ActionOutcome Act(DeviceId target, Affordance affordance, string? argument = null)
    {
        if (!House.HasDevice(target) || House.Device(target).Room != State.PlayerRoom)
        {
            return Refused(MoveRefusal.NotHere, $"There is no {target} in {State.PlayerRoom}.");
        }

        var device = House.Device(target);
        var name = Affordances.Spell(affordance);

        if (!device.PlayerAffordances.Contains(name))
        {
            return Refused(MoveRefusal.NotOffered, $"{target} does not offer {name}.");
        }

        if (NotBuilt(affordance) is { } waitsOn)
        {
            return Refused(MoveRefusal.NotBuilt, $"{name} is not built yet: it waits on {waitsOn}.");
        }

        if (State.Unit is { Status: UnitStatus.Standing } unit && unit.Room == State.PlayerRoom
            && affordance is not (Affordance.Examine or Affordance.Listen))
        {
            return Refused(MoveRefusal.UnitInTheWay, "The unit is standing beside you.");
        }

        if (Clock.DayIsSpent)
        {
            return Refused(MoveRefusal.DayIsSpent, "The day is gone.");
        }

        if (ArgumentProblem(device, affordance, argument) is { } problem)
        {
            return Refused(MoveRefusal.BadArgument, problem);
        }

        var cost = Affordances.Cost(affordance);
        var (spent, interrupted) = SpendUnlessInterrupted(cost.Slices);
        if (cost.Body)
        {
            State.BodySpentToday++;
        }

        // What Arthur is actually doing, for the interpreter to recover or miss.
        State.PlayerActivity = TruthOf(affordance);

        // The act is seen whether or not it finishes: it was begun in front of
        // whatever is watching.
        var detections = Observe(device, affordance);
        var description = interrupted
            ? $"{name} {target}: interrupted after {spent} slice{(spent == 1 ? string.Empty : "s")}."
            : Resolve(device, affordance, argument!, detections);

        var read = TurnBoundary(detections);
        var events = UnitTick(interrupted ? $"{name} {target}" : null);

        return new ActionOutcome
        {
            Happened = true,
            SlicesSpent = spent,
            Interrupted = interrupted,
            Detections = detections,
            Understandings = read.Understandings,
            Focus = read.Focus,
            Events = events,
            Description = description,
        };
    }

    /// <summary>What an affordance does once it has been paid for and not interrupted.</summary>
    private string Resolve(Devices.Device device, Affordance affordance, string argument, List<Detection> detections)
    {
        var id = device.Id;

        switch (affordance)
        {
            case Affordance.Examine:
                return $"{id}: a {device.Class}, {World.DeviceState(id)}, "
                    + (World.IsPowered(id) ? "powered" : "dead") + ", "
                    + (device.ReachableByHouse ? "on the house's network" : "on no network at all") + ".";

            case Affordance.Listen:
                return World.IsPowered(id) && World.DeviceState(id) is not ("destroyed" or "off")
                    ? $"{id} hums. It is {World.DeviceState(id)}."
                    : $"{id} is silent.";

            case Affordance.Operate when device.OperateStates.Count == 0:
                return $"{id} has nothing on it to set.";

            case Affordance.Operate:
            case Affordance.Open:
            case Affordance.Close:
            {
                var to = affordance switch { Affordance.Open => "open", Affordance.Close => "closed", _ => argument };
                State.DeviceStates[id] = to;
                return $"{id} is now {to}.";
            }

            case Affordance.Disassemble:
            {
                // A durable delta (ADR 0015): nothing in the effect vocabulary
                // puts it back, and the house would need a body to.
                var to = device.States.Contains("destroyed") ? "destroyed" : "faulted";
                State.DeviceStates[id] = to;
                return $"{id} is in pieces ({to}).";
            }

            case Affordance.PowerToggle:
            {
                var circuit = new CircuitId(argument);
                if (World.IsCircuitCut(circuit))
                {
                    State.PowerCutUntil.Remove(circuit);
                    return $"{circuit} is back on.";
                }

                // A breaker stays off until someone puts it back. The timed cut
                // is the effect vocabulary's; a hand on the switch is not timed.
                State.PowerCutUntil[circuit] = new TimePoint(int.MaxValue, 0);
                detections.AddRange(EmitSilence(circuit));
                return $"{circuit} is off.";
            }

            case Affordance.Combine:
            {
                var other = House.Device(new DeviceId(argument));
                var request = new CombineRequest(device, other, State.PlayerRoom, House.ZoneOf(State.PlayerRoom));
                var adjudication = _adjudicator.Rule(request);
                var result = Adjudicate(adjudication);

                // A rejected effect reads as nothing happening, which is also a
                // legitimate outcome (affordances.md §3).
                return result.AnythingHappened
                    ? $"{id} with {other.Id}: {adjudication.Rationale}"
                    : $"{id} with {other.Id}: nothing happened.";
            }

            default:
                return $"{Affordances.Spell(affordance)} {id}.";
        }
    }

    /// <summary>
    /// <c>affordances.md</c> §4's observable column, as readings. A reading
    /// exists only if something in the zone senses that channel.
    /// </summary>
    private List<Detection> Observe(Devices.Device device, Affordance affordance)
    {
        var zone = House.ZoneOf(State.PlayerRoom);
        var detections = new List<Detection>();

        void Read(Channel channel, Magnitude magnitude) => detections.AddRange(Sense(zone, channel, magnitude));

        switch (affordance)
        {
            case Affordance.Examine or Affordance.Listen:
                // "Rarely": standing still and looking is nothing much to see.
                break;

            case Affordance.Open or Affordance.Close:
                Read(Channel.Motion, Magnitude.Low);
                Read(Channel.DoorState, Magnitude.Medium);
                break;

            case Affordance.PowerToggle:
                Read(Channel.Motion, Magnitude.Low);
                break;

            case Affordance.Operate:
                // "Depends on the device's senses": using a thing shows up on
                // whatever that thing itself reports.
                Read(Channel.Motion, Magnitude.Low);
                foreach (var channel in World.Senses(device.Id))
                {
                    Read(channel, Magnitude.Medium);
                }

                break;

            case Affordance.Combine:
                // "Often, and loudly."
                Read(Channel.Motion, Magnitude.Medium);
                Read(Channel.AudioLevel, Magnitude.Medium);
                break;

            case Affordance.Disassemble:
                Read(Channel.Motion, Magnitude.High);
                Read(Channel.AudioLevel, Magnitude.High);
                Read(Channel.PowerDraw, Magnitude.High);
                break;
        }

        return detections;
    }

    /// <summary>
    /// What the player is really doing, in the claim vocabulary. The interpreter
    /// gets the readings, never this; it is the truth its guess is scored against.
    /// </summary>
    private static Claim? TruthOf(Affordance affordance) => affordance switch
    {
        Affordance.Disassemble => Claim.Dismantling,
        Affordance.PowerToggle => Claim.TracingCircuit,
        Affordance.Operate or Affordance.Open or Affordance.Close or Affordance.Combine => Claim.HandlingDevice,
        Affordance.Examine or Affordance.Listen => Claim.Searching,
        _ => null,
    };

    /// <summary>The affordances whose schemas or records do not exist yet, and what each waits on.</summary>
    private static string? NotBuilt(Affordance affordance) => affordance switch
    {
        Affordance.WriteOn or Affordance.PlaceInZone => "injection-vectors.md, provisional until D2",
        Affordance.Take or Affordance.Drop => "item records, which no schema defines yet",
        Affordance.Enter => "openings as objects; for now entering is moving through a hatch or vent",
        _ => null,
    };

    private string? ArgumentProblem(Devices.Device device, Affordance affordance, string? argument) => affordance switch
    {
        Affordance.Operate when device.OperateStates.Count > 0 && !device.OperateStates.Contains(argument ?? string.Empty) =>
            $"{device.Id} can be set to {string.Join(", ", device.OperateStates)}.",
        Affordance.Open when !device.States.Contains("open") => $"{device.Id} has nothing to open.",
        Affordance.Close when !device.States.Contains("closed") => $"{device.Id} has nothing to close.",
        Affordance.PowerToggle when argument is null || !House.HasCircuit(new CircuitId(argument)) =>
            "Which circuit?",
        Affordance.Combine when argument is null || !House.HasDevice(new DeviceId(argument))
                                || House.Device(new DeviceId(argument)).Room != State.PlayerRoom
                                || argument == device.Id.Value =>
            "Combine it with something else in this room.",
        _ => null,
    };
}
