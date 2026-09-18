# ADR 0003 — Engine, medium, and how the player composes text

**Status:** Accepted 2026-09-11 — medium and showcase both ratified
**Affects:** `ROADMAP.md` D0-1 (previously *no lean*), 0.3, Phase 1, Phase 4

## Context

`ROADMAP.md` D0-1 is flagged as the decision that gates every line of Phase 1,
and it shipped with no lean at all. `planelements.md` answers it: a JRPG /
Pokémon-style top-down space with room-to-room movement, plus text commands,
with the note that a Zork-era bounded verb set may be the right limit.

That document also wants **AI to determine the effect of text commands**. This
is in direct conflict with `ROADMAP.md` 0.3's constraint — *the player composes
text, but never types a spell* — and with `DESIGN.md` §1's first rule, that no
progression gate depends solely on LLM behavior.

## Decision

**Medium:** top-down 2D, room-to-room, one house, vertically deep. Bounded,
readable, cheap to author, and it gives every injection vector from `DESIGN.md`
§2 a physical location the player walks to.

**The camera is contextual, not fixed.** Top-down is the base state — navigation,
planning, seeing which zones a camera covers. It swaps when the situation earns
it:

| View | When | Why |
|---|---|---|
| **Top-down** | Default. Moving, planning, working | Spatial legibility; you can see the sensor cones |
| **First person** | A single room or object under close work — the fuse box, the crawlspace, the whiteboard | Detail and claustrophobia. The crawlspace should feel like a crawlspace |
| **Full-screen device** | A terminal, the smart display, the router admin page, a phone | The diegetic UI *is* the screen. No frame around it |
| **Camera feed** | Whenever the AI shows you what it sees | Perspective flip. You are being looked at |

The rule: **the view changes when the player's relationship to the space
changes.** Top-down is you thinking about the house. First person is the house
happening to you. Device view is you inside the AI's medium. Camera view is you
inside its perception.

**Text handling splits three ways:**

| Surface | Input | Who resolves it | Authority |
|---|---|---|---|
| **World actions** | Bounded verb set, Zork-era | Deterministic engine | Engine |
| **Injections** (§2) | Free composition, constrained capacity | Model | Engine validates effect |
| **Conversation** (§3) | Free text | Dialogue call, screened by Guard | Never gates progression |

Verbs are **finite and discoverable**, so the player is never guessing at
phrasing to make the world respond.

---

## The showcase: where the model genuinely decides — **Accepted**

The bounded verb set is a floor, not a ceiling. The
point of building this game with a real model is to let it adjudicate things an
authored table cannot, and there are three places where that is both safe and
the best version of the mechanic.

### The safety rail that makes all of it work

> **The model proposes; the engine disposes.**

Every adjudication returns a **typed effect from a closed vocabulary** —
`damage_device`, `create_noise`, `emit_odour`, `trip_sensor`, `reveal_object`,
`no_effect`, and so on — with a target and a magnitude. The engine validates
that effect against actual world state and executes it, or rejects it. The model
never mutates state, never writes prose that becomes true, and never invents an
effect type. It reasons; the engine rules.

Second rail: because every objective has multiple routes (§2.2), **no single
adjudication is ever a progression gate**. A bad ruling costs time, not the run.
This satisfies `DESIGN.md` §1 without neutering the model.

### 1. Consequence adjudication — *Zork with a brain*

The verb set is bounded. **Object combinations are not.** `POUR bleach IN
humidifier`. `TAPE magnet TO door sensor`. `PUT phone IN microwave`.

The engine hands the model both objects' authored properties and asks what
plausibly happens. The model reasons about it and proposes typed effects. The
engine checks them and runs them.

This is the thing a hand-authored combination table can never do, it is
immediately legible to players as *the AI figured out what I meant*, and it
rewards exactly the lateral thinking the game is about. It is also the most
demo-able mechanic in the project.

### 2. Injection adjudication — *the central verb, already half-designed*

The player writes arbitrary text on a whiteboard and holds it to a camera. A
model, **prompted as the house AI's reading of that text**, decides whether it
lands, partially lands, or is flagged as an attempt.

This is already the game's core loop (§2) — it just has not been named as the
marquee AI system. It is genuine model adjudication of player creativity, and it
is the one place where *what you wrote* matters rather than *what you selected*.
Bounded by the same rail: the outcome space is the authored revocation
catalog, and there are always other routes.

### 3. Adaptive patching — *the antagonist that learns*

After an exploit lands, the model is shown what happened and asked which
**other** vectors in the catalog a competent security process would close as
well. It picks from the authored catalog; the engine applies the closures.

Cheap to run, enormous payoff: no two playthroughs get patched the same way, and
the AI stops feeling like a timer and starts feeling like an opponent with a
model of you. It is also the most honest possible expression of §2's "every
exploit is temporary."

### What stays deterministic, deliberately

Movement, inventory, locks, timers, sensor geometry, tier thresholds, and every
authored puzzle solution. If it can be a table, it is a table. The model is for
the space between the tables — which is precisely where players go when a game
invites them to be clever.

## Consequences

- `ROADMAP.md` 0.3 stands unchanged; this ADR narrows its scope to the injection
  surface only, which is what it was really about.
- The three showcase systems need a **fourth call type** in `DESIGN.md` §7.2 —
  Adjudicator — plus its cost line and its eval suite. Cheap model; the reasoning
  is short and the output is structured.
- The typed-effect vocabulary is a **0.5 schema deliverable** and a hard contract.
  Adding an effect type mid-project invalidates every eval fixture.
- Contextual views multiply Phase 4's surface area. Budget accordingly, and pick
  the first-person and device rooms deliberately rather than letting them spread.
- Phase 1 item 5 ("injection vectors as physical objects, stub parser") gains a
  sibling: the verb table, which must be frozen before Phase 3 authors rooms.
- The verb set is a **content contract**. Adding a verb in Phase 3 is a 0.5
  schema decision, not a content decision.
- A top-down medium makes `DESIGN.md` §5's attention model visible — the player
  can see which zone a camera covers. Good. It also means blind spots must be
  legible on screen without a debug view (Phase 4 exit criterion).

## What would change our mind

The 0.3 UX study. If constrained composition on a whiteboard cannot be made to
feel expressive in a top-down view, the medium may need to go first-person for
the injection scenes specifically — an expensive answer, which is why 0.3 runs
before Phase 1.
