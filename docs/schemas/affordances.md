# The affordance table

> **D3 deliverable. Status: frozen 2026-09-15.** Implements ADR 0019, which
> replaced the verb table with this. A content contract: adding an action class
> is a D3 decision, adding an object that reuses existing classes is content.

The old question was *what phrasings does the parser accept*. The question now
is **what does this object offer when the player selects it** — which is what a
click-driven surface needs, and what Lane C authors against.

## 1. The action classes

Twelve. The set is closed.

| Class | Reads as | Notes |
|---|---|---|
| `examine` | look closely | Swaps to a close view (ADR 0003). Always available |
| `take` | pick up | Bounded by hands, not by a weight stat |
| `drop` | put down | Position matters — a dropped thing is in a zone |
| `open` / `close` | doors, lids, panels | Emits `door_state` where a sensor exists |
| `operate` | use it as intended | Parameterised by the object's declared `states` |
| `write_on` | compose on a surface | **The only affordance that opens a text field.** Surface 2 |
| `place_in_zone` | expose an artifact | The other half of surface 2. Separate act, separate turn |
| `combine` | select-and-use with a second object | **The open space.** Routes to the Adjudicator |
| `disassemble` | take apart, get at the inside | Durable delta; often `patchable: false` |
| `power_toggle` | switch, breaker, unplug | Circuit-level version lives in `effects.md` |
| `enter` | crawl in, climb through | Openings with `kind: vent` and the crawlspace |
| `listen` | hold still and hear | Cheap, costs a slice, reveals device state |

Everything else is one of these with a different object.

## 2. Declaration

Objects declare a subset. No object gets an affordance that is not in §1, and
the engine rejects a record that tries.

```yaml
id: office.whiteboard
class: writing_surface
affordances: [examine, take, write_on, place_in_zone]
write_on:
  capacity: 50              # characters — provisional, D2 tests this
  erasable: true
place_in_zone:
  requires_line_of_sight: true
```

```yaml
id: utility.fuse_box
class: wall_panel
affordances: [examine, open, close, operate, disassemble, power_toggle]
operate:
  states: [normal, isolated]
disassemble:
  durable: true             # survives patching (ADR 0015)
  reversal_cost: high       # denominated in ADR 0006's currency
```

## 3. Combination is the one that is not a table

`combine` takes two object ids and does **not** consult an authored pair list.
The engine hands the Adjudicator both objects' authored properties, the model
proposes typed effects from [`effects.md`](effects.md)'s closed vocabulary, and
the engine validates against real state before executing (ADR 0003 showcase 1,
standing rule 2).

Three constraints that keep this safe:

- **It never runs inside a pressure window.** Combination resolves in turn time.
- **It is never a progression gate.** Every objective has multiple routes (§2.2),
  so a bad ruling costs time, not the run.
- **It cannot invent.** An effect outside the vocabulary, or targeting something
  that is not there, is rejected and reads to the player as *nothing happened* —
  which is also a legitimate outcome, so the failure mode is in-fiction.

## 4. Costs

Every affordance spends **slices** (ADR 0008). Nothing here spends quota; only
speech does ([`interaction-model.md`](interaction-model.md) §6).

| | Slices | Observable |
|---|---|---|
| `examine`, `listen` | 1 | rarely |
| `take`, `drop`, `open`, `close`, `power_toggle` | 1 | `motion`, `door_state`, `power_draw` |
| `operate` | 1–2 | depends on the device's `senses` |
| `write_on` | 1–3, by capacity | only if the zone is focused |
| `place_in_zone` | 1 | **this is the act that is meant to be seen** |
| `combine` | 2 | often, and loudly |
| `disassemble` | 3+ | high-magnitude |
| `enter` | 2 | `motion`, once, at the opening |

## 5. Open

- [ ] **`write_on: capacity`** is provisional — the 50-character bound is D2's
      to test, and it is the most exposed number in D3.
- [ ] Whether `listen` earns its slot or is `examine` with an audio channel.
      Kept for now because it is the player's only *sensor*, and a game about
      being observed should let the player observe back.
