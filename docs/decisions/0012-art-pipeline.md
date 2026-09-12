# ADR 0012 — Art pipeline

**Status:** Proposed
**Affects:** Lane U entirely, **D3's room schema**, ADR 0003, ADR 0010

## Context

ADR 0003 committed to contextual views: top-down base, first-person for close
work, full-screen device UIs, camera feed. That means **the same space must be
depicted from several angles** — expensive to hand-draw across 12–16 spaces
(ADR 0010), and hard to keep lit consistently, which matters because the blackout
setpiece and light-flicker-as-latency both depend on controllable lighting.

This is a solo project (ADR 0009) and the intent is for most of the art
production to be AI-assisted.

## What was verified, not assumed

Blender 5.2 LTS is installed and runs headless on the dev machine. A probe
confirmed the full loop end to end:

1. Write a `bpy` script that builds geometry, materials, camera and lights
2. `blender --background --python script.py` renders a PNG in ~3 seconds
3. The rendered PNG is read back and inspected
4. The script is corrected and re-run

The first probe rendered flat grey with the props invisible; the second, after
correcting the engine and adding materials, read correctly. **The loop is real
and self-correcting** — that is the load-bearing fact behind this decision.

Note for future scripts: this Blender build exposes exactly one render engine
identifier, `BLENDER_EEVEE`. The `BLENDER_EEVEE_NEXT` name used by some 4.x
builds is not valid here.

**ComfyUI** is also installed, at
`M:\ComfyUi\ComfyUI_windows_portable\ComfyUI` — but as a **bare framework**.
Verified 2026-09-11: no checkpoints, no ControlNet models, one LoRA,
`extra_model_paths.yaml` still the unedited sample, custom nodes limited to
Manager, GGUF and IPAdapter, server not running. Any plan that depends on it
starts with acquiring models, which is a deliberate step and not a small one.

## Decision

**Pre-rendered 3D → 2D sprites, built procedurally from the world schema.**

```
D3 room schema ──> bpy build script ──> scene ──> batch render ──> sprite sheets
   (one source            (code)                   (camera rig)      (Lane U)
    of truth)
```

### The schema drives the geometry

This is the part that matters now, well before any art is made. The D3 room
schema already has to describe rooms, dimensions, devices, observation zones and
retrofit dates for the engine. **The same records can drive the Blender build.**

One source of truth produces both the playable house and the rendered house, so
they cannot drift apart. A room resized in the schema is a room resized in the
render, without anybody redrawing anything.

**Actionable now:** D3's room schema must carry the fields geometry needs —
dimensions, wall openings, door and window positions, fixed-fixture placement,
and floor level. Those are cheap to include while the schema is being designed
and expensive to retrofit later.

### Why pre-rendering fits this project specifically

- **One asset serves every view.** Model once, render top-down, first-person, and
  camera-feed angles from the same scene.
- **Lighting is consistent and controllable** across all views, which the
  blackout and the latency-flicker both need.
- **Topology does not matter.** Since the output is 2D, mesh quality only has to
  hold up from the rendered angle. This forgives the main weakness of generated
  or asset-pack meshes and widens what is usable.
- **Two art modes, not four.** Device views are diegetic UI — screens, not
  drawings. Camera-feed is the top-down render with a filter and grain. Only
  top-down and first-person need actual art production.

### Division of labour

| Well suited to AI | Needs the human |
|---|---|
| Procedural geometry from the schema | **Art direction** — what the house feels like |
| Camera rigs, per-room view positions | Silhouette readability at sprite size |
| Lighting setups and their state variants | Style consistency across the asset set |
| Materials and texture assignment | Taste. Every call about whether it looks good |
| Batch render and export pipelines | Approving or rejecting each pass |
| Iterating scripts against rendered output | — |

The probe demonstrates the left column. It also demonstrates the limit of it: the
render was legible and completely characterless. **Getting a picture is
automatable; getting a look is not.**

### Props

Three sources, in order of preference:

1. **Procedural** — anything box-like, which in a house is most of it
2. **Asset packs**, arranged procedurally. Fastest path to a furnished room
3. **Generated meshes** (text-to-3D) for background clutter only. Viable here
   *because* topology is irrelevant to a pre-render, but style consistency is
   poor, so keep them out of hero positions

## ComfyUI

Diffusion has a place here, and it is a narrow one. The question is not whether
it is capable — it is *where in the pipeline the style is allowed to enter*,
because that choice determines both the consistency risk and the public framing.

### Where it fits, best first

1. **Concepting.** Exploring what the house should feel like before an art
   direction is committed. Immediate value, zero downstream exposure, and it is
   genuine design work rather than production.
2. **Textures.** PBR and tileable surfaces applied to Blender materials. Style
   enters at the *material* level, so the renderer enforces consistency across
   every view for free. This is the highest-value production use.
3. **Style pass over renders.** img2img at low denoise with ControlNet (depth or
   lineart) driven by the Blender render itself. The control image locks
   geometry; diffusion supplies the character a raw render lacks. Directly
   addresses the weakness the probe exposed.
4. **Clutter meshes.** Hunyuan3D / TripoSR-class nodes for background props.
   Viable because topology is irrelevant to a pre-render; keep out of hero
   positions.

Driving it is the same closed loop as Blender: ComfyUI exposes an HTTP API
(`/prompt` with workflow JSON), so a workflow can be submitted, the output PNG
read back, and the workflow corrected.

### The consistency risk

This lands precisely where the project is most exposed. ADR 0003 requires **the
same space depicted from several angles**. Two angles passed through diffusion
independently will not match — different grain, different micro-detail, subtly
different materials — and mismatch between views is exactly the artefact players
notice.

Ranked mitigations:

- **Push style into textures** (use 2 above, not 3). The geometry then enforces
  consistency and diffusion never sees the final frame. Safest by a wide margin.
- Tight ControlNet plus low denoise, locked seed, fixed prompt.
- A LoRA trained on the approved house style, once a style exists to train on.

**Trial both on one room before committing.** If the texture route holds up, the
style pass is unnecessary and the risk disappears with it.

### The framing question

`ROADMAP.md`'s devlog plan commits to public posts and to disclosing AI
assistance. AI-assisted code and generated *textures* are one conversation.
**Final visible pixels out of a diffusion model are a different one**, and in the
communities this project will be posted to it will dominate the response
regardless of the game's merits. That is a description of the audience, not a
judgement of the method.

Three defensible positions:

| | Position | Exposure | Cost |
|---|---|---|---|
| **A** | Concepting only | None | Forgoes production speedup |
| **B** | Interior only — concepting and textures; final pixels are Blender renders | Low, easy to state plainly | Some manual art direction remains |
| **C** | Style pass on final frames, disclosed | High — becomes the topic | Best-looking output, fastest |

**Decision: start at A, evaluate B during U4.** Concepting pays off immediately
and carries no downside. The final-pixel question does not need resolving until
Lane U opens, which is months out behind E6 — and by then there will be renders
to judge rather than predictions to argue about.

C is not ruled out. It is deferred, deliberately, until there is evidence.

## Consequences

- **Do not start this now.** Lane U is gated behind E6 and that gate holds. The
  only thing this ADR changes today is D3's schema fields.
- **ComfyUI needs models before it is useful at all.** Treat acquiring them as a
  Lane U task, not a prerequisite for anything earlier. Concepting (position A)
  is the one exception and can happen whenever it is useful.
- The build script is **project code**, versioned and reviewed like engine code.
  It is not a throwaway artist file.
- Renders are build artifacts. Decide early whether sprite sheets are committed
  or generated on demand; committing large PNGs to git is a decision, not a default.
- An **art direction pass is a real Lane U stage** (U4) and cannot be delegated.
  Budget it as design work, not production work.
- Public framing (see `ROADMAP.md` Devlog): this is authored 3D rendered to 2D,
  not generated images. That distinction matters to the audiences this project
  will be posted to, and it is worth stating plainly rather than leaving to
  inference.

## What would change our mind

If the first real room takes a week of script iteration to look acceptable, the
procedural approach is losing to hand-authoring and the fallback is hand-placed
Blender scenes — keeping the render pipeline, dropping the schema-driven build.
The pipeline is the valuable half; the procedural build is the optimisation.

On ComfyUI specifically: if the texture route (position B) cannot carry the look
and the renders stay characterless through U4, that is the point to reconsider
position C on its merits — with finished renders in hand and a clear view of what
disclosing it would cost.
