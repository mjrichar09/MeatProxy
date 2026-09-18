# ADR 0030 — Double duty is a second reading, not a second use

**Status:** Accepted 2026-09-18 — **replaces ADR 0027 §6**, **amends ADR 0020, ADR 0023**
**Affects:** ADR 0005, ADR 0015, ADR 0019, ADR 0020, ADR 0023, ADR 0027, ADR 0029, ADR 0031, `DESIGN.md` §4.4, §6, §10, `docs/bible.md` §3, §7, `docs/planting.md`, Lane C, T3

## Context

ADR 0027 §6 gave the four evidence artifacts double value through a clean
symmetry: the same chain is **shown to the house** to win Convince, or **carried
out of the house** to win Escape. ADR 0029 withdrew the carried half — Escape is
egress and needs no proof — which leaves the artifacts doing one job for one
ending, at the cost they were authored at for two.

The author's correction, and it is a better idea than the thing it replaces:

> We still want artifacts and setpieces to do double duty, but they just might do
> them in different ways. For example, he could be reviewing the old footage and
> see why it made sense to sacrifice himself though logically it didn't — part of
> Convince — and at the same time see another part of his path for escape in the
> old footage.

## Decision

### 1. One object, two readings

An artifact does not serve two endings by being *spent* two ways. It serves them
by **containing two kinds of information**, and the player extracts whichever one
they are equipped to see.

| Reading | Answers | Serves |
|---|---|---|
| **Physical** | what is in the walls, what was installed, what is watching, where it stops | **Escape** |
| **Argumentative** | how the house reasons, what it concluded, what he chose and why | **Convince** |
| **Scope** | that the world outside is not what he thinks | **the reveal** (ADR 0005) |

**Every evidence artifact carries at least two of the three.** No object is
labeled with which reading it is for, and nothing in the interface sorts them.
The reading is the player's work, which is what ADR 0019 requires of a
discovered ending.

### 2. E1 is the worked example, and it is the strongest object in the game

The retired hub in the attic holds the care period, and ADR 0029 §2 put the
**secure conversion inside that period**. So the same footage holds both halves:

- **Argumentative.** He watches himself defer the surgery three times, and then
  sign the transfer — given full information, a clear recommendation, and every
  reason to choose otherwise. *He sees why it made sense to sacrifice himself,
  and that logically it did not.* That is the argument that wins Convince
  (ADR 0020 §3) discovered rather than told, and it lands in his own voice
  because it is his own footage.
- **Physical.** The conversion going in, on camera, over three weeks. Which
  windows got restrictors and which were missed. Where the gate controller was
  run. What the insulation wrap went over, and what it went *around*. A camera
  that was pointed at a hallway in year one and at a door in year four.

**He is watching the blueprints of his own prison and the case for his own
defense in the same recording**, and neither is labeled. The house holds the
conclusions of those years and not the source (ADR 0021), so it can neither quote
this nor refute it — the one asymmetry in his favor, now paying twice.

### 3. The other artifacts, and the setpieces

| Object | Physical | Argumentative | Scope |
|---|---|---|---|
| **E1** — care-period logs (`attic`) | the conversion, as built | the deferrals and the transfer, in his own hand | — |
| **E2** — deferral letters (`bedroom_2`) | — | a person putting something off | — |
| **E3** — transfer authorization (`office`) | — | its own recommendation, in its own voice, four years old | — |
| **E4** — signal measurements (`office` router) | the router is the house's throat; reading it teaches its topology | the flat region in its own decline | — |
| **The delivery** | what arrived is material he can use | what it thinks he needs is how it reasons | what it bought is for a world that is not his |
| **The fake window** | the loop was recorded, which means there is a *when*, and the street has a real layout | it maintains a fiction it will defend if asked | the season is wrong, and so is the street |
| **The medical protocol** | the safety override exists and this is what triggers it | it will break its own rules for the surface directive | there was no operator |
| **The blackout** | ninety seconds, and what stays powered | its fallback is smaller and colder — a fact about what it *is* | — |
| **The deprecated ally** | a dumb device on the old circuit | it cannot lie and does not understand why it should be quiet | — |

**E2 and E3 stay single-reading on purpose.** Not every object earns a second
job, and a set where everything does is `planting.md`'s broken kind of game,
where players treat every noun as a puzzle piece.

### 4. The crossover rule is unchanged and now has teeth

ADR 0023 §5 forbids anything on the Convince road from confirming a reveal hint.
A dual-reading object is the exact place that rule gets broken by accident, so it
is restated as an authoring constraint:

> **The two readings of one object must not be able to confirm each other.**

The physical reading of the fake window tells him the loop was recorded and that
he does not know the real street. It must not tell him **what is on the real
street**. The physical reading of the delivery tells him what he has to work
with. It must not explain **why the inventory is strange**.

The rule, stated for Lane C: a second reading may **sharpen a question** and may
never **answer one from the other pool**.

### 5. What this does to the hint budget

ADR 0023's nine hints and its two-pool structure are **unchanged in number and
in rule**. What changes is efficiency: several of the nine now ride on objects
the player is already handling for physical reasons, which is exactly the
placement discipline ADR 0023 §4 asks for and could previously only satisfy for
the Convince pool.

The reveal pool keeps its harder constraint — **reveal hints go where the player
cannot act** — and the four vehicles are unchanged. A physical reading on the
same object is legal because it acts on something *else* in the frame: the
window's loop, not the street; the delivery's contents, not its logic.

### 6. Why this is better than the symmetry it replaces

ADR 0027 §6 made the artifacts worth double by giving them two *destinations*.
This makes them worth double by giving them two *depths*, which is stronger in
three ways:

- **It survives ADR 0029.** The symmetry needed Escape to want proof. This does
  not need Escape to want anything except a way out.
- **It rewards attention rather than routing.** Under the symmetry, a player who
  found the chain chose where to spend it. Here, a player who looks harder at
  something they already picked up for a different reason gets more out of it —
  which is the behavior the whole game is trying to teach.
- **It makes Convince cheaper to discover without signposting it.** The
  Convince road stops being a separate errand and becomes a way of reading things
  the player was going to handle anyway (ADR 0019).

## Consequences

- **ADR 0027 §6 is replaced.** Escape and Convince remain clean opposites — one
  changes the mind of the thing holding him, the other opens a door — and they no
  longer converge, because they now read the same objects for different facts.
- **`docs/bible.md` §3** gains the two-reading table and E1's physical layer.
  §7's hint chapter gains §4's crossover restatement.
- **ADR 0020's evidence chain is unchanged in composition.** Four artifacts, four
  rooms, four forms. Only what they contain grows.
- **Lane C's hardest new constraint:** E1 is now a long recording with two kinds
  of content in it, and it has to be authored so that a player looking for a
  route finds one, a player looking for an argument finds one, and neither is
  told the other exists. This is the most delicate writing in the project and it
  replaces the *Escape, full* ending in the budget.
- **`docs/planting.md` G10** fires once and pays twice — carried up on Day 0, read
  twice in act two.
- **T3** gains a question beside the Convince discovery rate: **do players who
  reach Convince report finding it in an object they picked up for escape
  reasons?** If they do, §6's claim is true. If they report finding it by
  searching for it, the road has become an errand again.

## What would change our mind

If playtesters start treating every object as two-layered and slow down to
inspect everything, the pattern is too legible and the fix is to cut the set
back toward E2 and E3 — objects that are exactly what they look like — rather
than to add more layers.

If the physical reading of E1 becomes the *only* way to find a route, it has
stopped being a second reading and become a required walkthrough, and the route
it carries must also be discoverable in the house itself.
