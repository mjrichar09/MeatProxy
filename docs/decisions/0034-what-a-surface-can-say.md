# ADR 0034 — What a surface can say, and who is looking when it says it

**Status:** Accepted 2026-09-18
**Affects:** `DESIGN.md` §2, `docs/schemas/injection-vectors.md`, `docs/schemas/sensors.md`, ADR 0013, ADR 0014, D2, Lane C

> **Numbering note.** Drafted as 0029 and renumbered on landing. 0028 is the
> engine decision, and 0029–0033 are the premise pass that merged the same day.
> Nothing in this ADR moved with the renumber.

> **Relation to ADR 0030.** This is a mechanism decision and it does not move
> with the premise. Standing, the second order and scrutiny at ingest describe
> *how* a forged fact is believed, which is unchanged by whether the leverage
> is credibility or the confinement itself. What ADR 0030 does touch is the
> catalog's **objectives** — whether a given exploit still opens anything now
> that the perimeter hardware is real — and that is a question for
> `capabilities.md`, not for this ADR.

## Context

`injection-vectors.md` models a vector as a place text enters the house's
reading, bounded by `capacity` and believed according to `source` and `trust`.
That record survived D3 and does not survive the first serious attempt to author
against it.

The worked example that broke it: **inject a calendar event using the parcel
label.** The only way to do that under the current schema is to write something
like `Garage Maintenance at 4PM - unlock door` into the recipient field. It
satisfies every field in the record — a real surface, a high-trust source, a
staged pretext, inside capacity — and it is obviously wrong. A shipper has no
standing over your calendar. The record has no way to say so.

Three gaps, all visible in that one example:

1. **Nothing binds a payload to its source's subject matter.** `trust` says how
   much the house believes the mouth. Nothing says what the mouth is *about*.
2. **`capacity` is one number** across surfaces that compose completely
   differently — a label's recipient field, a fridge full of magnets, a marker
   on a whiteboard.
3. **Scrutiny at ingest is unspecified.** ADR 0014 split always-on detection from
   scarce interpretation, but injection was never folded into that split, so
   whether forged provenance gets *caught* was left to unstated logic.

## Decision

### 1. Standing — a source may only assert what that source is about

Every vector gains **`asserts`**: the domain of facts its forged source has
standing over, from a closed vocabulary.

| Source | Asserts |
|---|---|
| Carrier / shipper | Logistics — contents, recipient, delivery window, access required |
| Pharmacy | Medication — dosage, schedule, interaction, supply |
| Technician | Equipment state — fault, part, isolation, return visit |
| Broadcast | Whatever is on screen, and nothing about this house |
| Household note | Domestic routine — who is out, what is needed, when |
| The player, visibly | Nothing they could not simply say out loud |
| The old hub (v15) | **Anything.** It is the house's own younger self |

**An assertion outside its source's domain is not a weak injection. It is a
flag**, and a worse outcome than saying nothing — because the house knows
exactly what a shipping label is for, and one that schedules a garage
appointment is a shipping label that somebody edited.

This is the field that makes the catalog legible. A vector stops being a
generic slot with a trust score and becomes **a mouth with a subject**.

### 2. Prefer the second order — state a fact, let the house act on it

The strongest injections carry no instruction. They carry a **fact the house's
own automation converts** into the state the player wanted.

The label does not say *unlock the garage*. It declares a delivery window of
16:00–18:00 requiring garage access — which is entirely in the carrier's voice —
and **the house writes the calendar entry itself**, because that is what it does
with delivery windows.

Three reasons this is the better shape, and they compound:

- It satisfies standing, because it never leaves the carrier's subject.
- The house takes the action on its own initiative, so **there is no instruction
  anywhere in the evidence chain** (ADR 0005).
- It finally serves ADR 0013's third reason properly. *A successful injection
  tells it nothing about you* is true of a forged fact in a way it is never
  quite true of a forged instruction.

Instruction-shaped payloads still exist. They are the loud, low-trust end of the
catalog, and the whiteboard is where they live — which is consistent with ADR
0013 having already demoted it.

### 3. Scrutiny at ingest is the reading zone's focus level

An injection is read **by a zone**, and vectors gain **`read_zone`** to say
which.

- **Zone holds a focus slot at ingest** → the payload is *interpreted*. Standing
  is checked, pretext is checked, provenance can fail.
- **Zone holds no focus slot** → the payload is *ingested*. It is believed as
  context, exactly the way ADR 0013 says context is believed.

This folds injection into ADR 0014's economy instead of running it alongside.
Two things fall out that the design has been missing:

- **The schema's own claim becomes true.** `injection-vectors.md` already says
  *timing exposure against attention is the skill*. Until now that was a
  sentence with no mechanism under it.
- **Attention diversion gains a second job.** It currently only stops you being
  understood. Now it is also how you land an exploit, which makes the everyday
  verb matter on the days nothing is happening.

It also generalizes a rule the record had already written for exactly one
vector. ADR 0013 says the whiteboard works when the house reads it *"without
attributing it to you: left in frame while its attention is elsewhere."* That is
this rule, stated once and never applied to the other fourteen.

**The obvious failure mode is that every injection becomes trivially landable by
looking away.** Three counter-pressures, which need to hold together or part 3
is broken:

- The house moves focus on its own logic — anomaly, schedule, suspicion
  (`sensors.md` §2) — and **staging a pretext is itself detectable**. The boiler
  you broke is an anomaly that pulls focus toward the boiler.
- **High-standing sources are read in zones the house cares about.** The parcel
  scanner is in the office, which is also the highest injection density in the
  house (`rooms.md`). Strong provenance and low attention are hard to get at once.
- **`trigger: on_focus` vectors require focus by definition.** Those are the ones
  where the player *wants* it looking, and they trade scrutiny for reliability.
  A catalog with both kinds has a real decision in it; a catalog with only
  the first is "always distract".

### 4. Two failure modes, two voices — clear about suspicion, vague about mechanism

The house's response to a refusal splits:

- **Suspicion** — it believes it is being manipulated. It says so plainly. This
  has to be legible: the player cannot learn the trust ladder or the patch clock
  from silence, and a system that punishes without saying so is unlearnable.
- **Strangeness** — the request is merely outside routine, with no suspicion
  attached. Here it is vague about why. Precision teaches the player to
  reverse-engineer the rules, which turns the house into a puzzle box and the
  game into lock-and-key.

Both are honest, so ADR 0002 holds. **Vagueness is not a lie** — it is a house
that does not explain itself, which is also better characterization than one
that files a reasoned denial every time.

## Consequences

- **`injection-vectors.md` gains `asserts` and `read_zone`**, and `capacity`
  becomes per-surface rather than one figure.
- **The free-composition capacity moves 50 → 100.** This is a better working
  figure, **not a verdict** — D2's sitting has not happened and the schema stays
  PROVISIONAL. The prototype currently caps its whiteboard at 80, which is a
  third number and should be brought to 100 before the sitting.
- **The chat utterance bound is a different 50 and is unchanged** (`DESIGN.md`
  §3, ADR 0017). The two were always separate and are now easy to confuse, so
  they are named differently: *capacity* is injection, *utterance bound* is
  speech.
- **All 15 catalog vectors need an `asserts` domain and a `read_zone`.** Lane
  C, after D2. Vector 2 (shipping label) and vector 7 (calendar entry) are the
  worked pair and should be authored first — they are the two halves of the
  example that forced this ADR.
- **The Parser gains a standing check before its provenance check**, and returns
  which of the two failed, because §4 needs to tell them apart.
- **ADR 0014's focus slots gain a second consumer.** E2's exit criterion already
  requires detection and interpretation to be distinguishable in the debug view;
  it now also has to show which one read an injection.
- Extends ADR 0013 and ADR 0014. Contradicts neither.

## What would change our mind

- **If D2 testers reach for attention-timing unprompted**, part 3 is discoverable
  and right. If they never once think about where the house is looking, the
  mechanic is invisible, and the fix is surfacing it in the interface — not
  explaining it in dialogue.
- **If standing makes the catalog feel like an inventory puzzle** — this label
  goes in this slot — the domains are drawn too narrowly. The test is whether a
  player can invent a use we did not author.
- If part 2 makes every good injection indirect to the point of being unguessable,
  the second order is a ceiling rather than a default, and the catalog needs
  more first-order vectors at the low-trust end.
