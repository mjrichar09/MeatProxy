# ADR 0008 — Time model

**Status:** Accepted 2026-09-11
**Supersedes:** `ROADMAP.md` D0-2
**Affects:** `DESIGN.md` §5, §7.1, ADR 0007, Lane E entirely, Lane U

## Context

`DESIGN.md` §11 flagged this as affecting all of §5, and it does. Four systems
each want a different answer:

| System | What it needs from time |
|---|---|
| **The patch cycle** (§2.1) | Exploits close within ~1 hour of *game* time |
| **Attention** (§5) | Working windows of ~40 minutes in a blinded zone |
| **Clarity** (ADR 0007) | Days as a unit, with a shrinkable budget inside them |
| **The endgame stack** (§2.1) | Genuine live pressure — a chain closing behind you |
| **Tier walk-back** (§3) | *Days* of good behaviour, measured in days |
| **Overnight batch** (§9.2) | A sleep boundary, for half-price non-realtime calls |

Everything except the endgame wants structured, discrete time. The endgame wants
a clock you can hear.

## Decision

**Day-structured turn economy, with real time confined to declared pressure
windows.**

### The day is the unit

Each in-game day carries a budget of **time slices**. Actions cost slices.
Sleeping ends the day, banks the AI's overnight review (§9.2), and advances tier
walk-back.

This makes two things concrete that were previously hand-waved:

- **Comfort activities cost something countable.** §5 asserts the comfort loop is
  a real trade; slices are what make it one.
- **Clarity's first symptom becomes mechanical rather than cosmetic.** "The day
  gets shorter" (ADR 0007) *is* a shrinking slice budget. The player experiences
  it as time slipping; the engine implements it as a smaller number.

### Turn-based by default

Take an action, the world ticks, the AI may react. No pressure while thinking.
This is correct for a game whose central verb is composing text (§2) — a player
writing on a whiteboard should never be racing a clock, because that punishes the
exact activity the game wants them to enjoy.

### Pressure windows are the exception

When a timer is genuinely running — a blinded camera zone, a fake delivery slot,
the endgame chain — the game shifts to real time and says so. The shift is
explicit, diegetic, and visible.

**The rule that keeps this honest:**

> **Composition happens in turn time. Execution happens in pressure time.**

You *write* the injection at your leisure. You *run* the chain against a clock.
The endgame is exhilarating because everything was prepared calmly and is now
being spent quickly.

### The constraint this puts on Lane A

> **No pressure window may depend on a model call.**

Latency is diegetic (§7.1) — rack hum and light flicker — and that works fine in
turn time, where waiting is atmosphere. Inside a pressure window it is unfair:
the player loses seconds to inference they cannot control or predict.

So every system that runs during a pressure window must be **deterministic engine
code**. Model calls happen between windows, not inside them. This falls straight
out of standing rule 1 and it is a hard architectural boundary, not a preference.

## Consequences

- Lane E needs slices in the tick from **E1**, not bolted on at E5.
- The endgame stack (D2's first prototype) must be paper-prototyped **with a real
  clock**, because pressure is the thing being tested.
- Lane U has two presentation modes to build, and the transition between them is
  a designed moment rather than a state change.
- Adjudicator calls (ADR 0003) are turn-time only. An improvised combination
  cannot be attempted mid-chain — which is a real design constraint on the
  endgame, and probably a good one: the chain should be executed, not improvised.
- Attention windows (~40 min) and patch timers (~1 hour) need to be expressed in
  slices, so the slice-to-minute ratio is a D3 schema number.

## What would change our mind

The D2 endgame prototype. If real-time pressure makes the stack feel frantic
rather than tense — people fumbling rather than executing — the fallback is a
**visible tick-down in turn time**: each action advances the closing timers, so
hesitation still costs, but reading and thinking do not. That preserves the
pressure without the reflex test, and it is a smaller change than it sounds.
