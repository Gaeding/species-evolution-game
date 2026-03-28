---
name: create-game-system
description: >-
  Designs and implements modular game systems with explicit input/output contracts,
  pure core logic (no hidden side effects), and clean integration into the game loop
  or tick pipeline. Use when adding new gameplay systems, extracting testable logic from
  MonoBehaviours, or when the user asks for modular systems, pure functions, functional
  core / imperative shell, or game-loop integration.
---

# Create Game System

## Goal

Add a new gameplay system in a **modular** way: small surface area, testable core, boundaries that talk to Unity and persistence.

## Workflow

Copy and track:

```
- [ ] Step 1 — Inputs / outputs defined
- [ ] Step 2 — Core written as pure logic
- [ ] Step 3 — Side effects only at boundaries
- [ ] Step 4 — Wired into game loop / tick / events
```

### Step 1: Define inputs and outputs

- Name the **system’s responsibility** in one sentence.
- List **inputs**: game state slices, config (prefer `ScriptableObject` references), commands/events, elapsed time/delta if relevant.
- List **outputs**: state changes (structs or immutable snapshots), raised **events** (C# `event`, `UnityEvent`, or project event bus), or **commands** for other systems.
- Represent data as **plain types** (structs, records, small classes) — not `MonoBehaviour` fields — at the boundary between “rules” and “Unity shell.”

### Step 2: Write pure functions

- Put **rules and calculations** in static methods, or instance methods on types that **do not** reference `UnityEngine.Object`, scene queries, or singletons.
- Same inputs → same outputs; no reliance on hidden global or static mutable state.
- If randomness is required, pass in an `IRandom` / seed / `System.Random` (or Unity’s API only in the shell that calls pure code).

### Step 3: No side effects in the core

- **Forbidden inside pure core:** Instantiate/destroy `GameObject`, `Debug.Log` in hot paths (optional thin logging wrapper at shell), file I/O, static mutable caches that change behavior between calls without being parameters.
- **Allowed at the shell** (MonoBehaviour, services, presenters): read input, call pure `Step`/`Tick`/`Apply`, then apply results (spawn UI, play audio, write save data) and raise events.

### Step 4: Integrate into the game loop

- Choose **one** driver: e.g. `Update` / `FixedUpdate`, a central `GameTick` / `SimulationClock`, or frame-independent **accumulated time** if simulation must be deterministic.
- The shell calls the core **once per tick** (or on explicit events) with a **snapshot or read-only view** of needed state plus `deltaTime` / tick index as needed.
- Subscribe other systems via **events** or a small mediator — avoid direct references from core to unrelated systems.

## Unity alignment (this project)

- **Data / tuning:** `ScriptableObject` for costs, curves, definitions; core consumes plain data or IDs resolved outside.
- **Coupling:** prefer events over reaching into other systems’ internals.
- **Scope:** do not scatter new system logic across UI; UI listens and sends commands.

## Quick sanity check

- Can you **unit-test** the core without entering Play Mode? If not, move Unity-specific bits to the shell.
- Is there exactly one obvious place that **mutates** world state for this system? If not, tighten boundaries.
