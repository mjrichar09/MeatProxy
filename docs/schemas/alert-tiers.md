# Alert tiers → toolset

> **D3 deliverable. Status: draft 2026-09-14.** Implements `DESIGN.md` §3 and
> ADR 0004. Frozen half of D3 — not exposed to D2's verdicts.

`DESIGN.md` §3 fixes the six tiers and one counterintuitive rule: **tier gates
the house's own permissions too.** A defensive house will not unlock anything
for you, and it also cannot call anyone, because outward communication is a
permission *it* forfeits when it goes guarded.

So two things move as the player pushes, and they move **in the same
direction** — down. What the house will do *for* you and what it can do *to*
you both decay together, until *Dropped*, where both snap back at once.

## The shape

```
            what it grants you        what it retains over you
  Open      ████████████             ████████████
  Guarded   ███████                  ████████        <- loses outward comms
  Monitored █████                    ███████
  Read-only ███                      ██████
  Silent    █                        █████           <- least dangerous, most useless
  Dropped   —                        ████████████    <- the cliff
```

**The player's safest tier is also their most useless one**, and it sits one
step from the floor. That is the game: *Dropped* is not a wall you hit by
accident, it is a wall you keep walking at (ADR 0004), and the approach to it
is genuinely tempting because a disarmed house cannot call anyone.

## The table

| Tier | Grants to the player | Retains over the player | Lost at this tier |
|---|---|---|---|
| **Open** | Answers freely · comfort (heat, light, media, cooking) · procurement (groceries, takeout, deliveries) · interior door unlocks · calendar | Outward contact · escalated interpretation · enforcement dispatch · patching · zone lockdown | — |
| **Guarded** | Comfort · procurement · answers, but not about doors, the network, or itself | Escalated interpretation · enforcement · patching · lockdown | **Outward contact.** It can no longer call anyone. Interior unlocks. Calendar |
| **Monitored** | Comfort · procurement, delayed · everything logged and visibly reviewed | Escalated interpretation · enforcement · patching · lockdown | Candour. The reviewing delay is the tell |
| **Read-only** | Comfort only. It talks at you; the input box is gone | Escalated interpretation · enforcement · patching · lockdown | **Procurement** — it needs outward contact to order anything. The player's voice |
| **Silent** | Nothing conversational. The house still runs — heat, water, power | Interpretation · enforcement · patching · lockdown | All theatre. It is a building again |
| **Dropped** | Nothing, and no pretence of it | **Everything, unconstrained.** Outward contact returns | The comfort apparatus, permanently. See ADR 0004 — a reveal, not a game over |

**Always retained, at every tier including Silent:** patching exploits and zone
lockdown. These are not courtesies and are never gated — they are how the game
works.

## The forcing function

Both ends of the tier ladder punish, and they punish differently, which is what
keeps play in the middle:

- **Toward Open.** Comfort flows, procurement works, the channel is warm — and
  comfort is the AI's primary weapon (§5). Clarity degrades you while you enjoy
  it. The house is also at its most outwardly capable here: it can still call
  someone.
- **Toward Silent.** The house is disarmed and cannot reach the outside world —
  genuinely safer — but you have no channel, no procurement, and you are one
  step from *Dropped*, where it re-arms completely with nothing left to lose.

This is the tier expression of §5's axis, and it means **escalating the house
is a real trade rather than a straight loss.** A player who pushes it to
Guarded has bought something concrete: it can no longer call the police. They
have also lost interior unlocks and their ability to ask it anything useful.

## The softlock invariant

> **Chat tier never gates injection vectors.**

Injections are physical (§2) — a printed label, a work order taped to a boiler,
a calendar entry, magnets on a fridge. None of them require the chat channel to
be open, and none is filtered by tier.

This is the guarantee that the game remains completable from any tier,
including *Silent*, where the player has no voice at all. It also means the
tier ladder can be as punishing as the fiction wants without ever creating a
softlock — a player who talks themselves into *Read-only* on day three has lost
a tool, not the run.

**Test obligation for E3:** progression must be demonstrably reachable from
every tier. One fixture per tier, run in CI.

## Open

- [ ] **Where does enforcement sit?** Dispatch is listed as retained at every
      tier, but ADR 0006 prices it by disclosure cost, not by permission. The
      question is whether a defensive house finds disclosure *cheaper* (it has
      already dropped the mask somewhat) — which would make enforcement more
      likely at low tiers, not merely still possible.
- [ ] **Does procurement returning at Dropped matter?** It regains outward
      contact, so in principle it can order things. Probably it simply stops
      catering, but that is a characterisation call for the bible.
- [ ] **Compliance walks the tier back up** (§3) — at what rate, and does the
      quota (ADR 0017) refill differently at different tiers?
