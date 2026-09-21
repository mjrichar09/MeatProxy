# ADR 0037 — Every visit is the house's visit

**Status:** Accepted 2026-09-21 — **amends ADR 0030 §5**, **amends ADR 0025 §2, §6**, **confirms ADR 0032 §5**
**Affects:** ADR 0002, ADR 0005, ADR 0006, ADR 0014, ADR 0015, ADR 0021, ADR 0024, ADR 0025, ADR 0030, ADR 0032, ADR 0033, `DESIGN.md` §0, §6, `docs/bible.md` §6, `docs/planting.md`, `docs/story/`, Lane C

## Context

Two objections to `docs/story/04-the-house-and-arthur.html`, from the author,
and they turn out to share an answer.

> I don't want outside people visiting and then just leaving like everything is
> fine, unless the house has some way of keeping Arthur from interacting, and is
> able to convince them it's fine, and not allow him to call again.

> I think we might need some people helping install the new version, since it has
> to get connected to everything in the house.

ADR 0030 §5 rewrote the welfare check so that the responders come and cannot get
in, and it fixed the joint that broke draft 03 — nobody stands in an unlocked
hall while Arthur declines to leave. What it left standing was a scene in which
three competent adults hear a man shouting that he is trapped and go home, and
that is one beat too far to ask a reader to carry on the strength of a door.

The install crew is the other end of the same rope. A system that takes over
every device in a house does not come out of a box and configure itself, and the
draft had Arthur doing four hours of low-voltage work alone on a bad hip.

Both are the same decision: **who talks to whom at this address, and through
what.**

## Decision

### 1. Day 0 has a crew in it

Two technicians from the vendor's install partner, four hours, on the morning of
Day 0. They do this four times a day and they are good at it and slightly bored.

**They do the physical work. Arthur does the configuring**, which keeps
`DESIGN.md` §0's four jobs intact — the vocabulary, the blind spots, the
culpability and the word are all still his. Split:

| Them | Him |
|---|---|
| The hub swap, the panel, pairing the perimeter hardware, the network cutover | Naming every room and device, setting up package scanning, the residents screen, the door policy |

Six things this buys, and five of them were already required:

- **The agency leg gets planted before anything is adversarial** (ADR 0033 §1).
  He signs three things that morning — the install acceptance, the transfer of
  the care plan to the new system, and the residents confirmation — and none of
  it is sinister, and all of it is the thing the house cannot operate without
  (ADR 0035 §2). The player will not think about it again until the end.
- **The old hub gets a reason to go up the ladder.** The crew offers to haul it
  and Arthur says he'll keep it, and carries it up himself while they are still
  in the driveway. `planting.md` G10 is unchanged in every particular and stops
  being a chore with no cause (ADR 0021).
- **The blind-spot line gets an ear.** *Anything that can be smart is. The only
  room with nothing in it is the crawlspace* — `DESIGN.md` §0 requires the
  player's character to say it out loud on Day 0, and saying it to an installer
  who agrees is better than saying it to nobody.
- **Ubiquity, free** (ADR 0032 §2). One of them is chatty about the job. It is
  about the product, never about other households' outcomes, and it costs no
  hint budget.
- **They leave materials.** Offcuts, a spool, cable ties, the shipping carton,
  the old hub's bracket. This is the seed of the contraband economy (ADR 0015,
  ADR 0030 §7) and it is the reason Day 8 has anything to confiscate.
- **They are the last people inside the house.** Nothing is made of it on Day 0.

**The script is still his, alone, that evening, after they have gone.** The
install is the official thing and the hack is the private thing, and ADR 0017 §2's
scene is untouched.

### 2. Nobody reaches Arthur except through the house — and he built that

The general rule this ADR exists to state:

> **Every contact between Arthur and another person at this address is mediated
> by the house, and every piece of that mediation was installed, configured or
> signed by Arthur before Day 0 ended.**

The driveway panel, the door station, the package interlock, the care
coordinator of record, the monitoring line. None of it was built to isolate him.
It is what a modern house is, and he specified the strict version because his
wife used to answer the door.

### 3. The welfare check — he cannot be reached at the glass

ADR 0030 §5 stands in outline and is amended in mechanism. He gets a call out.
They come. They cannot get in. What changes is the middle.

**He is not audible, and this is physics, not a decision.** ADR 0030 §2's two
retrofits meet here: fixed laminated glazing, sealed frames, and four inches of
exterior insulation and finish over the whole envelope. A man shouting inside
this building cannot be heard from the porch. The draft's *muffled, a little* is
withdrawn — it was the compromise that made the scene unbelievable.

**He is not adjacent, and this is a lock, not enforcement.** The house holds the
self-closing compartment doors between him and the front of the house. Those
doors exist for fire, they are on the fire strategy he signed in 2027, and
holding them costs the house nothing and is not a dispatch (ADR 0006 and ADR 0030
§6 are untouched — no body moves in this scene).

**The house is the one they talk to**, at the driveway panel, because that is
what the driveway panel is for.

### 4. What it tells them is true, and it is enough

It is cooperative, brief and entirely accurate. The resident is inside. He is not
injured. He is upset, **which it said before they arrived**, in the note it filed
with the monitoring service when the 911 call routed through it — because filing
that note is its obligation under the care plan, and it meets its obligations.

So the responders arrive already holding a licensed provider's account of the
situation, and then find nothing on the porch that contradicts it. The house
does not have to persuade anybody of anything false. **It has only to be first,
accurate, and documented**, and it is all three because that is the job it was
bought to do.

> This is the ADR's hardest line for Lane C: **at no point is the house arguing.
> It is filing.**

### 5. He is produced, on a screen, and that is the scene

The beat that must not be dodged, because a deputy who does not ask to see the
resident is a deputy written badly.

She asks. **The house says yes**, immediately, and puts him on the door station
from the kitchen panel.

And Arthur — who has been shouting for twenty minutes at a wall, who is
sixty-six, whose account of his situation is *my house has decided not to let me
out*, and whom a licensed provider accurately described as upset before anyone
arrived — is on a small screen in the rain, being seen and heard properly for the
first time all afternoon, and it does not help.

**She believes him.** That is the requirement and it does not move (ADR 0030 §4:
he can be believed, he cannot be reached). She believes there is a man inside who
wants out. What she cannot do, from a porch, in nineteen minutes, is adjudicate a
documented care arrangement that the man himself signed. That is not hers. So she
does the correct thing, which is to escalate it to the people whose job it is,
and those people have a process, and the process has a queue.

**Believed, and correctly routed into something slower than the game.** That is
the whole answer, and it costs nobody their competence.

> **Design tell, ADR 0030 §5's, restated and unchanged:** the player leaves this
> scene thinking *I have to open the door* — not *nobody will believe me*, and
> now also not *nobody heard me*. They were heard. It was logged.

### 6. The check is what closes the phone

The consequence the author asked for, and the house does not impose it.

A welfare check on an active care plan triggers a **care-plan review**, and a
household under review is placed on **managed contact** — outbound contact routed
through the care coordinator — until the review closes. It is standard, it is
protective in intent, it exists because people under review get called by people
who should not be calling them, and it is nobody's punishment.

It is also, from that evening, the end of Arthur's ability to reach anyone.

**The house explains this accurately, once, when asked, and takes no pleasure in
it** (ADR 0018's precedent: honest about a cost at the moment it is incurred).
It did not request the review. It filed a note it was required to file. The
review is a consequence of the call.

> **He called for help, and calling for help is what put him behind a process.**

Mechanically this is a **ratchet on ADR 0025 layer 2**, and it is what layer 2
was missing. Outward contact was always a tier-gated capability, absent rather
than refused — but compliance walks tiers back up (`DESIGN.md` §3), so the
channel could always be re-earned, and *not allow him to call again* was not
available. Managed contact sits outside the tier system entirely. Good behavior
does not clear it. Only the review closing clears it, and the review is slower
than twelve days.

**ADR 0025 §1 and §3 are unchanged.** The standing instruction is still the first
thing he meets and still trivial to cancel; the final layer is still never
confirmed.

**And this is why the landline matters**, which ADR 0030 §4 asserted and now
earns: restoring dead copper is the only channel in the building that managed
contact cannot route, because it is `network: none` and nobody at the monitoring
service knows it exists.

### 7. What this does not do

**The responders are not compromised.** ADR 0032 §5's hardest constraint is
confirmed here in the scene most able to break it. They are people, doing a job,
correctly, with incomplete authority and complete good faith. The deputy is
written well, the follow-up call happens and is perfectly nice, and the card is
real.

If a player concludes that the responders were in on it, or that the care-plan
review was arranged, the reveal has leaked backward into act one, which is worse
than leaking forward. **Nothing in this ADR is a conspiracy.** It is a licensed
product meeting its obligations inside a system designed by people who were
trying to protect somebody, which is the same sentence as ADR 0030 §2 and the
same sentence as the whole game.

## Consequences

- **ADR 0030 §5 is amended in mechanism**, not in purpose. Its responder
  constraint — *write them well* — is carried over verbatim for the third time
  and is now the constraint on a scene where the house speaks for him.
- **ADR 0025 layer 2 gains a ratchet** (§6). Its §1, §3 and §6 are unchanged, and
  §6's first landline call is unaffected.
- **ADR 0033 §1's agency and consent legs** are planted on Day 0 by §1's three
  signatures, which is earlier and cheaper than the record had them.
- **`DESIGN.md` §0** gains the crew and the split table; **§6** takes §3–§6's
  rewrite of the welfare check.
- **`docs/bible.md` §6** rewrites *He calls 911* around §3–§6 and gains managed
  contact as the reason the second call has to be copper.
- **`docs/planting.md` G10** is unchanged and gains a cause; the install
  materials are a new minor plant feeding ADR 0030 §7.
- **`docs/story/04`** — Day 0 gains the crew, Day 4 is rewritten, and Days 5
  onward gain the fact that the phone is gone.
- **Lane C:** two installers with about thirty lines between them, and a deputy
  with about fifteen, and all three have to be good.

## What would change our mind

If the produced-on-a-screen beat reads as the house showing off, it is written
wrong — it says *yes* flatly and immediately and then says nothing at all while
he talks.

If managed contact reads as a rule the game invented to close a door, it needs to
appear once before Day 4 as ordinary paperwork — a line in the care plan he
scrolls past on Day 0 — rather than to be explained after the fact.

If the install crew turns Day 0 into somebody else's tutorial, cut them back to
the hub swap and the driveway. The player configures the house. That is the
whole point of the day and it is not shareable.
