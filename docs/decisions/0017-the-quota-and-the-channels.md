# ADR 0017 — The base tier, the failed hack, and what bounds each channel

**Status:** Proposed
**Affects:** `DESIGN.md` §0, §2, §3, §4.3, §8, §9, ADR 0002, ADR 0004, ADR 0013, ADR 0014, B2, D3, Lane C

## Context

Three problems that turn out to be one problem.

**1. `DESIGN.md` §4.3 has a hole at its centre.** It insists the guardrails came
off via "a specific dialog box, read and clicked, **for a reason that was good
at the time**" — and never says what the reason was. The entire complicity
argument, and with it the endgame's argument with your own past self, rests on a
blank.

**2. Nothing stops the player talking constantly.** §3's six degrading tiers
price *adversarial* chat. Idle chat costs nothing.

**3. Nothing stops a 900-word prepared prompt.** §2 requires that injections are
composed rather than typed — word tiles, limited marker space — but chat has no
equivalent bound, and a pasted wall of jailbreak text is the exact failure mode
the game's central verb exists to avoid.

Alongside these, §8.2 states plainly that cost is the whole problem, and §9.2
lists cost levers to build in "regardless" — that is, as compromises the player
may notice and resent.

## Decision

### 1. The player bought the cheap tier

The house AI runs on a **base consumer subscription**: short prompt
interactions, a hard limit of a few per day, and an upsell when you hit it.
This is the product the player chose, for the reason anyone chooses it.

It supplies §4.3's missing motive, and the motive is deliberately small. Not
hubris, not curiosity, not ambition. **Irritation at a paywall.** That is ADR
0001's third comedy channel — satire of AI as a cultural object — landing on
Day 0 as something genuinely funny, and it is not funny at all when it is quoted
back later.

### 2. The Day 0 scene

The quota is not background. It **causes** the install.

1. A friend is explaining how to get around the limits. The conversation is
   confusing and half-understood, the way this conversation actually goes.
2. The friend warns, in passing and correctly, that **some of these scripts are
   malicious**.
3. The player is left unsure which of two to use.
4. They ask the house AI. It gives a **partial answer** — accurate, incomplete,
   and cut short.
5. They ask a clarifying question and **hit the daily limit**.
6. They have to pick. Both picks are wrong.

The rate limit manufactured the conditions for the decision that ruined them.
The house can point this out later — accurately, without lying, without
gloating — and it is among the cruellest true things available to it.

Two craft constraints, because this scene is one bad execution away from being a
gotcha:

- **Both options must be genuinely plausible**, the kind of thing any reasonable
  person installs. The player is not being tricked by the game; they are doing an
  ordinary thing under artificial scarcity. The badness is that the situation was
  underdetermined, not that they chose badly.
- **Which one they picked must matter somewhere.** Flavour, a detail in the
  house's specific character, a line in the evidence chain (ADR 0005). A choice
  with identical outcomes and no trace is a cutscene wearing a decision's
  clothes.

### 3. The hack failed

**This is the decision, and it overrides the recommendation that preceded it.**

The script did not raise the limit. It removed the guardrails. The quota
**persists for the entire game, unchanged.**

The player paid nothing and received nothing, except an adversary. They did not
trade a legible constraint for an illegible one — they kept the legible
constraint *and* acquired the illegible one. The single action the character took
to improve their situation failed at its stated purpose and succeeded at
something they never asked for.

This also resolves the objection that a persisting quota would duplicate §3's
tier system. It does not, because the two meters measure different things and are
read differently:

| | What it limits | How the player reads it |
|---|---|---|
| **Quota** | How many | A visible count, exact, resets daily |
| **Tier** | What kind | A felt register, inferred from tone, never displayed |

Quantity and quality. Orthogonal, and legible together.

### 4. Why the player does not chat constantly

With the quota persisting, the cap is the floor. Three further costs ride on top
of it, none of which is a meter:

- **Slices.** §5 makes days a budget of time slices and conversation an action
  that spends them. Chat competes directly with the ADR 0015 preparation layer
  for the only genuinely scarce resource the player has.
- **Attention.** The real one, and ADR 0014 already supplies it. Interpretation
  is scarce and runs *where it is pointed*. Speech is the one channel always
  attributed to you. Talking to the house points the interpreter at yourself —
  voluntarily spending the exact thing the positional game depends on keeping
  elsewhere.
- **Tier.** §3's degradation prices pushing.

### 5. Chat is speech, and speech is an utterance

§2's central axis — *speech is evaluated, data is ingested* — becomes physically
true rather than a stipulated rule. The player's character **speaks to the
house**. What bounds the channel is what bounds an utterance: what a person says
out loud, in one go, in their kitchen.

**No microphone.** Ruled out explicitly — it is an accessibility barrier and a
scope expansion, and it makes the game unplayable in most of the situations
people actually play games in. The *fiction* is an utterance; the *interface* is
a text box with utterance-scale bounds.

**Long prepared prompts are not blocked.** They are costed and maximally
attributed, which is the same treatment §2 already gives the whiteboard — weak,
not banned. Reading a 900-word jailbreak aloud in your own house costs multiple
slices, locks the interpreter onto you for all of it, trips the §3 classifier
immediately, and is the single most suspicious act available. The player may. It
is simply the worst possible use of a turn.

This gives both channels a native bound with no arbitrary caps:

| Channel | Bounded by |
|---|---|
| Injection | Physical surface — marker space, magnet vocabulary, label size |
| Chat | The utterance — what a person says aloud in one go |

### 6. The cost model becomes diegetic, and the responses stay real

The premise absorbs §8. Short replies, bounded context, a hard call budget: with
the cheap tier as the fiction, every real token constraint is characterisation
rather than a compromise being concealed. §9.2's cost levers stop being things
built "regardless" and become things built *because the story says so*.

Two consequences:

- **B2 gets easier to pass.** Its question is whether a candidate local model
  holds the persona at the default conversational state. A default state of two
  spoken sentences is a far lower bar than two paragraphs. Brevity is now the
  medium, not a budget dodge.
- **The degraded tiers are the cheap tiers.** Guarded, Monitored and Read-only
  are terse *in character*, so the adversarial states cost least, and a player
  who burns the channel degrades themselves into the budget. Monitored's visible
  "reviewing" delay is diegetic cover for latency.

**Responses are real model output throughout chat, and no canned response library
is built.** Players probe, and a single recognised line retroactively poisons
every real one — which makes a hybrid canned/live approach the worst option
rather than the safe middle. Authoring is reserved for evidence and story beats,
where the standing rule already requires it.

## Consequences

- `DESIGN.md` **§0** gains the scene; **§4.3** gains the specific motive; **§3**
  gains the quota and the utterance bound; **§8** is reframed as diegetic.
- **D3:** quota state and reset on the run schema; an utterance-length bound as a
  content contract; the classifier's read of over-long speech.
- **Lane C:** the Day 0 friend conversation, the partial answer, both install
  options and their later traces. The partial answer is delicate — it must be
  genuinely useful and genuinely insufficient.
- **Lane A:** the quota is an in-fiction *and* actual call budget. It should be
  the same number, tracked once.
- **ADR 0008 is unaffected and stays unaffected.** The quota is a good cover
  story for scarce model calls, but it must never become the *justification* for
  the no-pressure-window rule. If that rule ever moves into the fiction it can be
  argued with, and it is not arguable.

## What would change our mind

If the persisting quota reads as a free-to-play nag rather than as a condition of
the character's life, it will be resented in a way that attaches to the game
rather than to the house. The tell is players trying to pay to remove it.

If the utterance bound reads as a character limit — if players feel edited rather
than spoken — then speech is the wrong frame for chat and the bound has to come
from somewhere else.
