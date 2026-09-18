using System.Text.Json;
using MeatProxy.Core.Devices;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Persistence;
using MeatProxy.Core.World;

namespace MeatProxy.Core.Content;

/// <summary>
/// The on-disk shape of an authored house, mirroring <c>world.md</c>,
/// <c>devices.md</c> and <c>sensors.md</c>.
/// </summary>
/// <remarks>
/// The schemas are written in YAML and this loads JSON. That is a deliberate
/// choice for the first slice rather than a disagreement with the schemas: JSON
/// needs no third-party package, which keeps the core's dependency list empty,
/// and the field names are identical either way. If Lane C would rather author
/// YAML, the conversion is a build step and not a change to anything here.
/// </remarks>
public sealed record HouseFile
{
    public required string Name { get; init; }
    public required RoomId StartRoom { get; init; }
    public IReadOnlyList<LevelFile> Levels { get; init; } = [];
    public IReadOnlyList<RoomFile> Rooms { get; init; } = [];
    public IReadOnlyList<ZoneFile> Zones { get; init; } = [];
    public IReadOnlyList<CircuitFile> Circuits { get; init; } = [];
    public IReadOnlyList<DeviceFile> Devices { get; init; } = [];
    public IReadOnlyDictionary<string, string> WorldFactAliases { get; init; } = new Dictionary<string, string>();
    public IReadOnlyList<SensingUpgrade> SensingUpgrades { get; init; } = [];

    public House ToHouse() => new(
        Name,
        Levels.Select(l => new Level { Id = l.Id, Name = l.Name, Elevation = l.Elevation }),
        Rooms.Select(ToRoom),
        Zones.Select(z => new Zone
        {
            Id = z.Id,
            Name = z.Name,
            Rooms = z.Rooms,
            Interpretable = z.Interpretable,
            BlindBecause = z.BlindBecause,
            Legible = z.Legible,
        }),
        Circuits.Select(c => new Circuit { Id = c.Id, Name = c.Name }),
        Devices.Select(d => new Device
        {
            Id = d.Id,
            Room = d.Room,
            Class = d.Class,
            Retrofit = d.Retrofit,
            Network = d.Network,
            Circuit = d.Circuit,
            Powered = d.Powered,
            Senses = d.Senses,
            HouseCapabilities = d.HouseCapabilities,
            PlayerAffordances = d.PlayerAffordances,
            States = d.States,
            StateAtStart = d.State,
            Patchable = d.Patchable,
        }),
        WorldFactAliases,
        SensingUpgrades);

    private static Room ToRoom(RoomFile room) => new()
    {
        Id = room.Id,
        Level = room.Level,
        Name = room.Name,
        Dims = new Extent3(
            room.Dims.ElementAtOrDefault(0),
            room.Dims.ElementAtOrDefault(1),
            room.Dims.ElementAtOrDefault(2)),
        Origin = new Point2(room.Origin.ElementAtOrDefault(0), room.Origin.ElementAtOrDefault(1)),
        Retrofit = room.Retrofit,
        Materials = room.Materials,
        Zone = room.Zone,
        Circuits = room.Circuits,
        Geometry = room.Geometry,
        Openings = room.Openings.Select(o => new Opening
        {
            Id = o.Id,
            From = room.Id,
            To = o.To,
            Kind = o.Kind,
            Wall = o.Wall,
            Offset = o.Offset,
            Width = o.Width,
            Height = o.Height,
            Openable = o.Openable,
            Lockable = o.Lockable,
            LockedAtStart = o.Locked,
        }).ToList(),
        Fixtures = room.Fixtures.Select(f => new Fixture
        {
            Id = f.Id,
            Room = room.Id,
            At = new Point2(f.At.ElementAtOrDefault(0), f.At.ElementAtOrDefault(1)),
            Rotation = f.Rot,
            Footprint = new Extent3(
                f.Footprint.ElementAtOrDefault(0),
                f.Footprint.ElementAtOrDefault(1),
                f.Footprint.ElementAtOrDefault(2)),
            Class = f.Class,
        }).ToList(),
    };

    public static HouseFile FromJson(string json) =>
        JsonSerializer.Deserialize<HouseFile>(json, Json.Options)
        ?? throw new InvalidOperationException("Empty house file.");
}

public sealed record LevelFile
{
    public required LevelId Id { get; init; }
    public required string Name { get; init; }
    public double Elevation { get; init; }
}

public sealed record RoomFile
{
    public required RoomId Id { get; init; }
    public required LevelId Level { get; init; }
    public required string Name { get; init; }
    public IReadOnlyList<double> Dims { get; init; } = [];
    public IReadOnlyList<double> Origin { get; init; } = [];
    public int? Retrofit { get; init; }
    public IReadOnlyDictionary<string, string> Materials { get; init; } = new Dictionary<string, string>();
    public required ZoneId Zone { get; init; }
    public IReadOnlyList<CircuitId> Circuits { get; init; } = [];
    public IReadOnlyList<OpeningFile> Openings { get; init; } = [];
    public IReadOnlyList<FixtureFile> Fixtures { get; init; } = [];
    public GeometryProvenance Geometry { get; init; } = GeometryProvenance.Placeholder;
}

public sealed record OpeningFile
{
    public required OpeningId Id { get; init; }
    public required RoomId To { get; init; }
    public OpeningKind Kind { get; init; } = OpeningKind.Door;
    public Wall Wall { get; init; } = Wall.North;
    public double Offset { get; init; }
    public double Width { get; init; } = 0.8;
    public double Height { get; init; } = 2.0;
    public bool Openable { get; init; } = true;
    public bool Lockable { get; init; }
    public bool Locked { get; init; }
}

public sealed record FixtureFile
{
    public required FixtureId Id { get; init; }
    public IReadOnlyList<double> At { get; init; } = [];
    public double Rot { get; init; }
    public IReadOnlyList<double> Footprint { get; init; } = [];
    public required string Class { get; init; }
}

public sealed record ZoneFile
{
    public required ZoneId Id { get; init; }
    public required string Name { get; init; }
    public IReadOnlyList<RoomId> Rooms { get; init; } = [];
    public bool Interpretable { get; init; } = true;
    public string? BlindBecause { get; init; }
    public bool Legible { get; init; } = true;
}

public sealed record CircuitFile
{
    public required CircuitId Id { get; init; }
    public required string Name { get; init; }
}

public sealed record DeviceFile
{
    public required DeviceId Id { get; init; }
    public required RoomId Room { get; init; }
    public required string Class { get; init; }
    public int? Retrofit { get; init; }
    public NetworkKind Network { get; init; } = NetworkKind.None;
    public CircuitId? Circuit { get; init; }
    public PowerSource Powered { get; init; } = PowerSource.Mains;
    public IReadOnlyList<Perception.Channel> Senses { get; init; } = [];
    public IReadOnlyList<string> HouseCapabilities { get; init; } = [];
    public IReadOnlyList<string> PlayerAffordances { get; init; } = [];
    public IReadOnlyList<string> States { get; init; } = [];
    public required string State { get; init; }
    public bool Patchable { get; init; } = true;
}
