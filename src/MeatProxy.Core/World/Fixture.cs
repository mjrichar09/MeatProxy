namespace MeatProxy.Core.World;

/// <summary>
/// Furniture. A fixture that does something is a device as well, and carries a
/// separate id, so the same boiler can be dumb metal with a smart controller
/// bolted to it (<c>world.md</c> §1).
/// </summary>
public sealed record Fixture
{
    public required FixtureId Id { get; init; }
    public required RoomId Room { get; init; }
    public Point2 At { get; init; }

    /// <summary>Degrees, clockwise from north.</summary>
    public double Rotation { get; init; }

    public Extent3 Footprint { get; init; }

    /// <summary>A build-script class such as <c>appliance_large</c>, <c>wall_panel</c>.</summary>
    public required string Class { get; init; }
}
