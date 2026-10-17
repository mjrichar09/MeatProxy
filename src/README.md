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
dotnet run --project src/MeatProxy.Harness -- --view house-monitor.html   # the debug view
```

The harness takes `look`, `go <room>`, `wait <n>`, `sleep`, `ask <predicate>`,
`devices`, `tools`, `tier`, `escalate [reason]`, `view <path>`, `save <path>`,
`load <path>`.

`--view` plays a short scripted run and writes one HTML page with a frame per
step: floor plans shaded by seen and understood, the zone table, the toolset
split into held, withheld by tier and out of reach, and the ladder log. It is
E2's debug view and it draws the simulation, not the game — placeholder rooms
are drawn dashed, and they overlap where the placeholder numbers do (basement
and utility). CI writes it and keeps it as a build artifact.

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
| `sensors.md` §1 | `Perception/Channel`, `Detection` — layer one |
| `sensors.md` §2 | `Perception/Attention`, `Interpreter`, `Understanding` — layer two |
| `sensors.md` §3 | `World/Zone`, and `WorldView.IsCovered` vs `IsUnderstood` |
| `devices.md` §5 | `Perception/SensingUpgrade` — the wifi turn |
| `claims.md` §1 | `Perception/Claim` — twenty, closed, with the confusable pairs |
| `claims.md` §2 | `Predicates/Predicate`, answered by `WorldView` |
| `effects.md` | `Effects/Effect` — thirteen, closed — and `EffectValidator` |
| `alert-tiers.md` | `AlertTier`, `Adversary/AlertLadder` (the ladder), `Adversary/TierPolicy` (the table) |
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
instruction to this lane. Focus moves at one call site, `Simulation.TurnBoundary`,
so the rule that it never moves inside a window stays true by construction.

## Seen is not understood

The two perception layers are different data structures, not one with a flag
(ADR 0014). `IsCovered(zone)` is whether anything is sensing there — free, total,
always on. `IsUnderstood(zone)` is whether the interpreter is pointed there and
can make something of it — scarce, and one zone at a time to begin with.

The interpreter works by elimination against `ActivitySignatures`, and that table
is where the confusable pairs stop being a design note. Fixing a thing and taking
it apart leave the same evidence, so the house picks whichever it finds more
ordinary — which means the default misread runs toward innocence. What flips it
is the tier: suspicion bends the reading, so pushing the house up the ladder is
what costs you the benefit of the doubt.

It also prices the three tools apart, without any of them being special-cased:

| Tool | What it does to the reading |
|---|---|
| Divert | A bigger anomaly elsewhere takes the slot. Nothing about you changed |
| Blind a sensor | A channel goes dark, so more activities fit what is left |
| Cut power | The zone stops being covered — and the silence is itself a reading |

Try it: `do at_a_surface`, then break both speakers and do it again. With audio
the house knows you were writing. Without it, it thinks you were tidying up.

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

## E3, the half that is not gated

`alert-tiers.md` is frozen; `capabilities.md` is provisional and says not to
build against it until D2's endgame verdict. So the ladder is in and the patch
clock is not. E3's exit — *an exploit can be opened, used, and patched shut on a
timer* — waits on that verdict.

What is in: `WorldView.HouseCapabilities()` is now the toolset after the tier
has its say, with `ReachableCapabilities()` and `WithheldByTier()` beside it. A
withheld tool is absent, not refused. Tier steps down through `Simulation.Escalate`
and walks back up overnight after quiet days; nothing walks back from Dropped.
`TierTests` carries `alert-tiers.md`'s test obligation, one fixture per tier,
against the one progression that exists before the vector catalog: the walk to
the crawlspace.

Three more defaults, each called out where it sits:

- **Which class each tool is in** (`Adversary/Tools`). No schema sorts tool
  names into the table's rows, so the engine does, and `House.Validate` refuses
  a device that declares a tool with no class. This wants ratifying as a D3 item.
- **The walk-back rate** — two quiet days a rung (`AlertLadder.CleanDaysPerStep`),
  a tuning target per ADR 0004.
- **Edge readings of the table**: Utility (heat, the hob) survives every tier
  including Dropped; Speech survives everything but Silent; Procurement does not
  return at Dropped. ADR 0036's always-present speech would change the second,
  and it is a pending D3 change to `alert-tiers.md`, not applied here.

No authored device carries an outward-contact tool, so the class Guarded loses
first is empty in the shipped house. That device is Lane C's.
