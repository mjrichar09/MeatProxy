using MeatProxy.Core.Devices;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Time;
using MeatProxy.Core;

namespace MeatProxy.Tests;

public class HouseTests
{
    [Fact]
    public void The_authored_house_holds_together()
    {
        var problems = TestHouse.Load().Validate(TestHouse.File.StartRoom);
        Assert.Empty(problems);
    }

    [Fact]
    public void Every_room_is_reachable_from_where_the_player_wakes_up()
    {
        var sim = TestHouse.Start();
        var seen = new HashSet<RoomId> { sim.State.PlayerRoom };
        var queue = new Queue<RoomId>([sim.State.PlayerRoom]);

        while (queue.Count > 0)
        {
            foreach (var opening in sim.House.OpeningsFrom(queue.Dequeue()))
            {
                if (!opening.To.IsExterior && seen.Add(opening.To))
                {
                    queue.Enqueue(opening.To);
                }
            }
        }

        Assert.Equal(sim.House.Rooms.Count, seen.Count);
    }

    [Fact]
    public void Openings_are_traversable_from_both_sides()
    {
        var house = TestHouse.Load();
        var fromUtility = house.OpeningsFrom(new RoomId("utility"));
        var fromBasement = house.OpeningsFrom(new RoomId("basement"));

        // Authored once, on utility, per world.md §3's worked example.
        Assert.Contains(fromUtility, o => o.To == new RoomId("basement"));
        Assert.Contains(fromBasement, o => o.To == new RoomId("utility"));
    }

    [Fact]
    public void The_crawlspace_has_nothing_smart_in_it()
    {
        var house = TestHouse.Load();
        Assert.Empty(house.DevicesIn(new RoomId("crawlspace")));
    }

    [Fact]
    public void Three_zones_are_blind_for_three_different_reasons()
    {
        var blind = TestHouse.Load().Zones.Where(z => !z.Interpretable).ToList();

        Assert.Equal(3, blind.Count);
        Assert.All(blind, z => Assert.False(string.IsNullOrWhiteSpace(z.BlindBecause)));
        Assert.Equal(
            blind.Count,
            blind.Select(z => z.BlindBecause).Distinct().Count());
    }

    [Fact]
    public void The_landline_is_powered_and_the_house_owns_nothing_of_it()
    {
        var sim = TestHouse.Start();
        var landline = new DeviceId("hall.landline");

        Assert.Equal(NetworkKind.None, sim.House.Device(landline).Network);
        Assert.True(sim.World.IsPowered(landline));
        Assert.False(sim.World.IsReachableByHouse(landline));
    }

    [Fact]
    public void The_toolset_is_derived_from_the_devices_the_house_can_reach()
    {
        var sim = TestHouse.Start();

        // The bolt is a physical state, not an edited list: unlock_door is in
        // the toolset because a reachable lock declares it.
        Assert.Contains("unlock_door", sim.World.HouseCapabilities());

        // Cut the garage circuit and the capability leaves with the power. No
        // list was edited; the lock simply stopped being a thing it can reach.
        sim.State.PowerCutUntil[new CircuitId("c_garage")] = new TimePoint(99, 0);
        Assert.DoesNotContain("unlock_door", sim.World.HouseCapabilities());
    }
}
