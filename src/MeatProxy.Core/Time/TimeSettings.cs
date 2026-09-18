namespace MeatProxy.Core.Time;

/// <summary>
/// The numbers time is denominated in.
/// </summary>
/// <remarks>
/// ADR 0008 leaves the slice-to-minute ratio as a schema number and it has not
/// been ruled on, so these are tunables carried in data rather than constants
/// compiled in. The defaults are working figures chosen to make the systems that
/// depend on them land where the decisions say they should: a patch cycle of
/// about an hour of game time is six slices, and an attention window of about
/// forty minutes is four.
/// </remarks>
public sealed record TimeSettings
{
    /// <summary>Minutes of game time in one slice. A working figure, not a ruling.</summary>
    public int MinutesPerSlice { get; init; } = 10;

    /// <summary>
    /// The slice budget of an undegraded day — sixteen waking hours at ten
    /// minutes a slice. Clarity's first symptom is this number shrinking
    /// (ADR 0007), which is why it is a budget and not a constant.
    /// </summary>
    public int FullDayBudget { get; init; } = 96;

    /// <summary>The first day of a run.</summary>
    public int FirstDay { get; init; } = 1;

    public static TimeSettings Default { get; } = new();
}
