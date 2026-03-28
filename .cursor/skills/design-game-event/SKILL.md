---
name: design-game-event
description: >-
  Designs narrative events with 2–4 player choices and explicit consequences
  (resource changes and story flags). Use when writing story beats, decision
  nodes, quest-like moments, or when the user asks for events, Entscheidungen,
  Konsequenzen, or branching narrative content for Species Evolution.
---

# Design Game Event

## Goal

Produce a **single self-contained story event** the player resolves by choosing one of **2–4** options. Every option must state **mechanical** outcomes (resources + story flags), not only flavor.

## Output format (required)

Use this structure every time. Localize headings to the user’s language if they write in German (e.g. Titel / Beschreibung / Entscheidungen / Konsequenzen).

```markdown
## Titel
[Short, evocative title]

## Beschreibung
[2–6 sentences: situation, stakes, what the player is deciding. No hidden info the PC could not reasonably know unless framed as rumor/unknown.]

## Entscheidungen (2–4)

### Option A — [Label]
- **Text:** [What the player commits to; one clear action]
- **Konsequenzen:**
  - **Ressourcen:** [resource id]: [+/- amount]; … (use project vocabulary when known; otherwise neutral ids like `energy`, `biomass`, `influence`)
  - **Story-Flags:** `SET` / `CLEAR` / `TOGGLE` — `FLAG_NAME` (one per line; explain briefly in parentheses if non-obvious)

### Option B — [Label]
…

### Option C — [Label] *(only if 3–4 options)*
…

### Option D — [Label] *(only if 4 options)*
…
```

## Design rules

1. **Meaningful fork:** At least two options should differ in *both* resources and flags when possible; avoid “fake” choices that only change wording.
2. **Cost and reward:** Include tradeoffs (spend something to gain stability, take a risk for a flag, etc.). Pure upside on all options is allowed only if the *story* cost is clear (reputation, moral tone, locked branches).
3. **Story flags:** Use **stable, machine-friendly names** (e.g. `DIPLOMACY_SPORE_TRUST`, `PHASE2_GATE_OPENED`). Prefer `SET` for persistent facts; `CLEAR` when a prior flag is invalidated.
4. **Scope:** One event = one decision moment. If the user asks for a chain, output **one** event and note follow-ups as optional `Hinweis` lines, not full extra events unless asked.
5. **Game fit (Species Evolution):** When relevant, hint **phase** (Planetary / Interplanetary / Interstellar), **tech/diplomacy** hooks, or **resource** pressure without inventing unrelated systems.

## Unity alignment (optional follow-up)

If implementation is in scope: suggest storing definitions in a **ScriptableObject** (text, options, costs, flag ops) and resolving choices through a small **event service** that applies deltas and raises **events** for UI/audio/other systems — without coupling narrative text to UI layout.

## Quick checklist

```
- [ ] Titel + Beschreibung present
- [ ] 2–4 Entscheidungen, each with player-facing Text
- [ ] Every option lists Ressourcen (even if “none” / explicit 0)
- [ ] Every option lists Story-Flags with SET/CLEAR/TOGGLE
- [ ] No more than four options
```
