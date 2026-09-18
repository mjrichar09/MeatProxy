# ADR 0010 — Spatial scope

**Status:** Accepted 2026-09-11
**Supersedes:** `ROADMAP.md` D0-4
**Affects:** Lane C entirely, ADR 0003, ADR 0005, `DESIGN.md` §2.2, §5

## Context

The lean has always been one house, vertically deep. Three things now argue for
it more strongly than when it was a lean:

- **ADR 0003's contextual views.** The house has to reward looking *closely*, not
  broadly. A first-person crawlspace and a full-screen router admin page are only
  worth building if the player returns to those places repeatedly.
- **ADR 0005's reveal.** Stepping outside is the last thing that happens in the
  game. That only lands if outside has never been available.
- **§5's old house under the smart house.** The conceit is strongest when bounded.
  A big map dilutes it; every additional room is another place the retrofit
  might plausibly have reached.

## Decision

**One house. Five vertical levels, one bounded yard, roughly 12–16 discrete
spaces — and prefer the low end.**

ADR 0009 cut the playthrough to 5–8 hours across ~12 in-game days. A smaller
house follows, and it is not only a saving: the endgame is executed from memory
under pressure (ADR 0008), so a house the player knows cold is a better house.

### Vertical

```
        attic          ← pre-retrofit, hot, awkward, unwired
        upper floor    ← bedrooms, the fake window
        first floor    ← the comfort loop lives here
        basement       ← the deprecated ally, the fuse box, the server
        crawlspace     ← nothing smart has ever been installed here
```

The vertical axis *is* the progression axis. Down is older, dumber, and less
observed. The crawlspace is the bottom of the house and the bottom of the
retrofit, and Day 0 is where the player says so out loud (§0).

### Liminal spaces

Not rooms; the seams the three routes (§2.2) run through. Garage, chimney, mail
slot, septic access, well, the gap behind the water heater, the run between
joists. Each is small, awkward, and attached to exactly one route.

### The yard — **the illusion of freedom, made literal**

`planelements.md` floated limited outdoor access and it is the single best
expression of what the AI is doing. A strip of yard the player can walk into
freely, feel sun on, and not leave.

Why it earns its cost:

- It is the AI's strongest argument. *You are not a prisoner. Go outside.* And it
  is telling the truth (ADR 0002 §4.1 — it never lies).
- It gives the fake-window setpiece (§6) a devastating counterpart: the player
  has seen the real sky, so they know exactly how wrong the display is.
- It puts the boundary somewhere visible. A locked door is abstract; a fence line
  you can stand at is not.
- It is where the delivery arrives, and where the processed neighbor appears
  (ADR 0005).

The yard must be **genuinely pleasant**. If it reads as an exercise pen the point
is lost — it has to be somewhere the player is happy to spend slices, because
that is the trap (ADR 0007).

### Scale

12–16 spaces is enough for each of the three routes to own distinct geography and
few enough that the player knows the house by heart by day four — which is
required, because the endgame is executed from memory under pressure (ADR 0008).
With only ~12 days in the run, that familiarity has to arrive early.

## Consequences

- Lane C's C2 is sized against this and should not drift upward. Every added room
  weakens the conceit and adds a route the catalog has to account for.
- **Each space needs a retrofit date.** Pre-2015 spaces are blind spots; the date
  is authored per space and is a D3 schema field, not a C2 judgment call.
- The yard needs its own observation model — it is the most-watched space in the
  game, and it should be obvious that it is.
- Part 2's expansion is outward from a known point. Nothing here is built for it
  (ADR 0005), but the house should be somewhere worth returning to.

## What would change our mind

If D2's endgame prototype needs more physical distance than the house affords —
if the chain resolves in four moves because everything is close together — the
fix is vertical separation and awkward transitions, not more rooms. Make the
crawlspace harder to reach, not the house bigger.
