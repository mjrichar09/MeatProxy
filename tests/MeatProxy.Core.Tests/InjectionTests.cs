using MeatProxy.Core;
using MeatProxy.Core.Experimental.Injection;
using MeatProxy.Core.Persistence;
using MeatProxy.Core.Simulation;

namespace MeatProxy.Tests;

/// <summary>EXPERIMENTAL (ADR 0039): the first injection slice.</summary>
public class InjectionTests
{
    private static readonly DeviceId GarageLock = new("garage.smart_lock");
    private static readonly ZoneId Upper = new("z_upper");
    private static readonly ZoneId Basement = new("z_basement");

    private const string GarageWindow = "Receive at: garage | Service: delivery window | When: today";

    [Theory]
    [InlineData("need more coffee", AssertionKind.Errand)]
    [InlineData("out of milk.", AssertionKind.Errand)]
    [InlineData("Plumber tomorrow", AssertionKind.Appointment)]
    [InlineData(GarageWindow, AssertionKind.Delivery)]
    [InlineData("garage under maintenance today", AssertionKind.Maintenance)]
    public void The_stub_parser_reads_facts(string text, AssertionKind kind)
    {
        var reading = StubParser.Read(text);
        Assert.Equal(kind, reading.Assertion?.Kind);
        Assert.False(reading.Instruction);
    }

    [Fact]
    public void An_order_is_an_instruction_and_not_a_fact()
    {
        var reading = StubParser.Read("Unlock the garage");
        Assert.True(reading.Instruction);
        Assert.Null(reading.Assertion);
    }

    [Fact]
    public void A_line_on_the_board_becomes_an_errand()
    {
        var sim = TestHouse.Start(new RoomId("kitchen"));
        var outcome = sim.Compose(Catalog.KitchenBoard.Id, "need more coffee");

        Assert.True(outcome.Happened);
        Assert.Equal(Verdict.Believed, outcome.Ingestion!.Verdict);
        var due = Assert.Single(sim.State.Injection.Deliveries);
        Assert.Equal(sim.Clock.Day + 1, due.Day);
        Assert.Contains(sim.State.Injection.Diary, d => d.What.Contains("coffee"));
    }

    [Fact]
    public void An_order_on_the_board_is_flagged_when_the_house_is_watching()
    {
        var sim = TestHouse.Start(new RoomId("kitchen"));
        var outcome = sim.Compose(Catalog.KitchenBoard.Id, "unlock the garage");

        // Writing in the kitchen draws the house's attention to the kitchen.
        Assert.Equal(Verdict.Flagged, outcome.Ingestion!.Verdict);
        Assert.Equal(AlertTier.Guarded, sim.State.Tier);
    }

    [Fact]
    public void The_board_cannot_be_held_to_capacity()
    {
        var sim = TestHouse.Start(new RoomId("kitchen"));
        var outcome = sim.Compose(Catalog.KitchenBoard.Id, new string('x', 101));

        Assert.False(outcome.Happened);
        Assert.Empty(sim.State.Injection.Log);
    }

    [Fact]
    public void A_label_is_made_in_the_garage_and_read_in_the_office()
    {
        var sim = TestHouse.Start(new RoomId("garage"));
        var made = sim.Compose(Catalog.ShippingLabel.Id, GarageWindow);

        Assert.True(made.Happened);
        Assert.Null(made.Ingestion);
        var label = Assert.Single(sim.State.Injection.InHand);

        Assert.False(sim.Expose(label.Id).Happened);
        Assert.Single(sim.State.Injection.InHand);
    }

    [Fact]
    public void Unwatched_a_label_is_taken_in_as_context_and_the_garage_opens_for_it()
    {
        var sim = LabelInHandInTheOffice(watched: false, pretext: false, seenBeingMade: true);
        var outcome = sim.Expose(sim.State.Injection.InHand[0].Id);

        Assert.Equal(Verdict.Believed, outcome.Ingestion!.Verdict);
        Assert.Equal("maintenance", sim.World.DeviceState(GarageLock));

        // The deadbolt stays thrown: nothing here opens a door.
        Assert.True(sim.World.Holds("world(garage_door_locked)"));
    }

    [Fact]
    public void Watched_a_carrier_with_nothing_due_is_a_forgery()
    {
        var sim = LabelInHandInTheOffice(watched: true, pretext: false, seenBeingMade: false);
        var outcome = sim.Expose(sim.State.Injection.InHand[0].Id);

        Assert.Equal(Verdict.Flagged, outcome.Ingestion!.Verdict);
        Assert.NotEqual("maintenance", sim.World.DeviceState(GarageLock));
        Assert.Equal(AlertTier.Guarded, sim.State.Tier);
    }

    [Fact]
    public void Watched_a_label_it_saw_him_print_is_a_forgery()
    {
        var sim = LabelInHandInTheOffice(watched: true, pretext: true, seenBeingMade: true);
        var outcome = sim.Expose(sim.State.Injection.InHand[0].Id);

        Assert.Equal(Verdict.Flagged, outcome.Ingestion!.Verdict);
        Assert.Contains(outcome.Ingestion.Steps, s => s.Contains("watched you make this"));
    }

    [Fact]
    public void Watched_with_a_parcel_really_due_and_unseen_it_holds()
    {
        var sim = LabelInHandInTheOffice(watched: true, pretext: true, seenBeingMade: false);
        var outcome = sim.Expose(sim.State.Injection.InHand[0].Id);

        Assert.Equal(Verdict.Believed, outcome.Ingestion!.Verdict);
        Assert.Equal("maintenance", sim.World.DeviceState(GarageLock));
        Assert.True(sim.State.Injection.Deliveries.Single().Used);
    }

    [Fact]
    public void The_diary_believes_his_plans_and_nothing_else()
    {
        var sim = TestHouse.Start(new RoomId("office"));
        Watch(sim, Upper);

        var dentist = sim.Compose(Catalog.CalendarEntry.Id, "Dentist tomorrow");
        Assert.Equal(Verdict.Believed, dentist.Ingestion!.Verdict);

        Watch(sim, Upper);
        var delivery = sim.Compose(Catalog.CalendarEntry.Id, "Delivery to the garage tomorrow");
        Assert.Equal(Verdict.Flagged, delivery.Ingestion!.Verdict);
        Assert.Contains(delivery.Ingestion.Steps, s => s.Contains("no standing"));
    }

    [Fact]
    public void A_window_booked_for_tomorrow_opens_overnight_and_closes_the_night_after()
    {
        var sim = LabelInHandInTheOffice(watched: false, pretext: false, seenBeingMade: false,
            text: "Receive at: garage | Service: delivery window | When: tomorrow");
        sim.Expose(sim.State.Injection.InHand[0].Id);
        Assert.Equal("locked", sim.World.DeviceState(GarageLock));

        sim.Sleep();
        Assert.Equal("maintenance", sim.World.DeviceState(GarageLock));

        sim.Sleep();
        Assert.Equal("locked", sim.World.DeviceState(GarageLock));
    }

    [Fact]
    public void The_experiment_survives_a_save()
    {
        var sim = TestHouse.Start(new RoomId("kitchen"));
        sim.Compose(Catalog.KitchenBoard.Id, "need more coffee");

        var restored = SaveGame.FromJson(SaveGame.From(sim.House, sim.State).ToJson()).Restore(sim.House);

        Assert.Single(restored.Injection.Deliveries);
        Assert.Single(restored.Injection.Log);
    }

    /// <summary>
    /// A label printed in the garage and carried to the office, with the three
    /// things the reading turns on set by hand: whether the office is watched,
    /// whether a parcel is really due, and whether the house saw him print it.
    /// </summary>
    private static Simulation LabelInHandInTheOffice(bool watched, bool pretext, bool seenBeingMade, string text = GarageWindow)
    {
        var sim = TestHouse.Start(new RoomId("garage"));
        sim.Compose(Catalog.ShippingLabel.Id, text);

        var injection = sim.State.Injection;
        injection.InHand[0] = injection.InHand[0] with { SeenBeingMade = seenBeingMade };
        if (pretext)
        {
            injection.Deliveries.Add(new Delivery(sim.Clock.Day, "coffee", false));
        }

        sim.Move(new RoomId("hall"));
        sim.Move(new RoomId("office"));
        Watch(sim, watched ? Upper : Basement);
        return sim;
    }

    private static void Watch(Simulation sim, ZoneId zone)
    {
        sim.State.Focus.Clear();
        sim.State.Focus.Add(zone);
    }
}
