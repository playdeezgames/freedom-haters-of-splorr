# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Status and direction

"Freedom Haters of SPLORR!!" (FHOS) is a satirical terminal game ("Live Free Or Die" is the joke). It was published on itch.io as `thegrumpygamedev/freedom-haters-of-splorr`; the page is **not currently live**. The goal is to get it back to a shippable state **on an Odin → `js_wasm32` (browser) stack**, replacing the current VB.NET/Spectre.Console terminal app. The author's wider direction (games are now vibe-coded with Claude, user is product owner/QA; old versions get removed as web versions replace them) is described in `/home/yermom/git/bok-of-splorr/splorr/Concepts/Metaphor design.md`, which is an additional working directory for this session. Keep that tone: minimal, retro/1-bit, short deadpan text, absurdist; keep the "of SPLORR!!" branding. Don't "fix" intentionally unfair mechanics.

The Odin toolchain skeleton exists (see "Odin / wasm stack" below); no game logic has been ported yet. The existing VB.NET code is the reference implementation of the game design; treat it as the spec to port from, not as the shipping target. `README.md` doubles as the design notes (episode checklist, SIGMO-vs-anarchist factions, the "Rules", and the faction ASC model: Authority/Standards/Conviction axes 0–100, relationship = Euclidean distance, 0–25 Friendly, 26–50 Neutral, 51+ Hostile).

## Odin / wasm stack (the future)

Odin is at `/home/yermom/ODIN/odin` (not on PATH; `build.sh` finds it).

```bash
./build.sh     # runs `odin test game`, builds dist/ (game.wasm, odin.js copied from the same Odin install, html/css/js)
./serve.sh     # http://localhost:8000 serving dist/ (wasm needs http, not file://); restart it after build.sh, which recreates dist/
odin test game                                  # native tests; whole suite
odin test game -define:ODIN_TEST_NAMES=game.render_draws_foreground_and_background   # one test
./ship.sh      # zips dist/ and `butler push`es the html5 channel. Publishes: run only when asked.
```

- `game/` is one Odin package. Everything is platform-independent except `platform_js.odin` (`#+build js`), which exports `step(dt: f64)` (odin.js calls it every animation frame) and imports `js_next_key` / `js_present` from `web/game.js`. Test files carry `#+build !js` because `core:testing` doesn't compile for wasm.
- UI is a 40x25 `Text_Buffer` of colored characters rasterized by `render` into a 320x200 RGBA `Frame` using the 8x8 bitmap `font` (one u64 per glyph, generated from the ROM font in `~/git/odin-wasm-framebuffer`); JS blits it once per frame. New game logic should stay platform-independent and testable natively.
- Screens: `Screen` (in `screens.odin`) is a union of per-screen structs holding their own state. `screen_draw` / `screen_key` dispatch on it; `screen_key` returns a `Transition` (`Push`, `Replace`, `Pop`, or nil to stay) which `stack_apply` applies to the fixed-size `Screen_Stack` in `App`. To add a screen: add its struct to the union, a draw proc and a key proc, and a case in both dispatchers. `menu.odin` has the shared cursor-menu helpers. The 8x8 font has no line gap, so menus and text use every other row.
- RNG (`rng.odin`): `Rng` is splitmix64, deliberately not `core:math/rand`, so a seed means the same universe forever (`sequence_is_pinned` guards this; changing the algorithm invalidates saves). All randomness goes through an `^Rng` passed in, never a global. Dice notation is the VB's (`"10d11+-10d1"`, `"12d6/6"`, `"1d6*10"`): `dice_parse` returns ok, `dice_roll` asserts on a bad literal, `dice_bounds` gives exact min/max for tests.
- Data model (`universe.odin`, `world_types.odin`): `Universe` owns dynamic arrays of Faction, Star_System, Planet, Satellite, Map and Actor, addressed by distinct 1-based ids (0 = none). Maps are sparse: a `Map` is a kind (fixed size in `map_sizes`), its owning actor, and the actors on it; any cell no actor covers is empty. An `Actor` has a center `pos`, an odd `size` footprint (planets 3x3 / 5x5, satellites 3x3), and an `interior` map that stepping into it leads to, so places nest galaxy > star system > planet vicinity > planet orbit. Names are fixed 32-byte `Name`s. Type tables (`planet_info`, `star_info`, ...) carry the VB descriptor data. `Hue` has CGA colors plus Orange/Pink/Tan because planet types need them.
- Keys cross the boundary as an i32: printable ASCII as its char code, others per `game/keys.odin`; `web/game.js` must use the same numbers.
- `~/git/odin-wasm-framebuffer`, `~/git/odin-metaphor` and `~/git/odin-webasm-sandbox` are the author's earlier Odin/wasm experiments. `PORT_PLAN.md` holds the port phases and design decisions.

## Legacy .NET stack

All code is VB.NET under `src/`, one solution `src/src.sln`, SDK 10.0 (`net10.0` for the exe and test projects, `netstandard2.1` for libraries). 451 of ~490 tracked files are `.vb`.

```bash
dotnet build src/src.sln
dotnet test src/src.sln                                   # xunit.v3 + Shouldly
dotnet test src/FHOS.Model.Tests                          # one test project
dotnet test src/FHOS.Model.Tests --filter "FullyQualifiedName~ActorModel_should"   # one class / method
dotnet run --project src/FHOS/FHOS.vbproj                 # needs a real terminal (Spectre.Console, ReadKey)
```

`shippit.sh` publishes single-file self-contained binaries for linux-x64/win-x64/osx-x64 and `butler push`es them to itch.io, then commits. `ship.sh` is its html5 replacement; delete `shippit.sh` once the port is playable.

### Layering (dependencies point downward)

```
FHOS (exe, Program.vb) → FHOS.Presentation → FHOS.Model → FHOS.Persistence → FHOS.Data
                                  │               └─→ SPLORR.Game (RNG, maze gen, utilities)
                                  └─→ SPLORR.Presentation.Spectre → SPLORR.Presentation (IUIContext, Mood)
```

- **FHOS.Data**: plain serializable data records (`UniverseData` and `*Data` for Actor/Group/Item/Location/Map/Store). The whole game state is one `UniverseData` graph, serialized with System.Text.Json (this is the save format). Entities reference each other by integer id, not object reference.
- **FHOS.Persistence**: `I*` interfaces + internal classes that are thin *clients* over `UniverseData` + an entity id (`ItemDataClient`/`Item` pattern: look up by id in the universe dictionaries, delete by removing from them). Per-entity folders also hold sub-aspects (actor equipment, inventory, offers, prices, yokes). Interfaces are public; implementations are `Friend`, built via `FromId`.
- **FHOS.Model**: game rules over persistence (`UniverseModel` is the root; Actor/Avatar/Group/Item/Location models). `Initializer/` builds a new universe step by step (`IInitializer` + `Steps/`).
- **FHOS.Presentation**: the UI as a state machine. `IState` / `BaseState` / `BoardState`; each state's `Start` runs until it hands off to the next. Folders map to screens: `MainMenu`, `Embark` (new game/generate), `SaveState` (+ "Scum" variants), `InPlay` (neutral, tactical, trader, dialog, inventory, equipment, verbs), and `Grimoire/` which centralizes all user-facing strings (prompts, messages, choices, key names). Put new copy there, not inline.
- **SPLORR.Presentation**: UI abstraction (`IUIContext`: Choose/Confirm/Ask/Message/Write/ReadKey, tagged with a `Mood`). Presentation code talks only to this interface; `SPLORR.Presentation.Spectre` is the one implementation. This seam is where a browser/canvas front end plugs in.

Tests mirror the layers (`FHOS.Data.Tests`, `FHOS.Persistence.Tests`, `FHOS.Model.Tests`); files are named `<Type>_should.vb`.

### Gotchas

- `schema/ecs.sql` and `schema.db` are an old MariaDB/HeidiSQL entity-component schema dump; the current code does not use a database.
- Assets: `sources/originals` (fonts, tileset, `FHOS_*.mid` music) and `sources/inputs` are raw sources; `ss/` holds itch.io screenshots/cover/icon. `src/FHOS/SIGMO.png` is the splash image loaded at runtime from the working directory.
