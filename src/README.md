# Lane E — the core

The simulation, as a plain .NET class library that holds no Godot reference
(ADR 0028). It draws nothing, it knows nothing about a renderer, and it runs
under `dotnet test`. Godot is the shell and it does not exist yet; when it does,
it references this and not the other way round.

```
src/MeatProxy.Core/       the simulation. No engine reference, ever
src/MeatProxy.Harness/    the headless debug harness — E1's exit criterion
tests/MeatProxy.Core.Tests/
```

## Running it

```
dotnet test                                      # the suite
dotnet run --project src/MeatProxy.Harness       # walk the house yourself
dotnet run --project src/MeatProxy.Harness -- --tour   # what CI runs
```

The harness takes `look`, `go <room>`, `wait <n>`, `sleep`, `ask <predicate>`,
`devices`, `tools`, `save <path>`, `load <path>`.

```
[day 1, slice 96 left] bedroom> go hall
bedroom -> hall (2 slices).
  sensed: motion in z_upper
  sensed: motion in z_ground
[day 1, slice 94 left] hall> ask world(front_door_locked)
world(front_door_locked) = True
```

## Where each schema landed

| Schema | Type |
|---|---|
| `world.md` | `World/Room`, `Opening`, `Fixture`, `Level`, `Identifiers.cs` |
| `rooms.md`, `sensors.md` §3 | `Content/house.json`, `World/Zone` |
| `devices.md` | `Devices/Device`, and `WorldView.HouseCapabilities()` for the derived toolset |
| `sensors.md` §1–2 | `Perception/Channel`, `Detection`, `Understanding` |
| `claims.md` §1 | `Perception/Claim` — twenty, closed, with the confusable pairs |
| `claims.md` §2 | `Predicates/Predicate`, answered by `WorldView` |
| `effects.md` | `Effects/Effect` — thirteen, closed — and `EffectValidator` |
| `alert-tiers.md` | `AlertTier`. The ladder itself is E3 |
| `world-state-summary.md` §2 | `State/SalientBlock` — the cap, and the eviction |
| ADR 0008 | `Time/DayClock`, `TimePoint`, `TimeSettings` |

## The three rules this code is shaped by

**The game state is authoritative.** `Effects/Effect` has no member that
unlocks, opens or grants passage, and `EffectTests` asserts that it never gains
one. A door is shut because an `Opening` says so, and `unlock_door` is in the
house's toolset only because a reachable, powered lock declares it — cut the
garage circuit and the capability leaves with the power, without a list being
edited anywhere.

**The model proposes; the engine disposes.** `EffectValidator` checks every
proposed effect against real state before anything runs, and a partially valid
adjudication runs its valid half. A rationale that disagrees with its effects
changes nothing.

**No pressure window may depend on a model call.** There is no real-time path in
the core at all. Slices are in the tick from the start, which is ADR 0008's
instruction to this lane.

## What is authored and what is a placeholder

`Content/house.json` carries all fifteen spaces, nine zones and thirty-two
devices, and every room is reachable from where the player wakes up. Only
`utility` has authored geometry — it is `world.md` §3's worked example, carried
over field for field. Every other room's `dims`, `origin` and opening positions
are placeholders marked `"geometry": "placeholder"`, good enough to walk through
and not good enough to render. That marker is a build-data annotation, not a
`world.md` field.

Three things in here are decisions that were not made for us, taken as defaults
so the slice could run, and each is called out where it sits: the slice length
and day budget (`TimeSettings`), the salient cap (`SalientBlock.Cap`), and the
upper landing being folded into `hall` rather than spending ADR 0010's one spare
room slot on it.
