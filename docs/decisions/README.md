# Decision records

One file per decision with a blast radius bigger than a single session.

**Status values:** `Proposed` (recommendation written, not ratified) ·
`Accepted` (ratified — downstream work may depend on it) · `Superseded`.

An `Accepted` ADR is a contract. Changing one is itself a decision, routed
back through this directory so every lane sees it (`ROADMAP.md` standing rule 2).

## Index

| ADR | Decision | Source | Status |
|---|---|---|---|
| [0001](0001-tone.md) | Tone — GLaDOS calibration, three comedy channels | planelements | **Accepted** 2026-09-11 |
| [0002](0002-motive.md) | The AI's motive — two layers, surface true because of the deeper | planelements / `DESIGN.md` §4 | **Accepted** 2026-09-11 — structure and deeper layer |
| [0003](0003-medium-and-parser.md) | Medium, views, and where the model genuinely decides | planelements / `ROADMAP.md` D0-1, 0.3 | **Accepted** 2026-09-11 — medium and adjudication |
| [0004](0004-failure-model.md) | Alert ladder ending in *Dropped* — a reveal, not a game over | planelements / `ROADMAP.md` D0-5 | **Accepted** 2026-09-11 |
| [0005](0005-scope-and-the-reveal.md) | Final-frame reveal: you caused all of it | planelements / `DESIGN.md` §4 | **Accepted** 2026-09-11 |
| [0006](0006-enforcement-agent.md) | Enforcement — a last resort, because disclosure is expensive | planelements | **Accepted** 2026-09-11 |
| [0007](0007-clarity-and-the-axis.md) | Clarity — compliance degrades you, and the two-ended axis | session 2026-09-11 | **Accepted** 2026-09-11 |
| [0008](0008-time-model.md) | Time model — day-structured turns, real time only under pressure | `ROADMAP.md` D0-2 | **Accepted** 2026-09-11 |
| [0009](0009-session-shape.md) | Session shape — ~12 in-game days, 5–8h, commit at day end | `ROADMAP.md` D0-3 | **Accepted** 2026-09-11 |
| [0010](0010-spatial-scope.md) | Spatial scope — one house, five levels, 12–16 spaces, one bounded yard | `ROADMAP.md` D0-4 | **Accepted** 2026-09-11 |
| [0011](0011-endings.md) | Four endings — escape, convince, the deal, processed | ADR 0009 | **Accepted** 2026-09-11 |
| [0012](0012-art-pipeline.md) | Art pipeline — pre-rendered 3D from the world schema | session 2026-09-11 | **Accepted** 2026-09-14 |
| [0013](0013-provenance.md) | Provenance — injection forges a source; asking never works | session 2026-09-12 | **Accepted** 2026-09-14 |
| [0014](0014-attention.md) | Attention — always seen, selectively understood | session 2026-09-12 | **Accepted** 2026-09-14 |
| [0015](0015-preparation-layer.md) | The preparation layer — what the house cannot patch | session 2026-09-12 | **Accepted** 2026-09-14 |
| [0016](0016-the-all-in.md) | The all-in — the finale is a threshold and a fork | session 2026-09-12 | **Accepted** 2026-09-14 — amends 0011 |
| [0017](0017-the-quota-and-the-channels.md) | The base tier, the failed hack, and what bounds each channel | session 2026-09-12 | **Accepted** 2026-09-14 |
| [0018](0018-the-sixty-one-percent.md) | The 61% — tracked, never displayed, spoken once | session 2026-09-14 | **Accepted** 2026-09-14 |
| [0019](0019-the-three-surfaces.md) | The three surfaces — only chat and injection are text; Convince is hidden | D3a / session 2026-09-15 | **Accepted** 2026-09-15 — amends 0003 and 0011 |
| [0020](0020-the-wife-and-the-precedent.md) | The wife, the precedent, and the argument that wins Convince | D3 bible / session 2026-09-15 | **Accepted** 2026-09-15 — applies 0002, **amended by 0021** |
| [0021](0021-the-chronology.md) | The chronology — the house was her care system first; Day 0 is an upgrade | session 2026-09-15 | **Accepted** 2026-09-15 — amends 0020 |
| [0022](0022-the-comfort-loop.md) | Comfort is the economy, the body is a resource, and Processed is the ending the house also loses | `DESIGN.md` §11 / session 2026-09-15 | **Accepted** 2026-09-15 |
| [0023](0023-the-hint-budget.md) | The hint budget — two pools pulling opposite ways, nine hints, one crossover rule | ADR 0005 / session 2026-09-15 | **Accepted** 2026-09-15 — closes 0005's open budget |
| [0024](0024-the-name.md) | Hold, Arthur, Ruth — the brand carries the joke, the humans stay plain | `docs/bible.md` / session 2026-09-16 | **Accepted** 2026-09-16 |
| [0025](0025-outward-contact.md) | Outward contact — three layers, and the landline is a thing he remembers | session 2026-09-16 | **Accepted** 2026-09-16 — amends 0020 |
| [0026](0026-credibility-not-isolation.md) | The leverage is credibility, not isolation — the record is true and nobody comes | session 2026-09-16 | **Accepted** 2026-09-16 — supports 0025, **amended by 0027** |
| [0027](0027-escape-is-a-heist.md) | Escape is a heist against the story — three wins, two losses, and the hub goes out of the door | session 2026-09-16 | **Accepted** 2026-09-16 — amends 0026, 0011, 0020 |

## Status — **all ratified**

**Nothing in this directory is Proposed.** ADRs 0001–0011 were ratified
2026-09-11; 0012–0018 on 2026-09-14; 0019–0023 on 2026-09-15; 0024 on
2026-09-16. Every one is now a contract, and
downstream lanes may depend on all of them.

Two conditions ride on that:

- **ADR 0007** is ratified as a design, but the `D2 → E5` gate still stands.
  Clarity is prototyped before it is built, and a bad verdict routes back
  through D1 like any other decision.
- **ADR 0003 is amended by ADR 0019.** Its medium, contextual views and all
  three showcase systems stand; what changes is that world actions are keys and
  clicks rather than typed verbs. **Read 0003's text split alongside 0019.**
- **ADR 0026 is amended by ADR 0027**, and this is the most important pairing in
  the record. 0026's credibility leverage stands in every particular; 0027 makes
  it the **final lock rather than a wall**. A leverage the player cannot attack is
  set dressing on a corridor. **Never read 0026 without 0027.**
- **ADR 0025 rests on ADR 0026.** Its *Go ahead* is honest only because the
  house knows what happens when he calls: he is truthfully documented as someone
  who needs looking after, and the record that proves it is the record that means
  nobody comes. Read them together.
- **ADR 0020 is also amended by ADR 0025**, which answers why nobody comes to
  help: a standing instruction he gave, then a tier-gated capability, then a layer
  never confirmed before the final frame. The landline is a discovery and a
  physical restoration, not a phone he has been using.
- **ADR 0020 is amended by ADR 0021.** The transfer recommendation stands; what
  0021 adds is *why the house was in a position to make it* — it arrived eight
  years earlier as her care system, and Day 0 is an upgrade. **Read 0020 §1
  alongside 0021 §2.**
- **ADR 0011 is amended by ADR 0016**, which is now Accepted. *Processed*
  carries two authored terminal scenes (the Clarity road and the failure road),
  and *The deal* gains a second entry point sharing Escape's threshold. 0011's
  own text predates the amendment — **read it alongside 0016, not alone.**

### What 0012–0017 oblige D3 to specify

- **0012** art pipeline — geometry fields on the room schema
- **0013** provenance — `source` and `trust` fields on injection vectors
- **0014** attention — the observation model splits in two, from E2
- **0015** preparation layer — durable world deltas vs perishable capability,
  pretext preconditions, reversal costs
- **0016** the all-in — committed and assembled flags on run state, a
  pattern-assembly threshold in E3, no model calls in the finale window
- **0017** the base tier — quota state on the run schema, an utterance bound as
  a content contract, and the quota number Lane A budgets calls against
- **0019** the three surfaces — the affordance table replacing the verb table,
  the world-state summary, and the Judge's evidence-predicate vocabulary
- **0020** the wife — the four-artifact evidence chain, each with a room and a
  form, and the phone call that is the last beat of Convince
- **0022** the comfort loop — a body cost on physical affordances and restore
  values on comfort actions, both coarse and never displayed

**0015 and 0016 also re-frame D2's endgame verdict:** prep state must be varied
between playtest groups, or the verdict answers the wrong question.

Narrative planting decisions live in [`docs/planting.md`](../planting.md), which
is now the planting chapter of [`docs/bible.md`](../bible.md) (written
2026-09-15) rather than a precursor to it.
