# Schemas

The interface contract between Lane E (engine), Lane A (AI layer) and Lane C
(content). D3's deliverable. Get these wrong and all three churn.

**Frozen** means nothing D2 can say would move it, and E and C may build against
it. **Provisional** means specify it, mark it, and do not build against it until
D2 reports.

| Schema | What it fixes | Status |
|---|---|---|
| [`rooms.md`](rooms.md) | 15 spaces, five levels, what each earns its place by | frozen · draft 2026-09-14 |
| [`world.md`](world.md) | Identifiers, levels, room geometry for the Blender build (ADR 0012) | **frozen** 2026-09-15 |
| [`devices.md`](devices.md) | One record read three ways — the house's powers, its senses, the player's targets | **frozen** 2026-09-15, one provisional field |
| [`sensors.md`](sensors.md) | Detection vs interpretation, channels, observation zones (ADR 0014) | **frozen** 2026-09-15 |
| [`alert-tiers.md`](alert-tiers.md) | Six tiers, both directions, the softlock invariant | frozen · draft 2026-09-14 |
| [`interaction-model.md`](interaction-model.md) | The three surfaces at one interface (ADR 0019) | frozen · draft 2026-09-15 |
| [`affordances.md`](affordances.md) | What an object offers when selected — replaces the verb table | **frozen** 2026-09-15 |
| [`effects.md`](effects.md) | The closed typed-effect vocabulary the Adjudicator returns | **frozen** 2026-09-15 · `emit_odour` → `emit_odor` 2026-09-18 (ADR 0028) |
| [`world-state-summary.md`](world-state-summary.md) | Everything the house is, and everything the player can push out of it | **frozen** 2026-09-15 |
| [`claims.md`](claims.md) | What the house may conclude you were doing, and what the Judge tests an argument against | **frozen** 2026-09-15 |

## Provisional — written, marked, **not to be built against**

Blocked on D2's verdicts. Specified so the shape is known and the dependencies
are visible; E and C wait.

| Schema | What it fixes | Blocked on |
|---|---|---|
| [`injection-vectors.md`](injection-vectors.md) | The exploit catalog — 15 vectors, `source`, `trust`, pretext preconditions | D2's injection-surface verdict. **The most exposed document in D3** |
| [`capabilities.md`](capabilities.md) | Twelve capabilities, never one route each, and the patch clock | D2's endgame verdict |
| [`preparation.md`](preparation.md) | Durable deltas, the four currencies, reversal costs | D2's endgame verdict, run against **varied prep state** |
| [`endgame.md`](endgame.md) | `assembled` vs `committed`, the assembly threshold | D2's endgame verdict |

The cheapest partial retirement is still the injection surface: one sitting
against `prototypes/d2/index.html` sets the 50-character bound and unblocks the
catalog for Lane C.

**The shared open item is closed** — [`claims.md`](claims.md), 2026-09-15. It
turned out to be **two** vocabularies rather than one: claims are beliefs and can
be wrong, predicates are facts and cannot. Promotion to salient is the bridge
between them, which is what makes the context exploit able to retract a belief.

## How they fit

```
  world.md ──> rooms ──> devices ──> senses ────> sensors.md ──> understandings
                  │          │                                        │
                  │          ├─ house_capabilities ──> alert-tiers.md │
                  │          └─ player_affordances ──> affordances.md │
                  │                                         │         │
                  └─────────────────────────────> combine ──┘         │
                                                      │               │
                                                 effects.md           │
                                                      │               │
                                                      └──> world-state-summary.md
                                                                │
                                                       claims.md ─┘
                                                    (claim -> predicate
                                                     on promotion)
```

Devices are the hinge. Everything the house can do, everything it can sense, and
everything the player can touch are three columns of one table, which is why
they cannot drift.
