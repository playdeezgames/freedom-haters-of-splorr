# Port plan: VB.NET terminal game -> Odin / js_wasm32

Goal: shippable browser build on itch.io (HTML5 zip channel). The VB code is the design reference, not the target.
Baseline: 297 .NET tests pass (Data 192, Persistence 62, Model 43). Presentation has no tests. "Works" below means *playable*, which only the owner can judge.

## Principles

- Port the **game**, not the layering. The VB stack (Data -> Persistence clients -> Model -> Presentation states) is heavy for what it does; in Odin prefer plain structs + arrays/handles, one `Universe` struct, procs over it.
- Keep the seam: game logic knows nothing about the browser. A thin platform layer provides input (keys), text/tile drawing, and save/load (localStorage via JS bridge).
- Vertical slice first, then breadth. Don't port a system until it's been triaged as worth keeping.
- Deterministic RNG (seeded) so universes and bugs are reproducible.

## Phase 0 - Triage (owner + Claude, before any porting)

Play the .NET build and mark each system **keep / fix / cut** below. Claude can read the VB for each to report what it actually does and what looks unfinished.

| System | VB location | Status | Decision |
|---|---|---|---|
| Main menu / About | Presentation/MainMenu | ? | ? |
| Embark options (faction count, galactic age, density, starting wealth) | Presentation/Embark | ? | ? |
| Universe generation (galaxy, star systems, planets, satellites, orbits, vicinities, wormholes, nexus) | Model/Initializer/Steps | ? | ? |
| Factions + ASC relationship model | Initializer FactionInit/Factionize, README | ? | ? |
| Avatar creation / status / bio | Model/Models/Avatar | ? | ? |
| Movement + tactical navigation / scanner | InPlay/Tactical, Verbs/Movement | ? | ? |
| Encounters | Initializer EncounterInit | ? | ? |
| Inventory + equipment | InPlay/Inventory, Equipment | ? | ? |
| Trading (offers, prices) | InPlay/Trader | ? | ? |
| Dialog | InPlay/Dialog | ? | ? |
| Yokes / vessels | Model/Models/Avatar/Yokes, Vessel | ? | ? |
| SPLORRPedia | InPlay/Informational | ? | ? |
| Save / load (+ "scum" variants) | Presentation/SaveState | ? | ? |
| Game over | InPlay/Informational | ? | ? |
| Music (FHOS_*.mid x4) | sources/originals | ? | ? |

## Phase 1 - Toolchain and skeleton

- [x] Layout: `game/` (Odin package), `web/` (html/css/js glue), `build.sh`, `serve.sh`, `ship.sh`; output in gitignored `dist/`.
- [x] Platform layer: key queue in, 40x25 text grid -> RGBA frame -> canvas out; verified in the browser.
- [x] `ship.sh`: build -> zip -> `butler push ...:html5` (written, not yet run; `shippit.sh` stays until the port is playable).
- [ ] Window focus handling, turn/time clock if needed.
- [ ] HTML button pad (d-pad + Enter/Esc) shown on touch devices, sending the same key codes as the keyboard.
- [x] Font: keep the 8x8 ROM font (swap is a one-file change to `game/font.odin`).
- [ ] Save/load via a localStorage bridge in `web/game.js`. Also persist the Embark settings across sessions (the VB did; the port resets them each visit for now).
- [ ] itch.io: confirm an `html5` channel set to "playable in browser" on the page.

## Phase 2 - Vertical slice

Main menu -> embark -> generate a universe -> place avatar -> move on a map spending fuel and oxygen -> game over -> save/load (scope fixed above). Proves the platform layer, RNG, state-machine/UI pattern and persistence.

## Phase 3 - Systems, in triage order

Port each "keep" system with its own small tests (`odin test`), most-fundamental first: generation -> factions -> inventory/equipment -> trading -> dialog -> encounters -> pedia.

## Phase 4 - Ship

Audio, itch.io page copy (honest, deadpan; screenshots in `ss/`), final butler push, restore the page.

## Decisions (Phase 0, from the code review)

Findings: commodities are dead code (supply/demand throw, never called); factions/ASC only surface in the pedia; military vessels spawn but never act; MIDI files unused; `Equip(slot, Nothing)` throws; refuel/oxygen prices are hardcoded or unlabeled.

- **Visuals:** text grid, closest to the original and the CoCo font.
- **Saves:** new format in browser localStorage; old .NET JSON saves are not imported.
- **Music:** deferred to post-launch; ship silent first.
- **Unequip:** shipyard service only (with fee); the equipment screen stays read-only.
- **Prices:** replace magic numbers (emergency refuel) and show prices on Refuel / Refill Oxygen. No decision needed.
- **Commodities (new design):** keep, as tradeable cargo.
  - Cargo is not hard-capped; weight costs extra fuel/oxygen per move.
  - Supply/demand per trading post comes from planet traits (tech level, faction), plus drift over turns and from the player's own buying/selling.
  - Open: unit/weight values, drift rates, which of Production/Metal/Oxygen/Fuel/Hype are goods vs. modifiers.
- **Factions and patrols (new design):** keep, full scope.
  - Station faction relationship (ASC distance model from README) affects prices and access.
  - Military vessels move and pursue the player when their faction is hostile.
  - Caught: fine/shakedown by default. Combat when provoked: standing past a threshold, resisting a shakedown, or the player attacking first.
  - The .NET game has no combat, so this is new work. Open: combat model, ship stats, patrol AI, standing changes.

## Decisions (Phase 1/2 architecture)

- **Input:** arrows + Enter/Esc on keyboard, plus an HTML button pad for touch/mouse. Menu rows are not tappable; the pad sends ordinary key codes so game code is unaware of it.
- **Screens:** a screen stack. Each screen has `draw` and `on_key` and can push, pop or replace; Escape pops. Replaces the VB `endState` chaining and blocking `ui.Choose`.
- **Data model:** typed structs in arrays with index handles and enums instead of strings (Actor, Group, Item, Location, Map). A rewrite of the data layer rather than a transliteration of the entity/yoke model, so VB tests are a reference for behavior, not code to port.
- **RNG:** seeded, with the seed hidden from the player. Generation and events draw from it; saves store it; tests use fixed seeds.
- **Phase 2 slice scope:** main menu, embark options, universe generation, flying the ship on the map spending fuel and oxygen, death/bankruptcy game over, save/load. No stations, salvage or trading yet.

## Suggested port order

Core survival/trade loop first (fuel, oxygen, jools, salvage, trading, delivery missions, shipyard), then commodities, then faction effects, then patrols and combat last, since it is the largest and least defined.

## Open questions

- Remaining design detail for commodities, patrols and combat (see Decisions).
- Triage of the systems the code review rated "looks complete" still needs the owner's playtest.
