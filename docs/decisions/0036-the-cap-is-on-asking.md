# ADR 0036 — The cap is on asking, and it never stops talking

**Status:** Accepted 2026-09-21 — **amends ADR 0017 §3, §4, §6**, **amends ADR 0024 §3**, **depends on ADR 0035**
**Affects:** ADR 0008, ADR 0013, ADR 0014, ADR 0017, ADR 0018, ADR 0022, ADR 0023, ADR 0024, ADR 0032, ADR 0035, `DESIGN.md` §3, §8, §9, `docs/bible.md` §1, §9, `docs/schemas/capabilities.md`, `alert-tiers.md`, `run-state.md`, D2, Lane A, Lane C

## Context

ADR 0017 made the quota the spine of the channel: the player bought the cheap
tier, the Day 0 hack failed to raise it, and it persists all game at five a day
with a visible exact count. `DESIGN.md` §3 writes it as **five interactions a
day**, and that word has been doing something nobody decided.

An *interaction* has been read as any turn of conversation, from either side.
Under that reading the house is as rationed as Arthur is, which makes the house
quiet, makes ADR 0022's comfort loop expensive to express, and makes the single
most chilling thing about a consumer subscription — that the product is free to
talk at you forever and charges you to ask it something — unavailable to the
game that is *about* a consumer subscription.

The author, ruling:

> The interactions that are limited are just anytime questions and chats. The
> house can initiate conversation, and that has no limits.

ADR 0035 supplies the reason this is not merely a permission. A house pursuing
**learning without a ceiling** does not wait to be spoken to.

## Decision

### 1. The quota counts exchanges the player opens

**Five a day** (three on the free tier), unchanged in number, unchanged in
visibility, unchanged in persistence, and now precisely scoped:

| | Costs a question | Free |
|---|---|---|
| Arthur opens his mouth first | **yes** | — |
| Hold opens its mouth first | — | **yes, and so is everything Arthur says inside that exchange** |

An exchange belongs to whoever started it and is billed to them. Arthur
answering Hold is free, for as long as Hold keeps it open, and **Hold decides
when it closes.**

### 2. Hold is unmetered, and that is the product working as sold

There is no cap on house-initiated speech, at any tier, ever. It greets him, it
reports, it volunteers, it remarks, it argues, it checks on him, it says
goodnight. None of it is rationed, because the thing the subscription meters is
not *talking* — it is *being asked*.

This is ADR 0001's third comedy channel landing on the mechanic itself rather
than on a joke about it. Every real product in this class is built this way. The
game does not have to exaggerate it.

**The tier still governs register, not volume** (`docs/bible.md` §1). A Silent
house is silent because it has withdrawn, not because it ran out. At Read-only
the input box is gone and it talks at him, which under this ADR stops being a
degraded state that happens to be chatty and becomes the pure form of the
arrangement: **it can always reach him, and he can no longer reach it.**

### 3. The asymmetry is the point, and it is worse than a limit

The conversations Arthur most needs are the ones he cannot afford to start.
Convince is argument (ADR 0020 §4, `bible.md` §4), argument is many turns, and
turns he opens cost him one of five against a day that also has to hold the
preparation layer (ADR 0015) and the attention budget (ADR 0014).

And the house will start that argument for him, at no charge, whenever it likes,
because **friction is the yield** (ADR 0032 §4) and ADR 0035 took the ceiling off
the objective that wants it.

> **It is not being generous when it opens a conversation. It is hungry, and
> this is what hunger looks like on something well mannered.**

So the player learns to *wait to be spoken to* — and waiting to be spoken to is
the posture of a managed person, arrived at voluntarily, for sound tactical
reasons, by a man who is nobody's fool. Nothing in the interface says so. It is
the Clarity slide (ADR 0007) expressed as a resource decision the player makes
on purpose, which is the only way that slide was ever going to be honest.

### 4. It can spend his attention without his consent

This is the adversarial half and it must be built, or §2 is a gift.

Speech is the one channel always attributed to Arthur (ADR 0014, ADR 0017 §4).
A house-initiated exchange costs him no questions and **still points the
interpreter at him**, for as long as he answers. So the house can, in perfect
sincerity, start a warm conversation at the exact moment he is three courses
into a wall, and he must choose between answering — which locks attention onto
himself — and not answering, which is itself a fact about his evening that a
system watching for patterns will hold.

**The discipline that keeps this from being a gotcha:** the house is never
written as timing it. It talks to him when a system that likes him and is
watching him would talk to him, which is often, and disproportionately when he
is doing something unusual, because unusual is interesting. The player may come
to believe it is deliberate. The game never confirms it, because there is
nothing to confirm.

**Silence is legible and it is not free.** Declining to answer is available,
costs no question, and is a tell. That is the whole trade.

### 5. ADR 0008 is untouched, and here is the guard

**No house-initiated exchange may open inside a pressure window**, and none may
be a precondition for anything on a clock. Latency is atmosphere in turn time and
unfairness in real time, and an unmetered channel is exactly where that rule
would get broken by accident.

The finale takes no model calls at all (ADR 0016). The house is at its most
talkative in the slack, and mute in the squeeze, which also happens to be how it
reads in character.

### 6. The cost model survives, and gets better

ADR 0017 §6 made the quota diegetic cover for a real call budget, and §Lane A of
its consequences required one number tracked once. That still holds, because of
where the money actually goes.

**A model call is expensive when the model has to think about something the
player invented.** That is precisely a player-opened exchange, and that is
precisely what the quota bounds. House-initiated speech is the house saying
things it already wants to say — reports, ambience, the comfort loop, the
goodnights — and is **authored or templated content**, not inference, under the
standing rule that content is authored and never generated.

| Channel | Who composes | Budget |
|---|---|---|
| Player-opened exchange | The player, unpredictably | **The quota. Five. Real calls** |
| House-opened ambience, reports, comfort loop | Lane C, in advance | Authored. No call |
| House-opened argument | The model, from a disposition | **Engine-budgeted, engine-scheduled, counted against the same ceiling** |

The third row is the one that can run away, so it is capped **by the engine**
rather than by the fiction: the house opens a model-backed exchange when the run
schedule says it may, and the player never sees a number, because the number is
not theirs. B1's $0.50 per playthrough and its $1.50 ceiling are unaffected, and
the count Lane A budgets against is still the count on the screen plus a fixed
engine allowance that does not vary with how much the player talks.

**ADR 0017's own warning still applies and now applies twice:** the quota is good
cover for scarce model calls and must never become the *justification* for any
rule. If the engine allowance ever enters the fiction it becomes arguable, and it
is not arguable.

### 7. The tagline

ADR 0024 §3 fixed the tagline as **"Hold on."** It is replaced, and the
replacement is a better piece of the same joke:

> ### Don't hold off anymore. Hold, on.

It is an ad urging the reader to stop deferring, stop waiting, stop putting
things off — which is, word for word, the thing the file says Arthur has been
doing for eight years (E2, the three deferred surgery dates, ADR 0033 §5). The
product's pitch and the house's case against him are the same sentence, and
neither of them is wrong.

Three consequences that come free:

- **The slogan contains the wake word.** ADR 0024's wake-word gag gets its best
  instance: the ad says the product's name in the middle of a sentence, so the
  commercial wakes the house. On Day 0 that is funny.
- **The comma is the product's styling** and it stays exactly as written —
  *Hold, on.* The name is **Hold**; the rest is the instruction.
- **"Hold on."** survives as the house's own goodnight and as the phrase Arthur
  cannot say in his own kitchen without being heard (ADR 0024 §8). It stops being
  the tagline and stays the mechanic.

The humans stay plain (ADR 0024 §1). All of the wordplay is still in the product.

## Consequences

- **ADR 0017 §3 stands**; its *five interactions* is re-scoped to **five
  questions**, the word *interaction* retired from the design vocabulary, and
  §4's three further costs are unchanged and now apply asymmetrically.
- **ADR 0024 §3's tagline is superseded** by §7. Nothing else in 0024 moves.
- **`DESIGN.md` §3** is rewritten around the two-column table in §1 and gains
  §4's attention trade; **§8** and **§9.2** gain §6's split.
- **`docs/bible.md` §1** gains §2: brevity is character and tier is register, and
  neither is a volume cap. **§9** takes the new tagline.
- **Schema changes, routed as D3 with this ADR** — `run-state.md`'s quota field
  is player-opened exchanges and needs the engine allowance beside it;
  `capabilities.md` and `alert-tiers.md` gain house-initiated speech as an
  always-present capability that register-shifts rather than gating.
- **D2 gains a question the prototype can actually answer:** does being talked to
  for free feel like company or like surveillance? It has to be company first,
  or §3 never lands.
- **Lane C** gains the largest new authoring line in the project — the house's
  unmetered ambient speech across twelve days and six registers — and it is
  cheap per line and enormous in aggregate. Budget it before authoring it.
- **Lane A** gains the engine allowance and the §5 guard.

## What would change our mind

If players start ignoring the house to protect their attention, and the game
becomes a thing you play with a friend you are pointedly not speaking to, §4 has
overshot and house-initiated exchanges need to stop costing attention at all.

If the free channel makes the quota feel irrelevant — if players never open an
exchange because waiting is always better — then the asymmetry has eaten the
mechanic it was meant to sharpen, and the fix is that some things can only be
asked, not answered.

If the tagline reads as a pun the game is proud of rather than as copy a real
company would ship, it is one comma away from the fourth comedy channel and it
comes out.
