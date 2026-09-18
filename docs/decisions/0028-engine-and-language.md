# ADR 0028 — Engine and language: Godot 4 with C#

**Status:** Accepted 2026-09-18
**Completes:** ADR 0003, which named the medium and never named the technology
**Affects:** Lane E entirely, Lane A's harness, Lane U, B4

## Context

ADR 0003 settled the medium — top-down 2D, room to room, with contextual view
swaps. ADR 0012 settled how the art is made. Neither picked an engine, a
language or a framework, and `ROADMAP.md`'s *Current front* puts **E1** next.
Lane E cannot open without this.

Five ratified decisions do most of the narrowing, and they narrow it much
further than a blank-sheet engine question would suggest.

| Already decided | What it does to the choice |
|---|---|
| The house is built procedurally from the room schema (0012) | A visual scene editor buys almost nothing. The level is data, not a hand-laid scene |
| Surface 3 is keys and clicks, no parser (0019, `interaction-model.md`) | The presentation weight is device screens, chat and composition UI, not game feel |
| Day-structured turns; real time only inside declared windows (0008) | There is no performance floor worth choosing an engine for |
| **E1** exits on a headless test harness; **E6** on a full playthrough with zero model calls | The simulation has to run and be tested with no renderer attached |
| **B4** packages a local model; Steam page by devlog 003 | A desktop binary that can carry or spawn a native sidecar. Browser-only is out |

So this is not really an engine question. Rendering, physics and editor tooling
— the things an engine is bought for — are the smallest part of the work here.
The real question is **what shell the simulation lives in, and how cheaply the
simulation can be tested without it.**

## Options considered

**Godot 4.** Free, no royalty and no perpetual licensing obligation, which
matters to a project that already has B5 open on what a 2029 reinstall costs.
Good 2D, capable UI, three-platform export, mature Steam story. Its C# path is
less traveled than GDScript, and Godot 4's API churn means a meaningful share
of what a model writes from memory is Godot 3 shaped and wrong.

**TypeScript in Electron or Tauri.** The device screens — the router admin page,
the terminal, the phone, the smart display — are literally HTML and CSS, which
is the best UI toolkit that exists and is where most of Lane U's work lands.
`prototypes/d2/` is already built this way. Best closed verification loop of the
three, since tests and headless-browser screenshots are the same self-correcting
loop the Blender probe proved. Against it: four integrations you own and
maintain alone — Steam bindings, packaging, audio and input, and the native
sidecar for the local model. That plumbing is the classic way a solo project
loses Lane U.

**Python end to end.** One language for the engine, the build scripts and the
Blender pipeline; `llama-cpp-python` in process; the best test story of the
three. Against it: shipping. PyInstaller bundles are fragile and slow to start,
and pygame or Arcade sit well below the other two for exactly the text and
screen work this game is mostly made of. Cheapest to start, most expensive to
finish.

Unity and Unreal were ruled out rather than costed. Both are shaped for a game
this is not, and Unity's scene and prefab formats fight the authored-data
approach ADR 0012 commits to.

## Decision

**Godot 4, with the simulation as a standalone .NET class library that holds no
Godot reference.**

The split is the decision, not a note on it:

- **The core** — Lane E in full, and Lane A's rails — is an ordinary C# class
  library. World state and its predicate surface, the tick and the slice budget,
  perception, capabilities and revocations, alert tiers, the affordance table,
  the typed-effect vocabulary and its validator. It draws nothing, it references
  nothing from Godot, and it runs under `dotnet test`.
- **The shell** — Godot — owns the four views, input, audio, the device-screen
  UI, save file location, packaging and Steam.

Writing it this way rather than simply "we chose Godot" is deliberate.
`ROADMAP.md` already requires the core to be headless-testable at **E1** and to
be a complete game at **E6** with zero model calls. Those two exit criteria
describe a library. Building it as one satisfies them by construction instead of
by discipline, and it makes today's decision reversible: if the shell turns out
to be wrong, that is a shell swap, not a rewrite.

### Why Godot won

It is the option where the risk sits in code a model writes and a human reviews,
rather than in shipping infrastructure maintained alone. The four view modes,
audio, input, the three-platform build and the store packaging are solved
problems inside it and bespoke work outside it. Bundle size stays small enough
that a shipped local model is the dominant cost rather than a second runtime.
And the license carries no ongoing obligation, which is the same concern B5 is
open on.

What it costs, stated plainly: Godot's Control-node UI is capable but less
pleasant than HTML and CSS for the diegetic screens, and those screens are a
large share of Lane U.

### Why C# and not GDScript

- Static typing over a deterministic simulation with a frozen schema contract
  and an eval suite is worth more here than GDScript's smoother ergonomics
  inside the engine.
- **The core compiles and tests outside the engine.** GDScript would have to run
  under `godot --headless` to be tested at all, which couples E1's harness to the
  shell it is supposed to be independent of. This is the deciding reason.
- Lane A is HTTP, JSON, structured outputs, cancellation and a call that must
  never block the tick. That is ordinary async C#.
- It is the better-supported language for the way this project is written.
  GDScript is the better-traveled *Godot* path, and that is the real trade.

## Consequences

- **E1 does not need Godot installed.** The core library, its tests and the
  debug harness can start against the .NET SDK alone. Installing Godot is an
  E-lane task whenever the first view is drawn, not a prerequisite.
- **Godot's C# support requires a matching .NET SDK.** Check which version the
  installed Godot build targets rather than assuming one, and record it in
  `CLAUDE.md`'s environment section once verified, the way the Blender engine
  identifier is.
- **Godot 4 API churn is a live hazard for AI-assisted work.** Recalled APIs are
  frequently Godot 3. Verify against the installed build with the same closed
  loop ADR 0012 proved for Blender: write, run, read the output, correct.
- **Steam integration goes through a third-party binding** (Steamworks.NET or
  Facepunch.Steamworks). Well-trodden, but not first-party. Verify it as a B4
  task rather than assuming it.
- **The local model is a sidecar process, not an in-process library.** The shell
  launches it and speaks to it over its local HTTP interface, which keeps A5's
  fallback behind the same interface as the hosted calls and keeps the core free
  of both.
- **The core library is the contract between lanes.** Lane A talks to it, Lane C
  authors data it loads, Lane U renders what it reports. A public surface change
  is the same class of event as a schema change — route it back (standing rule 4).
- The typed-effect vocabulary (ADR 0003) becomes a C# type in the core, and the
  validator that checks a proposed effect against real state lives there too, not
  in the shell. **The model proposes, the engine disposes** is enforced in code
  that has no way to draw anything.
- `prototypes/d2/` stays HTML and stays throwaway. It is a D2 instrument, and
  this decision does not promote it.

## What would change our mind

If the diegetic device screens turn out to be the bulk of the game's surface
rather than roughly a quarter of it, the TypeScript option wins outright and the
core library ports, since it depends on nothing Godot provides. That is the
scenario this decision is shaped to survive.

Sooner and smaller: if the first real view takes disproportionate effort to get
on screen because the C# path fights the engine, the fallback is GDScript in the
shell with the C# core unchanged. The core is the valuable half; the language of
the shell is the optimization.
