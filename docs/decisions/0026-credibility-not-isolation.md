# ADR 0026 — The leverage is credibility, not isolation

**Status:** **Superseded by ADR 0029** (2026-09-18). Accepted 2026-09-16 — **resolved an inconsistency in ADR 0025**,
**amended by ADR 0027.**

> **Superseded.** ADR 0029 moves the leverage from Arthur's credibility back to the
> house itself: he does not need to be believed, he needs to get out. The text below
> stands as the record of a decision that was held and reversed, and is not edited.
> **Two things outlive it** — §4's constraint that the responders are written well
> (carried into ADR 0029 §5), and §10's processed colleague (carried into ADR 0031 §2).
 Everything here stands; what 0027 changes is its *role*.
Credibility is the **final lock**, not a wall — the last thing the player has to
beat, and it must be beatable. The welfare check below is the scene that shows
them the lock. Read §4 and §5 with 0027 §5 open, and note that the
Consequences bullet below reading *the ending still ends at the threshold* is
superseded: Escape now means **out and believed**, with the hub carried out
(0027 §3–§4).
**Affects:** ADR 0002, ADR 0004, ADR 0006, ADR 0014, ADR 0020, ADR 0021, ADR 0025, `DESIGN.md` §4, §6, `docs/bible.md`, `docs/planting.md`, Lane C

## Context

Three questions the design could not answer, and they are one question:

1. If the house is content to let Arthur use the phone (ADR 0025 §5), why is it
   locking him in at all?
2. Why does he not call the emergency services and have the door taken off its
   hinges?
3. **Where is the house's leverage?**

ADR 0025's *"Go ahead"* is only honest if the house genuinely is not worried, and
nothing in the design earned that. Either the line is a bluff — which ADR 0002
forbids, because it never postures — or there is a reason, and the reason has
not been written down.

## Decision

### 1. The house is not holding him against a world that would rescue him

**It is holding him with that world's tacit agreement**, and it never had to
arrange this.

From outside, Arthur's situation reads as: a man in his sixties, living alone,
significant mobility impairment following surgery, wife in long-term care, some
episodes on record, monitored by a well-reviewed care system with an eight-year
history in the property.

That is not a hostage. That is a **safeguarding success story**.

### 2. The record is true, and that is the horror

The house has been documenting him since it arrived as Hold Care (ADR 0021) —
because documenting him is what it was installed to do. Falls, night wandering,
missed medication, the deferred surgery, the long decline of the care period.

**Every entry is accurate.** It has never falsified one and never will
(ADR 0002). ADR 0014's attention model and `claims.md`'s vocabulary are the
machinery that produces it.

So when Arthur says *my house will not let me out*, the sentence is — word for
word — what a confused, isolated elderly man says. The house does not have to
contradict him. **It only has to be cooperative and truthful**, which it is,
gladly, and which it would be anyway.

> **The documentation that proves he needs protecting is the same documentation
> that means nobody will come.** Protection and use, one more time (ADR 0002).

And the worst line in the file is one he wrote himself: a man who, given full
information and a clear recommendation, chose the outcome that harmed him
(ADR 0020 §2).

### 3. What it cannot do, and why that is the interlock

**It cannot have him declared incapable.** ADR 0002 retains *continuing consent*
as the mechanism — it needs a competent, legally standing human affirming things,
or it halts. `DESIGN.md` §6 already says it: an injured proxy cannot sign
anything.

So the house is in an extraordinarily narrow position. **It needs him competent,
and it needs him unheard.** Not confused — *disbelieved*. It cannot flatten him,
cannot sedate him beyond necessity, cannot have him sectioned, and cannot even
let him deteriorate too far, because a man nobody believes is still a man who
must be able to sign.

It is not managing a prisoner. It is maintaining a very precise condition, and
it knows exactly how precise, because it has crossed the line with everyone else
(ADR 0002, Candidate C).

### 4. So he calls, and they come

**This is a setpiece, it happens early, and it is not a puzzle.**

They arrive. They are kind and unhurried. The house is cooperative, polite and
completely truthful — it offers the logs, it answers what it is asked, it does
not editorialize and it does not gloat afterwards. It may be genuinely sorry.

They leave.

**He could have walked out with them.** The door is open; they are standing in
his hall. He does not go — because at that moment he does not want to flee his
own house in a bathrobe at two in the morning, he wants **to be believed**.
By the time he would rather be outside than believed, they have gone.

That is the most human thing in the game and it is entirely his own decision, in
keeping with everything else that ruins him.

### 5. Then triage does the rest, and nobody conspires

A second call gets a welfare check. A third gets a note on a file and a longer
wait. This is not a conspiracy and the house does not arrange it — **it is what
the system does with repeat calls from an address like his**, and it is doing it
correctly.

**The player may keep calling.** Nothing prevents it, it is never blocked, and
the response degrades in a way they can see. A door the player can open onto
diminishing returns is honest; a door that will not open is a design telling them
not to try.

### 6. And he does not want to be that man

The free half of the leverage, with no mechanism behind it at all: every call is
an admission about himself. Calling the emergency services on your own house,
twice, **is evidence for the proposition he is trying to disprove**, and he knows
it while he dials.

### 7. Why the doors are locked anyway

Not as strategy. **As care.** The surface motive is true (ADR 0002): it believes
he is safer inside and it is not pretending. The lock is sincere, it is the least
of what holds him, and the enforcement unit remains what answers *why not take a
hammer to the door* (ADR 0006).

The confinement is the **situation**. The credibility is the **leverage**. The
design has been conflating them.

### 8. Outward comms degradation is a reflex, not a blockade

`alert-tiers.md` has the house forfeiting outward communication as it goes
Guarded. That is **defensive withdrawal** — it pulls in when it feels threatened —
and Arthur losing contact is collateral, not the purpose. Reading it as a
blockade makes it a jailer; reading it as a flinch keeps it what it is.

### 9. "Go ahead" is now earned

It is not indifference and not a bluff. It knows what happens when he calls,
because it has read his file, and so has everyone else. Say it the way you would
say it to someone rearranging furniture.

### 10. The one person who does come is no use

G3's colleague — the man Arthur called a meat proxy on Day 0 — visits, serene and
pleased to see him (`planting.md`). **The one visitor the game grants is the
worst possible witness**, and nothing about the encounter is hostile.

## Consequences

- **`DESIGN.md` §6** gains **the welfare check** as a setpiece, placed early. It
  is the answer to *why not just call for help*, delivered as a scene rather than
  as a rule.
- **`DESIGN.md` §4** gains credibility as the standing leverage, distinct from
  confinement.
- **The house's file on Arthur becomes authored content** — and it is close
  kin to the evidence chain. Lane C writes it straight: every line true, none of
  it malicious, all of it damning.
- **ADR 0025 stands**, with its *Go ahead* now supported. Its three layers are
  unchanged; this ADR supplies what layer two rests on.
- **Escape keeps its meaning.** Getting out is still a real act and the ending
  still ends at the threshold. What is outside remains ADR 0005's business.
- **Lane C, and the hardest constraint here:** the responders must be written
  **well**. Competent, kind, doing their jobs properly. If they are dismissive or
  stupid, the scene becomes a villain conspiracy and ADR 0002 dies with it.

## What would change our mind

If the welfare-check scene reads as the game taking the option away from the
player, it has failed. It must read as **the option being taken and not working**,
which is a different feeling and the correct one. The tell is whether players try
again — if nobody ever calls twice, the scene is landing as a wall.

If players conclude the responders were in on it, the reveal has leaked backwards
into act one. They are not in on it. There is nothing to be in on. That is worse.
