#!/usr/bin/env bash
# Builds the browser version into dist/. Set ODIN to override the compiler path.
set -euo pipefail
cd "$(dirname "$0")"
ODIN="${ODIN:-odin}"
if ! command -v "$ODIN" >/dev/null 2>&1 && [ -x "$HOME/ODIN/odin" ]; then ODIN="$HOME/ODIN/odin"; fi

"$ODIN" test game
rm -rf dist
mkdir -p dist
"$ODIN" build game -target:js_wasm32 -out:dist/game.wasm -o:speed
cp web/index.html web/app.css web/game.js dist/
# odin.js must come from the same Odin version that built the wasm
cp "$("$ODIN" root)/core/sys/wasm/js/odin.js" dist/odin.js
echo "built dist/ ($(du -h dist/game.wasm | cut -f1) wasm)"
