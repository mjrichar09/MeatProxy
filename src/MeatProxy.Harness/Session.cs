using MeatProxy.Core;
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
                    var outcome = _sim.Move(new RoomId(argument));
                    Console.WriteLine(outcome.Description);
                    foreach (var detection in outcome.Detections)
                    {
                        Console.WriteLine($"  sensed: {detection.Channel} in {detection.Zone}");
                    }

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
                    Console.WriteLine("look | go <room> | wait <n> | sleep | ask <predicate> | "
                        + "devices | tools | save <path> | load <path> | quit");
                    break;
            }
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
