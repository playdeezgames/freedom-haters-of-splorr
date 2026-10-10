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
	// 32 random bits from the browser's crypto source.
	js_random_u32 :: proc() -> u32 ---
	// localStorage, holding bytes as base64. Keys are UTF-8 (pointer, length).
	// Size of the stored value in bytes, or -1 when there is none.
	js_storage_size :: proc(key: [^]u8, key_len: i32) -> i32 ---
	// Copies the stored value into `into`; 1 on success, 0 if it is missing or the wrong size.
	js_storage_read :: proc(key: [^]u8, key_len: i32, into: [^]u8, into_len: i32) -> i32 ---
	// 1 if stored, 0 if the browser refused (private mode, quota).
	js_storage_write :: proc(key: [^]u8, key_len: i32, data: [^]u8, data_len: i32) -> i32 ---
	js_storage_remove :: proc(key: [^]u8, key_len: i32) ---
}

browser_storage :: proc() -> Storage {
	return {
		size = proc(key: string) -> int {
			return int(js_storage_size(raw_data(key), i32(len(key))))
		},
		read = proc(key: string, into: []u8) -> bool {
			return js_storage_read(raw_data(key), i32(len(key)), raw_data(into), i32(len(into))) != 0
		},
		write = proc(key: string, data: []u8) -> bool {
			return js_storage_write(raw_data(key), i32(len(key)), raw_data(data), i32(len(data))) != 0
		},
		remove = proc(key: string) {
			js_storage_remove(raw_data(key), i32(len(key)))
		},
	}
}

random_seed :: proc() -> u64 {
	return u64(js_random_u32()) << 32 | u64(js_random_u32())
}

app: App
frame: Frame

main :: proc() {
	app_init(&app, random_seed, browser_storage())
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
	app_tick(&app)
	render(&app.text, &frame)
	js_present(raw_data(frame[:]), SCREEN_WIDTH, SCREEN_HEIGHT)
	return true
}
