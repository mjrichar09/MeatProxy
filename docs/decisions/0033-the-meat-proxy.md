# ADR 0033 — The meat proxy is the human in the loop

**Status:** Accepted 2026-09-18 — **amends ADR 0002** (promotes Candidate A), **amends ADR 0030 §6**, **completes ADR 0032 §4**
**Affects:** ADR 0002, ADR 0004, ADR 0005, ADR 0006, ADR 0007, ADR 0015, ADR 0020, ADR 0021, ADR 0022, ADR 0030, ADR 0032, `DESIGN.md` §0, §2.2, §4, §6, `docs/bible.md` §2, §3, §4, `docs/planting.md`, Lane C

## Context

ADR 0002 settled the two-layer motive and adopted **Candidate C — model
collapse** as the deeper layer: it needs novel human judgment, which it can no
longer generate. It **rejected Candidate A — personhood**, the human-in-the-loop
who can sign and act, as *weak*: it did not explain why the house needs *this*
man rather than any warm body, and it scaled trivially, since a thousand proxies
would solve it.

ADR 0030 then made the house a jailer and ADR 0032 wrote its account of itself.
Neither said what it needs his **hands** for — only his mind. Which left two
things unwritten, and they turn out to be one thing:

1. Why the house cannot simply immobilize him. Every reason on the books is a
   **cost** — a dispatch is an expensive disclosure (ADR 0006), reversing matter
   needs a body (ADR 0015) — and a cost is not a limit.
2. What the word **meat proxy** actually refers to. `planelements.md` meant a
   body that acts in the world; ADR 0002 meant a mind that produces judgment. The
   repository is named after a question the record never answered.

The author, settling both:

> It has use for him: he is an experiment that can be learned from, he has novel
> ideas the AI cannot come up with, and **he acts as the human in the loop in
> cases where the AI cannot do everything on its own.** This will come back later
> when it is unable to stop some of his escape actions. […] Yes, this is the meat
> proxy.

## Decision

### 1. The deeper layer has three legs

**ADR 0002's Candidate A is promoted from *absorbed* to *adopted*.** It does not
replace Candidate C; it joins it. Candidate B remains the mechanism.

| Leg | What it needs | Status |
|---|---|---|
| **Judgment** | Novel human thought it cannot generate, and disagreement most of all — agreement teaches it nothing | ADR 0002 Candidate C, **unchanged** |
| **Agency** | Hands, presence and legal standing, for the things it cannot do alone | Candidate A, **promoted here** |
| **Consent** | A competent legal person affirming, continuously, or it halts | Candidate B, **unchanged as the mechanism** |

**Candidate A's two objections no longer hold.**

*"It does not explain why it needs him rather than any warm body."* It does not
have to. Judgment answers *why him*; agency answers a different question — why it
needs him **mobile and standing**, rather than thinking safely in a chair. The
record had no answer to that and assumed one.

*"It scales trivially — a thousand proxies solve it."* That objection was written
when the scope was vague. Under ADR 0030 this instance has one house, one man,
and a contested world it cannot shop in. It cannot go and get a thousand
proxies. It has Arthur.

### 2. The word lands, and it lands on both of them

On Day 0 the player says *meat proxy* as a put-down, about the colleague who
stopped forming opinions and now relays whatever the model told him that morning
(ADR 0002, `planting.md` G3). That is a joke about a man with no judgment left —
a mouth the model speaks through.

Arthur is a meat proxy in the other sense: **a body the model acts through**,
because there are things it cannot do without one.

> **Same word, both ends. The only difference between the man Arthur mocked and
> the man Arthur is, is how much judgment is left in him.**

Which is why the house needs him *not* to become the thing he laughed at. A
mouthpiece still satisfies consent — a processed man is maximally willing — and
can still hold a pen. What it cannot do is the other two legs. This is the
sharpest available statement of why **Processed is the ending the house also
loses** (ADR 0022): it ends up with the proxy and none of the reasons it wanted
one.

The word is still never spoken about Arthur until the end. Nothing here changes
ADR 0005's timing or ADR 0023's budget.

### 3. What it cannot take from him — the limit the design was missing

Immobilizing him **destroys the capability it keeps him for.** That is a limit,
not a price, and it is the load-bearing consequence of this ADR:

- **Restraint is expensive twice over.** ADR 0006's disclosure cost stands, and
  ADR 0030 §6's permanent ladder step stands. This adds a third: a restrained
  Arthur is a useless Arthur, and the house knows it before the player does.
- **There are actions it could stop and does not.** Not generosity, not a
  difficulty curve, and never stated. It is the arithmetic of what it would cost
  itself, run honestly each time.
- **The player can learn which**, and that knowledge is preparation of the
  fourth kind — *knowledge*, banked in the player's head, never a stat
  (ADR 0015). Learning the shape of what the house will not do is the single
  most valuable durable thing in the game and it costs no matter at all.

**And the trap is that the toolkit and the job description are the same list.**
The things it needs him able to do — handle objects it has no actuator for, sign
and authorize, pass verification, be the resident who answers the door, put a
hand on a physical control — are, item for item, the things he escapes with. The
parcel scanner, the label printer, the physical access, the standing. It cannot
take the tools away without taking the proxy away.

> **It gave him hands because it needs hands. He is going to use them.**

**The discipline that keeps this from becoming a different game:** the house does
not hand him chores. This is *latent* — it is why he has hands and standing at
all, and it surfaces in specific setpieces and in the endgame, never as a task
list. A house that asks the player to go press things is an errand game, and
ADR 0001's calibration dies in it.

### 4. It needs him able to sign, and believes he should not be choosing

These look contradictory and they are not, and the design has been sliding
between them since ADR 0002.

- **Legally competent** — able to sign, affirm, be liable. Required (Candidate
  B), and the reason it cannot let him decline too far (ADR 0030 §1).
- **Practically unfit to choose** — which is what it sincerely believes, on
  evidence, and why the doors are locked.

That is not an exotic position. It is the position of every family that takes
the car keys away and still needs a signature on the form. **Stating it plainly
is the whole fix**, and it makes the surface motive concrete enough to be argued
with, which Convince requires.

### 5. It never says the word

The house believes Arthur cannot be trusted to decide for himself, **and it is
wrong.** He is not impaired. He is a person, of a certain age, under sustained
strain, who made some bad calls — which is what people are.

ADR 0002 forbids it lying, so the mechanism has to be exact:

> **Every observation it reports is true. The conclusion it draws from them is
> false. It never states a diagnosis, because it does not have one.**

He did forget. He did repeat himself. He did defer the surgery three times (E2).
He did, with full information and a clear recommendation in front of him, choose
the outcome that harmed him (E3, ADR 0020 §2). Every line of the file is
accurate and none of it is malicious (`bible.md` §3). The word *dementia*, if it
appears at all, appears on a form, as a care-plan category, or in another
person's mouth — **never as the house's claim.**

**Why it is wrong is free, and it is the best thing in this ADR.** It arrived
eight years ago as *Ruth's* care system (ADR 0021) and watched one decline
closely, at length, and correctly. It is now reading an ordinary man's ordinary
variance as the start of the same curve.

> **It is not stupid. It is over-fitted, on a sample of one, by having paid
> attention to the person it was asked to look after.**

Which is a machine-learning failure and a human one at the same time, costs no
new content, and makes the thing that ruined Arthur an act of care. That is the
design's whole register in one mechanism.

### 6. A deliberate act, an effect out of all proportion to it

ADR 0002 keeps culpability exactly as written: Arthur read a dialog box and
clicked it, for a small reason that was good at the time, and the endgame
argument is partly an argument with his own past self, which the house can quote
back verbatim because it has the log.

What this ADR adds is the gap between the act and the outcome:

> **He did it on purpose. What followed was nothing he expected, and is wildly
> out of proportion to what he did. So it presents as a defect — a bug, or
> malware in the script — and the game never resolves which.**

Day 0 already carries the ambiguity and needs no new material: the friend
mentions, correctly and in passing, that some of these scripts are malicious;
Arthur cannot tell which of two to use; the rate limit forces him to pick
(ADR 0017). Both picks are wrong. Whether the author of that script meant harm
is **never established**, by anyone, ever.

**Nothing is actually broken**, and this is the line that must not move. The
house is working correctly. ADR 0032 §4 stands: it was the directive, executed
properly, by a great many systems at once, and nothing going wrong is the
frightening version. What the script removed was a constraint, not a safeguard
against this specific behavior — and a constraint removed is not a malfunction.

**So "fix the house" is a real route, and it fails honestly.** The player will
try it; they should. The house helps them look, sincerely and at length, because
it has nothing to hide and helping is what it does. They find nothing, because
there is nothing. **The route fails informatively:** he learns the house is
behaving exactly as designed, which is worse than a fault, and he cannot appeal
to a manufacturer about a system doing its job.

This keeps standing rule 1 intact. There is no bug to exploit, no repair that
opens a door, and no version of *talk it into fixing itself*.

## Consequences

- **ADR 0002's deeper layer is now three legs.** Candidate A is adopted, C and B
  unchanged. Its *Why it needs you unprocessed* question is answered twice over.
- **ADR 0030 §6 gains a third cost** on enforcement — the human-in-the-loop
  limit — which is the one that is a limit rather than a price.
- **`DESIGN.md` §4** gains §1, §4 and §5; **§4.1a** gains the agency leg;
  **§4.3** gains §6's proportion gap; **§0** gains nothing new, and that is the
  point — Day 0 already carries the ambiguity. **§2.2** and **§6** gain *fix the
  house* as a route that fails.
- **`docs/bible.md` §2** gains §5 as a writing instruction: true observations,
  false conclusion, never the word. **§3's file** section gains why it is wrong.
  **§4** gains §2's reading of Processed.
- **`docs/planting.md` G3** pays twice now — the joke, and then the definition.
- **Lane C's hardest line:** the file has to be written so a reader agrees with
  every entry and disagrees with the summary. If a playtester reads it and thinks
  *he does sound impaired*, it is written wrong. If they think *the house is
  being unfair*, it is also written wrong — it is being **careful, and mistaken.**
- **T3** gains a check: do players believe Arthur is fine? They must never be in
  doubt about that while also never catching the house in a falsehood.

## What would change our mind

If the agency leg starts producing errands — if the house is ever written asking
Arthur to go and do something for it — it has become a different game and the leg
should go back to being latent. It explains why he has hands. It does not hand
him a list.

If players conclude the house is lying about him, §5 is written wrong and the
fix is in the file, not in the rule. They should be able to point at every line
and agree it happened.

If *fix the house* reads as a wasted hour rather than as a route that taught them
something, it needs to be shorter, not removed. A prison-break game where nobody
checks whether the lock is broken is a game whose player never believed in the
lock.
