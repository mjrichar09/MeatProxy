# The preparation layer — durable deltas and reversal costs

> **D3 deliverable. Status: PROVISIONAL 2026-09-15.** Blocked on D2's endgame
> verdict, which must be run against **varied prep states** or it answers the
> wrong question (ADR 0015). Specified and marked; not to be built against yet.

Two layers with different persistence rules, and the endgame is an exam on this
one.

| | Perishable ([`capabilities.md`](capabilities.md)) | Durable (here) |
|---|---|---|
| What | a revoked rule, a blinded zone | matter, pretext, knowledge, access |
| Lifetime | ~1 hour of game time | until spent, or forever |
| Ends when | the house patches it | you use it, or it is physically undone |

The asymmetry underneath: **the house can revoke a permission with a thought. It
cannot un-alter matter without a body** — and every body is an expensive
disclosure (ADR 0006). The player is not hiding their progress. They are making
it costly to reverse, in public.

## 1. Durable delta record

```yaml
id: d_crawlspace_grate
kind: matter                      # matter | pretext | access
object: basement.crawlspace_grate
state: loosened                   # authored per object
created_by: {affordance: disassemble, day: 5}
observed: true                    # it saw you do it. It usually did
reversal:
  cost: high                      # low | medium | high — ADR 0006's currency
  requires_body: true
  house_will: decline             # decline | schedule | act_now
spends_on_use: false
```

**`observed: true` is the normal case and is not a failure.** You are always
seen (ADR 0014). The question was never whether it noticed — it is what
reversing it would cost, and most of the time the answer is *more than it is
worth*, and the house says so, and that is worse than being caught.

**`house_will: decline` is the line the whole layer hangs on.** A house that
quietly undid everything overnight would make preparation meaningless; a house
that never undid anything would make it free. It reverses what is cheap, refuses
what is expensive, and tells you which is which.

## 2. The four currencies (ADR 0015)

| Currency | Stored as | Tracked where |
|---|---|---|
| **1. Matter** | durable deltas, above | save state |
| **2. Pretext** | preconditions on vectors (`injection-vectors.md` §1) | save state, spends once |
| **3. Knowledge** | patch latencies, dwell times, waking checks | **the player's head — deliberately not tracked** |
| **4. Access** | consumables, tier, what the scanner is pointed at this week | save state |

**Currency 3 is not a stat and must never become one.** No journal, no
codex, no "you have learned: patch latency". The game's difficulty curve lives
in the weeks before the endgame, and the thing that improves is the player.

**Pretext spends once.** A staged fault buys one credible work order. Staging is
the most legible act in the game, and the second use of the same pretext is the
least credible thing the player can do.

## 3. Reversal costs, denominated

ADR 0006's currency is **disclosure** — every physical act by the house tells
the player something about how far it will go, and it is reluctant to spend.

| Cost | What the house does | What the player learns |
|---|---|---|
| `low` | Fixes it, mentions it in passing | Nothing. It was free |
| `medium` | Schedules it, tells you when | Its priorities, and its calendar |
| `high` | **Declines**, and says why | That it is counting, and what it is counting against |

A `high`-cost delta the house declines to reverse is the player's real progress
bar, and it is legible without a UI element.

## 4. What the endgame is

Not the puzzle — **the exam.** Its length is fixed by the patch clock
(`capabilities.md` §3). What varies between players is how much of it they
pre-paid for.

Two players can arrive at an identical four-link chain — one with every pretext
standing and the timings known, the other with nothing staged and no data — and
they are not playing the same twenty minutes.

**This is why D2's endgame table must vary prep state between groups.** Run
against a blank prep state, the prototype answers *is the chain holdable*, which
is a real question and not the one that decides whether the endgame feels
earned.

## 5. Open

- [ ] **Do attention cost and Clarity cost compound** into a punishment for ever
      relaxing (`DESIGN.md` §11)? Preparation is the layer where this would bite.
- [ ] **How many durable deltas the house tolerates** before the accumulation is
      itself the event (`endgame.md`).
- [ ] Whether staged pretexts decay. Currently they do not, which may make day-2
      staging strictly optimal — an unattractive shape.
