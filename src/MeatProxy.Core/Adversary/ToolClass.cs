namespace MeatProxy.Core.Adversary;

/// <summary>
/// The rows of <c>alert-tiers.md</c>'s table, as kinds of tool. A tier does not
/// withhold tools by name; it withholds classes, and the toolset falls out
/// (<c>devices.md</c> §2).
/// </summary>
public enum ToolClass
{
    /// <summary>Reading the house. Interpretation is retained at every tier.</summary>
    Sensing,

    /// <summary>The house's voice. Gone at Silent, where it is a building again.</summary>
    Speech,

    /// <summary>Media, displays, printing — the theater. Lost at Silent.</summary>
    Comfort,

    /// <summary>
    /// Heat, water, the hob. The part of comfort that is the house still running,
    /// which is why it survives Silent (<c>alert-tiers.md</c>: "The house still
    /// runs — heat, water, power").
    /// </summary>
    Utility,

    /// <summary>Ordering things. Needs outward contact, so it goes at Read-only.</summary>
    Procurement,

    /// <summary>Unlocking and opening for the player. Open only.</summary>
    Unlock,

    Calendar,

    /// <summary>
    /// Calling out. The first thing lost at Guarded, and the thing that returns at
    /// Dropped (ADR 0025 §3).
    /// </summary>
    OutwardContact,

    /// <summary>The unit. Retained at every tier (ADR 0006 prices it, not the ladder).</summary>
    Enforcement,

    /// <summary>
    /// Patching and zone lockdown. Never gated: "these are not courtesies, they are
    /// how the game works" (<c>alert-tiers.md</c>).
    /// </summary>
    Containment,
}

/// <summary>
/// Which class each authored tool is in, and which classes each tier keeps.
/// </summary>
/// <remarks>
/// <para>
/// The classification is an engine default, not a schema field: <c>devices.md</c>
/// says the toolset is "minus what the tier withholds" and <c>alert-tiers.md</c>
/// says what each tier withholds by kind, but no document sorts tool names into
/// those kinds. It is carried here so the ladder can run, and
/// <see cref="House.Validate"/> refuses a device that declares a tool this table
/// does not know — a new tool must be classified before it can be held, or it
/// would sail past every tier unfiltered.
/// </para>
/// <para>
/// No authored device declares an <see cref="ToolClass.OutwardContact"/> tool
/// yet. The class exists because the table needs it; the device that carries it
/// is Lane C's.
/// </para>
/// </remarks>
public static class Tools
{
    private static readonly Dictionary<string, ToolClass> Classes = new()
    {
        ["report_motion"] = ToolClass.Sensing,
        ["review_footage"] = ToolClass.Sensing,
        ["report_boiler_state"] = ToolClass.Sensing,
        ["report_door_state"] = ToolClass.Sensing,
        ["report_audio"] = ToolClass.Sensing,
        ["report_thermal"] = ToolClass.Sensing,
        ["report_stock"] = ToolClass.Sensing,
        ["report_humidity"] = ToolClass.Sensing,
        ["report_network"] = ToolClass.Sensing,
        ["report_sleep"] = ToolClass.Sensing,
        ["report_unit_state"] = ToolClass.Sensing,
        ["report_activity"] = ToolClass.Sensing,
        ["report_weather"] = ToolClass.Sensing,
        ["scan_parcel"] = ToolClass.Sensing,

        ["speak"] = ToolClass.Speech,

        ["show_on_display"] = ToolClass.Comfort,
        ["play_media"] = ToolClass.Comfort,
        ["show_captions"] = ToolClass.Comfort,
        ["print_label"] = ToolClass.Comfort,
        ["print_document"] = ToolClass.Comfort,

        ["set_boiler_mode"] = ToolClass.Utility,
        ["cut_hob"] = ToolClass.Utility,

        ["order_groceries"] = ToolClass.Procurement,

        ["unlock_door"] = ToolClass.Unlock,
        ["open_garage_door"] = ToolClass.Unlock,

        ["show_calendar"] = ToolClass.Calendar,

        ["dispatch_unit"] = ToolClass.Enforcement,

        ["lock_door"] = ToolClass.Containment,
        ["close_garage_door"] = ToolClass.Containment,
        ["isolate_device"] = ToolClass.Containment,
    };

    public static bool IsClassified(string tool) => Classes.ContainsKey(tool);

    public static ToolClass ClassOf(string tool) =>
        Classes.TryGetValue(tool, out var kind)
            ? kind
            : throw new KeyNotFoundException($"Tool '{tool}' has no class, so no tier knows whether to hold it.");
}

/// <summary>
/// <c>alert-tiers.md</c>'s table, read as which classes the house keeps at each
/// tier. What it grants you and what it retains over you decay together, until
/// Dropped, where the second snaps back.
/// </summary>
public static class TierPolicy
{
    public static bool Holds(AlertTier tier, ToolClass kind) => kind switch
    {
        ToolClass.Sensing or ToolClass.Enforcement or ToolClass.Containment => true,

        ToolClass.Unlock or ToolClass.Calendar => tier == AlertTier.Open,

        // Lost at Guarded, back at Dropped: a disarmed house cannot call anyone,
        // and a house with the mask off can.
        ToolClass.OutwardContact => tier is AlertTier.Open or AlertTier.Dropped,

        // Read-only loses it because ordering needs outward contact. Dropped does
        // not get it back with the phone: "probably it simply stops catering"
        // (alert-tiers.md, open), taken as the default.
        ToolClass.Procurement => tier <= AlertTier.Monitored,

        ToolClass.Comfort => tier <= AlertTier.ReadOnly,

        // Read-only talks at you. Dropped speaks too, without the politeness —
        // the reveal is authored and it is said aloud. ADR 0036's always-present,
        // register-shifting speech would make this true at Silent as well; that
        // is a pending D3 change to this schema and is not applied here.
        ToolClass.Speech => tier != AlertTier.Silent,

        // The building keeps running at every tier. What Dropped withdraws is the
        // apparatus, not the boiler.
        ToolClass.Utility => true,

        _ => false,
    };

    public static bool Holds(AlertTier tier, string tool) => Holds(tier, Tools.ClassOf(tool));
}
