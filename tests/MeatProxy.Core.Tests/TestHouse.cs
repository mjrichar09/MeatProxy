using MeatProxy.Core;
using MeatProxy.Core.Content;
using MeatProxy.Core.Simulation;
using MeatProxy.Core.State;

namespace MeatProxy.Tests;

/// <summary>
/// Every test runs against the house that ships, not against a fixture built in
/// code. A fixture would pass while the authored house was broken, which is the
/// failure mode this whole file exists to prevent.
/// </summary>
public static class TestHouse
{
    public static HouseFile File { get; } = HouseData.LoadDefault();

    public static House Load() => File.ToHouse();

    public static Simulation Start(RoomId? at = null)
    {
        var house = Load();
        var state = new WorldState { PlayerRoom = at ?? File.StartRoom };
        return new Simulation(house, state);
    }
}
