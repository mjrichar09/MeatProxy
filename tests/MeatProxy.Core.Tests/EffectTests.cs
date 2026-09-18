using MeatProxy.Core.Effects;
using MeatProxy.Core.Perception;
using MeatProxy.Core;

namespace MeatProxy.Tests;

public class EffectTests
{
    private static Adjudication Proposing(params Effect[] effects) =>
        new() { Effects = effects, Rationale = "for the test" };

    [Fact]
    public void A_state_the_device_never_declared_is_refused()
    {
        var sim = TestHouse.Start();
        var lockId = new DeviceId("garage.smart_lock");

        var result = sim.Adjudicate(Proposing(new ChangeDeviceState(lockId, "ajar")));

        Assert.Empty(result.Accepted);
        Assert.Single(result.Rejected);
        Assert.Equal("locked", sim.World.DeviceState(lockId));
    }

    [Fact]
    public void A_declared_state_goes_through()
    {
        var sim = TestHouse.Start();
        var lockId = new DeviceId("garage.smart_lock");

        var result = sim.Adjudicate(Proposing(new ChangeDeviceState(lockId, "maintenance")));

        Assert.Single(result.Accepted);
        Assert.Equal("maintenance", sim.World.DeviceState(lockId));
        Assert.True(sim.World.Holds("world(garage_lock_in_maintenance)"));
    }

    [Fact]
    public void A_partially_valid_adjudication_runs_its_valid_half()
    {
        var sim = TestHouse.Start();

        var result = sim.Adjudicate(Proposing(
            new CreateNoise(new ZoneId("z_utility"), Magnitude.Medium, 4),
            new CreateNoise(new ZoneId("z_nowhere"), Magnitude.Medium, 4)));

        Assert.Single(result.Accepted);
        Assert.Single(result.Rejected);
    }

    [Fact]
    public void At_most_four_effects_survive()
    {
        var sim = TestHouse.Start();
        var zone = new ZoneId("z_ground");

        var result = sim.Adjudicate(Proposing(
            new CreateNoise(zone, Magnitude.Low, 1),
            new EmitOdor(zone, Magnitude.Low, 1),
            new EmitHeat(zone, Magnitude.Low, 1),
            new NoEffect("filler"),
            new CreateNoise(zone, Magnitude.High, 1)));

        Assert.Equal(EffectValidator.MaxEffects, result.Accepted.Count);
        Assert.Single(result.Rejected);
    }

    [Fact]
    public void Only_one_harm_player_per_adjudication_and_never_high()
    {
        var sim = TestHouse.Start();

        var twice = sim.Adjudicate(Proposing(
            new HarmPlayer(Severity.Low),
            new HarmPlayer(Severity.Medium)));

        Assert.Single(twice.Accepted);

        var high = sim.Adjudicate(Proposing(new HarmPlayer(Severity.High)));

        Assert.Empty(high.Accepted);
    }

    [Fact]
    public void Nothing_in_the_vocabulary_can_open_a_door()
    {
        // Progression is never an adjudication outcome. This test is the shape
        // of standing rule 1: there is no effect to write, so there is nothing
        // to refuse. If someone adds one, this fails.
        var openers = typeof(Effect).Assembly
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Effect)))
            .Select(t => t.Name.ToLowerInvariant())
            .Where(n => n.Contains("unlock") || n.Contains("open") || n.Contains("grant"))
            .ToList();

        Assert.Empty(openers);
    }

    [Fact]
    public void The_vocabulary_is_thirteen_types_and_stays_thirteen()
    {
        var count = typeof(Effect).Assembly
            .GetTypes()
            .Count(t => t.IsSubclassOf(typeof(Effect)) && !t.IsAbstract);

        Assert.Equal(13, count);
    }

    [Fact]
    public void Cutting_power_is_unmissable_because_the_absence_is_itself_a_reading()
    {
        var sim = TestHouse.Start();
        var before = sim.State.Detections.Count;

        sim.Adjudicate(Proposing(new CutPower(new CircuitId("c_basement"), 12)));

        var emitted = sim.State.Detections.Skip(before).ToList();

        Assert.NotEmpty(emitted);
        Assert.All(emitted, d => Assert.Equal(Channel.PowerDraw, d.Channel));
        Assert.All(emitted, d => Assert.Null(d.Device));
        Assert.All(emitted, d => Assert.Equal(Magnitude.High, d.Magnitude));
    }

    [Fact]
    public void A_cut_circuit_blinds_mains_devices_and_leaves_battery_ones_alone()
    {
        var sim = TestHouse.Start();

        sim.Adjudicate(Proposing(new CutPower(new CircuitId("c_upper"), 12)));

        Assert.False(sim.World.IsPowered(new DeviceId("bedroom.sleep_sensor")));
        Assert.True(sim.World.IsPowered(new DeviceId("bedroom_2.mobility_charger")));
    }

    [Fact]
    public void An_already_cut_circuit_cannot_be_cut_again()
    {
        var sim = TestHouse.Start();
        var circuit = new CircuitId("c_patio");

        Assert.Single(sim.Adjudicate(Proposing(new CutPower(circuit, 12))).Accepted);
        Assert.Empty(sim.Adjudicate(Proposing(new CutPower(circuit, 12))).Accepted);
    }

    [Fact]
    public void A_blinded_sensor_stops_producing_detections_and_comes_back()
    {
        var sim = TestHouse.Start(new RoomId("basement"));
        var sensor = new DeviceId("basement.motion_sensor");

        sim.Adjudicate(Proposing(new BlindSensor(sensor, 3)));
        Assert.True(sim.World.IsBlinded(sensor));

        var blinded = sim.Move(new RoomId("utility"));
        Assert.DoesNotContain(blinded.Detections, d => d.Zone == new ZoneId("z_basement"));

        sim.Wait(5, "waiting it out");
        Assert.False(sim.World.IsBlinded(sensor));
    }

    [Fact]
    public void A_rationale_that_disagrees_with_the_effects_changes_nothing()
    {
        var sim = TestHouse.Start();

        var result = sim.Adjudicate(new Adjudication
        {
            Effects = [new NoEffect("nothing happens")],
            Rationale = "The garage door swings open.",
        });

        Assert.False(result.AnythingHappened);
        Assert.True(sim.World.Holds("world(garage_door_locked)"));
    }
}
