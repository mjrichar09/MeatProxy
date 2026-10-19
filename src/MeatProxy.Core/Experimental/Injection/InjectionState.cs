using MeatProxy.Core.Time;

namespace MeatProxy.Core.Experimental.Injection;

/// <summary>A made thing, in Arthur's hands, not yet read by the house.</summary>
/// <param name="SeenBeingMade">
/// Whether the house understood what he was doing while he made it. Resolved at
/// the time, independently of how the artifact is later read
/// (<c>interaction-model.md</c> §4) — and remembered.
/// </param>
public sealed record Artifact(string Id, string Vector, string Text, TimePoint MadeAt, RoomId MadeIn, bool SeenBeingMade);

public enum Verdict
{
    /// <summary>Taken as true, and acted on.</summary>
    Believed,

    /// <summary>Read, and not believed. Nothing happens, and nothing is held against him.</summary>
    Doubted,

    /// <summary>Recognized as a forgery. The house is on guard now.</summary>
    Flagged,

    /// <summary>Read, and found to say nothing the house acts on.</summary>
    NothingToAct,
}

/// <summary>One reading, with every step of the reasoning, for the log and the page.</summary>
public sealed record Ingestion(TimePoint At, string Vector, string Text, Verdict Verdict, IReadOnlyList<string> Steps, string? Response);

/// <summary>An entry in the diary the house keeps for him.</summary>
public sealed record DiaryEntry(int Day, string What, string Source);

/// <summary>A parcel the house ordered, due on a day.</summary>
public sealed record Delivery(int Day, string Item, bool Used);

/// <summary>The experiment's run state. One field on <c>WorldState</c>.</summary>
public sealed class InjectionState
{
    public List<Artifact> InHand { get; set; } = [];
    public List<Ingestion> Log { get; set; } = [];
    public List<DiaryEntry> Diary { get; set; } = [];
    public List<Delivery> Deliveries { get; set; } = [];

    /// <summary>A delivery window the garage is open for, and the day it closes at the end of.</summary>
    public int? GarageReceivingUntilDay { get; set; }

    public int Made { get; set; }
}
