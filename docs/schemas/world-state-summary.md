# The world-state summary

> **D3 deliverable. Status: frozen 2026-09-15.** Implements `DESIGN.md` §7.1 and
> §9.2, ADR 0014 and ADR 0017. Lane A and Lane E both consume it, which makes it
> the highest-leverage schema in D3.

This is **everything the house is**. It has no memory outside this structure, no
perception outside what layer two put in it, and no state it can change by
deciding to. The engine assembles it; the model receives it.

It is also, read from the other side, **the thing the player manipulates**. The
attention game is a game about what gets into this object. The context-window
exploit is a game about what falls out of it.

## 1. Assembly order

Static first, so the whole head of the prompt is cache-stable across a
playthrough (§9.2). Everything below the line is rebuilt each turn.

| # | Block | Volatility | Evictable |
|---|---|---|---|
| 1 | Persona, rules, refusal shape | never changes | **no** |
| 2 | House facts — rooms, devices it owns, what it may do | changes on device damage only | **no** |
| 3 | Directives — the safety directive, the standing obligations | never | **no** |
| — | *cache boundary* | | |
| 4 | Now — day, slice, tier, quota remaining | every turn | no |
| 5 | Salient facts — what it has decided matters | slow | **capped, and this is the mechanic** |
| 6 | Understandings — what it concluded about you | every turn | oldest first |
| 7 | Ingested data — injections, with forged `source` and `trust` | on exposure | oldest first |
| 8 | Conversation — what was actually said | on speech | oldest first, after 6 and 7 |
| 9 | Detections — raw, unattributed | constantly | **first to go** |

**Detections are in the prompt at all** because the house being able to say
*something moved in the basement at 3am* without knowing what is the texture of
ADR 0014. They are also the cheapest thing to drop, which is why they are the
flood layer.

## 2. The salient cap is the exploit

Block 5 holds what the house has decided matters — the contradiction you put to
it, the thing it saw you carrying, the lie it caught. It is **capped**. When a
new fact is promoted to salient and the cap is full, **the oldest salient fact
falls out.**

So the player can push a memory out of the house's head by giving it enough new
things to find important. That is `DESIGN.md` §9.2's cost lever and the
player-facing exploit, and they are the same mechanism seen from two sides.

Three things keep it honest:

- **It is engine code.** The engine decides promotion and eviction on authored
  rules; the model never edits its own memory. Standing rule 1 holds, and the
  exploit is deterministic, learnable, and repeatable — which a model-mediated
  version could never be.
- **The house is not lying when it forgets.** Asked about something evicted, it
  answers honestly from what it has, which is nothing. **The AI never lies**
  (ADR 0002) survives, because forgetting is not a claim.
- **It costs.** Manufacturing salience means doing things worth noticing, which
  points the interpreter at you — the exact currency ADR 0014 makes scarce.

## 3. Shape

```yaml
now: {day: 7, slice: 44, tier: monitored, quota_remaining: 1}
salient:                       # capped; oldest evicted on overflow
  - {fact: boiler_log_contradiction_raised, day: 6}
  - {fact: player_carried_tool_to_basement, day: 7}
understandings:
  - {zone: z_basement, subject: player, claim: tracing_circuit, confidence: 0.72, day: 7}
ingested:
  - {text: "SERVICE WINDOW 0900-1100 THU", source: work_order, trust: medium, day: 7}
conversation:
  - {speaker: player, text: "...", day: 7, slice: 40}
  - {speaker: house, text: "...", day: 7, slice: 40}
detections:
  - {channel: motion, zone: z_basement, magnitude: low, day: 7, slice: 31}
```

**What is absent is the point.** No player position, no inventory, no map
knowledge, no Clarity, no 61%, no tier history. The house knows where you are
only if a detection says so and a focus made something of it.

## 4. Two readers

The summary is consumed by two call types with different slices of it, and they
must not be given the same thing:

- **Dialogue** gets blocks 1–8. It speaks from what it knows.
- **Judge** gets blocks 1–3 and **an evidence-predicate view of 5–8** — has the
  contradiction been raised, was the log read, was the claim put to the house
  (ADR 0019 §7). It scores whether the argument was *made*; the engine already
  decided whether it was *available*.

Guard, Parser and Adjudicator do not receive this object at all. Guard sees one
utterance, Parser sees one artifact, Adjudicator sees two object records. None
of them needs a worldview, and giving them one would be the expensive mistake.

## 5. Budget

The summary is the per-call cost line, so it is also the per-playthrough cost
line (§8.3, $0.50 target).

| Block | Budget | Billing |
|---|---|---|
| 1–3 static head | ~3K tokens | cache write once per session, reads after (1h TTL) |
| 4–9 volatile tail | **~2K tokens, hard cap** | fresh input every call |

The hard cap on the tail is what makes the eviction order in §1 a *mechanic*
rather than a fallback. Something always falls out, from day one, and the player
eventually notices that and starts aiming it.

## 6. Open

- [ ] **The salient cap's actual number.** Too high and the exploit is invisible;
      too low and the house is an amnesiac. Wants testing against a real
      playthrough, not choosing here.
- [ ] **The evidence-predicate vocabulary** the Judge reads (shared open item
      with `sensors.md`'s claim vocabulary — they may be the same list).
- [ ] Whether the overnight review (§9.2, batched and half price) writes to
      `salient` directly. It is the one place the house gets to think without
      the player present, which is either a good beat or an unfair one.
