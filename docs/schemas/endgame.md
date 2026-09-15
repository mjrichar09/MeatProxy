# The endgame — run flags and the assembly threshold

> **D3 deliverable. Status: PROVISIONAL 2026-09-15.** Blocked on D2's endgame
> verdict. Specified and marked; not to be built against yet. Implements
> ADR 0016, and the endings table Lane C authors against (`bible.md` §4).

The finale is a **threshold and a fork** (ADR 0016). Three of the five endings
resolve inside one pressure window, on one choice, under a real clock.

## 1. Run flags

```yaml
run:
  assembled: false      # the house has recognised the pattern, not just the acts
  committed: false      # the player has crossed the threshold; no way back
  all_in:
    prepared: false     # the chain is stageable from current durable state
    refused: false      # offered the deal at the threshold and declined it
  convince:
    predicates: []      # evidence-predicate ids established (bible.md §3)
  ending: null          # escape | convince | deal | processed_clarity | processed_failure
```

**`assembled` and `committed` are different flags on purpose.** The first is the
*house's* state — it has stopped reading your actions individually and started
reading them as a plan. The second is the *player's* — they have crossed and
cannot go back. A run can be assembled for days without being committed, and
those days are the tensest in the game.

## 2. The assembly threshold

The house does not notice a plan by counting acts. It notices when the acts stop
being explicable separately.

**Threshold inputs**, all deterministic engine code (no model call, ADR 0008):

| Input | From |
|---|---|
| Count of standing durable deltas | `preparation.md` |
| Count of concurrently revoked capabilities | `capabilities.md` |
| Pretexts staged and unspent | `injection-vectors.md` |
| Spread across routes — one route or three | `DESIGN.md` §2.2 |
| Recency — how much of it happened this week | run state |

**Spread is weighted hardest.** Three deltas on one route is a project. One
delta on each of three routes is a *plan*, and the house reads it as one.

When `assembled` flips, the house **says so** — honestly, once, without
threatening. It does not patch everything and does not escalate tier
automatically. It tells the player it has understood, which is worse, and from
that moment the tone of the campaign changes.

## 3. The fork

At the threshold, in one window, under a real clock:

| Outcome | Requires | Flag path |
|---|---|---|
| **Escape** | the chain holds, the window is beaten | `committed`, chain resolves |
| **The deal** | the offer taken — here, or earlier (ADR 0016's second entry) | `ending: deal` |
| **Processed — failure road** | `all_in.prepared`, `all_in.refused`, chain fails | the only uncomplicit ending |

**Convince is not on this fork.** It accrues across the campaign, is never
gambled on, and resolves later and elsewhere (`bible.md` §4). A player may arrive
at the threshold with Convince fully available and choose to walk through it
anyway; the two are not exclusive and the game does not warn them.

**No model call inside this window** (ADR 0008, ADR 0016). Everything under the
clock is engine code. The house's lines in the finale are its most important of
the game and they are *authored*, for the same reason the clock is
deterministic: because it is the one moment that must be fair.

## 4. The deal's second entry point

ADR 0016 gave *The deal* an entry sharing Escape's threshold. Mechanically: the
offer is available at the fork, and taking it there is **not** a loss state and
must not be presented as one. The game is arguing that the house is not evil
(`DESIGN.md` §10); a player who concludes that and acts on it has finished the
argument, not failed it.

## 5. Open

- [ ] **The threshold's actual weights.** D2's endgame table, run against varied
      prep states, is what sets them.
- [ ] **Does `assembled` firing feel like being caught or like being seen?** It
      must be the second. If playtesters read it as a failure state, the timing
      or the wording is wrong, not the mechanic.
- [ ] Whether a player can *deliberately* trip `assembled` early as a feint. It
      would be a wonderful move and it may be too clever to support.
