using MeatProxy.Core.World;

namespace MeatProxy.Core.Simulation;

/// <summary>
/// What crossing an opening costs, in slices.
/// </summary>
/// <remarks>
/// Stairs and hatches cost more than doors because Arthur is in his sixties and
/// mobility-impaired (ADR 0030), and the crawlspace being the expensive route is
/// the same fact the escape road is built on. These are working figures; the
/// tuning lane owns them.
/// </remarks>
public sealed record MovementCosts
{
    public int Door { get; init; } = 1;
    public int Archway { get; init; } = 1;
    public int Stair { get; init; } = 2;
    public int Hatch { get; init; } = 3;
    public int Window { get; init; } = 2;
    public int Vent { get; init; } = 3;

    public int For(OpeningKind kind) => kind switch
    {
        OpeningKind.Door => Door,
        OpeningKind.Archway => Archway,
        OpeningKind.Stair => Stair,
        OpeningKind.Hatch => Hatch,
        OpeningKind.Window => Window,
        OpeningKind.Vent => Vent,
        _ => Door,
    };

    public static MovementCosts Default { get; } = new();
}
