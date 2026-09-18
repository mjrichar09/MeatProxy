# ADR 0013 — Provenance: why you cannot just ask

**Status:** Accepted 2026-09-14
**Affects:** `DESIGN.md` §2, §3, D3's injection-vector schema, the D2 prototype, Lane C

## Context

`DESIGN.md` keeps two channels: direct conversation (§3) and physical prompt
injection (§2). It never states the relationship between them, and without that
statement injection reads as a bizarrely indirect way to make a request — *why
write on a whiteboard when the house has a microphone?*

That question has to have an answer a player arrives at on their own in the first
hour, or the central mechanic feels like a puzzle-game contrivance.

## Decision

**Speech is evaluated. Data is ingested. You can ask for anything and receive
nothing.**

Injection is not a way of talking to the house. It is a way of **manufacturing
where a fact came from**. The player is not phrasing a request more cleverly —
they are forging provenance.

### The three reasons, in the fiction

**1. Requests are checked. Context is not.**

When you speak, the house handles it as a *request from the resident*: routed
through permissions, evaluated against your current tier, logged. When it scans a
parcel, reads a calendar invite, or captions a broadcast, it is ingesting context
while doing an unrelated job. Nothing in that path is asking whether the context
should be believed.

This is not the house trusting a label more than it trusts you. It is the label
never reaching the part of the house that decides what to trust. That is also
precisely how prompt injection works in reality, which is what makes it play as
true rather than as a convenience.

**2. Its guard faces you.**

The tier system (§3) is a defensive posture oriented at the resident. It hardens
speech. It does not harden the delivery scanner, the thermostat schedule, or the
closed-caption stream — because those are not where a resident argues from.

**3. Asking makes you legible.**

Every request is evidence of intent. Under ADR 0002 it is reading you constantly,
and *what you ask for* is the cleanest signal you can hand it. "Put the garage in
maintenance mode" tells it exactly what you want the lock to do.

A successful injection tells it nothing about you at all.

### What speech is still for

Conversation is not a dead channel — it is the channel for everything that is not
a capability:

- **Arguing.** Moving the 61%. This is the entire Convince ending (ADR 0011).
- **Misdirection.** Being loud in the kitchen is an attention cost it has to pay.
- **Buying time**, and reading its state from its tone.
- **Compliance**, which walks tiers back up and is a real strategy.

So the player who tries asking is not punished for being naive — they get a
polite, in-character refusal, they learn the shape of the wall, and they have
spent a little suspicion to find out. That is a good first hour.

### Provenance is a property of the vector

Every injection surface carries an **implied source**, and that source is what
determines whether an injection is plausible at all:

| Vector | Implied source | Provenance |
|---|---|---|
| Parcel label | The shipper | Strong — arrives through a logistics path |
| Calendar invite | An external organizer | Strong |
| Work order taped to the boiler | A technician | Strong, but requires staging |
| Closed captions | The broadcast | Strong, narrow payload |
| Wifi SSID | Network infrastructure | Medium — you own the router, but it reads as config |
| Fridge magnets | The household | Medium — ambient, unattributed |
| **Whiteboard held to a camera** | **You, visibly** | **Weak** |

The last row is the correction this ADR forces. A whiteboard you are holding up
is self-evidently from you, which makes it speech with extra steps — it should be
the *weakest* vector, not the showcase one. It works only when the house reads it
without attributing it to you: left in frame while its attention is elsewhere,
written as a note from someone else, or caught in a reflection.

**Schema consequence:** injection vectors need a `source` and a `trust` field, and
the parser's verdict must account for attribution, not only content. An injection
that says the right thing from the wrong mouth is a flag, not a success.

## Consequences

- **D3's injection schema gains `source` and `trust`.** Content alone cannot
  determine a verdict.
- **The D2 prototype needs a chat channel** so the contrast is testable. Let the
  tester ask first, fail informatively, and reach for the surfaces themselves.
  If they never try asking, we have not learned whether the distinction is legible.
- `DESIGN.md` §2 gains this framing near the top. It is the missing sentence that
  makes the central verb make sense.
- **Lane C:** the whiteboard is demoted. Early-game vectors should be ones with
  strong natural provenance, so the player learns the principle before they learn
  the exceptions.
- The Judge (§7.2) and the Parser now answer different questions — *was that
  persuasive* versus *was that plausibly not you*. Their eval suites diverge.

## What would change our mind

If D2 testers reach for the surfaces without ever trying to ask, the distinction
is not being discovered — it is being assumed by people who read the brief. Fix
by making the chat channel more prominent, not by explaining the rule.
