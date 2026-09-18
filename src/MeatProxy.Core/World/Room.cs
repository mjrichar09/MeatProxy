namespace MeatProxy.Core.World;

/// <summary>A space in the house (<c>world.md</c> §3, <c>rooms.md</c>).</summary>
public sealed record Room
{
    public required RoomId Id { get; init; }
    public required LevelId Level { get; init; }
    public required string Name { get; init; }

    public Extent3 Dims { get; init; }

    /// <summary>The room's near-left corner on its level.</summary>
    public Point2 Origin { get; init; }

    /// <summary>
    /// The year the room's fabric or its hardware dates from. Load-bearing, not
    /// flavor: it decides what the house can see there (<c>world.md</c> §3). The
    /// attic is pre-2025 and therefore a blind spot; the crawlspace has no date
    /// at all because it has nothing in it.
    /// </summary>
    public int? Retrofit { get; init; }

    public IReadOnlyDictionary<string, string> Materials { get; init; } =
        new Dictionary<string, string>();

    public IReadOnlyList<Opening> Openings { get; init; } = [];
    public IReadOnlyList<Fixture> Fixtures { get; init; } = [];

    /// <summary>The observation zone this room belongs to (<c>sensors.md</c> §3).</summary>
    public required ZoneId Zone { get; init; }

    public IReadOnlyList<CircuitId> Circuits { get; init; } = [];

    public GeometryProvenance Geometry { get; init; } = GeometryProvenance.Placeholder;
}
