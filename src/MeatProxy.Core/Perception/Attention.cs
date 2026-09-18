namespace MeatProxy.Core.Perception;

/// <summary>
/// Where the interpreter is pointed, and why.
/// </summary>
/// <remarks>
/// <para>
/// The house holds a small number of focus slots and moves them on its own
/// logic — anomaly, and what it already suspects. The logic is deterministic
/// engine code and the player learns it by playing, which is what makes
/// diverting attention a verb rather than a cooldown: you are not hiding, you
/// are giving it something better to look at somewhere else.
/// </para>
/// <para>
/// Slots move at turn boundaries only, never inside a pressure window (ADR 0008).
/// </para>
/// </remarks>
public sealed class Attention
{
    /// <summary>
    /// Focus slots start at one (<c>sensors.md</c> §2). Adding one is a
    /// concession in ADR 0006's currency, not a difficulty knob, so it is an
    /// explicit act with a reason attached rather than a number that drifts up.
    /// </summary>
    public const int StartingSlots = 1;

    /// <summary>
    /// A little inertia, so the interpreter does not flicker between two zones
    /// that are scoring within a hair of each other.
    /// </summary>
    private const double Stickiness = 0.15;

    /// <summary>A zone the house already holds a belief about is worth another look.</summary>
    private const double SuspicionBonus = 0.4;

    /// <summary>
    /// Point the slots at the zones worth reading. Returns where they ended up.
    /// </summary>
    public IReadOnlyList<ZoneId> Reassign(WorldView world, IReadOnlyList<Detection> thisTurn)
    {
        var state = world.State;
        var scores = new Dictionary<ZoneId, double>();

        foreach (var zone in world.House.Zones)
        {
            if (!world.IsInterpretable(zone.Id) || !world.IsCovered(zone.Id))
            {
                continue;
            }

            scores[zone.Id] = Score(world, zone.Id, thisTurn);
        }

        var focused = scores
            .OrderByDescending(s => s.Value)
            .ThenBy(s => s.Key.Value)
            .Where(s => s.Value > 0)
            .Take(state.FocusSlots)
            .Select(s => s.Key)
            .ToList();

        state.Focus.Clear();
        state.Focus.UnionWith(focused);
        return focused;
    }

    private double Score(WorldView world, ZoneId zone, IReadOnlyList<Detection> thisTurn)
    {
        var anomaly = thisTurn
            .Where(d => d.Zone == zone)
            .Sum(d => d.Magnitude switch
            {
                Magnitude.High => 1.0,
                Magnitude.Medium => 0.6,
                _ => 0.3,
            });

        if (anomaly == 0)
        {
            return 0;
        }

        if (world.State.Focus.Contains(zone))
        {
            anomaly += Stickiness;
        }

        if (world.State.Understandings.Any(u => u.Zone == zone && u.Claim is not null
            && world.State.Salient.Believes(u.Claim.Value)))
        {
            anomaly += SuspicionBonus;
        }

        return anomaly;
    }
}
