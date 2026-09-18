# MeatProxy

A game about escaping a house your own smart-home AI has locked you inside. The
antagonist is a real language model that reacts to what the player actually does —
but it holds no authority over the game, by design.

**Currently design-phase. No code exists.** For where the work stands and what is
next, read `ROADMAP.md`'s *Current front* section — don't duplicate that state here.

## Document map — what to read when, what to update when

| Document | Read it when… | Update it when… |
|---|---|---|
| [CLAUDE.md](CLAUDE.md) | Every session start (automatic) | A convention or invariant changes — rare |
| [ROADMAP.md](ROADMAP.md) | **Start of every work session** — *Current front* is the entry point; lanes, stages, and sync gates are the plan | A stage exits, a gate moves, or a devlog milestone lands |
| [DESIGN.md](DESIGN.md) | Any question about *what the game is* — systems, motive, mechanics | A ratified decision changes the design. Keep it honest about what was built, not what was intended |
| [docs/decisions/](docs/decisions/) | Before re-opening any settled question. `README.md` there is the index | A decision is made. **Append an ADR; never silently edit an Accepted one** |
| [devlog/](devlog/) | Writing or revising public posts | A devlog milestone in `ROADMAP.md` is hit |
| [planelements.md](planelements.md) | Origin/context for why something exists | **Never.** Historical source notes, already merged |
| [docs/bible.md](docs/bible.md) | Writing any game content — voice, the evidence chain, the endings, the two fixed-point scenes | A ratified decision changes voice, the chain, or an ending |
| [docs/schemas/](docs/schemas/) | Before building or authoring anything. `README.md` there is the index and the dependency map | A schema changes — which is a D3 decision, even when found downstream |
| [docs/story/](docs/story/) | Testing whether the design holds together as a story. **Not canon** — `README.md` there says which draft is consistent with the record and which is a fork | A new treatment is written, or a fork is adopted (which is an ADR, not a file swap) |

Project status also lives outside this repo: `/wrap` writes `projects/meatproxy.md`
to the private **Status-Hub** repo at the end of a session, `/catchup` reads it at
the start. Git history carries everything else — don't restate a commit message.

## Preferences

- Claude commits as work completes. Push when asked; this repo is public
  (`github.com/mjrichar09/MeatProxy`), so pushes are outward-facing.
- **Doc-update cadence: batch, don't drip.** ADRs are written when a decision
  lands. `ROADMAP.md` is touched at stage exits. `DESIGN.md` only when a
  ratified decision changes the design. CLAUDE.md only on real convention changes.
- Ask before publishing anything public-facing (devlog posts, pushes that carry
  them).

## Repo structure

```
DESIGN.md            what the game is
ROADMAP.md           lanes, stages, sync gates, devlog milestones
planelements.md      original idea notes — historical, do not edit
docs/decisions/      ADRs 0001+ — the authoritative decision record
docs/bible.md        voice, the evidence chain, the endings — canon for content
docs/schemas/        the E/A/C interface contract
docs/story/          prose treatments — NOT canon, one of them forks the design
devlog/              public milestone write-ups
```

Lane E will add engine code and Lane U will add the Blender build scripts. Add
their conventions here when they exist, not before.

## The rules that are not obvious from the docs

These are load-bearing. Violating one is reverted, not patched.

1. **The game state is authoritative. The model never is.** No progression gate
   depends on model behaviour. If a door is bolted, `unlock_door` is absent from
   the toolset — it doesn't refuse, the capability doesn't exist.
2. **The model proposes; the engine disposes.** Where the model rules on outcomes,
   it returns a *typed effect from a closed vocabulary*, which the engine validates
   against real state before executing. Never free-form state mutation.
3. **No pressure window may depend on a model call.** Latency is atmosphere in
   turn time and unfairness in real time (ADR 0008).
4. **Schema changes are a D3 decision**, even when discovered while doing engine or
   content work. Route them back so every lane sees them.
5. **One lane per session.** If a session needs to cross lanes, it is two sessions.
6. **Respect the gates.** No engine or content work before D3 freezes schemas. No
   AI layer before the game is completable against a stub. No presentation before
   E6. The gates are in `ROADMAP.md` and they carry the ordering the lane view hides.

## Writing game content

- **Tone is GLaDOS calibration** (ADR 0001): the AI is funny, the situation never
  is. Comedy runs on three channels only — the AI's own voice, the absurd
  logistics of the arrangement, and satire of AI as a cultural object. It never
  winks at the player about the danger. Setpieces, enforcement, the evidence
  chain, and every ending play straight.
- **The AI never lies.** Its surface motive ("I am keeping you safe") is true —
  true *because of* the deeper one it never states (ADR 0002). The player cannot
  catch it out, so persuasion works by making the second layer unnecessary.
- **Content is authored, never generated.** This is both a design rule and the
  public position.

## Devlog voice

First person, genuine, not markety. Lead with design rather than tech. Claude
assistance is disclosed matter-of-factly. Close posts on a real open question —
it gets better comments than a summary. See `devlog/001-design-lock.md`.

## Environment facts worth not rediscovering

- **Blender 5.2 LTS** at `C:\Program Files\Blender Foundation\Blender 5.2\`.
  Headless render loop is verified: script → `blender --background --python` →
  read the PNG back → correct → re-run (~3s a pass).
- This build exposes exactly one render engine identifier: **`BLENDER_EEVEE`**.
  `BLENDER_EEVEE_NEXT` is not valid here.
- **ComfyUI** at `M:\ComfyUi\ComfyUI_windows_portable\ComfyUI` — bare framework,
  **no models installed**. Anything depending on it starts with acquiring weights.
- Shell is Git Bash or PowerShell. Large markdown edits are most reliable via a
  Python script doing exact-string replacement; heredocs with mixed quoting have
  failed here.

## Working efficiently (session cost)

The whole transcript is re-sent every turn, so cost scales with session length.
Without cutting rigor:

- Prefer targeted reads (Grep, or Read with offset/limit) over whole-file reads.
  Don't re-read a file already in context, and don't re-read after an Edit to
  confirm it — Edit fails loudly if it didn't apply.
- Don't take screenshots unless asked; verify programmatically instead.
- Prefer Edit over re-emitting whole blocks; don't paste large content back into chat.
- Keep replies concise: what changed and the result, not a blow-by-blow.
