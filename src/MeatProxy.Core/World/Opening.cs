namespace MeatProxy.Core.World;

/// <summary>
/// A connection between two rooms, or between a room and the outside
/// (<c>world.md</c> §3). Movement is room to room through these; there is no
/// navmesh and no pathing data, deliberately (<c>world.md</c> §4).
/// </summary>
public sealed record Opening
{
    public required OpeningId Id { get; init; }

    /// <summary>The room this opening is recorded against.</summary>
    public required RoomId From { get; init; }

    /// <summary>
    /// The far side. <see cref="RoomId.Exterior"/> means it leaves the house,
    /// which is what makes the front door, the garage door and the patio gate
    /// the three most interesting records in the file.
    /// </summary>
    public required RoomId To { get; init; }

    public required OpeningKind Kind { get; init; }
    public required Wall Wall { get; init; }

    /// <summary>Meters from the wall's near end.</summary>
    public double Offset { get; init; }

    public double Width { get; init; }
    public double Height { get; init; }

    /// <summary>Whether it can be opened at all. A vent cannot.</summary>
    public bool Openable { get; init; } = true;

    /// <summary>Whether it carries a lock, smart or otherwise.</summary>
    public bool Lockable { get; init; }

    /// <summary>
    /// Whether it is locked right now. Authored as the starting state; the run's
    /// copy lives in <see cref="State.WorldState"/>, because a door that gets
    /// unlocked must not edit the authored house.
    /// </summary>
    public bool LockedAtStart { get; init; }

    public bool LeavesTheHouse => To.IsExterior;
}
