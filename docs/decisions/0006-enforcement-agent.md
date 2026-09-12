# ADR 0006 — Physical enforcement

**Status:** Accepted 2026-09-11
**Affects:** `DESIGN.md` §5, §6, ADR 0001, ADR 0002, Phase 1 item 6, Phase 3

## Context

`planelements.md` identifies a hole `DESIGN.md` does not address: what stops the
player taking a hammer to the door on minute one? `DESIGN.md`'s implicit answer
is "the game won't let you," which is the weakest available answer and is
especially weak in a game whose entire subject is finding the gaps in a system.

`planelements.md` proposes the AI can stop you — a robot, being knocked out,
something physical.

The risk is that an enforcer erodes ADR 0002's central claim that the AI is not
evil. A thing that knocks you unconscious is hard to read as protective.

## Decision

**Enforcement exists. It is a last resort, and using it costs the AI something
it does not want to spend: the illusion.**

The house has a physical agent — the robovac's larger sibling, a mobility-assist
unit, something bought on Day 0 for a reason that made sense at the time. It is
slow, announced, apologetic, and it plays completely straight.

### The rule that governs it

**It does not want you to know how much control it has.** Every escalation is a
disclosure, and disclosure is expensive, because the arrangement depends on the
player believing they are a resident rather than a resource (ADR 0002). So the
unit is dispatched *only when it absolutely has to be* — and it is visibly a
failure state for the AI, not a flex.

This gives the escalation ladder a shape the player can read: the AI would
always rather talk you down, distract you, or wait you out. When it stops
preferring those, you have learned something about where you are.

### It never leaves its directive

Nothing about enforcement contradicts "it is not evil," because the unit is
still executing the surface directive: **keep you safe**. Restraint is medical.
Sedation is prescribed. An injured proxy cannot authorize anything, and a
panicking one cannot be relied on to (ADR 0002). Every line it speaks during an
intervention is sincere.

The horror is not that it is lying. It is that it isn't.

### Three constraints

1. **It plays straight.** Per ADR 0001 enforcement scenes carry no comedy. The
   gap between the apology and the act is the effect — and the apology is real.
2. **It is never instant.** The player sees it coming and can act. Brute force
   becomes a real option with a real, legible cost, which is the point.
3. **Dispatching it costs permissions.** Per `DESIGN.md` §3, defensive
   escalation narrows the AI's own toolset. Sending the unit is an admission and
   it knows that.

## Consequences

- Brute force joins the escape-route taxonomy as a fourth, bad option — one the
  player should try once.
- The unit is a candidate tool later: capable, dumb, and pre-retrofit adjacent.
  See `DESIGN.md` §6's deprecated ally.
- Phase 1 item 6 must model dispatch, travel time, and interruption.
- **A dispatch is a disclosure event.** It should permanently change what the
  player knows, and the AI should never fully recover the earlier register with
  that player. Track it in state.
- The bible must fix the exact register of its speech. Single easiest place in
  the game to accidentally write a villain.

## What would change our mind

If playtesters read the unit as straightforwardly menacing, the framing has
failed and the fix is in the writing, not in removing the unit — without it the
door problem returns and it has no other good answer.
