---
name: expand-tech-tree
description: >-
  Adds technologies to Species Evolution with prerequisites and unlock/effect hooks.
  Use when extending the tech tree, defining Tech-Voraussetzungen, research costs,
  or technology effects; or when the user mentions Technologien, Tech Tree, or Forschung.
---

# Expand Tech Tree

## Goal

Introduce **new technologies** as data-driven content: stable IDs, clear **prerequisites**, and explicit **effects** applied when a tech is unlocked — without scattering logic across UI.

## Workflow

```
- [ ] Step 1 — Technology asset / definition
- [ ] Step 2 — Prerequisites (graph rules)
- [ ] Step 3 — Effects (what changes when researched)
- [ ] Step 4 — Wire TechTree service + events
```

### Step 1: Define the technology

- Add or extend a **`ScriptableObject`** (e.g. `TechnologyDefinition`) with: **stable id** (string or enum-backed), display name, description, **phase** (`GamePhase` or tier) if relevant, **cost** (resource amounts referencing `ResourceDefinition` assets).
- Keep **tunable numbers** on the asset; avoid magic numbers in MonoBehaviours.
- If the project uses a **registry** (list of all tech SOs) or addressables, register the new asset there.

### Step 2: Prerequisites

- Represent prerequisites as **explicit references**: e.g. `TechnologyDefinition[]` or a small struct listing required tech ids — **no** hidden “string id only” coupling without a single resolver.
- Enforce **DAG** (no cycles) or document intentional exceptions; the **unlock check** should be one place (e.g. `TechTreeService` / pure `CanResearch(tech, unlockedSet)`).
- Optional: **alternative** prereqs (“A or B”) as a dedicated field or nested rule type — avoid duplicating OR logic in UI.

### Step 3: Effects

- Split **data** from **application**: definition holds *what* unlocks (e.g. modifiers, unlocked buildings, story flags); a **single applicator** or system applies effects once on research complete.
- Prefer **small effect descriptors** (resource production +10%, unlock `X`, set flag `Y`) over one-off code per tech unless the effect is truly unique.
- Raise **events** after state changes (`TechnologyResearched`, etc.) so UI, diplomacy, and other systems react without tight coupling.

### Step 4: Integration

- **Research flow**: player command → validate cost + prereqs → deduct resources → mark unlocked → **apply effects** → broadcast event.
- **Saves**: persist unlocked tech set (ids or guids), not only scene state.
- Do **not** put prerequisite checks or effect side effects inside UI buttons; UI calls one service/API.

## Unity alignment (this project)

- **Data:** `ScriptableObject` for definitions, costs, prereqs; core logic testable without Play Mode where possible.
- **Coupling:** events or a thin `TechTreeService` facade; other systems subscribe.
- **Scope:** keep tech IDs/costs/prereqs out of random scripts — central definition + one unlock pipeline.

## Quick sanity check

- Can you add a second tech with the **same effect type** without copy-pasting C#? If not, generalize the effect model slightly.
- Is **“can research?”** implemented in **one** place and reused by UI and AI?
