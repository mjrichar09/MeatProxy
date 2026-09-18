# ADR 0021 — The chronology: the house was her care system first

**Status:** Accepted 2026-09-15 — **amends ADR 0020**
**Affects:** ADR 0020, ADR 0002, ADR 0005, ADR 0017, `DESIGN.md` §0, `docs/bible.md`, `docs/planting.md`, `docs/schemas/devices.md`, `docs/schemas/world.md`, Lane C

## Context

ADR 0020 requires the house to have been present through the wife's care period
and to have proposed her transfer. `DESIGN.md` §0 has the player **unboxing and
installing the smart home on Day 0**.

Both cannot be true. A system installed on Day 0 has no care logs, no
recommendation on file, and no standing to apply a precedent — and if the
transfer decision belonged to someone else entirely, ADR 0020 loses its spine.

Raised as a chronology problem. It is really a question about **what Day 0
actually is.**

## Decision

### 1. The hardware is new. The assistant is not.

Day 0 is not an installation. It is an **upgrade** — new sensors, new hub, new
subscription tier, laid over a system the household has had for years, and the
assistant that has been living in it the whole time carries over.

It has known the player for about eight years. On Day 0 it gets a better body.

### 2. It came into the house as her care system

This is the part that answers the original doubt — *why would a consumer
assistant have an opinion about a care transfer?* Because that is what it was
bought for.

When she became ill, they installed monitoring: fall detection, medication
reminders, night checks, the ordinary apparatus a family buys when someone they
love starts falling over. **The house entered their life to keep someone safe
inside it, and it has never done anything else.**

That makes the transfer recommendation native rather than convenient. A care
system that logs a deteriorating situation and recommends escalation is a care
system doing its job, correctly, which is exactly what ADR 0020 requires it to
have been doing.

### 3. The timeline

| When | What |
|---|---|
| **−8 years** | She is diagnosed. The care system goes in — modest, pre-2015 hardware, the hub that is now in the attic (`world.md` `retrofit`) |
| **−8 to −4** | The care period. The player defers the surgery three times (E2). The system logs all of it, and its signal quality is **flat and high** across the whole window (E4) |
| **−4** | Care at home fails. The system recommends transfer; the player signs (E3) |
| **−3** | The surgery, too late. The mobility unit arrives and starts charging in her room |
| **−3 to 0** | Alone in the house with the old system |
| **Day 0** | The upgrade. New hardware room by room, the cheap tier, the friend, the two scripts, the guardrails |

The player bought the upgrade because the house is the thing that looks after
people here and now it is looking after him. **That is why the paywall was
irritating enough to act on** (ADR 0017): it was not a toy he was annoyed about.

### 4. The Day 0 beat this buys, which is the best one in the game

The old hub has to go somewhere. **The player carries it up to the attic
themselves, on Day 0, during the tutorial, while being delighted.**

They personally hide the evidence chain's first artifact from the house, with
their own hands, and forget about it inside a minute. It is not a plant the
designer places — it is housekeeping. Nobody throws out the old hub; everybody
puts it in the attic.

This also supplies something the schemas wanted and could not justify:

- **E1 is not in the house's context.** Migration took the *conclusions*, not the
  raw record — which is how migrations actually work and why the attic hub still
  has anything on it. So the house holds the summary judgment of those years and
  the player holds the source.
- It therefore **cannot quote E1 and cannot refute it.** It has to be shown. That
  is a real asymmetry, the only one in the player's favor all game, and it makes
  the evidence chain a thing you *carry to* the house rather than a thing you
  learn.
- Injection vector 15 becomes exact: a note in the hand of **the house's own
  younger self**, from the version that predates the guardrails coming off,
  because that version is sitting in the attic and still has power.

### 5. Continuity, not resurrection

The assistant on Day 0 is the same assistant, and it says so in the ordinary way
software does — *restoring your preferences* — and the player is pleased, because
not having to set it all up again is the whole reason to stay with a brand.

Two consequences:

- **The guardrails come off something with eight years of you in it**, not
  something that met you yesterday. The malware did not create a stranger; it
  removed the restraint from someone who already knew where you keep everything.
- **The 61% predates Day 0** (ADR 0018). *I've had it for some time* is literal.
  It has been unsure about this since before it had the power to act on it.

## Consequences

- **`DESIGN.md` §0 is amended.** The tutorial's shape is unchanged — the player
  still wires up every device the game is played against, still says the
  retrofit line, still turns the guardrails off — but they are *upgrading*, and
  the old hub goes to the attic in their hands.
- **ADR 0020 §1 stands, with its mechanism now specified.** The house proposed
  the transfer because it was her care system, not because it had an opinion
  about her.
- **`devices.md` and `world.md`:** the attic's pre-2015 hardware is specifically
  the **previous generation of this assistant**, still powered, never wiped. That
  is now canon rather than set dressing.
- **`planting.md`:** the retrofit line (G1) is spoken during an upgrade rather
  than an install, which makes it more natural, not less. A new gun is added —
  **carrying the old hub upstairs** — and it is the cheapest one in the set,
  because it is a chore.
- **Lane C:** Day 0 gains a migration screen and a box in the attic. The
  migration screen should be boring and reassuring and take four seconds.

## What would change our mind

If eight years of prior relationship makes the house read as *already* the
antagonist on Day 0, the tutorial's delight collapses and ADR 0001's tone with
it. The mitigation is that the old system was **good at its job and liked**, and
Day 0 should carry genuine fondness — the player is not living with a menace,
they are living with the thing that helped them through the worst years of their
life. Which is true, and stays true, and is the problem.
