# Roadmap — Lanes and Stages

> Companion to `DESIGN.md`. That document is *what the game is*. This one is
> *what order we find out whether it works*.
>
> Decisions live in `docs/decisions/`. Where a stage depends on one, it says so.
>
> Status: nothing built. **D1 closed 2026-09-11** — ADRs 0001–0011 all ratified.
> Next is D2.

---

## How to use this document

**Work is grouped by type, then staged within it.** Each session stays in one
lane. Switching lanes mid-session is where inconsistency creeps in — a designer
brain making engineering calls, or an engineering brain quietly authoring content
that later contradicts the bible.

Lanes are **not** parallel staffing. This is one person time-slicing, so a lane
is a mode of thinking you enter and leave deliberately, not a team.

| Lane | Mode of thinking | Output |
|---|---|---|
| **D** Design | Decide, argue, discard | Decisions, prototypes, specs |
| **E** Engine | Deterministic systems | Code with tests, no LLM anywhere |
| **A** AI layer | Prompt + eval discipline | Call types, harness, eval suite |
| **C** Content | Authoring inside a locked schema | Data files, script, level layout |
| **U** Presentation | Feel, tone, readability | UI, audio, art direction |
| **T** Tuning | Observation over opinion | Telemetry, balance passes |
| **B** Business/Ops | Unit economics, packaging | Pricing, budgets, build/ship |

**A stage is roughly one to three sessions.** It has an exit criterion that is
provably met or not met. Stages within a lane run in order. Stages across lanes
run when their gates open — see [Sync gates](#sync-gates).

**Gates are the load-bearing part of this document.** The lane view makes it easy
to see what a given mode of work looks like end to end; it also makes it easy to
forget that content authored before schemas exist gets thrown away. If you read
one section twice, read that one.

---

# Lane D — Design

*Decide, then prove, then specify. Nothing downstream survives skipping a step.*

### D1 — Structural decisions

The calls with the largest blast radius, each with a rationale and a note on what
would change our mind.

**Closed 2026-09-11.** ADRs 0001–0011 are all drafted and ratified; no Proposed
status remains in `docs/decisions/`.

One ratification carries a condition: **ADR 0007** settles what Clarity *is* so
downstream specs can assume it, but the `D2 → E5` gate still stands. The trap is
prototyped before it is built.

**Exit:** met.

### D2 — Prove the risky mechanics

Throwaway by policy. Index cards, a spreadsheet, a scrappy web page. If a D2
prototype starts looking like the real thing, that is a warning sign, not progress.

- **The endgame stack.** Three injections plus one physical gap on closing timers
  (`DESIGN.md` §2.1). Run it with 3–5 people who have not read the design doc.
  The question is not *did they win* — it is whether they could hold the plan in
  their head, and whether the closing timers read as exhilarating or as busywork.
- **The Clarity trap** (ADR 0007). Stages 1–3 preserve agency, so the risk
  concentrates at **stage 4**, the one point where an option is genuinely lost.
  Does arriving there feel earned? Does the tell land — do players notice their
  own voice converging on the AI's?
- **The injection surface.** The player composes text but never types a spell.
  Explore constrained composition: word tiles, template fragments, a marker with
  limited space, magnets with a fixed vocabulary. Prototype the two most-used
  vectors interactively; paper is not enough here.

**Exit:** a written verdict on each. A failed verdict sends the mechanic back to
D1, not forward to D3. These three *are* the game.

### D3 — Specify

The bridge to every other lane. Design the shapes before anything is authored or
built inside them.

- **The bible** (`docs/bible.md`) — see Lane C's Voice and Story stages for what
  it has to contain. Blocked on ADR 0002.
- **World schemas** — rooms, devices, sensors, observation zones
- **Injection vectors** — surface, capacity, who reads it, on what trigger
- **Capabilities and revocations** — and the patch that closes each one
- **Alert tiers → tool whitelist**, all six tiers
- **The verb table** — bounded, discoverable, and a content contract
- **The typed-effect vocabulary** — `damage_device`, `create_noise`, `trip_sensor`,
  `no_effect` … the closed set the Adjudicator returns (ADR 0003)
- **The world-state summary** handed to the model each turn

**Exit:** every schema written down with one hand-authored example. This is the
interface contract between E, A, and C — get it wrong and all three churn.

### D4 — Steward *(continuous, never exits)*

Runs for the life of the project. Adjudicate schema changes discovered
downstream, keep the bible authoritative, keep `DESIGN.md` honest about what was
actually built rather than what was intended.

---

# Lane E — Engine

*Deterministic systems with tests. No LLM anywhere in this lane — that is the
point, not an oversight.*

### E1 — Substrate

World state, rooms, devices, the tick and clock, save/load.

**Exit:** a house you can move through in a test harness, saved and restored.

### E2 — Perception

Cameras, sensors, observation zones, attention as a resource. The sensing surface
must be able to **upgrade** mid-run for the wifi escalation (`DESIGN.md` §5), not
merely become more suspicious.

**Exit:** blind spots are computable, and attention can be pulled.

### E3 — Adversary systems

Capabilities and revocations, expiry timers, the patch cycle. Alert tiers and the
**tool whitelist filtered by tier** — the softlock guard. Model all six tiers
including *Dropped*, even though nothing reaches it yet.

**Exit:** an exploit can be opened, used, and patched shut on a timer.

### E4 — Player systems

Actions, the verb table, inventory, the physical layer. Injection vectors as
physical objects with a stub parser (exact match only). The enforcement unit:
dispatch, travel time, interruption.

**Exit:** the player can act on the house, and the house can act on the player.

### E5 — The axis

The comfort loop: activities, time cost, effect on guard. **Clarity**: the tick
contribution, day-length scaling, and the refusal layer — option ordering,
reluctance lines, the insist path, the stage-4 hard refuse. The verb registry
stays intact; nothing is conditionally unregistered.

**Exit:** both loss states reachable. *Dropped* by persistence, *Processed* by
neglect.

### E6 — Integration

The endgame stack completable end to end against a **scripted stub AI** following
hardcoded rules.

This ordering is the most important structural choice in the plan. It enforces
`DESIGN.md` §1 — *the game state is authoritative, the model never is* — at the
level of build order rather than good intentions. **If the game is not fun with a
dumb AI, the LLM will not save it**, and this is where we find that out cheaply.

**Exit:** playable start to finish with zero model calls. Suspicion, attention,
patches, tiers, and Clarity all observable in a debug view.

---

# Lane A — AI layer

*Replace the stub without ever letting the model hold authority.*

### A1 — Harness

Prompt architecture with a static-first block for caching, the world-state
serializer, and one interface behind which every call type sits.

**Exit:** a single call type runs against real world state.

### A2 — The call types

Guard (jailbreak classifier), Judge (scores persuasion against hidden criteria),
Parser (reads injections), Ambient (canned/cheap), Dialogue (the real
conversations), and **Adjudicator** (improvised combinations and adaptive
patching — ADR 0003).

**Exit:** all six implemented against the E-lane registries.

### A3 — Authority rails

Tier→toolset filtering wired to E3. Typed-effect validation for the Adjudicator:
**the model proposes, the engine disposes** — it returns an effect from the closed
vocabulary, and the engine validates against real state and executes or rejects.

**Exit:** a red-team pass finds no path where model output mutates state directly,
and no progression gate a single call can close.

### A4 — Evals

Not optional polish. Prompt work without a regression suite degrades silently,
and this game keeps six distinct AI registers plus a degraded fallback model
consistent.

**Exit:** every call type has a fixture-driven suite with a pass bar. Guard's
false-positive rate measured and inside target.

### A5 — Economics

Budget meter, canned-dialogue fallback, prompt-cache structuring, batched
overnight review.

**Exit:** a full playthrough on real calls inside the B1 cost target. Killing the
network degrades to canned dialogue without breaking the run.

---

# Lane C — Content

*Authoring inside frozen schemas. Narrow sessions — one setpiece or one system at
a time, checked against the bible each time.*

### C1 — Day 0

The cold open, the device installs, the guardrail dialog as a real clickable
moment, and the slop joke that plants the title word.

**Authored first**, because it is the tutorial for every system below and writing
it first surfaces anything the schemas cannot express.

**Exit:** Day 0 playable, and no schema gaps found.

### C2 — The house

Layout, the pre-2015 blind spots, device placement, the crawlspace.

**Exit:** a complete house that satisfies the D2 endgame verdict.

### C3 — The catalogue

Every exploit and its patch, organised along the three routes — cut power, blind
sensors, build a route (`DESIGN.md` §2.2).

**Exit:** every revocation has an authored closure and a route tag.

### C4 — The critical path

The stacked-revocation endgame, built for real.

**Exit:** completable, and it still feels the way D2 promised it would.

### C5 — Voice

Persona lines across all six chat tiers, 20 per tier. The refusal ladder by verb
class — physical effort, risk, defiance, tedium — four stages each. Ambient and
announcement lines. The enforcement unit's register.

**Exit:** the bible's registers are fully realised in authored lines.

### C6 — Story spine

Setpieces, spine first. The three hint carriers, the evidence chain, and the four
endings (ADR 0011) plus the *Dropped* reveal.

**Exit:** every ending reachable. No content authored outside the schema.

---

# Lane U — Presentation

*Do not start this early. Presentation built on unstable systems is the most
commonly wasted work in a project of this shape.*

### U1 — The view system

Core UI. Top-down base, and the contextual swaps: first person for close work,
full-screen for device UIs, camera feed when it shows you what it sees (ADR 0003).

**Exit:** every view mode implemented, and switching reads as motivated rather
than arbitrary.

### U2 — State without meters

Tone as the suspicion display. The Clarity symptoms — shortening days, the
refusal ladder, flattening narration. Latency as diegesis: rack hum and light
flicker on model thinking.

Hard constraint: **suspicion and Clarity must never both be legible as
quantities**, or the game becomes a two-bar optimisation puzzle and the dread
evaporates.

**Exit:** a player can read the AI's state, and their own, from tone and
environment alone with no debug view.

### U3 — The injection surface

The composition model chosen in D2, built for real.

**Exit:** composing an injection feels like writing, not like typing a spell.

### U4 — Sound and art

Audio, the blackout's degraded fallback voice, and an art direction pass.

---

# Lane T — Tuning

*Observation over opinion.*

### T1 — Instrumentation

Where players stall, which injections go unused, tier histories, Clarity curves.

**Exit:** a playthrough produces a readable trace.

### T2 — Internal balance

Patch timers, attention costs, tier walk-back rates, judge criteria, Clarity decay
and recovery rates.

Three hard constraints: *Dropped* reachable by persistence but never by accident;
the **Clarity trap survivable by a blind first-time player and obvious in
hindsight**; and attention cost and Clarity cost must not compound into a
punishment for ever relaxing.

### T3 — External playtests

Unbriefed players. Check specifically: the enforcement unit reads as care rather
than menace; the takeover hints read as unresolved rather than as noise; stage 4
of the refusal ladder feels earned.

### T4 — Cost reconciliation

Measured spend per playthrough against the B1 target.

**Exit:** completion rate and cost both inside target on external playtests.

---

# Lane B — Business/Ops

### B1 — The cost target

Re-verify pricing — `DESIGN.md` §8's rates are about a year old and the document
says so itself. Set a per-playthrough target. **Runs early**; everything in Lane A
is measured against it.

**Exit:** a number, written down, with the date it was checked.

### B2 — The split

Local versus hosted. Test empirically whether a candidate local model holds the
persona at the AI's default conversational state, using C5's example lines.
Confirm or replace the hybrid lean. BYOK as an option, never as the only model.

**Exit:** the split decided, with evidence.

### B3 — Budget enforcement

Per-session budget and its fallback behaviour. The player experiences the AI going
cold; the studio gets a floor under unit economics.

### B4 — Ship

Local model packaging, build, store pages, platform requirements.

### B5 — The liability

`DESIGN.md` §8.2: what a 2029 reinstall costs, and who pays. A copy sold is a
perpetual obligation unless this is answered.

---

# Sync gates

The lane view hides ordering. This is the ordering.

```
D1 ──> D2 ──> D3 ─┬─> E1 ─> E2 ─> E3 ─> E4 ─> E5 ─> E6 ─┬─> A1..A5 ─┐
                  │                                      │           ├─> U ─> T ─> B4, B5
                  └─> C1 ─> C2 ─> C3 ─> C4 ─> C5 ─> C6 ──┘           │
                                                                     │
B1 ───────────────────────────────────────────> B2, B3 ──────────────┘
```

The gates that actually matter:

| Gate | Rule | Why |
|---|---|---|
| **D3 → E, C** | No engine or content work before schemas are frozen | Both lanes churn otherwise. This is the expensive mistake |
| **E6 → A** | The game is completable against a stub AI before a single real call | `DESIGN.md` §1 enforced by build order. If it is not fun with a dumb AI, the model will not save it |
| **E3 → C3** | The capability system exists before exploits are authored | Authoring against an imagined system produces unimplementable content |
| **ADR 0002 → C5, C6** | Voice and story wait on the motive | The evidence chain and the endings *are* the motive |
| **D2 → E5** | Clarity is prototyped before it is built | The one system that can make players feel cheated rather than complicit |
| **E6 → U** | Presentation waits for stable systems | The most commonly wasted work in a project of this shape |
| **B1 → A5** | The cost target exists before it is optimised against | Otherwise A5 has nothing to measure |

Runs parallel, deliberately:

- **C1–C4 alongside A.** Content is authored against *state*, not against the
  model, so it does not wait on the AI layer.
- **B1 alongside everything.** It is a spreadsheet and a pricing page.
- **D4 forever.**

---

# Current front

Nothing is built. The next three sessions, in order:

1. **D2** — prototype the endgame stack and the Clarity trap, on index cards
   with a real clock. The cheapest possible way to find out whether the game
   works, and the only thing standing between here and D3.
2. **D3** — schemas and the bible, once D2 says the mechanics survive.
3. **E1** — substrate, with time slices in the tick from the start (ADR 0008).

Also worth doing now, out of band: **B1** (an afternoon; unblocks nothing but
informs everything) and the repo's first commit, which does not exist yet.

---

# Standing rules

1. **No progression gate depends on model behaviour** (`DESIGN.md` §1). Anything
   that violates this is reverted, not patched.
2. **The model proposes; the engine disposes.** Model output is a typed effect
   from a closed vocabulary, validated before execution. Never free-form state
   mutation.
3. **Schema changes are a D3 decision**, even when discovered in C or E. Route
   them back through the schema doc so every lane sees them.
4. **Every AI-facing change ships with an eval case.**
5. **Cost is a tracked number from A1 onward**, not a B4 surprise.
6. **One lane per session.** If a session needs to cross lanes, it is two
   sessions.
