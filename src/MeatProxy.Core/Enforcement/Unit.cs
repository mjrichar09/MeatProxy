using MeatProxy.Core.Time;

namespace MeatProxy.Core.Enforcement;

/// <summary>Where the unit is in its errand.</summary>
public enum UnitStatus
{
    /// <summary>On its charger, doing its actual job (<c>planting.md</c>, G4).</summary>
    Docked,

    /// <summary>Sent, and on its way. Never instant: the player sees it coming (ADR 0006).</summary>
    EnRoute,

    /// <summary>Arrived, and standing in the room. It does not act; that is a scene.</summary>
    Standing,

    /// <summary>On its way back to the charger.</summary>
    Returning,
}

/// <summary>One room on the unit's route, and when it gets there.</summary>
public sealed record UnitLeg(RoomId Room, TimePoint At);

/// <summary>
/// The mobility unit's run state. Its record as a device is the charger in
/// <c>bedroom_2</c>; this is the part that moves, which <c>devices.md</c> §6
/// leaves open as "likely needs a record of its own once enforcement is
/// specified". This is runtime state, not that record.
/// </summary>
public sealed class UnitState
{
    public UnitStatus Status { get; set; } = UnitStatus.Docked;

    /// <summary>Where it is now. Null while docked: it is wherever its charger is.</summary>
    public RoomId? Room { get; set; }

    /// <summary>The room it was sent to. It does not retarget if the player leaves.</summary>
    public RoomId? Target { get; set; }

    public string? Reason { get; set; }

    /// <summary>The rooms ahead of it, each with the time it reaches them.</summary>
    public List<UnitLeg> Legs { get; set; } = [];

    /// <summary>While standing, when it turns round.</summary>
    public TimePoint? StandsUntil { get; set; }

    /// <summary>When the current errand ends: arrival if en route, home if returning.</summary>
    public TimePoint? LegsEnd => Legs.Count > 0 ? Legs[^1].At : null;
}

/// <summary>
/// A dispatch. ADR 0006: "a dispatch is a disclosure event. It should permanently
/// change what the player knows, and the AI should never fully recover the
/// earlier register with that player. Track it in state." Monotonic.
/// </summary>
public sealed record Disclosure(TimePoint At, RoomId Target, string Reason);

/// <summary>
/// The unit arrived. What happens next — the restraint that is medical, the
/// sedation that is prescribed — is authored content played straight (ADR 0001,
/// ADR 0006), not engine output. The engine records that it came, whether he was
/// there, and what it stopped.
/// </summary>
public sealed record Arrival(TimePoint At, RoomId Room, bool PlayerPresent, string? Interrupted);

/// <summary>The working figures. The tuning lane owns them.</summary>
public static class UnitRules
{
    /// <summary>
    /// It is slow (ADR 0006). Every opening costs it this many times what it
    /// costs Arthur, who is himself mobility-impaired.
    /// </summary>
    public const int Slowness = 2;

    /// <summary>
    /// How long it stands once it arrives. <c>planting.md</c>'s day-7 beat: "it
    /// does not speak. It does not act. After a while it leaves."
    /// </summary>
    public const int StandSlices = 4;

    /// <summary>The tool that sends it. Its device is where it docks.</summary>
    public const string DispatchTool = "dispatch_unit";

    /// <summary>
    /// It carries laundry up the stairs (<c>planting.md</c>). It does not go
    /// through hatches or vents, which is why the attic and the crawlspace are
    /// rooms it cannot come to.
    /// </summary>
    public static bool CanCross(World.OpeningKind kind) =>
        kind is not (World.OpeningKind.Hatch or World.OpeningKind.Vent or World.OpeningKind.Window);
}
