namespace MeatProxy.Core.Perception;

/// <summary>
/// Layer one's channels (<c>sensors.md</c> §1). Always on, everywhere there is
/// hardware, essentially free.
/// </summary>
public enum Channel
{
    Motion,
    DoorState,
    PowerDraw,
    Thermal,
    AudioLevel,
    Humidity,
    NetworkEvent,

    /// <summary>
    /// A body through a wall. The router gains this at the wifi-sensing turn
    /// (<c>DESIGN.md</c> §5) — one field, one plot beat, no new system, which is
    /// the test of whether the device schema is shaped right.
    /// </summary>
    PresenceRf,
}

/// <summary>Three buckets, never numbers (<c>effects.md</c> §1).</summary>
public enum Magnitude
{
    Low,
    Medium,
    High,
}
