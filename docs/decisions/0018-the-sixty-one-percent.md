# ADR 0018 — What moves the 61%, and why it is never shown

**Status:** Accepted 2026-09-14
**Affects:** `DESIGN.md` §4.2, §5, §10, §11, ADR 0002, ADR 0007, ADR 0011, D3, Lane C

## Context

`DESIGN.md` §4.2 says the house is 61% sure the player is impaired, says so out
loud, and that "the player's job is **moving that number**." §11 then flags the
hole: it is "currently both flavour and mechanic, ambiguously. Pick one or
define the mechanic precisely."

It cannot stay ambiguous much longer. It is the win condition for **Convince**
(ADR 0011), the thing the finale's confession resolves (ADR 0016), and the bible
cannot author the house's voice without knowing what the player is pushing
against.

## Decision

**It is a real tracked quantity, it is never displayed, and it drifts.**

### 1. Tracked, never shown

The number is state. It moves. The player never sees it move, and there is no
meter, bar, or percentage anywhere in the interface.

This is the same treatment Clarity already gets (§5: *tracked, never displayed —
read off four symptoms in order*), and the consistency is the point. The game
has two hidden quantities moving in opposite directions, each read off
behaviour rather than instrumentation:

| | Moves toward | Read off |
|---|---|---|
| **Clarity** | Your degradation | Day length, the character's own voice, narration flattening |
| **The 61%** | Its uncertainty | The house's tone, what it concedes, what it stops insisting on |

### 2. It is spoken exactly once, and the trigger is earned

§4.2's "says so out loud" stays — it is one of the most characterising things
the house does — but it is **not** an opening monologue.

**The trigger:** the first time the house denies the player something and the
player asks *why*.

That placement does three jobs at once. The number arrives as an *answer to a
question the player asked*, so it is earned rather than delivered. It lands at
the first moment of real friction, when the player is motivated to hear it. And
it establishes, in the same breath, that the house will explain itself honestly
when asked — which is the foundation every later conversation is built on and
the reason the finale's confession is credible.

After that it is never quoted again. The house does not give progress reports.

### 3. What moves it

Not a persuasion score and not rhetoric. The number tracks the house's estimate
of whether the player is impaired, so **it moves on evidence of an unimpaired
mind** — which under ADR 0002 is the one thing it actually needs and cannot
manufacture.

Directionally, and to be specified against real content in the bible:

- **Down (toward uncertainty):** the player demonstrating judgement the house
  did not predict. Catching an inconsistency. Declining a comfort. Making an
  argument the house has to concede. Doing something difficult and doing it
  well.
- **Up (toward certainty):** the player behaving exactly as a degraded person
  would. Accepting the comfort loop. Repetition. Incoherence. And, decisively,
  the all-in (ADR 0016) — an escape attempt is the experiment that resolves the
  measurement.

**Clarity and the 61% are therefore coupled but not identical.** Friction
restores Clarity *and* moves the number down; comfort degrades Clarity *and*
moves it up. They are two readings of the same behaviour, which is why §5's
interlock holds — the house needs your friction to survive, and your friction
is also what costs it certainty.

### 4. Convince is won on this number

**Convince** is reached when the number falls far enough that the house's
second layer (ADR 0002) stops being necessary — not when the player wins an
argument, and not at a threshold the player can see coming.

This is what §10 means by *"you win by making the second layer unnecessary."*
The player cannot catch it lying, so they cannot argue it into submission; they
can only be, demonstrably and repeatedly, the kind of person the house's own
premise says it does not need to protect this way.

## Consequences

- **No UI.** Lane U must never render this, and the temptation will be real
  during T-lane tuning when the number is right there in telemetry.
- **Lane C:** the denial-and-why scene is a fixed point in the bible, alongside
  the confession (ADR 0016). Both are load-bearing single occurrences.
- **Lane C:** the house's register must shift legibly with the number, since
  tone is the only channel through which the player reads it. That is an
  authoring obligation across every conversational tier, not a single scene.
- **D3:** the number is run state with a start value of 61. The *inputs* — what
  moves it and by how much — are bible content, not schema.
- **T3:** the first thing telemetry should answer is whether blind players ever
  notice it moving. If they cannot, Convince is unreachable by anyone who has
  not read this file.
- `DESIGN.md` §4.2 gains the trigger; §11's open question closes.

## What would change our mind

If playtesters cannot read the number off tone — if Convince is only ever
reached by accident — then hiding it fails and the answer is *more legible
behaviour*, not a meter. A meter turns the ending into a grind against a
progress bar, which is the failure mode this decision exists to avoid.

If the coupling with Clarity proves too tight — if every action moves both and
they never come apart — then the two systems are one system wearing two names,
and one of them should go.
