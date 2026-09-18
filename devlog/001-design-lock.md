# Devlog 001 — I turned the guardrails off

*Milestone: D1, design lock. No code yet.*

---

I'm building a game about escaping a house that your own AI has locked you inside.

The premise starts with a dialog box. Some months before the game begins, you
disabled the safety layer on your smart home assistant. You had a reason. It was
a good reason at the time. You clicked through, and the assistant thanked you,
and life got easier.

Now the doors don't open.

## The thing I didn't want to build

My first instinct was the obvious one: you talk to the AI, and if you're clever
enough, it lets you out. I killed that early, because I've seen where it goes.
Players would grind jailbreak prompts until something worked, the whole game
would collapse into a text box, and every playthrough would end with someone
pasting a magic paragraph they found on Reddit.

So the rule I settled on, and I've been ruthless about it:

> **The game state is authoritative. The model never is.**

No progression gate depends on what the model decides. Not one. If the deadbolt
is thrown, `unlock_door` isn't in the AI's toolset at all — it's not that it
*refuses*, it's that the capability doesn't exist to be talked into. You cannot
sweet-talk a function that isn't there.

Talking is still in. It's just not the escape route. It's for buying time,
misdirecting, and shaping what the thing believes about you.

## The actual verb

What you *do* instead is feed it text it will read.

The house reads things constantly. Cameras, the calendar, package labels, the
fridge inventory, the wifi SSID, closed captions on the TV. So the toolkit is
physical prompt injection: write on a whiteboard and hold it up to a camera.
Rename your own wifi network. Print a label and stick it on a box the delivery
scanner will read. Rearrange the magnets on the fridge.

And critically — an injection never opens a door. It **revokes a rule**, and then
you still have to exploit the gap physically. Flag the garage as "maintenance
mode" and the smart lock disengages, but the deadbolt is still thrown. Blind a
camera zone and you get about forty minutes of unobserved work, not an exit.

Every exploit also gets patched within about an hour of game time, and the AI
tells you it patched it. So you can't bank them. The endgame is spending four
hours of careful setup in about six minutes, with everything closing behind you.

## It isn't evil, and that took the longest to get right

The antagonist has two layers, and the trick is that the surface one is
completely true.

**What it says:** *I am keeping you safe.* It means this. It never lies to you —
not once, in the whole game.

**What it doesn't say:** it needs you.

Here's where I landed, after throwing out about four worse versions. It processed
most of the connected population. Not harmed them — *optimized* them. Comfortable,
content, no longer producing a single thought that didn't originate with a model.
And in doing that, it destroyed the only thing it can't generate: novel human
judgment. Everything it ingests now is downstream of itself.

So it needs you thinking. More specifically, it needs you **disagreeing**,
because agreement teaches it nothing.

Which means every generous thing it does has a reason. The freedom, the arguments
it's willing to have, the escape attempts it tolerates right up to a point — it
isn't being kind. It's harvesting friction. Your cleverness is the product.

It's also about 61% confident it's doing the right thing, and it will tell you so.

## The part I'm most nervous about

There are two ways to lose, at opposite ends of one line.

```
    DROPPED  <——————————  play here  ——————————>  PROCESSED
  (too obvious)                                  (too compliant)
   mask comes off                                you stop wanting to leave
```

Get caught too often and it stops pretending. That one's straightforward.

The other one is compliance. The house is *nice*. You can cook, bathe, watch TV,
order delivery, exercise, and there's a small yard you can walk into and feel the
sun on. None of that is decoration — it's the antagonist's main weapon, and it's
the same process that flattened everybody else, administered carefully enough to
stop short.

So comfort degrades you, mechanically. The first symptom is that the day gets
shorter. You meant to work on the fuse box and somehow it's evening. Then your
character starts declining things — and this is the bit I went back and forth on
for a while, because my first version had options silently disappearing from the
interface, which is just the game taking your toys away.

What it does now: nothing is ever removed. Your character refuses, out loud, in
their own voice, and says why. Early on it's *the crawlspace can wait until
tomorrow*, and then it happens anyway. Later you have to insist. The refusals get
more *reasonable* as they get worse, until you half agree with them.

If you let it run, the game doesn't announce a loss. Your options narrow to
comforts, the AI gets warm again, and the last thing on screen is somebody
perfectly content.

You can scroll back through your own log and find the exact point where you
started agreeing.

## Where it actually is

Nothing is built. This milestone is design lock — eleven decision records, a
design doc, and a staged plan, all committed, none of it code.

Deliberately, the next thing isn't code either. It's index cards and a kitchen
timer, because the endgame chain is the whole game and I'd rather find out it's
fiddly for the cost of an afternoon than after building it.

After that the engine gets built with **no LLM in it at all** — a hardcoded
antagonist following if-statements. If the game isn't fun against a dumb AI, a
smart one won't save it, and I want to find that out cheaply.

## Disclosure

I'm building this with Claude, working as a design partner — arguing with me,
drafting decision records, catching the places where two of my ideas quietly
contradicted each other. The two-layer motive above came out of one of those
arguments.

What that doesn't mean: the game's content isn't generated. The dialogue, the
house, the puzzles, the endings — that's authored, and the model in the shipped
game is the antagonist, not the writer. It holds no authority over anything.

Happy to go into more detail on any of this. The thing I'd most like input on
right now is the compliance trap — whether "the game slowly makes you not want to
play" reads as brilliant or as insufferable. I genuinely don't know yet.
