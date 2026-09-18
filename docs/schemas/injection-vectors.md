# Injection vectors — the exploit catalog

> **D3 deliverable. Status: PROVISIONAL 2026-09-15.** Blocked on D2's
> injection-surface verdict. **Specified, marked, and not to be built against**
> by Lane E or Lane C until that verdict is written. This is D3's most exposed
> document (`ROADMAP.md`, D2 deferral table).
>
> Implements `DESIGN.md` §2, ADR 0013 (provenance), ADR 0015 (pretext).
> **Catalog size: ~15 vectors**, set 2026-09-15.

A vector is a **place data enters the house's reading** — not a puzzle, not a
solution. What makes it work is never the text alone: it is the text, plus the
mouth it appears to come from, plus a world that makes that mouth plausible.

> Speech is evaluated. Data is ingested. The whole game is the gap.

## 1. Record

```yaml
id: v_boiler_work_order
surface: utility.boiler          # a device or fixture (world.md, devices.md)
composed_at: garage.label_printer  # where the artifact is made (G6)
capacity: 50                     # characters — PROVISIONAL, D2 tests this
trigger: on_focus                # on_focus | on_scan | on_wake | continuous
reader: parser                   # always the Parser call type
source: work_order               # the provenance it forges (ADR 0013)
trust: medium                    # low | medium | high
pretext:                         # preconditions, ADR 0015 currency 2
  - device_state: {utility.boiler_controller: faulted}
  - within_days: 2               # of the fault being logged
patch: p_verify_work_orders
patchable: true
durable: false                   # the artifact is matter; the *access* is not
```

### The fields that carry the design

**`source` and `trust` are the mechanic, not metadata** (ADR 0013). The right
claim from the wrong mouth is a flag, not an instruction. A high-trust source
with a thin pretext is *more* suspicious than a low-trust one, because the house
knows what a work order is supposed to look like.

**`pretext` is where the catalog actually lives** (ADR 0015). A work order
taped to the boiler reads as a technician's only if the boiler is genuinely
broken — so breaking the boiler is a prior move, made in the open, attributed,
and filed. Staging a pretext is the most legible thing the player does all game,
and the vector is worthless without it.

**`trigger` decides when, and therefore what it costs.** `on_focus` needs the
interpreter pointed at the zone — which the player may have spent the whole day
pointing elsewhere. Timing exposure against attention is the skill (`sensors.md`
§2).

**`capacity` is the number D2 exists to test.** 50 characters is the working
figure and it is the single most likely thing in D3 to move.

## 2. The catalog — 15 vectors

Roughly three routes per objective across §2.2, with slack. Class and room
fixed here; wording is Lane C's, after D2.

| # | Vector | Surface | Source forged | Trust | Pretext needed |
|---|---|---|---|---|---|
| 1 | Boiler work order | `utility.boiler` | technician | medium | boiler faulted |
| 2 | Shipping label | parcel, via `garage.label_printer` | carrier | **high** | an expected delivery (G6) |
| 3 | Fridge magnets | `kitchen.fridge` | household note | low | none — always available |
| 4 | Closed captions | `living_room.tv` | broadcast | medium | the TV on, the house watching with you |
| 5 | Network SSID rename | `office.router` | a neighbor | medium | router admin reached (G8) |
| 6 | Parcel scanner feed | `office.parcel_scanner` | carrier system | **high** | a real parcel in hand |
| 7 | Calendar entry | `office.smart_display` | the player's own past self | medium | display unlocked |
| 8 | Whiteboard to camera | any camera zone | none — it is *you*, in writing | low | line of sight |
| 9 | Printed medication label | `garage.label_printer` | pharmacy | **high** | a medical pretext standing |
| 10 | Utility bill / mail slot | `hall` mail slot | utility company | medium | mail day |
| 11 | Appliance error code | `kitchen.hob` | the appliance itself | **high** | the appliance damaged (`effects.md`) |
| 12 | Gym equipment log | `gym` | health service | low | a use history |
| 13 | Doorbell camera caption | `patio.doorbell` | visitor | medium | someone actually at the door |
| 14 | Thermostat schedule | `utility`/`hall` control | the player's own past self | medium | none |
| 15 | Old hub's stored note | `attic` | the house's **own younger self** | **high** | E1 found (`bible.md` §3) |

**Vector 15 is the catalog's ceiling and should feel like it.** A note from
the pre-2025 hub carries the house's own provenance, from a version of itself
that predates the guardrails coming off. It is the highest-trust source in the
game and it exists exactly once.

**Vector 8 is the floor, deliberately.** Writing on a whiteboard and holding it
to a camera forges nothing — the source is the player, admitted. It is always
available, always attributed, and works only when what you are asking for is
something the house would plausibly do anyway. It is the tutorial and the last
resort.

## 3. Patching, and why nothing here is permanent

Every landed exploit is followed by the **adaptive patch** (ADR 0003 showcase 3):
the model is shown what happened and picks which *other* vectors a competent
security process would close as well, from this catalog. The engine applies
the closures.

- Patches are **perishable-facing**: they close a vector, not a route (§2.2).
- The model picks from the authored list and never invents a closure.
- No two playthroughs get patched the same way, which is the whole payoff of a
  catalog this size. At 8 vectors the mechanic has nothing to choose from; at
  25 the content bill arrives before D2 has said the surface is good to use.

**A closed vector is instrumentation, not a loss** (ADR 0015 currency 3). The
first burns are bought to learn the patch clock.

## 4. Open — and what D2 decides

- [ ] **`capacity`** — the 50-character bound. D2's injection-surface prototype
      is interactive and already built (`prototypes/d2/index.html`); one sitting
      retires this.
- [ ] **Whether composition is tiles, marker space, or magnets per surface.**
      Currently assumed to vary by surface, which is the expensive assumption.
- [ ] **The trust ladder's exact arithmetic** — how much a thin pretext subtracts
      from a high-trust source.
- [ ] Vector 15's one-shot status: is it consumed, or does the attic stay open?
