# World geometry and identifiers

> **D3 deliverable. Status: frozen 2026-09-15.** Implements ADR 0010 and carries
> the fields ADR 0012 needs to build the house in Blender from the same records
> the engine plays. Room list and rationale live in
> [`rooms.md`](rooms.md); this is their shape.

One source of truth produces both the playable house and the rendered house
(ADR 0012). A room resized here is a room resized in the render, and nobody
redraws anything.

## 1. Identifiers

Snake case, stable for the life of the project, never reused after deletion.

| Kind | Form | Example |
|---|---|---|
| Room | `<name>` | `utility`, `bedroom_2` |
| Level | `l<n>` | `l1` |
| Opening | `<room_a>__<room_b>` | `utility__basement` |
| Fixture | `<room>.<name>` | `utility.boiler` |
| Device | `<room>.<name>` | `utility.boiler_controller` |
| Zone | `z_<name>` | `z_utility` |
| Circuit | `c_<name>` | `c_basement` |

A fixture is furniture. A device is a fixture that does something (see
[`devices.md`](devices.md)). A fixture may carry a device; the device id is
independent so the same boiler can be dumb metal and a smart controller bolted
to it, which is exactly what the utility room needs.

## 2. Levels

Five, per ADR 0010. Elevation is the floor plane in metres, used by the build
script and by nothing in the engine.

| Level | id | Elevation | Rooms |
|---|---|---|---|
| Crawlspace | `l0` | −2.6 | `crawlspace` |
| Basement | `l1` | −2.4 | `basement`, `utility` |
| Ground | `l2` | 0.0 | `hall`, `kitchen`, `pantry`, `living_room`, `bathroom`, `garage` |
| Upper | `l3` | 2.7 | `office`, `bedroom`, `bedroom_2`, `gym` |
| Attic | `l4` | 5.4 | `attic` |
| — | — | 0.0 | `patio` (exterior, bounded) |

The crawlspace sits *below* the basement floor and is reached through it, which
is why it is its own level and why it is the one space with nothing smart in it.

## 3. Room record

```yaml
id: utility
level: l1
name: "Utility"
dims: [3.2, 2.4, 2.2]        # width, depth, height, metres
origin: [4.0, 0.0]           # x, y of the room's near-left corner on its level
retrofit: 1974               # build date of the room's fabric
materials:
  floor: concrete_sealed
  walls: plaster_bare
  ceiling: joists_exposed
openings:
  - id: utility__basement
    kind: door
    wall: north             # north | south | east | west
    offset: 0.6             # metres from the wall's near end
    width: 0.8
    height: 2.0
    openable: true
    lockable: false
  - id: utility__vent_shaft
    kind: vent
    wall: east
    offset: 2.4
    width: 0.4
    height: 0.3
fixtures:
  - id: utility.boiler
    at: [0.4, 1.8]          # x, y within the room
    rot: 90                 # degrees, clockwise from north
    footprint: [0.6, 0.6]
    class: appliance_large
  - id: utility.fuse_box
    at: [2.9, 0.1]
    rot: 180
    footprint: [0.4, 0.1]
    class: wall_panel
zone: z_utility
circuits: [c_basement]
```

**`retrofit` is load-bearing, not flavour.** It is the year the room's fabric or
its hardware dates from, and it decides what the house can see there. The attic
is pre-2015 and therefore a blind spot (`DESIGN.md` §5); the crawlspace has no
date at all because it has nothing in it. Retrofit on a *device* overrides
retrofit on its room.

## 4. What is deliberately absent

- **No navmesh, no pathing data.** Movement is room-to-room with openings; the
  top-down view is a presentation of the same records (ADR 0003).
- **No sprite or asset references.** Those are Lane U outputs *derived* from
  this file, and putting them here would invert the dependency ADR 0012 exists
  to establish.
- **No lighting.** A build-script concern with state variants, not a world fact.

## 5. Contract

Changing `dims`, `openings` or `fixtures` re-renders a room and is cheap while
Lane U has not run. After Lane U's first full pass it is a scheduling decision.
Changing an **id** is never cheap — every other schema references them.
