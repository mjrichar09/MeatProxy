using MeatProxy.Core.State;

namespace MeatProxy.Core.Perception;

/// <summary>
/// When a conclusion stops being something the house thought and becomes
/// something the house holds.
/// </summary>
/// <remarks>
/// <para>
/// This is <c>claims.md</c> §2's bridge. A claim is a belief and can be wrong; a
/// predicate is a fact and cannot. The house <em>holding</em> a wrong belief is
/// itself a fact, and that is what <c>house_believes</c> reports.
/// </para>
/// <para>
/// It is also what makes the salient cap a real exploit rather than a gimmick.
/// Promotion is the only way in, eviction is the only way out, and both are
/// engine rules the player can learn.
/// </para>
/// <para>
/// The thresholds are open in <c>claims.md</c> §4 and want tuning against a
/// playthrough, so they are carried in the save rather than compiled in.
/// </para>
/// </remarks>
public sealed record PromotionRule
{
    /// <summary>Below this the house noticed something but is not sure enough to keep it.</summary>
    public double MinConfidence { get; init; } = 0.5;

    /// <summary>How many times it has to read you the same way before it sticks.</summary>
    public int MinSightings { get; init; } = 2;

    /// <summary>How far back those sightings may reach.</summary>
    public int WithinDays { get; init; } = 2;

    public static PromotionRule Default { get; } = new();

    /// <summary>The salient fact a promoted claim is filed under.</summary>
    public static string FactFor(Claim claim, ZoneId zone) =>
        $"believes_{Predicates.Predicate.Spell(claim)}_in_{zone.Value}";

    /// <summary>
    /// Whether this understanding has now been read often enough, recently
    /// enough, to be held. Returns the fact to promote, or null.
    /// </summary>
    public SalientFact? Consider(Understanding latest, IReadOnlyList<Understanding> history)
    {
        if (latest.Claim is not { } claim || latest.Confidence < MinConfidence)
        {
            return null;
        }

        var sightings = history.Count(u =>
            u.Zone == latest.Zone
            && u.Claim == claim
            && u.Confidence >= MinConfidence
            && u.At.Day > latest.At.Day - WithinDays);

        if (sightings < MinSightings)
        {
            return null;
        }

        return new SalientFact
        {
            Fact = FactFor(claim, latest.Zone),
            Day = latest.At.Day,
            Claim = claim,
        };
    }
}
