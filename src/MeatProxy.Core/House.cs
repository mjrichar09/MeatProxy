using MeatProxy.Core.Devices;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Perception;
using MeatProxy.Core.World;

namespace MeatProxy.Core;

/// <summary>
/// The authored house: rooms, openings, devices, zones, circuits. Immutable for
/// the life of a run.
/// </summary>
/// <remarks>
/// This is block 2 of the world-state summary — <em>house facts, changes on
/// device damage only</em> — and separating it from <see cref="State.WorldState"/>
/// is what lets the static head of the prompt stay cache-stable across a
/// playthrough (<c>world-state-summary.md</c> §1). It is also what makes a save
/// file small: a save records what the run did to the house, never the house.
/// </remarks>
public sealed class House
{
    private readonly Dictionary<RoomId, Room> _rooms;
    private readonly Dictionary<ZoneId, Zone> _zones;
    private readonly Dictionary<CircuitId, Circuit> _circuits;
    private readonly Dictionary<DeviceId, Device> _devices;
    private readonly Dictionary<LevelId, Level> _levels;
    private readonly Dictionary<RoomId, List<Opening>> _openingsByRoom;
    private readonly Dictionary<OpeningId, Opening> _openings;
    private readonly Dictionary<RoomId, List<Device>> _devicesByRoom;
    private readonly Dictionary<ZoneId, List<Device>> _devicesByZone;

    public House(
        string name,
        IEnumerable<Level> levels,
        IEnumerable<Room> rooms,
        IEnumerable<Zone> zones,
        IEnumerable<Circuit> circuits,
        IEnumerable<Device> devices,
        IReadOnlyDictionary<string, string>? worldFactAliases = null,
        IEnumerable<SensingUpgrade>? sensingUpgrades = null)
    {
        SensingUpgrades = sensingUpgrades?.ToList() ?? [];
        Name = name;
        _levels = levels.ToDictionary(l => l.Id);
        _rooms = rooms.ToDictionary(r => r.Id);
        _zones = zones.ToDictionary(z => z.Id);
        _circuits = circuits.ToDictionary(c => c.Id);
        _devices = devices.ToDictionary(d => d.Id);
        WorldFactAliases = worldFactAliases ?? new Dictionary<string, string>();

        _openings = [];
        _openingsByRoom = _rooms.Keys.ToDictionary(id => id, _ => new List<Opening>());

        foreach (var opening in _rooms.Values.SelectMany(r => r.Openings))
        {
            _openings[opening.Id] = opening;
            _openingsByRoom[opening.From].Add(opening);

            // Authored once, traversable both ways. An opening is a hole in a
            // wall, not a one-way valve, and authoring each side separately is
            // how the two halves eventually disagree.
            if (!opening.To.IsExterior && _openingsByRoom.TryGetValue(opening.To, out var farSide))
            {
                farSide.Add(opening with { From = opening.To, To = opening.From });
            }
        }

        _devicesByRoom = _rooms.Keys.ToDictionary(id => id, _ => new List<Device>());
        _devicesByZone = _zones.Keys.ToDictionary(id => id, _ => new List<Device>());

        foreach (var device in _devices.Values)
        {
            _devicesByRoom[device.Room].Add(device);
            _devicesByZone[ZoneOf(device.Room)].Add(device);
        }
    }

    public string Name { get; }

    /// <summary>
    /// Short names for world facts, mapping the spelling the schemas use onto a
    /// query the engine can answer — <c>boiler_faulted</c> onto
    /// <c>device_state:utility.boiler_controller=faulted</c>. Authored in data so
    /// Lane C can name a fact without an engine change.
    /// </summary>
    public IReadOnlyDictionary<string, string> WorldFactAliases { get; }

    public IReadOnlyCollection<Level> Levels => _levels.Values;
    public IReadOnlyCollection<Room> Rooms => _rooms.Values;
    public IReadOnlyCollection<Zone> Zones => _zones.Values;
    public IReadOnlyCollection<Circuit> Circuits => _circuits.Values;
    public IReadOnlyCollection<Device> Devices => _devices.Values;

    /// <summary>
    /// The ways the sensing surface can get better mid-run. Authored, so the wifi
    /// turn is a data record rather than a branch in the engine.
    /// </summary>
    public IReadOnlyList<SensingUpgrade> SensingUpgrades { get; }

    public Room Room(RoomId id) => _rooms.TryGetValue(id, out var room)
        ? room
        : throw new KeyNotFoundException($"No room '{id}' in {Name}.");

    public bool HasRoom(RoomId id) => _rooms.ContainsKey(id);

    public Zone Zone(ZoneId id) => _zones.TryGetValue(id, out var zone)
        ? zone
        : throw new KeyNotFoundException($"No zone '{id}' in {Name}.");

    public bool HasZone(ZoneId id) => _zones.ContainsKey(id);

    public Device Device(DeviceId id) => _devices.TryGetValue(id, out var device)
        ? device
        : throw new KeyNotFoundException($"No device '{id}' in {Name}.");

    public bool HasDevice(DeviceId id) => _devices.ContainsKey(id);

    public bool HasCircuit(CircuitId id) => _circuits.ContainsKey(id);

    public Opening? Opening(OpeningId id) => _openings.GetValueOrDefault(id);

    public IReadOnlyList<Opening> OpeningsFrom(RoomId room) =>
        _openingsByRoom.TryGetValue(room, out var openings) ? openings : [];

    public IReadOnlyList<Device> DevicesIn(RoomId room) =>
        _devicesByRoom.TryGetValue(room, out var devices) ? devices : [];

    public IReadOnlyList<Device> DevicesIn(ZoneId zone) =>
        _devicesByZone.TryGetValue(zone, out var devices) ? devices : [];

    public ZoneId ZoneOf(RoomId room) => Room(room).Zone;

    /// <summary>
    /// The channels a zone could carry if everything in it were working. What it
    /// <em>is</em> carrying depends on power and damage, which is run state —
    /// see <c>WorldQuery</c>.
    /// </summary>
    public IReadOnlySet<Channel> ChannelsAuthoredIn(ZoneId zone) =>
        DevicesIn(zone).SelectMany(d => d.Senses).ToHashSet();

    /// <summary>
    /// Every id in the house references something that exists, and every room is
    /// reachable from the start room. Run in the tests, because a house with a
    /// dangling zone id is a house that fails in the middle of a playthrough.
    /// </summary>
    public IReadOnlyList<string> Validate(RoomId startRoom)
    {
        var problems = new List<string>();

        foreach (var room in _rooms.Values)
        {
            if (!_levels.ContainsKey(room.Level))
            {
                problems.Add($"Room '{room.Id}' sits on unknown level '{room.Level}'.");
            }

            if (!_zones.ContainsKey(room.Zone))
            {
                problems.Add($"Room '{room.Id}' is in unknown zone '{room.Zone}'.");
            }

            foreach (var circuit in room.Circuits.Where(c => !_circuits.ContainsKey(c)))
            {
                problems.Add($"Room '{room.Id}' is on unknown circuit '{circuit}'.");
            }

            foreach (var opening in room.Openings)
            {
                if (opening.From != room.Id)
                {
                    problems.Add($"Opening '{opening.Id}' is filed under '{room.Id}' but starts at '{opening.From}'.");
                }

                if (!opening.To.IsExterior && !_rooms.ContainsKey(opening.To))
                {
                    problems.Add($"Opening '{opening.Id}' leads to unknown room '{opening.To}'.");
                }
            }
        }

        foreach (var device in _devices.Values)
        {
            if (!_rooms.ContainsKey(device.Room))
            {
                problems.Add($"Device '{device.Id}' is in unknown room '{device.Room}'.");
            }

            if (device.Circuit is { } circuit && !_circuits.ContainsKey(circuit))
            {
                problems.Add($"Device '{device.Id}' is on unknown circuit '{circuit}'.");
            }

            foreach (var tool in device.HouseCapabilities.Where(t => !Adversary.Tools.IsClassified(t)))
            {
                problems.Add($"Device '{device.Id}' declares tool '{tool}', which no tier knows whether to hold.");
            }

            if (device.States.Count > 0 && !device.States.Contains(device.StateAtStart))
            {
                problems.Add(
                    $"Device '{device.Id}' starts in '{device.StateAtStart}', which is not one of its declared states.");
            }
        }

        foreach (var zone in _zones.Values)
        {
            foreach (var room in zone.Rooms.Where(r => !_rooms.ContainsKey(r)))
            {
                problems.Add($"Zone '{zone.Id}' covers unknown room '{room}'.");
            }
        }

        problems.AddRange(Unreachable(startRoom).Select(r => $"Room '{r}' cannot be reached from '{startRoom}'."));
        problems.AddRange(GeometryProblems());

        return problems;
    }

    /// <summary>
    /// Whether the geometry agrees with the graph. One record drives the engine
    /// and the Blender build (ADR 0012), so a door the engine walks through has
    /// to be a hole in a wall both rooms share, or the render and the game
    /// disagree about the house.
    /// </summary>
    /// <remarks>
    /// Offsets run from the west end of a north or south wall and from the south
    /// end of an east or west wall. <c>world.md</c> says "the wall's near end" and
    /// does not say which end that is; this is the reading, and the check is what
    /// holds the file to it. Stairs and hatches change level, so they have no wall
    /// on the far side to agree with and are only held to their own wall.
    /// </remarks>
    private IEnumerable<string> GeometryProblems()
    {
        const double eps = 1e-6;
        var authored = _rooms.Values.Where(r => r.Geometry == GeometryProvenance.Authored).ToList();

        foreach (var room in authored)
        {
            foreach (var opening in room.Openings)
            {
                var wall = WallOf(room, opening.Wall);
                if (opening.Offset < -eps || opening.Offset + opening.Width > wall.Length + eps)
                {
                    yield return $"Opening '{opening.Id}' runs off the end of {room.Id}'s {opening.Wall} wall.";
                    continue;
                }

                var (from, to) = (wall.Start + opening.Offset, wall.Start + opening.Offset + opening.Width);
                var facing = authored
                    .Where(other => other.Id != room.Id && other.Level == room.Level)
                    .Select(other => (other, wall: WallOf(other, Opposite(opening.Wall))))
                    .Where(x => Math.Abs(x.wall.Line - wall.Line) < eps
                        && x.wall.Start < to - eps && x.wall.End > from + eps)
                    .ToList();

                if (opening.LeavesTheHouse)
                {
                    foreach (var (other, _) in facing)
                    {
                        yield return $"Opening '{opening.Id}' leads outside, but opens into '{other.Id}'.";
                    }

                    continue;
                }

                if (!_rooms.TryGetValue(opening.To, out var target) || target.Level != room.Level
                    || target.Geometry != GeometryProvenance.Authored)
                {
                    continue;
                }

                var shared = facing.FirstOrDefault(x => x.other.Id == target.Id);
                if (shared.other is null)
                {
                    yield return $"Opening '{opening.Id}' is in {room.Id}'s {opening.Wall} wall, which '{target.Id}' does not share.";
                }
                else if (shared.wall.Start > from + eps || shared.wall.End < to - eps)
                {
                    yield return $"Opening '{opening.Id}' is only partly in the wall {room.Id} shares with '{target.Id}'.";
                }
            }
        }

        foreach (var (a, i) in authored.Select((r, i) => (r, i)))
        {
            foreach (var b in authored.Skip(i + 1).Where(b => b.Level == a.Level))
            {
                var overlapX = Math.Min(a.Origin.X + a.Dims.Width, b.Origin.X + b.Dims.Width) - Math.Max(a.Origin.X, b.Origin.X);
                var overlapY = Math.Min(a.Origin.Y + a.Dims.Depth, b.Origin.Y + b.Dims.Depth) - Math.Max(a.Origin.Y, b.Origin.Y);
                if (overlapX > eps && overlapY > eps)
                {
                    yield return $"Rooms '{a.Id}' and '{b.Id}' overlap on {a.Level}.";
                }
            }
        }
    }

    /// <summary>A wall as a segment: the line it sits on, and where it starts and ends along it.</summary>
    private readonly record struct WallSegment(double Line, double Start, double End)
    {
        public double Length => End - Start;
    }

    private static WallSegment WallOf(Room room, Wall wall)
    {
        var (x, y, w, d) = (room.Origin.X, room.Origin.Y, room.Dims.Width, room.Dims.Depth);
        return wall switch
        {
            Wall.North => new WallSegment(y + d, x, x + w),
            Wall.South => new WallSegment(y, x, x + w),
            Wall.East => new WallSegment(x + w, y, y + d),
            _ => new WallSegment(x, y, y + d),
        };
    }

    private static Wall Opposite(Wall wall) => wall switch
    {
        Wall.North => Wall.South,
        Wall.South => Wall.North,
        Wall.East => Wall.West,
        _ => Wall.East,
    };

    private IEnumerable<RoomId> Unreachable(RoomId startRoom)
    {
        if (!_rooms.ContainsKey(startRoom))
        {
            return _rooms.Keys;
        }

        var seen = new HashSet<RoomId> { startRoom };
        var queue = new Queue<RoomId>([startRoom]);

        while (queue.Count > 0)
        {
            foreach (var opening in OpeningsFrom(queue.Dequeue()))
            {
                if (!opening.To.IsExterior && seen.Add(opening.To))
                {
                    queue.Enqueue(opening.To);
                }
            }
        }

        return _rooms.Keys.Where(r => !seen.Contains(r));
    }
}
