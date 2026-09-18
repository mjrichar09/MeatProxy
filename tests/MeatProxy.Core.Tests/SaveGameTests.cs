using MeatProxy.Core.Effects;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Persistence;
using MeatProxy.Core.Simulation;
using MeatProxy.Core.State;
using MeatProxy.Core;

namespace MeatProxy.Tests;

public class SaveGameTests
{
    [Fact]
    public void A_run_survives_being_written_out_and_read_back()
    {
        var sim = TestHouse.Start(new RoomId("hall"));

        sim.Move(new RoomId("kitchen"));
        sim.Move(new RoomId("pantry"));
        sim.Wait(7, "reading the labels");
        sim.Adjudicate(new Adjudication
        {
            Effects = [new ChangeDeviceState(new DeviceId("garage.smart_lock"), "maintenance")],
        });
        sim.State.ArtifactsHeld.Add("E1");
        sim.State.Salient.Promote(new SalientFact
        {
            Fact = "you_were_in_the_pantry",
            Day = 1,
            Claim = Claim.Searching,
        });

        var json = SaveGame.From(sim.House, sim.State).ToJson();
        var restored = SaveGame.FromJson(json).Restore(sim.House);
        var after = new Simulation(sim.House, restored);

        Assert.Equal(sim.State.PlayerRoom, restored.PlayerRoom);
        Assert.Equal(sim.Clock.Day, restored.Clock.Day);
        Assert.Equal(sim.Clock.SlicesSpent, restored.Clock.SlicesSpent);
        Assert.True(sim.State.RoomsVisited.SetEquals(restored.RoomsVisited));
        Assert.Equal(sim.State.Detections.Count, restored.Detections.Count);

        Assert.True(after.World.Holds("held(E1)"));
        Assert.True(after.World.Holds("house_believes(searching)"));
        Assert.True(after.World.Holds("world(garage_lock_in_maintenance)"));
        Assert.True(after.World.Holds("world(front_door_locked)"));
    }

    [Fact]
    public void A_save_carries_the_run_and_not_the_house()
    {
        var sim = TestHouse.Start();
        var json = SaveGame.From(sim.House, sim.State).ToJson();

        // No room dimensions, no device lists, no capability names. Content fixes
        // must not invalidate a save.
        Assert.DoesNotContain("\"dims\"", json);
        Assert.DoesNotContain("house_capabilities", json);
        Assert.DoesNotContain("\"levels\"", json);
    }

    [Fact]
    public void A_save_will_not_restore_against_a_different_house()
    {
        var sim = TestHouse.Start();
        var save = SaveGame.From(sim.House, sim.State) with { HouseName = "somewhere else" };

        Assert.Throws<InvalidOperationException>(() => save.Restore(sim.House));
    }

    [Fact]
    public void Identifiers_are_written_as_plain_strings()
    {
        var sim = TestHouse.Start(new RoomId("hall"));
        var json = SaveGame.From(sim.House, sim.State).ToJson();

        Assert.Contains("\"player_room\": \"hall\"", json);
    }
}
