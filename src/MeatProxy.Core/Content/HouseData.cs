using System.Reflection;

namespace MeatProxy.Core.Content;

/// <summary>Loads the authored house that ships with the core.</summary>
public static class HouseData
{
    public const string FileName = "house.json";

    /// <summary>
    /// The house as authored, next to the assembly. The harness and the tests
    /// both load this rather than building a fixture in code, so what the tests
    /// prove is what the game runs.
    /// </summary>
    public static HouseFile LoadDefault()
    {
        var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? Directory.GetCurrentDirectory();
        var path = Path.Combine(directory, "Content", FileName);

        if (!File.Exists(path))
        {
            path = Path.Combine(directory, FileName);
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Could not find {FileName} beside {directory}.");
        }

        return HouseFile.FromJson(File.ReadAllText(path));
    }

    public static HouseFile LoadFrom(string path) => HouseFile.FromJson(File.ReadAllText(path));
}
