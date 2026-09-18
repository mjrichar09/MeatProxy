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

        return problems;
    }

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
