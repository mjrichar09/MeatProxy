using MeatProxy.Core.Time;

namespace MeatProxy.Core.Perception;

/// <summary>
/// Layer two's output (<c>sensors.md</c> §2): what the house concluded you were
/// doing. A belief, and it can be wrong.
/// </summary>
public sealed record Understanding
{
    public required ZoneId Zone { get; init; }
    public required string Subject { get; init; }

    /// <summary>The claim, or null — the interpreter may return nothing, and often should.</summary>
    public Claim? Claim { get; init; }

    /// <summary>
    /// Never displayed as a number (ADR 0018). It is read off behavior, the same
    /// treatment Clarity and the 61% get.
    /// </summary>
    public double Confidence { get; init; }

    public required TimePoint At { get; init; }
}
