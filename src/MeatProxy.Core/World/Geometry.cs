namespace MeatProxy.Core.World;

/// <summary>A point on a level's floor plane, in meters (<c>world.md</c> §3).</summary>
public readonly record struct Point2(double X, double Y);

/// <summary>Width, depth, height in meters.</summary>
public readonly record struct Extent3(double Width, double Depth, double Height);

/// <summary>Which wall of a room an opening sits in.</summary>
public enum Wall
{
    North,
    South,
    East,
    West,
}

/// <summary>What kind of hole in the wall this is.</summary>
public enum OpeningKind
{
    Door,
    Stair,
    Hatch,
    Vent,
    Window,
    Archway,
}

/// <summary>
/// How a room's geometry got its numbers. Not part of <c>world.md</c> — it is a
/// build-data annotation so nothing downstream mistakes placeholder dimensions
/// for an authored room. <c>utility</c> is the schema's worked example and is
/// the only <see cref="Authored"/> room today.
/// </summary>
public enum GeometryProvenance
{
    Placeholder,
    Authored,
}
