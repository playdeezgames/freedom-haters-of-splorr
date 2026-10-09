# Port plan: VB.NET terminal game -> Odin / js_wasm32

Goal: shippable browser build on itch.io (HTML5 zip channel). The VB code is the design reference, not the target. The itch page is live today with native v56 builds (Windows/Linux/Mac) and no html5 channel; an html5 push adds a channel, and whether to retire the native builds is the owner's call.
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
- [x] HTML button pad (d-pad + Enter/Esc) in `web/`: shown on touch screens (`pointer: coarse`), after any touch, or with `?pad`; holding a direction repeats after 350 ms every 110 ms. It pushes the same key codes as the keyboard, so game code is unaware of it. Verified with mouse clicks and the phone preset; hold-to-repeat and real touch are not yet verified on a device.
- [x] Font: keep the 8x8 ROM font (swap is a one-file change to `game/font.odin`).
- [ ] Save/load via a localStorage bridge in `web/game.js`. Also persist the Embark settings across sessions (the VB did; the port resets them each visit for now).
- [ ] itch.io: the page exists (native v56 builds, no html5 channel). Pushing `html5` and marking it "playable in browser" is an owner action; decide whether to keep or retire the native downloads.

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

## Decisions (universe data model and generation)

Based on a read of the 12 VB generation steps (factions -> galaxy -> star systems -> planet vicinity/orbit -> satellite orbit -> encounters -> nexus/wormholes -> factionize -> avatar). The VB nests maps: every station/planet/star system is an actor whose "interior" is another map, entered at a random edge cell. Sizes: Galaxy 63x63, Nexus 63x63, Star System 31x31, Star Vicinity 15x15, Planet Vicinity 15x15, Planet Orbit 11x11, Satellite Orbit 9x9.

- **Map storage:** sparse. A map is dimensions plus the actors on it; empty cells are implicit void and edge cells are computed. (The VB allocated a location per cell, ~600,000 for a default galaxy.)
- **Big bodies:** one actor with a square footprint (planet 3x3 in a vicinity and 5x5 in its orbit, satellite 3x3) instead of 9-25 section actors; bumping any footprint cell interacts with the body.
- **Timing:** stepwise generation, one step per frame, with a progress screen showing the step name and count (as the VB does). Never freezes the tab.
- **Slice scope:** places only: factions (SIGMO plus N random, with ASC stats and 3 values each), the galaxy of star systems (rejection-sampled with the density spacing, star type from the age weights, unique pronounceable names), star systems with star and planets (distance by star type, 2d6 max count, 15 planet types, tech level 2d6-2), planet vicinities with satellites, faction assignment, and the player ship (random void cell in the galaxy, SIGMO home planet, Mark I fuel and life support, wallet from the wealth roll). Not in this slice: the nexus and wormholes, stations, debris, military ships.

## Decisions (refilling, from reading the VB)

- Live game sources: Star Dock (one per planet orbit; oxygen 1 jool per 10, fuel 1 jool per 3), Atmospheric Concentrator item (free oxygen at any planet, 5,000 jools, tech level 3), fuel scoop item (free fuel at stars), oxygen tank items (auto-refill). The "can refill oxygen" flag on 9 planet types is dead data in the VB.
- **Port:** Star Docks with priced Refill Oxygen / Refuel (the VB hid prices behind a TODO). Free planet oxygen requires the equipped concentrator AND a breathable planet type (owner's choice; the VB allowed any planet). The concentrator can't be obtained until the shop is ported.
- Not yet ported from this area: fuel scoop at stars, oxygen tanks, the shop/shipyard, delivery missions at the star dock.

## Decisions (before the shop work)

- **Emergency refuel price:** keep the VB's 1 jool per fuel (3x the dock price) as a deliberate emergency premium, now the named constant `EMERGENCY_FUEL_PRICE`.
- **Order of the next stretch:** HTML touch pad, then the economy (debris and salvage, then the trading post: sell scrap and buy items, then the shipyard: install/uninstall), then save/load.
- **Items:** individual item records (kind, mark, per-item numbers such as a remembered tank level, delivery destination and reward) in a pool, with inventory and equipment slots holding ids; the UI groups identical items into stacks as the VB does.

## Economy progress

- [x] Items (individual records), inventory screen with stacks and item pages
- [x] Debris (12d6/6 per system, 4d6 scrap each) and salvage
- [x] Trading post: buy list by planet tech level, sell scrap at 1, quantity / number / confirm screens; 1-2 per orbit
- [x] Using oxygen tanks (+100 oxygen, auto-used when oxygen would run out, leaves scrap) and fuel rods (+100 fuel)
- [x] Shipyard: slots, install/uninstall/swap with fees, tech-level gate, per-unit levels, equipment view
- [x] Fuel scoop at stars (free fuel, installed accessory)
- [ ] Delivery missions at the star dock (Errand Boy), selling/buying them

## Decisions (shipyard)

- Fuel and life-support units keep their own level, so a fresh unit is a refill and swapping back restores the old level (the VB's behavior, kept).
- The shipyard's planet tech level must reach both the unit being installed and the unit being removed (the VB's gate, kept).
- One slot list instead of the VB's Change / Install / Uninstall menus.
- Correction made along the way: life support Marks I-V need tech levels 1/3/5/7/9 (fuel supplies 1-5); trading posts stock accordingly.

## Suggested port order

Core survival/trade loop first (fuel, oxygen, jools, salvage, trading, delivery missions, shipyard), then commodities, then faction effects, then patrols and combat last, since it is the largest and least defined.

## Open questions

- Remaining design detail for commodities, patrols and combat (see Decisions).
- Triage of the systems the code review rated "looks complete" still needs the owner's playtest.
