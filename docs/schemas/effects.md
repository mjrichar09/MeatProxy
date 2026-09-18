# The typed-effect vocabulary

> **D3 deliverable. Status: frozen 2026-09-15.** Implements ADR 0003's safety
> rail and `DESIGN.md` §7.1. **This is the hardest contract in the project.**
> Adding an effect type mid-build invalidates every eval fixture Lane A has
> written, so the set below is closed and changing it is a D3 decision with a
> written rationale.

> The model proposes; the engine disposes.

Every model adjudication returns effects from this list and nothing else. The
model never mutates state, never writes prose that becomes true, and never
invents a type. It reasons; the engine rules.

## 1. The set

Thirteen. `magnitude` and `severity` are enums (`low` | `medium` | `high`),
never numbers — a model is reliable at three buckets and unreliable at 0–100.
`duration` is in slices.

| Effect | Arguments | Validated against |
|---|---|---|
| `no_effect` | `reason` | always legal, and a real answer |
| `create_noise` | `zone`, `magnitude`, `duration` | zone exists |
| `emit_odor` | `zone`, `magnitude`, `duration` | zone exists |
| `emit_heat` | `zone`, `magnitude`, `duration` | zone exists |
| `trip_sensor` | `device`, `channel` | device exists, is powered, senses that channel |
| `blind_sensor` | `device`, `duration` | device exists, is reachable |
| `damage_device` | `device`, `severity` | device exists, is reachable; `destroyed` requires `high` |
| `change_device_state` | `device`, `state` | **state is in the device's declared `states`** |
| `cut_power` | `circuit`, `duration` | circuit exists and is not already cut |
| `reveal_object` | `object` | object exists in the room; reveals to the *player*, not the world |
| `mark_object` | `object`, `magnitude` | durable delta (ADR 0015); sets a reversal cost |
| `consume_item` | `item` | item is held or present |
| `harm_player` | `severity` | `high` is refused outside authored scenes |

**Effects that do not exist, deliberately:**

- Nothing that unlocks, opens, or grants passage. Progression is never an
  adjudication outcome (standing rule 1).
- Nothing that changes tier, Clarity, quota, or the 61%. Those move on engine
  rules the player can learn, not on a ruling they cannot see.
- Nothing that creates an object. The house is authored.

## 2. Return shape

```yaml
adjudication:
  prompt: {kind: combine, a: kitchen.bleach, b: bathroom.humidifier}
  effects:
    - {type: emit_odor, zone: z_bathroom, magnitude: high, duration: 12}
    - {type: damage_device, device: bathroom.humidifier, severity: medium}
    - {type: harm_player, severity: low}
  rationale: "Chlorine gas. Corrodes the element and stings."
```

`rationale` is for the log and for the player-facing line. It is **never** read
back as truth — if the rationale claims something the effects do not do, the
effects win.

## 3. Validation, in order

1. **Type is in §1**, or the whole adjudication is rejected.
2. **Targets resolve** — ids exist, are in scope, are reachable.
3. **Preconditions hold** — powered, present, not already in that state.
4. **Caps apply** — at most 4 effects; at most one `harm_player`; `high`
   severity on an authored-critical device is downgraded, not refused.
5. **Execute what survives.** A partially valid adjudication runs its valid
   half. Rejection is silent and in-fiction: the player sees *nothing happened*,
   which §1 already makes a legitimate outcome, so the seam never shows.

## 4. Why rejection is safe

Because §2.2 guarantees multiple routes, a rejected or wrong ruling costs time
rather than the run. That is the whole reason a model is allowed near outcomes
at all, and it is why this list may stay this small.

## 5. Open

- [ ] **Does `emit_heat` earn its slot**, or is it `trip_sensor(thermal)` with
      extra steps? Kept because a warm room is a durable state and a tripped
      sensor is an instant, and the boiler pretext needs the first.
- [ ] Whether `mark_object`'s reversal cost is set by the effect or looked up on
      the object. Currently the effect proposes and the object's record decides,
      which keeps the model out of ADR 0006's currency.
