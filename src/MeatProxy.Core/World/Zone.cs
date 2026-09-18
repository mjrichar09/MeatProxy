namespace MeatProxy.Core.World;

/// <summary>
/// The unit both perception layers address (<c>sensors.md</c> §3). Usually one
/// room, sometimes several.
/// </summary>
public sealed record Zone
{
    public required ZoneId Id { get; init; }
    public required string Name { get; init; }
    public IReadOnlyList<RoomId> Rooms { get; init; } = [];

    /// <summary>
    /// Whether the interpreter can be pointed here at all. Three zones are false
    /// for three different reasons — the attic is neglect, the crawlspace is
    /// absence, the bathroom is a choice — and the player should be able to tell
    /// them apart (<c>sensors.md</c> §3).
    /// </summary>
    public bool Interpretable { get; init; } = true;

    /// <summary>Why it is not interpretable, for the debug view and for the bible.</summary>
    public string? BlindBecause { get; init; }

    /// <summary>
    /// Must be readable on screen without a debug view. An exit criterion, not a
    /// hint (<c>sensors.md</c> §3).
    /// </summary>
    public bool Legible { get; init; } = true;
}

/// <summary>
/// An electrical circuit. Cutting one stops every non-battery device on it,
/// which is what makes cutting power a real verb (<c>devices.md</c> §1).
/// </summary>
public sealed record Circuit
{
    public required CircuitId Id { get; init; }
    public required string Name { get; init; }
}
