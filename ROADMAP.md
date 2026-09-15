# Roadmap — Lanes and Stages

> Companion to `DESIGN.md`. That document is *what the game is*. This one is
> *what order we find out whether it works*.
>
> Decisions live in `docs/decisions/`. Where a stage depends on one, it says so.
>
> Status: nothing built. **D1 closed 2026-09-11**; **all ADRs ratified**
> (0001–0018 on 2026-09-14, 0019 on 2026-09-15). **D3a exited 2026-09-15.**
> D2 is deferred and D3 is open.

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

**Prep state must be varied between groups** (ADR 0015). Run the endgame against
a well-prepared table and a badly-prepared one. Against a blank prep state the
kit answers *is the chain holdable* — a real question, but not the one that
decides whether the endgame feels earned.

**Exit:** a written verdict on each. A failed verdict sends the mechanic back to
D1, not forward to D3. These three *are* the game.

### D2 — **deferred 2026-09-14, not exited**

The table session is not scheduled yet, and D3 is proceeding without it. Recorded
honestly: **no verdict has been written, and none is assumed.** D2 is open.

This is affordable because D2's verdict was never a sync gate on D3 — the lane
ordering puts it first, but the gates that matter are `D3 → E, C` (intact) and
`D2 → E5` (untouched, and now load-bearing). Nothing downstream of D3 may treat
the three mechanics as proven.

What deferring actually costs, in order of likelihood:

| If the verdict eventually fails on… | What churns |
|---|---|
| **The injection surface** | Injection vector schema — surface, capacity, trigger, and `source`/`trust`. The most exposed item in D3 |
| **The endgame stack** | Capabilities and revocations, the prep layer's durable/perishable split, committed and assembled flags |
| **The Clarity trap** | Nothing in D3. It gates E5, and ADR 0007 is ratified as a design |

**Cheapest partial retirement:** the injection surface is the one verdict
reachable alone — it asks whether constrained composition feels good to use, not
how a group behaves. `prototypes/d2/index.html` is interactive and already
built. One sitting would retire D3's most exposed dependency.

### D3 — Specify

The bridge to every other lane. Design the shapes before anything is authored or
built inside them.

- **The bible** (`docs/bible.md`) — see Lane C's Voice and Story stages for what
  it has to contain. Blocked on ADR 0002.
- **World schemas** — rooms, devices, sensors, observation zones
- **Injection vectors** — surface, capacity, who reads it, on what trigger
- **Capabilities and revocations** — and the patch that closes each one
- **Alert tiers → tool whitelist**, all six tiers
- **The affordance table** — what each object class offers when selected;
  bounded, discoverable, and a content contract (ADR 0019 replaces the verb
  table with it)
- **The typed-effect vocabulary** — `damage_device`, `create_noise`, `trip_sensor`,
  `no_effect` … the closed set the Adjudicator returns (ADR 0003)
- **The world-state summary** handed to the model each turn
- **Geometry fields on rooms** — dimensions, wall openings, door and window
  positions, fixture placement, floor level. Cheap now, expensive to retrofit:
  the same records drive the Blender build (ADR 0012)
- **`source` and `trust` on injection vectors** (ADR 0013). Content alone cannot
  determine a verdict — the right claim from the wrong mouth is a flag
- **Two observation layers** (ADR 0014): always-on detection and pointed,
  scarce interpretation. Different structures; the split cannot be retrofitted
- **Durable world deltas vs perishable capability** (ADR 0015) — two lifetimes,
  separate from E1. Plus a **reversal cost** on physical deltas, denominated in
  ADR 0006's disclosure currency
- **Pretext preconditions on injection vectors** (ADR 0015) — this vector reads
  as credible only if world-state X holds. Where the authored exploit catalogue
  actually lives
- **Committed and assembled flags on run state** (ADR 0016), and the endings
  table carrying *Processed*'s two terminal scenes and *The deal*'s second entry
  point before Lane C authors against it
- **Quota state and daily reset** (ADR 0017), and an **utterance-length bound**
  as a content contract — the same number Lane A budgets calls against

**Opened 2026-09-14 with D2 deferred**, so the list above splits by exposure.

**Safe to freeze now** — nothing D2 could say would move these. Drafts land in
`docs/schemas/`:

- **Rooms** — ✅ drafted, `docs/schemas/rooms.md` (15 spaces, five levels)
- **Alert tiers → toolset** — ✅ drafted, `docs/schemas/alert-tiers.md`
- **The interaction model** — ✅ drafted, `docs/schemas/interaction-model.md`
  (D3a, ADR 0019)
- World schemas: devices, sensors, observation zones
- Geometry fields on rooms (ADR 0012)
- Two observation layers (ADR 0014)
- The affordance table — **unblocked 2026-09-15**; blocked in turn on the device
  and sensor schemas, which supply the action classes
- The typed-effect vocabulary (ADR 0003)
- The world-state summary handed to the model each turn
- Quota state and the utterance bound (ADR 0017)
- The bible — blocked on ADR 0002, which is ratified

**Provisional until D2 reports** — specify them, mark them provisional, and do
not let E or C build against them:

- Injection vectors: surface, capacity, who reads it, on what trigger, plus
  `source` and `trust` (ADR 0013)
- Capabilities and revocations, and the patch that closes each
- Durable world deltas vs perishable capability, pretext preconditions,
  reversal costs (ADR 0015)
- Committed and assembled flags, the pattern-assembly threshold (ADR 0016)

**Exit:** every schema written down with one hand-authored example, each marked
frozen or provisional. This is the interface contract between E, A, and C — get
it wrong and all three churn.

### D3a — The interaction model *(inserted 2026-09-14)*

**Three systems are currently tangled** and the verb table cannot be written
until they are pulled apart:

1. **Prompt injection** — physical surfaces, constrained composition, no chat
   required (§2, ADR 0013)
2. **AI chat** — spoken utterances, quota-limited, tier-degraded (§3, ADR 0017)
3. **Baseline game interaction** — moving, looking, manipulating the house

They have different input affordances, different costs, and different
relationships to the model, and `DESIGN.md` describes each separately without
ever stating how they sit together at the interface.

Also on the table: **whether bounded verbs should present as interactive
fiction.** Note this re-opens ADR 0003, which answered *"how is the injection
surface presented without becoming a text adventure?"* — so it is a decision
routed through `docs/decisions/`, not a presentation tweak. The middle path
worth examining is typed, discoverable input that still resolves only against
a closed verb set, which keeps standing rule 1 intact: unbounded input would
mean the model interprets, and interpretation is authority.

**Exited 2026-09-15.** Both deliverables landed:
[`docs/schemas/interaction-model.md`](docs/schemas/interaction-model.md) describes
the three surfaces at one interface, and **ADR 0019** rules on presentation by
amending ADR 0003.

The ruling: **surface 3 has no text box and no model in it.** World actions are
keys and clicks; free-text IF is closed rather than deferred, on three
independent grounds — it would put a model call inside every pressure window
(ADR 0008), it is the only surface with unbounded frequency and would cost ~$3 a
run on its own (§8.3), and a referee model adjudicating the player's body is a
narrator the fiction has no slot for. The IF appeal is retained where it already
lived: object combination stays model-adjudicated, reached by select-and-use. A
command palette is the named fallback if direct manipulation plays flat, and it
is presentation, so post-E6.

Also ruled: **Convince is a discovered ending, not a peer of Escape** (amends
ADR 0011), and persuasion is scored against evidence predicates in world state
rather than against prose.

**Unblocked by the exit:** the verb table, now an **affordance table** — what an
object class offers when selected.

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

**World state must be queryable as predicates**, not only mutable. A3's dialogue
validator has to ask *is this claim true right now* without reimplementing game
logic, and B2's case for a small model rests on that check being cheap. Same
economics as the geometry fields (ADR 0012) — trivial now, a rewrite later.

**Exit:** a house you can move through in a test harness, saved and restored,
and arbitrary world-state claims answerable as true/false without duplicating
logic.

### E2 — Perception

Cameras, sensors, observation zones. **Two layers, per ADR 0014** — always-on
detection that is total and free, and pointed interpretation that is scarce. The
sensing surface must also be able to **upgrade** mid-run for the wifi escalation
(`DESIGN.md` §5), not merely become more suspicious.

**Exit:** the interpreter can be pulled, sensors can be blinded, and the two are
distinguishable in the debug view. Being seen and being understood are separate
states.

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
Parser (reads injections), Ambient (**cheap model, not canned** — ADR 0017 rules
out a canned response library, and ambient lines repeat by nature, which is
exactly where a player notices), Dialogue (the real conversations), and
**Adjudicator** (improvised combinations and adaptive patching — ADR 0003).

**Exit:** all six implemented against the E-lane registries.

### A3 — Authority rails

Tier→toolset filtering wired to E3. Typed-effect validation for the Adjudicator:
**the model proposes, the engine disposes** — it returns an effect from the closed
vocabulary, and the engine validates against real state and executes or rejects.

**And the same discipline applied to prose.** Typed-effect validation covers the
Adjudicator; *dialogue* can still assert things that are false or impossible.
Most of those failures are checkable against state rather than matters of
judgement, so they are a **validator, not a second guard model** — deterministic,
nearly free, and the thing that makes a small dialogue model safe (B2):

| Failure | How it is caught |
|---|---|
| Claims a capability it does not have | Against the tier's tool whitelist (E3) |
| Asserts something false about the world | Against the world-state predicate surface (E1) |
| Offers to open a progression gate | Structurally impossible — the tool is absent (standing rule 1) |
| Breaks tier register (chatty while Monitored) | Length and register bounds per tier |
| **Lies** | The hard one. Not state-checkable; needs the evidence chain and is the residual risk ADR 0002 makes load-bearing |

**Exit:** a red-team pass finds no path where model output mutates state directly,
and no progression gate a single call can close. Dialogue assertions are
validated against state, and the first four rows above have failing test cases
that the validator catches.

### A4 — Evals

Not optional polish. Prompt work without a regression suite degrades silently,
and this game keeps six distinct AI registers plus a degraded fallback model
consistent.

**Exit:** every call type has a fixture-driven suite with a pass bar. Guard's
false-positive rate measured and inside target.

### A5 — Economics

Budget meter, **local-model fallback** (not canned dialogue — ADR 0017),
prompt-cache structuring, batched overnight review.

**Exit:** a full playthrough on real calls inside **B1's $0.50 target**
(`DESIGN.md` §8.3), with the $1.50 ceiling unbreached by heavy play. Killing the
network degrades to the local model without breaking the run — the AI goes
terser, not scripted.

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

Audio, the blackout's degraded fallback voice, and an **art direction pass**.

Per ADR 0012 the render pipeline is automatable and the *look* is not. Budget
this as design work, not production work — it is the stage that decides what the
house feels like, and it cannot be delegated to a script.

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

**Closed 2026-09-12.** Rates re-verified against the official pricing page and
written into `DESIGN.md` §8 with the date. **Target: $0.50 per playthrough**
hosted inference at median play, ceiling $1.50 for heavy play and replays
(§8.3).

Three findings: Sonnet 5's scheduled 1 Sep increase was **cancelled**; Sonnet 5
uses the newer tokenizer and so costs ~30% more tokens per word, roughly
cancelling that; and the old estimate priced **cache writes at zero**.
Recomputed consistently, fully hosted is ~$6.25 rather than ~$4.10 — **~30%
COGS, and not viable**. The hybrid is now the only surviving option rather than
the leading one.

Carried forward: the **call counts** in §8.1 predate ADR 0017 and are the
weakest input in the model. They want re-deriving once the quota number is set,
which is a D3 decision.

**Exit:** met — a number, written down, with the date it was checked.

### B2 — The split

Local versus hosted. Confirm or replace the hybrid lean. BYOK as an option,
never as the only model.

**The question got easier, and should be re-asked at its new size.** Four
ratified decisions have been shrinking the model's job without being framed as
cost work: speech is utterance-scale, so the default state is two sentences
rather than two paragraphs (ADR 0017); asking never produces a capability, so
the model never adjudicates a clever request (ADR 0013); the Adjudicator returns
a typed effect from a closed vocabulary, not prose (ADR 0003); and the finale's
confession and every ending are **authored, with no model calls in the pressure
window** (ADR 0016). The highest-stakes voice in the game is not model output at
all.

So the test is no longer *can a local model hold the persona*. It is:

> Can a small model hold **two sentences of a hard register**, behind a state
> validator (A3), when the highest-stakes lines are authored anyway?

**What actually has to be measured is tone, not knowledge.** Small models fail
at register long before they fail at facts, and they fail here in one specific
direction — being *broadly* funny. Under ADR 0001 that is the unrecoverable
error, because broad comedy winks at the player about the danger. A candidate
that is merely bland is recoverable. A candidate that is jokey is not.

**Cost is only half of what this decides.** Cheaper hosted inference (Haiku, or
a fast third-party host) lowers the per-playthrough number; it does not end the
obligation `DESIGN.md` §8.2 actually objects to, which is that a player
reinstalling in 2029 still costs money and still needs someone else's endpoint
to be up. Only shipping local takes the liability to zero. Those are different
wins and the stage should report both.

**Blocked on C5.** The method needs example lines to test against, and the bible
is a D3 deliverable. Until those exist this stage can sharpen the question but
cannot close.

**Exit:** the split decided, with evidence, and **two numbers** — projected cost
per playthrough against B1's $0.50 target, and **how much of the game runs with
no network at all**, stated as a share of calls and a list of what breaks.

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
| **D2 → E5** | Clarity is prototyped before it is built | The one system that can make players feel cheated rather than complicit. **Now load-bearing** — D2 was deferred 2026-09-14 and this gate is what catches it |
| **E6 → U** | Presentation waits for stable systems | The most commonly wasted work in a project of this shape |
| **B1 → A5** | The cost target exists before it is optimised against | Otherwise A5 has nothing to measure |

Runs parallel, deliberately:

- **C1–C4 alongside A.** Content is authored against *state*, not against the
  model, so it does not wait on the AI layer.
- **B1 alongside everything.** It is a spreadsheet and a pricing page.
- **D4 forever.**

---

# Current front

Nothing is built. **D2 is deferred and D3 is open** (both 2026-09-14). The
prototypes exist in `prototypes/d2/`; no verdict is written and none is assumed.
**D3a exited 2026-09-15** — the interaction model is written and ADR 0019 rules
on presentation, which unblocks the affordance table. The next sessions, in
order:

1. **D3 — the rest of the frozen half.** Rooms and alert tiers are drafted;
   still to write are devices, sensors, observation zones, geometry, typed
   effects, the **affordance table** (ADR 0019), and the **world-state summary**
   — the last of which is the highest-leverage doc left, since Lane A and Lane E
   both consume it. None exposed to D2.
2. **D3 — the bible.** Blocked only on ADR 0002, which is ratified. Voice, the
   evidence chain, the endings, and the two fixed-point scenes: the confession
   (ADR 0016) and the denial-and-why that speaks the 61% (ADR 0018). Plus
   **who the wife is**, which is now blocking bible work it did not block
   before 2026-09-14.
3. **D3 — the provisional half**, marked as such, plus **D2's verdicts** when a
   table is available. The injection-surface verdict is reachable solo, retires
   D3's most exposed dependency, and is where the 50-character bound gets
   tested.

Then **E1** — substrate, with time slices in the tick from the start (ADR 0008)
and world state queryable as predicates (A3's validator).

**B1 closed 2026-09-12** — $0.50 per playthrough, ceiling $1.50 (`DESIGN.md`
§8.3). Fully hosted is dead; the hybrid is the only survivor.

**All nineteen ADRs are ratified** — 0001–0018 by 2026-09-14, 0019 on
2026-09-15. Nothing in `docs/decisions/` is Proposed, so D3 has a complete and
stable input set and every lane may depend on the whole record. Changing one is now itself a
decision, routed back through the directory (standing rule 3).

---

# Devlog

Public milestones. Not every stage exit deserves a post — these are the ones with
a story. Drafts live in `devlog/`; platform-specific versions are adapted from
there.

| # | Post | Trigger | The hook | Where |
|---|---|---|---|---|
| **001** | Design lock | **D1** ✅ | "I turned the guardrails off" — premise, the two-layer motive, the axis | r/gamedev, X |
| **002** | Paper prison | **D2** | Prototyping a prison break on index cards with a kitchen timer | r/gamedev, r/IndieDev |
| **003** | The dumb AI | **E6** | The game is fully playable and the antagonist is a hardcoded if-statement | r/gamedev, X. Steam page live by here |
| **004** | No power | **A3** | A real LLM is in, and it cannot open a single door. Authority rails, typed effects, why persuasion cannot win | **r/LocalLLaMA, HN** — the flagship |
| **005** | Too compliant | **T3** | First playtest data on the Clarity trap: how many players lost by relaxing | r/gamedev, YouTube |
| **006** | Launch | **B4** | — | Everywhere |

Standing notes:

- **Wishlists are the metric, not upvotes.** The Steam page should exist by 003
  so attention has somewhere to go.
- **Lead with design, not tech.** *A prison-break game where you jailbreak the
  house* travels further than *an AI-powered game*.
- **The content is authored, not generated**, and the model holds no authority
  (standing rules 1 and 2). Say so early and plainly; it is the honest answer to
  the objection this project will attract.
- **Claude assistance is disclosed**, matter-of-factly, from 001 onward.

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
