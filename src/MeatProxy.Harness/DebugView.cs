using System.Reflection;
using System.Text.Json;
using MeatProxy.Core;
using MeatProxy.Core.Adversary;
using MeatProxy.Core.Predicates;
using MeatProxy.Core.Simulation;
using MeatProxy.Core.World;

namespace MeatProxy.Harness;

/// <summary>
/// The debug view E2's exit asks for: one page, written from the real state,
/// that shows seen and understood as different things — and the tier ladder,
/// so E3 is visible too.
/// </summary>
/// <remarks>
/// Not presentation. It draws whatever the core says, placeholder geometry and
/// all, and the page labels placeholder rooms as such. Godot draws the game;
/// this draws the simulation.
/// </remarks>
public sealed class DebugView
{
    private static readonly JsonSerializerOptions Options =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly List<JsonElement> _frames = [];

    public int Count => _frames.Count;

    /// <summary>Capture the house as it stands, with what the last action did to it.</summary>
    public void Capture(Simulation sim, string caption, string note, ActionOutcome? last = null)
    {
        var world = sim.World;
        var house = sim.House;
        var focus = sim.State.Focus;

        // Serialized now, not at write time: the projections below are lazy, and
        // a frame read later would show the house as it ended rather than as it was.
        _frames.Add(JsonSerializer.SerializeToElement(new
        {
            caption,
            note,
            day = sim.Clock.Day,
            slicesLeft = sim.Clock.SlicesRemaining,
            tier = sim.State.Tier.ToString(),
            tierIndex = (int)sim.State.Tier,
            cleanDays = sim.State.CleanDays,
            cleanDaysPerStep = AlertLadder.CleanDaysPerStep,
            player = sim.State.PlayerRoom.Value,
            playerActivity = sim.State.PlayerActivity is { } activity ? Predicate.Spell(activity) : null,
            focusSlots = sim.State.FocusSlots,
            levels = house.Levels.OrderByDescending(l => l.Elevation).Select(level => new
            {
                id = level.Id.Value,
                name = level.Name,
                rooms = house.Rooms.Where(r => r.Level == level.Id).Select(room => new
                {
                    id = room.Id.Value,
                    name = room.Name,
                    x = room.Origin.X,
                    y = room.Origin.Y,
                    w = room.Dims.Width,
                    d = room.Dims.Depth,
                    zone = room.Zone.Value,
                    placeholder = room.Geometry == GeometryProvenance.Placeholder,
                    visited = sim.State.RoomsVisited.Contains(room.Id),
                    openings = house.OpeningsFrom(room.Id).Where(o => o.From == room.Id).Select(o => new
                    {
                        id = o.Id.Value,
                        to = o.To.Value,
                        kind = o.Kind.ToString(),
                        wall = o.Wall.ToString(),
                        offset = o.Offset,
                        width = o.Width,
                        locked = world.IsLocked(o),
                        openable = o.Openable,
                        leaves = o.LeavesTheHouse,
                        crossesLevel = !o.To.IsExterior && house.Room(o.To).Level != room.Level,
                    }),
                    devices = house.DevicesIn(room.Id).Select(d => new
                    {
                        id = d.Id.Value,
                        cls = d.Class,
                        state = world.DeviceState(d.Id),
                        powered = world.IsPowered(d.Id),
                        reachable = world.IsReachableByHouse(d.Id),
                        dumb = !d.ReachableByHouse,
                        blinded = world.IsBlinded(d.Id),
                        senses = world.Senses(d.Id).Select(c => c.ToString()).Order(),
                    }),
                }),
            }),
            zones = house.Zones.OrderBy(z => z.Id.Value).Select(z => new
            {
                id = z.Id.Value,
                name = z.Name,
                covered = world.IsCovered(z.Id),
                interpretable = world.IsInterpretable(z.Id),
                focused = focus.Contains(z.Id),
                understood = world.IsUnderstood(z.Id),
                channels = world.LiveChannels(z.Id).Select(c => c.ToString()).Order(),
                blindBecause = world.IsInterpretable(z.Id) ? null : z.BlindBecause,
            }),
            tools = world.ReachableCapabilities()
                .Concat(house.Devices.SelectMany(d => d.HouseCapabilities))
                .Distinct()
                .Order()
                .Select(t => new
                {
                    name = t,
                    cls = Tools.ClassOf(t).ToString(),
                    reachable = world.ReachableCapabilities().Contains(t),
                    held = world.HouseCapabilities().Contains(t),
                }),
            seen = last?.Detections.Select(d => new
            {
                channel = d.Channel.ToString(),
                zone = d.Zone.Value,
                magnitude = d.Magnitude.ToString(),
                device = d.Device?.Value,
            }) ?? [],
            understood = last?.Understandings.Select(u => new
            {
                zone = u.Zone.Value,
                claim = u.Claim is { } claim ? Predicate.Spell(claim) : null,
                confidence = u.Confidence,
            }) ?? [],
            believes = sim.State.Salient.Facts.Select(f => f.Fact),
            ladder = sim.State.TierHistory.Select(c => new
            {
                day = c.At.Day,
                slice = c.At.Slice,
                from = c.From.ToString(),
                to = c.To.ToString(),
                reason = c.Reason,
                escalation = c.IsEscalation,
            }),
        }, Options));
    }

    public void Write(string path, string houseName)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MeatProxy.Harness.DebugView.html")
            ?? throw new InvalidOperationException("The debug view template is not embedded.");
        using var reader = new StreamReader(stream);

        var data = JsonSerializer.Serialize(new { house = houseName, frames = _frames }, Options);

        // Inside a <script> block, so a closing tag in any string must not end it.
        data = data.Replace("</", "<\\/");

        File.WriteAllText(path, reader.ReadToEnd().Replace("/*FRAMES*/null", data));
    }
}
