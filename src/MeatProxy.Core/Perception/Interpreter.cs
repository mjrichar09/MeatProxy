using MeatProxy.Core.Time;

namespace MeatProxy.Core.Perception;

/// <summary>
/// Layer two. Scarce, pointed, and capable of being wrong.
/// </summary>
/// <remarks>
/// <para>
/// Deterministic engine code, and it stays that way. Lane A never replaces this
/// with a model: the house concluding something about you is a fact the game
/// gates on, and standing rule 1 puts facts on this side of the line. What Lane A
/// gets is the voice that talks about the conclusion, not the conclusion.
/// </para>
/// <para>
/// It works by elimination. It takes the readings a zone actually produced, finds
/// every claim whose signature matches on the channels that zone can currently
/// sense, and picks the one it finds most ordinary. So the default misread runs
/// toward innocence — the pry bar you hid reads as tidying up — and it takes a
/// house that has already been pushed up the tier ladder to land on the
/// suspicious twin. The same escalation that arms it is what makes it read you
/// badly, which is the trade <c>alert-tiers.md</c> is built on.
/// </para>
/// </remarks>
public sealed class Interpreter
{
    /// <summary>
    /// How far the house's reading is bent toward the suspicious member of a
    /// confusable set, by tier. At <see cref="AlertTier.Open"/> it reads you the
    /// ordinary way; at <see cref="AlertTier.Dropped"/> it reads you the other way
    /// entirely.
    /// </summary>
    public static double SuspicionAt(AlertTier tier) => tier switch
    {
        AlertTier.Open => 0.0,
        AlertTier.Guarded => 0.2,
        AlertTier.Monitored => 0.4,
        AlertTier.ReadOnly => 0.6,
        AlertTier.Silent => 0.8,
        AlertTier.Dropped => 1.0,
        _ => 0.0,
    };

    /// <summary>
    /// Read a zone. Returns null when there is nothing to conclude, which is the
    /// common case and a real answer: an unfocused zone produces detections only.
    /// </summary>
    public Understanding? Interpret(
        WorldView world,
        ZoneId zone,
        IReadOnlyList<Detection> readings,
        TimePoint at)
    {
        if (readings.Count == 0)
        {
            return null;
        }

        // The strongest reading on each channel is what the zone reports.
        var observed = new Dictionary<Channel, Magnitude>();
        foreach (var reading in readings.Where(r => r.Zone == zone))
        {
            if (!observed.TryGetValue(reading.Channel, out var already) || reading.Magnitude > already)
            {
                observed[reading.Channel] = reading.Magnitude;
            }
        }

        if (observed.Count == 0)
        {
            return null;
        }

        var live = world.LiveChannels(zone);
        var candidates = ActivitySignatures.All
            .Where(s => Matches(s.AsSeenThrough(live), observed))
            .ToList();

        if (candidates.Count == 0)
        {
            // Readings that fit no authored activity. The house saw something and
            // made nothing of it, which is the texture ADR 0014 is after.
            return new Understanding
            {
                Zone = zone,
                Subject = "player",
                Claim = null,
                Confidence = 0,
                At = at,
            };
        }

        var suspicion = SuspicionAt(world.State.Tier);
        var weights = candidates.ToDictionary(c => c.Claim, c => Weigh(c.Prior, suspicion));
        var total = weights.Values.Sum();
        var chosen = weights.OrderByDescending(w => w.Value).ThenBy(w => w.Key).First();

        return new Understanding
        {
            Zone = zone,
            Subject = "player",
            Claim = chosen.Key,
            Confidence = total > 0 ? chosen.Value / total : 0,
            At = at,
        };
    }

    /// <summary>
    /// A claim is a candidate when the evidence it would have left, seen through
    /// the channels this zone still has, is exactly what the zone reported.
    /// </summary>
    private static bool Matches(
        IReadOnlyDictionary<Channel, Magnitude> expected,
        IReadOnlyDictionary<Channel, Magnitude> observed)
    {
        if (expected.Count != observed.Count)
        {
            return false;
        }

        return expected.All(e => observed.TryGetValue(e.Key, out var seen) && seen == e.Value);
    }

    /// <summary>
    /// At no suspicion this is the prior. At full suspicion it is its inverse, so
    /// the reading a calm house finds obvious is the one an armed house discounts.
    /// </summary>
    private static double Weigh(double prior, double suspicion) =>
        (prior * (1 - suspicion)) + ((1 - prior) * suspicion);
}
