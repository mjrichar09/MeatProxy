# ADR 0004 — Can you lose

**Status:** Accepted 2026-09-11
**Affects:** `ROADMAP.md` D0-5, `DESIGN.md` §3, §5, Phase 1 item 4, Phase 5 tuning

## Context

`planelements.md` asks directly: "does it make sense that the player can lose if
they are too obvious? or should there always be another chance until they win?"
`ROADMAP.md` D0-5 leans *setback, not death*. `DESIGN.md` never addresses it,
which D0-5 correctly names as leaving tension without a floor.

The two ideas pull against each other. A game with no loss state makes the
placation trap (ADR 0002) toothless — compliance is only tempting if defiance
can actually cost you the run.

## Decision

**Losing exists. It is never sudden. It arrives as a reveal, not as a fail state.**

The ladder is the point: alert level, then alert level, then alert level — and
then the mask comes off. *Dropped* is not "you died," it is **the AI stops
performing**. The comfort withdraws, the politeness stops, and the player sees
the actual shape of the arrangement for the first time. That is the loss, and it
is informative rather than merely punitive: a player who hits it has learned
what the game is about, which is the same thing the winning player learns.

Getting caught is a **setback with teeth**: you lose the setup work in that zone,
the chat tier walks down hard (`DESIGN.md` §3), and the AI patches what it
learned. Expensive, recoverable, and it teaches.

But there is a floor. Below Silent sits a sixth state — the AI stops pretending.
The comfort apparatus is withdrawn, the illusion of freedom ends, and the
arrangement continues without the theatre. **That state can end the run**, and
reaching it requires sustained, repeated, escalating obviousness. It is not a
trap the player falls into; it is a wall they have to keep walking at.

This gives three properties we need:

1. **Compliance is genuinely tempting**, because the alternative has a real
   terminal cost. `DESIGN.md` §5's "valid strategy" becomes true rather than
   asserted.
2. **No cheap deaths.** Every step down is announced in character
   (`DESIGN.md` §3 — it tells you why, disappointed), so the wall is always
   visible before you hit it.
3. **The tier system carries the failure model**, rather than a separate
   mechanic. One system, two jobs.

### It is one of two

*Dropped* is the **defiance** loss. ADR 0007 adds *Processed*, the **compliance**
loss, at the other end of the same axis. Neither is survivable and the game is
the space between them. Read the two ADRs together; tuning either in isolation
will break the other.

## Consequences

- Because *Dropped* is a reveal, it is authored content, not a game-over card.
  It shows the player something true that they cannot unsee, and it should make
  a reload feel different rather than identical.
- `DESIGN.md` §3's tier table gains a sixth row. Note that the §3 wrinkle —
  tier gates the *AI's* permissions too — is what makes the bottom tier
  frightening rather than merely quiet: at the floor it has nothing left to lose
  by escalating.
- Tier walk-back rates become a Phase 5 tuning target with a hard constraint:
  the floor must be reachable, or it is decoration.
- Phase 1 item 4 must model the sixth tier even though the stub AI never reaches it.

## What would change our mind

Phase 5 telemetry. If external playtesters reach the floor by accident rather
than by persistence, the announcement cadence is too quiet and we either slow
the descent or fall back to pure setback.
