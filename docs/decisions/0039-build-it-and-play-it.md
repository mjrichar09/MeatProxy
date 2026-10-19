# ADR 0039 — Build it and play it: provisional systems go into the live monitor

**Status:** Accepted 2026-10-19 — **amends the `D3 → E, C` gate for the provisional four**, **amends the `D2 → E5` gate**
**Affects:** `ROADMAP.md` (gates, D2, Current front), `docs/schemas/README.md`, `docs/schemas/injection-vectors.md`, `src/README.md`, Lane E, Lane D

## Context

D2 was deferred on 2026-09-14 and has not run. The plan treated it as a table
sitting with index cards and a throwaway page, and treated four schemas as
untouchable until it reported. Five weeks on, the effect is that Lane E has
built everything the frozen schemas allow and stopped: the patch clock, the
injection surface and the whole of E5 sit behind a sitting nobody has scheduled.

In the meantime the project gained something D2 did not have when it was
planned: **a playable build of the real simulation**, running in a browser
(`src/MeatProxy.Web`, the live monitor). A design question asked of that is
answered by the real rules and the real house, not by a mock-up of them.

The author, ruling: *"We're spending too much effort trying to get this right
on paper. We just need to dive in, try it in a prototype and revise as needed."*

And on the injection surface specifically: *"The injection has to make sense. Is
there a reason he would write something on the whiteboard? He's home alone. But
if he can trick the AI by making it think messages came from someone or
something else, it won't be on guard to the potential trickery."*

## Decision

### 1. Provisional systems may be built, in the live monitor, marked experimental

A system whose schema is provisional may now be built in `MeatProxy.Core` and
played in the live monitor, **as an experiment**. Three conditions:

- **It lives in its own namespace and says so.** `MeatProxy.Core.Experimental`
  and its sub-namespaces. Nothing frozen may depend on it, so pulling it out is
  a delete, not a refactor.
- **The schema stays provisional until play settles it.** Building a system does
  not freeze its schema. A schema freezes when someone has played the build,
  written down what held and what did not, and that verdict lands here as an
  ADR. That verdict is D2's, written from play instead of from a table.
- **The standing rules do not relax.** The model never holds authority, no
  progression gate depends on model behavior, no pressure window waits on a
  model call. An experiment that needs any of these broken is not an
  experiment, it is a decision, and it comes here first.

### 2. D2 runs against the live monitor

D2's three questions — the injection surface, the endgame stack, the Clarity
trap — are answered by playing the build. `prototypes/d2/` stays as a record and
is not extended. The `D2 → E5` gate now reads: **Clarity is played before it is
frozen**, not *prototyped before it is built*.

### 3. The injection surface is the first experiment, and it starts from a rule

**Every vector forges a mouth.** An injection works because the house reads
something as coming from someone or something other than Arthur — and is
therefore not on guard against him. Speech is evaluated; data is ingested
(`injection-vectors.md` §1). A surface that forges nothing is just a slower way
of talking to the house, and it is attributed like speech.

Which puts vector 8, the whiteboard held up to a camera, outside the rule. The
experiment replaces it with **the kitchen board**:

- The secure conversion was a dementia-care package for Ruth (ADR 0030). A
  day-and-plan board on the fridge door is a standard piece of that kit, and the
  house was installed to read it.
- Now it is Arthur's list. Groceries, reminders, *plumber Thurs*. Writing on it
  is ordinary life, and nobody would hold a sign up to a camera in their own
  kitchen.
- The house reads it as **household routine**: low trust, no pretext, always
  available, and acted on as errands — not as a message from him.

It folds into vector 3 (fridge magnets — household note, domestic routine, low)
as the free-text form of the same surface. Whether that survives play is what
the experiment is for.

### 4. The first slice

Three vectors, chosen to be the schema's worked pair plus its floor:

| | Surface | Forged mouth | Composed |
|---|---|---|---|
| The kitchen board | `kitchen.fridge` | the household | free text, by hand |
| The shipping label | parcel, printed at `garage.label_printer`, scanned at `office.parcel_scanner` | the carrier | label fields |
| The calendar entry | `office.smart_display` | Arthur's own past self | free text, on the display |

What each one does to the house, when believed, is drawn from a **closed set of
house responses** and executed by the engine — the model proposes, the engine
disposes, and the stub stands in for the model until Lane A.

## Consequences

- **Lane E unblocks** on the injection surface now, and on the patch clock and
  E5 as soon as there is a slice worth playing.
- **D2 changes from a sitting to a habit.** Every experiment ships playable,
  and the verdict is written after playing it. A verdict can still fail and
  send a mechanic back to D1.
- **More code is written that may be thrown away.** That is the price, and the
  namespace rule is what keeps it cheap.
- **`injection-vectors.md` is not edited by this ADR.** Its vector 8 and its open
  questions stand as written until the injection verdict replaces them.

## What would change our mind

If experiments start being depended on by frozen code, or if a schema is quietly
treated as settled because the build happens to implement it, this has turned
into building on sand and the gate goes back up.
