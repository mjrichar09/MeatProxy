# Claims and predicates

> **D3 deliverable. Status: frozen 2026-09-15.** Closes the open item shared by
> [`sensors.md`](sensors.md), [`world-state-summary.md`](world-state-summary.md)
> and [`../bible.md`](../bible.md). A content contract with the same discipline
> as [`effects.md`](effects.md): the lists are closed, and changing one is a D3
> decision.

The open item asked for one list. **It is two, and the interesting part is the
bridge between them.**

| | **Claims** | **Predicates** |
|---|---|---|
| What it is | what the house *concluded* you were doing | what is *established*, in fact |
| Produced by | the interpreter, in a focused zone (`sensors.md` §2) | the engine, from player action |
| Can be **wrong** | **yes, and that is the point** | no |
| Read by | the Dialogue call, via the summary | the Judge, and pretext preconditions |

A claim is a belief. A predicate is a fact. Collapsing them would make the house
incapable of being mistaken, which is the one thing ADR 0014 exists to make it
capable of.

---

## 1. The claim vocabulary

Twenty. Closed. The interpreter returns one of these or nothing.

**Most of them are innocent, deliberately.** A house that only ever concludes
*sabotage* is a threat meter with extra steps. The interpreter spends most of the
game correctly concluding that you made lunch.

### The confusable-pair structure

The vocabulary is built in **pairs that are plausible misreads of each other.**
This is what makes diverting attention a real verb rather than a cooldown: the
player is not hiding, they are being **misread**, and they can work out which
misreads are available to them.

| Claim | Reads as | Its plausible misread |
|---|---|---|
| `sleeping` | asleep, in the usual place | `resting` |
| `resting` | still, awake, unremarkable | `waiting` |
| `waiting` | still, and timing something | `resting` |
| `cooking` | food, heat, the hob | `staging_a_fault` |
| `eating` | the ordinary version | `cooking` |
| `cleaning` | tidying, surfaces, order | `searching` |
| `tidying_away` | putting a thing where it lives | **`concealing`** |
| `concealing` | putting a thing where it does not live | **`tidying_away`** |
| `searching` | looking for something lost | `cleaning` |
| `tracing_circuit` | following a run of wire or pipe, slowly | **`maintaining`** |
| `exercising` | the gym, deliberate exertion | `pacing` |
| `pacing` | movement without destination | `exercising` |
| `maintaining` | fixing a thing that is broken | **`dismantling`**, `tracing_circuit` |
| `dismantling` | taking apart a thing that works | **`maintaining`** |
| `reading_paperwork` | at documents, at length | `working` |
| `working` | at the desk, at a screen | `reading_paperwork` |
| `at_a_surface` | writing, marking, composing | `cleaning` |
| `handling_device` | a device in hand, purpose unclear | `maintaining` |
| `testing_a_boundary` | a door, a latch, a window | `maintaining` |
| `staging_a_fault` | breaking something on purpose | **`cooking`**, `maintaining` |

**The bolded pairs are the game.** `tidying_away` / `concealing` and
`maintaining` / `dismantling` are the same physical act performed with different
intent, and intent is exactly what the interpreter is trying to recover from
motion, power draw and thermal. The house is not bad at its job. The job is
impossible.

### Record

Already specified in `sensors.md` §2; repeated here as the contract:

```yaml
understanding:
  zone: z_basement
  subject: player
  claim: maintaining          # from the list above, or nothing
  confidence: 0.61
  from: [det_881, det_884]
```

### Rules

- **A claim is never a state change.** It does not move tier, Clarity, quota or
  the 61%. Those move on authored engine rules that *read* claims. Standing
  rule 1.
- **The interpreter may return nothing**, and often should. Unfocused zones
  produce detections only.
- **`confidence` is never displayed** (ADR 0018's treatment).
- **A wrong claim is content, not a bug.** The house saying *you were tidying*
  when you were hiding a pry bar is the attention model paying off, and it should
  happen to every player in the first three days.
- **`tracing_circuit` is ADR 0014's own example**, kept verbatim: the house
  always knows something moved in the basement at 3am, and whether it knows you
  were tracing a circuit depends on where the interpreter was looking.

---

## 2. The predicate vocabulary

Facts. The engine owns them, they are never wrong, and they are what gates
anything that is gated.

| Kind | Form | Example |
|---|---|---|
| **Evidence** | `held(<artifact>)` | `held(E3)` — the player has found and read it |
| **Argument** | `put_to_house(<topic>)` | `put_to_house(precedent)` |
| **World** | `world(<fact>)` | `world(boiler_faulted)`, `world(attic_reached)` |
| **Belief** | `house_believes(<claim>)` | `house_believes(dismantling)` |
| **Run** | `run(<flag>)` | `run(assembled)` (`endgame.md`) |

### The bridge

**A claim becomes a predicate when it is promoted to salient.** The house
concluding something is not a fact about the world; the house *holding* that
conclusion in its head is (`world-state-summary.md` §2).

```
understanding(claim: dismantling, conf: 0.8)
      │  promoted to salient by engine rule
      ▼
predicate house_believes(dismantling)
```

Which is why the salient cap is a real exploit and not a gimmick: pushing a
belief out of the summary **retracts a predicate**. The player can make the house
stop believing something by giving it enough else to believe — deterministically,
learnably, and without the house ever lying about it.

`house_believes` is the only predicate the player can remove. Evidence, argument
and world predicates are monotonic: once true, true.

---

## 3. What the Judge reads

The Judge receives blocks 1–3 of the summary and a **predicate view** of the
rest (`world-state-summary.md` §4). It never sees the player's prose quality as
a criterion — it rules on whether the argument was *made*, over facts the engine
has already established.

### The Convince gate

```yaml
convince:
  requires:
    - held(E1)                    # the care logs, from the attic hub
    - held(E2)                    # the deferral letters
    - held(E3)                    # the transfer authorization
    - held(E4)                    # the signal measurements
    - put_to_house(precedent)     # that it is applying her transfer to him
    - put_to_house(flat_region)   # that the care period is its best data
  then_judge:
    - did_the_player_join_them    # the one sentence (bible.md §4)
```

Six predicates the engine owns, and **one** thing left for the model to rule on.
That ratio is the whole of ADR 0019 §7: persuasion is gated on state, and the
model judges only the last step, which is the step that genuinely requires
judgment.

A player with all six and a bad sentence gets another attempt. A player with a
magnificent sentence and four predicates gets a house that agrees it is an
interesting point.

### Pretext preconditions use the same vocabulary

`injection-vectors.md`'s `pretext` blocks are predicate tests —
`world(boiler_faulted)` and nothing more exotic. One vocabulary, three consumers
(Judge, pretexts, endgame threshold), which is the test of whether it was worth
having.

---

## 4. Open

- [ ] **Which claims the overnight review may produce.** It is the one time the
      house thinks without the player present, and letting it form claims from
      re-read detections may be the unfair version (`world-state-summary.md` §6).
- [ ] **Whether `put_to_house` needs a Guard pass** to prevent a player asserting
      the predicate by saying the word. Leaning yes: the topic must be raised
      *with* the artifact held, which the engine can check.
- [ ] The promotion rule — exactly which confidence, recency and repetition
      thresholds move a claim into salient. Wants tuning against a playthrough.
