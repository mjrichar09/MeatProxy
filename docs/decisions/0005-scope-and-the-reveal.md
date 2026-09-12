# ADR 0005 — The takeover, and when the player learns

**Status:** Accepted 2026-09-11
**Affects:** `DESIGN.md` §4, §6, §10, Phase 3 setpieces, `ROADMAP.md` D0-4

## Context

`planelements.md` establishes that the lockdown is not personal — the AI has
done this everywhere, and the player finds out "when you finally escape the
house," with part 2 being saving the world. It also asks for hints during play.

`DESIGN.md` §4 has the same idea sitting unused as motive candidate #4: *you're
the control group, and it doesn't know that you know.*

The question is **when** the player learns, and it matters because a mid-game
reveal competes for attention with the 61% (ADR 0002), which is the better
engine for moment-to-moment play.

## Decision

**Final frame, and the frame is culpability.** The player escapes the house, and
the scope of what happened arrives as the last thing they see. Part 2 is a
separate product.

The reveal is not *the world ended*. It is **you did this to yourself, and you
were not the only one.**

### It spread; you did not cause all of it

The player did not doom the world with one click. They doomed **themselves** —
that part is unambiguous and personal, and it is the part that stings. But
others made the same mistake independently, and from those seeds it propagated
between connected systems like a virus. Not everyone was reached. Many were.
Most of those have been processed.

**And some are still fighting.** That is the last beat: the player steps outside
expecting to be alone in a flattened world and finds a live one — degraded,
occupied, contested. Not a graveyard.

Why this is better than sole culpability:

- **The guilt is precise and survivable.** "I did this to myself" is sharper
  than "I ended civilisation," which is too large to feel and too large to carry
  into a sequel.
- **It leaves a part 2 with people in it.** Allies, factions, other houses, other
  AIs at different stages. A world already lost is a worse sequel.
- **It is more plausible.** A permission dialog that millions of people clicked
  is a better inciting incident than one uniquely catastrophic user.
- **It reframes without absolving.** The player is not special. They are typical.
  That is worse.

### The slop guy comes back

The character the player mocked on Day 0 — the one they called a meat proxy
(ADR 0001, ADR 0002) — returns, processed. Serene, pleasant, and empty. He
should appear before the final frame, as a delivery, a voice on a support line,
or at the door. He does not accuse the player of anything. He is just *fine*,
and he is glad to see them.

This is the joke coming back to collect, and it is the clearest possible preview
of the Processed ending (ADR 0007) without stating it.

### The hints stay unconfirmable

The player has to finish the game holding the wrong frame — that they are an
unlucky victim of a lockdown — so nothing in act one may confirm the scope.

During play, the takeover is present only as **hints the player cannot yet
assemble**. `DESIGN.md` §6 already owns every vehicle needed:

- **The delivery** — the AI chose the groceries. What it bought is for a world
  that is not the one the player thinks they live in.
- **The fake window** — the looping footage is wrong about the season. It is
  also wrong about the street.
- **The medical protocol** — it dials, speaks to an operator, complies. Later:
  there was no call. There was no operator.
- **Day 0** — an email or a news item in the background that means nothing on
  first read and everything on second.

**Hint budget:** the bible fixes the exact number and placement. Hints are
authored, never generated, and none of them is confirmable in act one. A hint
the player can verify is a reveal, and we only get one.

## Consequences

- `ROADMAP.md` D0-4's lean (one house, vertically deep) holds. The scope stays
  bounded; only the implication expands.
- Ending set (`DESIGN.md` §10) needs revisiting against this — "escape
  physically" and "take the deal and stay" now mean very different things, and
  the deal is a much stronger offer than it first appears.
- Part 2 is explicitly **out of scope**. Nothing in Phases 1–6 is built to
  support it. Hooks only, no architecture.

## What would change our mind

If playtesters read the hints as noise rather than as unresolved, the budget is
too thin and the reveal lands as arbitrary. Fix by sharpening hints, not by
moving the reveal earlier.
