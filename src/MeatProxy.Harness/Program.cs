using MeatProxy.Core;
using MeatProxy.Core.Content;
using MeatProxy.Core.Persistence;
using MeatProxy.Core.Simulation;
using MeatProxy.Core.State;

namespace MeatProxy.Harness;

/// <summary>
/// The headless debug harness. Walk the house, spend the day, ask the world
/// whether something is true, save and restore.
/// </summary>
public static class Program
{
    public static int Main(string[] args)
    {
        var file = HouseData.LoadDefault();
        var house = file.ToHouse();

        var problems = house.Validate(file.StartRoom);
        if (problems.Count > 0)
        {
            Console.Error.WriteLine("The house does not hold together:");
            foreach (var problem in problems)
            {
                Console.Error.WriteLine($"  - {problem}");
            }

            return 1;
        }

        var state = new WorldState { PlayerRoom = file.StartRoom };
        var session = new Session(new Simulation(house, state));

        if (args.Contains("--tour"))
        {
            return session.Tour();
        }

        return session.Repl();
    }
}
