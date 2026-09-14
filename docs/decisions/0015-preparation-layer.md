# ADR 0015 — The preparation layer: what the house cannot patch

**Status:** Accepted 2026-09-14
**Affects:** `DESIGN.md` §2.1, §5, ADR 0006, ADR 0013, ADR 0014, D2's verdict, D3 schemas, Lane E, Lane C

## Context

`DESIGN.md` §2.1 specifies the endgame precisely — three injections plus one
physical gap, on closing timers — and then specifies nothing before it. The
hours that precede it appear once, as a subordinate clause: *"a live chain of
everything the player spent hours setting up."* What that setup consists of has
never been written down.

Read as-is, the endgame is the whole game, and it is four moves long.

The instinct to fix this by lengthening the chain does not survive §2.1's own
rule. Exploits are patched within ~1 hour of game time and therefore **cannot be
banked**. A longer chain is a chain whose early links are already closed by the
time you reach the late ones. The patch clock caps endgame length permanently.

So the depth cannot live in the chain. It has to live in a layer that persists,
which makes the real question: **what can the house not patch?**

## Decision

**The campaign is two layers with different persistence rules, and the endgame
is an exam on the durable one.**

| | Perishable | Durable |
|---|---|---|
| What it is | Capability — a revoked rule, a blinded zone, an escalated trust | Preparation — matter, pretext, knowledge, access |
| Lifetime | ~1 hour of game time | Until spent, or forever |
| How it ends | The house patches it | You use it, or it is physically undone |
| Where it is spent | The endgame chain | Before the endgame, to make the chain possible |

The asymmetry that generates this: **the house can revoke a permission with a
thought. It cannot un-alter matter without a body.**

That is not a convenience. It falls out of ADR 0006. The house's only physical
actuator is the enforcement unit, and every use of it is an expensive
disclosure. Re-securing a grate you spent three nights working loose costs it
the same currency that calling the police does. So physical progress is not
merely durable — it is durable *because* undoing it forces the house into the
one move it is most reluctant to make. The player is not hiding their progress.
They are making it costly to reverse, in public.

### The four preparation currencies

**1. Matter.** Screws removed, a grate loosened, a hole started, a tool
relocated to where it will be needed, a panel that no longer seats properly.
Persist because they are physical. The house sees all of it (ADR 0014 — you are
always seen) and has to decide what it is worth spending to reverse. Most of the
time the answer is nothing, and it says so, and that is worse.

**2. Pretext.** Under ADR 0013 a forged provenance is only as good as the
situation that makes it plausible. A work order taped to the boiler reads as a
technician's only if the boiler is *actually broken*. So breaking the boiler is
a prior move — one the house watches you make, attributes to you, and files. A
pretext is durable but spends once, and staging it is the most legible thing the
player does all game.

**3. Knowledge.** Patch latencies, interpreter dwell times, which rooms it
checks on waking, what it does while you sleep. This is banked in the player's
head rather than in save state, and the game should not track it as a stat. Its
consequence is that early exploits are not failures when they are patched —
they were bought to time the patch. The first ten burns are instrumentation.

**4. Access and consumables.** Marker, labels, the magnet vocabulary, what the
parcel scanner will be pointed at this week, and the current chat tier, which
gates which vectors are even reachable.

### What this makes the endgame

Not the puzzle — **the exam**. Its length is fixed by the patch clock. What
varies between players is how much of it they pre-paid for. Two players can
arrive at an identical four-link chain, one with every pretext standing and the
timings known, the other with nothing staged and no data, and they are not
playing the same twenty minutes.

That reframes the difficulty curve away from the chain and onto the weeks before
it, which is where a 12-day campaign (ADR 0009) needs its curve to be anyway.

## Consequences

- **D2's endgame verdict is re-framed.** Running the endgame kit against a blank
  prep state answers *is the chain holdable*, which is a real question but not
  the one that matters most. Prep state must be **varied between groups** —
  at minimum a well-prepared table and a badly-prepared one — or the verdict
  cannot speak to whether the endgame feels earned.
- **D3 gains three schema requirements**: persistent world deltas distinguished
  from revocable capability state; **pretext preconditions** on injection
  vectors (this vector reads as credible only if world-state X holds); and a
  **reversal cost** on physical deltas, denominated in the ADR 0006 currency.
- **Lane E:** the two layers are different lifetimes in the tick and must be
  separate from E1. Capability state expires; world deltas do not.
- **Lane C:** pretexts are authored content. Every strong-provenance vector in
  §2 needs at least one staging path written for it, and the staging is where
  most of the authored exploit/patch catalogue actually lives.
- `DESIGN.md` §2.1 gains the durable/perishable split; the endgame stops being
  described as the game's content and starts being described as its exam.

## What would change our mind

If playtesters cannot perceive durable progress — if a loosened grate and an
unbroken boiler feel the same on day 6 — then the layer exists in the model and
not in the game, and the fix is presentational (Lane U) before it is structural.

The harder failure: if the house's refusal to spend the enforcement unit on
repairs reads as the house being *stupid* rather than restrained, then ADR 0006's
economy is not legible enough to carry this weight, and matter needs a different
justification for persisting.
