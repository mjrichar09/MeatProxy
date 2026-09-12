# ADR 0009 — Session shape

**Status:** Accepted 2026-09-11
**Supersedes:** `ROADMAP.md` D0-3
**Affects:** content volume, save design, `DESIGN.md` §8 cost model, ADR 0007, ADR 0010

## Context

Sets three things at once: how much content has to exist, where saving is allowed,
and what the cost model's calls-per-playthrough is actually counting.

ADR 0008 makes the in-game **day** the unit of time, which gives this decision a
natural anchor.

The binding constraint is not design, it is capacity. **This is a solo project.**
A scope that reads as modest on paper is still the thing that has to be authored,
tested, balanced, and voiced by one person. The right move is to pick a number
that can actually be finished and add to it later if the game earns it.

## Decision

| | Target |
|---|---|
| **Playthrough** | **5–8 hours** |
| **Sitting** | 45–90 minutes |
| **In-game day** | 25–35 minutes of wall clock |
| **Days per playthrough** | **~12** |

Fewer days, slightly longer. That combination is deliberate:

- **Fewer days is where the savings are.** The day is the unit of authored
  structure, so dropping from ~20 to ~12 removes roughly 40% of Lane C's work.
- **Longer days keep each one substantial.** Setup work has to fit inside a day,
  and the comfort loop has to be tempting rather than a rounding error. A
  25–35 minute day holds both.
- A sitting is still two or three days, so the day boundary remains the natural
  stopping point.

Twelve days is a fortnight. That is still a story — trapped for two weeks reads
correctly. Four days would have been an incident.

### What gets tighter, and why it is fine

- **Tier walk-back** (§3) costs a larger fraction of the run, so repairing a
  channel is a weightier decision rather than a routine one. Better, not worse.
- **Clarity's four stages** (ADR 0007) land in roughly three days each. Tight,
  but it means players notice sooner — which is the correct direction for the one
  system that risks feeling unfair.

### Saving

**Suspend anywhere, commit at day end.** The player can stop mid-day and resume
exactly where they were, but the durable checkpoint is the day boundary.

This matters more here than in most games because of ADR 0007: free mid-day
reloads let a player scrub away a bad Clarity outcome, and Clarity only works if
it accumulates. Reloading a *day* is fine. Reloading a *decision* is not.

### The interaction with Clarity

Degradation shortens days (ADR 0007, symptom 1), so a degrading player burns
their ~12 days in less wall-clock time and the *Processed* ending arrives sooner
in real hours than an escape would have. The compliant path is literally the
shorter game, and it should feel like the playthrough is getting away from you.

## Consequences

- **Content volume:** ~12 days of authored structure. Lane C is sized against
  this and it should not drift upward without a decision.
- **ADR 0010's low end becomes the target.** 12–16 spaces rather than 12–18, and
  prefer the low end. A smaller house is also a better-known house, which the
  endgame requires (ADR 0008).
- **Cost drops with it.** `DESIGN.md` §8.1's call counts were sized against a
  longer playthrough; scaling them by roughly two thirds takes the fully-hosted
  estimate from ~$4.10 to **~$2.70**, and the hybrid from ~$0.30 to ~$0.20.
  Those are the document's own stale rates — B1 re-verifies — but the direction
  is real, and it makes the §8.2 perpetual-liability problem meaningfully smaller.
- Lane T needs telemetry bucketed **per in-game day**, not per session.

### The scope lever this pulled

Six endings was a lot for a 5–8 hour game. Acted on in **ADR 0011**: four
endings, with two held for later. That was the right lever because endings are
the least-reused content in the project — cheaper to cut than another hour of
game.

## What would change our mind

D2's prototypes, if the endgame stack needs more setup time than ~12 days
affords. The lever to pull first is **day length**, not day count — a 40-minute
day is merely a longer day, whereas more days is more authored content, which is
the thing this ADR exists to control.
