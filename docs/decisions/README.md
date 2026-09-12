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
| [0002](0002-motive.md) | The AI's motive — two layers, surface true because of the deeper | planelements / `DESIGN.md` §4 | **Accepted** **Accepted** 2026-09-11 — structure and deeper layer |
| [0003](0003-medium-and-parser.md) | Medium, views, and where the model genuinely decides | planelements / `ROADMAP.md` D0-1, 0.3 | **Accepted** **Accepted** 2026-09-11 — medium and adjudication |
| [0004](0004-failure-model.md) | Alert ladder ending in *Dropped* — a reveal, not a game over | planelements / `ROADMAP.md` D0-5 | **Accepted** 2026-09-11 |
| [0005](0005-scope-and-the-reveal.md) | Final-frame reveal: you caused all of it | planelements / `DESIGN.md` §4 | **Accepted** 2026-09-11 |
| [0006](0006-enforcement-agent.md) | Enforcement — a last resort, because disclosure is expensive | planelements | **Accepted** 2026-09-11 |
| [0007](0007-clarity-and-the-axis.md) | Clarity — compliance degrades you, and the two-ended axis | session 2026-09-11 | **Accepted** 2026-09-11 |
| [0008](0008-time-model.md) | Time model — day-structured turns, real time only under pressure | `ROADMAP.md` D0-2 | **Accepted** 2026-09-11 |
| [0009](0009-session-shape.md) | Session shape — ~12 in-game days, 5–8h, commit at day end | `ROADMAP.md` D0-3 | **Accepted** 2026-09-11 |
| [0010](0010-spatial-scope.md) | Spatial scope — one house, five levels, 12–16 spaces, one bounded yard | `ROADMAP.md` D0-4 | **Accepted** 2026-09-11 |
| [0011](0011-endings.md) | Four endings — escape, convince, the deal, processed | ADR 0009 | **Accepted** 2026-09-11 |
| [0012](0012-art-pipeline.md) | Art pipeline — pre-rendered 3D from the world schema | session 2026-09-11 | Proposed |
| [0013](0013-provenance.md) | Provenance — injection forges a source; asking never works | session 2026-09-12 | Proposed |
| [0014](0014-attention.md) | Attention — always seen, selectively understood | session 2026-09-12 | Proposed |

## D1 status — **closed**

Every structural decision is drafted and ratified. **No ADR in D1's scope remains Proposed.**

One carries a condition: **ADR 0007** is ratified as a design but the `D2 → E5`
gate still stands — Clarity is prototyped before it is built, and a bad verdict
routes back through D1 like any other decision.

Lane D's gate is open. Next is **D2**.

Three Proposed ADRs sit outside D1, all raised during D2 prototyping. None gates
D2; all three constrain D3:

- **0012** art pipeline — geometry fields on the room schema
- **0013** provenance — `source` and `trust` fields on injection vectors
- **0014** attention — the observation model splits in two, from E2

Narrative planting decisions live in [`docs/planting.md`](../planting.md), a
precursor that folds into `docs/bible.md` when D3 opens.
