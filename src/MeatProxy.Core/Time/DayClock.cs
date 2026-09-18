namespace MeatProxy.Core.Time;

/// <summary>
/// The tick. Actions spend slices; when the budget is gone the day is over.
/// Sleeping ends the day early and banks the overnight review (ADR 0008).
/// </summary>
/// <remarks>
/// Slices are in the tick from E1 rather than bolted on at E5, which is ADR
/// 0008's explicit instruction to this lane. There is no real-time path here at
/// all: pressure windows are a later, separate mode, and the rule that keeps
/// them honest — no pressure window may depend on a model call — is easier to
/// hold if turn time is the only thing the core knows how to count.
/// </remarks>
public sealed class DayClock
{
    private readonly TimeSettings _settings;

    public DayClock(TimeSettings settings, int day, int slicesSpent, int budget)
    {
        _settings = settings;
        Day = day;
        SlicesSpent = slicesSpent;
        Budget = budget;
    }

    public static DayClock StartOfRun(TimeSettings settings) =>
        new(settings, settings.FirstDay, 0, settings.FullDayBudget);

    public int Day { get; private set; }

    /// <summary>Slices spent so far today. This is the <c>slice</c> of the summary's <c>now</c>.</summary>
    public int SlicesSpent { get; private set; }

    /// <summary>
    /// Today's budget. Shrinks as Clarity degrades — the player experiences it as
    /// time slipping and the engine implements it as a smaller number (ADR 0007).
    /// </summary>
    public int Budget { get; private set; }

    public int SlicesRemaining => Math.Max(0, Budget - SlicesSpent);

    public bool DayIsSpent => SlicesRemaining == 0;

    public TimePoint Now => new(Day, SlicesSpent);

    public TimeSettings Settings => _settings;

    /// <summary>
    /// Spend slices. Returns how many were actually spent, which is less than
    /// asked for when the day runs out underneath the action. The caller decides
    /// what a half-finished action means; the clock only counts.
    /// </summary>
    public int Spend(int slices)
    {
        if (slices < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(slices), "Time does not run backwards.");
        }

        var spent = Math.Min(slices, SlicesRemaining);
        SlicesSpent += spent;
        return spent;
    }

    /// <summary>
    /// End the day. The next day starts with <paramref name="nextBudget"/>, which
    /// the Clarity system supplies once it exists; until then it is the full day.
    /// </summary>
    public void EndDay(int? nextBudget = null)
    {
        Day++;
        SlicesSpent = 0;
        Budget = nextBudget ?? _settings.FullDayBudget;
    }

    /// <summary>Set today's budget, for Clarity to shrink (ADR 0007).</summary>
    public void SetBudget(int budget)
    {
        if (budget < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(budget));
        }

        Budget = budget;
    }
}
