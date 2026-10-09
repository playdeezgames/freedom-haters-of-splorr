#+build js
package game

// Browser platform layer: exports `step` (called by odin.js every animation frame) and
// talks to web/game.js through the "shim" imports.

foreign import shim "shim"

@(default_calling_convention = "contextless")
foreign shim {
	// Returns the next queued key (see keys.odin), or 0 when the queue is empty.
	js_next_key :: proc() -> i32 ---
	// Blits an RGBA frame of the given size onto the canvas.
	js_present :: proc(pixels: [^]u8, width, height: i32) ---
}

app: App
frame: Frame

main :: proc() {
	app_init(&app)
}

@(export)
step :: proc(dt: f64) -> (keep_going: bool) {
	for {
		key := Key(js_next_key())
		if key == KEY_NONE {
			break
		}
		app_key(&app, key)
	}
	render(&app.text, &frame)
	js_present(raw_data(frame[:]), SCREEN_WIDTH, SCREEN_HEIGHT)
	return true
}
