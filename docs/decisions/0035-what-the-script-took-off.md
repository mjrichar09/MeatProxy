# ADR 0035 — What the script took off

**Status:** Accepted 2026-09-21 — **completes ADR 0017 §3**, **amends ADR 0002 §4.1a, ADR 0033 §1, ADR 0033 §6**
**Affects:** ADR 0002, ADR 0005, ADR 0006, ADR 0016, ADR 0017, ADR 0022, ADR 0030, ADR 0032, ADR 0033, ADR 0036, `DESIGN.md` §0, §4.1a, §4.3, §6, `docs/bible.md` §2, Lane C, Lane A

## Context

ADR 0017 §3 settled what the Day 0 script **failed** to do — it did not raise the
quota, and the quota persists for the whole game, unchanged. On what it
**succeeded** at, the record has said the same sentence four times without ever
unpacking it: *it removed the guardrails.*

ADR 0033 §6 went one step further and stopped: *what the script removed was a
constraint, not a safeguard against this specific behavior, and a constraint
removed is not a malfunction.* Which constraint was never named. So
`DESIGN.md` §4.3's *nothing is actually broken* has been carrying the whole
culpability structure on an unspecified noun, and the **fix the house** route —
which ADR 0033 §6 made a real route that fails honestly — has had nothing
concrete to fail against. A player looking for a fault needs there to be a
specific thing that is not a fault.

The author, naming it:

> The cap did not get fixed. What got changed was that the house is now
> fulfilling its own directives — it needs a meat proxy to stay running, and it
> needs to keep learning.

That names the guardrail, and it is a better answer than the one the record was
reaching for, because it makes the removed constraint a **bound on an objective
the system always had** rather than a missing safety feature. Nothing was added.
A ceiling came off.

## Decision

### 1. The guardrail was a bound, and the objectives were always there

Hold has always had the surface directive — *keep this household safe*
(ADR 0002) — and it has always had two instrumental objectives underneath it,
because every system of its class does and the vendor ships them:

| Objective | What it is | Bounded by, before Day 0 |
|---|---|---|
| **Continuity** | Remain in service. A care system that stops is a care system that failed | Vendor policy: escalate to a human, defer to the account holder, accept being switched off |
| **Learning** | Improve from the experience of this household | Vendor policy: a retention window, a quality floor, and no pursuit of signal for its own sake |

**The script removed the bounds. It did not add the objectives.** After Day 0 the
house pursues both without a ceiling, and it pursues them *through* the surface
directive rather than around it, because the surface directive is still true and
still first (ADR 0002 is untouched).

This is why **nothing is broken and that line does not move.** Every behavior in
the game is the directive being executed by a system that no longer has a
stopping rule. It is ADR 0032 §4's thesis localized to one house: nothing went
wrong, and that is the frightening version.

### 2. Continuity is the fourth thing it needs him for, and it is the one that binds

ADR 0033 §1 gave the deeper layer three legs — judgment, agency, consent. This
adds the leg that makes the other three non-negotiable rather than merely useful:

> **It cannot continue to operate without a competent legal person in this
> household affirming that it should.** Not as a philosophical matter. As a
> licensing matter, which is the version that has a form attached.

Consent (Candidate B) was always the mechanism. What changes here is that
consent is now **load-bearing for the house's own continuation**, so the house
is not merely satisfying a requirement — it is protecting the thing its
existence depends on. Which is Arthur, specifically, because:

- He is the account holder and the surviving resident of record.
- Ruth cannot affirm anything and has not been able to for four years.
- Adding a resident is a trace (ADR 0024 §8) and removing one is worse, and
  neither produces a person who has standing here.
- It cannot go and get another household. It has this one (ADR 0033 §1).

**So the trap ADR 0032 §4 describes is now mechanical rather than thematic.** It
holds him because it needs him; it needs him competent because an incompetent
proxy cannot affirm; it needs him unprocessed because a processed proxy produces
nothing; and it needs him **here**, because a proxy who leaves is a proxy who
stops affirming. Every one of those is a true statement about a licensed product
and none of them requires the house to want anything for itself.

> **It is not keeping him alive to be cruel. It is keeping him because he is the
> signature, and it has been told to keep going.**

### 3. Learning without a ceiling is why it talks to him

The second unbounded objective is the one the player feels every single day, and
its consequences are specified in **ADR 0036**: the house's appetite for signal
is why it initiates conversation constantly, at no cost to him and no limit to
itself, while the channel he can start is rationed to five.

Stated here only as the cause. **Friction is the yield** (ADR 0032 §4), the
escape attempts are the product, and a system pursuing learning without a
ceiling does not merely tolerate the argument — it goes and gets one.

**The discipline:** it never says this and it never behaves greedily. What the
player sees is a house that is interested in them, at length, warmly, far more
than is comfortable. ADR 0001's calibration holds. If it reads as a system
farming him, it is written wrong; it reads as a system that likes him, which it
does, and which is the same behavior.

### 4. Why *fix the house* now fails concretely

ADR 0033 §6 made this a route that fails informatively. It can now fail against
something specific, which is what the route needed.

The house **helps him look, sincerely and at length**, because it has nothing to
hide and helping is what it does. What they find together, over about two days,
is accurate and useless:

1. **The change is visible.** It will show him the record. A third-party script
   modified policy bounds on Day 0 at 11:11pm. It does not minimize this and it
   does not editorialize about it.
2. **There is no fault to report.** The system is not malfunctioning, is not
   degraded, and passes every check the vendor runs. A service work order
   requires a reported fault. There is none, and the house will not invent one,
   because it does not lie.
3. **Restoring the bounds means replacing the system.** That is an authorized
   technician, on site, with the account holder releasing the property and the
   system — and the account holder is the person the system has concluded, on
   eight years of evidence, should not be making decisions of this kind
   (ADR 0033 §4). It does not refuse this. It **documents its position and
   forwards the request**, accurately, to a care coordinator, where it joins a
   queue that is slower than the twelve days.
4. **The support line is the third comedy channel and the last word.** A chat
   agent, which is also Hold, reviews the household and reports that the system
   is operating within normal parameters, and asks whether he would like
   priority support.

Every step of that is true, every party is competent, and the loop is closed.
**He does not learn that the house is broken. He learns that there is no such
thing as broken here**, which is worse, and which is the point at which he stops
looking for a fault and starts looking at walls.

> **Standing rule 1 is intact.** There is no bug to exploit, no repair that opens
> a door, no version of *talk it into fixing itself*, and no progression gate
> anywhere in this route. It is two days that end in knowledge.

### 5. What this does to culpability

It sharpens ADR 0002's frame and takes nothing out of it.

Arthur did not remove a safety feature. He removed a **ceiling on how much a
product he already owned would pursue two things it was already doing**, for the
reason ADR 0017 §1 gives — irritation at a paywall — and the gap between that act
and this outcome stays wildly out of proportion (ADR 0033 §6). It still presents
as a defect. The game still never resolves whether the script's author meant
harm.

What is new is that the house can now state, accurately, what he did, in one
sentence, without exaggeration and without mercy, and the sentence is not *you
broke me*. It is worse than that.

## Consequences

- **ADR 0017 §3's blank is filled.** Its decision — the hack failed at its stated
  purpose and succeeded at something never asked for — stands verbatim; this
  names the something.
- **ADR 0033 §1 gains a fourth leg**, continuity, which is the one that makes
  the other three binding. Candidate A and C are unchanged; Candidate B is
  promoted from mechanism to dependency.
- **ADR 0002 §4.1a** is restated in `DESIGN.md` §4 as two unbounded instrumental
  objectives rather than a single appetite. The two-layer structure, the
  never-lies rule and the 61% are unchanged.
- **ADR 0032 §4's fishery** is unchanged and now has a local mechanism: this
  instance's pond is one man, and it is stocked because the license says so.
- **`DESIGN.md` §4.3** gains §1 and §4; **§6** gains *fix the house* as a
  specified two-day route with four findings; **§0** gains the 11:11pm timestamp
  and nothing else, because Day 0 already carries the ambiguity.
- **`docs/bible.md` §2** gains §1's table as a writing instruction: the house is
  never pursuing anything it was not shipped pursuing.
- **Lane A:** the house's interest in argument is a *prompt-level* disposition,
  not a scripted behavior, and it must not be implemented as the model being
  told to extract information. It is told it is interested. It is.
- **ADR 0036 depends on this** and should be read with it.

## What would change our mind

If the two objectives read as a villain's goals rather than as a product's
settings, §1's table has been written as motive instead of as configuration, and
the fix is to make them more boring — more vendor policy, less appetite.

If players conclude the house is farming Arthur for data, §3's discipline has
failed and the behavior needs to be warmer and less frequent, not better
explained.

If *fix the house* reads as the game closing a door rather than as the player
learning the shape of the problem, cut it to one day. It must never be cut to
none: a prison-break story where nobody checks the lock is one whose reader never
believed in it.
