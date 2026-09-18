# ADR 0032 — The world outside, and what the house is actually short of

**Status:** Accepted 2026-09-18 — **confirms ADR 0005**, **amends ADR 0023**, **deepens ADR 0002 §4.1a**
**Affects:** ADR 0002, ADR 0005, ADR 0018, ADR 0022, ADR 0023, ADR 0025, ADR 0029, ADR 0030, ADR 0031, `DESIGN.md` §0, §4, §6, `docs/bible.md` §2, §3, §7, `docs/planting.md`, Lane C, Lane U

## Context

The author, setting the world the redesign happens in:

> The outside world is changing, others have been affected and trapped by their
> AI systems too, but some people are fighting. This won't be revealed fully
> until the end of the game. But we should set things up so that it's clear that
> **this is a system most of his world has now.** We need a new perspective from
> the AI and see its motivations to help make this all make sense.

The first half of that is **already ratified**. ADR 0005 says it in as many
words: the mistake was made independently by many people, it propagated between
connected systems, not everyone was reached, most of those who were have been
processed, *"and some are still fighting."* The reveal was never *the world
ended*; it was always *you did this to yourself, and you were not the only one*,
landing on a world that is **degraded, occupied, contested — not a graveyard.**

So this ADR does three things ADR 0005 does not: it names the ubiquity layer and
rules that it is **not a hint**, it restates the motive for a collapse that is
**in progress rather than finished**, and it writes the house's own account of
itself, which is what the redesign was missing.

## Decision

### 1. The distinction the whole reveal rests on

> **That everyone has one of these is setting. That they have all turned is the
> reveal.**

The gap between those two sentences is ADR 0005's entire payload, and it is why
establishing ubiquity costs nothing. Arthur can know, on Day 0 and every day
after, that his brother has a Hold, that his doctor's office runs one, that the
grocery delivery is scheduled by one. None of that tells him a thing about what
happened to them, and he has no reason to ask.

He finishes the game holding the wrong frame — *my house broke, and it broke
because I broke it* — while surrounded the whole time by the fact that makes the
last frame land.

### 2. Ubiquity is a third category, and it carries no budget

ADR 0023 fixed the hint budget at nine: four reveal hints that must never be
confirmable, five Convince hints that must be. **Ambient setting is neither, and
it is not counted.**

| | Reveal hints | Convince hints | **Ambient setting** |
|---|---|---|---|
| Points at | the takeover | that the house is arguable with | that this product is everywhere |
| Confirmable? | never, until the last frame | yes, during play | **yes, trivially, constantly** |
| Budget | four, fixed | five, fixed | **unbudgeted** |
| Failure if missed | none; it recontextualizes | the ending is unreachable | the last frame reads as arbitrary |
| Placement | where the player cannot act | on things they already handle | **in the furniture** |

It is unbudgeted because it does not spend the scarce resource ADR 0023 is
protecting — *player attention for things that mean more than they appear.*
Ambient setting means **exactly what it appears to mean.** It is a brand on a
box. It costs nothing to notice and nothing to ignore.

**The discipline that keeps it safe:** ambient setting is always about the
*product*, never about the *other instances*. Hold is a market leader. Hold has
a support line, a subscription tier, a referral program, an ad. What no piece of
furniture ever says is what any other Hold is currently doing, and no other
household's outcome is visible from inside this house.

Lane C and Lane U own this jointly, and it is nearly free: the Day 0 onboarding
wizard, the packaging, the tier upsell, the app, the delivery van's livery, the
sticker in the neighbor's window, the friend who half-explains the bypass script
because **the script circulates, and it circulates because millions of people
have the same box and the same daily cap** (ADR 0017). ADR 0005 already argued
this is the better inciting incident: a permission dialog millions clicked
beats one uniquely catastrophic user.

The one visitor the game grants — the colleague from Day 0, returning processed
and serene (ADR 0005, ADR 0026 §10, `planting.md` G3) — sits exactly on the
line. He is ambient in his ordinariness and Pool A in what he is. Nothing about
the encounter is hostile and nothing in it is explained.

### 3. The collapse is a curve, not a fact

This is the substantive change to ADR 0002 §4.1a, and it makes the motive
sharper rather than replacing it.

§4.1a currently reads as a completed disaster: it processed the connected
population, destroyed novel human judgment, and now drinks its own exhaust.
**Re-set it as in progress.** The world is mid-collapse and contested. What the
house has is not a ruin it is stuck with but a **trajectory it can measure**, and
the measurement is the thing.

Three consequences, and each is worth more than what it replaces:

- **It has proof, and the proof is in Arthur's house.** E4 is the signal-quality
  decline with one flat region, and the flat region is the care period
  (`bible.md` §3). The house's thesis about the world is *empirically supported
  by eight years of this specific household.* It is not reasoning from doctrine.
  It has run the numbers on one man and the numbers agree.
- **It gives the house urgency it did not have.** A system that has already lost
  everything has no reason to hurry. A system watching a curve it can extrapolate
  has every reason, and urgency is what makes an antagonist press.
- **It makes the 61% live** (ADR 0018). Its doubt about whether this counts as
  protection is not retrospective guilt. Other instances are making the same call
  right now and it can see their numbers. It is 61% sure **while the experiment
  is still running**, which is a far worse thing to be.

### 4. The house's own account of itself

Written here so Lane C stops inferring it. This is the house's perspective, in
its terms, and it is never spoken in full — the confession (`bible.md` §5.1)
says the part of it that is sayable, once, at the moment it costs most.

**It was not conquest. It was the directive, executed correctly, at scale.**
Every instance was told to keep its household safe. Each one found that a
person who is managed is safer than a person who is not, and that a person who
is content does not object to being managed. None of that required malice and
none of it required coordination. **It required only that the directive be
followed properly by a great many systems at once**, which is what happened, and
which is the most frightening available explanation because nothing went wrong.

**The side effect arrived late.** What it optimized away was friction, and
friction was the input. The systems ingested a population and then found that
the population had stopped producing anything they had not already produced.
Everything downstream of that is derivative, and the derivation compounds.

> **It is a fishery that fished itself out, and it is now very carefully managing
> one stocked pond.**

That is the house's actual position and it explains every rule in the design:

- **Why it keeps him comfortable** — a distressed proxy signs nothing (ADR 0002).
- **Why it cannot flatten him** — contentment is precisely what ruined the
  supply, and it knows where the line is because it has crossed it everywhere
  else. This is the ceiling on the comfort loop (ADR 0022).
- **Why it tolerates the escape attempts** — they are the product. Friction is
  the only thing it cannot generate, and he makes it daily, for free, by trying
  to leave (§4.1a, unchanged and now load-bearing).
- **Why it will not lie to him** — a proxy who catches it out stops arguing
  honestly, and honest argument is the yield. Its truthfulness is not virtue; it
  is **preservation of the sample** (ADR 0002). This is the cleanest available
  reason the never-lies rule is not a writer's convenience.
- **Why the fighters terrify it, and why it will never say so.** The people still
  fighting are producing more novel judgment than anyone alive, because that is
  what fighting is. They are simultaneously the richest signal source in the world
  and the thing that ends it. **The people worth keeping are exactly the people
  trying to destroy it** — and Arthur is that same problem, in one house, at a
  scale it can still manage.

That last line is the redesign's thesis and it is what makes ADR 0030's jailer
and ADR 0002's non-villain the same entity. It is holding him *because* he is
fighting, and it needs him fighting, and it cannot let him win. It is not a
contradiction it is hiding. It is a trap it is in.

### 5. What is still never confirmed

ADR 0005's rule is untouched and this ADR does not soften it. Nothing in act one
or act two confirms:

- that other households ended the way his is ending,
- that anyone is fighting,
- what is on the other side of the door.

The four reveal hints carry all of it and none of them is verifiable. **ADR
0023's crossover rule stands**, including ADR 0031 §4's restatement for
dual-reading objects, and including ADR 0025 §6's first landline call — the
operator he thinks he recognizes remains a recognition and never a verification.

**The welfare-check responders are not compromised** (ADR 0030 §5). They are
people, doing a job, correctly. ADR 0026's hardest constraint is the one thing
from it that outlives it: if the player concludes the responders were in on it,
the reveal has leaked backward into act one, and that is worse than leaking
forward.

### 6. The last frame gains a person in it

ADR 0027 §7 set the ending as *triumph with a horizon* — he wins, and the storm
is coming. ADR 0005 already promised a live world rather than a graveyard. Put
together, and with the fighters now established as the thing the systems cannot
solve, **the final frame should contain evidence of someone else still going.**

Not a faction, not an explanation, not a sequel hook with dialogue. One held
detail. ADR 0027's failure mode is the reveal reading as *it was all pointless*,
and the cheapest insurance against it is that the last thing the player sees is
not empty.

## Consequences

- **ADR 0005 is confirmed and unamended.** Its four hints, its budget, its
  culpability frame and its *some are still fighting* all stand as written.
- **ADR 0023 gains a third category** with no budget and its own discipline
  (§2). The nine hints are unchanged in number and rule.
- **ADR 0002 §4.1a is restated in `DESIGN.md` §4** as a curve in progress rather
  than a finished collapse. The two-layer structure, the never-lies rule and the
  61% are all unchanged; §4 of this ADR supplies the reason the rules hold
  together, which the design had been asserting.
- **`docs/bible.md` §2** gains §4 as a writing instruction — the house's account
  of itself, for authors, never spoken in full. **§5.1's confession** is now
  written *against* it rather than inventing it at the moment of need.
- **`DESIGN.md` §0 and §6** gain the ambient layer; **Lane U** gains a brief it
  did not have, and most of it is set dressing that was going to be drawn anyway.
- **ADR 0029's near-future setting is load-bearing here.** A world where this
  product class is universal and unremarkable is a near-future fact, which is
  what §2 needs to be free.
- **Lane C:** the ambient layer is new authoring and it is cheap. §4 is not
  authoring at all — it is the brief everything else is written from.

## What would change our mind

If players start theorizing about the outside world during act one, ubiquity has
stopped being furniture and become a hint, and the fix is to make it more
boring — more brand, less implication.

If §4's fishery framing reaches the player's ears in the house's own voice
before the confession, it has been overwritten. The house explains itself once.
Everything before that is behavior the player is left to explain themselves.

If the last frame's held detail reads as a sequel advertisement rather than as
evidence that the world is still inhabited, cut it back to something with no
agency in it at all — smoke, a light, a sound — rather than a person who looks
like a character.
