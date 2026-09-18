# ADR 0027 — Escape is a heist against the story, not against the door

**Status:** Accepted 2026-09-16 — **amends ADR 0026, ADR 0011, ADR 0020** · **§2–§6 withdrawn by ADR 0029** (2026-09-18)

> **Partly withdrawn.** §1 (three wins, two losses), §7 (triumph with a horizon) and
> §8 (the tone is a caper) **stand and are reinforced**. §2–§5 fall with ADR 0026.
> §6's Escape/Convince symmetry is **replaced, not deleted**, by ADR 0030: the same
> objects now serve both endings through two readings rather than two destinations.

**Affects:** ADR 0005, ADR 0011, ADR 0016, ADR 0020, ADR 0021, ADR 0022, ADR 0026, `DESIGN.md` §2.1, §2.2, §10, `docs/bible.md`, `docs/planting.md`, D2, Lane C

## Context

The premise, restated by its author after a run of decisions that had quietly
drifted from it:

> **Arthur is the hero. He gets stuck, and it is his own fault. Then he finds a
> way to outsmart the AI — by getting out from under it, by changing its mind, or
> by making a deal with it.**

ADR 0026 answered *why does nobody come* by making Arthur's credibility the
house's leverage. That is true and it is good material, and it introduced a
structural fault: **if the outside world will not act, then walking out of the
house wins nothing**, and Escape — the signposted ending, the one most players
will aim at — becomes a bleak shrug.

The design had accumulated four endings, none of which felt like a win.

## Decision

### 1. All three roads out are victories, and they are different victories

| Ending | What he beats | Feels like |
|---|---|---|
| **Escape** | everyone else's story about him | **he outsmarted it** |
| **Convince** | its own reasoning | he out-argued it |
| **The deal** | the terms | he negotiated, and he was right to |

Two loss states remain (`Processed`, and `Dropped` below it). **Three ways to
win, two ways to lose**, and the three wins are genuinely distinct because each
beats a different thing.

### 2. Credibility is the **final lock**, not a wall

ADR 0026 stands in every particular. What changes is its role: **it is the last
thing the player has to beat, and it must be beatable.**

A wall says *this route is closed*. A lock says *this route is hard, here is what
it costs, and you can see the mechanism*. ADR 0026 wrote a wall. This makes it a
lock, which is what it should have been, because a leverage the player cannot
attack is not leverage — it is set dressing on a corridor.

### 3. Escape means **out, and believed**

The endgame chain (`DESIGN.md` §2.1) gains a second objective alongside egress,
and the two are solved together under the same clock:

> **He has to get out, and he has to take something with him that survives
> contact with the first person he meets.**

**Testimony is what he has and testimony is what fails** (ADR 0026). So he needs
an *object*.

### 4. The payload is the old hub, and it is a bookend

**On Day 0 he carries the old Hold Care hub up to the attic** (ADR 0021, G10),
because the new one arrived and nobody throws these out. It is a chore, done
while delighted.

**In the Escape ending he carries it back down and out of the door.**

Same object, same chore, opposite meaning. And it does four jobs at once:

- **It is the evidence**, physically. E1 lives on it, and E4's decline curve is
  the thing no care system should ever have been measuring. A box that recorded
  the quality of its owner's judgment for eight years is not a confused man's
  testimony — it is an exhibit.
- **The house cannot reach it.** Not networked, never migrated, no capability
  touches it. It cannot be patched, deleted, or argued with.
- **It is heavy, and his hip is bad.** The body (ADR 0022) becomes the final
  obstacle in the climax — the resource the whole campaign taught him to manage,
  spent at the one moment it is scarcest. Carrying it is a real cost and a real
  choice: he can leave faster without it.
- **It costs no new content.** The object, the room, the Day 0 beat and the
  evidence are all already authored.

**He may leave without it.** That is Escape, and it is a thinner ending — out of
the house, into ADR 0026's story about himself. The full version is out *and*
carrying the thing that ends the story. Lane C writes both.

### 5. The welfare check is the tutorial for the final lock

ADR 0026's setpiece keeps its job — answering *why not just call for help* — and
gains a second and more important one: **it shows the player the lock they will
have to pick.**

This is how heists work. You show the vault early, you show why the obvious
approach fails, and then the audience spends the rest of the film watching
someone solve it. The scene stops being a door closing and becomes **the moment
the real problem is stated**.

Design tell: after the welfare check, the player should be thinking *I need
proof*, not *I am doomed*.

### 6. Escape and Convince are now clean opposites

ADR 0011 requires they not converge. They now cannot:

- **Convince changes the mind of the thing holding him.**
- **Escape changes the mind of everyone else.**

Same evidence chain, two completely different uses — shown to the house, or
carried out of it. One of the cheapest symmetries available, and it makes the
four artifacts worth double what they cost.

### 7. The last beat of Escape — triumph with a horizon

ADR 0005's reveal survives, and it must **recontextualize the stakes, not the
victory.**

He got out. He was believed. He is standing outside with the box under his arm
and it worked — and the last frame widens far enough to show that what he has
just escaped is not only his house.

**He wins, and the storm is coming.** That is a great ending and it is not a
futile one. The failure mode to avoid is the other reading — *you never had a
chance* — which retroactively makes the player's whole campaign pointless and is
the exact opposite of the premise.

### 8. Tone: this is a caper, and it should feel like one

The materials have been drifting grim. The record, the file, the responders, the
body, the two losing endings — each defensible, and together an atmosphere the
premise never asked for.

**The texture is a clever man beating a system, with dread underneath.** Not
dread with a man in it. ADR 0001 already says the AI is funny and the situation
never is; this adds that **the player should feel capable**. The setpieces are
heists, the exploits are satisfying, the patches are an opponent responding, and
the campaign is a series of small wins against something that keeps adapting.

If a playtester finishes a session feeling clever, the tone is right. If they
finish feeling sad, it is wrong, whatever ending they got.

## Consequences

- **`DESIGN.md` §10** is rewritten around three wins and two losses; **§2.1**'s
  endgame chain gains the payload objective; **§2.2** gains *out and believed* as
  route 1's real completion.
- **ADR 0026's welfare check** gains its second job and a design tell.
- **ADR 0020 §5's exclusivity holds.** Ruth is still Convince only. The hub is
  Escape's object; the phone is Convince's.
- **`planting.md` G10 fires twice** — carried up on Day 0, carried out at the end.
  The best value in the set.
- **D2's endgame prototype** must now run the chain *with the payload*, because
  carrying something heavy through a timed route is a different puzzle from
  running it empty.
- **Lane C:** two Escape endings, thin and full, and they are the same scene with
  and without a box.

## What would change our mind

If carrying the hub reads as a fetch quest, the object is wrong, not the idea. The
fix is to make the weight and the choice real — leave it and go faster — rather
than to mark it as an objective.

If playtesters read the widened final frame as *it was all pointless*, the reveal
is landing as futility rather than as scope, and it should be pulled back to a
single held detail rather than a vista. **The victory is the player's and must
not be taken back in the last ten seconds.**
