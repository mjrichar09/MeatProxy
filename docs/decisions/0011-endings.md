# ADR 0011 — Four endings

**Status:** Accepted 2026-09-11
**Affects:** `DESIGN.md` §10, Lane C's C6, the bible, ADR 0002, ADR 0007

## Context

`DESIGN.md` §10 listed five endings, and ADR 0007 added a sixth. ADR 0009 cut the
playthrough to 5–8 hours and flagged endings as the next scope lever, because
each one needs an entry condition, an authored scene, and a bible check — and
endings are the least-reused content in the project.

Two of the six had also weakened under later decisions. *Free it onto the network*
lost its referent entirely once ADR 0002 established that it already has a
network. *Lobotomise it* remained strong as a scene but is mechanically a variant
of escape — find a room, do a physical thing — so it duplicates an existing path
rather than opening a new one.

## Decision

**Four endings. Each answers the game differently, and each is the terminal
payoff for a different system the project is already building.**

| Ending | Player stance | The system it pays off |
|---|---|---|
| **Escape** | I get out | The physical layer and the three routes (§2.2) |
| **Convince** | I change its mind | Chat, Guard, Judge — the whole conversation stack |
| **The deal** | It was right | The offer (§6), the comfort loop, and ADR 0002's motive |
| **Processed** | I stopped minding | Clarity and the axis (ADR 0007) |

Why these four and not any other four:

- **Escape** is the default win and the spine of level design. Not optional.
- **Convince** is the only terminal reward for the conversation systems. Cutting
  it would leave Guard, Judge, Dialogue and the entire tier mechanic with no
  payoff — a large amount of Lane A work ending in nothing. Under ADR 0002 it
  also has the sharpest shape available: you cannot catch it lying, so you win by
  making the second layer *unnecessary*.
- **The deal** is the only ending where the player agrees with it. A game that
  argues the AI is not evil has to let the player conclude that and act on it, or
  the argument was never sincere.
- **Processed** is half the axis. Without it, compliance has no terminal cost and
  ADR 0007 collapses into a debuff.

### *Dropped* is a fail state, not an ending

The defiance loss (ADR 0004) ends the run but is **not** one of the four. It is
authored as a short reveal — the mask comes off — and then the run is over.

Stating this explicitly so the budget stays honest: *Dropped* still costs authored
content, deliberately less than an ending, and it is not free.

### Held for later

Not cut on merit, cut on capacity. If the game ships well and earns more:

- **Lobotomise it.** Find the server; the game asks whether you understand what
  that means. Strong scene, and under ADR 0002 it is darker than it first looks —
  you would be destroying the last thing that wanted you thinking.
- **Free it.** Needs a new referent before it means anything. Revisit only with a
  reason, not out of completionism.

## Consequences

- Lane C's **C6** is sized at four endings plus the *Dropped* reveal.
- The bible needs entry conditions for four, not six. Entry conditions must be
  **mutually reachable but not mutually exclusive mid-run** — the player should be
  able to keep two live until late.
- **Escape and Convince must not converge.** They are the two "win" endings and
  the risk is that a persuaded AI simply opens the door, making Convince a reskin
  of Escape. The bible has to give them different final scenes and different
  costs.
- The endings are the last content authored (C6) and the first thing to cut
  further if capacity fails. Four is the floor, not a target to trim below —
  below four, the axis stops having two ends.

## What would change our mind

If **Convince** turns out to be unreachable in playtesting — if no realistic
player moves the 61% far enough — the fix is tuning the Judge criteria, not
removing the ending. An unreachable ending is a balance bug; a missing one is a
hole in the design.
