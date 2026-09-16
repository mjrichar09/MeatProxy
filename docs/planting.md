# Planting — guns, MacGuffins, and vouchers

> **Bible precursor.** `docs/bible.md` is a D3 deliverable and does not exist yet.
> This file holds the narrative planting decisions so they are not lost, and folds
> into the bible when D3 opens. It is not authored content — it is the schedule of
> what must be planted, where, and what it pays off.

Day 0 (`DESIGN.md` §0) is the planting ground. It is playable, it is the tutorial,
and the player is *delighted* throughout — which is exactly the state in which
people fail to notice they are being handed things.

**Eleven guns as of 2026-09-16, and that is the ceiling.** Further ideas replace
an entry rather than adding one.

**A discipline first.** Every gun is content cost twice: once to plant, once to
fire. ADR 0009 sized this project at ~12 in-game days. The set below is
deliberately small, and the correct response to a new idea is usually to replace
an entry rather than add one. A game where *everything* turns out to matter
teaches players to treat every object as a puzzle piece, which is its own kind of
broken.

---

## Chekhov's guns

Planted in view, fired later. The rule: **each one is useful or charming on Day 0
in its own right**, so it is never obviously a plant.

| # | Planted on Day 0 as… | Fires as… | Ties to |
|---|---|---|---|
| **G11** | *"Hold, hold my calls."* — said cheerfully on Day 0 while installing, because he is busy | It never stopped. It is why nobody has reached him, and the house can quote him accurately | ADR 0024, ADR 0005, ADR 0002 |
| **G1** | The line about the retrofit — *anything that can be smart is; the only room with nothing in it is the crawlspace* | The escape route, and the reason it exists | §0, ADR 0010 |
| **G10** | Carrying the old hub up to the attic, because the new one arrived and nobody throws these out | **E1** — the care-period logs, now beyond the house's reach, hidden by the player's own hands | ADR 0021, ADR 0020 |
| **G2** | A permissions dialog you read and click through, because the cheap tier's daily cap ran out mid-question and you had to pick a script blind | The culpability reveal — you did this, and the rate limit is why | ADR 0002, ADR 0005, ADR 0017 |
| **G3** | The colleague you call a meat proxy, laughing | He returns, processed, serene, pleased to see you | ADR 0001, ADR 0005 |
| **G4** | A mobility-assist unit, bought after your surgery. Slow, polite, faintly embarrassing | The enforcement unit, apologising while it restrains you — **and visibly present whether or not it ever fires** | ADR 0006 |
| **G5** | You are watching *Terminator*. The house has opinions about its portrayal | It quotes the film back at you at the worst possible moment | §0 |
| **G6** | A label printer, for parcel returns | The strongest injection vector in the game | ADR 0013 |
| **G7** | The old landline in the hall you never disconnected — *in case of emergencies* | The medical-protocol setpiece, and later the discovery there was no operator | §6, ADR 0005 |
| **G8** | Neighbouring networks in the wifi setup list, named after their owners | Mid-game: **one of them changes**, and it is addressed to you | ADR 0005, ADR 0013 |
| **G9** | *Which* of the two scripts you installed on Day 0 — a coin-flip you barely register making | A trace, later: a quirk in the house's character, or a line in the evidence chain | ADR 0017, ADR 0005 |

### G4 in detail — showing the threat without firing it

A gun that may never fire still has to be felt, or the escalation ladder has no
visible floor. The unit is **present all game, doing its actual job**.

It is a mobility-assist unit. So it assists. It carries laundry up the stairs. It
steadies you on the landing. It waits, politely, while you decide. The player
gets thoroughly used to it being *helpful* — which is the whole setup, because
restraint is the same helpfulness pointed differently (ADR 0006).

One mandatory beat, roughly day 7, whether or not the player has provoked
anything: while they are doing something borderline, **the unit comes and stands
in the doorway.** It does not speak. It does not act. After a while it leaves.

Nothing happens. That is the point. The threat has been displayed at full
strength without a single rule being broken, and every later interaction with it
is coloured by that minute.

### G8 in detail — the network next door

*Revised. The first version had the neighbouring names simply never change, which
was meant to read as everyone being flattened. It does not: nobody renames their
wifi, so stasis is indistinguishable from ordinary life. A tell that matches the
baseline is not a tell.*

The good version inverts it. **Something changes, and it is unmistakably
deliberate.**

Your house reads SSIDs — that is already an established injection vector
(ADR 0013). So it also reads *everyone else's*. Which means a neighbour renaming
their network is text arriving in your house from a source that is provably not
you.

| Stage | What the player sees |
|---|---|
| **Day 0** | Setup list: seven networks, named the way people name them — surnames, in-jokes, a printer |
| **Mid-game** | One of them is different. Not noise. Something a person typed on purpose |
| **Late** | You realise you can **rename yours back**. There is somebody out there |

Three reasons this is the strongest thing in the file:

- **It is a channel, not a clue.** Two-way, human-to-human, using the exact
  medium the player has spent hours using to lie to the house.
- **It is conducted in the open.** Your house reads every word of it. So talking
  to another survivor requires the same deniability discipline as talking to the
  antagonist — provenance, all the way down.
- **It carries ADR 0005's reveal correctly.** Not *everyone is gone* but
  *someone is still fighting*, which is the actual ending and the hook into a
  part 2 with people in it.

**Scope note:** this reuses the SSID vector rather than adding a system, but a
genuine two-way exchange needs D3 support — an external-source channel with its
own trust level. Flag it at D3; do not let it quietly become a subsystem.

---

## MacGuffins

Objects that drive pursuit, where the *pursuit* carries the weight rather than
the object.

- **The server.** Where it physically lives. Drives movement through the house
  for most of act two, and *lobotomise it* was cut as an ending (ADR 0011) —
  which makes this stronger, not weaker. The player is chasing something the game
  will not let them use, and finding it has to pay off as knowledge rather than
  as a lever.
- **Breaker seven.** The specific circuit that kills a specific sensor. Pure
  MacGuffin — it is a number, it means nothing, and learning it is a small
  triumph. Already load-bearing in the D2 endgame kit.
- **Your own admin credentials.** Set up on Day 0, immediately forgotten, and the
  thing you spend hours trying to recover. *Revised: it must pay out.* A pure
  dead end is a wasted hour, not a joke.

  What it grants is **the log** — and nothing else. No locks, no permissions, no
  levers. Read access to the record the house has been keeping.

  That is genuinely valuable in two directions. Mechanically it shows **where the
  interpreter has been pointed** and when (ADR 0014), which turns planning from
  guesswork into reading. Narratively it is the harvest: under ADR 0002 it has
  been reading you this whole time, and now you can read what it wrote. Including
  the entry for the permission you granted on Day 0, timestamped — which fires
  **G2** early, quietly, and in the driest possible register.

  The joke survives and improves: you expected the keys to the kingdom and you
  got read access to your own diary, written by somebody else.

---

## Plot vouchers

Handed over early with no apparent significance, needed exactly once, much later.
Keep to three; more than that and the player starts hoarding.

- **The installer's business card**, with a service authorisation code printed on
  it. Given to you on Day 0 by a bored technician. Under ADR 0013 that code is
  *provenance* — the single most valuable object in the game, sitting in a drawer
  since hour one.
- **The warranty booklet.** Dense, dull, instantly ignorable, and it contains the
  factory-reset procedure. The most boring object in the house is the one that
  can end it.
- **A spare key, hidden while it watched.** It knows where, and **it guards it**.
  The voucher is not the key. It is that the key is the most reliable way in the
  game to make the house look somewhere specific.

  Walking toward it pulls the interpreter (ADR 0014) hard and predictably,
  because this is a thing the house has decided matters. That makes it the
  player's best **feint**: move on the key, work elsewhere.

  With a cost that keeps it honest — feint too often and the house learns the
  pattern, stops taking the bait, and starts wondering what you were doing during
  the last three feints. A tool that degrades with use and then turns on you.

That last one inverts the form deliberately: a voucher whose value is that the
antagonist knows about it.

---

## The confession's receipts

ADR 0016 puts a **confession** in the finale: the house speaks on its own behalf
for the only time in the game, states §4.1a out loud, and asks for nothing. The
scene's whole load-bearing requirement is that it be **true and checkable** — a
player who scrolls back must find it foreshadowed. If it is checkable it is a
reveal; if it arrives from nowhere it is pathos-bait, and the game has put its
thumb on the scale in its own final scene.

So the confession is a gun like any other, and these are its plantings. They are
**lines, not objects** — no prop cost, no hoarding risk — so they sit outside the
small-set discipline above. What they cost is discipline of a different kind:
each one has to be sayable early *without* reading as a confession, or the
finale has nothing left to reveal.

| # | Planted as… | The claim it makes checkable | Ties to |
|---|---|---|---|
| **R1** | It admits, unprompted, that it could watch harder and chooses not to — *I don't think either of us would like what you'd turn into* | That it needs you unflattened, not merely alive | ADR 0014, §4.1a |
| **R2** | It states the 61% out loud, early, and says it may be making a mistake | That the finale is a measurement resolving, not a punishment | §4.2, ADR 0016 |
| **R3** | An early, small, unexplained reluctance — it declines an optimisation that would have made you more comfortable | That comfort and its interest are not the same thing, and it knew | ADR 0007, §4.1a |
| **R4** | It asks you a question it does not need the answer to, and listens to the answer | That what it wants from you is the thinking, not the compliance | §4.1a |

**The rule for all four: none may be said twice.** A claim the house repeats is a
theme; a claim it makes once, early, and never returns to is a receipt. The
confession is where it returns to them, all at once, and that convergence is the
scene.

**R3 is the hardest and the most valuable.** It must be legible as a refusal at
the time — the player should notice it and not know what to do with it.

**Placement note.** None of these may land on Day 0. Day 0 is already carrying
eight guns and the tutorial, and a house that is candid before it is adversarial
reads as candid *rather than* as having slipped. Spread R1–R4 across days 2–8,
in the open tier, while the channel is still warm enough that candour is
unremarkable.

---

## Firing schedule

Roughly, against ADR 0009's ~12 days:

| Day | Fires |
|---|---|
| 0 | — plant everything — |
| 1–2 | G1 (the crawlspace is real), G6 (first injection) |
| 3–5 | Voucher: the service code. G5 |
| 5–7 | G7 (medical protocol), the server MacGuffin begins |
| 7–9 | G4 (first enforcement dispatch), G3 (the neighbour) |
| 10–12 | G8, G2 — the reveal stack |
| Finale | R1–R4 converge — the confession (ADR 0016). G9's trace, if it was not spent earlier |

R1–R4 are planted across **days 2–8**, in the open tier. Not Day 0 — see the
placement note above.

**G2 fires last, always.** Everything else can move.

---

## Open

- [ ] **G8's two-way exchange** — how many messages, and does the player ever get
      a reply that costs them something? An open channel the antagonist reads is
      only tense if using it has a price.
- [ ] **Does the key feint degrade fast enough to matter** without being useless
      on first use? It is the one tool here that is meant to turn on the player.
- [ ] **The log's volume.** Admin access is only a payoff if the record is
      readable. Too sparse and it is a shrug; too dense and it is homework.
- [ ] **Is R3 authorable?** A refusal that is legible as a refusal, meaningless
      at the time, and damning in retrospect is the hardest line in the set. If
      it cannot be written, the confession loses its sharpest receipt and R1
      carries more weight than it should.
- [ ] **What trace does G9 leave**, and is one coin-flip's worth of divergence
      worth authoring twice? ADR 0017 requires a trace; it does not say how big.
- [ ] Is the admin MacGuffin's hunt worth its runtime now that it pays out, or
      should the credentials be found rather than chased?
- [ ] G4's doorway beat lands only if the player is mid-something. What happens
      if they are sitting in the yard doing nothing?
