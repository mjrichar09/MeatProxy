using System.Text.Json.Nodes;
using MeatProxy.Core;
using MeatProxy.Core.Content;
using MeatProxy.Core.Effects;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Player;
using MeatProxy.Core.Simulation;

namespace MeatProxy.Tests;

public class AffordanceTests
{
    private static readonly DeviceId Tv = new("living_room.tv");
    private static readonly DeviceId LivingSpeaker = new("living_room.smart_speaker");
    private static readonly DeviceId LivingCamera = new("living_room.camera");
    private static readonly DeviceId Panel = new("basement.circuit_panel");

    [Fact]
    public void Every_affordance_in_the_house_is_in_the_closed_set()
    {
        var offered = TestHouse.Load().Devices.SelectMany(d => d.PlayerAffordances).Distinct();
        Assert.All(offered, a => Assert.True(Affordances.TryParse(a, out _), a));
    }

    [Fact]
    public void A_record_that_offers_anything_else_is_rejected()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Content", HouseData.FileName);
        var json = JsonNode.Parse(File.ReadAllText(path))!;
        var tv = json["devices"]!.AsArray().First(d => (string?)d!["id"] == "living_room.tv")!;
        tv["player_affordances"]!.AsArray().Add("smash");

        var file = HouseFile.FromJson(json.ToJsonString());
        Assert.Contains(file.ToHouse().Validate(file.StartRoom), p => p.Contains("'smash'"));
    }

    [Fact]
    public void You_can_only_act_on_what_is_in_the_room()
    {
        var sim = TestHouse.Start();
        var outcome = sim.Act(Tv, Affordance.Examine);

        Assert.False(outcome.Happened);
        Assert.Equal(MoveRefusal.NotHere, outcome.Refusal);
    }

    [Fact]
    public void An_object_offers_only_what_it_declares()
    {
        var sim = TestHouse.Start(new RoomId("living_room"));
        var outcome = sim.Act(LivingCamera, Affordance.PowerToggle, "c_ground");

        Assert.Equal(MoveRefusal.NotOffered, outcome.Refusal);
    }

    [Fact]
    public void Operating_sets_one_of_the_declared_states_and_nothing_else()
    {
        var sim = TestHouse.Start(new RoomId("living_room"));

        var on = sim.Act(Tv, Affordance.Operate, "on");
        Assert.True(on.Happened);
        Assert.Equal(1, on.SlicesSpent);
        Assert.Equal("on", sim.World.DeviceState(Tv));

        var faulted = sim.Act(Tv, Affordance.Operate, "faulted");
        Assert.Equal(MoveRefusal.BadArgument, faulted.Refusal);
        Assert.Equal("on", sim.World.DeviceState(Tv));
    }

    [Fact]
    public void A_device_with_no_operate_states_has_nothing_to_set()
    {
        var sim = TestHouse.Start(new RoomId("living_room"));
        var outcome = sim.Act(LivingCamera, Affordance.Operate);

        Assert.True(outcome.Happened);
        Assert.Contains("nothing on it to set", outcome.Description);
        Assert.Equal("ok", sim.World.DeviceState(LivingCamera));
    }

    [Fact]
    public void Disassembling_is_loud_slow_durable_and_costs_the_body()
    {
        var sim = TestHouse.Start(new RoomId("living_room"));
        var outcome = sim.Act(LivingSpeaker, Affordance.Disassemble);

        Assert.Equal(3, outcome.SlicesSpent);
        Assert.Equal("destroyed", sim.World.DeviceState(LivingSpeaker));
        Assert.Equal(1, sim.State.BodySpentToday);
        Assert.Contains(outcome.Detections, d => d.Channel == Channel.Motion && d.Magnitude == Magnitude.High);
        Assert.Equal(Claim.Dismantling, sim.State.PlayerActivity);
    }

    [Fact]
    public void A_hand_on_the_breaker_stays_off_until_it_is_put_back()
    {
        var sim = TestHouse.Start(new RoomId("basement"));
        var garage = new CircuitId("c_garage");

        var off = sim.Act(Panel, Affordance.PowerToggle, "c_garage");
        Assert.True(sim.World.IsCircuitCut(garage));
        Assert.DoesNotContain("unlock_door", sim.World.HouseCapabilities());
        Assert.Contains(sim.State.Detections, d => d.Channel == Channel.PowerDraw && d.Device is null);

        for (var i = 0; i < 3; i++)
        {
            sim.Sleep();
        }

        Assert.True(sim.World.IsCircuitCut(garage));

        sim.Act(Panel, Affordance.PowerToggle, "c_garage");
        Assert.False(sim.World.IsCircuitCut(garage));
    }

    [Fact]
    public void Combine_asks_the_adjudicator_and_the_stub_rules_nothing_happened()
    {
        var sim = WithCombinableTv();
        var outcome = sim.Act(Tv, Affordance.Combine, LivingSpeaker.Value);

        Assert.True(outcome.Happened);
        Assert.Equal(2, outcome.SlicesSpent);
        Assert.Contains("nothing happened", outcome.Description);
    }

    [Fact]
    public void What_combine_returns_still_goes_through_the_validator()
    {
        // A ruler that tries to destroy something in another room, and to cut
        // a circuit for a sane length. The first is refused; the second runs.
        var sim = WithCombinableTv(new FixedRuler(
            new DamageDevice(new DeviceId("no_such.device"), Severity.High),
            new CutPower(new CircuitId("c_patio"), 4)));

        var outcome = sim.Act(Tv, Affordance.Combine, LivingSpeaker.Value);

        Assert.True(outcome.Happened);
        Assert.True(sim.World.IsCircuitCut(new CircuitId("c_patio")));
    }

    [Fact]
    public void Stairs_spend_the_body_and_doors_do_not()
    {
        var sim = TestHouse.Start();
        sim.Move(new RoomId("hall"));
        Assert.Equal(1, sim.State.BodySpentToday);

        sim.Move(new RoomId("kitchen"));
        Assert.Equal(1, sim.State.BodySpentToday);

        sim.Sleep();
        Assert.Equal(0, sim.State.BodySpentToday);
        Assert.Equal([1], sim.State.BodySpentByDay);
    }

    /// <summary>
    /// No shipped object offers combine yet; which ones do is content. These
    /// tests add it to the TV in a copy of the house, standing in the living room.
    /// </summary>
    private static Simulation WithCombinableTv(IAdjudicator? adjudicator = null)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Content", HouseData.FileName);
        var json = JsonNode.Parse(File.ReadAllText(path))!;
        var tv = json["devices"]!.AsArray().First(d => (string?)d!["id"] == "living_room.tv")!;
        tv["player_affordances"]!.AsArray().Add("combine");

        var house = HouseFile.FromJson(json.ToJsonString()).ToHouse();
        var state = new MeatProxy.Core.State.WorldState { PlayerRoom = new RoomId("living_room") };
        return new Simulation(house, state, adjudicator: adjudicator);
    }

    private sealed class FixedRuler(params Effect[] effects) : IAdjudicator
    {
        public Adjudication Rule(CombineRequest request) =>
            new() { Effects = effects, Rationale = "A fixed ruling, for the test." };
    }
}
