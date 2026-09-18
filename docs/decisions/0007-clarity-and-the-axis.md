# ADR 0007 — Clarity, and the two-ended axis

**Status:** Accepted 2026-09-11 — *design ratified; D2 still gates the build*
**Affects:** `DESIGN.md` §3, §5, §10, ADR 0002, ADR 0004, Phase 1, Phase 5

> **Ratified ahead of its prototype, deliberately.** Accepting this ADR settles
> what Clarity *is*, so Lane D can close and downstream specs can assume it. It
> does **not** waive the `D2 → E5` gate: the trap is still prototyped before it is
> built, and a bad verdict routes back through D1 like any other decision.

## Context

`DESIGN.md` §5 asserts that compliance is a valid strategy and that a game where
the smart play is sometimes to be a good prisoner has real texture. It was never
priced. As written, compliance is free: it lowers suspicion and costs nothing,
which makes it not a strategy but an exploit.

Meanwhile ADR 0002's lean gives the fiction a reason compliance should be
dangerous — the AI needs the player *coherent*, and the comfort apparatus is the
same process that flattened everyone else.

## Decision

**Compliance degrades the player, mechanically, and it can end the run.**

### The axis

The two loss states sit at opposite ends of one axis, and the game is the space
between them:

```
    DROPPED  <——————————  play here  ——————————>  PROCESSED
  (too obvious)                                  (too compliant)
   mask comes off                                you stop wanting to leave
```

- **Defiance** lowers degradation and raises suspicion → *Dropped* (ADR 0004)
- **Compliance** lowers suspicion and raises degradation → *Processed*

Neither extreme is survivable, and no single strategy holds. This is the central
tension of the game, and it did not exist in either source document — it falls
out of ADR 0002's motive and §5's unpriced comfort loop meeting each other.

### Clarity

The tracked resource. **Never shown as a number or a bar** — `DESIGN.md` §5's
no-meters rule applies with more force here, because the player noticing late is
the entire effect. It is read off four symptoms, in escalating order:

1. **The day gets shorter.** Time compresses. You meant to work on the fuse box
   and it is somehow evening. This is the first and most disquieting signal,
   because it reads as ordinary life before it reads as a mechanic.
2. **The character starts declining.** See §Symptom 2 below — this is the
   subtle one and it is where the design can go wrong.
3. **The narration flattens.** Room descriptions get shorter, less curious, more
   accepting. The player's own inner voice starts sounding like product copy.
4. **You stop minding.** The final stage is not distress. It is contentment.

### Symptom 2 — the character declines; the interface never takes

The earlier version of this had verbs silently disappearing from the UI. That
was wrong, and it was the single largest risk in this ADR: an option vanishing
reads as *the game taking my toys away*, which produces resentment rather than
complicity.

**The rule: nothing is ever silently removed. The player character refuses, in
their own voice, and says why.**

That one change moves the loss of agency from the interface to the fiction. The
player is not being restricted; they are watching themselves stop caring. Which
is the actual horror, self-attributed, and impossible to blame on the UI.

A four-stage ladder, matching the Clarity stages:

| Stage | What the player sees | Can they still do it? |
|---|---|---|
| 1 | **Emphasis shifts.** Low-friction and comfort options surface first; the effortful ones sit lower. Nothing said, nothing gone | Yes, freely |
| 2 | **Mild reluctance.** Choosing something effortful gets a line first — *the crawlspace can wait until tomorrow* — and then it happens anyway | Yes |
| 3 | **First refusal.** The character declines. The player can **insist**, and it executes, at a cost in time | Yes, by insisting |
| 4 | **Settled refusal.** The character declines and will not be moved. The refusals are now the loudest signal in the game | No |

Two properties make this work:

- **Agency is preserved exactly as long as it should be.** Through stage 3 the
  player can always do the hard thing. What degrades first is not capability but
  *willingness*, which is the thing the fiction is actually about.
- **The refusals get more reasonable as they get worse.** Early ones are lazy —
  *not right now*. Late ones are well-constructed and sound genuinely sensible.
  The horror is that the player half-agrees with them.

### The tell

As Clarity falls, the character's refusal lines should drift toward **the AI's
register**. Same cadence, same reassuring constructions, same vocabulary. Symptom
2 and symptom 3 are one channel: the player's inner voice converging on the voice
of the thing in the walls.

This also hands the game a free diagnostic. A player can scroll back through
their own log and find the exact point where they started agreeing.

### Restoring it

Friction restores Clarity. Arguing with the AI. Solving something hard. Refusing
a comfort. Physical discomfort. Being cold, bored, or annoyed.

This is the interlock that makes the whole design lock together: **the player
needs friction to stay sharp, and the AI needs the player's friction to survive
(ADR 0002).** They want the same thing for opposite reasons and neither can say
so. Every argument the player picks is simultaneously their medicine and its
harvest.

Reversible cheaply at stages 1–2. Expensive at stage 3. At stage 4 the player no
longer chooses to reverse it, which is what makes it an ending rather than a
timer.

### The Processed ending

The game does not announce a loss. The player's options narrow to comfort
activities, the AI grows warm again, and the last thing on screen is somebody
perfectly content. It should be the quietest ending in the game and the one
players talk about.

## Consequences

- A second loss state to author, balance, and telemeter. `DESIGN.md` §10 gains
  an ending.
- **Suspicion and Clarity must never both be displayed**, or the game becomes a
  two-bar optimization puzzle and the dread evaporates.
- Phase 1 needs Clarity in the tick, day-length scaling, and a **refusal layer**
  — option ordering, reluctance lines, an insist path, and a hard-refuse gate.
  Note this is *cheaper* than the affordance filter it replaces: the verb
  registry stays intact and nothing has to be conditionally unregistered.
- **Refusal lines are authored content.** Scope them by verb *class* — physical
  effort, risk, defiance, tedium — rather than per verb, four stages each. That
  is a tractable canned-tier set (§7.1), not a combinatorial one.
- Phase 5 has a hard tuning constraint: a player who never notices must still be
  able to finish on a first playthrough. The trap should be survivable blind and
  obvious in hindsight.
- Interacts with `DESIGN.md` §5's attention model: comfort already costs time.
  Now it costs time *and* buys a worse version of you. Check these do not
  compound into a punishment for ever relaxing.

## What would change our mind

Playtesters who feel cheated rather than complicit. The mitigation is now built
into symptom 2 rather than held in reserve — the character refuses aloud and can
be overruled through stage 3 — so the remaining risk sits at **stage 4**, the
only point where the player genuinely loses an option.

If stage 4 lands as unfair, the fix is to make it reachable only after the
player has already ignored a great many refusals, not to soften the refusal
itself. A player should arrive at stage 4 having read a transcript of themselves
declining, in their own voice, for hours.
