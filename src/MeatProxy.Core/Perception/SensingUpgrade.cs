namespace MeatProxy.Core.Perception;

/// <summary>
/// The sensing surface getting better rather than merely more suspicious.
/// </summary>
/// <remarks>
/// <para>
/// The wifi turn is a device change, not a rule change (<c>devices.md</c> §5).
/// The router gains a channel, some zones it could not read become readable, and
/// no new system appears anywhere — which is the test of whether the device
/// schema was shaped right.
/// </para>
/// <para>
/// It is a concession and not an upgrade (ADR 0014). When the house starts seeing
/// through walls it has decided that the risk of losing him outweighs the cost of
/// flattening him, and that is a plot beat. <see cref="Reason"/> is where the
/// house's own account of it lives, so applying one is never silent.
/// </para>
/// </remarks>
public sealed record SensingUpgrade
{
    public required string Id { get; init; }

    /// <summary>The device that gains the channel.</summary>
    public required DeviceId Device { get; init; }

    public required Channel Channel { get; init; }

    /// <summary>
    /// Zones this makes interpretable that were not. Which zones the wifi turn
    /// actually opens is a content call: the attic is neglect and can be seen
    /// through, the crawlspace has nothing to see through, and the bathroom is
    /// the concession the house will defend if asked — so that one is a separate,
    /// heavier decision and is not in here.
    /// </summary>
    public IReadOnlyList<ZoneId> OpensZones { get; init; } = [];

    public string Reason { get; init; } = string.Empty;
}
