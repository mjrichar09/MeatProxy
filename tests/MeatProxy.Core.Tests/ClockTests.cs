using MeatProxy.Core.Time;
using MeatProxy.Core;

namespace MeatProxy.Tests;

public class ClockTests
{
    [Fact]
    public void Actions_spend_the_day_and_the_day_runs_out()
    {
        var clock = DayClock.StartOfRun(TimeSettings.Default with { FullDayBudget = 10 });

        Assert.Equal(10, clock.SlicesRemaining);
        Assert.Equal(4, clock.Spend(4));
        Assert.False(clock.DayIsSpent);

        // Asked for more than is left, it spends what there is and says so.
        Assert.Equal(6, clock.Spend(20));
        Assert.True(clock.DayIsSpent);
        Assert.Equal(0, clock.SlicesRemaining);
    }

    [Fact]
    public void Sleeping_turns_the_day_over()
    {
        var sim = TestHouse.Start();
        sim.Wait(5);

        var outcome = sim.Sleep();

        Assert.True(outcome.Happened);
        Assert.Equal(2, sim.Clock.Day);
        Assert.Equal(0, sim.Clock.SlicesSpent);
        Assert.Equal(TimeSettings.Default.FullDayBudget, sim.Clock.Budget);
    }

    [Fact]
    public void Clarity_degrading_is_a_smaller_number_and_nothing_more()
    {
        // ADR 0007's first symptom is mechanical: the day gets shorter.
        var sim = TestHouse.Start();
        sim.Sleep(nextBudget: 60);

        Assert.Equal(60, sim.Clock.Budget);
        Assert.Equal(60, sim.Clock.SlicesRemaining);
    }

    [Fact]
    public void Time_does_not_run_backwards()
    {
        var clock = DayClock.StartOfRun(TimeSettings.Default);
        Assert.Throws<ArgumentOutOfRangeException>(() => clock.Spend(-1));
    }

    [Fact]
    public void A_time_point_orders_by_day_then_slice()
    {
        Assert.True(new TimePoint(1, 90) < new TimePoint(2, 0));
        Assert.True(new TimePoint(3, 4) > new TimePoint(3, 3));
        Assert.True(new TimePoint(3, 4) >= new TimePoint(3, 4));
    }
}
