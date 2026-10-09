// Browser glue for the Odin build. Key numbers must match game/keys.odin.
const canvas = document.getElementById("screen");
const ctx = canvas.getContext("2d");
const mem = new odin.WasmMemoryInterface();

const NAMED_KEYS = {
	ArrowUp: 1, ArrowDown: 2, ArrowLeft: 3, ArrowRight: 4,
	Enter: 5, Escape: 6, Tab: 7, Backspace: 8,
};
const keyQueue = [];

document.addEventListener("keydown", (e) => {
	if (e.ctrlKey || e.metaKey || e.altKey) return;
	let code = NAMED_KEYS[e.key];
	if (code === undefined && e.key.length === 1) {
		const c = e.key.charCodeAt(0);
		if (c >= 32 && c < 127) code = c;
	}
	if (code === undefined) return;
	e.preventDefault();
	keyQueue.push(code);
});

odin.runWasm("game.wasm", null, {
	shim: {
		js_next_key: () => keyQueue.shift() ?? 0,
		js_random_u32: () => crypto.getRandomValues(new Uint32Array(1))[0],
		js_present: (ptr, width, height) => {
			const pixels = new Uint8ClampedArray(mem.memory.buffer, ptr, width * height * 4);
			ctx.putImageData(new ImageData(pixels, width, height), 0, 0);
		},
	},
}, mem);
