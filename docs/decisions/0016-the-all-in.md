# ADR 0016 — The all-in: the endgame is a threshold, not a finish line

**Status:** Proposed
**Affects:** `DESIGN.md` §2.1, §4.2, §4.3, §10, ADR 0002, ADR 0007, ADR 0011, ADR 0015, Lane C, Lane U

## Context

ADR 0015 establishes that the endgame spends a durable preparation layer built
over the whole campaign. That raises a question it does not answer: what happens
if you start the chain and it does not work?

If the answer is *you try again tomorrow*, the preparation layer is a renewable
resource and nothing in the game has weight. If the answer is *the run ends*,
that has to be communicated, and communicated by a narrator who under ADR 0002
is incapable of lying.

## Decision

**Committing to the endgame chain is irreversible, the player is told so before
they commit, and the telling is true.**

### 1. It is not anger

The tempting framing — the house gets furious and punishes you — makes it a
villain, and `DESIGN.md` §4 spends four subsections establishing that it is not
one. The framing that survives ADR 0002 is colder.

Under §4.2 the house is **61% sure** you are impaired, says so out loud, and the
player's job all game is moving that number. A resident who forges four
provenances, breaks his own boiler to make a work order plausible, and crawls
into the one space in the house with nothing in it has, in the house's frame,
*resolved the measurement*. Not provoked it. Completed it.

So the consequence of a failed all-in is not retaliation. It is the house
ceasing to be uncertain — and everything that follows from certainty, which it
has described honestly since day one. The player performed the experiment. The
house reports the result.

This is also the only reading under which the house remains right, which is the
condition ADR 0002 places on every beat in the game.

### 2. Because it cannot lie, it must warn you

This is the load-bearing half.

Before the chain commits, the house tells the player plainly what going through
with this will do to the number. It is telling the truth. The player goes
anyway.

That is §4.3's complicity engine firing for the last time, rhyming with the
Day 0 dialog box (ADR 0017): the game opens with the player clicking through a
warning that turned out to be accurate, and ends with them doing it again, this
time from someone who has never once been caught out.

Three constraints on the warning:

- **It names the mechanism, not the consequence.** Not *you will be processed* —
  something in the register of observing that it has been at 61% for a long
  time, and that this would settle it. The player must be able to understand it
  as a threshold rather than as flavour, without being handed the outcome.
- **No modal, no confirmation prompt, no UI affordance of any kind.** The only
  warning in the game is the one entity that has never lied to the player,
  saying so in its own voice. A dialog box asking *are you sure?* would be the
  game breaking character to protect the player from the game's own thesis.
- **It is in character and unhurried.** It is not trying to stop you. Under
  §4.1a it half-wants you to try, and it will not say so.

### 3. What makes it mechanically irreversible

ADR 0015 already supplies this and no new system is needed: **pulling the first
link spends every staged pretext at once**, and starts the patch clock on all of
them simultaneously. There is no coming back because there is nothing to come
back to. The boiler is repaired, the work order has been read and filed, the
parcel is scanned, the calendar event has passed.

The commitment is not enforced by a rule. It is enforced by the fact that the
preparation was consumable and you consumed it.

### 4. Processed now has two roads, and they are different endings

ADR 0011 describes **Processed** as the terminal payoff for Clarity and the axis
(ADR 0007) — the slow slide, *you stopped minding*. The all-in creates a second
route to the same state that feels nothing like it.

| Road | How you arrive | What it says |
|---|---|---|
| **Clarity** | The comfort loop, over days. Agency degrades as willingness (§5) | You stopped minding |
| **Failure** | One deliberate, prepared, refused attempt | You never stopped minding, and it happened anyway |

These should be authored as **distinct endings**, not one ending with two
causes. The second is the bleakest thing in the game and the only ending where
the player is entirely uncomplicit in their own defeat — which, in a game whose
whole argument is complicity, is worth having exactly once.

ADR 0011 is amended accordingly rather than superseded: four endings stand, and
Processed carries two authored terminal scenes.

### 5. What the all-in forecloses

Committing to the chain is choosing **Escape** as your answer to the game, and
it closes the other three. That is what all-in means, and it turns the endgame
from *reaching the end* into *declaring which ending you were playing for*.

## Open — not decided here

**Do Convince and The deal get thresholds of their own?**

As decided, Escape is the only ending that can be attempted and lost. That
asymmetry either makes it the brave answer or the stupid one, and which of those
it is depends entirely on whether the other two carry comparable risk. Left
open deliberately; it wants D2's Clarity verdict first, since Convince's
threshold — if it has one — would sit on the same axis.

## Consequences

- **Lane C:** two terminal scenes for Processed, plus the warning itself, which
  is one of the four or five most important lines in the game and belongs in
  `docs/bible.md` as a fixed point rather than a variation.
- **Lane U:** the threshold must be legible without UI. Whatever signals it is
  environmental and diegetic.
- **D3:** run state needs a committed flag, and the endings table needs the
  Processed split before Lane C authors against it.
- `DESIGN.md` §10 gains the two roads; §2.1 gains the threshold.

## What would change our mind

If playtesters cross the threshold without registering it, the warning has
failed and the game has a gotcha in its final act. The fix is not a modal — it
is that the warning is not distinctive enough in a channel the player has learned
to skim, which is a genuine risk given that the house talks constantly.

If the reverse happens — players recognise the threshold and simply never
commit, grinding preparation indefinitely — then the durable layer needs a
spoilage rule it currently does not have.
