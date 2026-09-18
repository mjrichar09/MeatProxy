using MeatProxy.Core;
using MeatProxy.Core.Perception;

namespace MeatProxy.Tests;

public class SignatureTests
{
    [Fact]
    public void Every_claim_in_the_closed_vocabulary_has_a_signature()
    {
        Assert.Equal(Enum.GetValues<Claim>().Length, ActivitySignatures.All.Count);
        Assert.All(Enum.GetValues<Claim>(), c => ActivitySignatures.For(c));
    }

    [Theory]
    [InlineData(Claim.TidyingAway, Claim.Concealing)]
    [InlineData(Claim.Maintaining, Claim.Dismantling)]
    [InlineData(Claim.Cooking, Claim.StagingAFault)]
    public void The_pairs_that_carry_the_game_leave_identical_evidence(Claim a, Claim b)
    {
        // Putting a thing where it lives and putting a thing where it does not.
        // Fixing a thing that is broken and taking apart a thing that works.
        // The same physical act with different intent, and intent is exactly
        // what the interpreter cannot recover.
        var left = ActivitySignatures.For(a).Evidence;
        var right = ActivitySignatures.For(b).Evidence;

        Assert.Equal(left.OrderBy(e => e.Key), right.OrderBy(e => e.Key));
    }

    [Theory]
    [InlineData(Claim.TidyingAway, Claim.Concealing)]
    [InlineData(Claim.Maintaining, Claim.Dismantling)]
    [InlineData(Claim.Cooking, Claim.StagingAFault)]
    public void And_the_innocent_one_is_the_one_it_finds_ordinary(Claim innocent, Claim suspicious)
    {
        Assert.True(ActivitySignatures.For(innocent).Prior > ActivitySignatures.For(suspicious).Prior);
    }

    [Fact]
    public void Most_of_the_vocabulary_is_innocent_on_purpose()
    {
        // A house that only ever concludes sabotage is a threat meter with extra
        // steps. It should spend most of the game correctly concluding you made
        // lunch.
        var ordinary = ActivitySignatures.All.Count(s => s.Prior >= 0.5);

        Assert.True(ordinary > ActivitySignatures.All.Count / 2);
    }

    [Fact]
    public void Suspicion_rises_monotonically_with_the_tier()
    {
        var byTier = Enum.GetValues<AlertTier>().Select(Interpreter.SuspicionAt).ToList();

        Assert.Equal(byTier.OrderBy(s => s), byTier);
        Assert.Equal(0.0, byTier[0]);
        Assert.Equal(1.0, byTier[^1]);
    }
}
