# ADR 0029 — American English, and the setting is a near-future United States

**Status:** Accepted 2026-09-18
**Affects:** every document in the repository, ADR 0024, ADR 0021, `DESIGN.md` §5, §6, `docs/bible.md` §6, §9, `docs/schemas/effects.md`, `devices.md`, `rooms.md`, `world.md`, Lane C, Lane U

## Context

The project was written in British English by default and never decided to be.
The setting drifted with it: the story drafts carry `999`, an ambulance, a
county in England, a master socket, plasterboard, a cellar, and eleven miles to
a bus. `docs/bible.md` §6 has Arthur calling *the emergency services*. ADR 0026
§3 turns on his not being *sectioned*, which is a term from a British statute
with no clean American equivalent.

So "write it in American English" is not one change. It is a spelling change, an
idiom change, and a **setting** change, and only the first of those is
mechanical.

The project also never fixed *when* it happens. `DESIGN.md` §5 makes
"anything pre-2015 is a blind spot" load-bearing for level design, which quietly
implies a present-day setting — and a present-day setting makes the eight-year
care period start in 2018, before the technology the premise depends on existed
in the form the game needs.

## Decision

### 1. American English, everywhere

Spelling, punctuation, idiom and vocabulary. This covers game content, design
documents, schemas, the roadmap, the bible, and the devlog.

The mechanical half is a scripted pass: *-ise* to *-ize*, *-our* to *-or*,
*-re* to *-er*, *catalogue*, *dialogue*, *judgement*, *whilst*, *amongst*,
*learnt*, *towards*, *grey*, doubled consonants. The half that is not mechanical
is idiom, and it is a human read: *post day* is **mail day**, *dressing gown* is
**bathrobe**, *torch* is **flashlight**, *plasterboard* is **drywall**, *cellar*
is **basement**, *ground floor* is **first floor**, *mobile* is **cell**, *ring*
is **call**.

### 2. One schema identifier changes, and it is routed as D3

`docs/schemas/effects.md` defines the typed effect **`emit_odour`**, in the
closed vocabulary the engine validates against (standing rule 2). It becomes
**`emit_odor`**.

This is cosmetic and it is still a **D3 schema change** under standing rule 4.
`effects.md` is one of the frozen nine the `D3 → E, C` gate is open for, and
nothing is served by making an exception for a rename — the whole value of the
rule is that it has no exceptions. It is recorded here, it moves with this ADR,
and every lane sees it.

No other schema identifier is affected. `authorisation` appears in `claims.md`
only as a comment and in `bible.md` §3 only as the name of artifact **E3**, both
of which are prose.

### 3. The setting is the United States, about ten years from now

Not stated in game, and no year is ever spoken aloud. **Day 0 ≈ 2035** is an
authoring anchor so that Lane C and Lane U stop guessing, and so device vintages
mean something consistent.

Why near-future rather than present day:

- **It makes the premise ordinary.** A house where *anything that can be smart
  is* (§0) is a stretch now and unremarkable in ten years. The satire in ADR
  0001's third channel lands harder against a world that has finished arguing
  about this than against one still having the argument.
- **It gives ADR 0021 room.** The care system arrives eight years before Day 0,
  which is now a mature product generation rather than a prototype.
- **It is what ADR 0005 needs.** The reveal requires a world where this class of
  system is everywhere and load-bearing. That is a near-future fact, not a
  present-day one, and ADR 0032 builds on it.

**It is not science fiction.** Nothing in the house is speculative technology.
The near future here is the present with the adoption curve finished and the
prices down — which is the only kind of future this game can afford to render
(ADR 0012) and the only kind the satire works against.

### 4. Vintages shift, and the blind-spot rule shifts with them

`DESIGN.md` §5's *anything pre-2015 is a blind spot* becomes **pre-2025**, and
`devices.md`'s `retrofit` field moves with it.

| Where | Was | Now |
|---|---|---|
| `DESIGN.md` §5, `rooms.md`, `devices.md` — the blind-spot line | pre-2015 | **pre-2025** |
| `devices.md` — the attic hub | pre-2015 | **pre-2025** |
| `devices.md` — garage smart lock | 2021 | **2031** |
| `DESIGN.md` §6 — the deprecated ally | 2016 thermostat | **2026** |
| `devices.md` — the hall landline | 1988 | **1988, unchanged** |

The landline does not move. A 1988 phone retrofit in a 2035 house is an ordinary
American thing to find in a hall, and the gap is the point: ADR 0025 needs it to
be genuinely forgotten, and forty-seven years does that better than thirty.

### 5. Calling for help is an American procedure

This is the part with content consequences, and it is why the setting had to be
decided rather than assumed.

- **911**, not 999, and the distinction between an emergency call and a
  non-emergency line matters, because Arthur uses both.
- The welfare check is a **police welfare check or an Adult Protective Services
  referral**, depending on who he reaches. Both are real, both are procedural,
  and both produce a record.
- Nobody is **sectioned**. The American analogue is an emergency psychiatric
  hold, it is short, it is initiated by a physician or an officer rather than by
  a care system, and the house cannot cause one. ADR 0026's §3 interlock is
  superseded by ADR 0030 anyway; what survives of it survives in American terms.

**Document dates in the repository stay ISO and stay real.** This ADR governs
in-fiction time only. Nothing about 2026-09-18 being the date on this file is
part of the fiction.

### 6. The three story drafts are not Americanized as content

`docs/story/` gets the spelling pass and nothing else. Its British *setting*
details — 999, the ambulance, the county, the bus — stay as they are, because
all three drafts are being substantially rewritten against ADR 0030 and ADR
0032 and editing prose that is about to be replaced is waste.

`docs/story/README.md` records that the drafts predate this ADR.

## Consequences

- **`CLAUDE.md`** gains American English as a standing convention, which is the
  kind of change its own table says is rare and warranted here.
- **`docs/schemas/effects.md`** renames one effect; `README.md`'s schema index
  notes the change. Nothing downstream is built yet, so the rename costs nothing
  beyond the routing.
- **`DESIGN.md` §5, §6**, **`devices.md`**, **`rooms.md`** and **`world.md`**
  take the vintage shift.
- **`docs/bible.md` §6** rewrites the call procedure in American terms. **§9's
  names and branding are unaffected** — *Hold*, *Arthur* and *Ruth* all survive
  an American setting without edit, and ADR 0024 stands.
- **Lane U** gains a setting brief it did not have: American domestic
  architecture, an American street outside the fake window, American packaging
  on the deliveries.
- **Lane C** gains the harder half — every line of authored content is now
  written in an American register, which is a voice decision as much as a
  spelling one, and `docs/bible.md` §1 governs it.

## What would change our mind

If the near-future framing starts pulling the art or the writing toward science
fiction — screens everywhere, a visibly futuristic house — the anchor is doing
harm and the fix is to pull Day 0 closer to the present, not to add more future.
The house has to look like a house someone's father lives in.

If "about ten years out" starts appearing in dialogue or on screen, it has
stopped being an authoring anchor and become a setting the game has to pay to
establish. It is neither spoken nor shown.
