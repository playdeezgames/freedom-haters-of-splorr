#!/usr/bin/env bash
# Zips dist/ and pushes it to itch.io as the html5 channel. Run ./build.sh first.
# Not run automatically: this publishes. (Replaces the old dotnet publish in shippit.sh once the port is playable.)
set -euo pipefail
cd "$(dirname "$0")"
[ -f dist/game.wasm ] || { echo "run ./build.sh first" >&2; exit 1; }
rm -f fhos-html5.zip
(cd dist && zip -qr ../fhos-html5.zip .)
butler push fhos-html5.zip thegrumpygamedev/freedom-haters-of-splorr:html5
