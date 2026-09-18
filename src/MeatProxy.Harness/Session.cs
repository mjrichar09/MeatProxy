using MeatProxy.Core;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Persistence;
using MeatProxy.Core.Predicates;
using MeatProxy.Core.Simulation;

namespace MeatProxy.Harness;

/// <summary>One sitting with the house.</summary>
public sealed class Session
{
    private Simulation _sim;

    public Session(Simulation sim) => _sim = sim;

    /// <summary>
    /// Walk every room the house says is reachable, then save and restore.
    /// Non-interactive, so CI can run it and so a broken house fails a build
    /// rather than a playthrough.
    /// </summary>
    public int Tour()
    {
        Console.WriteLine($"{_sim.House.Name}: {_sim.House.Rooms.Count} rooms, "
            + $"{_sim.House.Devices.Count} devices, {_sim.House.Zones.Count} zones.");
        Console.WriteLine();

        var start = _sim.State.PlayerRoom;
        var queued = new HashSet<RoomId> { start };
        var toVisit = new Queue<RoomId>([start]);
        var refusedAtTheBoundary = new List<string>();

        while (toVisit.Count > 0)
        {
            var here = toVisit.Dequeue();

            // Only what the player could actually get to. A room behind a door
            // that will not open has not been visited, whatever the map says.
            if (!Walk(here))
            {
                continue;
            }

            foreach (var opening in _sim.House.OpeningsFrom(here))
            {
                if (opening.To.IsExterior)
                {
                    var attempt = _sim.Move(opening.To);
                    refusedAtTheBoundary.Add($"{opening.Id}: {attempt.Description}");
                    continue;
                }

                if (queued.Add(opening.To))
                {
                    toVisit.Enqueue(opening.To);
                }
            }
        }

        var visited = _sim.State.RoomsVisited;
        Console.WriteLine($"Visited {visited.Count} of {_sim.House.Rooms.Count} rooms.");
        Console.WriteLine();
        Console.WriteLine("The boundary:");
        foreach (var line in refusedAtTheBoundary)
        {
            Console.WriteLine($"  {line}");
        }

        Console.WriteLine();
        Console.WriteLine("Predicates:");
        foreach (var predicate in new[]
                 {
                     "world(attic_reached)", "world(crawlspace_reached)",
                     "world(front_door_locked)", "world(boiler_faulted)",
                     "held(E1)",
                 })
        {
            Console.WriteLine($"  {predicate,-30} {_sim.World.Holds(predicate)}");
        }

        Console.WriteLine();
        Console.WriteLine("Seen and understood:");
        Perception();

        Console.WriteLine();
        Console.WriteLine("The same act, read twice:");
        Walk(new RoomId("kitchen"));

        // A calm house reads the innocent twin. Push it up the ladder and the
        // same evidence reads the other way. ADR 0014's trade, in two lines.
        foreach (var tier in new[] { AlertTier.Open, AlertTier.Dropped })
        {
            _sim.State.Tier = tier;
            var outcome = _sim.Do(Claim.Dismantling, 2);
            var read = outcome.Understandings.FirstOrDefault()?.Claim;
            Console.WriteLine($"  dismantling at tier {tier,-8} reads as "
                + (read is { } claim ? Predicate.Spell(claim) : "nothing"));
        }

        _sim.State.Tier = AlertTier.Open;

        Console.WriteLine();
        Console.WriteLine("The wifi turn:");
        var attic = new ZoneId("z_attic");
        Console.WriteLine($"  before: z_attic interpretable {_sim.World.IsInterpretable(attic)}, "
            + $"covered {_sim.World.IsCovered(attic)}");
        _sim.World.ApplyUpgrade("wifi_sensing");
        Console.WriteLine($"  after:  z_attic interpretable {_sim.World.IsInterpretable(attic)}, "
            + $"covered {_sim.World.IsCovered(attic)}");

        Console.WriteLine();
        var save = SaveGame.From(_sim.House, _sim.State).ToJson();
        var restored = SaveGame.FromJson(save).Restore(_sim.House);
        Console.WriteLine($"Saved {save.Length} bytes and restored to "
            + $"{restored.PlayerRoom}, {restored.Clock.Now}, "
            + $"{restored.RoomsVisited.Count} rooms visited.");

        var unvisited = _sim.House.Rooms.Where(r => !visited.Contains(r.Id)).ToList();
        if (unvisited.Count > 0)
        {
            Console.Error.WriteLine("Unreachable: " + string.Join(", ", unvisited.Select(r => r.Id)));
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Move to a room by whatever route the house allows, sleeping when the day
    /// runs out. False when there is no open route to it from here.
    /// </summary>
    private bool Walk(RoomId destination)
    {
        if (_sim.State.PlayerRoom == destination)
        {
            return true;
        }

        var route = RouteTo(destination);
        if (route.Count == 0)
        {
            Console.WriteLine($"  no open route from {_sim.State.PlayerRoom} to {destination}.");
            return false;
        }

        foreach (var step in route)
        {
            if (_sim.Clock.DayIsSpent)
            {
                _sim.Sleep();
            }

            var outcome = _sim.Move(step);
            Console.WriteLine($"  {outcome.Description}"
                + (outcome.Detections.Count > 0
                    ? $"  [{string.Join(", ", outcome.Detections.Select(d => $"{d.Channel} in {d.Zone}"))}]"
                    : "  [unobserved]"));
        }

        return _sim.State.PlayerRoom == destination;
    }

    private List<RoomId> RouteTo(RoomId destination)
    {
        var from = _sim.State.PlayerRoom;
        var cameFrom = new Dictionary<RoomId, RoomId>();
        var seen = new HashSet<RoomId> { from };
        var queue = new Queue<RoomId>([from]);

        while (queue.Count > 0)
        {
            var here = queue.Dequeue();
            if (here == destination)
            {
                break;
            }

            foreach (var opening in _sim.House.OpeningsFrom(here))
            {
                if (opening.To.IsExterior || _sim.World.IsLocked(opening) || !opening.Openable)
                {
                    continue;
                }

                if (seen.Add(opening.To))
                {
                    cameFrom[opening.To] = here;
                    queue.Enqueue(opening.To);
                }
            }
        }

        var route = new List<RoomId>();
        for (var at = destination; at != from; at = cameFrom[at])
        {
            if (!cameFrom.ContainsKey(at))
            {
                return [];
            }

            route.Add(at);
        }

        route.Reverse();
        return route;
    }

    public int Repl()
    {
        Look();

        while (true)
        {
            Console.Write($"\n[{_sim.Clock.Now}, {_sim.Clock.SlicesRemaining} left] {_sim.State.PlayerRoom}> ");
            var line = Console.ReadLine();
            if (line is null or "quit" or "exit")
            {
                return 0;
            }

            var parts = line.Trim().Split(' ', 2);
            var argument = parts.Length > 1 ? parts[1].Trim() : string.Empty;

            switch (parts[0])
            {
                case "":
                    break;
                case "look":
                    Look();
                    break;
                case "go":
                    Report(_sim.Move(new RoomId(argument)));
                    break;
                case "wait":
                    Console.WriteLine(_sim.Wait(int.TryParse(argument, out var n) ? n : 1).Description);
                    break;
                case "sleep":
                    Console.WriteLine(_sim.Sleep().Description);
                    break;
                case "ask":
                    Ask(argument);
                    break;
                case "devices":
                    Devices();
                    break;
                case "do":
                    Activity(argument);
                    break;
                case "perception":
                    Perception();
                    break;
                case "upgrade":
                    Console.WriteLine(_sim.World.ApplyUpgrade(argument)
                        ? $"Applied {argument}."
                        : $"No upgrade '{argument}', or it is already in place.");
                    break;
                case "slots":
                    _sim.State.FocusSlots = int.TryParse(argument, out var slots) ? slots : _sim.State.FocusSlots;
                    Console.WriteLine($"Focus slots: {_sim.State.FocusSlots}.");
                    break;
                case "tools":
                    Console.WriteLine(string.Join(", ", _sim.World.HouseCapabilities().Order()));
                    break;
                case "save":
                    File.WriteAllText(argument, SaveGame.From(_sim.House, _sim.State).ToJson());
                    Console.WriteLine($"Saved to {argument}.");
                    break;
                case "load":
                    var restored = SaveGame.FromJson(File.ReadAllText(argument)).Restore(_sim.House);
                    _sim = new Simulation(_sim.House, restored);
                    Console.WriteLine($"Restored to {restored.PlayerRoom}, {restored.Clock.Now}.");
                    break;
                default:
                    Console.WriteLine("look | go <room> | do <activity> [slices] | wait <n> | sleep\n"
                        + "ask <predicate> | devices | perception | tools | slots <n> | upgrade <id>\n"
                        + "save <path> | load <path> | quit");
                    break;
            }
        }
    }

    private static void Report(ActionOutcome outcome)
    {
        Console.WriteLine(outcome.Description);

        foreach (var detection in outcome.Detections)
        {
            Console.WriteLine($"  seen: {detection.Channel} in {detection.Zone}, {detection.Magnitude}");
        }

        foreach (var understanding in outcome.Understandings)
        {
            Console.WriteLine(understanding.Claim is { } claim
                ? $"  understood: it thinks you were {Predicate.Spell(claim)} in {understanding.Zone}"
                : $"  understood: it made nothing of {understanding.Zone}");
        }
    }

    private void Activity(string argument)
    {
        var parts = argument.Split(' ', 2);
        if (!Enum.TryParse<Claim>(parts[0].Replace("_", string.Empty), ignoreCase: true, out var claim))
        {
            Console.WriteLine("Not in the claim vocabulary. Try: "
                + string.Join(", ", Enum.GetValues<Claim>().Take(6).Select(Predicate.Spell)) + ", ...");
            return;
        }

        var slices = parts.Length > 1 && int.TryParse(parts[1], out var n) ? n : 4;
        var outcome = _sim.Do(claim, slices);

        Report(outcome);

        // The whole of ADR 0014 in one line: what you did, and what it made of it.
        var read = outcome.Understandings.FirstOrDefault(u => u.Zone == _sim.House.ZoneOf(_sim.State.PlayerRoom));
        if (read?.Claim is { } concluded && concluded != claim)
        {
            Console.WriteLine($"  (you were {Predicate.Spell(claim)}. It thinks you were {Predicate.Spell(concluded)}.)");
        }
    }

    private void Perception()
    {
        Console.WriteLine($"  {_sim.State.FocusSlots} focus slot(s), tier {_sim.State.Tier}.");
        Console.WriteLine($"  {"zone",-14} {"seen",-6} {"understood",-11} channels");

        foreach (var zone in _sim.House.Zones.OrderBy(z => z.Id.Value))
        {
            var world = _sim.World;
            var channels = world.LiveChannels(zone.Id);
            var understood = world.IsUnderstood(zone.Id)
                ? "yes"
                : world.IsInterpretable(zone.Id) ? "not focused" : "blind";

            Console.WriteLine($"  {zone.Id.Value,-14} {(world.IsCovered(zone.Id) ? "yes" : "no"),-6} "
                + $"{understood,-11} {string.Join('/', channels.Order())}"
                + (world.IsInterpretable(zone.Id) ? string.Empty : $"  — {zone.BlindBecause}"));
        }
    }

    private void Look()
    {
        var room = _sim.House.Room(_sim.State.PlayerRoom);
        var zone = _sim.House.Zone(room.Zone);

        Console.WriteLine($"\n{room.Name} ({room.Id}), {room.Level}, zone {zone.Id}"
            + (zone.Interpretable ? string.Empty : $" — blind: {zone.BlindBecause}"));

        Console.WriteLine(_sim.World.CanSpeakToHouseFrom(room.Id)
            ? "You can speak to the house here."
            : "Nothing here is listening. You cannot speak to the house.");

        foreach (var opening in _sim.House.OpeningsFrom(room.Id))
        {
            var locked = _sim.World.IsLocked(opening) ? " (locked)" : string.Empty;
            var shut = opening.Openable ? string.Empty : " (does not open)";
            Console.WriteLine($"  {opening.Kind,-8} to {opening.To}{locked}{shut}");
        }
    }

    private void Devices()
    {
        foreach (var device in _sim.House.DevicesIn(_sim.State.PlayerRoom))
        {
            var power = _sim.World.IsPowered(device.Id) ? "powered" : "dead";
            var reach = _sim.World.IsReachableByHouse(device.Id) ? "reachable" : "off-network";
            Console.WriteLine($"  {device.Id,-32} {_sim.World.DeviceState(device.Id),-12} {power}, {reach}"
                + (device.Senses.Count > 0 ? $", senses {string.Join('/', device.Senses)}" : string.Empty));
        }
    }

    private void Ask(string text)
    {
        if (!Predicate.TryParse(text, out var predicate))
        {
            Console.WriteLine("Not a predicate. Try held(E1), world(attic_reached), "
                + "house_believes(dismantling), put_to_house(precedent), run(assembled).");
            return;
        }

        Console.WriteLine($"{predicate} = {_sim.World.Holds(predicate!)}");
    }
}
