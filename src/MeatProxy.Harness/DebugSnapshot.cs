using System.Text.Json;
using MeatProxy.Core;
using MeatProxy.Core.Adversary;
using MeatProxy.Core.Enforcement;
using MeatProxy.Core.Predicates;
using MeatProxy.Core.Simulation;
using MeatProxy.Core.World;

namespace MeatProxy.Harness;

/// <summary>
/// One frame of the debug view: the house as the core sees it, flattened to
/// JSON for a page to draw. Shared by the harness's static view and the
/// playable one in <c>MeatProxy.Web</c>, so both draw the same thing.
/// </summary>
public static class DebugSnapshot
{
    public static readonly JsonSerializerOptions Options =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>The house as it stands, with what the last action did to it.</summary>
    public static JsonElement Of(Simulation sim, string caption, string note, ActionOutcome? last = null)
    {
        var world = sim.World;
        var house = sim.House;
        var focus = sim.State.Focus;

        // Serialized now, not when the caller gets round to it: the projections
        // below are lazy, and a frame read later would show the house as it ended
        // rather than as it was.
        return JsonSerializer.SerializeToElement(new
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
            canSpeakHere = world.CanSpeakToHouseFrom(sim.State.PlayerRoom),
            bodyToday = sim.State.BodySpentToday,
            bodyByDay = sim.State.BodySpentByDay,
            unit = new
            {
                status = sim.State.Unit.Status.ToString(),
                room = sim.UnitRoom.Value,
                dock = sim.UnitDock.Value,
                target = sim.State.Unit.Target?.Value,
                reason = sim.State.Unit.Reason,
                arrives = sim.State.Unit.Status == UnitStatus.EnRoute ? sim.State.Unit.LegsEnd?.ToString() : null,
                standsUntil = sim.State.Unit.StandsUntil?.ToString(),
            },
            disclosures = sim.State.Disclosures.Select(d => new { day = d.At.Day, slice = d.At.Slice, target = d.Target.Value, reason = d.Reason }),
            arrivals = sim.State.Arrivals.Select(a => new
            {
                day = a.At.Day,
                slice = a.At.Slice,
                room = a.Room.Value,
                present = a.PlayerPresent,
                interrupted = a.Interrupted,
            }),
            circuits = house.Circuits.Select(c => new { id = c.Id.Value, name = c.Name, cut = world.IsCircuitCut(c.Id) }),
            upgrades = house.SensingUpgrades.Select(u => new
            {
                id = u.Id,
                applied = sim.State.SensingUpgradesApplied.Contains(u.Id),
            }),
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
                    openings = room.Openings.Select(o => new
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
                        affordances = d.PlayerAffordances,
                        operateStates = d.OperateStates,
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
            events = last?.Events ?? [],
            interrupted = last?.Interrupted ?? false,
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
        }, Options);
    }
}
