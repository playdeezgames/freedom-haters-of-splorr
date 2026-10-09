# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Status and direction

"Freedom Haters of SPLORR!!" (FHOS) is a satirical terminal game ("Live Free Or Die" is the joke). It was published on itch.io as `thegrumpygamedev/freedom-haters-of-splorr`; the page is **not currently live**. The goal is to get it back to a shippable state **on an Odin → `js_wasm32` (browser) stack**, replacing the current VB.NET/Spectre.Console terminal app. The author's wider direction (games are now vibe-coded with Claude, user is product owner/QA; old versions get removed as web versions replace them) is described in `/home/yermom/git/bok-of-splorr/splorr/Concepts/Metaphor design.md`, which is an additional working directory for this session. Keep that tone: minimal, retro/1-bit, short deadpan text, absurdist; keep the "of SPLORR!!" branding. Don't "fix" intentionally unfair mechanics.

Nothing Odin exists in the repo yet (`odin` is at `/home/yermom/ODIN/odin`). The existing VB.NET code is the reference implementation of the game design; treat it as the spec to port from, not as the shipping target. `README.md` doubles as the design notes (episode checklist, SIGMO-vs-anarchist factions, the "Rules", and the faction ASC model: Authority/Standards/Conviction axes 0–100, relationship = Euclidean distance, 0–25 Friendly, 26–50 Neutral, 51+ Hostile).

## Current (legacy) .NET stack

All code is VB.NET under `src/`, one solution `src/src.sln`, SDK 10.0 (`net10.0` for the exe and test projects, `netstandard2.1` for libraries). 451 of ~490 tracked files are `.vb`.

```bash
dotnet build src/src.sln
dotnet test src/src.sln                                   # xunit.v3 + Shouldly
dotnet test src/FHOS.Model.Tests                          # one test project
dotnet test src/FHOS.Model.Tests --filter "FullyQualifiedName~ActorModel_should"   # one class / method
dotnet run --project src/FHOS/FHOS.vbproj                 # needs a real terminal (Spectre.Console, ReadKey)
```

`shippit.sh` publishes single-file self-contained binaries for linux-x64/win-x64/osx-x64 and `butler push`es them to itch.io, then commits. It will need replacing with a wasm build + `butler push` of an HTML5 zip channel.

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
