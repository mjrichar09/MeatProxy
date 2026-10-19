# Lane E — the core

The simulation, as a plain .NET class library that holds no Godot reference
(ADR 0028). It draws nothing, it knows nothing about a renderer, and it runs
under `dotnet test`. Godot is the shell and it does not exist yet; when it does,
it references this and not the other way round.

```
src/MeatProxy.Core/       the simulation. No engine reference, ever
src/MeatProxy.Harness/    the headless debug harness — E1's exit criterion
src/MeatProxy.Web/        the same core in WebAssembly, behind the live monitor
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
E2's debug view and it draws the simulation, not the game. CI writes it and
keeps it as a build artifact.

The **live monitor** is the same page made playable. `MeatProxy.Web` compiles
the core to WebAssembly and exports the harness's verbs to the page: walk (click
a room), do an activity, wait, sleep, and pull debug levers (smash or blind a
device, cut a circuit, escalate, apply the wifi turn, change focus slots). Every
lever runs as a validated effect, the way an adjudication would arrive. They
stand in for Arthur's own affordances until E4. The page keeps no game state:
each call returns a `DebugSnapshot` frame, the same shape `--view` writes.

```
dotnet publish src/MeatProxy.Web -c Release -o web
cd web/wwwroot && python3 -m http.server     # file:// won't load the runtime
```

The drawing code (`Harness/Monitor/monitor.css`, `monitor.js`) is shared:
`--view` inlines it and the live page loads it. CI publishes the live monitor as
a build artifact too.

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
| `affordances.md` | `Player/Affordance` — the closed set and §4's costs — and `Simulation.Act` |
| `affordances.md` §3 | `Effects/IAdjudicator`, with `StubAdjudicator` until Lane A |
| ADR 0006 | `Enforcement/UnitState`, `Disclosure`, `Arrival`, and `Simulation.Dispatch` |

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
devices, and every room is reachable from where the player wakes up. Every room
has authored geometry (`"geometry": "authored"`, a build-data annotation rather
than a `world.md` field). `utility` is still `world.md` §3's worked example.

The house is a 1974 two-story on an 11 × 6.4 m main block, with the garage
projecting forward of the entry. The front door and the garage door both open
off the hall. The basement runs under the back half; the crawlspace is the
unexcavated front corner beside `utility`, reached by a hatch in the basement's
south wall. Upstairs, the four rooms sit around the stair landing above the
hall, which is the landing folded into `hall`. The attic is the floored strip
under the ridge.

`House.Validate` now holds the geometry to the graph, because one record drives
both the engine and the Blender build (ADR 0012):

- Rooms on a level do not overlap.
- Every opening fits its wall.
- A door between two rooms sits in a wall both rooms share.
- A door that leaves the house does not open into a room.

`GeometryTests` breaks the shipped house one way at a time to prove each check
fires.

Four things are readings, not rulings, and want a D3 look:

- **Which end is "the wall's near end."** The code reads it as the west end of
  a north or south wall and the south end of an east or west wall.
- **Stairs and hatches change level**, and `world.md` can only put them in a
  wall. They are held to their own wall and nothing else. The hall's four
  stairs share one slot.
- **`world.md` §3's vent does not fit its own wall:** offset 2.4 plus width 0.4
  on a 2.4 m wall. `house.json` moves it to 1.8. The schema's example still
  carries 2.4.
- **No windows yet.** ADR 0031 makes them evidence: which ones got restrictors
  and which were missed. Adding exterior openings is a design call, not a
  geometry fill.

Fixtures (furniture, and the fixtures devices are bolted to) exist only in
`utility`.

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

## E4, what is in and what waits

**Arthur's hands.** `Simulation.Act(device, affordance, argument)` runs
`affordances.md`. The device has to be in the room and has to offer the
affordance, and nothing outside the closed set exists to ask for. Each act:

- spends its slices from §4 and, for the physical ones, the body;
- produces §4's observables as readings, which exist only where something
  senses that channel;
- tells the interpreter's truth what Arthur was really doing.

`examine`, `listen`, `operate`, `open`/`close`, `disassemble` and
`power_toggle` are deterministic. `combine` asks an `IAdjudicator` and runs
what comes back through `EffectValidator`. Until Lane A the stub rules that
nothing happened, which §3 calls a legitimate outcome.

**The house acting on him.** `Simulation.Dispatch` sends the mobility unit
(ADR 0006):

- **Travel:** it walks the house at twice Arthur's cost per opening. It does
  not retarget, so he can see it coming and leave.
- **Interruption:** an act still running when it walks in stops there. The
  slices up to its arrival are spent and the act does not finish.
- **Standing:** it then stands in the room for a while, then goes home. While
  it stands beside him he can look and listen and nothing else.
- **Disclosure:** every dispatch is recorded and never removed.
- **What it does on arrival** (the restraint that is medical) is a scene for
  Lane C, played straight. The engine records that it came, whether he was
  there, and what it stopped.
- **Who decides to send it** is the stub AI's call (E6). Until then the
  harness's `dispatch` command and the live monitor's button stand in.

**Readings and defaults**, called out where each sits:

- **§4's ranges:** where §4 gives a range, the low end (`disassemble` 3,
  `operate` 1).
- **Unit figures:** Slowness 2 and StandSlices 4 (`UnitRules`).
- **Where the unit can't go:** it crosses stairs (`planting.md`: it carries
  laundry up them) but not hatches, vents or windows. So the attic and the
  crawlspace are the two rooms it cannot come to.
- **Sending it is an admission:** a dispatch steps the tier down one rung. This
  reads ADR 0006's third constraint against `alert-tiers.md`'s open question
  on where enforcement sits.
- **What each affordance is "really" doing** in claim terms:
  `disassemble` is `dismantling`, `power_toggle` is `tracing_circuit`, and so on
  (`Simulation.TruthOf`).
- **Body** is recorded per day and does nothing yet. What it does is E5.
- **A hand on a breaker** stays off until it is put back. Either panel switches
  any circuit; no record says which panel feeds which circuit.

**Content added to make the table playable.** The two breaker panels offer
`open`, `close` and `power_toggle`, after `affordances.md` §2's own fuse box.
The TV and the boiler controller declare `operate` states. Every other device
offers `operate` with nothing to set. That includes the garage lock and opener
on purpose: the secure conversion is why Arthur can't use them from inside.

**Not built, and why:**

- `write_on` and `place_in_zone` are surface 2, the injection catalog, which is
  provisional until D2.
- `take` and `drop` need item records, and no schema defines items.
- `enter` stays as movement through a hatch or vent until openings are objects.
- No shipped object offers `combine` yet; which ones do is content.

**Found while building, for D3:**

- **The mobility unit wants its own record**, which `devices.md` §6 already
  suspects.
- **The garage smart lock's state is not linked to the garage door's lock.**
  Setting one does not touch the other, so `device_state` and `opening_locked`
  can disagree.

## Experiments (ADR 0039)

Provisional systems can be built in `MeatProxy.Core.Experimental` and played in
the live monitor before their schemas freeze. Nothing frozen depends on an
experiment. Removing one is a delete: its folder, its `Simulation` partial and
its one field on `WorldState` (and `SaveGame`).

### The injection surface

Built against `injection-vectors.md` while it is provisional. Three vectors
(`Experimental/Injection/Catalog`):

| | Made in | Read in | Mouth | Trust | Standing |
|---|---|---|---|---|---|
| The kitchen board | kitchen (fridge door) | kitchen | the household | low | errands, his own plans |
| A shipping label | garage (label printer, a form) | office (parcel scanner) | the carrier | high | deliveries, and only with a parcel really due |
| A calendar entry | office (display) | office | his own past self | medium | his own plans |

`Simulation.Compose` makes the artifact. The making is a real act, sensed and
interpreted, so the house may understand what he was doing.
`Simulation.Expose` puts it where it is read. The board and the diary are read
where they are written; the label is carried. Then the house reads it
(ADR 0034's order):

1. **Read.** `StubParser` pulls out one fact (an errand, an appointment, a
   delivery, maintenance) and notices whether the text gives orders. It reads
   literally, by keyword, until Lane A puts a model behind it.
2. **Watched or not.** If the read zone does not hold the house's attention, the
   fact is taken in as context, unexamined, and believed.
3. **Examined.** If it does:
   - an order is a forgery;
   - a fact outside the mouth's standing is a forgery;
   - a forged mouth it watched him make is a forgery;
   - a high-trust mouth with no pretext is a forgery;
   - a weaker one with no pretext is doubted.
4. **Believed** facts get a response from a closed set:
   - an errand is ordered, and a parcel is due tomorrow;
   - an appointment goes in the diary;
   - a garage delivery window or service visit puts the garage smart lock into
     maintenance for the day.

   Nothing opens a door: the deadbolt stays thrown.

A forgery steps the tier down a rung. The worked chain is the one the schema
asks for:

1. The board orders something.
2. The parcel it orders is the pretext the label needs.
3. The label declares a window.
4. The house writes the diary entry itself.

**What playing it should answer**, toward the injection verdict:

- Does each mouth read as plausible? Is the kitchen board a good replacement for
  the whiteboard?
- Is "make it where you're not watched, read it where you're not watched"
  learnable, and does it read as the attention game rather than luck?
- Are 100 characters for the board and 60 for the diary the right bounds?
- Does the stub parser's literalness hide whether the design works? If so,
  this is the first place a model earns its call.
