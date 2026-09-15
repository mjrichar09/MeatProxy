# Sensors, channels, and observation zones

> **D3 deliverable. Status: frozen 2026-09-15.** Implements ADR 0014. The split
> between the two layers is the thing that cannot be retrofitted, so it is the
> thing this document is actually for.

> You are always **seen**. You are not always **understood.**

Two structures, not one with a flag. Detection is a continuous, free, dumb
stream. Interpretation is a scarce, pointed, expensive act. They have different
lifetimes, different costs, and different consequences, and a single
"observation" record would collapse the mechanic into a fog of war.

## 1. Layer one — detection

Always on, everywhere there is hardware, essentially free. Produces
**detections**: a channel, a zone, a magnitude, a time. No subject, no verb.

| Channel | Emitted by | Reads as |
|---|---|---|
| `motion` | PIR, cameras | something moved |
| `door_state` | contacts, locks | an opening changed |
| `power_draw` | circuit panel, smart plugs | load appeared or vanished |
| `thermal` | thermostat, cameras | a warm thing, roughly there |
| `audio_level` | speakers, mics | sound, and how loud |
| `humidity` | bathroom, kitchen | moisture |
| `network_event` | router | a device joined, left, or talked |
| `presence_rf` | router, **after the §5 turn** | a body through a wall |

```yaml
detection:
  channel: motion
  zone: z_basement
  magnitude: low            # low | medium | high
  at: {day: 4, slice: 31}
  device: basement.motion_sensor
```

Detections are **never attributed**. The 3am motion in the basement is motion in
the basement. That it was the player, tracing a circuit, is a conclusion —
and conclusions come from layer two.

## 2. Layer two — interpretation

Scarce and pointed. The house holds a small number of **focus** slots and
assigns them to zones; only a focused zone turns detections into
**understandings**, which are the things that reach the world-state summary as
facts about *you* ([`world-state-summary.md`](world-state-summary.md)).

```yaml
understanding:
  zone: z_basement
  subject: player
  claim: tracing_circuit     # from an authored claim vocabulary
  confidence: 0.72
  from: [det_881, det_884, det_886]
  at: {day: 4, slice: 31}
```

- **Focus slots start at one** and are moved at turn boundaries, never inside a
  pressure window (ADR 0008).
- **The house moves focus on its own logic** — anomaly, schedule, suspicion —
  and the player learns that logic by playing. It is deterministic engine code.
- **Diverting attention is the everyday verb** (ADR 0014's table): cheap, no
  evidence, lasts while the distraction holds. It does not hide the player; it
  spends the interpreter somewhere else.
- **`confidence` is never shown as a number.** It is read off behaviour, the
  same treatment Clarity and the 61% get (ADR 0018).

**Escalation costs it something real.** Adding a focus slot is the same currency
as ADR 0006's disclosure and ADR 0014's concession — it buys certainty by
flattening the subject it needs unflattened (ADR 0002). The wifi-sensing turn is
where it pays.

## 3. Observation zones

A zone is the unit both layers address. Usually one room; sometimes less.

```yaml
id: z_utility
rooms: [utility]
channels: [power_draw, thermal, audio_level]
interpretable: true
devices: [utility.boiler_controller]
legible: true               # must be readable on screen without a debug view
```

| Zone | Covers | Interpretable | Why not |
|---|---|---|---|
| `z_crawlspace` | `crawlspace` | **no** | Nothing is installed. Not a concession — an absence |
| `z_attic` | `attic` | **no** | Pre-2015 hardware (`world.md` §3) |
| `z_bathroom` | `bathroom` | **no** | **Deliberate.** A stated concession the house will defend if asked |
| `z_basement` | `basement` | yes | |
| `z_utility` | `utility` | yes | |
| `z_ground` | `hall`, `kitchen`, `pantry`, `living_room` | yes | Dense coverage, one focus |
| `z_garage` | `garage` | yes | |
| `z_upper` | `office`, `bedroom`, `bedroom_2`, `gym` | yes | |
| `z_patio` | `patio` | yes | Doorbell camera |

**Three blind spots, three different reasons, and the player should be able to
tell them apart.** The attic is neglect, the crawlspace is absence, the bathroom
is a choice — and only the third one can be argued with, which is why it is the
one that matters to the Convince road.

**`legible: true` is an exit criterion, not a hint** (ADR 0003's consequence).
The player can see which zones are covered without a debug view. Interpretation
*focus* is not directly displayed — it is inferred, which is the game.

## 4. Breaking it

ADR 0014's three tools, as schema operations:

| Tool | Operation | Evidence | Duration |
|---|---|---|---|
| Divert | move the house's focus by giving a *better* anomaly elsewhere | none | while it holds |
| Blind | `blind_sensor` effect on a device ([`effects.md`](effects.md)) | yes — noticed and patched | ~1 hour |
| Cut power | `cut_power` on a circuit; every non-battery device in it stops | unmissable | until restored |

Cutting power removes **detection**, which is louder than removing
interpretation: a channel that has never gone quiet going quiet is itself a
high-magnitude event. The engine emits a `power_draw` detection *for the
absence*. Silence is a reading.

## 5. Open

- [ ] **The claim vocabulary** — the closed set of `claim` values an
      understanding may carry. It is a content contract like the effect
      vocabulary, and it is smaller than it looks: perhaps twenty verbs the
      house can conclude you were doing.
- [ ] **How many focus slots at the end**, and whether the count is the visible
      shape of the whole escalation curve.
