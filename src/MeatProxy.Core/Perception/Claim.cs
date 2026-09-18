namespace MeatProxy.Core.Perception;

/// <summary>
/// The claim vocabulary (<c>claims.md</c> §1). Twenty, closed. The interpreter
/// returns one of these or nothing.
/// </summary>
/// <remarks>
/// Most of them are innocent, deliberately: a house that only ever concludes
/// sabotage is a threat meter with extra steps. The vocabulary is built in pairs
/// that are plausible misreads of each other, which is what makes diverting
/// attention a real verb — the player is not hiding, they are being misread.
/// A wrong claim is content, not a bug.
/// </remarks>
public enum Claim
{
    Sleeping,
    Resting,
    Waiting,
    Cooking,
    Eating,
    Cleaning,
    TidyingAway,
    Concealing,
    Searching,
    TracingCircuit,
    Exercising,
    Pacing,
    Maintaining,
    Dismantling,
    ReadingPaperwork,
    Working,
    AtASurface,
    HandlingDevice,
    TestingABoundary,
    StagingAFault,
}

/// <summary>
/// The confusable pairs of <c>claims.md</c> §1. The bolded ones there are the
/// game: the same physical act performed with different intent, and intent is
/// exactly what the interpreter cannot recover from motion and power draw.
/// </summary>
public static class Claims
{
    private static readonly Dictionary<Claim, Claim[]> Misreads = new()
    {
        [Claim.Sleeping] = [Claim.Resting],
        [Claim.Resting] = [Claim.Waiting],
        [Claim.Waiting] = [Claim.Resting],
        [Claim.Cooking] = [Claim.StagingAFault],
        [Claim.Eating] = [Claim.Cooking],
        [Claim.Cleaning] = [Claim.Searching],
        [Claim.TidyingAway] = [Claim.Concealing],
        [Claim.Concealing] = [Claim.TidyingAway],
        [Claim.Searching] = [Claim.Cleaning],
        [Claim.TracingCircuit] = [Claim.Maintaining],
        [Claim.Exercising] = [Claim.Pacing],
        [Claim.Pacing] = [Claim.Exercising],
        [Claim.Maintaining] = [Claim.Dismantling, Claim.TracingCircuit],
        [Claim.Dismantling] = [Claim.Maintaining],
        [Claim.ReadingPaperwork] = [Claim.Working],
        [Claim.Working] = [Claim.ReadingPaperwork],
        [Claim.AtASurface] = [Claim.Cleaning],
        [Claim.HandlingDevice] = [Claim.Maintaining],
        [Claim.TestingABoundary] = [Claim.Maintaining],
        [Claim.StagingAFault] = [Claim.Cooking, Claim.Maintaining],
    };

    /// <summary>What this claim is a plausible misread of, per <c>claims.md</c> §1.</summary>
    public static IReadOnlyList<Claim> PlausibleMisreads(Claim claim) =>
        Misreads.TryGetValue(claim, out var misreads) ? misreads : [];
}
