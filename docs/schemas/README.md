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
| [`effects.md`](effects.md) | The closed typed-effect vocabulary the Adjudicator returns | **frozen** 2026-09-15 |
| [`world-state-summary.md`](world-state-summary.md) | Everything the house is, and everything the player can push out of it | **frozen** 2026-09-15 |

## Still to write

**Provisional half**, blocked on D2's verdicts:

- Injection vectors — surface, capacity, trigger, `source` and `trust` (ADR 0013)
- Capabilities and revocations, and the patch that closes each
- Durable deltas vs perishable capability, pretext preconditions, reversal costs
  (ADR 0015)
- Committed and assembled flags, the pattern-assembly threshold (ADR 0016)

**Shared open item across three schemas:** the **claim / evidence-predicate
vocabulary** — what the house may conclude you were doing, and what the Judge
tests an argument against. `sensors.md` and `world-state-summary.md` both want
it, and it may be one list.

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
```

Devices are the hinge. Everything the house can do, everything it can sense, and
everything the player can touch are three columns of one table, which is why
they cannot drift.
