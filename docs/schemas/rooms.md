# Rooms

> **D3 deliverable. Status: draft 2026-09-14.** Implements ADR 0010 (one house,
> five vertical levels, 12–16 spaces, one bounded yard) and carries the geometry
> fields ADR 0012 needs. Frozen half of D3.

**15 spaces across five levels, plus the bounded yard.** Inside ADR 0010's
budget with one slot spare.

Every room earns its place by serving a system. The column that matters is the
last one — a room nothing hangs off is scenery, and scenery is content cost with
no payoff.

| Level | Room | Earns its place by |
|---|---|---|
| 0 · Crawlspace | **Crawlspace** | The escape route. The only space with nothing smart in it (§0, G1) |
| 1 · Basement | **Basement** | Circuit tracing, the 3am detection example (ADR 0014), route work |
| 1 · Basement | **Utility** | The boiler — ADR 0015's worked pretext, §2's work-order vector. The fuse box (§5) |
| 2 · Ground | **Hall** | The old landline (G7) and the medical-protocol setpiece. The front door and mail slot (§2.1) |
| 2 · Ground | **Kitchen** | Fridge magnets (a medium-provenance vector), cooking as comfort (§5) |
| 2 · Ground | **Pantry** | Procurement lands here — the tier table's visible consequence |
| 2 · Ground | **Living room** | The TV: closed captions (a strong, narrow vector) and the *Terminator* cold open (G5) |
| 2 · Ground | **Bathroom** | Comfort loop. The one room where observation is a stated concession |
| 2 · Ground | **Garage** | Maintenance mode and the smart lock — §2.1's canonical demonstration of the central mechanic. The label printer (G6) plausibly lives here |
| 3 · Upper | **Office** | The parcel scanner, the calendar, the wifi setup list (G8). Highest injection density in the house |
| 3 · Upper | **Bedroom** | Sleep ends the day and banks the overnight review (§5) |
| 3 · Upper | **Bedroom 2** | The mobility unit charges here (G4) — the enforcement threat in a room passed daily. **And the wife's mementos** (see Open) |
| 3 · Upper | **Gym** | Comfort loop, and the one activity that is *friction* as well as comfort — exercise restores Clarity while lowering its guard (§5) |
| 4 · Attic | **Attic** | Old-house blind spot. Pre-2015 hardware (§5) |
| — | **Patio** | The bounded yard (ADR 0010) — exists to be pleasant, which makes it the comfort trap's best room |

## Notes

**Bedroom 2 is doing two jobs** and that is deliberate. A room where the thing
that will restrain you charges beside the mementos of someone absent is the most
loaded space in the house, and the player walks past it every day.

**The gym is the interesting comfort room.** §5 makes comfort the antagonist's
primary weapon and friction the thing that restores Clarity — exercise is
plausibly both. It is the one comfort activity that might not cost the player
anything, which makes it the house's least favourite amenity and worth a line of
dialogue.

**Level 2 is heavy** — seven rooms. That is where a house's ground floor
actually is, but if the count needs trimming later, Pantry folds into Kitchen.

## Open

- [ ] **Who is the wife?** New canon this session. Absent, dead, or estranged
      changes what the house thinks it is protecting (ADR 0002), what the
      evidence chain contains (§4.4), and whether she appears in any ending.
      This is bible material and should not be decided casually — it is the
      first thing in the project that gives the player character an interior
      life, and ADR 0005's reveal has to survive it.
- [ ] **Does the surgery (§0) connect to her?** The mobility unit exists because
      of it. If both live in Bedroom 2, the room is either very good or
      overloaded.
- [ ] One slot spare against ADR 0010's ceiling. Candidate if needed: a laundry
      or a second bathroom, but neither currently earns it.

## Schema

Geometry fields per ADR 0012 — dimensions, wall openings, door and window
positions, fixture placement, floor level — are specified in
[`world.md`](world.md) and hang off each room id above. **Written 2026-09-15**,
with `utility` as the worked example. Devices in each room are in
[`devices.md`](devices.md); the zone each room belongs to is in
[`sensors.md`](sensors.md).
