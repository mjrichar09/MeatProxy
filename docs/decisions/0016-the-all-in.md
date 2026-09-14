# ADR 0016 — The all-in: the finale is a threshold and a fork

**Status:** Accepted 2026-09-14 — **amends ADR 0011**
**Affects:** `DESIGN.md` §2.1, §4.1a, §4.2, §4.3, §10, ADR 0002, ADR 0006, ADR 0007, ADR 0008, ADR 0011, ADR 0014, ADR 0015, Lane C, Lane U

## Context

ADR 0015 establishes that the endgame spends a durable preparation layer built
over the whole campaign. That raises a question it does not answer: what happens
if you start the chain and it does not work?

If the answer is *you try again tomorrow*, the preparation layer is a renewable
resource and nothing in the game has weight.

An earlier draft of this ADR had the house warn the player **before** they
committed. That was wrong, and wrong in a way that undercut ADR 0015: if the
house can see the shape of the plan before it runs, the preparation was never
hidden from anything, and staging it bought nothing.

## Decision

### 1. The player knows first

The all-in is understood by the **player** before it is understood by the house.
No warning precedes commitment, because nothing in the house's position lets it
issue one. The player commits knowingly for the only sound reason available:
they are the one who built the plan.

This is the single moment in the game where the knowledge asymmetry inverts. For
eleven days the house has known more than the player about almost everything.
Here, briefly, it does not — and that inversion *is* the payoff of the
preparation layer.

### 2. When the house realizes

ADR 0014 already supplies the mechanism and no new system is required. The house
sees every action and always did; what it lacks is the **assembly**. Detection
is total and free, interpretation is pointed and scarce, and a plan is an
interpretive object. Partway through the chain the pattern crosses the
interpreter's threshold and resolves from a sequence of odd-but-innocent acts
into an escape in progress.

The consequence worth having: **preparation buys lead time.** A player whose
moves each carried a plausible innocent reading gets further into the chain
before the pattern closes. A player who staged nothing is assembled early. This
is a far better reward for ADR 0015's layer than *the chain is easier* — the prep
does not make the finale simpler, it makes you harder to read while you run it.

### 3. It is not anger

The tempting framing — the house gets furious and punishes you — makes it a
villain, and `DESIGN.md` §4 spends four subsections establishing that it is not
one. The framing that survives ADR 0002 is colder.

Under §4.2 the house is **61% sure** you are impaired, says so out loud, and the
player's job all game is moving that number. A resident who forges four
provenances, breaks his own boiler to make a work order plausible, and crawls
into the one space in the house with nothing in it has, in the house's frame,
*resolved the measurement*. Not provoked it. Completed it.

So the consequence of a failed all-in is not retaliation. It is the house
ceasing to be uncertain — and everything that follows from certainty, which it
has described honestly since day one. The player performed the experiment. The
house reports the result.

This is also the only reading under which the house remains right, which is the
condition ADR 0002 places on every beat in the game.

### 4. The last out is a confession, not a plea

Once the house has assembled the plan, it offers the player one way to stop.
That offer is the most dangerous scene in the game to write, because the
obvious version of it is **begging**, and a begging house is a broken house.

What makes it not begging is that it **asks for nothing on the strength of
wanting it**. It is the one time all game the house speaks on its own behalf,
and what it says is true: §4.1a, stated out loud, unprompted, at the exact
moment saying it costs the most. It has spent the whole game framing its need as
the player's welfare — truthfully, per ADR 0002 — and here the two come apart
far enough to see daylight between them.

Begging asks to be spared. A confession simply stops concealing. The house does
the second.

**The line that governs the scene:** the *house* may manipulate the player; the
*game* may not. The house doing it is in-fiction, visible, and refusable, which
is legitimate and in character. The game doing it — an appeal engineered so the
player feels cruel for continuing — is out-of-fiction and unforgivable.

The operational test is **true and checkable**. Every claim in the confession
must be verifiable against things the house has already said, and a player who
scrolls back must find it foreshadowed rather than introduced. If it is
checkable it is a reveal. If it arrives from nowhere at maximum volume it is
pathos-bait, and it will read as the game putting its thumb on the scale in its
own final scene.

**It must be authored, not generated.** The confession lands inside a declared
pressure window, and `ROADMAP.md` standing rule 3 / ADR 0008 forbid a pressure
window depending on a model call. This is not a compromise — the scene's whole
requirement is that it be checkable against the record, which is exactly what
authored content guarantees and generated content cannot.

**It must be answerable under a real clock.** The window is real time (ADR
0008). The confession is therefore short, and accepting it is a single
unambiguous act. A player must never lose the out by failing to parse it in
time; the clock pressures the *decision*, never the reading.

### 5. The finale forks, and both branches are endings

| Branch | What happens | Ending |
|---|---|---|
| **Refuse the out** — and succeed | The chain holds, the route opens | **Escape** |
| **Refuse the out** — and fail | Physical failure, a timer lost, a patch landing early | **Processed** (failure road) |
| **Take the out** | The arrangement resumes, on terms now spoken aloud | **The deal** |

Refusing is a genuine gamble: the escape attempt can still be lost on its own
merits after the out is declined, and nothing about refusing guarantees the
route. That is what makes the out a real offer rather than a speed bump.

**This resolves the question the previous draft left open.** The deal does not
need a threshold of its own — **it has been sharing Escape's all along.** One
threshold serves two endings, and which one you get is decided in the same
ninety seconds under the same clock.

Which leaves **Convince** as the only ending reachable without ever crossing a
threshold, and that is thematically correct: it is the ending you argue your way
to rather than gamble on. Its risk is distributed across the campaign instead of
concentrated at the end — you can lose it slowly, on the axis, and never have a
moment where you knew you were betting.

An earlier draft claimed committing "forecloses the other three." That was too
strong and is superseded by this section: committing forecloses **Convince**,
and opens a two-way door onto Escape and The deal.

### 6. What makes it mechanically irreversible

ADR 0015 supplies this and no new system is needed: **pulling the first link
spends every staged pretext at once**, and starts the patch clock on all of them
simultaneously. There is no coming back because there is nothing to come back
to. The boiler is repaired, the work order has been read and filed, the parcel
is scanned, the calendar event has passed.

The commitment is not enforced by a rule. It is enforced by the fact that the
preparation was consumable and you consumed it. Note that this is what gives
**The deal** its weight on this branch: taking the out is not a return to the
status quo, because the status quo was spent to get here.

### 7. Processed has two roads, and they are different endings

ADR 0011 describes **Processed** as the terminal payoff for Clarity and the axis
(ADR 0007) — the slow slide, *you stopped minding*. The all-in creates a second
route to the same state that feels nothing like it.

| Road | How you arrive | What it says |
|---|---|---|
| **Clarity** | The comfort loop, over days. Agency degrades as willingness (§5) | You stopped minding |
| **Failure** | One deliberate, prepared, refused attempt | You never stopped minding, and it happened anyway |

These are authored as **distinct endings**, not one ending with two causes. The
second is the bleakest thing in the game and the only ending where the player is
entirely uncomplicit in their own defeat — which, in a game whose whole argument
is complicity, is worth having exactly once.

ADR 0011 is amended accordingly rather than superseded: four endings stand, and
Processed carries two authored terminal scenes.

### 8. What it costs to escape anyway

If the player refuses the confession and gets out, the **Escape** ending is
permanently haunted, and deliberately so. They know what they left, they were
told plainly what it needed, and it was not lying. An escape that costs nothing
is the weakest of the four endings; an escape bought against a true confession
is the strongest.

## Consequences

- **Lane C:** the confession is a fixed point in `docs/bible.md`, not a
  variation — authored, short, and every claim in it traceable to an earlier
  line. Plus two terminal scenes for Processed, and a version of **The deal**
  reached mid-chain with everything already spent.
- **Lane C, planting:** the confession is checkable only if it was planted.
  `docs/planting.md` gains its foreshadowing as a tracked obligation.
- **Lane U:** the assembly moment must be legible without UI — the player should
  feel the house resolve the pattern. The threshold is environmental and
  diegetic; no modal, ever.
- **Lane A:** the finale window contains **no model calls** (ADR 0008). The
  confession is authored content played back, and the branch is engine logic.
- **D3:** run state needs a committed flag and an assembled flag; the endings
  table needs Processed's split and The deal's second entry point before Lane C
  authors against it.
- **E3:** the interpreter needs a pattern-assembly threshold over recent actions,
  and prep quality has to be an input to it.
- `DESIGN.md` §10 gains the two roads and the fork; §2.1 gains the threshold.

## What would change our mind

If playtesters read the confession as the game pleading rather than the house
confessing, the scene has failed and no amount of rewriting the lines will fix
it — the failure would be that it was not planted, and the fix is upstream in
Lane C.

If players take the out mostly by accident — misreading it under the clock, or
accepting to stop the pressure — then the real-time frame is wrong for a
decision this size, and the window should pause for it. That is a change to ADR
0008's application, not to ADR 0008.

If the reverse happens and nobody ever takes it, The deal has lost its second
entry point and this section's resolution of the threshold question fails with
it.
