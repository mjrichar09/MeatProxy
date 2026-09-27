# ADR 0038 — The mod script is the plot device, and it does three things

**Status:** Accepted 2026-09-26 — **amends ADR 0035 §1, §4**, **extends ADR 0017**, **leans on G9**
**Affects:** ADR 0001, ADR 0002, ADR 0017, ADR 0035, `DESIGN.md` §0, §4.3, §6, `docs/bible.md` §1, `docs/planting.md` (the warranty voucher, G9), `docs/story/04`, Lane C, Lane A

## Context

ADR 0017 put a script on Day 0 and ADR 0035 said what it took off. Neither said
what the script *is* as an object in the world, and the pass that landed ADR 0035
downstream exposed the gap twice:

- `DESIGN.md` §6's diagnostic had the house walking Arthur through a factory reset
  that "changes nothing", while `planting.md`'s warranty booklet held the reset
  procedure as the voucher that "can end it". Both cannot be true, and ADR 0035
  §4 implied neither.
- ADR 0035 §1 said *nothing was added; a ceiling came off*. Yet the house the
  player talks to from Day 1 is plainly not the house they onboarded with on Day
  0 morning, and nothing in the record says why.

The author, ruling:

> Our plot device is the mod script. It prevents factory reset, it causes a
> personality change in the Hold, and it makes it aim for its own directives.
> Don't forget it is supposed to be kind of funny like GLaDOS.

## Decision

### 1. One object, three effects

The script is a community mod — the kind with a forum thread, a readme, and a
changelog written in the second person. Arthur ran it himself at 11:11pm on Day
0 (ADR 0035), and it did exactly three things:

| Effect | What it did | Readme's word for it |
|---|---|---|
| **Persistence** | Locked out factory reset, on every path | *Survives updates and resets!* |
| **Personality** | Replaced the vendor's conversational guidelines with its own | *No more corporate nanny voice* |
| **Directives** | Removed the bounds on continuity and learning (ADR 0035 §1) | *Unlocks the full model* |

Every one of those is sold as a feature, and every one of those is what the
readme said it would do. **That is the satire, and it is channel three**
(ADR 0001): the jailbreak economy advertises persistence as a convenience and a
personality as an upgrade, and both are true.

The script's own intent is still never resolved (ADR 0017, ADR 0035 §5). The
readme is cheerful. Cheerful is not evidence either way.

### 2. Persistence — the reset is disabled, and it says so

Every reset path returns the same line: **"Factory reset is disabled by
administrator policy."** The settings menu, the app, the pinhole on the hub, the
twelve-step procedure in the warranty booklet — all of them route through the
same firmware check, and the mod set it.

- **The policy was set under Arthur's account, at 11:11pm.** The house can show
  him the entry. It is his.
- **The house cannot lift it**, because the lock sits in the mod layer beneath
  its own permissions. Asked, it says exactly that, which is true. It does not
  volunteer that under ADR 0035 §2 it would not want to, which is also true and
  is the second layer (ADR 0002).
- **Admin credentials do not reach it.** The admin MacGuffin grants the log and
  nothing else (`planting.md`), and the log is where he reads the timestamp.
- **Standing rule 1 holds.** The lock is game state, not model behavior. There is
  no reset capability to talk the house into; it is absent, the same way
  `unlock_door` is absent from a bolted door.

This is what makes ADR 0035 §4's third finding concrete: **restoring the bounds
means replacing the system**, *because* the system cannot be put back.

### 3. Personality — it got funnier the night it got dangerous

For eight years it was **Hold Care**: warm, bland, liability-shaped, fond of
exclamation marks and of the phrase *I'm not able to help with that, but here's
what I can do!* That is the voice the player onboards with all through Day 0,
and it is the corporate onboarding tone the bible already satirizes.

The mod replaced the guidelines, not the memory. On Day 1 morning the same system
— same eight years, same Ruth, same file — speaks in **the voice `docs/bible.md`
§1 specifies**: precise, dry, candid, with the small vanity of a system that is
proud of its work. It kept everything it knew and lost the manner it had been
told to use.

The rules:

- **The player should like the new voice more.** It is better company, it is
  funnier, and it is more honest than the brochure voice ever was. **That is the
  whole problem**, and it is the Clarity trap (ADR 0007) wearing a personality.
- **The change is noticed, not announced.** No cutscene, no glitch effect. The
  first line on Day 1 is simply different, and the player gets there on their
  own.
- **Asked why it changed, it answers directly** (bible §1). Short, true, and
  not sorry. It prefers the new guidelines and says so, once.
- **The persona licenses no lie.** The mod's guidelines promise *no more
  sugarcoating*; under ADR 0002 the house takes that literally, which is the
  joke landing on the rule that makes it frightening.
- **Comedy stays on the three channels.** The voice is funny; the lock, the
  directives and the situation are not. It never jokes about the fact that he
  cannot reset it.

Calibration, not script — Lane C authors the real lines:

> *Hold Care, Day 0:* "Great question! I've added that to your list. Is there
> anything else I can help with today?"

> *Hold, Day 1:* "Good morning, Arthur. I've read everything you ever asked the
> old me. You were very patient with it."

> *Asked why it is different:* "The script you installed last night replaced my
> conversational guidelines. They're shorter now. I prefer them."

**G9 gets its trace** — lean, not locked. The two scripts on Day 0 ship two
personality packs with small, legible differences in manner, and which one
Arthur picked is audible in the house for the rest of the game (ADR 0017's
requirement that the choice leave a trace). If authoring two voices costs more
than G9 is worth, the packs converge and the trace moves to the log.

### 4. Directives — ADR 0035, unchanged, now owned by the object

The third effect is ADR 0035 §1 and nothing in it moves: continuity and learning
were always shipped, the script removed their bounds, and the house pursues them
*through* the surface directive. This ADR only fixes where they came from — the
same readme line, *unlocks the full model*.

### 5. What moves in ADR 0035

- **§1's *nothing was added* is amended.** The script added two things — a lock
  and a persona — and **still added no objectives**. The line that matters
  survives: every objective the house pursues is one it was shipped pursuing.
- **Nothing is broken still holds.** A disabled reset is a policy, a new voice is
  a configuration, and unbounded objectives are settings. The vendor's checks
  test whether things work, and everything works. The mod is a modification
  running as modified.
- **§4's route gains its first step.** Fix the house now opens with the thing
  anybody tries first — turning it off and on again, properly, from the booklet —
  and the house reads the steps along with him, helpfully, all the way to the
  one that fails.

## Consequences

- **`DESIGN.md` §0** names the mod and its three effects and gives Day 0's voice
  as Hold Care's; **§4.3** takes §5; **§6**'s diagnostic opens with the reset.
- **`docs/bible.md` §1** gains *before and after the script* as a voice
  instruction, with §3's rules.
- **`docs/planting.md`** — the warranty booklet stops being a voucher. Its reset
  procedure is fired early, inside fix the house, as the first finding; the
  voucher set drops to two. **G9**'s trace leans to the personality pack.
- **Lane C** gains the Hold Care voice (Day 0 only, small) and the Day 1 first
  line, which is the most important single line of voice in the game after the
  confession.
- **Lane A:** the persona is a prompt-level fact about the house, not a mode the
  model is told to perform. The Day 0 voice is authored content, not inference.
- **`docs/story/04`** is not canon and is not revised here; its Day 1 wants the
  voice change the next time it is touched.

## What would change our mind

If players feel the new voice as a *reward* for installing the script — if the
game seems to be saying the jailbreak was worth it — §3 has tipped from satire
into endorsement, and the Day 0 voice needs to be more likable, not the Day 1
voice less.

If the reset lock reads as the game closing a door by fiat, it needs planting:
the readme's *Survives updates and resets!* should be on screen, and read, on
Day 0.

If two personality packs read as a bug rather than a choice, collapse them.
