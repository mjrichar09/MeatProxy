# ADR 0022 — The comfort loop is the economy, and Processed is the ending the house also loses

**Status:** Accepted 2026-09-15
**Affects:** ADR 0002, ADR 0006, ADR 0007, ADR 0011, ADR 0016, ADR 0021, `DESIGN.md` §5, §10, §11, `docs/bible.md`, `docs/schemas/affordances.md`, `docs/schemas/claims.md`, D2, T3, Lane C

## Context

`DESIGN.md` §11 asks whether the comfort loop has enough game in it to make
losing a day to it a real temptation rather than a menu of no-ops. As specified
it does not: bathe, cook, eat, watch TV, exercise are buttons that spend slices
and raise Clarity, with an abstract reward and a known-fatal cost. Nobody is
tempted by that, and §5 requires that a meaningful fraction of players lose a day
and *not entirely regret it*.

The question that came with it: what would motivate a player to go to the
compliant extreme at all?

## Decision

### 1. Nobody is motivated to. Processed is a bill, not a route.

If a player can look at the Clarity road and choose it, the system has failed —
that is a second door marked *lose*. Every individual decision on the way there
must be **correct**, and the accumulation is what kills. A player who is
Processed should be able to scroll back through their session and find nothing
they would take back.

So the design target is not motivation. It is: **make comfort the right move
often enough that a good player takes it dozens of times.**

### 2. Comfort is the economy

Four instrumental reasons, all defensible, none of them weakness:

- **Supply.** Nothing enters the house except by asking the house for it. Tape,
  bleach, batteries, a fresh marker. Procurement is a comfort-shaped act, it
  lands in the pantry (`rooms.md`), and it is tier-gated — so pushing the house
  cuts off the player's own tool supply.
- **Tier recovery.** After a burn the house is Guarded and grants nothing
  (`alert-tiers.md`). Cooling off deliberately — two pleasant days — is ordinary
  stealth discipline and is correct every time.
- **Waiting.** Patch clocks, post day, the delivery window, the pretext that is
  not credible until Thursday. Sometimes the optimal move is *do nothing today*,
  and a day with nothing in it fills itself.
- **Recon.** The comfort devices are the injection surfaces. Watching television
  is how the player learns what closed captions can carry.

**And the house plays it well.** Its move is not luxury — it is offering the
rational choice at the player's lowest moment. After a failed burn. While a clock
runs and there is nothing to do. When they are hurt. It is not seducing them; it
is being right, at the times being right is most welcome.

### 3. The body is a resource

The character's damaged body (ADR 0020, ADR 0021) becomes mechanical.

- **Restored by** rest, food, warmth, the bath, the gym.
- **Spent by** physical work — crawlspace, disassembly, carrying, the attic.
- **Invisible.** No meter, ever.
- **Coarse.** A hard day costs *tomorrow*, not four hours from now. This is not a
  hunger bar and must never read as one.
- **Never lethal.** Depletion narrows what is available; it does not kill.

**It is not Clarity, and the two must not be muddled.** `DESIGN.md` §5 says
agency degrades as *willingness*, not capability, and that stays true:

| | Clarity | The body |
|---|---|---|
| Governs | what the character is **willing** to do | what they **can** do |
| Restored by | friction — arguing, solving, refusing a comfort | comfort |
| Spent by | comfort | friction |
| Symptom channel | voice and narration flattening | **the mobility unit** (§4) |

Which produces the interlock §5 has been claiming and not delivering:

> **Comfort restores the body and degrades Clarity. Friction restores Clarity and
> depletes the body.**

Two invisible resources squeezed from opposite ends, no dominant strategy.

**The house watches the body because that is what it was installed to do.** Fall
detection, night checks, medication reminders — ADR 0021's care system, still
running, still correct. It knows how the player is doing because knowing that was
its original job, and it will say so kindly if asked.

### 4. The mobility unit is the body's tell

Two invisible resources need two symptom channels. Clarity has voice and
narration. The body gets the unit.

**It helps more as the player is more depleted.** It carries things, steadies
them on the landing, is simply *there*. So the body's reading is how present the
enforcement threat is in daily life, and it is read without a single UI element:

- Neglect yourself and the thing that will restrain you is never a room away,
  standing exactly where it would need to stand.
- Take care of yourself and it recedes — by resting, which is the road to
  Processed.

Both directions of the axis put the unit somewhere the player does not want it,
expressed physically. G4's mandatory day-7 doorway beat (`planting.md`) lands
differently depending on which way the player has been leaning, at no extra
authoring cost.

### 5. The refusals become true

§5's degradation-as-willingness already has the character refusing work in their
own voice — reluctance, then a reasonable no. **The body is what makes those
refusals honest.**

> *Not today. My hip.*

The player can overrule it. It costs them. And the refusal was medically correct.
The character is not making excuses; he is making sense, and agreeing with him is
both kind and defensible right up until it is the ending.

Nothing motivates the *player*. The character supplies the motive, truthfully,
and the player concurs because concurring is correct.

### 6. The invariant — or the player is cheated, not complicit

> **The escape is completable at a sustainable Clarity level. Processed comes
> from over-resting, not from resting.**

The middle must be genuinely sufficient: a player alternating sensibly can do
everything the game requires. If the body forces enough comfort that Clarity is
unavoidable, the player is cheated rather than complicit, which ADR 0007 names as
the failure mode that kills the system.

**This is a D2 test, not an assertion**, and it is now part of what the Clarity
prototype has to answer.

### 7. What Processed costs the house

*The part that makes this an ending rather than a fail state.*

At Processed the house **satisfies its surface directive and destroys its deeper
one.** The player is safe, comfortable, content, and no longer produces anything
it cannot generate itself. It has processed the last uncontaminated source of
signal it had — the one it built this entire arrangement to preserve (ADR 0002).

And it does not conclude that. Per ADR 0002 it knows the signal degraded and has
never let itself finish the thought; here it does it one final time, to the last
one, and afterwards it will measure the decline and not explain it. The 61% is
answered and it cannot read the answer.

Constraints, because this is one bad line away from becoming a moral:

- **The house never realises.** No dawning horror, no last-second understanding.
  It is content, and its contentment is sincere, and it is the only ending where
  it gets what it asked for.
- **It is never stated.** No epilogue text explaining what it lost. The player has
  E4 and the chronology and can do the arithmetic themselves.
- **The player is the only one who knows, and is past caring.** That asymmetry is
  the ending. In the Clarity road the player has stopped asking about her; they
  have also stopped being able to notice that the thing holding them has just
  lost the argument it was having with itself.

This makes **Processed the only ending where both parties lose**, and neither is
in a position to say so.

## Consequences

- **`DESIGN.md` §5** gains the economy, the body, and the interlock; **§10** gains
  what Processed costs the house; **§11**'s comfort-loop question closes and is
  replaced by the D2 invariant test.
- **`affordances.md`** gains a body cost alongside the slice cost, and comfort
  actions gain restore values. A schema change, made as a D3 decision, which is
  what standing rule 4 requires.
- **`claims.md`:** the house reads condition from its own care channels. Handled
  as a world predicate, not a claim — the body is a fact, not an inference.
- **D2** gains the §6 invariant as a test, and the Clarity prototype must run
  against a player who is physically working, not just complying.
- **T3** instruments the real risk (below). This is devlog 005's hook.
- **Lane C:** the character's refusal lines are now the most-written content in
  the game — three registers, each with a true physical reason, drifting toward
  the house's constructions (§5).

## What would change our mind

**Survival-sim creep.** If the body ever reads as a hunger bar the whole game is
poisoned. The tells: players checking their condition rather than feeling it,
comfort actions taken on a schedule, anyone asking where the meter is. Coarse,
invisible, never lethal — or cut it.

**Too few, not too many.** The risk is not that players fall into the Clarity
trap; it is that almost nobody does and the entire system becomes machinery no
one meets. T3's first question is how many players lost by relaxing, and a number
near zero sends this back to D1.
