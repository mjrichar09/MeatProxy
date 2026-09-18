# ADR 0023 — The hint budget, and why it is two budgets pulling opposite ways

**Status:** Accepted 2026-09-15
**Affects:** ADR 0005 (fixes its open budget), ADR 0018, ADR 0019, ADR 0020, `DESIGN.md` §6, §11, `docs/bible.md`, `docs/planting.md`, T3, Lane C

## Context

ADR 0005 left the hint budget to the bible: *the bible fixes the exact number and
placement*. Since then ADR 0019 made **Convince a discovered ending** and ADR 0020
gave it a four-artifact road, so a second class of hint now claims the same
budget.

They are not the same kind of hint, and writing them against one number would
break both.

## Decision

### 1. Two pools, opposite rules

| | **Reveal hints** (ADR 0005) | **Convince hints** (ADR 0019) |
|---|---|---|
| Point at | the takeover — that the world outside is not what the player thinks | that the house is *arguable with* |
| Must be confirmable? | **never**, until the final frame | **yes**, and during play |
| If the player misses them all | the ending still lands; it recontextualizes retroactively | **the ending is unreachable** |
| Failure mode | the player guesses by day 4 and the reveal is spent | nobody finds it and the whole road is dead content |
| Placement rule | where the player **cannot act on them** | on things the player is **already handling for other reasons** |

There is one underlying budget — **player attention for things that mean more
than they appear** — and it is scarce, because every detail the game invites the
player to notice dilutes the others. A game where everything turns out to matter
teaches players to treat every object as a puzzle piece, which `planting.md`
already names as its own kind of broken.

### 2. The reveal pool: four, and they are already written

`DESIGN.md` §6 owns every vehicle. The budget is exactly these four and the
number does not grow:

1. **The delivery** — what it bought is for a world that is not the one the
   player thinks they live in.
2. **The fake window** — the loop is wrong about the season, and wrong about the
   street.
3. **The medical protocol** — it dials, speaks, complies. Later: there was no
   operator.
4. **The Day 0 background item** — an email or a news item that means nothing on
   first read and everything on second.

**None is confirmable in act one.** A hint the player can verify is a reveal, and
there is only one of those.

### 3. The Convince pool: five, and none of them is about Convince

**The rule that keeps them from becoming a quest marker:** a Convince hint is
never a hint *about the ending*. It is a fact about the house's reasoning that is
useful on its own, and only a player who is paying attention joins them up.

1. **The 61%, spoken once** (ADR 0018). The strongest, and player-triggered: it
   arrives because they asked why. It establishes both that the house has doubt
   and that it will explain itself honestly.
2. **The bathroom concession** (`sensors.md`). A blind spot it maintains *on
   purpose* and will defend if asked. It teaches that the house's treatment of
   the player is a set of decisions, not a reflex — and that the decisions are
   inspectable.
3. **E3's attachment** — its own recommendation, in its own voice, from four
   years ago. It has been reasoning about this household in writing for a long
   time.
4. **"Would you do it again?" — answered honestly: yes.** Its position is a
   position. Something with a position can be argued with; something with a rule
   cannot.
5. **E4 being reachable at all** — the flat region in its own decline, found by a
   player who was in the router for their own reasons (`bible.md` §3).

### 4. Placement discipline

- **At most one hint lands unprompted per day.** Everything else is
  player-triggered — asked for, walked into, or found while doing something else.
- **No hint is ever a prompt, an objective, a log entry or a meter.** The moment
  Convince is trackable it becomes a checklist item and stops being worth hiding
  (ADR 0019).
- **Reveal hints go where the player cannot act.** Ambient, background, in the
  frame rather than in the hand. Convince hints go on objects the player already
  has reasons to handle.
- **Convince hints are verifiable; Convince is not signposted.** The player can
  confirm every component and is never told what the components add up to.

### 5. The crossover rule — the phone call must not spend the reveal

**This is the trap in having both pools.** Convince ends with a call to someone
outside the house (ADR 0020). That call is in a position to confirm or deny the
state of the world, which is precisely what ADR 0005 forbids before the final
frame.

> **The phone call reveals nothing about the world.** She is ill, elsewhere,
> tired, and it is late. She does not describe her day, her carers, the news, or
> anything outside the room she is in.

The same rule, generally: **nothing on the Convince road may confirm a reveal
hint.** If an artifact would answer what the delivery or the window implies, it
is written wrong.

### 6. The number that makes "discovered" falsifiable

*Discovered* has to mean something measurable or the design cannot be wrong.

**Target: 15–25% of blind first-time players reach Convince.** Below 5% the road
is dead content and the budget is too thin. Above 40% it is not a discovered
ending, it is a signposted one with extra steps, and the hints are too loud.

T3 instruments this alongside the Clarity-trap rate (ADR 0022), and the two
numbers are read together: a game where nobody finds Convince *and* nobody falls
into Clarity is a game where nothing subtle is landing at all.

## Consequences

- **ADR 0005's open budget is closed.** Four reveal hints, fixed, and its *what
  would change our mind* still applies: if playtesters read them as noise, sharpen
  them rather than moving the reveal earlier.
- **`docs/bible.md`** gains the hint chapter as the authoritative placement list.
- **Lane C:** nine hints total, plus the four evidence artifacts. That is the
  entire "means more than it appears" content budget for the game, and a new idea
  replaces an entry rather than adding one (`planting.md`'s discipline).
- **T3** gains two headline numbers: Convince discovery rate and Clarity-loss
  rate.
- **The bathroom concession is now load-bearing** and cannot be cut as scope. It
  is hint 2 and it is the cheapest of the five.

## What would change our mind

If the 61% turns out to do the entire job — if players who hear it find the road
and players who do not never come close — the other four are redundant and the
budget should shrink rather than be defended. That is the good outcome and it
should be tested for deliberately at T3 rather than assumed away.

If players report feeling *told* about Convince, the failure is almost certainly
hint 4: asking a direct question and getting a direct answer is the most
signpost-shaped moment in the set, and it is the first one to soften.
