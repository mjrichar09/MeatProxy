using MeatProxy.Core.Time;

namespace MeatProxy.Core.Perception;

/// <summary>
/// Layer one's output: a channel, a zone, a magnitude, a time. No subject and no
/// verb (<c>sensors.md</c> §1).
/// </summary>
/// <remarks>
/// Detections are never attributed. The 3am motion in the basement is motion in
/// the basement; that it was the player, tracing a circuit, is a conclusion, and
/// conclusions come from layer two.
/// </remarks>
public sealed record Detection
{
    public required Channel Channel { get; init; }
    public required ZoneId Zone { get; init; }
    public required Magnitude Magnitude { get; init; }
    public required TimePoint At { get; init; }

    /// <summary>
    /// The device that emitted it. Null when the detection is an
    /// <em>absence</em> — a channel that has never gone quiet going quiet is
    /// itself a high-magnitude event (<c>sensors.md</c> §4).
    /// </summary>
    public DeviceId? Device { get; init; }
}
