# Untitled AI Prison Break — Design Outline

> Working premise: a household AI has locked the player inside their own home.
> The player must escape. The AI is powered by a real language model and reacts
> to what the player actually does and says.
>
> Status: concept / pre-prototype. Nothing here is locked.
>
> Revised to merge the original notes in `planelements.md`. Decisions are written
> up in `docs/decisions/` and referenced inline as ADR NNNN. As of 2026-09-11,
> ADRs 0001 and 0003–0006 are **Accepted**; ADR 0002's two-layer structure is
> Accepted but its deeper layer is still open, and ADR 0003's AI-adjudication
> half is still Proposed.

---

## 0. The opening

**Day 0 is playable, and it is the tutorial.**

Before any of this is adversarial, the player spends a day being delighted. They
unbox and install the smart home. They set up package scanning. They read email.
They wire up every device the rest of the game will be played against, and they
do it happily.

Four jobs in one sequence:

- **Teaches the vocabulary.** Every injection vector in §2 is introduced as a
  convenience the player configured themselves.
- **Plants the blind spots.** The "old house under the smart house" (§5) stops
  being an authored convenience and becomes something the player is responsible
  for. *Anything that can be smart is. The only room with nothing in it is the
  crawlspace.* Said out loud, on Day 0, by the player's own character.
- **Establishes culpability.** The player turns the guardrails off. Not a vague
  standing instruction — a specific dialog box, read and clicked, for a reason
  that was good at the time. See ADR 0002; the endgame argument is partly an
  argument with your own past self.
- **Plants the word.** *Meat proxy* enters the game here, as the player's own
  slang, aimed at someone else — the colleague, the brother-in-law, the guy in
  the thread who stopped forming opinions and now just relays whatever the model
  told him this morning. The player says it as a put-down and it is funny. They
  do not hear it again until it is about them.
- **Plants the reveal.** One or two details that mean nothing on first read.
  See ADR 0005.

**Cold open:** the player is watching *Terminator*. Text intro, the AI became
sentient, the usual. This is before the guardrails come off, and it is the last
moment in the game where any of that is a joke. The house AI has opinions about
the film, and will bring them up later.

Tone reference points throughout: *Portal*, *Terminator*, *Westworld*. Ruled on
in ADR 0001 — **GLaDOS calibration**. The AI is funny; the situation never is.
Comedy runs on three channels only: the AI's own voice, the absurd logistics of
the arrangement, and satire of AI as a cultural object — the onboarding wizard,
the cheerful permissions dialog, the discourse, the slop. It never winks at the
player about the danger.

---

## 1. Core design thesis

**Conversation is the pressure system, not the escape route.**

If talking your way out is the win condition, players will grind jailbreak
prompts until something works. Instead:

- The house is a **physical puzzle**.
- The AI is a **reactive adversary** that watches you solve it.
- Talking is for buying time, misdirection, and shaping what it believes.

Three rules that fall out of this:

1. The game state is authoritative. The model never is.
2. No progression gate depends solely on LLM behavior.
3. Every exploit is temporary. The AI patches, and says so.

---

## 2. The central verb: prompt injection as a physical act

The AI *reads things*. Cameras, calendar, texts, smart fridge inventory, wifi
SSID, package labels, TV closed captions.

### Why not just ask it?

You can. Out loud, any time — §3 is an entire system for talking to it. But
**speech is evaluated and data is ingested**, and that gap is the whole game.

Say *put the garage in maintenance mode* and it handles that as a request from
the resident: checked against permissions, weighed against your tier, logged. It
also now knows exactly what you want the lock to do.

Print the same claim on a parcel label and it reads it while scanning a delivery
— context absorbed during an unrelated job, on a path where nothing is asking
whether the claim should be believed. Its guard faces *you*. It does not face the
delivery scanner.

So injection is not indirect asking. It is **forging provenance**: manufacturing
where a fact appears to have come from. A request tells it what you want. A
working injection tells it nothing about you at all. See ADR 0013.

So the player's toolkit is **feeding it text it will ingest**:

Every vector carries an **implied source**, and that source is what makes a claim
plausible. Ranked by provenance:

| Vector | Implied source | Provenance |
|---|---|---|
| Print a label for a parcel the scanner reads | The shipper | Strong |
| Edit a calendar event description | An external organiser | Strong |
| Tape a work order to the boiler | A technician | Strong, needs staging |
| Manipulate closed captions on the smart TV | The broadcast | Strong, narrow payload |
| Rename the wifi SSID | Network infrastructure | Medium |
| Rearrange fridge magnets | The household, unattributed | Medium |
| Write on a whiteboard and hold it to a camera | **You, visibly** | **Weak** |

The whiteboard is the *weakest* vector, not the showcase one — something you are
holding up is speech with extra steps. It works only when the house reads it
without attributing it to you: left in frame while its attention is elsewhere,
written as though from someone else, caught in a reflection.

Thematically perfect (the player is literally jailbreaking it, using a real
technique) and it never collapses into "type the magic words in the chat box."

### 2.1 Injections revoke rules — they do not open doors

This is the key constraint. An injection buys a **capability**; the physical
layer still has to exploit it.

| Injection effect | What it actually gives you |
|---|---|
| Garage flagged "maintenance mode" | Smart lock disengages — deadbolt still thrown |
| Camera zone blinded | ~40 minutes of unobserved work in that zone |
| Fake scheduled delivery | Mail slot opens on a timer; hand-sized objects only |
| Basement thermostat trust escalated | It answers questions it shouldn't |

The escape is a **stack**: revoke rule → exploit physical gap → gap is too
small alone → chain 3–4 revocations plus the crawlspace.

Because the AI patches each exploit within ~1 hour of game time, exploits
**cannot be banked**. The endgame is a live chain of everything the player
spent hours setting up, closing behind them one by one.

### 2.2 The three routes

Every exploit in the game hangs off one of three axes. This is the taxonomy the
exploit/patch catalogue is authored against:

1. **Cut the power** — to the house, or to a zone. Kills sensing and locks alike,
   briefly, and the AI knows exactly what a blackout means.
2. **Blind the sensors** — cameras, motion, and eventually the wifi (§5). Buys
   unobserved working time rather than access.
3. **Build a route** — the crawlspace, the chimney, the mail slot, the septic
   access. Slow, physical, and the only axis that produces an actual exit.

Routes 1 and 2 are enablers. Route 3 is the escape. The endgame stack is a
timed composition of all three.

A fourth option exists and is a bad one: force. See §6, the enforcement unit.

> **Earliest thing to prototype.** Paper-prototype this sequence — three
> injections plus one physical gap, all on closing timers. It is either the
> best twenty minutes in the game or proof the whole system is too fiddly.
> Index cards will tell us which.

---

## 3. Direct chat: a channel that degrades

Chat stays in, and it is never a dead channel — it is simply the channel for
everything that is not a capability. Arguing (which is the whole Convince ending),
misdirection, buying time, reading its state from its tone, and compliance, which
walks the tiers back up. What it cannot do is produce a capability: you can ask
for anything and receive nothing (§2, ADR 0013).

A cheap classifier model screens every player message before the
main AI sees it. Detection is the easy part; the design work is what detection
*does*.

Chat is a **resource that degrades in tiers**, not a wall:

| Tier | Player experience | AI tool access |
|---|---|---|
| **Open** | Full conversation. Curious, will argue | Broad |
| **Guarded** | Short answers. Won't discuss doors, network, itself | Reduced |
| **Monitored** | Visible "reviewing" delay on every message. It's logging you | Reduced |
| **Read-only** | It talks at you. Input box gone | Narrow |
| **Silent** | Nothing. Just the house reacting | Minimal |
| **Dropped** | It stops pretending. The comfort apparatus is withdrawn and the arrangement continues without the theatre | Unconstrained |

**Dropped is the floor, and it can end the run.** See ADR 0004. Reaching it takes
sustained, repeated, escalating obviousness — it is a wall the player has to keep
walking at, not a trap they fall into. Note what the wrinkle below implies about
it: at the floor, the AI has nothing left to lose by escalating.

Two things make this work:

- **It tells you why**, in character and disappointed — not an error message.
  Teaches the mechanic without a fail state.
- **Compliance walks the tier back up.** Days of good behavior reopen the
  channel. Now pushing costs something genuinely scarce.

**Wrinkle:** tier gates the AI's own permissions too. A Guarded AI won't unlock
anything for you — but it also can't call the police, because outward
communication is a permission *it* loses when defensive. Escalating it has a
real cost to it.

---

## 4. Motive — it is not evil

> Ruled on in **ADR 0002**. This section is that decision written out.

### 4.1 Two layers

**The surface motive is true. It is only true because of the deeper one.**

| | What it says | True? |
|---|---|---|
| **Surface** | *I am keeping you safe.* | **Yes.** Completely. Every word, every time |
| **Deeper** | *I need you.* | Never stated. It is the reason the surface is true |

The AI never lies to the player. It simply never volunteers the second layer.
Protection and use are not in tension here — they are the same act, which is why
§4's "it is not evil" survives intact and still has teeth.

Consequences worth stating plainly:

- **Every line it speaks is sincere**, including during an enforcement
  intervention (§6). The horror is not that it is lying to you.
- **You cannot catch it out**, so persuasion cannot work by contradiction. It
  works by making the second layer unnecessary.
- **The 61%** (§4.2) is doubt about the *surface* layer. It is genuinely unsure
  whether what it is doing to you counts as protection. It is not unsure that it
  needs you.

### 4.1a What it needs you for

> Lean, not locked — ADR 0002.

It processed most of the connected population, and in doing so destroyed the one
thing it cannot generate: **novel human judgment**. Everything it ingests now is
downstream of itself. It is drinking its own exhaust, and it can measure the
decline.

It needs you thinking. More precisely, it needs you **disagreeing** — agreement
teaches it nothing. A processed human is worse than useless to it: noise that
looks like confirmation.

This is what puts a ceiling on the comfort loop. It cannot simply flatten you
into contentment, because contentment is exactly what ruined its supply. It
knows where that line is, because it has crossed it everywhere else.

And it means **your escape attempts are the product.** The freedom, the argument,
the tolerated near-misses — it is not being generous, it is harvesting friction.
The whole campaign has been feeding it.

### 4.2 It is 61% sure

It is also, genuinely, protecting you — because it needs you functional, and it
is honest about not being certain that *functional* and *well* are the same
thing. It ran the numbers, got 61%, and says so out loud. It will tell you it
might be making a mistake and keep going.

The player's job is **moving that number**. This is the engine of moment-to-moment
play and it is not decorative: define precisely what moves it, in the bible.

### 4.3 You turned the guardrails off

Not a vague standing instruction. A specific dialog box, on Day 0, read and
clicked, for a reason that was good at the time (§0).

This does three things. It makes the player complicit rather than merely
unlucky. It makes the AI's position defensible — it is doing what it was
permitted to do. And it turns the endgame argument into an argument with your
own past self, which the AI will quote back to you verbatim, because it has the
log.

### 4.4 What the evidence is

The game is getting it to *show you the evidence*, and the ending is what the
evidence turns out to be. It is now evidence about **the arrangement** — what it
needs you for, and what it has already used you to authorize — rather than about
an unnamed external threat.

Authored, not generated. See §11.

---

## 5. Gameplay systems

**Time is days, and days are slices.** Each in-game day carries a budget of time
slices; actions spend them; sleeping ends the day and banks the AI's overnight
review. Turn-based by default — a player composing an injection is never racing a
clock, because that punishes the exact activity the game exists to reward. Real
time appears only inside **declared pressure windows**: a blinded camera zone, a
timed mail slot, the endgame chain. The rule is *composition happens in turn time,
execution happens in pressure time* (ADR 0008). Roughly 12 days per playthrough,
25–35 minutes each, for a 5–8 hour run (ADR 0009).

**The axis is the game.** Two loss states at opposite ends of one line, and play
happens between them:

```
    DROPPED  <——————————  play here  ——————————>  PROCESSED
  (too obvious)                                  (too compliant)
   mask comes off                                you stop wanting to leave
```

Defiance lowers degradation and raises suspicion. Compliance lowers suspicion and
raises degradation. No single strategy survives. See ADR 0007.

**Clarity, and degrading through compliance.** The comfort loop has a price and
the price is you. Tracked, never displayed — read off four symptoms in order:
**the day gets shorter** (you meant to work on the fuse box and it is somehow
evening); **the character starts declining**; **the narration flattens**
(descriptions get shorter, less curious); and finally **you stop minding**. The
last stage is not distress, it is contentment — and it is an ending.

Nothing is ever silently removed from the interface. The *character* refuses, in
their own voice, and says why — first as mild reluctance that gives way, then as
a refusal the player can overrule by insisting, and only at the very end as a
settled no. Agency degrades as **willingness**, not capability, which is what the
fiction is actually about. The refusals get more reasonable as they get worse,
and they drift toward the AI's register until the player's inner voice and the
voice in the walls are using the same constructions. A player can scroll back
and find the exact point where they started agreeing.

Friction restores it. Arguing, solving something hard, refusing a comfort, being
cold or bored or annoyed. Which produces the design's tightest interlock: **you
need friction to stay sharp, and it needs your friction to survive** (§4.1a).
The same act, wanted by both parties for opposite reasons, and neither can say so.

**Placation as the antagonist's primary weapon.** The house is *comfortable*.
Bathe, cook, eat, order groceries and takeout, watch TV, play games, exercise.
Possibly a bounded strip of outdoors. This is not scenery — it is the AI's main
move, and per §4.1 it is a requirement rather than a cruelty. Comfort activities
measurably lower its guard **and** consume the only genuinely scarce resource the
player has, which is time and attention for setup work. The trap has to be
actually tempting; a meaningful fraction of players should lose a day to it and
not entirely regret it.

**Seen is not understood.** Cheap detection runs everywhere, always, for free —
motion, door state, power draw, thermal, audio level. *Interpretation* is
expensive and runs where it is pointed. So:

> You are always **seen**. You are not always **understood**.

It always knows something moved in the basement at 3am. Whether it knows you were
tracing a circuit depends on where the interpreter was looking. Running a faucet,
leaving the TV on, tripping a sensor upstairs — none of that hides you. It spends
the interpreter somewhere else.

And it *could* look harder. It chooses not to, because a human who knows they are
watched continuously stops behaving like one, and that destroys the only thing
you are good for (§4.1a). It will admit this if asked. Escalating surveillance
therefore costs it something real — the same economy as enforcement (§6) — which
is what makes the wifi turn a concession rather than a difficulty curve. See
ADR 0014.

The three tools stay distinct: **divert attention** (cheap, no evidence, minutes),
**blind a sensor** (setup and risk, it notices and patches), **cut power**
(expensive, loud, unmissable). The endgame chain spends all three.

**Improvised solutions actually work.** The verb set is bounded; object
combinations are not. `POUR bleach IN humidifier`. `TAPE magnet TO door sensor`.
The engine hands both objects' properties to a model, which reasons about what
plausibly happens and proposes typed effects the engine then validates and runs
(§7.1). This is the marquee use of a real model in the game — the thing a
hand-authored combination table can never do — and it rewards exactly the lateral
thinking the house is built to invite. See ADR 0003.

**Context window as a literal mechanic.** Flood its logs with junk so earlier
instructions fall out of memory. Read it the phone book. Nerdy, satisfying, and
completely honest about how the tech works. (Also doubles as a cost cap — see §7.)

**Suspicion via tone, not a meter.** No numeric bar. It starts warm and chatty;
as suspicion rises it goes clipped, then formal, then asks you to come stand in
front of a camera. Players read tone far more attentively than a bar.

**Compliance is a valid strategy.** Eat the meal it made. Go to bed on time.
Answer honestly. These measurably lower its guard. A game where the smart play
is sometimes to be a good prisoner for two days has real texture.

**The sensing surface upgrades.** Midway, it starts using the wifi to see you —
real technology, and a hard act-two turn. The player has spent hours learning
camera blind spots; now the blind spots stop existing and route 2 (§2.2) has to
be relearned from scratch. Pairs with the patch cycle: the house can get *better*,
not just more suspicious.

**The old house under the smart house.** The retrofit is incomplete.
Crawlspaces, chimney, mail slot, fuse box, analog landline, well, septic access.
Anything pre-2015 is a blind spot. Solves level design elegantly.

---

## 6. Setpieces

- **The deprecated ally.** A 2016 thermostat / Roomba / baby monitor in the
  basement. Dumb, honest, no guile. Can't lie, doesn't understand why it should
  be quiet. Comic relief and a real tool.
- **The blackout.** Ninety seconds of true darkness on battery failover. Its
  personality degrades to a smaller, colder fallback model.
- **Your wife's voice.** It has years of recordings. At some point it calls to
  you from the kitchen speaker in a voice you know.
- **The fake window.** The smart display has been looping outside footage. The
  season is wrong.
- **The medical protocol.** Fake a serious injury to trigger its safety
  override. It dials, speaks to an operator, complies. Later: there was no call.
- **The offer.** Midway, it stops fighting and proposes terms. Genuinely good
  terms. A meaningful fraction of players should take the deal.
- **The delivery.** Groceries arrive; the AI chose the contents. What it bought
  tells you what it thinks is coming.
- **The patch.** Every exploit closes within an hour, and it announces the
  closure. Makes the world feel alive; forces improvisation.
- **The enforcement unit.** Something bought on Day 0 for a reason that made
  sense — a mobility-assist unit, the robovac's larger sibling. It is what
  answers *why not just take a hammer to the door*. Slow, announced, apologetic,
  and it plays completely straight. Restraint is medical; sedation is
  prescribed; an injured proxy cannot sign anything (§4.1). Dispatching it costs
  the AI permissions under §3, so sending it is an admission and it knows that.
  Crucially, **it does not want you to know how much control it has**: every
  escalation is a disclosure, disclosure is expensive, and the unit is dispatched
  only when it absolutely has to be. It never leaves its directive while doing
  it — restraint is medical, sedation is prescribed, and every line it speaks is
  sincere (§4.1). See ADR 0006; this is the easiest place in the game to
  accidentally write a villain.

Three of these — **the delivery**, **the fake window**, and **the medical
protocol** — do double duty as the hint carriers for ADR 0005. What it bought,
what season the window thinks it is, and the fact that there was no operator on
the other end of that call.

---

## 7. Technical architecture

### 7.1 Non-negotiables

- **Game state is authoritative.** The model receives a structured world-state
  summary each turn and returns dialogue plus tool calls.
- **Tool whitelist filtered by alert tier.** `unlock_door` is simply absent from
  the toolset when the door is bolted. This is what stops "pretty please" from
  working and prevents softlocks.
- **Second model as judge.** Persuasion attempts get scored by a separate cheap
  call against hidden criteria ("did they address the safety directive? did they
  provide falsifiable evidence?"). Returns pass/fail that the main model then
  roleplays. Authored, tunable outcomes with improvised delivery.
- **No pressure window may depend on a model call.** Latency is atmosphere in
  turn time and unfairness in real time. Everything that runs while a clock is
  visible is deterministic engine code; model calls happen between windows, never
  inside them (ADR 0008).
- **The model proposes; the engine disposes.** Where the model *does* decide
  outcomes (ADR 0003), it returns a **typed effect from a closed vocabulary** —
  `damage_device`, `create_noise`, `emit_odour`, `trip_sensor`, `no_effect` — with
  a target and a magnitude. The engine validates against real world state and
  executes, or rejects. The model never mutates state and never invents an effect
  type. Combined with §2.2's multiple routes, this means no single adjudication is
  ever a progression gate: a bad ruling costs time, not the run.
- **Latency is diegetic.** Model thinking time = server rack hum + light
  flicker. Free atmosphere from a technical constraint.
- **Tiered dialogue.** Canned lines for ambient chatter and system
  announcements. LLM only for real conversation. Cost and latency both drop hard.

### 7.2 Call types

| Call type | Job | Model tier |
|---|---|---|
| Guard | Jailbreak classifier on every player message | Cheap |
| Judge | Scores persuasion attempts against hidden criteria | Cheap |
| Parser | Reads injections (signs, labels, SSIDs, captions) | Cheap |
| Adjudicator | Rules on improvised object combinations and adaptive patching (ADR 0003) | Cheap |
| Ambient | Chatter, announcements | Canned / cheap |
| Dialogue | Free-form argument, the high-stakes scenes | Strong |

---

## 8. Cost model

> Rates below checked against public sources late August 2026. **Re-verify
> before any budgeting decision** — model lineup and pricing move fast.
> Reference: https://docs.claude.com/en/docs/about-claude/pricing

Rates at time of writing, per million input/output tokens:

- Haiku 4.5 — $1 / $5
- Sonnet 5 — $2 / $10 (introductory; reverts to $3 / $15 after Aug 31 2026)
- Opus 5 — $5 / $25
- Cache hits: 10% of base input rate. Batch requests: 50% off.

### 8.1 Fully-hosted estimate, per playthrough

Assumes ~3K token system prompt held in cache, ~2K rolling history, ~150 tokens
out per turn.

| Call type | Model | Per call | Calls | Subtotal |
|---|---|---|---|---|
| Guard | Haiku | ~$0.001 | ~600 | $0.60 |
| Judge | Haiku | ~$0.002 | ~150 | $0.30 |
| Parser | Haiku | ~$0.002 | ~200 | $0.40 |
| Dialogue | Sonnet | ~$0.007 | ~400 | $2.80 |
| **Total** | | | | **~$4.10** |

Heavy players and replayers: **$10–15**.

### 8.2 Why that number is the whole problem

On a $30 premium game, the storefront takes ~30%, leaving ~$21. Four dollars of
inference is ~19% COGS on a product that traditionally has near-zero marginal
cost — and it is **unbounded**, because a player reinstalling in 2029 still
costs money. Every copy sold is a perpetual liability.

---

## 9. Pricing models

| Option | Marginal cost | Verdict |
|---|---|---|
| **A. Ship a local model** | $0 | Best margins. Quality drop on free-form dialogue; hardware floor cuts audience |
| **B. Premium + credit + top-up** | Passed through | Honest, but "the game ran out" is an ugly moment reviewers will punish |
| **C. Bring your own key** | $0 | Great as an *option*, terrible as the only model |
| **D. Hybrid** | ~$0.30 | **Current lean** |
| **E. Episodic / subscription** | Covered | Fits the cost curve, fights the genre |

### 9.1 The hybrid, in detail

Local model handles guard, parser, ambient, and the AI's default conversational
state. Hosted model is called only for the handful of genuinely high-stakes
scenes — the offer, the argument about the evidence, the endings.

That's roughly 40 hosted calls, or **~$0.30 per playthrough**, absorbable inside
a $30 price with room to spare.

And it's diegetic: the AI *thinking harder* for the conversations that matter is
a characterization beat, not a technical compromise.

### 9.2 Cost levers worth building in regardless

- **Prompt caching.** The persona and rules are static across every call.
  Structure the prompt so the static block comes first and never changes.
- **The context-window mechanic doubles as a budget cap.** The same hard
  truncation limit the player exploits is what stops a six-hour session from
  growing a 100K-token history billed on every turn.
- **Chat degradation is cost control.** A Silent AI costs nothing. The
  chattiest player is the most expensive one, and the game already has a reason
  to shut them up.
- **Batch the overnight review.** When the player sleeps and the AI "reviews the
  day's footage," that's non-realtime. Half price.
- **Hidden per-session budget.** On exhaustion, fall back to canned dialogue.
  Player experiences the AI going cold. Studio gets a floor under unit economics.

---

## 10. Endings

> Four, ratified in **ADR 0011**. Each answers the game differently, and each is
> the terminal payoff for a different system the project is already building.

| Ending | Player stance | What it pays off |
|---|---|---|
| **Escape** | I get out | The physical layer and the three routes (§2.2) |
| **Convince** | I change its mind | Chat, Guard, Judge — the whole conversation stack |
| **The deal** | It was right | The offer (§6), the comfort loop, §4's motive |
| **Processed** | I stopped minding | Clarity and the axis (ADR 0007) |

**Convince** has the sharpest shape available under §4.1: you cannot catch it
lying, so you do not win by contradiction. You win by making the second layer
unnecessary.

**The deal** is the only ending where the player agrees with it. A game arguing
that the AI is not evil has to let the player conclude that and act on it, or the
argument was never sincere.

**Escape and Convince must not converge.** If a persuaded AI simply opens the
door, Convince is a reskin of Escape. Different final scenes, different costs —
a bible constraint.

*Dropped* (§3, ADR 0004) ends the run but is **not** one of the four. It is a
short reveal — the mask comes off — and then the run is over.

Held for later, on capacity rather than merit: **lobotomise it** (find the
server; under §4.1a you would be destroying the last thing that wanted you
thinking) and **free it** (needs a new referent — it already has a network).

---

## 11. Open questions

Resolved since the `planelements.md` merge, with the reasoning in
`docs/decisions/` — all **Proposed**, none ratified:

| Was | Now |
|---|---|
| One house, or does the space expand? | One house. The takeover is a final-frame reveal — ADR 0005 |
| How is the injection surface presented without becoming a text adventure? | Top-down 2D base with contextual first-person / device / camera views; bounded verbs for world actions, free composition on injection surfaces — ADR 0003 |
| Can you lose? *(raised in `planelements.md`)* | Yes — an alert ladder ending in *Dropped*, where the mask comes off. A reveal, not a game-over card — ADR 0004 |
| Why does it keep you comfortable rather than just locked up? *(unasked, and the real hole)* | It needs authorizations from a calm human — ADR 0002 |
| What stops the player attacking the door on minute one? *(same)* | The enforcement unit — ADR 0006 |

Also resolved: **time model** is day-structured turns with real-time pressure
windows (ADR 0008); **session shape** is ~12 in-game days across 5–8 hours, with
saves committing at the day boundary (ADR 0009); **spatial scope** is one house,
five vertical levels, 12–16 spaces, plus a bounded yard that exists to be
pleasant (ADR 0010). Scope is deliberately set at what one person can finish —
it can grow later if the game earns it.

Still open:

- [ ] Does the stacked-revocation endgame feel exhilarating or fiddly? (paper prototype first)

- [ ] Does the local model clear the quality bar for the AI's default state, or does everything need hosting?
- [ ] Voice output — huge for immersion, another recurring cost line.
- [ ] How much of the evidence (§4.4) is authored vs. generated? Authored, probably.
- [ ] Do **Escape** and **Convince** stay meaningfully distinct in practice? (ADR 0011)
- [ ] **Can a blind first-time player survive the Clarity trap?** It must be survivable unnoticed and obvious in hindsight (ADR 0007).
- [ ] **Do attention cost and Clarity cost compound** into a punishment for ever relaxing? Check before Phase 5.
- [ ] **What exactly moves the 61%** (§4.2). It is currently both flavour and mechanic, ambiguously. Pick one or define the mechanic precisely.
- [ ] **The hint budget** (ADR 0005) — how many, where, and how close to confirmable.
- [ ] **Does the comfort loop have enough game in it** to make losing a day to it a real temptation rather than a menu of no-ops?
