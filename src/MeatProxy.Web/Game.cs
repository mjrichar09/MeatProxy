using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using MeatProxy.Core;
using MeatProxy.Core.Content;
using MeatProxy.Core.Effects;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Persistence;
using MeatProxy.Core.Player;
using MeatProxy.Core.Predicates;
using MeatProxy.Core.Simulation;
using MeatProxy.Core.State;
using MeatProxy.Harness;

namespace MeatProxy.Web;

/// <summary>
/// The harness's verbs, exported to the page. Every call does one thing to the
/// simulation and returns the frame after it, plus the lines it would have
/// printed. The page holds no game state of its own.
/// </summary>
/// <remarks>
/// Arthur's hands go through <see cref="Simulation.Act"/>, the affordance
/// table. Blinding and timed cuts are debug levers that run as validated
/// effects, the way an adjudication would arrive; they are not his verbs.
/// </remarks>
public static partial class Game
{
    private static House? _house;
    private static RoomId _start;
    private static Simulation? _sim;

    private static Simulation Sim => _sim ?? throw new InvalidOperationException("Call Start first.");

    public static void Main()
    {
    }

    [JSExport]
    public static string Start()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("house.json")
            ?? throw new InvalidOperationException("house.json is not embedded.");
        using var reader = new StreamReader(stream);
        var file = HouseFile.FromJson(reader.ReadToEnd());

        _house = file.ToHouse();
        _start = file.StartRoom;
        _sim = new Simulation(_house, new WorldState { PlayerRoom = _start });

        return Frame("Day 1, waking up", "Arthur wakes in the bedroom.", null, []);
    }

    [JSExport]
    public static string Claims() =>
        JsonSerializer.Serialize(Enum.GetValues<Claim>().Select(Predicate.Spell));

    [JSExport]
    public static string Go(string room)
    {
        var destination = new RoomId(room);
        var route = RouteTo(destination);
        if (route.Count == 0)
        {
            return Frame($"No way to {room}", $"There is no open route from {Sim.State.PlayerRoom} to {room}.", null, []);
        }

        var lines = new List<string>();
        var detections = new List<Detection>();
        var understandings = new List<Understanding>();

        foreach (var step in route)
        {
            var outcome = Sim.Move(step);
            lines.Add(outcome.Description);
            lines.AddRange(outcome.Events);
            if (!outcome.Happened)
            {
                break;
            }

            detections.AddRange(outcome.Detections);
            understandings.AddRange(outcome.Understandings);
        }

        var walked = new ActionOutcome
        {
            Happened = true,
            Detections = detections,
            Understandings = understandings,
        };

        return Frame($"Walked to {Sim.State.PlayerRoom}", string.Join(" ", lines), walked, lines);
    }

    [JSExport]
    public static string Do(string activity, int slices)
    {
        if (!Enum.TryParse<Claim>(activity.Replace("_", string.Empty), ignoreCase: true, out var claim))
        {
            return Frame("Not an activity", $"'{activity}' is not in the claim vocabulary.", null, []);
        }

        var outcome = Sim.Do(claim, slices);
        return Frame($"{activity} in {Sim.State.PlayerRoom}", outcome.Description, outcome, [outcome.Description, .. outcome.Events]);
    }

    [JSExport]
    public static string Wait(int slices)
    {
        var outcome = Sim.Wait(slices);
        return Frame("Waited", outcome.Description, outcome, [outcome.Description, .. outcome.Events]);
    }

    [JSExport]
    public static string Sleep()
    {
        var outcome = Sim.Sleep();
        return Frame("Slept", outcome.Description, outcome, [outcome.Description, .. outcome.Events]);
    }

    /// <summary>Arthur's own hands: an affordance on a device in the room he is in.</summary>
    [JSExport]
    public static string Act(string device, string affordance, string argument)
    {
        if (!Affordances.TryParse(affordance, out var which))
        {
            return Frame("Not an affordance", $"'{affordance}' is not in affordances.md's closed set.", null, []);
        }

        var outcome = Sim.Act(new DeviceId(device), which, argument.Length > 0 ? argument : null);
        return Frame($"{affordance} {device}", outcome.Description, outcome, [outcome.Description, .. outcome.Events]);
    }

    [JSExport]
    public static string Dispatch(string reason)
    {
        var outcome = Sim.Dispatch(reason);
        return Frame("The unit", outcome.Description, null, [outcome.Description]);
    }

    [JSExport]
    public static string Blind(string device, int slices) =>
        Effect($"Blinded {device}", new BlindSensor(new DeviceId(device), slices));

    [JSExport]
    public static string Cut(string circuit, int slices) =>
        Effect($"Cut {circuit}", new CutPower(new CircuitId(circuit), slices));

    [JSExport]
    public static string Escalate(string reason)
    {
        var change = Sim.Escalate(reason);
        var line = change is { } c ? $"{c.From} -> {c.To}: {reason}." : "Already at the floor.";
        return Frame("Escalated", line, null, [line]);
    }

    [JSExport]
    public static string Upgrade(string id)
    {
        var line = Sim.World.ApplyUpgrade(id) ? $"Applied {id}." : $"{id} is already in place.";
        return Frame("Sensing upgrade", line, null, [line]);
    }

    [JSExport]
    public static string Slots(int slots)
    {
        Sim.State.FocusSlots = Math.Clamp(slots, 1, 9);
        var line = $"Focus slots: {Sim.State.FocusSlots}.";
        return Frame("Focus slots", line, null, [line]);
    }

    [JSExport]
    public static string Save() => SaveGame.From(Sim.House, Sim.State).ToJson();

    [JSExport]
    public static string Load(string json)
    {
        var restored = SaveGame.FromJson(json).Restore(Sim.House);
        _sim = new Simulation(Sim.House, restored);
        return Frame("Loaded", $"Restored to {restored.PlayerRoom}, {restored.Clock.Now}.", null, []);
    }

    private static string Effect(string caption, Effect effect)
    {
        var result = Sim.Adjudicate(new Adjudication { Effects = [effect], Rationale = caption });
        var lines = result.Rejected.Select(r => $"Rejected: {r.Reason}").ToList();
        if (result.AnythingHappened)
        {
            lines.Insert(0, caption + ".");
        }

        return Frame(caption, string.Join(" ", lines), null, lines);
    }

    private static string Frame(string caption, string note, ActionOutcome? last, IReadOnlyList<string> lines) =>
        JsonSerializer.Serialize(new
        {
            house = Sim.House.Name,
            frame = DebugSnapshot.Of(Sim, caption, note, last),
            lines,
        }, DebugSnapshot.Options);

    private static List<RoomId> RouteTo(RoomId destination)
    {
        var from = Sim.State.PlayerRoom;
        var cameFrom = new Dictionary<RoomId, RoomId>();
        var queue = new Queue<RoomId>([from]);
        var seen = new HashSet<RoomId> { from };

        while (queue.Count > 0)
        {
            var here = queue.Dequeue();
            foreach (var opening in Sim.House.OpeningsFrom(here))
            {
                if (opening.To.IsExterior || !opening.Openable || Sim.World.IsLocked(opening) || !seen.Add(opening.To))
                {
                    continue;
                }

                cameFrom[opening.To] = here;
                queue.Enqueue(opening.To);
            }
        }

        if (destination == from || !cameFrom.ContainsKey(destination))
        {
            return [];
        }

        var route = new List<RoomId>();
        for (var at = destination; at != from; at = cameFrom[at])
        {
            route.Add(at);
        }

        route.Reverse();
        return route;
    }
}
