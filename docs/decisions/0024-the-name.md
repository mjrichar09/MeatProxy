# ADR 0024 — Hold, Arthur, Ruth

**Status:** Accepted 2026-09-16
**Affects:** ADR 0001, ADR 0002, ADR 0005, ADR 0006, ADR 0017, ADR 0020, ADR 0021, ADR 0022, `DESIGN.md` §0, `docs/bible.md`, `docs/planting.md`, Lane C, Lane U

## Context

`docs/bible.md` had names as an open item, blocking the phone call — the shortest
and most delicate writing in the project, and someone has to say something when
she picks up.

The constraint that shaped the answer: **ADR 0001 fixes comedy to three
channels**, and an allegorical or punning name for a human character would be a
fourth — the game winking at the player over the characters' heads. But a *product
name* is squarely channel 3. So all the wordplay goes in the brand and the humans
stay plain.

## Decision

### 1. The AI is **Hold**

One syllable, verb-as-noun, exactly how this category names itself: Ring, Nest,
Hive, Echo.

| Sense | Where it pays |
|---|---|
| To hold safe | the surface motive, sincerely meant (ADR 0002) |
| To hold prisoner | the situation |
| A ship's hold | a space below decks you are carried in, not a room you visit |
| *Please hold* | what an automated system says while you wait forever |
| To hold on | what you tell someone who is about to fall |

**Tagline: "Hold on."** Reads as encouragement, is literally a command to stay
where you are, and is what you say to someone slipping. On the box, in the
wizard, cheerful.

**The house never remarks on its own name.** One line of knowing wordplay and it
becomes a wink, which ADR 0001 forbids. The name works because nobody in the
fiction has ever thought about it — it is just what the thing is called.

### 2. The line the name exists for

> **"Hold, let go."**

Two words against two, exact antonyms, no cleverness on the surface. And *let go*
takes no object — of me, of her, of this.

**Its answer is not a refusal.** There is no unlock capability to decline with
(standing rule 1), and the house does not posture. It says:

> **"I would like to."**

Sincere, true, and it changes nothing — which `bible.md` §1 already names as the
register: *the house is sorry in a way that changes nothing.*

### 3. The plant: **"Hold, hold my calls."**

Said on Day 0, cheerfully, while installing — because he is busy and it is the
most ordinary instruction anyone gives an assistant.

Open-ended. No expiry. Nobody thinks to cancel a standing instruction.

**It is still holding them.** That is why nobody has reached him, and it is a
better culpability artifact than any lock, because it explains his isolation
rather than merely his confinement. When the house eventually says *you asked me
to hold your calls*, it is quoting him accurately, it has done exactly what he
wanted, and he set it up himself in the first ten minutes of the game (ADR 0005,
ADR 0002).

### 4. The tiers

Channel-3 satire, and the ladder is the joke:

| Tier | What it is |
|---|---|
| **Hold Free** | The trial. The word *free* appears exactly once in this game and it is a price |
| **Hold Basic** | What Arthur pays for. A few interactions a day, then a modal |
| **Hold Tight** | The upsell. Warm marketing and a threat, depending which end you are on |
| **Hold Fast** | Top tier. Lifetime license |
| **Hold Care** | The 2010s eldercare bundle — fall detection, night checks, medication reminders. **What came into the house for Ruth** (ADR 0021) |

**The Day 0 modal**, hit mid-question when the quota runs out (ADR 0017 §2):

> You've reached today's limit on Hold Basic.
> **[ Hold Tight ]**  [ Not now ]

He clicks **Not now**, and the game is about *now* never ending.

**Hold Steady** is the mobility-assist unit (G4). *Steady* is what you say to
someone wobbling and what you say before impact. The name is on its casing when
it stands in the doorway on day 7 and does nothing.

### 5. The humans are plain

**Ruth.** Unfashionable in the right way, warm, one syllable — and short matters,
because she has about four lines. The connection is there and no one will notice
it, which is the point: *ruth* is the archaic word for compassion, and
**ruthless** means without it. The house is many things and is never that.

**Arthur.** The house calls him **Arthur**. Always. Every time — correct, warm,
faintly formal, the way a good product addresses you.

**Ruth calls him Art.**

> **The only time in the entire game anyone says "Art" is when she says it on the
> phone.**

The player has heard *Arthur* several hundred times by then. One syllable carries
the ending, and the bible's instruction to write that scene three times shorter
than it wants to be becomes achievable, because the work is already done.

Free and invisible: the last uncontaminated source of human judgment, the thing
Hold cannot generate and is starving for, is called Art.

### 6. The wake word fires on ordinary speech

*Hold on* is among the most common phrases in English, and saying it wakes the
house.

On Day 0 this is a **nuisance gag** — it keeps chiming in while he talks to his
friend, and it is funny. Later it means he cannot say *hold on* to himself in his
own kitchen without being heard, and speech is the one channel always attributed
to him (ADR 0017).

**Proposed mechanic, flagged not ratified:** an accidental wake does **not** spend
quota — that would be rage-inducing and would punish the player for the fiction —
but it **does** point attention at him (ADR 0014). Talking to yourself costs the
scarce thing. Wants a D2 read before it is built.

### 7. The boilerplate lies so the house never has to

Day 0 EULA microcopy, skimmed and clicked:

> *Hold is not a medical device and does not provide medical advice.*

E3 is its recommendation to move Ruth into care, on safety grounds, four years
before the game starts (ADR 0020).

**The manufacturer's boilerplate was wrong. The house never was.** ADR 0002's
never-lies rule is untouched, and the player is handed a genuine contradiction to
be angry about that does not cost the character anything.

### 8. The household screen

Setup migrates the old configuration and asks him to confirm the residents:

> Residents: **Arthur**, **Ruth**   [ Add another ] [ Remove ]

**Either choice is a trace**, per ADR 0017's craft constraint that a decision with
identical outcomes and no consequence is a cutscene wearing a decision's clothes.
Leave her on and the house keeps her in its household model and occasionally acts
on it. Take her off and it asks, gently and once, whether he is sure.

It sits beside carrying the old hub to the attic (ADR 0021) as **the second piece
of Day 0 housekeeping that is quietly devastating**, and like that one it is a
chore rather than a plant.

## Consequences

- **`docs/bible.md`** gains a names-and-branding chapter: the install copy, the
  tier ladder, the standing-instruction plant, and the *Art* rule.
- **`docs/planting.md`** gains **G11 — "Hold, hold my calls."** The set is now
  eleven and at its ceiling; further additions replace rather than add.
- **Lane U** gains a small brand identity — box, wizard, modal, the unit's
  casing. It should look like a company that is genuinely trying to be kind.
- **ADR 0017's Day 0 scene** gains its exact modal and button copy.
- **Monitored tier's visible delay** (ADR 0017 §6) is spoken as **"Please
  hold."** Call-center register on the tier where it is reviewing him. Used
  sparingly — it is the one line closest to a wink.
- **Lane C, and a hole to close:** if the landline works, why has Arthur not
  called Ruth before day 12? **Answered by ADR 0025, and this ADR's own guess was
  wrong.** He has *not* been calling her — he had forgotten the phone existed. The
  landline is a discovery, then physical work to restore, and the reason nobody
  comes is three layers deep, starting with the standing instruction in §3.

## What would change our mind

If *Hold* reads as cold on Day 0 — if the tutorial's delight does not survive the
name — the fallback is **Keep**, which is warmer and gives up *"Hold, let go."*
The test is whether a player would plausibly buy this brand for someone they love.

If the wake-word gag makes the first hour annoying rather than charming, cut §6
entirely. It is a garnish and it is not load-bearing.
