using MeatProxy.Core.Perception;
using MeatProxy.Core.State;
using MeatProxy.Core;

namespace MeatProxy.Tests;

public class SalientBlockTests
{
    [Fact]
    public void Promoting_into_a_full_block_evicts_the_oldest()
    {
        var block = new SalientBlock { Cap = 3 };

        block.Promote(new SalientFact { Fact = "first", Day = 1 });
        block.Promote(new SalientFact { Fact = "second", Day = 1 });
        block.Promote(new SalientFact { Fact = "third", Day = 2 });

        var evicted = block.Promote(new SalientFact { Fact = "fourth", Day = 2 });

        Assert.Equal("first", evicted?.Fact);
        Assert.False(block.Holds("first"));
        Assert.Equal(3, block.Facts.Count);
    }

    [Fact]
    public void Re_promoting_a_fact_refreshes_it_rather_than_duplicating_it()
    {
        var block = new SalientBlock { Cap = 3 };

        block.Promote(new SalientFact { Fact = "first", Day = 1 });
        block.Promote(new SalientFact { Fact = "second", Day = 1 });
        block.Promote(new SalientFact { Fact = "first", Day = 3 });

        Assert.Equal(2, block.Facts.Count);
        Assert.Equal("first", block.Facts[^1].Fact);
        Assert.Equal(3, block.Facts[^1].Day);
    }

    [Fact]
    public void A_block_under_its_cap_evicts_nothing()
    {
        var block = new SalientBlock { Cap = 7 };
        Assert.Null(block.Promote(new SalientFact { Fact = "only", Day = 1 }));
    }

    [Fact]
    public void A_promoted_claim_is_what_makes_the_house_believe_something()
    {
        var block = new SalientBlock();

        Assert.False(block.Believes(Claim.Concealing));

        block.Promote(new SalientFact { Fact = "saw_you_put_it_away", Day = 4, Claim = Claim.Concealing });

        Assert.True(block.Believes(Claim.Concealing));
    }
}
