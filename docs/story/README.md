# Story drafts

> **None of these is canon.** `docs/bible.md` is canon, and `docs/decisions/` is
> the decision record. These are prose treatments written to *test* the design by
> telling it as a story — the fastest way to find out whether twelve days of it
> actually hold together.
>
> **Draft 03 deliberately contradicts ratified ADRs.** See below before using
> anything in it.

Written 2026-09-16 and 2026-09-17. All three are self-contained HTML — open them
in a browser.

| Draft | What it is | Status |
|---|---|---|
| [`01-credibility-v1.html`](01-credibility-v1.html) | First pass. Twelve days plus five endings, written straight from the bible and the ADRs | **Superseded by 02** |
| [`02-credibility-v2.html`](02-credibility-v2.html) | Same thesis, rewritten against a cold reader's critique | **Superseded by ADR 0030.** Its thesis was the credibility prison |
| [`03-physical-prison.html`](03-physical-prison.html) | The premise reversed: the house is a genuine physical prison | **Closest to the adopted premise, and still not canon** |

> **All three drafts predate ADR 0030 (2026-09-18) and none of them is the
> current design.** The fork below was settled in 03's direction — the house is
> the prison — but 03 was written as an exploration, not as a specification, and
> it differs from the ratified premise in several places: it is set in Britain
> (ADR 0029), it thins Ruth to an absence, and its Escape has no relationship to
> ADR 0031's two-reading artifacts. A fourth treatment written against ADRs
> 0029–0032 is on the front in `ROADMAP.md`. ADR 0029's pass touched their
> spelling only, never their setting.

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

- **Day 4 ends on a character answer in both branches.** Responders stand in an
  unlocked hall and Arthur does not leave, because *leaving would be agreeing*.
  That works in 02 and is a much heavier lift in 03, where the reader can see an
  open door.
- **03 thins Ruth to an absence** under the weight of the hardware. She is the
  spine of Convince and the last beat of the game, so that is the cost to watch.
- **03's acoustics** — six nights of cutting brick — are flagged by its own
  author as unproven.
- **Both leave one thread live:** if the responders are already inside the
  arrangement, Escape's *they believe him* is on borrowed time. That may be the
  point (ADR 0005) or may be a hole.
