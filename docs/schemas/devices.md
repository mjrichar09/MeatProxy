# Devices

> **D3 deliverable. Status: frozen 2026-09-15**, with one field marked
> provisional. Implements `DESIGN.md` §2 and §5, ADR 0014 and ADR 0019. The
> device list is where the house's senses, its powers, and the player's tools
> are the same objects seen from three sides.

A device is the intersection of every system in the game. That is the point of
this schema: **one record, three readings.**

| Read as | Gives you |
|---|---|
| What the house can *do* with it | its toolset, filtered by tier ([`alert-tiers.md`](alert-tiers.md)) |
| What the house can *sense* with it | the detection layer ([`sensors.md`](sensors.md)) |
| What the player can *do* to it | the affordance table ([`affordances.md`](affordances.md)) |

Three systems that would otherwise drift apart cannot, because they are columns
of one table.

## 1. Record

```yaml
id: garage.smart_lock
room: garage
class: lock
retrofit: 2021
network: zigbee              # wifi | zigbee | wired | none
circuit: c_garage
powered: mains               # mains | battery | both | none
senses: [door_state]         # detection channels it contributes
house_capabilities:          # tools the house may hold for this device
  - lock_door
  - unlock_door
  - report_door_state
player_affordances:          # what it offers when selected
  - examine
  - operate
  - disassemble
states: [locked, unlocked, maintenance, faulted]
state: locked
injection_surface: null      # provisional — see §5
patchable: true
```

### Fields that carry weight

**`network: none` is the strongest property in the schema.** A device with no
network is a thing the house owns nothing of. The landline in the hall, the fuse
box, the crawlspace's nothing-at-all. These are not oversights in the house's
coverage; they are the coverage's edge, and the game is played along it.

**`retrofit` decides what the house can see**, overriding the room's date
(`world.md` §3). Pre-2015 hardware is dumb hardware.

**`powered` and `circuit` are what makes cutting power a real verb.** ADR 0014
prices it as expensive and unmissable; this is where that lands. Battery devices
survive the cut, which is why the enforcement unit has one.

**`patchable: false` is a promise to the player.** §2 says every exploit is
temporary — but a physical delta the house cannot reach is the preparation
layer's whole basis (ADR 0015). If the house cannot patch it, it has to *route
around* it, which costs it something in ADR 0006's currency.

## 2. `house_capabilities` is the toolset, at source

The tool whitelist is not authored separately. It is **derived**: for a given
tier, the toolset is every capability of every device the house can currently
reach, minus what the tier withholds. `unlock_door` is absent when the door is
bolted because the bolt is a physical state the capability's precondition tests
— not because a list was edited (standing rule 1, `DESIGN.md` §7.1).

Same derivation in the other direction: the house going Guarded forfeits
outward comms (`alert-tiers.md`) by losing the capabilities of every device with
`network: wifi` that talks off-premises. The ladder acts on devices, and the
toolset falls out.

## 3. `senses` is the detection layer, at source

A room has audio because a device in it has `audio_level` in `senses`. This
answers the open question left by [`interaction-model.md`](interaction-model.md)
§7 and answers it as a *consequence* rather than a ruling:

- **Chat is available where a reachable, powered device senses audio.**
- The crawlspace has no devices, so the player cannot speak to the house there.
  The one room that is a genuine escape route is the one room the channel does
  not exist in.
- Killing the kitchen's audio device removes the channel in the kitchen. The
  player can *delete their own ability to talk*, permanently, with a hammer.
  That should be possible and should not be warned about.

## 4. The house's devices, by room

Class, not inventory — Lane C fills in the rest against this list.

| Room | Devices | Notes |
|---|---|---|
| `crawlspace` | — | Nothing. The only such room (G1) |
| `basement` | motion sensor, circuit panel *(dumb)* | ADR 0014's 3am detection example |
| `utility` | boiler controller, fuse box *(dumb, `network: none`)* | ADR 0015's worked pretext |
| `hall` | door sensor, camera, **landline** *(`network: none`, `retrofit: 1988`)* | G7 |
| `kitchen` | fridge *(display + magnets)*, smart speaker, hob sensor | A medium-provenance injection surface |
| `pantry` | shelf camera | Procurement's visible consequence |
| `living_room` | TV, smart speaker, camera | Closed captions; the *Terminator* cold open (G5) |
| `bathroom` | humidity sensor, **no camera** | Observation as a stated concession |
| `garage` | smart lock, door opener, label printer, camera | §2.1's canonical demonstration |
| `office` | router, parcel scanner, smart display, printer | Highest injection density (G8) |
| `bedroom` | sleep sensor, smart speaker | Sleep banks the overnight review |
| `bedroom_2` | **mobility unit charger** *(battery-backed)*, camera | ADR 0006's threat, passed daily |
| `gym` | equipment sensor, speaker | Friction and comfort at once |
| `attic` | **the retired hub** — pre-2015, still powered, never wiped | The old-house blind spot, and the **previous generation of this same assistant** (ADR 0021). Carries E1 |
| `patio` | doorbell camera, weather sensor | Pleasant, and therefore the comfort trap's best room |

**The wifi-sensing turn (§5) is a device change, not a rule change.** The
router gains `senses: [presence_rf]`, which makes previously-blind zones
interpretable. One field, one plot beat, no new system — which is the test of
whether this schema is shaped right.

## 5. Provisional

- **`injection_surface`** — the reference from a device to what can be written
  on, renamed, printed, or captioned through it, plus `source` and `trust`
  (ADR 0013). **Provisional until D2's injection-surface verdict.** Devices are
  frozen; this one field is not, and E and C must not build against it.

## 6. Open

- [ ] **Does the mobility unit belong in this schema or its own?** It is a
      device by every field here and an antagonist by ADR 0006. Currently
      listed; likely needs a record of its own once enforcement is specified.
- [ ] Exact capability names, which are a naming pass rather than a decision.
