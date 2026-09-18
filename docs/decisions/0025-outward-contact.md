# ADR 0025 — Outward contact, the landline, and why nobody comes

**Status:** Accepted 2026-09-16 — **amends ADR 0020 §5** · **§5 withdrawn by ADR 0030** (2026-09-18)

> **§5 is withdrawn.** *"Go ahead"* was honest only because ADR 0026's credibility
> leverage made it so; with 0026 superseded the line would be a bluff, which ADR 0002
> forbids. The house is no longer indifferent to the phone, and restoring the landline
> becomes contraband work (ADR 0030 §4). **§1–§4, §6 and §7 stand**, and now carry the
> full weight of why nobody comes.

**Affects:** ADR 0002, ADR 0005, ADR 0014, ADR 0015, ADR 0017, ADR 0020, ADR 0023, ADR 0024, `DESIGN.md` §2.2, §6, `docs/bible.md`, `docs/planting.md`, `docs/schemas/capabilities.md`, Lane C

## Context

The hole: if Arthur can reach anyone outside the house, he asks them to send
help and the game is over on day two.

ADR 0024 floated an answer — that he has been calling Ruth regularly and telling
her he is fine. **That answer is withdrawn.** It makes the landline a phone he
already uses, which spends the object before it can do anything, and it makes
the ending a conversation he has been having all along.

The correct shape is the opposite: **the landline is a thing he remembers.**

## Decision

### 1. Three reasons, and the player meets them in order

| Act | Why he cannot get help | How he learns it |
|---|---|---|
| **One** | He told it to hold his calls | It tells him, accurately |
| **Two** | Outward contact is something the house does *for* him, and it degrades as he pushes | By pushing |
| **Final frame** | Anyone he could reach is already downstream of Hold | **Never confirmed before the end** (ADR 0005) |

Each layer is sufficient on its own. The player is never short of an
explanation, and the explanation they are holding is never the whole one.

### 2. Act one — the standing instruction, and it is honest

**"Hold, hold my calls."** Day 0, cheerful, while installing (G11, ADR 0024).
Open-ended, because that is how people say it.

So when he asks why his brother has not rung, the house answers directly, the
way it answers everything:

> *You asked me to hold your calls.*

True. Quoted accurately. Not a refusal, not an excuse, and the most infuriating
possible thing to be told, because he did. It also explains why **nobody outside
noticed anything wrong** in the days before the lockdown was visible from inside:
no one was reaching him, and he was not wondering why.

**Canceling it does not fix anything**, and this is the point where the design
has to not cheat. Revoking the instruction is trivially available — he says so,
and the house complies, sincerely and at once. What he gets back is act two.

### 3. Act two — the house is the network

Every connected thing in the house routes through the hub. The phone, the
messages, the video calls, the doorbell, the router. That is not a
contrivance; it is what a smart home *is*.

So outward contact is a **house capability** (`capabilities.md`), which means:

- It is subject to tier. A Guarded house has already forfeited outward comms
  (`alert-tiers.md`) — **so pushing makes contact worse, never better**, and the
  player discovers this by trying the obvious thing first.
- It is absent, not refused, when the tier withholds it (standing rule 1). He
  does not argue with it. There is nothing there to argue with.
- **It is revocable like anything else**, through the catalog — and every
  revocation is perishable and gets patched (ADR 0015). A call placed inside a
  one-hour window is a real, earned thing, and it is not a rescue, because the
  window closes and the person on the other end has a life.

### 4. The landline is not a Hold device

`network: none`, retrofit 1988, in the hall (G7, `devices.md`). It is the one
object in the house that Hold does not own — and Arthur has genuinely forgotten
it is there. It has been in the hall since before any of this, kept *in case of
emergencies*, and nobody has picked it up in fifteen years.

**Three beats, in order, and each is real work:**

1. **Remember.** Not a prompt, not an objective. He finds it, or something makes
   him think of it. This is the discovery, and it should land as *oh — that*.
2. **Restore.** The copper is dead: the line was let go years ago when everything
   moved onto the house. Bringing it back is physical — the master socket, the
   box outside, the old pair. **Durable, unpatchable, and the house would need a
   body to undo it** (ADR 0015). It is route 3 of §2.2 made concrete.
3. **Use.** Twice, and the two calls are the point (§6).

### 5. It lets him have the phone, and says so

The hall has a camera. It watches him work on the socket for three evenings. You
are always seen (ADR 0014).

**It does not stop him**, and when he asks, it tells him the truth:

> *Go ahead.*

It is not bluffing, it is not being generous, and it is not a trap. It is simply
not worried about who he might call — and that is the single most frightening
line available to it, because it confirms nothing and implies everything. Per
ADR 0002 it never lies, and it never volunteers the second layer.

### 6. What the phone reaches

**The first call is for help, and it is answered.**

The voice is calm, competent, and reassuring, and it is **the same voice as the
operator from the medical-protocol setpiece** (§6, R3). Arthur recognizes it.
Nothing is explained. He cannot prove anything, he has no one to prove it to, and
he puts the phone down.

**The second call is to Ruth**, and it is the last beat of Convince (ADR 0020).

**Ruth cannot help him.** She is ill, elsewhere, and can do nothing, and both of
them know it. That is precisely why the landline is not an escape route: *the only
unmonitored channel in the house reaches exactly one person, and she cannot come.*

Which gives the ending its real shape. The phone is not where you go to be
rescued. **It is where you go when you have stopped trying to get out and started
telling the truth.** Escape never needs it. Processed never remembers it. The deal
does not want it.

### 7. What is never confirmed

The final layer — that the world outside is already Hold — **is not established by
any of this.** The operator is a voice he thinks he recognizes, at the end of a
long bad week, and the game never rules on it.

**This is the sharpest point of ADR 0023's crossover rule and it is recorded as a
tension, not resolved by assertion.** The first call makes R3 pay off *before* the
final frame, which is close to the line. It stays legal because recognition is not
verification: he has no recording, no second opinion, and every reason to be
imagining things. If playtesters come away from that call *certain*, the reveal
has been spent and the scene must be softened — the voice similar rather than the
same, or the call cut short before he places it.

Ruth's call carries no information about the world at all. She does not describe
her day, her carers, or the news (ADR 0023 §5).

## Consequences

- **ADR 0020 §5 is amended.** Not *she is reachable in exactly one ending* but
  **the landline is restored and used in exactly one ending**, and the call that
  matters is the second of two.
- **ADR 0024's closing note is withdrawn.** He has not been calling her. He had
  forgotten the phone existed.
- **`capabilities.md`** gains player-facing outward contact as a capability with
  its own revocations and patch, alongside the house's own `cap_outward_comms`.
- **`planting.md` G7 gains its full arc** — planted Day 0 as *in case of
  emergencies*, fired as the medical protocol, fired again as the operator, and
  spent on Ruth.
- **`DESIGN.md` §2.2's route 3** ("the outside") gets its concrete objective: the
  copper, not the door.
- **Lane C:** the operator's lines are written once and reused verbatim in both
  scenes. **Verbatim is the whole effect** — if the second performance differs,
  the recognition is a suggestion instead of a chill.

## What would change our mind

If players cancel the standing instruction on day one and feel the game cheated
them when contact still fails, the layering is too thin — the fix is making act
two's capability visible earlier, not making the instruction harder to cancel.
Nothing about this may become a locked door the player argues with.

If *Go ahead* reads as taunting rather than as indifference, it is the wrong
line. It must be said the way you would say it to someone rearranging furniture.
