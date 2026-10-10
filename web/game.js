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

// ---- on-screen pad: the same keys, pressed with a finger or a mouse ----

const REPEAT_DELAY_MS = 350; // holding a direction keeps moving
const REPEAT_EVERY_MS = 110;

function wirePad() {
	if (new URLSearchParams(location.search).has("pad")) document.body.classList.add("pad");

	for (const button of document.querySelectorAll("#pad button")) {
		const code = NAMED_KEYS[button.dataset.key];
		const repeats = button.parentElement.classList.contains("dpad");
		let delay = null;
		let timer = null;

		const stop = () => {
			clearTimeout(delay);
			clearInterval(timer);
			delay = timer = null;
			button.classList.remove("held");
		};
		button.addEventListener("pointerdown", (e) => {
			e.preventDefault();
			if (e.pointerType === "touch") document.body.classList.add("pad");
			keyQueue.push(code);
			button.classList.add("held");
			if (repeats) {
				delay = setTimeout(() => {
					timer = setInterval(() => keyQueue.push(code), REPEAT_EVERY_MS);
				}, REPEAT_DELAY_MS);
			}
		});
		for (const type of ["pointerup", "pointercancel", "pointerleave"]) {
			button.addEventListener(type, stop);
		}
		button.addEventListener("contextmenu", (e) => e.preventDefault());
	}
	// the first touch anywhere reveals the pad, even on devices that report a fine pointer
	document.addEventListener("pointerdown", (e) => {
		if (e.pointerType === "touch") document.body.classList.add("pad");
	}, { once: false });
}
wirePad();

// Saves are bytes in the wasm heap; localStorage holds them as base64 text. Every call is guarded because
// storage can be missing or refuse writes (private windows, blocked site data, a full quota).
const textDecoder = new TextDecoder();
const keyAt = (ptr, len) => textDecoder.decode(new Uint8Array(mem.memory.buffer, ptr, len));
const toBase64 = (bytes) => {
	let text = "";
	for (let i = 0; i < bytes.length; i += 0x8000) {
		text += String.fromCharCode.apply(null, bytes.subarray(i, i + 0x8000));
	}
	return btoa(text);
};
const fromBase64 = (text) => Uint8Array.from(atob(text), (c) => c.charCodeAt(0));
const storedBytes = (key) => {
	try {
		const text = localStorage.getItem(key);
		return text === null ? null : fromBase64(text);
	} catch (e) {
		return null;
	}
};

odin.runWasm("game.wasm", null, {
	shim: {
		js_next_key: () => keyQueue.shift() ?? 0,
		js_random_u32: () => crypto.getRandomValues(new Uint32Array(1))[0],
		js_storage_size: (keyPtr, keyLen) => {
			const bytes = storedBytes(keyAt(keyPtr, keyLen));
			return bytes === null ? -1 : bytes.length;
		},
		js_storage_read: (keyPtr, keyLen, into, intoLen) => {
			const bytes = storedBytes(keyAt(keyPtr, keyLen));
			if (bytes === null || bytes.length !== intoLen) {
				return 0;
			}
			new Uint8Array(mem.memory.buffer, into, intoLen).set(bytes);
			return 1;
		},
		js_storage_write: (keyPtr, keyLen, data, dataLen) => {
			try {
				localStorage.setItem(keyAt(keyPtr, keyLen), toBase64(new Uint8Array(mem.memory.buffer, data, dataLen)));
				return 1;
			} catch (e) {
				return 0;
			}
		},
		js_storage_remove: (keyPtr, keyLen) => {
			try {
				localStorage.removeItem(keyAt(keyPtr, keyLen));
			} catch (e) {}
		},
		js_present: (ptr, width, height) => {
			const pixels = new Uint8ClampedArray(mem.memory.buffer, ptr, width * height * 4);
			ctx.putImageData(new ImageData(pixels, width, height), 0, 0);
		},
	},
}, mem);
