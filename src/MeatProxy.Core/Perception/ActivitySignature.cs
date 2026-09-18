namespace MeatProxy.Core.Perception;

/// <summary>
/// What an activity leaves behind on the detection layer, and how readily the
/// house believes it.
/// </summary>
/// <remarks>
/// <para>
/// This table is where <c>claims.md</c>'s confusable pairs stop being a design
/// note and start being a mechanic. Two claims are confusable because they emit
/// the <em>same evidence</em>, not because a die is rolled: taking a thing apart
/// and fixing it move the same amount, draw the same power and make the same
/// noise. The interpreter cannot recover intent from that, and it is not bad at
/// its job — the job is impossible.
/// </para>
/// <para>
/// It also decides what losing a channel costs the house. A candidate is any
/// claim whose signature matches on the channels the zone can <em>currently</em>
/// sense, so blinding the audio in a room widens the set of things the house
/// could think you were doing. The player is not hidden. They are harder to read.
/// </para>
/// <para>
/// The prior is how ordinary the claim is. Most of the vocabulary is innocent on
/// purpose, and the interpreter spends most of the game correctly concluding
/// that you made lunch. These are working figures and belong to the tuning lane.
/// </para>
/// </remarks>
public sealed record ActivitySignature
{
    public required Claim Claim { get; init; }

    /// <summary>The strongest reading this activity produces on each channel.</summary>
    public required IReadOnlyDictionary<Channel, Magnitude> Evidence { get; init; }

    /// <summary>How unremarkable this claim is, from 0 to 1.</summary>
    public required double Prior { get; init; }

    /// <summary>This signature seen through only the channels a zone can sense.</summary>
    public IReadOnlyDictionary<Channel, Magnitude> AsSeenThrough(IReadOnlySet<Channel> live) =>
        Evidence.Where(e => live.Contains(e.Key)).ToDictionary(e => e.Key, e => e.Value);
}

/// <summary>The twenty signatures, one per claim in the closed vocabulary.</summary>
public static class ActivitySignatures
{
    private static readonly Dictionary<Claim, ActivitySignature> Table = Build();

    public static ActivitySignature For(Claim claim) => Table[claim];

    public static IReadOnlyCollection<ActivitySignature> All => Table.Values;

    private static Dictionary<Claim, ActivitySignature> Build()
    {
        var table = new Dictionary<Claim, ActivitySignature>();

        void Add(Claim claim, double prior, params (Channel Channel, Magnitude Magnitude)[] evidence) =>
            table[claim] = new ActivitySignature
            {
                Claim = claim,
                Prior = prior,
                Evidence = evidence.ToDictionary(e => e.Channel, e => e.Magnitude),
            };

        // Still, and hard to tell apart from being still for another reason.
        Add(Claim.Sleeping, 0.90, (Channel.Motion, Magnitude.Low));
        Add(Claim.Resting, 0.80, (Channel.Motion, Magnitude.Low));
        Add(Claim.Waiting, 0.35, (Channel.Motion, Magnitude.Low));

        Add(Claim.Eating, 0.90, (Channel.Motion, Magnitude.Low), (Channel.AudioLevel, Magnitude.Low));

        // At documents or at a screen. claims.md pairs these two.
        Add(Claim.ReadingPaperwork, 0.80, (Channel.Motion, Magnitude.Low), (Channel.PowerDraw, Magnitude.Low));
        Add(Claim.Working, 0.75, (Channel.Motion, Magnitude.Low), (Channel.PowerDraw, Magnitude.Low));

        // The three that matter most. Following a run of wire, fixing a thing
        // that is broken, and taking apart a thing that works are one signature.
        Add(Claim.TracingCircuit, 0.30,
            (Channel.Motion, Magnitude.Medium), (Channel.PowerDraw, Magnitude.Low), (Channel.AudioLevel, Magnitude.Low));
        Add(Claim.Maintaining, 0.70,
            (Channel.Motion, Magnitude.Medium), (Channel.PowerDraw, Magnitude.Low), (Channel.AudioLevel, Magnitude.Low));
        Add(Claim.Dismantling, 0.20,
            (Channel.Motion, Magnitude.Medium), (Channel.PowerDraw, Magnitude.Low), (Channel.AudioLevel, Magnitude.Low));

        // Putting a thing where it lives, and putting a thing where it does not.
        Add(Claim.TidyingAway, 0.80, (Channel.Motion, Magnitude.Medium));
        Add(Claim.Concealing, 0.20, (Channel.Motion, Magnitude.Medium));

        Add(Claim.HandlingDevice, 0.50, (Channel.Motion, Magnitude.Medium), (Channel.PowerDraw, Magnitude.Medium));
        Add(Claim.AtASurface, 0.70, (Channel.Motion, Magnitude.Medium), (Channel.AudioLevel, Magnitude.Low));
        Add(Claim.TestingABoundary, 0.30, (Channel.Motion, Magnitude.Medium), (Channel.DoorState, Magnitude.Medium));

        Add(Claim.Cleaning, 0.85, (Channel.Motion, Magnitude.High), (Channel.AudioLevel, Magnitude.Medium));
        Add(Claim.Searching, 0.40, (Channel.Motion, Magnitude.High), (Channel.AudioLevel, Magnitude.Medium));

        Add(Claim.Exercising, 0.85, (Channel.Motion, Magnitude.High), (Channel.Thermal, Magnitude.Medium));
        Add(Claim.Pacing, 0.45, (Channel.Motion, Magnitude.High), (Channel.Thermal, Magnitude.Medium));

        // Food, heat, the hob — and breaking something on purpose behind it.
        Add(Claim.Cooking, 0.90,
            (Channel.Motion, Magnitude.Medium), (Channel.Thermal, Magnitude.High), (Channel.PowerDraw, Magnitude.Medium));
        Add(Claim.StagingAFault, 0.15,
            (Channel.Motion, Magnitude.Medium), (Channel.Thermal, Magnitude.High), (Channel.PowerDraw, Magnitude.Medium));

        return table;
    }
}
