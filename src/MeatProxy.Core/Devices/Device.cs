using MeatProxy.Core.Perception;

namespace MeatProxy.Core.Devices;

/// <summary>How a device is reachable over the network (<c>devices.md</c> §1).</summary>
public enum NetworkKind
{
    /// <summary>
    /// The strongest property in the schema. A device with no network is a thing
    /// the house owns nothing of — the landline in the hall, the fuse box, the
    /// crawlspace's nothing-at-all. These are the edge of the house's coverage,
    /// and the game is played along it.
    /// </summary>
    None,
    Wifi,
    Zigbee,
    Wired,
}

/// <summary>Where a device gets its power, which decides what a cut circuit takes out.</summary>
public enum PowerSource
{
    None,
    Mains,
    Battery,

    /// <summary>Mains with a battery behind it. The enforcement unit has one.</summary>
    Both,
}

/// <summary>
/// One record, three readings (<c>devices.md</c>): what the house can do with it,
/// what the house can sense with it, and what the player can do to it. Three
/// systems that would otherwise drift apart cannot, because they are columns of
/// one table.
/// </summary>
public sealed record Device
{
    public required DeviceId Id { get; init; }
    public required RoomId Room { get; init; }

    /// <summary>A class such as <c>lock</c>, <c>camera</c>, <c>router</c>.</summary>
    public required string Class { get; init; }

    /// <summary>
    /// Overrides the room's date (<c>world.md</c> §3). Pre-2025 hardware is dumb
    /// hardware.
    /// </summary>
    public int? Retrofit { get; init; }

    public NetworkKind Network { get; init; } = NetworkKind.None;
    public CircuitId? Circuit { get; init; }
    public PowerSource Powered { get; init; } = PowerSource.Mains;

    /// <summary>
    /// The detection channels it contributes. A room has audio because a device
    /// in it has <c>audio_level</c> here — the channel is a consequence of the
    /// device list, not a separate authored fact (<c>devices.md</c> §3).
    /// </summary>
    public IReadOnlyList<Channel> Senses { get; init; } = [];

    /// <summary>
    /// The tools the house may hold for this device. The whitelist is never
    /// authored separately — it is derived from these, filtered by tier
    /// (<c>devices.md</c> §2). <c>unlock_door</c> is absent when the door is
    /// bolted because the bolt is a physical state the precondition tests, not
    /// because a list was edited.
    /// </summary>
    public IReadOnlyList<string> HouseCapabilities { get; init; } = [];

    /// <summary>What it offers the player when selected (<c>affordances.md</c>).</summary>
    public IReadOnlyList<string> PlayerAffordances { get; init; } = [];

    /// <summary>
    /// The closed set of states this device may be in. <c>change_device_state</c>
    /// is validated against exactly this list (<c>effects.md</c> §1).
    /// </summary>
    public IReadOnlyList<string> States { get; init; } = [];

    /// <summary>The authored starting state; the run's copy lives in the world state.</summary>
    public required string StateAtStart { get; init; }

    /// <summary>
    /// A promise to the player. Every exploit is temporary, but a physical delta
    /// the house cannot reach is the preparation layer's whole basis (ADR 0015).
    /// If the house cannot patch it, it has to route around it.
    /// </summary>
    public bool Patchable { get; init; } = true;

    /// <summary>
    /// Whether the house can address it at all. Not the same as powered: the
    /// landline is powered and unreachable, which is the point of it.
    /// </summary>
    public bool ReachableByHouse => Network != NetworkKind.None;

    /// <summary>Pre-2025 hardware is dumb hardware (<c>devices.md</c> §1).</summary>
    public bool IsSmart => Retrofit is null or >= 2025;
}
