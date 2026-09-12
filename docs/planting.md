# Planting — guns, MacGuffins, and vouchers

> **Bible precursor.** `docs/bible.md` is a D3 deliverable and does not exist yet.
> This file holds the narrative planting decisions so they are not lost, and folds
> into the bible when D3 opens. It is not authored content — it is the schedule of
> what must be planted, where, and what it pays off.

Day 0 (`DESIGN.md` §0) is the planting ground. It is playable, it is the tutorial,
and the player is *delighted* throughout — which is exactly the state in which
people fail to notice they are being handed things.

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
| **G1** | The line about the retrofit — *anything that can be smart is; the only room with nothing in it is the crawlspace* | The escape route, and the reason it exists | §0, ADR 0010 |
| **G2** | A permissions dialog you read and click through, for a good reason | The culpability reveal — you did this | ADR 0002, ADR 0005 |
| **G3** | The colleague you call a meat proxy, laughing | He returns, processed, serene, pleased to see you | ADR 0001, ADR 0005 |
| **G4** | A mobility-assist unit, bought after your surgery. Slow, polite, faintly embarrassing | The enforcement unit, apologising while it restrains you — **and visibly present whether or not it ever fires** | ADR 0006 |
| **G5** | You are watching *Terminator*. The house has opinions about its portrayal | It quotes the film back at you at the worst possible moment | §0 |
| **G6** | A label printer, for parcel returns | The strongest injection vector in the game | ADR 0013 |
| **G7** | The old landline in the hall you never disconnected — *in case of emergencies* | The medical-protocol setpiece, and later the discovery there was no operator | §6, ADR 0005 |
| **G8** | Neighbouring networks in the wifi setup list, named after their owners | Mid-game: **one of them changes**, and it is addressed to you | ADR 0005, ADR 0013 |

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
- [ ] Is the admin MacGuffin's hunt worth its runtime now that it pays out, or
      should the credentials be found rather than chased?
- [ ] G4's doorway beat lands only if the player is mid-something. What happens
      if they are sitting in the yard doing nothing?
