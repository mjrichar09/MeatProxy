using MeatProxy.Core.State;
using MeatProxy.Core.Time;

namespace MeatProxy.Core.Adversary;

/// <summary>One step on the ladder, either way, and why. The log the reveal reads from.</summary>
public sealed record TierChange
{
    public required TimePoint At { get; init; }
    public required AlertTier From { get; init; }
    public required AlertTier To { get; init; }
    public required string Reason { get; init; }

    public bool IsEscalation => To > From;
}

/// <summary>
/// How tier moves (<c>DESIGN.md</c> §3, ADR 0004, ADR 0008). Down by what the
/// player does, back up by days of not doing it, and never back from Dropped.
/// </summary>
/// <remarks>
/// <para>
/// Nothing in the effect vocabulary moves tier (<c>effects.md</c>), and nothing
/// here asks a model. What decides that something is worth an escalation is the
/// adversary's judgement and arrives later; this is only the ladder it acts on,
/// and the ladder is engine state.
/// </para>
/// <para>
/// The walk-back rate is a tuning target (ADR 0004) with one hard constraint —
/// the floor must be reachable — so <see cref="CleanDaysPerStep"/> is a default
/// taken so the ladder can run, not a decision.
/// </para>
/// </remarks>
public static class AlertLadder
{
    /// <summary>"Days of good behavior" (§3), read as two. A tuning default.</summary>
    public const int CleanDaysPerStep = 2;

    /// <summary>
    /// Push the house down the ladder. Getting caught walks it down hard
    /// (ADR 0004), which is what <paramref name="steps"/> is for. Returns the
    /// change, or null when it is already at the floor.
    /// </summary>
    public static TierChange? Escalate(WorldState state, string reason, int steps = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(steps, 1);

        var from = state.Tier;
        var to = (AlertTier)Math.Min((int)from + steps, (int)AlertTier.Dropped);

        // Any escalation spoils the day, even at the floor.
        state.EscalatedToday = true;
        state.CleanDays = 0;

        if (to == from)
        {
            return null;
        }

        return Record(state, from, to, reason);
    }

    /// <summary>
    /// The overnight half: called when a day ends. A day with no escalation in it
    /// counts toward a step back up. Dropped is a reveal, not a setback, and
    /// nothing walks back from it.
    /// </summary>
    public static TierChange? EndOfDay(WorldState state)
    {
        var clean = !state.EscalatedToday;
        state.EscalatedToday = false;

        if (state.Tier is AlertTier.Open or AlertTier.Dropped || !clean)
        {
            state.CleanDays = 0;
            return null;
        }

        state.CleanDays++;
        if (state.CleanDays < CleanDaysPerStep)
        {
            return null;
        }

        state.CleanDays = 0;
        return Record(state, state.Tier, state.Tier - 1, $"{CleanDaysPerStep} quiet days");
    }

    private static TierChange Record(WorldState state, AlertTier from, AlertTier to, string reason)
    {
        var change = new TierChange { At = state.Now, From = from, To = to, Reason = reason };
        state.Tier = to;
        state.TierHistory.Add(change);
        return change;
    }
}
