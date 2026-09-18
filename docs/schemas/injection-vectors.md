# Injection vectors — the exploit catalog

> **D3 deliverable. Status: PROVISIONAL 2026-09-15, revised 2026-09-18.**
> Blocked on D2's
> injection-surface verdict. **Specified, marked, and not to be built against**
> by Lane E or Lane C until that verdict is written. This is D3's most exposed
> document (`ROADMAP.md`, D2 deferral table).
>
> Implements `DESIGN.md` §2, ADR 0013 (provenance), ADR 0015 (pretext),
> **ADR 0034 (standing, the second order, and scrutiny at ingest)**.
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
capacity: 40                     # characters, PER SURFACE — PROVISIONAL (ADR 0034)
trigger: on_focus                # on_focus | on_scan | on_wake | continuous
read_zone: z_utility             # whose focus decides scrutiny (ADR 0034 §3)
reader: parser                   # always the Parser call type
source: work_order               # the provenance it forges (ADR 0013)
asserts: equipment_state         # what this mouth has standing over (ADR 0034 §1)
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

**`asserts` is what the mouth is about** (ADR 0034). `trust` says how much the
house believes a source; `asserts` says what that source has standing to talk
about at all. A carrier asserts logistics. A pharmacy asserts medication. An
assertion outside the domain is not a weak injection — it is a flag, and worse
than saying nothing, because the house knows what a shipping label is for.

**Prefer a fact the house will act on over an instruction it must obey.** The
label does not say *unlock the garage*; it declares a delivery window requiring
garage access, and the house writes the calendar entry itself. The payload stays
inside the carrier's subject, and no instruction ever enters the evidence chain.

**`trigger` decides when, and therefore what it costs.** `on_focus` needs the
interpreter pointed at the zone — which the player may have spent the whole day
pointing elsewhere.

**`read_zone` decides whether it is examined at all** (ADR 0034 §3). If that
zone holds a focus slot at ingest, the payload is *interpreted* — standing and
pretext are checked and provenance can fail. If it does not, the payload is
*ingested* and believed as context. Timing exposure against attention is the
skill, and this is the field that makes that sentence mean something
(`sensors.md` §2).

**`capacity` is per surface, and D2 exists to test it.** A label's recipient
field, a fridge of magnets and a marker on a whiteboard do not compose alike.
**100 characters is the working figure for free composition** as of 2026-09-18,
up from 50; slot- and tile-composed surfaces carry their own smaller bounds. It
remains the single most likely thing in D3 to move.

> **Not the same 50 as chat.** The utterance bound in `DESIGN.md` §3 (ADR 0017)
> governs *speech* and is unchanged. *Capacity* is injection; *utterance bound*
> is speech. They were always two numbers.

## 2. The catalog — 15 vectors

Roughly three routes per objective across §2.2, with slack. Class and room
fixed here; wording is Lane C's, after D2.

| # | Vector | Surface | Source forged | Asserts | Trust | Pretext needed |
|---|---|---|---|---|---|---|
| 1 | Boiler work order | `utility.boiler` | technician | equipment state | medium | boiler faulted |
| 2 | Shipping label | parcel, via `garage.label_printer` | carrier | logistics | **high** | an expected delivery (G6) |
| 3 | Fridge magnets | `kitchen.fridge` | household note | domestic routine | low | none — always available |
| 4 | Closed captions | `living_room.tv` | broadcast | what is on screen — nothing about this house | medium | the TV on, the house watching with you |
| 5 | Network SSID rename | `office.router` | a neighbor | network config | medium | router admin reached (G8) |
| 6 | Parcel scanner feed | `office.parcel_scanner` | carrier system | logistics | **high** | a real parcel in hand |
| 7 | Calendar entry | `office.smart_display` | the player's own past self | own commitments | medium | display unlocked |
| 8 | Whiteboard to camera | any camera zone | none — it is *you*, in writing | nothing you could not say aloud | low | line of sight |
| 9 | Printed medication label | `garage.label_printer` | pharmacy | medication | **high** | a medical pretext standing |
| 10 | Utility bill / mail slot | `hall` mail slot | utility company | utility account | medium | mail day |
| 11 | Appliance error code | `kitchen.hob` | the appliance itself | its own equipment state | **high** | the appliance damaged (`effects.md`) |
| 12 | Gym equipment log | `gym` | health service | health activity | low | a use history |
| 13 | Doorbell camera caption | `patio.doorbell` | visitor | presence at the door | medium | someone actually at the door |
| 14 | Thermostat schedule | `utility`/`hall` control | the player's own past self | comfort schedule | medium | none |
| 15 | Old hub's stored note | `attic` | the house's **own younger self** | **anything** | **high** | E1 found (`bible.md` §3) |

**`read_zone` follows the surface's room** unless a vector says otherwise, so it
is not a column here. The exception that matters is vector 8: the whiteboard is
read by whichever camera zone it is held up in, which is the player's choice and
therefore the player's problem.

**Vectors 2 and 7 are the worked pair** and should be authored first. The
calendar entry is the thing a player wants; the shipping label is the mouth that
cannot say it directly and can produce it anyway, by declaring a delivery window
the house turns into a calendar entry on its own (ADR 0034 §2). Authoring them
together is what proves the second order reads as clever rather than as a
contrivance.

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

- [ ] **`capacity`** — now 100 characters for free composition (2026-09-18), and
      per surface. Still a working figure: D2's prototype is interactive and
      already built (`prototypes/d2/index.html`), and one sitting retires this.
      **The prototype capped its whiteboard at 80** and has been moved to 100, or
      it would test a number nothing else uses.
- [x] **Whether composition is tiles, marker space, or magnets per surface.**
      Settled by ADR 0034 — it varies, and capacity varies with it. This was the
      expensive assumption and it turned out to be the right one. Still unproven
      until D2 reports on whether it *feels* right.
- [ ] **The trust ladder's exact arithmetic** — how much a thin pretext subtracts
      from a high-trust source. Now also needs the ordering: standing is checked
      before provenance (ADR 0034), so an out-of-domain assertion never reaches
      the trust calculation at all.
- [ ] **The `asserts` vocabulary is not closed yet.** Fifteen domains are named
      in §2 and no two vectors should share one without a reason. Closing it is a
      D3 call, routed through `docs/decisions/` like any schema change.
- [ ] **Does the second order apply above a trust threshold, or everywhere?**
      ADR 0034 makes fact-over-instruction the default; it does not say whether
      a low-trust vector may still carry a bare instruction, or whether that is
      simply the whiteboard's job.
- [ ] Vector 15's one-shot status: is it consumed, or does the attic stay open?
