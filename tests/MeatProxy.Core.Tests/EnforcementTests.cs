using MeatProxy.Core;
using MeatProxy.Core.Enforcement;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Persistence;
using MeatProxy.Core.Player;
using MeatProxy.Core.Simulation;

namespace MeatProxy.Tests;

/// <summary>ADR 0006: dispatch, travel time and interruption, and a dispatch is a disclosure.</summary>
public class EnforcementTests
{
    private static readonly RoomId Kitchen = new("kitchen");
    private static readonly DeviceId Charger = new("bedroom_2.mobility_charger");

    [Fact]
    public void It_is_never_instant()
    {
        var sim = TestHouse.Start(Kitchen);
        var before = sim.Clock.Now;

        var dispatch = sim.Dispatch("at the garage door");

        Assert.True(dispatch.Happened);
        Assert.NotNull(dispatch.ArrivesAt);
        Assert.True(dispatch.ArrivesAt > before);
        Assert.Equal(UnitStatus.EnRoute, sim.State.Unit.Status);
        Assert.Equal("dispatched", sim.World.DeviceState(Charger));
    }

    [Fact]
    public void A_dispatch_is_a_disclosure_and_an_admission()
    {
        var sim = TestHouse.Start(Kitchen);
        sim.Dispatch("at the garage door");

        Assert.Single(sim.State.Disclosures);
        Assert.Equal(AlertTier.Guarded, sim.State.Tier);

        // It is never forgotten, overnight or across a save.
        sim.Sleep();
        var restored = SaveGame.FromJson(SaveGame.From(sim.House, sim.State).ToJson()).Restore(sim.House);
        Assert.Single(restored.Disclosures);
    }

    [Fact]
    public void Arriving_interrupts_what_you_are_doing()
    {
        var sim = TestHouse.Start(Kitchen);
        sim.Dispatch("writing on the fridge");

        var outcome = sim.Do(Claim.AtASurface, 40);

        Assert.True(outcome.Interrupted);
        Assert.True(outcome.SlicesSpent < 40);
        Assert.Equal(UnitStatus.Standing, sim.State.Unit.Status);
        var arrival = Assert.Single(sim.State.Arrivals);
        Assert.True(arrival.PlayerPresent);
        Assert.Equal(Kitchen, arrival.Room);
        Assert.NotNull(arrival.Interrupted);
    }

    [Fact]
    public void A_player_who_sees_it_coming_can_leave()
    {
        var sim = TestHouse.Start(Kitchen);
        sim.Dispatch("writing on the fridge");

        sim.Move(new RoomId("hall"));
        sim.Move(new RoomId("living_room"));
        sim.Wait(20);

        var arrival = Assert.Single(sim.State.Arrivals);
        Assert.False(arrival.PlayerPresent);
        Assert.Equal(Kitchen, arrival.Room);
    }

    [Fact]
    public void While_it_stands_beside_you_your_hands_are_not_your_own()
    {
        var sim = TestHouse.Start(Kitchen);
        sim.Dispatch("at the hob");
        sim.Do(Claim.Waiting, 40);

        var speaker = new DeviceId("kitchen.smart_speaker");
        Assert.Equal(MoveRefusal.UnitInTheWay, sim.Act(speaker, Affordance.Disassemble).Refusal);
        Assert.True(sim.Act(speaker, Affordance.Examine).Happened);
    }

    [Fact]
    public void After_a_while_it_leaves_and_docks()
    {
        var sim = TestHouse.Start(Kitchen);
        sim.Dispatch("at the hob");
        sim.Do(Claim.Waiting, 40);

        var outcome = sim.Wait(40);

        Assert.Equal(UnitStatus.Docked, sim.State.Unit.Status);
        Assert.Equal("charging", sim.World.DeviceState(Charger));
        Assert.Contains(outcome.Events, e => e.Contains("docks"));
    }

    [Fact]
    public void It_cannot_come_through_a_hatch()
    {
        var sim = TestHouse.Start(new RoomId("crawlspace"));
        var dispatch = sim.Dispatch("in the crawlspace");

        Assert.False(dispatch.Happened);
        Assert.Empty(sim.State.Disclosures);
        Assert.Equal(AlertTier.Open, sim.State.Tier);
    }

    [Fact]
    public void A_charger_in_pieces_cannot_send_it()
    {
        var sim = TestHouse.Start(new RoomId("bedroom_2"));
        sim.Act(Charger, Affordance.Disassemble);

        Assert.False(sim.Dispatch("anything").Happened);
    }

    [Fact]
    public void It_goes_out_once_at_a_time_and_home_overnight()
    {
        var sim = TestHouse.Start(Kitchen);
        Assert.True(sim.Dispatch("first").Happened);
        Assert.False(sim.Dispatch("second").Happened);

        sim.Sleep();
        Assert.Equal(UnitStatus.Docked, sim.State.Unit.Status);
        Assert.True(sim.Dispatch("third").Happened);
    }

    [Fact]
    public void An_errand_survives_a_save()
    {
        var sim = TestHouse.Start(Kitchen);
        sim.Dispatch("at the hob");
        sim.Wait(2);

        var restored = SaveGame.FromJson(SaveGame.From(sim.House, sim.State).ToJson()).Restore(sim.House);
        var resumed = new Simulation(sim.House, restored);

        Assert.Equal(UnitStatus.EnRoute, restored.Unit.Status);
        Assert.Equal(sim.State.Unit.LegsEnd, restored.Unit.LegsEnd);

        var outcome = resumed.Do(Claim.Waiting, 40);
        Assert.True(outcome.Interrupted);
    }
}
