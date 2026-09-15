# ADR 0019 — The three surfaces, and why only one of them is a text box

**Status:** Accepted 2026-09-15
**Affects:** ADR 0003 (amends), ADR 0011 (amends), `DESIGN.md` §3, §7.2, §10, §11, D3, Lane E, Lane C

## Context

`ROADMAP.md` D3a exists because three systems were described separately and
never at the interface: prompt injection (§2, ADR 0013), AI chat (§3, ADR 0017),
and baseline world interaction. They have different input affordances, different
costs, and different relationships to the model, and the verb table could not be
written until they were pulled apart.

Carried with it: whether bounded verbs should present as interactive fiction —
free-composed text resolved by a model, the way AI Dungeon and its descendants
do it. `planelements.md` wanted this. It is the question ADR 0003 answered once
and is here re-opened properly rather than settled as a presentation tweak.

## Decision

### 1. The three surfaces, at the interface

| | Surface 1 — Chat | Surface 2 — Injection | Surface 3 — World |
|---|---|---|---|
| What the character does | **Speaks** | **Writes or places** | **Moves a body** |
| Input | Text box, utterance-scale (ADR 0017) | Free composition, physically bounded | Keys and clicks. **No text** |
| How the house receives it | Directly, always attributed to the player | Ingested as data; source is forged (ADR 0013) | Only through sensors, filtered by attention (ADR 0014) |
| Who resolves | Dialogue call, screened by Guard, scored by Judge | Parser call; engine validates the typed effect | Engine, deterministically |
| Bounded by | The quota, plus slices, attention and tier | Marker space, magnet vocabulary, label size | Slices and the body |
| Model calls per run | ~60, hard-capped | Tens, player-initiated | **Zero** |

The axis that separates them is already in the record — *speech is evaluated,
data is ingested* (ADR 0017 §5) — and the third surface completes it: **the body
is merely perceived.** Surface 3 is the only channel the house does not read. It
sees some of it, understands less of it, and what it understands is the subject
of ADR 0014.

### 2. Text in this game is diegetic, so surface 3 does not get a text box

Every character the player types is text the *character* produced: written on a
surface, or spoken aloud in their own kitchen. Both are things the house can get
hold of. A parser for world actions would put the player's hands in a text box
to perform an act the character does with their hands, next to a house that
reads text — and the one axis the whole design rests on blurs at the point of
input.

Under this decision the keyboard means exactly one thing: **the house is
listening.** That is a permanent, free source of tension bought with an
interface choice rather than a system.

### 3. Free-text world interaction is closed, not deferred

Three independent arguments, any one of which is sufficient:

- **ADR 0008.** Surface 3 is the surface used *inside pressure windows*. A model
  in the world-action loop puts a model call inside every visible clock in the
  game. Shipping a local model makes this free in dollars and leaves the latency
  exactly where it is not allowed to be. This is the decisive argument, and it
  is structural rather than budgetary.
- **Frequency.** Model cost tracks how often the player touches a surface.
  Chat is capped by the quota; injection is rare and deliberate. World
  interaction happens hundreds to thousands of times a run — at Haiku rates,
  ~1,000 parses is **$3**, six times the $0.50 target and twice the $1.50
  ceiling, from one surface (§8.3).
- **Diegesis.** Every model call in this game is the house or one of its
  subsystems. A referee model adjudicating the player's physical actions is a
  narrator with authority over the world that the fiction has no slot for — and
  if players read it *as* the house, the house is silently ruling on their
  bodies, which is standing rule 1 lost on a technicality.

### 4. What the player actually loses, and where it was returned

The appeal of the IF frame is not typing. It is *I tried something unauthored
and the world reasoned about it* — and ADR 0003's showcase 1 already grants
exactly that. Object combination stays open-ended and stays adjudicated by the
model; it is reached by select-and-use instead of by phrasing. Player-initiated,
rare, never inside a window.

What is genuinely lost is the texture of having typed it. Acknowledged, and
accepted.

### 5. The interface, concretely

- **Movement** — direct, keyed, top-down (ADR 0003's medium stands).
- **Manipulation** — click, with the contextual views doing the close work.
- **Combination** — select-and-use, which makes the combinatorial space
  *visible* rather than guessable, and routes to the Adjudicator unchanged.
- **Fallback, if direct manipulation plays flat:** a **command palette** — typed
  input that autocompletes into the closed verb set as it is entered, resolving
  visibly to bounded phrasing. It restores most of the IF texture at zero model
  cost and zero parse failure, and it *shows* the player the channel is bounded,
  the way the whiteboard's physical limits do. This is a presentation fallback
  within this ADR, not a re-opening of it: the palette still resolves
  deterministically against a closed set, with no model in the loop.

### 6. Convince is a discovered ending

**This amends ADR 0011**, which lists Convince as one of four peers.

The signposted victory is **Escape**. The game is legibly about getting out, and
a first-time player who never considers talking the house into it has played the
game as designed. Convince is reached by players who notice — from the
setpieces, the evidence chain, and the once-spoken 61% (ADR 0018) — that the
house is arguable with.

Two constraints, because a hidden ending is one bad execution away from being
either invisible or a walkthrough item:

- **The hints live in the setpieces, not in the UI.** No prompt, no tracked
  objective, no meter. The avenues open as a consequence of work the player did
  for escape-shaped reasons. This is the first real claim on ADR 0005's hint
  budget, which remains open.
- **It is never gambled on** (ADR 0016 already says this). It accrues across the
  campaign, so a player who half-noticed does not get punished for trying.

### 7. Persuasion is gated on state, never on prose

The Judge's hidden criteria are **predicates over world state**, not a quality
score on the player's writing: *has the boiler log been read; has the
contradiction been put to the house; was the mobility unit's charge history
established.* The model rules on whether the argument was made; the engine rules
on whether it was available to make.

This is what keeps standing rule 1 intact while the ending still feels won by
argument — and it is what makes the win-condition unavailable without the rest
of the game, which is the design requirement it exists to satisfy. It also gives
ADR 0018's 61% something real to move against.

## Consequences

- **ADR 0003 is amended, not superseded.** Its medium, its contextual views, its
  three-way text split and all three showcase systems stand. The one row that
  changes is *World actions → bounded verb set, Zork-era*: the verb set survives,
  its presentation as typed input does not. §5's palette is the fallback.
- **The verb table becomes an affordance table.** Not *what can be typed* but
  *what a given object class offers when selected*, which is what a click-driven
  surface needs and what Lane C authors against. Still a D3 deliverable, still a
  content contract, and now unblocked.
- **D3 gains the world-state summary as its highest-leverage remaining schema.**
  What the house holds in context is the attention model made concrete — the
  observation log filtered by ADR 0014, the conversation history, and §9.2's
  exploitable truncation limit. Lane A and Lane E both consume it.
- **The Judge's criteria become a schema item**, expressed as evidence
  predicates. Lane C authors the evidence; D3 fixes the predicate vocabulary.
- **`DESIGN.md` §7.2 is unchanged.** No call type is added or removed; the
  Adjudicator's trigger is a selection rather than a parse.
- **Lane E:** no parser in E1–E6. The palette, if it happens, is presentation and
  therefore post-E6.
- **ADR 0011's open question — do Escape and Convince stay distinct — gets
  easier**, since they are no longer presented as peers competing for the same
  player.

## What would change our mind

If playtesting shows the click surface makes the object-combination space feel
*small* — players trying fewer combinations because a menu implies a list — the
palette moves from fallback to default. That is a presentation change this ADR
already permits.

If Convince turns out to be found by almost nobody, the answer is more hint
budget in the setpieces, not a UI affordance. A tracked objective would make it
a checklist item and destroy the thing that makes it worth hiding.
