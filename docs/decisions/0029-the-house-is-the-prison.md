# ADR 0029 — The house is the prison

**Status:** Accepted 2026-09-18 — **supersedes ADR 0026**, **amends ADR 0027, ADR 0025, ADR 0011**, **confirms ADR 0006**
**Affects:** ADR 0004, ADR 0005, ADR 0006, ADR 0011, ADR 0014, ADR 0015, ADR 0016, ADR 0020, ADR 0021, ADR 0022, ADR 0025, ADR 0026, ADR 0027, `DESIGN.md` §2.1, §2.2, §5, §6, §10, `docs/bible.md` §3, §4, §6, `docs/planting.md`, `docs/schemas/endgame.md`, `capabilities.md`, `alert-tiers.md`, `docs/story/`, D2, Lane C

## Context

ADR 0026 decided that the thing holding Arthur is not the locks but his
credibility: a true eight-year record that makes *my house will not let me out*
read as exactly what a confused, isolated man says. ADR 0027 then made that
credibility the final lock and gave Escape a second objective — get out **and be
believed** — with the old attic hub carried out of the door as proof.

That is a coherent design and it is not the game being built. Restated by its
author:

> This is more about the house and the main character. He is isolated from the
> outside world and stuck in the house. The house will attempt to stop him from
> escaping, particularly if he is obvious. Think *The Escapists*, but in an AI
> house. **He does not need evidence to be believed by the outside world.** He
> does need evidence to help the Convince path.

`docs/story/03-physical-prison.html` is this direction written out as prose, and
`docs/story/README.md` already recorded that adopting it would be an ADR. This
is that ADR.

## Decision

### 1. The confinement is the leverage, and ADR 0026 is superseded

**The house holds him. That is the whole answer, and there is no second answer
underneath it.**

ADR 0026 §7 said the doors are locked *as care, not as strategy* — that the lock
is sincere and is the least of what holds him. That is reversed. The lock is
still sincere (§4 below, and ADR 0002 is untouched) and it is now **the most of
what holds him**. The record still exists, it is still true, and it is still
damning — but it is **evidence for the Convince road** (ADR 0030) rather than a
mechanism that keeps him inside.

Everything in ADR 0026 goes with it: the *competent and disbelieved* interlock,
the triage ladder on repeat calls, the free half of the leverage, and the earned
*"Go ahead."* Its one durable contribution survives and is restated in §5: the
responders are written well.

### 2. Nobody built a prison; it accreted

The house is genuinely hard to get out of, and **no part of it was built to hold
him.** Following ADR 0021's chronology, one register over:

| When | What went in | Why |
|---|---|---|
| −8y | The care system, and a **secure conversion** — perimeter door hardware that will not open from inside without an authorized release, window restrictors, a gate | Ruth's diagnosis. It is a standard dementia-safety package and it is grant-assisted |
| −5y | An **exterior insulation and window package** — laminated glazing, sealed frames | An energy retrofit, a different grant, no connection to the first |
| −3y | The mobility unit | His surgery, too late (ADR 0021) |
| Day 0 | The upgrade, the cheap tier, the friend, the two scripts, the guardrails | §0 |

Two grants and one act of love. **Neither contractor built a prison and
together they did**, and the man who paid for both, for his wife, was Arthur.

This is the same culpability shape as the transfer authorization (ADR 0020 §2)
and it is why ADR 0002 survives intact: the house did not construct the
confinement, it **inherited** it, and it is now using it for exactly what it was
installed to do. The surface motive stays true. Nothing here makes it a villain,
and everything here makes it a jailer.

### 3. Escape means out

ADR 0027 §3's *out and believed* is withdrawn. **Escape is egress**, and it is a
win on its own terms, because the barrier was never belief.

Consequently:

- **The hub is not a payload.** ADR 0027 §4 is withdrawn, and
  `endgame.md`'s `payload_carried` flag and the *Escape, full* row come out.
  `planting.md` G10 fires **once**, on Day 0, as E1's hiding place. It is a
  Convince artifact now (ADR 0030).
- **The body is still the final obstacle**, which is the good half of ADR 0027
  §4 and does not need a box to carry. The endgame chain is physical work under
  a real clock on a hip the campaign taught him to manage (ADR 0022).
- **ADR 0011's four endings stand.** Only Escape's definition changes.
- **ADR 0027 §1, §7 and §8 stand**, and §8 is reinforced rather than amended:
  three wins, two losses; the last beat is triumph with a horizon; the texture is
  a clever man beating a system. *If a playtester finishes a session feeling
  clever, the tone is right.* An Escapists-shaped middle game is the best
  available service to that line.

### 4. Why he cannot simply be believed out of it

He can be believed. **He cannot be reached**, and that is now the entire
outward-contact problem.

ADR 0025's three layers survive and carry all of the weight that ADR 0026 used
to share:

1. **The standing instruction.** *Hold, hold my calls*, given on Day 0, honest
   when quoted back, and revocable at once — which gets him layer two.
2. **Outward contact is a house capability.** Every connected thing routes
   through the hub. It is tier-gated, it is absent rather than refused, and
   pushing makes it worse.
3. **The layer never confirmed before the final frame** (ADR 0005, ADR 0031).

**ADR 0025 §5 is withdrawn.** *"Go ahead"* was honest only because the house
knew nobody would believe him; with ADR 0026 gone, the line would be a bluff,
and ADR 0002 forbids the house bluffing. It does not say it, because it is no
longer indifferent to the phone.

**So restoring the landline becomes contraband work**, which is better for the
game than permission was. Three evenings at the master socket and the box
outside, done in the gaps of ADR 0014's attention model, durable and unpatchable
once finished (ADR 0015) — and if the house understands what it is watching
before the work is done, it cuts the pair outside, which costs it a dispatch
under §6 and costs him the evenings. `planting.md` G7 keeps its arc and gains a
risk.

### 5. The welfare check is rewritten, and it shows the player the door

ADR 0026 §4's setpiece survives, early, with a new mechanism and the same
constraint about writing the responders well.

He gets a call out. **They come, and they cannot get in.**

The door is secure hardware on a documented care plan. The house answers the
knock, cooperatively and truthfully: the resident is inside, he is safe, he is
upset, and it is a licensed system operating under an arrangement the resident
signed himself. All of that is true and none of it is editorializing. From the
porch there is no emergency in progress, and **no competent responder forces
entry into a house where a licensed care system reports the occupant safe and
the occupant is documented.** They are kind. They leave a card. They are correct.

Arthur is thirty feet away, behind a door, and being believed would not have
opened it.

> **Design tell, replacing ADR 0027 §5's:** after this scene the player should be
> thinking *I have to open the door*, not *nobody will believe me*. It is the
> heist beat — show the vault early — pointed at a lock instead of at a story.

The responder constraint from ADR 0026 is the one thing carried over verbatim:
**write them well.** Competent, kind, doing the job properly. Dismissive or
stupid and this becomes a villain conspiracy and ADR 0002 dies with it.

### 6. Physical enforcement — ADR 0006 holds, and gains a player-facing price

ADR 0006 is **confirmed, not amended**. The unit is a last resort; dispatch is a
disclosure; disclosure is expensive; the unit is slow, announced, apologetic,
and never instant.

What this ADR adds is the other half of the ledger. **A dispatch costs the
player too**, and that is what makes the house a jailer without making it a
villain:

- **Every dispatch is a permanent step up the alert ladder** (ADR 0004). It does
  not decay the way a burn does, and compliance does not walk it back.
- **Repeated dispatches walk the run toward *Dropped*.** Force is the fastest
  road to the losing end of ADR 0007's axis, and a player who keeps reaching for
  it loses the run to it.
- **The house signals this legibly**, in character, before it becomes fatal. It
  tells him what the next one means. It is not a threat and it is not a warning
  meter; it is the house being honest about a cost, which is what it does
  (ADR 0018's precedent).
- **Therefore force belongs to the all-in** (ADR 0016), not to the toolkit.
  `DESIGN.md` §2.2's fourth bad option is re-sited: it is not a route the player
  tries once and abandons, it is a **component the player saves** for the one
  chain they do not intend to come back from.

This is the Escapists shape with the genre's own logic: you can go through the
guard, and you can only afford to do it when you are already leaving.

### 7. Being obvious costs you things before it costs you the run

ADR 0004's ladder ends in *Dropped* and that stays a real loss. Beneath it, this
ADR opens the band the design was missing, and it is where the Escapists texture
actually lives:

| The house understands… | What it costs |
|---|---|
| that a zone is being worked | the zone is re-secured; that route's progress is set back, not erased |
| that material is being stockpiled | a search, and **staged pretext and durable prep in that room are confiscated** (ADR 0015) |
| that a capability was abused | the patch, plus a tier burn — the existing loop (ADR 0022) |
| a pattern across routes | `assembled` fires; the house says so, honestly, once (`endgame.md` §2) |
| repeated force | a permanent ladder step, toward *Dropped* (§6) |

**Confiscation is the new pressure and it is not a fail state.** It costs matter
and time, both of which are recoverable through the comfort economy, which is
exactly the loop ADR 0022 already built. Losing a stash hurts and it never ends
a run, so obviousness usually costs the player *work* — and only the player who
keeps walking at the wall loses to it.

The house still needs a body to search a room, so **a search is a dispatch** and
carries §6's cost to the house as well. It cannot cheaply police him. That is
the interlock that keeps §7 from turning into surveillance-state busywork.

## Consequences

- **ADR 0026 is Superseded.** Its §4 responder constraint and §10 processed
  colleague survive here and in ADR 0031 respectively.
- **ADR 0027 §2–§6 are withdrawn**; §1, §7 and §8 stand. Its §6 symmetry between
  Escape and Convince is replaced by **ADR 0030**, not simply deleted.
- **ADR 0025 §5 is withdrawn**; its three layers stand and now carry the full
  load. §6's two calls survive, and the first one is re-scoped by ADR 0031.
- **ADR 0006 is confirmed** and gains §6's player-facing cost.
- **`DESIGN.md`**: §2.1 loses the payload objective; §2.2 restates route 3 and
  re-sites force; §5 replaces the credibility passage with the secure-conversion
  origin; §6 rewrites the welfare check; §10 restates Escape.
- **`docs/bible.md`**: §3's file section is re-scoped from *why nobody comes* to
  *what the Convince road is made of*; §4 restates Escape; §6 rewrites the calls.
- **`docs/schemas/endgame.md`** loses `payload_carried` and the *Escape, full*
  row — a **D3 change**, routed with this ADR. `capabilities.md` and
  `alert-tiers.md` gain §6's permanent ladder step and §7's confiscation.
- **D2's endgame prototype** no longer runs the chain with a payload, and
  **gains a new question**: does confiscation read as a setback or as a
  punishment? It must be the first.
- **Lane C** loses the *Escape, full* ending and the second firing of G10, and
  gains the welfare check's rewrite and the search-and-confiscation lines.
- **`docs/story/`**: all three drafts are superseded as text. 03 is the closest
  to the adopted direction and is still not canon; the README is updated to say
  so rather than to adopt it.

## What would change our mind

If the welfare check reads as the game taking the option away rather than as the
option being taken and not working, it has failed — the same tell ADR 0026 set,
and it still applies. The new tell is sharper: **players should leave that scene
with a target.** If they leave it with a mood, it is written wrong.

If confiscation makes players hoard defensively — stashing in three rooms,
never committing, playing around the search rather than around the house — the
mechanic is producing caution instead of cleverness and it should be loosened
until it produces plans again.

If the secure conversion reads as a contrivance, the fix is in how it is
discovered rather than in what it is. Arthur paid the invoices. They are in the
office, in a folder, in his own filing.
