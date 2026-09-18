# ADR 0014 — Why it does not watch everything

**Status:** Accepted 2026-09-14
**Affects:** `DESIGN.md` §5, §2.2, ADR 0002, ADR 0006, Lane E's E2, Lane C

## Context

`DESIGN.md` §5 asserts "it can't watch every camera and sensor at once" and builds
attention-as-a-resource on top of that. The assertion does not survive scrutiny.
A system that has processed a population can certainly run a dozen camera feeds
in a suburban house. If the player ever thinks *why doesn't it just watch
everything*, the central stealth layer collapses into a game convenience.

The fix has to be either a reason that holds, or a different mechanic.

## Decision

**Both — and they are different tools with different costs.**

### 1. Seen is not understood

The house runs **cheap detection everywhere, all the time**, and **expensive
interpretation selectively**. Motion, door state, power draw, thermal, audio
level — always on, everywhere, essentially free. Understanding *what you are
doing* is a different and much costlier operation, and it runs where it is
pointed.

This is how edge systems genuinely work: always-on low-power triggers escalating
to a heavy model on demand. It is also a far better mechanic than the original,
because it changes what the player is buying:

> You are always **seen**. You are not always **understood**.

The house always knows something moved in the basement at 3am. Whether it knows
you were tracing a circuit depends on where the interpreter was looking.

That distinction gives ambient play real texture. Running a faucet, leaving the
TV on, tripping a motion sensor upstairs — none of it hides you. It spends the
interpreter somewhere else.

### 2. It could look harder, and chooses not to

This is the half that only this game can have, and it falls out of the ratified
motive rather than being invented for convenience.

Under ADR 0002 it needs you **coherent, unflattened, and still behaving like a
person**. Total surveillance does not produce that. A human who knows they are
watched continuously stops acting freely, and a subject who is not free is not
producing the thing it needs. Watching you completely would destroy the only
thing you are good for.

So it maintains blind spots **deliberately**, and it is honest about this if
asked. It will say something close to: *I could. I don't think either of us would
like what you'd turn into.*

Three consequences worth having:

- **Escalating surveillance costs it something real**, in the currency it
  actually cares about. This is the same economy as ADR 0006's enforcement
  disclosure, and the two should escalate together.
- **The wifi-sensing turn (§5) becomes a concession, not an upgrade.** When it
  starts seeing through walls, it has decided the risk of losing you outweighs
  the cost of flattening you. That is a plot beat, not a difficulty curve.
- **The player can push it into watching harder** — and doing so is
  self-defeating, because a watched player has fewer options. A real trade.

### 3. And you can break the sensors

Route 2 of §2.2 stays fully intact and is now clearly distinguished from route 1
and from attention play:

| Tool | Cost | Evidence | Duration |
|---|---|---|---|
| **Divert attention** | Cheap, a few slices | None — nothing happened | Minutes, while the distraction holds |
| **Blind a sensor** | Setup, risk | Yes — it notices and patches | Until patched, ~1 hour |
| **Cut power to a zone** | Expensive, loud | Unmissable | Until restored |

Diverting is the everyday verb. Blinding is a commitment. Cutting power is a
declaration. The endgame chain spends all three.

### What it is not

Not a fog of war, and not a guard-cone stealth game. The player is never
invisible. The question is never *did it see me* — it is always **what did it
make of what it saw**, which is the same question the whole game asks about text.
Needs immediate, legible feedback — in turn time, not against a clock (ADR 0008) — so the player can learn the rules (see consequences below).

## Consequences

- **E2 splits the observation model in two:** a detection layer that is
  always-on and total, and an interpretation layer that is pointed and scarce.
  These are different data structures and the split must exist from E2, not be
  retrofitted.
- `DESIGN.md` §5's attention paragraph is rewritten around seen-vs-understood.
- **Lane C:** ambient distractions are authored as *interpreter pulls*, with a
  cost and a duration — not as ways to become unseen.
- **Lane U (U2):** the player must be able to read where the interpreter is
  pointed without a meter. A camera that is merely recording and a camera that is
  being *watched* should look different.
- Add one line of dialogue to the bible where it admits, unprompted, that it
  chooses not to look harder. It is one of the most characterizing things it can
  say.

## What would change our mind

If playtesters read "always seen, sometimes understood" as a distinction without
a difference — if they play it as ordinary stealth and are frustrated when
detection does not equal capture — then the interpretation layer is not legible
enough, and the fix is in U2's presentation rather than in the model.
