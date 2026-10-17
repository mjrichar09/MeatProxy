namespace MeatProxy.Core.Player;

/// <summary>
/// What an object offers when the player selects it (<c>affordances.md</c> §1).
/// The set is closed: adding one is a D3 decision, and the engine rejects a
/// record that declares anything else.
/// </summary>
/// <remarks>
/// <c>affordances.md</c> counts twelve classes because <c>open</c> and
/// <c>close</c> share a row. They are two members here because they are two acts.
/// </remarks>
public enum Affordance
{
    Examine,
    Take,
    Drop,
    Open,
    Close,
    Operate,
    WriteOn,
    PlaceInZone,
    Combine,
    Disassemble,
    PowerToggle,
    Enter,
    Listen,
}

/// <summary>What an affordance costs (<c>affordances.md</c> §4).</summary>
/// <param name="Slices">Turn time. Every affordance spends some.</param>
/// <param name="Body">
/// Whether it spends the body (ADR 0022). Coarse and invisible: it lands on
/// tomorrow, never as a number. What it does when it lands is E5's.
/// </param>
public readonly record struct AffordanceCost(int Slices, bool Body);

public static class Affordances
{
    private static readonly Dictionary<string, Affordance> ByName =
        Enum.GetValues<Affordance>().ToDictionary(Spell);

    /// <summary>The schema's spelling: <c>write_on</c>, <c>power_toggle</c>.</summary>
    public static string Spell(Affordance affordance) => Predicates.Predicate.Snake(affordance.ToString());

    public static bool TryParse(string name, out Affordance affordance) => ByName.TryGetValue(name, out affordance);

    /// <summary>
    /// <c>affordances.md</c> §4. Where the table gives a range, this is its low
    /// end: <c>operate</c> 1–2 is 1, <c>write_on</c> 1–3 is 1, <c>disassemble</c>
    /// 3+ is 3. The tuning lane owns the rest.
    /// </summary>
    public static AffordanceCost Cost(Affordance affordance) => affordance switch
    {
        Affordance.Examine or Affordance.Listen => new(1, false),
        Affordance.Take or Affordance.Drop or Affordance.Open or Affordance.Close or Affordance.PowerToggle => new(1, false),
        Affordance.Operate => new(1, false),
        Affordance.WriteOn => new(1, false),
        Affordance.PlaceInZone => new(1, false),
        Affordance.Combine => new(2, false),
        Affordance.Disassemble => new(3, true),
        Affordance.Enter => new(2, true),
        _ => new(1, false),
    };
}
