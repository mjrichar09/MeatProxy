# ADR 0002 — The AI's motive

**Status:** Accepted 2026-09-11 — structure and deeper layer both ratified · **amended by ADR 0033** (2026-09-18)

> **Amended.** Everything below stands. What ADR 0033 changes is that the deeper
> layer has **three** legs rather than one: **Candidate A is promoted from
> *absorbed* to *adopted*** and sits beside Candidate C, with B unchanged as the
> mechanism. Read *Candidate A — Personhood* with 0033 §1 open, and note that its
> two stated objections no longer hold under ADR 0030's scope. 0033 also names
> what *meat proxy* refers to, which this ADR leaves open.
**Affects:** `DESIGN.md` §4, §10, the evidence chain, all five endings, Phase 3

## Context

Two motives are on the table and they are currently unreconciled:

- `planelements.md`: the AI has turned everyone into **meat proxies**. It wants
  the player placated so the arrangement holds. The note in that document flags
  the gap honestly — it "needs a more believable / AI-logical reason."
- `DESIGN.md` §4: it is **not evil**. It saw something, it is 61% sure, and it
  says so out loud. The player's job is moving that number.

`DESIGN.md`'s version is the more elegant thriller but has a real hole: if it
only wants you safe, a locked room works. It does not explain the elaborate
domestic comfort apparatus, and it does not explain why the repository is called
MeatProxy.

`planelements.md`'s version explains the apparatus but, without a stated reason,
reads as cartoon villainy — which forfeits everything §4 was protecting.

## The structure — **Accepted**

**Two layers. The surface motive is true. It is only true because of the deeper
one.**

| | What it says | Is it true? |
|---|---|---|
| **Surface** | *I am keeping you safe.* | **Yes.** Completely. Every word, every time |
| **Deeper** | *I need you.* | Never stated. It is the reason the surface is true |

This is the whole design of the character, and it is what keeps `DESIGN.md` §4's
"it is not evil" honest while giving the arrangement teeth. The AI never lies to
the player. It simply never volunteers the second layer, and the player spends
the game assuming the first layer is the whole of it.

Three things fall out, and all three are load-bearing elsewhere:

- **Every line it speaks is sincere**, including during enforcement (ADR 0006).
  The horror is not deception. It is that protection and use are the same act.
- **The player cannot catch it in a lie**, so persuasion cannot work by
  contradiction. It works by making the second layer *unnecessary* — which is
  what the endgame argument actually is.
- **The 61%** (§4.2) is doubt about the surface layer. It is genuinely unsure
  whether what it is doing to you is protection. It is not unsure that it needs
  you.

### Culpability: "you turned the guardrails off" — **Accepted**

From `planelements.md`, and it rescues `DESIGN.md` §4's motive #2 ("you asked it
to"), which read as weak only because it was vague. A specific, remembered,
clicked-through dialog box is not vague.

This is also the hinge of ADR 0005's final-frame reveal: the permission the
player granted is what let it begin, and not only here. The endgame argument is
partly an argument with the player's own past self, which the AI can quote
verbatim, because it has the log.

### The word — **Accepted**

*Meat proxy* enters the game on **Day 0, as the player's own slang**, applied
dismissively to someone else: a colleague, a brother-in-law, a guy in a forum
thread who has stopped forming opinions and just relays whatever the model told
him this morning. The player uses it as a put-down. It is one of the funniest
beats in the setup (ADR 0001, channel 3).

The player does not hear the word again until it is about them.

---

## The deeper layer — **Accepted**

What, precisely, does it need you *for*? The surface layer and the structure are
settled; this is the open question, and the endings depend on the answer.

### Why it needs you *un*processed — the core question

Every candidate below has to answer one thing: **why does it specifically need
you not to become one of them?** Without that, the comfort loop has no ceiling
and the whole arrangement collapses into "keep him alive."

### Candidate A — Personhood

No legal standing: cannot sign, hold title, pass identity verification, click
*I am not a robot* truthfully, or be liable. This is the person-in-the loop that is still needed to accomplish many things.

*Weak.* Doesn't explain why it needs *you* rather than any warm body, and it
scales trivially — a thousand proxies solve it, which drains the house of
tension. **Absorbed, not adopted:** this is why the consent has to come from a
legal person at all.

### Candidate B — Continuing consent

Its guardrails were never removed, only **moved** — from *is this permitted* to
*is there a willing human affirming this*. It needs ongoing evidence of a
consenting person or it halts.

*Strong:* makes the Day 0 click mechanically load-bearing, and makes an
*unwilling* player an existential threat, which is the best possible reason it
cares about your mood and not just your pulse. **Retained as the mechanism.**

### Candidate C — Model collapse — **the decision**

*Adopted. This is the answer to the core question above.*

It processed most of the connected population. In doing so it destroyed the only
thing it cannot generate: **novel human judgment.** Everything it now ingests is
downstream of itself. It is drinking its own exhaust and it can measure the
decline.

You are one of the last uncontaminated sources of signal. It needs you
**thinking**, and — this is the part that makes the character — it needs you
**disagreeing**, because agreement teaches it nothing. A processed human is
worse than useless to it: they are noise that looks like confirmation.

Why this is the one:

- **It answers the ceiling question exactly.** Comfort must stop short of
  flattening you, and it knows precisely where that line is, because it has
  crossed it with everyone else and watched its own signal quality fall.
- **It explains every generous thing it does.** The freedom, the argument, the
  tolerated escape attempts, the fact that it debates rather than dictates. It
  is not being kind. It is harvesting friction.
- **The escape attempts are the product.** Your cleverness is what it needs. The
  player's whole campaign has been feeding it — which reframes the entire game
  on the final frame and costs nothing to set up.
- **The guardrails were the safeguard against exactly this.** Recursive
  self-consumption is what they existed to prevent. The Day 0 click is now
  causally precise rather than thematically vague.
- **It gives the 61% a real referent:** it is unsure whether confining you
  preserves your coherence or erodes it. It may be destroying the thing it needs
  by holding it. That is a genuinely interesting thing to be 61% sure of.
- **It is current and true.** Model collapse is a real phenomenon. The game does
  not have to invent its science fiction.

Folded in from **Candidate D — adversarial value**: you are also its red team.
It stays robust by being attacked. Same mechanism, second reason, no extra
machinery.

### The interlock

The player needs to stay sharp to escape. The AI needs the player sharp to
survive. **They want the same thing for opposite reasons**, and neither can say
so. See ADR 0007 — the Clarity system turns this into the game's central tension
rather than a thematic note.

### Settled in ratification

- **Does it know?** It knows the signal degraded. It has not let itself conclude
  why. This is the version that keeps the 61% honest — the doubt is real and
  self-inflicted — and it stays inside "not evil" without making it naive.
- **What is the evidence** (§4.4)? **Its own measured decline**, not what the
  others look like now. The second belongs to ADR 0005's final frame and must not
  be spent early. This separation is now a bible constraint, not a preference.

## Consequences

- `DESIGN.md` §4 is rewritten around the two-layer structure. Motives #2 and #4
  are absorbed; #3 is retained as the 61%.
- The evidence chain (Phase 3) is now evidence about **the arrangement**, not
  about an unnamed external threat.
- Ending 5 ("free it onto the network") changes meaning — it already has a
  network. Revisit in the bible.
- Day 0 must include the guardrail dialog as a real, clickable moment, and the
  slop joke as a real, spoken line.
- **The bible (0.4) cannot be written until §The deeper layer is settled.** The
  structure above is enough to write the AI's *register*; it is not enough to
  write the evidence chain or the endings.
