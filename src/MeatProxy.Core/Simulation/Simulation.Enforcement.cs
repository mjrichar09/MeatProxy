using MeatProxy.Core.Adversary;
using MeatProxy.Core.Enforcement;
using MeatProxy.Core.Time;
using MeatProxy.Core.World;

namespace MeatProxy.Core.Simulation;

/// <summary>What sending the unit did.</summary>
public sealed record DispatchOutcome
{
    public required bool Happened { get; init; }
    public required string Description { get; init; }

    /// <summary>When it will reach the room it was sent to.</summary>
    public TimePoint? ArrivesAt { get; init; }

    /// <summary>Sending it is an admission (ADR 0006), so it moves the ladder.</summary>
    public TierChange? TierChange { get; init; }
}

/// <summary>
/// The house acting on the player: the mobility unit (ADR 0006). Dispatch,
/// travel time and interruption, in turn time, with no model anywhere in it.
/// </summary>
/// <remarks>
/// When to send it is the adversary's judgement and is not here: the stub AI
/// (E6) decides that, and Lane A after it. This is only what happens once it is
/// sent, which has to be the same whoever sends it.
/// </remarks>
public sealed partial class Simulation
{
    /// <summary>Where the unit docks: the room of the device that holds the dispatch tool.</summary>
    public RoomId UnitDock => Charger.Room;

    /// <summary>Where the unit is right now.</summary>
    public RoomId UnitRoom => State.Unit.Room ?? UnitDock;

    private Devices.Device Charger =>
        House.Devices.First(d => d.HouseCapabilities.Contains(UnitRules.DispatchTool));

    /// <summary>
    /// Send the unit to the room the player is in. It goes where he was, not where
    /// he goes next: it does not retarget, so a player who sees it coming can
    /// leave (ADR 0006, "it is never instant").
    /// </summary>
    public DispatchOutcome Dispatch(string reason)
    {
        if (State.Unit.Status != UnitStatus.Docked)
        {
            return new DispatchOutcome { Happened = false, Description = "The unit is already out." };
        }

        // Absent, not refused: a dead or unreachable charger takes the tool with
        // it, and a faulted one cannot release the unit.
        if (!World.HouseCapabilities().Contains(UnitRules.DispatchTool)
            || World.DeviceState(Charger.Id) == "faulted")
        {
            return new DispatchOutcome { Happened = false, Description = "The house has no way to send the unit." };
        }

        var target = State.PlayerRoom;
        var legs = UnitRoute(UnitDock, target, Now);
        if (legs is null)
        {
            return new DispatchOutcome
            {
                Happened = false,
                Description = $"The unit cannot get to {target}.",
            };
        }

        State.Unit = new UnitState
        {
            Status = UnitStatus.EnRoute,
            Room = UnitDock,
            Target = target,
            Reason = reason,
            Legs = legs,
        };

        State.Disclosures.Add(new Disclosure(Now, target, reason));
        State.DeviceStates[Charger.Id] = "dispatched";
        var change = Escalate($"sent the unit: {reason}");

        return new DispatchOutcome
        {
            Happened = true,
            ArrivesAt = State.Unit.LegsEnd,
            TierChange = change,
            Description = $"The unit leaves {UnitDock} for {target}, arriving {State.Unit.LegsEnd}.",
        };
    }

    /// <summary>
    /// Spend the slices an act asks for, unless the unit walks into the room
    /// first. Then spend only up to its arrival, and the act does not finish.
    /// </summary>
    private (int Spent, bool Interrupted) SpendUnlessInterrupted(int slices)
    {
        var unit = State.Unit;
        if (unit is { Status: UnitStatus.EnRoute, LegsEnd: { } arrives }
            && unit.Target == State.PlayerRoom
            && arrives.Day == Clock.Day)
        {
            var until = arrives.Slice - Clock.SlicesSpent;
            if (until < slices)
            {
                return (Clock.Spend(Math.Max(0, until)), true);
            }
        }

        return (Clock.Spend(slices), false);
    }

    /// <summary>
    /// Bring the unit up to now: move it along its legs, arrive, stand, turn
    /// round, dock. Called after every action that spends time.
    /// </summary>
    private List<string> UnitTick(string? interruptedAct)
    {
        var events = new List<string>();
        var unit = State.Unit;

        while (true)
        {
            switch (unit.Status)
            {
                case UnitStatus.EnRoute or UnitStatus.Returning when unit.Legs.Count > 0 && unit.Legs[0].At <= Now:
                    unit.Room = unit.Legs[0].Room;
                    unit.Legs.RemoveAt(0);
                    continue;

                case UnitStatus.EnRoute when unit.Legs.Count == 0:
                {
                    var room = unit.Room ?? UnitDock;
                    var present = State.PlayerRoom == room;
                    var arrivedAt = Now;
                    State.Arrivals.Add(new Arrival(arrivedAt, room, present, present ? interruptedAct : null));

                    unit.Status = UnitStatus.Standing;
                    unit.StandsUntil = After(arrivedAt, UnitRules.StandSlices);
                    events.Add(present
                        ? $"The unit arrives in {room}" + (interruptedAct is null ? "." : $" and interrupts {interruptedAct}.")
                        : $"The unit arrives in {room}. You are not there.");
                    continue;
                }

                case UnitStatus.Standing when unit.StandsUntil is { } until && until <= Now:
                {
                    var from = unit.Room ?? UnitDock;
                    unit.Legs = UnitRoute(from, UnitDock, until) ?? [];
                    unit.Status = UnitStatus.Returning;
                    unit.StandsUntil = null;
                    events.Add($"The unit turns round in {from} and goes back to its charger.");
                    continue;
                }

                case UnitStatus.Returning when unit.Legs.Count == 0:
                    State.Unit = new UnitState();
                    State.DeviceStates[Charger.Id] = "charging";
                    events.Add("The unit docks.");
                    break;
            }

            return events;
        }
    }

    /// <summary>Overnight it goes home, whatever it was doing.</summary>
    private List<string> SendUnitHomeOvernight()
    {
        if (State.Unit.Status == UnitStatus.Docked)
        {
            return [];
        }

        State.Unit = new UnitState();
        State.DeviceStates[Charger.Id] = "charging";
        return ["The unit is back on its charger by morning."];
    }

    /// <summary>
    /// The unit's route, as rooms with the time it reaches each. Null when it
    /// cannot get there. A route to where it already is still takes a slice:
    /// it is never instant.
    /// </summary>
    private List<UnitLeg>? UnitRoute(RoomId from, RoomId to, TimePoint leaving)
    {
        if (from == to)
        {
            return [new UnitLeg(to, After(leaving, 1))];
        }

        var cameFrom = new Dictionary<RoomId, Opening>();
        var queue = new Queue<RoomId>([from]);
        var seen = new HashSet<RoomId> { from };

        while (queue.Count > 0 && !cameFrom.ContainsKey(to))
        {
            var here = queue.Dequeue();
            foreach (var opening in House.OpeningsFrom(here))
            {
                if (opening.To.IsExterior || !opening.Openable || World.IsLocked(opening)
                    || !UnitRules.CanCross(opening.Kind) || !seen.Add(opening.To))
                {
                    continue;
                }

                cameFrom[opening.To] = opening;
                queue.Enqueue(opening.To);
            }
        }

        if (!cameFrom.ContainsKey(to))
        {
            return null;
        }

        var steps = new List<Opening>();
        for (var at = to; at != from; at = cameFrom[at].From)
        {
            steps.Add(cameFrom[at]);
        }

        steps.Reverse();

        var legs = new List<UnitLeg>();
        var time = leaving;
        foreach (var step in steps)
        {
            time = After(time, _costs.For(step.Kind) * UnitRules.Slowness);
            legs.Add(new UnitLeg(step.To, time));
        }

        return legs;
    }
}
