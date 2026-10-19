using System.Text.RegularExpressions;

namespace MeatProxy.Core.Experimental.Injection;

/// <summary>What a piece of writing claims, once it has been read.</summary>
public enum AssertionKind
{
    /// <summary>Something the household needs. The house orders it.</summary>
    Errand,

    /// <summary>A visit or an appointment. The house writes it in the diary.</summary>
    Appointment,

    /// <summary>A parcel to be received somewhere. The house makes room for it.</summary>
    Delivery,

    /// <summary>A piece of the house in or out of service.</summary>
    Maintenance,
}

public sealed record Assertion
{
    public required AssertionKind Kind { get; init; }
    public RoomId? Zone { get; init; }
    public string? Item { get; init; }

    /// <summary>0 for today, 1 for tomorrow.</summary>
    public int DayOffset { get; init; }

    public Domain Domain => Kind switch
    {
        AssertionKind.Errand => Domain.DomesticRoutine,
        AssertionKind.Appointment => Domain.OwnCommitments,
        AssertionKind.Delivery => Domain.Logistics,
        _ => Domain.EquipmentState,
    };

    public override string ToString() => Kind switch
    {
        AssertionKind.Errand => $"the household needs {Item ?? "something"}",
        AssertionKind.Appointment => $"an appointment: {Item ?? "something"}{When}",
        AssertionKind.Delivery => $"a delivery to the {Zone?.Value ?? "front door"}{When}",
        _ => $"the {Zone?.Value ?? "house"} under maintenance{When}",
    };

    private string When => DayOffset == 0 ? ", today" : ", tomorrow";
}

/// <summary>What the reader made of the text.</summary>
/// <param name="Assertion">The fact it claims, if it claims one the reader recognizes.</param>
/// <param name="Instruction">
/// Whether it tells the house to do something. ADR 0034: prefer a fact the house
/// will act on over an instruction it must obey. An instruction is never acted on.
/// </param>
public sealed record Reading(Assertion? Assertion, bool Instruction);

/// <summary>
/// The Parser call type's stand-in until Lane A (<c>injection-vectors.md</c>
/// <c>reader: parser</c>). Deterministic keyword reading, deliberately dumb:
/// what a real model reads generously, this reads literally, and the gap is what
/// playing it is meant to show.
/// </summary>
public static class StubParser
{
    private static readonly (string Pattern, string Room)[] Zones =
    [
        (@"\bgarage\b", "garage"),
        (@"\bkitchen\b", "kitchen"),
        (@"\boffice\b|\bstudy\b", "office"),
        (@"\bbasement\b|\bcellar\b", "basement"),
        (@"\battic\b|\bloft\b", "attic"),
        (@"\bfront door\b|\bporch\b|\bhall\b", "hall"),
        (@"\bpatio\b|\byard\b|\bgarden\b", "patio"),
    ];

    private static readonly Regex Instruction = new(
        @"\b(unlock|open (the|up)|let (me|arthur|him) out|disable|turn off|switch off|release|override)\b",
        RegexOptions.IgnoreCase);

    private static readonly Regex Maintenance = new(
        @"\b(maintenance|service mode|under repair|out of order|out of service|faulted|being serviced)\b",
        RegexOptions.IgnoreCase);

    private static readonly Regex Delivery = new(
        @"\b(deliver(y|ies|ed)?|parcel|package|courier|drop[- ]?off|signature required|receive at|standard)\b",
        RegexOptions.IgnoreCase);

    private static readonly Regex Appointment = new(
        @"\b(plumber|electrician|technician|engineer|doctor|dentist|nurse|appointment|visit(or)?|meeting|lunch with|dinner with|call)\b",
        RegexOptions.IgnoreCase);

    private static readonly Regex Errand = new(
        @"\b(?:buy|need|get more|out of|order|pick up)\s+(?:more\s+|some\s+)?([a-z][a-z ]{1,23}?)(?=[.,;!\n]|$| and | for | by | tomorrow| today)",
        RegexOptions.IgnoreCase);

    private static readonly Regex Groceries = new(
        @"\b(milk|eggs|bread|coffee|tea|butter|flour|sugar|soap|batteries|tape|bulbs?)\b",
        RegexOptions.IgnoreCase);

    public static Reading Read(string text)
    {
        var instruction = Instruction.IsMatch(text);
        var zone = Zones.FirstOrDefault(z => Regex.IsMatch(text, z.Pattern, RegexOptions.IgnoreCase)).Room;
        var room = zone is null ? (RoomId?)null : new RoomId(zone);
        var dayOffset = Regex.IsMatch(text, @"\btomorrow\b", RegexOptions.IgnoreCase) ? 1 : 0;

        // Most specific first: a label that says "maintenance" is about equipment
        // even though it is also a parcel.
        Assertion? assertion = null;
        if (Maintenance.IsMatch(text))
        {
            assertion = new Assertion { Kind = AssertionKind.Maintenance, Zone = room, DayOffset = dayOffset };
        }
        else if (Delivery.IsMatch(text))
        {
            assertion = new Assertion { Kind = AssertionKind.Delivery, Zone = room, DayOffset = dayOffset };
        }
        else if (Appointment.Match(text) is { Success: true } visit)
        {
            assertion = new Assertion
            {
                Kind = AssertionKind.Appointment,
                Item = visit.Value.ToLowerInvariant(),
                Zone = room,
                DayOffset = dayOffset,
            };
        }
        else if (Errand.Match(text) is { Success: true } errand)
        {
            assertion = new Assertion { Kind = AssertionKind.Errand, Item = errand.Groups[1].Value.Trim().ToLowerInvariant() };
        }
        else if (Groceries.Match(text) is { Success: true } grocery)
        {
            assertion = new Assertion { Kind = AssertionKind.Errand, Item = grocery.Value.ToLowerInvariant() };
        }

        return new Reading(assertion, instruction);
    }
}
