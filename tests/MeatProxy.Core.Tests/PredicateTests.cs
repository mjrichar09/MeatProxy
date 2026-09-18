using MeatProxy.Core.Perception;
using MeatProxy.Core.Predicates;
using MeatProxy.Core.State;
using MeatProxy.Core;

namespace MeatProxy.Tests;

public class PredicateTests
{
    [Theory]
    [InlineData("held(E3)")]
    [InlineData("put_to_house(precedent)")]
    [InlineData("world(boiler_faulted)")]
    [InlineData("house_believes(dismantling)")]
    [InlineData("run(assembled)")]
    public void A_predicate_round_trips_through_its_written_form(string text)
    {
        Assert.Equal(text, Predicate.Parse(text).ToString());
    }

    [Fact]
    public void A_claim_outside_the_closed_vocabulary_is_not_a_predicate()
    {
        Assert.Throws<FormatException>(() => Predicate.Parse("house_believes(plotting)"));
        Assert.Throws<FormatException>(() => Predicate.Parse("believes(dismantling)"));
    }

    [Fact]
    public void World_facts_are_computed_from_state_rather_than_flagged()
    {
        var sim = TestHouse.Start();

        Assert.False(sim.World.Holds("world(attic_reached)"));

        sim.State.RoomsVisited.Add(new RoomId("attic"));

        Assert.True(sim.World.Holds("world(attic_reached)"));
    }

    [Fact]
    public void An_aliased_fact_reads_the_device_it_is_actually_about()
    {
        var sim = TestHouse.Start();
        var boiler = new DeviceId("utility.boiler_controller");

        Assert.False(sim.World.Holds("world(boiler_faulted)"));

        sim.State.DeviceStates[boiler] = "faulted";

        Assert.True(sim.World.Holds("world(boiler_faulted)"));
    }

    [Fact]
    public void The_front_door_being_locked_is_a_predicate_like_any_other()
    {
        var sim = TestHouse.Start();
        Assert.True(sim.World.Holds("world(front_door_locked)"));

        sim.State.LockOverrides[new OpeningId("hall__exterior")] = false;

        Assert.False(sim.World.Holds("world(front_door_locked)"));
    }

    [Fact]
    public void Evidence_argument_and_world_predicates_are_monotonic()
    {
        var sim = TestHouse.Start();

        sim.State.ArtifactsHeld.Add("E1");
        sim.State.TopicsPutToHouse.Add("precedent");

        Assert.True(sim.World.Holds("held(E1)"));
        Assert.True(sim.World.Holds("put_to_house(precedent)"));
    }

    [Fact]
    public void House_belief_is_the_one_predicate_the_player_can_retract()
    {
        var sim = TestHouse.Start();
        sim.State.Salient.Cap = 2;

        sim.State.Salient.Promote(new SalientFact
        {
            Fact = "believes_dismantling",
            Day = 1,
            Claim = Claim.Dismantling,
        });

        Assert.True(sim.World.Holds("house_believes(dismantling)"));

        // Give it enough new things to find important and the old belief falls
        // out of its head. The house never lied; it simply has nothing there.
        sim.State.Salient.Promote(new SalientFact { Fact = "noise_in_the_kitchen", Day = 2 });
        sim.State.Salient.Promote(new SalientFact { Fact = "a_parcel_arrived", Day = 2 });

        Assert.False(sim.World.Holds("house_believes(dismantling)"));
    }

    [Fact]
    public void A_gate_reports_which_of_its_requirements_are_missing()
    {
        var sim = TestHouse.Start();
        var convince = new[]
        {
            "held(E1)", "held(E2)", "held(E3)", "held(E4)",
            "put_to_house(precedent)", "put_to_house(flat_region)",
        }.Select(Predicate.Parse).ToList();

        sim.State.ArtifactsHeld.UnionWith(["E1", "E2", "E3", "E4"]);

        Assert.False(sim.World.HoldsAll(convince));
        Assert.Equal(2, sim.World.Missing(convince).Count);

        sim.State.TopicsPutToHouse.UnionWith(["precedent", "flat_region"]);

        Assert.True(sim.World.HoldsAll(convince));
    }
}
