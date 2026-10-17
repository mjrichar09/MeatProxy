using System.Reflection;
using System.Text.Json;
using MeatProxy.Core.Simulation;

namespace MeatProxy.Harness;

/// <summary>
/// The debug view E2's exit asks for: one page, written from the real state,
/// that shows seen and understood as different things — and the tier ladder,
/// so E3 is visible too.
/// </summary>
/// <remarks>
/// Not presentation. It draws whatever the core says, and draws a room with
/// placeholder geometry dashed if one ever ships again. Godot draws the game;
/// this draws the simulation.
/// </remarks>
public sealed class DebugView
{
    private readonly List<JsonElement> _frames = [];

    public int Count => _frames.Count;

    /// <summary>Capture the house as it stands, with what the last action did to it.</summary>
    public void Capture(Simulation sim, string caption, string note, ActionOutcome? last = null) =>
        _frames.Add(DebugSnapshot.Of(sim, caption, note, last));

    public void Write(string path, string houseName)
    {
        var data = JsonSerializer.Serialize(new { house = houseName, frames = _frames }, DebugSnapshot.Options);

        // Inside a <script> block, so a closing tag in any string must not end it.
        data = data.Replace("</", "<\\/");

        // One file, so it opens anywhere: the shared drawing code goes inline.
        var page = Resource("MeatProxy.Harness.DebugView.html")
            .Replace("/*CSS*/", Resource("MeatProxy.Harness.monitor.css"))
            .Replace("/*JS*/", Resource("MeatProxy.Harness.monitor.js"))
            .Replace("/*FRAMES*/null", data);

        File.WriteAllText(path, page);
    }

    private static string Resource(string name)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name)
            ?? throw new InvalidOperationException($"{name} is not embedded.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
