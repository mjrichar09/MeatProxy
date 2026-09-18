# Capabilities, revocations, and patches

> **D3 deliverable. Status: PROVISIONAL 2026-09-15.** Blocked on D2's endgame
> verdict. Specified and marked; **not to be built against** until that verdict
> is written. Implements `DESIGN.md` §2.1, ADR 0003, ADR 0004, ADR 0015.

A **capability** is a rule the house is currently enforcing. A **revocation** is
the player having removed it. A **patch** is the house putting it back.

Everything here is **perishable** (ADR 0015): capability lasts about an hour of
game time and then the house patches it. The durable layer is
[`preparation.md`](preparation.md), and the endgame is an exam on that one.

## 1. Record

```yaml
id: cap_garage_lock
enforced_by: garage.smart_lock      # devices.md
rule: exterior_door_locked
revoked_by:                         # any one of these — never a single route
  - v_boiler_work_order             # injection-vectors.md
  - maintenance_mode_via_operate
  - physical: disassemble_lock_plate
patch:
  id: p_relock_garage
  latency: 55                       # slices, roughly an hour — PROVISIONAL
  requires_disclosure: false        # true means it costs ADR 0006 currency
  closes_also: [v_appliance_error_code]   # adaptive, model-picked at runtime
stacking: true                      # can be held open alongside other revocations
```

## 2. The three rules that keep this safe

**1. Never one route.** Every capability lists at least two revocations of
different kinds — at minimum one injection and one physical. This is what makes
a bad adjudication cost time rather than the run (ADR 0003), and it is what
`DESIGN.md` §2.2 has been promising.

**2. Absence, not refusal** (standing rule 1). A capability the house is
enforcing means the corresponding tool is **absent from the toolset**, derived
from device state (`devices.md` §2). The house does not decline to unlock the
door; there is no `unlock_door` while the bolt is thrown. The player cannot
argue with a capability that does not exist, which is the point.

**3. Patching costs it something when it is physical.** `requires_disclosure:
true` means the patch needs the enforcement unit — a body — and every use of a
body is an expensive disclosure (ADR 0006). This is why matter is durable and
permissions are not.

## 3. The patch clock is the endgame's timer

`latency` is the whole shape of `DESIGN.md` §2.1: revocations decay, so a chain
of them has to be *assembled* before the first one lapses. The endgame's length
is fixed by this number, not by the puzzle.

- **Patch latency is deterministic** and never waits on a model call (ADR 0008).
  The clock is engine code, visible, and fair.
- **`closes_also` is the model's part**, and it runs *after* the window, between
  turns. Adaptive patching never ticks a clock.
- **Learning the clock is currency 3** (ADR 0015, knowledge). The first ten burns
  are instrumentation, and a player who has never burned a vector arrives at the
  endgame with no idea how long they have.

## 4. The capability list

Thirteen, matched to the three routes of §2.2. Provisional pending D2.

| Capability | Rule it enforces | Route it blocks |
|---|---|---|
| `cap_garage_lock` | exterior door locked | 1 — the door |
| `cap_front_door_bolt` | hall door bolted | 1 |
| `cap_window_latches` | ground-floor latches engaged | 1 |
| `cap_camera_coverage` | zones interpretable | 2 — the senses |
| `cap_motion_reporting` | detections reach the summary | 2 |
| `cap_rf_presence` | the §5 wifi-sensing turn | 2 |
| `cap_outward_comms` | the house may call out | 3 — the outside |
| `cap_player_contact` | **Arthur** may call out, through the house's network | 3 — and the first thing every player tries (ADR 0025) |
| `cap_delivery_hold` | parcels held, not admitted | 3 |
| `cap_mail_slot` | slot secured | 3 |
| `cap_enforcement_standby` | the unit charges and waits | all — ADR 0006 |
| `cap_quota_enforcement` | the daily chat cap | the chat channel (ADR 0017) |
| `cap_crawlspace_hatch` | the hatch fastened | **the escape route (G1)** |

**`cap_quota_enforcement` is in this list and cannot be revoked.** It is here to
be *tried*. The hack that failed on Day 0 failed at exactly this, and a player
who spends a week trying to lift the cap is replaying the mistake that started
the game (ADR 0017 §3). It has no `revoked_by` entries, and that absence is
authored rather than accidental.

**`cap_player_contact` is the honest one.** It is held shut first by a standing
instruction Arthur gave on Day 0 (*hold my calls*) — cancelable at once, and
canceling it reveals the capability underneath, which is tier-gated and
degrades as he pushes. Revoking it buys a real window and the window closes. The
landline is not in this table at all, because it is not a Hold device (ADR 0025).

**`cap_crawlspace_hatch` has only physical revocations.** The one room with no
devices in it cannot be opened by talking, by writing, or by forging anything —
it is opened with hands, over nights, and the house watches the whole time and
decides it is not worth a body to undo (ADR 0015).

## 5. Open

- [ ] **`latency: 55` slices** — the load-bearing number, and D2's endgame
      prototype exists to test whether the resulting window is exhilarating or
      fiddly.
- [ ] **How many revocations can stack** before the house treats the pattern
      itself as the event (`endgame.md`'s assembly threshold).
- [ ] Whether `cap_rf_presence` is revocable at all, or whether the §5 turn is
      one-way. Leaning one-way: it is a plot beat, not a difficulty knob.
