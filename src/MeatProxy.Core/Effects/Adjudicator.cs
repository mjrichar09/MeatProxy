using MeatProxy.Core.Devices;

namespace MeatProxy.Core.Effects;

/// <summary>
/// What <c>combine</c> hands the ruler: both objects' authored properties and
/// where the act happened (<c>affordances.md</c> §3). No authored pair list is
/// consulted, because unauthored pairs are the point.
/// </summary>
public sealed record CombineRequest(Device First, Device Second, RoomId Room, ZoneId Zone);

/// <summary>
/// Rules on a combination. Lane A's model sits behind this from A1; until then
/// the stub does. Whatever comes back still goes through
/// <see cref="EffectValidator"/>: the model proposes, the engine disposes.
/// </summary>
public interface IAdjudicator
{
    Adjudication Rule(CombineRequest request);
}

/// <summary>
/// The stub. It rules that nothing happened, which <c>affordances.md</c> §3 calls
/// a legitimate outcome and an in-fiction one. E6 replaces it with scripted rules.
/// </summary>
public sealed class StubAdjudicator : IAdjudicator
{
    public Adjudication Rule(CombineRequest request) => new()
    {
        Effects = [new NoEffect("Nothing happened.")],
        Rationale = $"No ruler yet for {request.First.Class} with {request.Second.Class}.",
    };
}
