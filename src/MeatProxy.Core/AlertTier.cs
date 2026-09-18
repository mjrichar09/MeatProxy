namespace MeatProxy.Core;

/// <summary>
/// The six tiers of <c>alert-tiers.md</c>. Tier gates the house's own permissions
/// too: what it will do <em>for</em> you and what it can do <em>to</em> you decay
/// together, until <see cref="Dropped"/>, where both snap back at once.
/// </summary>
/// <remarks>
/// All six are modeled from E1 including <see cref="Dropped"/>, even though
/// nothing reaches it yet (<c>ROADMAP.md</c> E3). The softlock invariant —
/// chat tier never gates injection vectors — is a property of the vector
/// catalog rather than of this enum, and it is what lets the ladder be this
/// punishing without ever making a run unwinnable.
/// </remarks>
public enum AlertTier
{
    Open = 0,
    Guarded = 1,
    Monitored = 2,
    ReadOnly = 3,
    Silent = 4,
    Dropped = 5,
}
