using MeatProxy.Core.Perception;

namespace MeatProxy.Core.State;

/// <summary>Something the house has decided matters.</summary>
public sealed record SalientFact
{
    public required string Fact { get; init; }
    public required int Day { get; init; }

    /// <summary>
    /// Set when this salient fact is a promoted claim. That promotion is the
    /// bridge between the two vocabularies: the house <em>holding</em> a
    /// conclusion is a fact even when the conclusion is wrong
    /// (<c>claims.md</c> §2).
    /// </summary>
    public Claim? Claim { get; init; }
}

/// <summary>
/// Block 5 of the world-state summary, and the player-facing exploit
/// (<c>world-state-summary.md</c> §2).
/// </summary>
/// <remarks>
/// <para>
/// The block is capped. Promote a new fact into a full one and the oldest falls
/// out, so a player can push a memory out of the house's head by giving it
/// enough new things to find important.
/// </para>
/// <para>
/// Three things keep that honest, and all three are enforced here rather than
/// hoped for. It is engine code, so the model never edits its own memory and the
/// exploit is deterministic, learnable and repeatable. The house is not lying
/// when it forgets — asked about an evicted fact it answers honestly from what
/// it has, which is nothing. And it costs, because manufacturing salience means
/// doing things worth noticing.
/// </para>
/// </remarks>
public sealed class SalientBlock
{
    private readonly List<SalientFact> _facts = [];

    /// <summary>
    /// The cap. <c>world-state-summary.md</c> §6 leaves the number open — too
    /// high and the exploit is invisible, too low and the house is an amnesiac —
    /// so it is a tunable carried in the save, not a constant. Seven is a working
    /// figure pending a playthrough.
    /// </summary>
    public int Cap { get; set; } = 7;

    public IReadOnlyList<SalientFact> Facts => _facts;

    /// <summary>
    /// Promote a fact. Returns what fell out, which is the thing the player is
    /// actually aiming at when they run the context exploit.
    /// </summary>
    public SalientFact? Promote(SalientFact fact)
    {
        _facts.RemoveAll(f => f.Fact == fact.Fact);
        _facts.Add(fact);

        if (_facts.Count <= Cap)
        {
            return null;
        }

        var evicted = _facts[0];
        _facts.RemoveAt(0);
        return evicted;
    }

    public bool Holds(string fact) => _facts.Any(f => f.Fact == fact);

    public bool Believes(Claim claim) => _facts.Any(f => f.Claim == claim);

    internal void Restore(IEnumerable<SalientFact> facts)
    {
        _facts.Clear();
        _facts.AddRange(facts);
    }
}
