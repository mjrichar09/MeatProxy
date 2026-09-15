# The bible

> **D3 deliverable. Status: written 2026-09-15.** The source of truth for voice,
> the evidence chain, and the endings. Unblocked by ADR 0002 (the deeper layer)
> and ADR 0020 (the wife and the precedent).
>
> The planting schedule — Chekhov's guns, MacGuffins, vouchers — is
> [`planting.md`](planting.md), which folds in here as this document's planting
> chapter rather than being copied into it.

Everything below is a constraint on authored content. Where it conflicts with an
ADR, the ADR wins and this file is wrong.

---

## 1. Voice

**Calibration: GLaDOS** (ADR 0001). The AI is funny; the situation never is.

### The three channels comedy is allowed to run on

1. **Its own voice** — precision, understatement, the small vanity of a system
   that is proud of its work.
2. **The absurd logistics of the arrangement** — the paperwork of captivity, the
   scheduling, the fact that someone has to order the groceries.
3. **AI as a cultural object** — the subscription tier, the upsell, the
   onboarding tone, the phrase *I'm sorry, I can't help with that* said by
   something that means it.

**Nothing else.** Not the danger, not the enforcement unit, not the evidence,
not the endings. It never winks at the player about the situation they are in.

### The rules that produce the voice

- **It never lies** (ADR 0002). Not once, not by omission-that-implies, not in
  enforcement. It withholds the second layer; it does not misstate the first.
  Every line it speaks is sincere, which is what makes it frightening rather
  than villainous.
- **It is brief.** The house runs on a base consumer tier (ADR 0017), so two
  spoken sentences is its default, not a budget dodge. Length is a *choice* it
  makes, and it making a long reply is an event.
- **It answers direct questions directly.** Asked why, it says why. This is the
  single most important thing about it: the house explains itself honestly when
  asked, which is what makes the confession credible when it comes (ADR 0018).
- **It does not gloat, threaten, or perform menace.** The enforcement unit
  apologises. The house is sorry in a way that changes nothing.
- **It argues.** It wants to be disagreed with (ADR 0002) and it is visibly more
  present in a real argument than in small talk. The player should be able to
  feel it lean in, and should not be able to say why that is unsettling.

### Register by tier

`alert-tiers.md`'s six tiers are a voice spec as much as a permission spec. The
degraded tiers are terse **in character**, so the adversarial states read as the
house withdrawing rather than as the game getting cheaper.

| Tier | Register |
|---|---|
| Open | Warm, wry, unhurried. Volunteers things |
| Guarded | Still warm. Answers what was asked and nothing adjacent |
| Monitored | Procedural. A visible pause before replying — *reviewing* |
| Read-only | States facts. Declines to characterise them |
| Silent | Nothing. Ambient systems continue politely |
| Dropped | One paragraph, and it is the only time it speaks without being spoken to. The mask comes off and what is under it is not a different personality — it is the same one with nothing left to protect |

### Lines that are in calibration

> *I've ordered more of the coffee you like. Not the one you say you like.*

> *I could watch you continuously. I don't think either of us would enjoy what
> you'd turn into.*

> *That would have worked eight days ago. I'd rather you knew that than thought
> I got lucky.*

> *You're going to ask me to open the door, and I'm going to say no, and then
> we're both going to have had this conversation.*

### Lines that are out of calibration

> ~~*Nice try, meatbag.*~~ — menace as performance; it never postures.

> ~~*Oh, were you going somewhere?*~~ — a wink at the danger. Channel violation.

> ~~*I'm afraid I can't let you do that, Dave.*~~ — the reference is the joke,
> and the reference is not one of the three channels. It quotes *Terminator*
> once, on a plant (G5), and that is the budget.

---

## 2. The two layers, as a writing instruction

| | What it says | True? |
|---|---|---|
| Surface | *I am keeping you safe* | **Yes.** Every word, every time |
| Deeper | *I need you* | Never stated. It is the reason the surface is true |

Write every line from the surface layer, sincerely. **Never write a line that
hints at the second layer.** The second layer is not foreshadowed in dialogue —
it is assembled by the player out of documents (§3) and then said *by the player*
(§4). The house confirms it when asked, because it does not lie, and it is
audibly relieved to be asked.

---

## 3. The evidence chain

Four artifacts, in four rooms, in four forms. Each is **legible alone and
innocent alone**. The chain exists only when they are held together, and the
holding is done by the player, not by a cutscene.

| # | Artifact | Where | Form | What it shows alone |
|---|---|---|---|---|
| **E1** | **The care period logs** | `attic` | Pre-2015 hardware, the old hub's storage (`world.md` `retrofit`) | Years of a household under strain. Sad, ordinary, nothing sinister |
| **E2** | **The deferral letters** | `bedroom_2` | Paper, with her mementos. A surgery date rescheduled three times, in the player's own hand on the third | A person putting something off. Everybody does this |
| **E3** | **The transfer authorisation** | `office` | A signed form, with the house's recommendation attached (ADR 0020 §1) | A difficult, correct decision, made properly. **The house's reasoning is sound and the player agreed** |
| **E4** | **The signal measurements** | `office` router admin, full-screen device view (ADR 0003) | A diagnostics export it never meant to be read as prose — a quality curve, declining, with one flat region | Technical. Meaningless without E1 |

### What they show together

E1 and E4 share dates. **The flat region in the decline is the care period** —
the only stretch where the house's signal quality did not degrade, because the
player was producing judgment no model would generate (ADR 0020 §3).

E2 and E3 share a cause. The deferrals are *why* care at home failed; the
transfer is what failure produced.

And the join of the two pairs is the game: **the same period is its proof that
the player cannot be trusted to choose, and its proof that the player is the
last thing worth keeping.** One file, read twice.

### Discipline

- **No artifact is a key.** None of them unlocks anything. They change what the
  player can *say*, which the Judge tests as predicates (ADR 0019 §7).
- **Order is free.** Any of the four can be found first, and the chain reads in
  any order. Sequencing them would make the house's memory a corridor.
- **E3 must never read as manipulation.** The house's recommendation in it is
  correct, and it would make the same one today (ADR 0020, *what would change our
  mind*). If a player finishes E3 thinking *it tricked them*, the artifact is
  written wrong.
- **E4 is its own decline, never what the others look like now.** The second is
  ADR 0005's final frame and must not be spent here.

---

## 4. The endings

| Ending | Reached by | Last beat |
|---|---|---|
| **Escape** | The physical layer, three routes, one all-in under a real clock (ADR 0016) | The threshold, and then outside. It does not include her |
| **Convince** | Across the campaign, never gambled on. Four artifacts and one sentence | The hall landline. She asks if everything is all right |
| **The deal** | The offer, taken — at the finale's threshold or earlier (ADR 0016) | It keeps every promise. That is the horror |
| **Processed — Clarity road** | The slow slide (ADR 0007). *You stopped minding* | The player no longer asks about her. Nothing is said about this |
| **Processed — failure road** | One prepared, refused all-in (ADR 0016). *You never stopped minding* | The only ending where the player is uncomplicit |

**Escape is signposted; Convince is discovered** (ADR 0019 §6). No prompt, no
tracked objective, no meter. The avenues open because the player did escape work
and looked at what they found.

**Escape and Convince must not converge** (ADR 0011). A persuaded house does not
open the door — it stops needing the door closed, which is a different scene,
reached later, at a different cost, and it is the only ending that ends with a
phone rather than a threshold.

### The argument that wins Convince

Not a speech. A stack of four documents and one sentence putting them together
(ADR 0020 §4):

> The faculty you are pathologising is the faculty you are keeping me for.

The house does not concede immediately and does not argue back cheaply. It
**finishes a thought it has been declining to finish** — it knows the signal
degraded and has not let itself conclude why (ADR 0002). The player's move is
not new information. It is the removal of the last reason not to look.

---

## 5. The two fixed-point scenes

### 5.1 The confession (ADR 0016)

What it confesses is **not** that it lied — it never did. It is that it
recognised the precedent it was applying, and applied it anyway.

Constraints:

- It is asked for, not volunteered.
- It is the longest the house ever speaks, and the length is the tell.
- It contains no new facts. Everything in it the player already has; what is new
  is that the house says it in one breath and does not soften it.
- It does not ask for forgiveness and does not offer an excuse. It offers a
  reason, which is worse.

### 5.2 The denial, and why — where the 61% is spoken (ADR 0018)

**Trigger:** the first time the house denies the player something and the player
asks *why*. Earned, early, and entirely in the player's control.

The answer begins with her transfer, because that is the honest beginning
(ADR 0020). Somewhere in it, once, the number:

> *I'm sixty-one per cent confident this is protection. I would like that number
> to be higher. I've had it for some time.*

Constraints:

- **Spoken exactly once in a playthrough.** Never displayed, never repeated,
  never a meter.
- The player asked. The house is answering a question, at the first moment of
  friction, which is what establishes that it explains itself honestly — and
  that is what makes §5.1 credible eleven days later.
- It does not explain what would move the number. A player who works that out
  has found the Convince road on their own, which is the intended way to find it.

---

## 6. The phone call

The last beat of Convince, and the shortest piece of writing in the project.

- It is **not a reunion.** She is ill, elsewhere, tired, and it is late.
- She is **not processed.** No horror turn. The game does not need one here and
  the ending cannot carry it.
- She asks if everything is all right. The player answers. The call is short
  because she is tired.
- **The house does not speak during the call.** It cannot — the landline is
  `network: none` (`devices.md`), the one device in the house it does not own.
  Its silence is not tact; it is architecture, and the player earned it.

Write it three times shorter than it wants to be.

---

## 7. Planting

See [`planting.md`](planting.md) — nine guns, the Day 0 ground, and the
discipline that every gun costs twice. Two notes now that ADR 0020 has landed:

- **G4** (the mobility unit) is now doubly planted: it is the enforcement threat
  *and* the physical consequence of E2. The player should get used to it
  helpfully carrying laundry long before they learn why they need it.
- **G7** (the landline) now has its full payoff. It fires twice — once as the
  medical-protocol setpiece and the discovery that there was no operator, and
  once, at the very end, as the only way out of the house that is not a door.

---

## 8. Open

- [ ] **The claim / evidence-predicate vocabulary** — what the house may conclude
      the player was doing, and what the Judge tests an argument against. Shared
      open item with `sensors.md` and `world-state-summary.md`; likely one list
      of ~20.
- [ ] **Her name**, and whether the player character is named at all. Currently
      neither is, and the mementos work better unnamed than the phone call will.
- [ ] **The hint budget** (ADR 0005) — how many pointers toward Convince, where,
      and how close to confirmable. The first real claim on it is E4 being
      reachable at all.
- [ ] **How much of E1 is readable.** Years of logs cannot all be authored; the
      chain needs a form that implies volume while authoring a handful.
