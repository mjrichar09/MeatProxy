# The interaction model — three surfaces at one interface

> **D3a deliverable. Status: draft 2026-09-15.** Implements ADR 0019, and the
> interface half of ADR 0003, 0008, 0013, 0014 and 0017. This is the contract
> between the player's hands and the engine; the model appears in it only where
> a row says so.

`DESIGN.md` describes injection (§2), chat (§3) and world interaction
separately and never says how they sit together. They are not three parallel
input methods. **Two of them are built out of the third.**

## 1. The stack

Surface 3 is the floor. Everything the player does is a body doing something in
a room; surfaces 1 and 2 are what that body produces.

```
  Surface 1  SPEAK ......... an utterance, in a room, heard where there is audio
  Surface 2  COMPOSE + EXPOSE  an artifact, made at a surface, placed in a zone
  ------------------------------------------------------------------------
  Surface 3  MOVE / LOOK / MANIPULATE / COMBINE        <- always available
```

This is why the verb table was blocked: two of the three "interaction systems"
are verbs in the third, plus one composition step each.

## 2. The surfaces

| | 1 — Chat | 2 — Injection | 3 — World |
|---|---|---|---|
| Character is | speaking | writing / placing | moving a body |
| Player input | text box, utterance-scale | free composition inside a physical bound | keys and clicks, **no text** |
| House receives it | directly | as ingested data | as sensor readings |
| Attribution | **always you** | forged source (ADR 0013) | inferred, often wrong |
| Resolver | Dialogue + Guard + Judge | Parser → typed effect → engine validates | engine, deterministic |
| Bound | quota, slices, attention, tier | marker space, magnet vocabulary, label size | slices, reachability |
| Model calls / run | ~60, hard-capped | tens, player-initiated | **0** |
| Available under a clock | **no** | **no** | **yes** |

The last row is ADR 0008 made concrete. Model-bearing surfaces are simply not
open inside a real-time pressure window, so no window can ever wait on a call.
It also gives the windows their character: when the clock is visible, the
player has a body and nothing else.

## 3. Surface 3 — the floor

Direct manipulation, no parser, no text box (ADR 0019 §2–3).

| Action class | Input | Notes |
|---|---|---|
| **Move** | keyed, top-down | ADR 0003's medium; view swaps are contextual, not chosen |
| **Look** | click / proximity | the close views do the detail work |
| **Manipulate** | click an affordance | affordances come from the object's class, not from typing |
| **Carry** | select | inventory is a physical carry, bounded by hands |
| **Combine** | select-and-use | **the one place the model rules on surface 3**, and it rules *after* the act, never inside a window |

**The affordance table replaces the verb table.** The contract Lane C authors
against is *what a given object class offers when selected*, not what phrasings
the parser accepts. Adding an affordance is a D3 schema decision; adding an
object that reuses existing affordances is content.

**Combination is the open space.** Object pairs are not enumerated. The engine
hands the Adjudicator both objects' authored properties, the model proposes
typed effects from the closed vocabulary, and the engine validates against real
state (ADR 0003 showcase 1, standing rule 2). Unauthored pairs are the point;
the ruling costs time, never the run.

## 4. Surface 2 — composition is a place you walk to

An injection is **three separate acts**, and they can be separated in time:

1. **Reach** the composition surface — whiteboard, label maker, magnet set,
   router admin page. A surface-3 action, and the surface has a location.
2. **Compose** within its physical bound. The only free text in the game other
   than speech, and it is bounded by the object rather than by a character
   count.
3. **Expose** it — hold it to a camera, leave it in an observation zone, rename
   the network. Also a surface-3 action, and this is the act that decides
   *when* the house reads it.

Composing is not exposing. A prepared artifact is durable world state (ADR 0015)
and can sit in a pocket for days. That separation is what makes preparation a
layer rather than a delay, and it is what lets exposure be timed against the
attention model.

**The house reads the artifact, not the act.** What it ingests carries a forged
source and a trust value (ADR 0013). Whether it also *saw the player make it* is
a surface-3 observation and is resolved independently — which is the entire
game of §2.

## 5. Surface 1 — speech is the only attributed channel

Bounded by what a person says aloud in one go (ADR 0017 §5). No microphone.

- **Costs, all four at once:** a quota unit (visible, exact, resets daily), one
  or more slices, attention pointed at the player for the duration (ADR 0014),
  and tier pressure if the Guard classifies it as pushing.
- **Speech has a location.** It is available where the house has audio. Rooms
  without it are private and therefore useless for talking — the player's safest
  rooms are the ones where the channel does not exist, which is the same shape
  as the tier ladder (`alert-tiers.md`).
- **The house remembers.** Conversation history, plus the observations it
  actually understood, form the world-state summary it is given each turn — the
  attention model made concrete. The truncation limit is real and exploitable
  (`DESIGN.md` §9.2).
- **Persuasion is scored on state, not prose.** The Judge's hidden criteria are
  predicates over world state — which evidence has been established, which
  contradiction has been put to the house. The model rules on whether the
  argument was made; the engine rules on whether it was available to make
  (ADR 0019 §7).

## 6. What each surface does to the meters

| | Quota | Slices | Attention | Tier | Clarity |
|---|---|---|---|---|---|
| Speak | **spends** | spends | **points it at you** | risks it | — |
| Compose | — | spends | only if observed | — | — |
| Expose | — | spends | only if observed | risks it, if flagged as an attempt | — |
| World action | — | spends | only if observed | indirectly, via what it concludes | comfort-loop actions raise it |

Only speech spends the quota. Only speech is unconditionally attributed. Every
other row is conditional on being seen, which is the lever the player actually
plays.

## 7. Open — carried into the rest of D3

- ~~The affordance vocabulary~~ — **closed 2026-09-15**,
  [`affordances.md`](affordances.md). Twelve action classes.
- ~~The world-state summary~~ — **closed 2026-09-15**,
  [`world-state-summary.md`](world-state-summary.md), including the salient cap
  that makes context eviction a player-facing exploit.
- **The evidence-predicate vocabulary** the Judge reads. Still open, and now
  shared with `sensors.md`'s claim vocabulary — possibly one list.
- ~~Which rooms have audio~~ — **closed 2026-09-15 by** [`devices.md`](devices.md)
  §3. Chat exists where a reachable, powered device senses audio, so the
  crawlspace has no channel and a player with a hammer can delete their own
  ability to speak.
- **The command palette** stays a named fallback (ADR 0019 §5), not a
  deliverable. It is presentation, so it cannot land before E6.
