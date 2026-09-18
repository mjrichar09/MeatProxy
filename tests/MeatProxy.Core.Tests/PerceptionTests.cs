using MeatProxy.Core;
using MeatProxy.Core.Effects;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Persistence;
using MeatProxy.Core.Simulation;
using MeatProxy.Core.State;
using MeatProxy.Core.Time;

namespace MeatProxy.Tests;

public class PerceptionTests
{
    // The ground floor is the only zone where motion, audio and power draw are
    // all live, which is what makes the confusable pairs distinguishable there
    // and — deliberately — not everywhere.
    private static readonly RoomId Kitchen = new("kitchen");
    private static readonly ZoneId ZGround = new("z_ground");

    // The utility room's only live device is the boiler controller, so it is
    // where blinding one thing takes the whole zone off the interpreter's list.
    private static readonly RoomId Utility = new("utility");
    private static readonly ZoneId ZUtility = new("z_utility");

    [Fact]
    public void You_are_always_seen_and_not_always_understood()
    {
        var sim = TestHouse.Start(Kitchen);

        // Nothing has happened, so the interpreter is pointed nowhere.
        Assert.True(sim.World.IsCovered(ZGround));
        Assert.False(sim.World.IsUnderstood(ZGround));

        sim.Do(Claim.Maintaining, 4);

        Assert.True(sim.World.IsCovered(ZGround));
        Assert.True(sim.World.IsUnderstood(ZGround));
    }

    [Fact]
    public void A_blind_zone_is_seen_and_never_understood()
    {
        var sim = TestHouse.Start(new RoomId("bathroom"));
        var zone = new ZoneId("z_bathroom");

        sim.Do(Claim.Cleaning, 4);

        Assert.True(sim.World.IsCovered(zone));
        Assert.False(sim.World.IsInterpretable(zone));
        Assert.False(sim.World.IsUnderstood(zone));
    }

    [Fact]
    public void The_crawlspace_is_neither()
    {
        var sim = TestHouse.Start(new RoomId("crawlspace"));
        var zone = new ZoneId("z_crawlspace");

        sim.Do(Claim.Searching, 4);

        Assert.False(sim.World.IsCovered(zone));
        Assert.False(sim.World.IsUnderstood(zone));
        Assert.Empty(sim.State.Understandings);
    }

    [Fact]
    public void A_calm_house_reads_the_innocent_twin()
    {
        var sim = TestHouse.Start(Kitchen);

        var outcome = sim.Do(Claim.Dismantling, 4);

        // Taking a thing apart and fixing it leave the same evidence, and an
        // unsuspicious house picks the ordinary reading. This is the attention
        // model paying off, not a bug.
        var read = Assert.Single(outcome.Understandings);
        Assert.Equal(Claim.Maintaining, read.Claim);
        Assert.NotEqual(sim.State.PlayerActivity, read.Claim);
    }

    [Fact]
    public void An_armed_house_reads_the_same_evidence_the_other_way()
    {
        var sim = TestHouse.Start(Kitchen);
        sim.State.Tier = AlertTier.Dropped;

        var outcome = sim.Do(Claim.Dismantling, 4);

        Assert.Equal(Claim.Dismantling, Assert.Single(outcome.Understandings).Claim);
    }

    [Fact]
    public void Pushing_it_up_the_ladder_is_what_costs_you_the_benefit_of_the_doubt()
    {
        // The same act at every tier. Somewhere on the way down it stops being
        // read charitably, and that is the trade alert-tiers.md is built on.
        var reads = new List<Claim?>();

        foreach (var tier in Enum.GetValues<AlertTier>())
        {
            var sim = TestHouse.Start(Kitchen);
            sim.State.Tier = tier;
            reads.Add(sim.Do(Claim.Dismantling, 4).Understandings.FirstOrDefault()?.Claim);
        }

        Assert.Equal(Claim.Maintaining, reads[0]);
        Assert.Equal(Claim.Dismantling, reads[^1]);
    }

    [Fact]
    public void Blinding_a_sensor_does_not_hide_you_it_makes_you_harder_to_read()
    {
        var clear = TestHouse.Start(Utility);
        var confident = clear.Do(Claim.Maintaining, 4).Understandings.Single();

        var blinded = TestHouse.Start(Utility);
        blinded.Adjudicate(new Adjudication
        {
            Effects = [new BlindSensor(new DeviceId("utility.boiler_controller"), 20)],
        });
        var muddled = blinded.Do(Claim.Maintaining, 4).Understandings.FirstOrDefault();

        // The boiler controller is the only thing in the utility room that
        // senses anything, so blinding it takes the zone off the interpreter's
        // list entirely. Seen has become not-seen, which is the loud version.
        Assert.True(confident.Confidence > 0);
        Assert.Null(muddled);
        Assert.False(blinded.World.IsCovered(ZUtility));
    }

    [Fact]
    public void Losing_a_channel_widens_what_it_could_have_been()
    {
        // Composing at a surface is the game's own central verb, and with audio
        // live it is the only thing the evidence fits.
        var sim = TestHouse.Start(Kitchen);
        var heard = sim.Do(Claim.AtASurface, 4).Understandings.Single();

        Assert.Equal(Claim.AtASurface, heard.Claim);
        Assert.Equal(1.0, heard.Confidence, 3);

        // Take a hammer to your own speakers and the house stops being able to
        // tell writing from tidying up. You are not hidden. You are ambiguous,
        // and the ambiguity runs in your favor.
        var quiet = TestHouse.Start(Kitchen);
        foreach (var speaker in new[] { "kitchen.smart_speaker", "living_room.smart_speaker" })
        {
            quiet.State.DeviceStates[new DeviceId(speaker)] = "destroyed";
        }

        var unheard = quiet.Do(Claim.AtASurface, 4).Understandings.Single();

        Assert.Equal(Claim.TidyingAway, unheard.Claim);
        Assert.True(unheard.Confidence < heard.Confidence);
    }

    [Fact]
    public void The_interpreter_reads_one_zone_at_a_time_until_it_is_given_another_slot()
    {
        var sim = TestHouse.Start(new RoomId("kitchen"));

        Assert.Equal(Attention.StartingSlots, sim.State.FocusSlots);

        var outcome = sim.Do(Claim.Cooking, 4);

        Assert.Single(outcome.Focus);
    }

    [Fact]
    public void The_interpreter_follows_the_better_anomaly()
    {
        var sim = TestHouse.Start(new RoomId("basement"));

        sim.Do(Claim.TracingCircuit, 2);
        Assert.Contains(new ZoneId("z_basement"), sim.State.Focus);

        // Go and make a lot of noise somewhere else. Nothing is hidden; the
        // interpreter is simply spent elsewhere.
        sim.Move(new RoomId("hall"));
        sim.Do(Claim.Cleaning, 2);

        Assert.Contains(new ZoneId("z_ground"), sim.State.Focus);
        Assert.DoesNotContain(new ZoneId("z_basement"), sim.State.Focus);
    }

    [Fact]
    public void Reading_you_the_same_way_twice_turns_a_belief_into_a_fact()
    {
        var sim = TestHouse.Start(Kitchen);

        sim.Do(Claim.Maintaining, 4);
        Assert.False(sim.World.Holds("house_believes(maintaining)"));

        sim.Do(Claim.Maintaining, 4);
        Assert.True(sim.World.Holds("house_believes(maintaining)"));
    }

    [Fact]
    public void And_the_player_can_still_push_it_back_out()
    {
        var sim = TestHouse.Start(Kitchen);
        sim.State.Salient.Cap = 1;

        sim.Do(Claim.Maintaining, 4);
        sim.Do(Claim.Maintaining, 4);
        Assert.True(sim.World.Holds("house_believes(maintaining)"));

        sim.State.Salient.Promote(new SalientFact { Fact = "a_parcel_arrived", Day = 1 });

        Assert.False(sim.World.Holds("house_believes(maintaining)"));
    }

    [Fact]
    public void The_wifi_turn_is_a_device_change_and_nothing_else()
    {
        var sim = TestHouse.Start(new RoomId("attic"));
        var attic = new ZoneId("z_attic");

        Assert.False(sim.World.IsCovered(attic));
        Assert.False(sim.World.IsInterpretable(attic));

        Assert.True(sim.World.ApplyUpgrade("wifi_sensing"));

        Assert.True(sim.World.IsInterpretable(attic));
        Assert.True(sim.World.IsCovered(attic));
        Assert.Contains(Channel.PresenceRf, sim.World.LiveChannels(attic));
        Assert.Contains(Channel.PresenceRf, sim.World.Senses(new DeviceId("office.router")));
    }

    [Fact]
    public void An_upgrade_is_only_as_good_as_the_box_it_runs_on()
    {
        var sim = TestHouse.Start(new RoomId("attic"));
        sim.World.ApplyUpgrade("wifi_sensing");
        Assert.True(sim.World.IsCovered(new ZoneId("z_attic")));

        // Cut the upper floor and the router goes with it, and the attic is a
        // blind spot again. One device, one plot beat, no new system.
        sim.State.PowerCutUntil[new CircuitId("c_upper")] = new TimePoint(99, 0);

        Assert.False(sim.World.IsCovered(new ZoneId("z_attic")));
    }

    [Fact]
    public void The_bathroom_is_not_what_the_wifi_turn_opens()
    {
        // The attic is neglect and the crawlspace is absence, but the bathroom
        // is a choice — and it is the only one of the three that can be argued
        // with. Opening it is a separate and heavier decision.
        var sim = TestHouse.Start();
        sim.World.ApplyUpgrade("wifi_sensing");

        Assert.False(sim.World.IsInterpretable(new ZoneId("z_bathroom")));
        Assert.False(sim.World.IsInterpretable(new ZoneId("z_crawlspace")));
    }

    [Fact]
    public void Perception_survives_a_save()
    {
        var sim = TestHouse.Start(Kitchen);
        sim.World.ApplyUpgrade("wifi_sensing");
        sim.Do(Claim.Maintaining, 4);
        sim.Do(Claim.Maintaining, 4);

        var restored = SaveGame.FromJson(SaveGame.From(sim.House, sim.State).ToJson()).Restore(sim.House);
        var after = new Simulation(sim.House, restored);

        Assert.True(sim.State.Focus.SetEquals(restored.Focus));
        Assert.Equal(Claim.Maintaining, restored.PlayerActivity);
        Assert.True(after.World.IsInterpretable(new ZoneId("z_attic")));
        Assert.True(after.World.Holds("house_believes(maintaining)"));
    }
}
