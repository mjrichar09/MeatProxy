namespace MeatProxy.Core.World;

/// <summary>
/// One of the five levels of ADR 0010, plus the bounded yard. Elevation is the
/// floor plane in meters, used by the Blender build script and by nothing in
/// the engine (<c>world.md</c> §2) — it is carried here so one record produces
/// both the playable house and the rendered one (ADR 0012).
/// </summary>
public sealed record Level
{
    public required LevelId Id { get; init; }
    public required string Name { get; init; }
    public double Elevation { get; init; }
}
