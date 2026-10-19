using MeatProxy.Core.Perception;

namespace MeatProxy.Core.Experimental.Injection;

// EXPERIMENTAL (ADR 0039). Built against injection-vectors.md while it is still
// provisional, to be played and revised. Nothing frozen depends on this
// namespace; deleting it, Simulation.Injection.cs and WorldState.Injection is the
// whole of removing it.

/// <summary>
/// What a mouth has standing to talk about (<c>injection-vectors.md</c>
/// <c>asserts</c>, ADR 0034). Not closed yet in the schema; these are the four the
/// first slice needs.
/// </summary>
public enum Domain
{
    /// <summary>Errands, lists, what the household is out of.</summary>
    DomesticRoutine,

    /// <summary>Appointments and visits Arthur has made for himself.</summary>
    OwnCommitments,

    /// <summary>Deliveries, and where and when they are received.</summary>
    Logistics,

    /// <summary>Whether a piece of the house is in service or being serviced.</summary>
    EquipmentState,
}

public enum Trust
{
    Low,
    Medium,
    High,
}

/// <summary>How the artifact is made. ADR 0034: it varies by surface, and so does capacity.</summary>
public enum Composition
{
    /// <summary>Free text inside a character bound. A marker, a keyboard.</summary>
    FreeText,

    /// <summary>A fixed form. A label printer's fields.</summary>
    Fields,
}

/// <summary>A field on a fixed form, and what it can be set to.</summary>
public sealed record FormField(string Name, IReadOnlyList<string> Options);

/// <summary>What has to already be true for the mouth to be plausible (ADR 0015).</summary>
public enum Pretext
{
    None,

    /// <summary>A parcel the house itself ordered is due today or earlier.</summary>
    ExpectedDelivery,
}

/// <summary>
/// One place data enters the house's reading (<c>injection-vectors.md</c> §1), cut
/// down to the fields the first slice plays.
/// </summary>
public sealed record Vector
{
    public required string Id { get; init; }
    public required string Name { get; init; }

    /// <summary>Where the artifact is made. The room Arthur has to be in to make it.</summary>
    public required DeviceId ComposedAt { get; init; }

    /// <summary>Where it is read. The room Arthur has to be in to expose it.</summary>
    public required DeviceId Surface { get; init; }

    /// <summary>The mouth it forges.</summary>
    public required string Source { get; init; }

    /// <summary>
    /// Whether the mouth is someone other than Arthur. The board's household and
    /// the diary's past self are him; seeing him write on them is ordinary life.
    /// The carrier is not, and watching him print its label is a giveaway.
    /// </summary>
    public bool ForgesSomeoneElse { get; init; }

    public required IReadOnlySet<Domain> Asserts { get; init; }
    public required Trust Trust { get; init; }
    public Pretext Pretext { get; init; } = Pretext.None;

    public required Composition Composition { get; init; }

    /// <summary>Characters, for free text. Per surface (ADR 0034).</summary>
    public int Capacity { get; init; }

    public IReadOnlyList<FormField> Fields { get; init; } = [];

    /// <summary>
    /// What Arthur is really doing while he makes it. The interpreter gets the
    /// readings; if it reads them right, the house saw him make it.
    /// </summary>
    public required Claim MakingIt { get; init; }

    public int SlicesToCompose { get; init; } = 2;

    /// <summary>True when making it and exposing it are the same act, in the same place.</summary>
    public bool ExposedWhereMade => ComposedAt == Surface;

    /// <summary>A line for the page about what this mouth is and why it is believed.</summary>
    public required string Blurb { get; init; }
}

/// <summary>
/// The first slice's three vectors (ADR 0039 §4): the floor, and the schema's
/// worked pair. Authored here, in code, while the schema is provisional; they move
/// to data when it freezes.
/// </summary>
public static class Catalog
{
    public static readonly Vector KitchenBoard = new()
    {
        Id = "v_kitchen_board",
        Name = "The kitchen board",
        ComposedAt = new DeviceId("kitchen.fridge"),
        Surface = new DeviceId("kitchen.fridge"),
        Source = "the household",
        Asserts = new HashSet<Domain> { Domain.DomesticRoutine, Domain.OwnCommitments },
        Trust = Trust.Low,
        Composition = Composition.FreeText,
        Capacity = 100,
        MakingIt = Claim.AtASurface,
        SlicesToCompose = 2,
        Blurb = "The day-and-plan board on the fridge door, from Ruth's care package. The house reads it as the household's list.",
    };

    public static readonly Vector ShippingLabel = new()
    {
        Id = "v_shipping_label",
        Name = "A shipping label",
        ComposedAt = new DeviceId("garage.label_printer"),
        Surface = new DeviceId("office.parcel_scanner"),
        Source = "the carrier",
        ForgesSomeoneElse = true,
        Asserts = new HashSet<Domain> { Domain.Logistics },
        Trust = Trust.High,
        Pretext = Pretext.ExpectedDelivery,
        Composition = Composition.Fields,
        Fields =
        [
            new FormField("Receive at", ["front door", "garage", "kitchen", "office"]),
            new FormField("Service", ["standard", "delivery window", "signature required", "maintenance"]),
            new FormField("When", ["today", "tomorrow"]),
        ],
        MakingIt = Claim.HandlingDevice,
        SlicesToCompose = 2,
        Blurb = "Printed in the garage, stuck on a parcel, scanned in the office. The carrier is trusted about deliveries and nothing else.",
    };

    public static readonly Vector CalendarEntry = new()
    {
        Id = "v_calendar_entry",
        Name = "A calendar entry",
        ComposedAt = new DeviceId("office.smart_display"),
        Surface = new DeviceId("office.smart_display"),
        Source = "Arthur's own past self",
        Asserts = new HashSet<Domain> { Domain.OwnCommitments },
        Trust = Trust.Medium,
        Composition = Composition.FreeText,
        Capacity = 60,
        MakingIt = Claim.Working,
        SlicesToCompose = 1,
        Blurb = "Typed on the office display. The house keeps his diary and believes it about his own plans.",
    };

    public static IReadOnlyList<Vector> All { get; } = [KitchenBoard, ShippingLabel, CalendarEntry];

    public static Vector? Find(string id) => All.FirstOrDefault(v => v.Id == id);
}
