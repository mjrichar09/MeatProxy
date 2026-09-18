using System.Text.RegularExpressions;
using MeatProxy.Core.Perception;

namespace MeatProxy.Core.Predicates;

/// <summary>
/// Facts (<c>claims.md</c> §2). The engine owns them, they are never wrong, and
/// they are what gates anything that is gated.
/// </summary>
/// <remarks>
/// A claim is a belief and a predicate is a fact, and collapsing the two would
/// make the house incapable of being mistaken — the one thing ADR 0014 exists to
/// make it capable of. They are joined by promotion:
/// <see cref="HouseBelieves"/> holds exactly when the claim is in the salient
/// block, which is why the salient cap is the only predicate the player can
/// retract.
/// </remarks>
public abstract record Predicate
{
    /// <summary>The written form used by authored gates — <c>held(E3)</c> and so on.</summary>
    public abstract override string ToString();

    private static readonly Regex Form = new(@"^\s*(\w+)\s*\(\s*([^)]*?)\s*\)\s*$", RegexOptions.Compiled);

    /// <summary>
    /// Read a predicate back from its written form, so the Convince gate and the
    /// pretext preconditions can be authored as data rather than as code.
    /// </summary>
    public static Predicate Parse(string text)
    {
        var match = Form.Match(text);
        if (!match.Success)
        {
            throw new FormatException($"'{text}' is not a predicate. Expected kind(argument).");
        }

        var kind = match.Groups[1].Value;
        var argument = match.Groups[2].Value;

        return kind switch
        {
            "held" => new Held(argument),
            "put_to_house" => new PutToHouse(argument),
            "world" => new WorldFact(argument),
            "house_believes" => new HouseBelieves(ParseClaim(argument)),
            "run" => new RunFlag(argument),
            _ => throw new FormatException(
                $"'{kind}' is not one of the five predicate kinds in claims.md §2."),
        };
    }

    public static bool TryParse(string text, out Predicate? predicate)
    {
        try
        {
            predicate = Parse(text);
            return true;
        }
        catch (FormatException)
        {
            predicate = null;
            return false;
        }
    }

    private static Claim ParseClaim(string argument) =>
        Enum.TryParse<Claim>(argument.Replace("_", string.Empty), ignoreCase: true, out var claim)
            ? claim
            : throw new FormatException(
                $"'{argument}' is not in the closed claim vocabulary of claims.md §1.");

    /// <summary>A claim in the spelling the schemas use — <c>tidying_away</c>.</summary>
    public static string Spell(Claim claim) =>
        Regex.Replace(claim.ToString(), "(?<!^)([A-Z])", "_$1").ToLowerInvariant();
}

/// <summary>The player has found and read an evidence artifact.</summary>
public sealed record Held(string Artifact) : Predicate
{
    public override string ToString() => $"held({Artifact})";
}

/// <summary>An argument has actually been put to the house.</summary>
public sealed record PutToHouse(string Topic) : Predicate
{
    public override string ToString() => $"put_to_house({Topic})";
}

/// <summary>Something is so about the world.</summary>
public sealed record WorldFact(string Fact) : Predicate
{
    public override string ToString() => $"world({Fact})";
}

/// <summary>
/// The house is holding this conclusion in its head. The only predicate the
/// player can <em>remove</em>: push the belief out of the salient block and this
/// stops being true, without the house ever having lied.
/// </summary>
public sealed record HouseBelieves(Claim Claim) : Predicate
{
    public override string ToString() => $"house_believes({Spell(Claim)})";
}

/// <summary>A flag about the run itself, such as <c>run(assembled)</c>.</summary>
public sealed record RunFlag(string Flag) : Predicate
{
    public override string ToString() => $"run({Flag})";
}
