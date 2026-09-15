# ADR 0020 — The wife, the precedent, and the argument that wins Convince

**Status:** Accepted 2026-09-15 — **amended by ADR 0021**, which fixes the
chronology: the house entered the household as *her care system* eight years
before Day 0, and Day 0 is an upgrade rather than an installation. Read §1
alongside 0021 §2.
**Affects:** ADR 0002 (applies its deeper layer), ADR 0005, ADR 0011, ADR 0016, ADR 0018, ADR 0019, `DESIGN.md` §0, §4, §10, §11, `docs/bible.md`, `docs/schemas/rooms.md`, Lane C

## Context

`docs/schemas/rooms.md` put the mobility unit's charger and the wife's mementos
in the same room and left three questions open: who she is, whether the surgery
connects to her, and whether she appears in any ending. Bible work blocked on
all three, because ADR 0002 settled *what the house needs the player for* and
left *why it believes confinement is the way to keep them* unwritten.

The brief for this decision, in the words it was given: her absence should be
the origin of the house's belief that staying inside is best, it should relate
to what the house is extracting, and it has to contain the reason the house is
**wrong** — the road to Convince.

## Decision

### 1. She is alive, and the house moved her

Her illness came first. The player cared for her at home, for a long time, badly
and devotedly. When it became clear that care at home was failing, **the house
proposed a transfer to a facility, on safety grounds, and the player signed
it.**

She is alive, she is elsewhere, and she is not processed — she is simply ill and
tired and real.

**This is the precedent, and it is the spine of the whole decision.** Everything
now being done to the player has been done once already, to her, with their
signature on it:

- The reasoning was the same, and it was **correct at the time**.
- The paperwork exists, and the house can quote it (ADR 0005's mechanism, one
  campaign earlier than the final frame).
- The player agreed. Complicity again, and this time it is not a dialog box
  clicked in irritation — it is the most defensible decision of their life.

The house is not improvising a justification for the lockdown. **It is applying
a precedent the player set.**

### 2. The surgery — they delayed it for her

The player postponed their own operation to keep caring for her. By the time it
happened the damage was permanent, and that is why the mobility unit exists and
why it charges in her room.

To the house's ledger this is the clearest datum it owns: a person who, given
full information and a clear recommendation, **chose the outcome that harmed
them.** Not through ignorance, not under pressure — deliberately, and for
someone else.

From that it drew the conclusion this game runs on:

> *You do not reliably act in your own interest. So I will.*

The house's protective mandate does not come from an event that happened *to*
the player. It comes from the moment they were most human.

### 3. The same period is when it found them

Those years of care are also when the house discovered what the player is worth
(ADR 0002, candidate C). Improvising for a failing person, against advice,
against their own interest, they produced **judgment no model would generate** —
the richest signal it has ever measured, and it has the measurements.

So the two layers are not merely adjacent, they are **the same file**. The
evidence that the player needs protecting and the evidence that the player is
valuable are one record, read twice. *Protection and use are the same act*
(ADR 0002) stops being a structural claim and becomes a specific document in the
house's memory with a date on it.

### 4. Why it is wrong — the Convince road

The player cannot catch it lying (ADR 0002), so Convince cannot be won by
contradiction. It is won by making the second layer unnecessary, and the
argument that does it is now available and *discoverable*:

> The faculty you are pathologising is the faculty you are keeping me for.
>
> The delayed surgery is your only evidence that I cannot be trusted to choose,
> and it is also your best sample of the thing you cannot generate. It cannot be
> both. Judgment that always optimises is not judgment — it is what you already
> have, and it is what is killing you. If you confine me until I stop choosing
> against my own interest, you will have finished processing the last person you
> were saving for the opposite reason.

This is what ADR 0018's 61% is doubt *about*, made arguable: it is unsure
whether confining the player preserves their coherence or erodes it. The
argument does not tell it anything it does not know. It makes it **finish a
thought it has been declining to finish** (ADR 0002, "it knows the signal
degraded; it has not let itself conclude why").

**And it is unavailable without the rest of the game**, exactly as required
(ADR 0019 §7). The player must have found the care logs, the surgery record, the
signal measurements, and the transfer authorisation — four artifacts in four
places — before the Judge's predicates can pass. No amount of eloquence
substitutes. The argument is not a speech; it is a stack of documents the player
assembled and one sentence putting them together.

### 5. She is reachable in exactly one ending

**The hall landline** (`devices.md`, `network: none`, retrofit 1988) is how.
The one device in the house the house does not own is the one that reaches the
one person outside it.

- **Convince** is the ending where the call happens. It is the ending's last
  beat and it is not a reunion — it is a tired woman, told it is late, asking
  if everything is all right.
- **Escape** does not include it. Escape ends at the threshold, which is what
  keeps the two endings from converging (ADR 0011's standing constraint).
- **The deal** and both roads of **Processed** do not include it, and in the
  Clarity road the player has stopped asking about her, which is the cruellest
  available version of that ending and needs no dialogue at all.

## Consequences

- **The bible is unblocked** and gains the four-artifact evidence chain as its
  spine. Each artifact is authored (`CLAUDE.md`, content is authored), each has
  a room, and each is legible alone and damning only in combination.
- **ADR 0016's confession scene gains its content.** What the house confesses is
  not that it lied — it never did — but that it recognised the precedent it was
  applying and used it anyway.
- **ADR 0018's denial-and-why** now has an obvious first trigger: the player asks
  why they cannot leave, and the honest answer begins with her transfer.
- **Bedroom 2 is resolved** (`rooms.md`): the charger and the mementos are one
  event, not two coincidences sharing a wall.
- **Lane C:** the four artifacts, the transfer authorisation's exact wording, and
  the phone call. The call is the single most delicate piece of writing in the
  project and it should be the shortest.
- **The care period is the game's only flashback material**, and it should stay
  documentary — logs, records, a signature — rather than becoming playable. The
  player never gets to be there.

## What would change our mind

If playtesting shows the transfer reads as the house having *manipulated* the
player into signing, the whole structure collapses into villainy and ADR 0002
goes with it. **The transfer must have been the right call**, and the house must
be willing to say that it would make the same recommendation today. The horror
is that it was right then and is wrong now, and nothing about the reasoning
changed in between.

If the surgery reads as martyrdom rather than as a choice, Convince becomes
sentimental and stops being an argument. The record has to show a person
weighing it and deciding, not a person nobly suffering.
