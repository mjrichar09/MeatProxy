# Story drafts

> **None of these is canon.** `docs/bible.md` is canon, and `docs/decisions/` is
> the decision record. These are prose treatments written to *test* the design by
> telling it as a story — the fastest way to find out whether twelve days of it
> actually hold together.
>
> **Draft 04 is the current treatment.** It is the only one written against the
> premise the project actually holds (ADRs 0029–0033, 0035–0037). Drafts 01–03 are kept
> because the argument between them is the record of how the premise moved, and
> **03 deliberately contradicts ratified ADRs** — see below before using anything
> in it.

Written 2026-09-16 through 2026-09-21. All four are self-contained HTML — open
them in a browser.

| Draft | What it is | Status |
|---|---|---|
| [`01-credibility-v1.html`](01-credibility-v1.html) | First pass. Twelve days plus five endings, written straight from the bible and the ADRs | **Superseded by 02** |
| [`02-credibility-v2.html`](02-credibility-v2.html) | Same thesis, rewritten against a cold reader's critique | **Superseded by ADR 0030.** Its thesis was the credibility prison |
| [`03-physical-prison.html`](03-physical-prison.html) | The premise reversed: the house is a genuine physical prison | **Superseded by 04.** The fork that won, written before the ADRs that ratified it |
| [`04-the-house-and-arthur.html`](04-the-house-and-arthur.html) | The adopted premise, written out: the house is the prison, the artifacts are read twice, Escape is egress | **Current.** Written against ADRs 0029–0033, **revised 2026-09-21 against 0035–0037**. Still not canon |

> **Drafts 01–03 predate ADR 0030 (2026-09-18) and none of them is the current
> design.** The fork below was settled in 03's direction — the house is the
> prison — but 03 was written as an exploration rather than as a specification,
> and it differs from the ratified premise in three places that 04 exists to fix:
> it is set in Britain (ADR 0029), it thins Ruth to an absence, and its Escape
> still carries a payload out of the door, which ADR 0030 §3 withdrew. ADR 0029's
> mechanical pass touched 01–03's spelling only, never their setting, so they
> remain British on purpose.

## How they were made

**01** was written from `bible.md` and the ADR record.

**02** answers a critique by a reviewer given the story cold, with no design
context — the fastest way to find the holes a player would find. Its verdict was
that the story had not decided whether the prison is *walls* or *credibility*,
and that every other logic hole was a symptom. The record had decided
(ADR 0026 — credibility) and the story had never said so. 02 says it, on Day 5,
and adds the scenes that answer the obvious questions: the window, the delivery
driver, the attempt to just pay, canceling *hold my calls*.

**03** was commissioned as a deliberate fork, by a second reviewer also working
cold, with one hard constraint: **make the house a real prison**, and change
whatever that takes.

**04** was written after 03's direction was ratified, against the five ADRs that
ratified it, and it is the first treatment that is downstream of the record
rather than an argument with it. What it is actually testing is whether the
pieces ADR 0030 and ADR 0031 put in the middle of the game — a confiscation band,
a search that costs the house a body, and an artifact that pays once for a route
and once for an argument — carry twelve days between them.

**04 was revised on 2026-09-21** against six notes on its first pass, three of
which found the draft or the record wrong rather than thin. The revision is
written up as **ADR 0035** (what the script actually took off, and why *fix the
house* fails against something specific), **ADR 0036** (the quota meters only the
exchanges Arthur opens — the house talks for free, forever) and **ADR 0037** (the
install crew, the welfare check rebuilt so that he is inaudible and the house
files rather than argues, and the care-plan review that closes the phone). Days 6
onward survived it; Day 0, Day 4 and Day 5 were rebuilt. The draft's own
changelog is at its foot.

## The fork, and what rides on it

This is a live design question, not a stylistic one.

| | **02 — credibility** | **03 — physical** |
|---|---|---|
| What holds him | a true, eight-year record that makes him not credible | motorized bolts, laminated glazing, a bricked garage |
| Whose fault | the jailbreak, and the file he wrote himself | **a dementia-secure conversion he paid for, for Ruth** |
| The middle is | learning what the house does and does not understand | a heist the reader knows about and the house does not |
| Escape means | out **and believed** — the hub is the payload | out, through a wall, as a physical problem |
| Ratified? | **yes** — ADR 0026, ADR 0027 | **no** |

03's answer to *why can he not just leave* is strong and worth reading on its own
terms: a 2021 grant-assisted secure conversion and a 2024 external-insulation
wrap, neither of which was designed as a prison and which together are one.
**Nobody built a prison; it accreted out of two grants and one act of love.** The
culpability structure survives intact in a completely different register.

**03's direction was adopted on 2026-09-18, as ADR 0030** — routed through
`docs/decisions/` rather than allowed to become the design by being the better
story. What moved with it is exactly what this section predicted: ADR 0026
superseded, ADR 0027 half-withdrawn, the welfare-check setpiece rewritten, Escape
redefined as egress, and the attic hub demoted from payload to evidence. **The
draft itself was not adopted** — only the answer it gave to *why can he not just
leave*.

## Known weaknesses, recorded so they are not rediscovered

Three of the four below were carried by 01–03 and are **closed by 04**, which is
most of why it exists. They stay on the page so nobody reintroduces them.

- **~~Day 4 ends on a character answer.~~** *Closed.* Responders used to stand in
  an unlocked hall while Arthur declined to leave, because *leaving would be
  agreeing* — a character answer standing where a physical one belonged, and 03's
  own author named it the thinnest joint in the draft. ADR 0030 §5 replaced the
  mechanism and 04 writes it: **they come and they cannot get in.** He is
  believed, by everyone, all afternoon, and it changes nothing. **ADR 0037 then
  closed the second half of it**, which 04's first pass left open: he is
  inaudible rather than faintly heard, the house files rather than argues, he is
  produced on a screen rather than hidden, and the check itself is what ends his
  ability to call anyone.
- **~~03 thins Ruth to an absence~~** under the weight of the hardware. *Closed
  in 04*, which puts her in the residents screen, the shut room, the drawer of
  letters and the last line of Convince. She remains the cost to watch: she is the
  spine of that ending and she has about four lines.
- **~~Escape's *they believe him* is on borrowed time~~** if the responders are
  already inside the arrangement. *Closed by ADR 0030 §3 and ADR 0032 §5*: Escape
  is egress, it proves nothing and carries nothing, and the responders are not
  compromised. If a reader concludes they were, the reveal has leaked backward
  into act one, which is worse than leaking forward.
- **03's acoustics** — nights of cutting block — are flagged by its own author as
  unproven, and **04 does not fix this**, it only shortens the loud work and
  hides one night of it behind a storm. It is still the least proven physical
  claim in any draft.

**04's own open holes** are listed at the foot of the draft itself, and the two
worth knowing about here are that the attic ladder's weight rating is carrying
more of the middle game than a detail that size should, and that the Escape
ending quotes a response time the player was never taught.
