#!/usr/bin/env bash
# Serves dist/ at http://localhost:8000 (wasm needs http, not file://).
cd "$(dirname "$0")/dist" && exec python3 -m http.server 8000
