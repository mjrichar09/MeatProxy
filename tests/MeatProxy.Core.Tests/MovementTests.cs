using MeatProxy.Core.Perception;
using MeatProxy.Core.Simulation;
using MeatProxy.Core;

namespace MeatProxy.Tests;

public class MovementTests
{
    [Fact]
    public void The_front_door_does_not_open_from_the_inside()
    {
        var sim = TestHouse.Start(new RoomId("hall"));
        var outcome = sim.Move(RoomId.Exterior);

        Assert.False(outcome.Happened);
        Assert.Equal(MoveRefusal.Locked, outcome.Refusal);
        Assert.Equal(new RoomId("hall"), sim.State.PlayerRoom);
    }

    [Fact]
    public void Every_way_out_of_the_house_is_shut()
    {
        var sim = TestHouse.Start();
        var exits = sim.House.Rooms
            .SelectMany(r => sim.House.OpeningsFrom(r.Id))
            .Where(o => o.LeavesTheHouse)
            .ToList();

        Assert.NotEmpty(exits);
        Assert.All(exits, o => Assert.True(sim.World.IsLocked(o) || !o.Openable));
    }

    [Fact]
    public void Moving_costs_slices_and_stairs_cost_more_than_doors()
    {
        var sim = TestHouse.Start(new RoomId("hall"));

        var throughADoor = sim.Move(new RoomId("bathroom"));
        var backAgain = sim.Move(new RoomId("hall"));
        var upTheStairs = sim.Move(new RoomId("bedroom"));

        Assert.True(throughADoor.Happened);
        Assert.True(backAgain.Happened);
        Assert.True(upTheStairs.Happened);
        Assert.True(upTheStairs.SlicesSpent > throughADoor.SlicesSpent);
    }

    [Fact]
    public void Walking_into_a_covered_zone_is_detected_and_never_attributed()
    {
        var sim = TestHouse.Start(new RoomId("utility"));
        var outcome = sim.Move(new RoomId("basement"));

        var detection = Assert.Single(outcome.Detections, d => d.Zone == new ZoneId("z_basement"));
        Assert.Equal(Channel.Motion, detection.Channel);

        // The utility room has a boiler controller and a dumb fuse box. Neither
        // sees motion, so leaving it is not seen at all.
        Assert.DoesNotContain(outcome.Detections, d => d.Zone == new ZoneId("z_utility"));

        // No subject and no verb. That it was the player is a conclusion, and
        // conclusions come from layer two.
        Assert.Equal(sim.Clock.Now.Day, detection.At.Day);
    }

    [Fact]
    public void The_one_room_that_is_a_genuine_escape_route_is_the_one_room_you_cannot_speak_in()
    {
        var sim = TestHouse.Start();

        Assert.False(sim.World.CanSpeakToHouseFrom(new RoomId("crawlspace")));
        Assert.True(sim.World.CanSpeakToHouseFrom(new RoomId("kitchen")));
    }

    [Fact]
    public void Killing_the_speaker_deletes_the_players_own_ability_to_talk_there()
    {
        var sim = TestHouse.Start(new RoomId("kitchen"));
        Assert.True(sim.World.CanSpeakToHouseFrom(new RoomId("kitchen")));

        sim.State.DeviceStates[new DeviceId("kitchen.smart_speaker")] = "destroyed";

        Assert.False(sim.World.CanSpeakToHouseFrom(new RoomId("kitchen")));
    }
}
