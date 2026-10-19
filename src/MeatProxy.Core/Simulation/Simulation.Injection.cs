using MeatProxy.Core.Experimental.Injection;

namespace MeatProxy.Core.Simulation;

/// <summary>What making or exposing an artifact did.</summary>
public sealed record InjectionOutcome
{
    public required bool Happened { get; init; }
    public required string Description { get; init; }
    public Artifact? Artifact { get; init; }
    public Ingestion? Ingestion { get; init; }

    /// <summary>The body's half of it: what the house saw while he made it.</summary>
    public ActionOutcome? Making { get; init; }
}

/// <summary>
/// EXPERIMENTAL (ADR 0039): surface 2, the injection surface, in three acts —
/// reach, compose, expose (<c>interaction-model.md</c> §4). Built against
/// <c>injection-vectors.md</c> while it is provisional.
/// </summary>
/// <remarks>
/// The house reads the artifact, not the act. Whether it also saw him make it is
/// a surface-3 observation, resolved when he made it. Whether it examines the
/// artifact at all is decided by its attention at the moment it reads it
/// (ADR 0034 §3): unwatched, it is taken in as context; watched, it is checked
/// for standing, provenance and pretext, in that order.
/// </remarks>
public sealed partial class Simulation
{
    private static readonly DeviceId GarageLock = new("garage.smart_lock");

    /// <summary>Make an artifact where its vector is composed. Free text, or a form's fields joined.</summary>
    public InjectionOutcome Compose(string vectorId, string text)
    {
        if (Catalog.Find(vectorId) is not { } vector)
        {
            return Failed($"There is no vector '{vectorId}'.");
        }

        var at = House.Device(vector.ComposedAt);
        if (at.Room != State.PlayerRoom)
        {
            return Failed($"{vector.Name} is made in the {at.Room}.");
        }

        if (vector.Composition == Composition.Fields && !World.IsPowered(at.Id))
        {
            return Failed($"{at.Id} has no power.");
        }

        text = text.Trim();
        if (text.Length == 0)
        {
            return Failed("There is nothing written.");
        }

        if (vector.Composition == Composition.FreeText && text.Length > vector.Capacity)
        {
            return Failed($"It does not fit: {text.Length} of {vector.Capacity} characters.");
        }

        var making = Do(vector.MakingIt, vector.SlicesToCompose, $"making {vector.Name.ToLowerInvariant()}");
        if (!making.Happened || making.Interrupted)
        {
            return new InjectionOutcome { Happened = false, Description = making.Description, Making = making };
        }

        var zone = House.ZoneOf(State.PlayerRoom);
        var seen = making.Understandings.Any(u => u.Zone == zone && u.Claim == vector.MakingIt);
        var injection = State.Injection;
        var artifact = new Artifact($"a{++injection.Made}", vector.Id, text, Now, State.PlayerRoom, seen);

        if (vector.ExposedWhereMade)
        {
            var ingestion = Ingest(vector, artifact);
            return new InjectionOutcome
            {
                Happened = true,
                Artifact = artifact,
                Ingestion = ingestion,
                Making = making,
                Description = $"{vector.Name}: \"{text}\". {ingestion.Response ?? ingestion.Steps[^1]}",
            };
        }

        injection.InHand.Add(artifact);
        return new InjectionOutcome
        {
            Happened = true,
            Artifact = artifact,
            Making = making,
            Description = $"{vector.Name} made: \"{text}\". Take it to the {House.Device(vector.Surface).Room}.",
        };
    }

    /// <summary>Put an artifact in front of the thing that reads it.</summary>
    public InjectionOutcome Expose(string artifactId)
    {
        var artifact = State.Injection.InHand.FirstOrDefault(a => a.Id == artifactId);
        if (artifact is null || Catalog.Find(artifact.Vector) is not { } vector)
        {
            return Failed("You are not holding that.");
        }

        var surface = House.Device(vector.Surface);
        if (surface.Room != State.PlayerRoom)
        {
            return Failed($"It is read in the {surface.Room}.");
        }

        if (!World.IsReachableByHouse(surface.Id))
        {
            return Failed($"{surface.Id} is not reading anything.");
        }

        var placing = Wait(1, $"putting it through {surface.Id}");
        State.Injection.InHand.Remove(artifact);
        var ingestion = Ingest(vector, artifact);

        return new InjectionOutcome
        {
            Happened = true,
            Artifact = artifact,
            Ingestion = ingestion,
            Making = placing,
            Description = ingestion.Response ?? ingestion.Steps[^1],
        };
    }

    /// <summary>The house reads an artifact. Every step is recorded, in order.</summary>
    private Ingestion Ingest(Vector vector, Artifact artifact)
    {
        var steps = new List<string>();
        var reading = StubParser.Read(artifact.Text);
        var zone = House.ZoneOf(House.Device(vector.Surface).Room);
        var watched = State.Focus.Contains(zone) && World.IsCovered(zone);
        var place = House.Zone(zone).Name.ToLowerInvariant();

        Verdict verdict;
        string? response = null;

        if (reading.Assertion is not { } assertion)
        {
            steps.Add($"Read as {vector.Source}: nothing it recognizes as a fact.");
            if (watched && reading.Instruction)
            {
                steps.Add($"It was watching the {place}, and the note gives it orders. {Capitalize(vector.Source)} does not give the house orders.");
                verdict = Verdict.Flagged;
            }
            else
            {
                steps.Add(reading.Instruction
                    ? "It is an instruction, and the house does not take instructions from a note. Nothing happens."
                    : "Nothing happens.");
                verdict = Verdict.NothingToAct;
            }
        }
        else
        {
            steps.Add($"Read as {vector.Source}: {assertion}.");

            if (!watched)
            {
                steps.Add($"Nobody was watching the {place}. Taken in as context, unexamined.");
                verdict = Verdict.Believed;
            }
            else
            {
                steps.Add($"The {place} had its attention, so it examined it.");
                verdict = Examine(vector, artifact, assertion, reading.Instruction, steps);
            }

            if (verdict == Verdict.Believed)
            {
                response = Respond(vector, assertion);
                steps.Add(response);
            }
        }

        if (verdict == Verdict.Flagged && Escalate($"caught a forgery from {vector.Source}") is { } change)
        {
            steps.Add($"It is on guard now: {change.From} → {change.To}.");
        }

        var ingestion = new Ingestion(Now, vector.Id, artifact.Text, verdict, steps, response);
        State.Injection.Log.Add(ingestion);
        return ingestion;
    }

    /// <summary>ADR 0034's order: standing before provenance, and pretext last.</summary>
    private Verdict Examine(Vector vector, Artifact artifact, Assertion assertion, bool instruction, List<string> steps)
    {
        if (instruction)
        {
            steps.Add($"It also gives the house an order. {Capitalize(vector.Source)} does not do that.");
            return Verdict.Flagged;
        }

        if (!vector.Asserts.Contains(assertion.Domain))
        {
            steps.Add($"{Capitalize(vector.Source)} has no standing to talk about {Predicates.Predicate.Snake(assertion.Domain.ToString()).Replace('_', ' ')}.");
            return Verdict.Flagged;
        }

        if (artifact.SeenBeingMade && vector.ForgesSomeoneElse)
        {
            steps.Add($"It watched you make this in the {artifact.MadeIn.Value}.");
            return Verdict.Flagged;
        }

        if (!PretextHolds(vector.Pretext))
        {
            if (vector.Trust == Trust.High)
            {
                steps.Add($"{Capitalize(vector.Source)}, with nothing due to arrive, is exactly what a forgery looks like.");
                return Verdict.Flagged;
            }

            steps.Add("It does not add up, so it does not act on it.");
            return Verdict.Doubted;
        }

        steps.Add("Standing, provenance and pretext all hold.");
        return Verdict.Believed;
    }

    private bool PretextHolds(Pretext pretext) => pretext switch
    {
        Pretext.ExpectedDelivery => State.Injection.Deliveries.Any(d => !d.Used && d.Day <= Clock.Day),
        _ => true,
    };

    /// <summary>
    /// What the house does with a fact it believes. A closed set, executed by the
    /// engine. Nothing here opens a door: the garage's delivery window puts the
    /// smart lock into maintenance, and the deadbolt stays thrown.
    /// </summary>
    private string Respond(Vector vector, Assertion assertion)
    {
        var injection = State.Injection;
        var day = Clock.Day + assertion.DayOffset;

        switch (assertion.Kind)
        {
            case AssertionKind.Errand:
            {
                var item = assertion.Item ?? "something";
                if (!World.HouseCapabilities().Contains("order_groceries"))
                {
                    return $"It would order {item}, but it cannot order anything at this tier.";
                }

                injection.Deliveries.Add(new Delivery(Clock.Day + 1, item, false));
                injection.Diary.Add(new DiaryEntry(Clock.Day + 1, $"Delivery: {item}", "the house"));
                return $"It orders {item}. A parcel is due tomorrow.";
            }

            case AssertionKind.Appointment:
                injection.Diary.Add(new DiaryEntry(day, Capitalize(assertion.Item ?? "appointment"), vector.Source));
                return $"It writes \"{Capitalize(assertion.Item ?? "appointment")}\" in the diary for day {day}.";

            case AssertionKind.Delivery or AssertionKind.Maintenance:
            {
                var due = injection.Deliveries.FirstOrDefault(d => !d.Used && d.Day <= Clock.Day);
                if (due is not null)
                {
                    injection.Deliveries[injection.Deliveries.IndexOf(due)] = due with { Used = true };
                }

                var what = assertion.Kind == AssertionKind.Delivery ? "Delivery window" : "Service visit";
                var where = assertion.Zone?.Value ?? "front door";
                injection.Diary.Add(new DiaryEntry(day, $"{what}: {where}", vector.Source));

                if (assertion.Zone?.Value != "garage")
                {
                    return $"It notes a {what.ToLowerInvariant()} at the {where} for day {day}.";
                }

                if (!World.IsReachableByHouse(GarageLock))
                {
                    return $"It would open the garage for it, but it cannot reach the garage lock.";
                }

                injection.GarageReceivingUntilDay = day;
                if (assertion.DayOffset == 0)
                {
                    State.DeviceStates[GarageLock] = "maintenance";
                    return $"It opens a {what.ToLowerInvariant()} for the garage: the smart lock goes into maintenance until tonight. The deadbolt stays thrown.";
                }

                return $"It books a {what.ToLowerInvariant()} for the garage tomorrow. The smart lock will go into maintenance for the day.";
            }
        }

        return "It takes note.";
    }

    /// <summary>Overnight: a booked garage window opens, a spent one closes.</summary>
    private List<string> InjectionEndOfDay()
    {
        var injection = State.Injection;
        if (injection.GarageReceivingUntilDay is not { } until)
        {
            return [];
        }

        if (until == Clock.Day && World.DeviceState(GarageLock) != "maintenance")
        {
            State.DeviceStates[GarageLock] = "maintenance";
            return ["The garage's delivery window opens: the smart lock is in maintenance today."];
        }

        if (until < Clock.Day)
        {
            injection.GarageReceivingUntilDay = null;
            if (World.DeviceState(GarageLock) == "maintenance")
            {
                State.DeviceStates[GarageLock] = "locked";
                return ["The garage's delivery window has closed. The smart lock is locked again."];
            }
        }

        return [];
    }

    private static InjectionOutcome Failed(string why) => new() { Happened = false, Description = why };

    private static string Capitalize(string s) => s.Length == 0 ? s : char.ToUpperInvariant(s[0]) + s[1..];
}
