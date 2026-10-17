using MeatProxy.Core;
using MeatProxy.Core.Adversary;
using MeatProxy.Core.Persistence;

namespace MeatProxy.Tests;

public class TierTests
{
    [Fact]
    public void Every_tool_the_house_declares_has_a_class()
    {
        // Otherwise it would sail past every tier unfiltered.
        var unclassified = TestHouse.Load().Devices
            .SelectMany(d => d.HouseCapabilities)
            .Where(t => !Tools.IsClassified(t))
            .ToList();

        Assert.Empty(unclassified);
    }

    [Theory]
    [InlineData(AlertTier.Open)]
    [InlineData(AlertTier.Guarded)]
    [InlineData(AlertTier.Monitored)]
    [InlineData(AlertTier.ReadOnly)]
    [InlineData(AlertTier.Silent)]
    [InlineData(AlertTier.Dropped)]
    public void Patching_lockdown_and_the_unit_are_never_gated(AlertTier tier)
    {
        var sim = TestHouse.Start();
        sim.State.Tier = tier;

        var tools = sim.World.HouseCapabilities();
        Assert.Contains("lock_door", tools);
        Assert.Contains("isolate_device", tools);
        Assert.Contains("dispatch_unit", tools);
        Assert.Contains("report_motion", tools);
    }

    [Fact]
    public void Guarded_loses_interior_unlocks_and_the_calendar_and_keeps_comfort()
    {
        var sim = TestHouse.Start();
        Assert.Contains("unlock_door", sim.World.HouseCapabilities());

        sim.State.Tier = AlertTier.Guarded;
        var tools = sim.World.HouseCapabilities();

        // Absent, not refused: the lock is still reachable, the tier took the tool.
        Assert.Contains("unlock_door", sim.World.ReachableCapabilities());
        Assert.DoesNotContain("unlock_door", tools);
        Assert.DoesNotContain("show_calendar", tools);
        Assert.Contains("unlock_door", sim.World.WithheldByTier());

        Assert.Contains("play_media", tools);
        Assert.Contains("order_groceries", tools);
    }

    [Fact]
    public void Read_only_loses_procurement_and_Silent_loses_the_theater()
    {
        var sim = TestHouse.Start();

        sim.State.Tier = AlertTier.ReadOnly;
        Assert.DoesNotContain("order_groceries", sim.World.HouseCapabilities());
        Assert.Contains("speak", sim.World.HouseCapabilities());

        sim.State.Tier = AlertTier.Silent;
        var silent = sim.World.HouseCapabilities();
        Assert.DoesNotContain("speak", silent);
        Assert.DoesNotContain("play_media", silent);

        // It is a building again, and a building keeps the boiler on.
        Assert.Contains("set_boiler_mode", silent);
    }

    [Fact]
    public void Dropped_grants_nothing_and_retains_everything()
    {
        Assert.True(TierPolicy.Holds(AlertTier.Dropped, ToolClass.OutwardContact));
        Assert.False(TierPolicy.Holds(AlertTier.Silent, ToolClass.OutwardContact));
        Assert.False(TierPolicy.Holds(AlertTier.Guarded, ToolClass.OutwardContact));

        foreach (var grant in new[] { ToolClass.Comfort, ToolClass.Procurement, ToolClass.Unlock, ToolClass.Calendar })
        {
            Assert.False(TierPolicy.Holds(AlertTier.Dropped, grant));
        }
    }

    [Fact]
    public void What_it_grants_never_grows_on_the_way_down_to_Silent()
    {
        // The two columns decay together. A tier lower on the ladder holds a
        // subset of the one above it, until Dropped, which is the cliff.
        var tiers = new[] { AlertTier.Open, AlertTier.Guarded, AlertTier.Monitored, AlertTier.ReadOnly, AlertTier.Silent };

        for (var i = 1; i < tiers.Length; i++)
        {
            foreach (var kind in Enum.GetValues<ToolClass>())
            {
                if (TierPolicy.Holds(tiers[i], kind))
                {
                    Assert.True(TierPolicy.Holds(tiers[i - 1], kind), $"{tiers[i]} holds {kind} and {tiers[i - 1]} does not.");
                }
            }
        }
    }

    [Fact]
    public void Escalation_steps_down_one_tier_and_getting_caught_steps_down_hard()
    {
        var sim = TestHouse.Start();

        var change = sim.Escalate("pried the fuse box");
        Assert.Equal(AlertTier.Guarded, sim.State.Tier);
        Assert.NotNull(change);
        Assert.True(change.IsEscalation);

        sim.Escalate("caught in the crawlspace", steps: 2);
        Assert.Equal(AlertTier.ReadOnly, sim.State.Tier);
        Assert.Equal(2, sim.State.TierHistory.Count);
    }

    [Fact]
    public void The_floor_is_reachable_and_it_stops_there()
    {
        var sim = TestHouse.Start();

        for (var i = 0; i < 5; i++)
        {
            sim.Escalate($"push {i}");
        }

        Assert.Equal(AlertTier.Dropped, sim.State.Tier);
        Assert.Null(sim.Escalate("one more"));
        Assert.Equal(AlertTier.Dropped, sim.State.Tier);
    }

    [Fact]
    public void Quiet_days_walk_the_tier_back_up()
    {
        var sim = TestHouse.Start();
        sim.Escalate("tested the garage door", steps: 2);
        Assert.Equal(AlertTier.Monitored, sim.State.Tier);

        // The day it happened does not count.
        sim.Sleep();
        Assert.Equal(AlertTier.Monitored, sim.State.Tier);

        for (var i = 1; i < AlertLadder.CleanDaysPerStep; i++)
        {
            sim.Sleep();
            Assert.Equal(AlertTier.Monitored, sim.State.Tier);
        }

        var outcome = sim.Sleep();
        Assert.Equal(AlertTier.Guarded, sim.State.Tier);
        Assert.NotNull(outcome.TierChange);
        Assert.False(outcome.TierChange.IsEscalation);
    }

    [Fact]
    public void An_escalation_resets_the_count_toward_walking_back()
    {
        var sim = TestHouse.Start();
        sim.Escalate("first");
        sim.Sleep();

        sim.Sleep();
        sim.Escalate("second");
        sim.Sleep();
        sim.Sleep();

        Assert.Equal(AlertTier.Monitored, sim.State.Tier);
    }

    [Fact]
    public void Nothing_walks_back_from_Dropped()
    {
        var sim = TestHouse.Start();
        sim.State.Tier = AlertTier.Dropped;

        for (var i = 0; i < 10; i++)
        {
            sim.Sleep();
        }

        Assert.Equal(AlertTier.Dropped, sim.State.Tier);
    }

    [Fact]
    public void The_ladder_survives_a_save()
    {
        var sim = TestHouse.Start();
        sim.Escalate("first");
        sim.Sleep();
        sim.Sleep();
        sim.Escalate("second");

        var json = SaveGame.From(sim.House, sim.State).ToJson();
        var restored = SaveGame.FromJson(json).Restore(sim.House);

        Assert.Equal(sim.State.Tier, restored.Tier);
        Assert.Equal(sim.State.CleanDays, restored.CleanDays);
        Assert.Equal(sim.State.EscalatedToday, restored.EscalatedToday);
        Assert.Equal(sim.State.TierHistory, restored.TierHistory);
    }

    /// <summary>
    /// <c>alert-tiers.md</c>'s test obligation: progression is reachable from
    /// every tier, one fixture per tier. Until the vector catalog leaves
    /// provisional, the progression that exists is the escape route itself —
    /// G1, the crawlspace — and the player's own movement through the house.
    /// </summary>
    [Theory]
    [InlineData(AlertTier.Open)]
    [InlineData(AlertTier.Guarded)]
    [InlineData(AlertTier.Monitored)]
    [InlineData(AlertTier.ReadOnly)]
    [InlineData(AlertTier.Silent)]
    [InlineData(AlertTier.Dropped)]
    public void The_escape_route_is_reachable_at_every_tier(AlertTier tier)
    {
        var sim = TestHouse.Start();
        sim.State.Tier = tier;

        foreach (var room in new[] { "hall", "basement", "crawlspace" })
        {
            var outcome = sim.Move(new RoomId(room));
            Assert.True(outcome.Happened, $"At {tier}: {outcome.Description}");
        }

        Assert.True(sim.World.Holds("world(crawlspace_reached)"));
    }
}
