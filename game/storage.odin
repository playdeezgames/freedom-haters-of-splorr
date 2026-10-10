package game

// Where saves live. The game only knows this small interface; the browser build backs it with localStorage
// (see platform_js.odin and web/game.js) and the tests with memory.

Storage :: struct {
	// The stored size of `key` in bytes, or -1 if there is nothing there.
	size:   proc(key: string) -> int,
	// Copies what is stored into `into` (which must be `size` bytes) and says whether it worked.
	read:   proc(key: string, into: []u8) -> bool,
	// Stores `data` under `key`; false if it could not be stored (private browsing, a full disk).
	write:  proc(key: string, data: []u8) -> bool,
	remove: proc(key: string),
}

// Reads a whole value. The caller frees the result.
storage_get :: proc(s: Storage, key: string) -> (data: []u8, ok: bool) {
	if s.size == nil {
		return nil, false
	}
	n := s.size(key)
	if n < 0 {
		return nil, false
	}
	data = make([]u8, n)
	if !s.read(key, data) {
		delete(data)
		return nil, false
	}
	return data, true
}

storage_has :: proc(s: Storage, key: string) -> bool {
	return s.size != nil && s.size(key) >= 0
}

// ---- Slots ----
//
// Slot 0 is the "Scum Slot" for quick saves and loads; slots 1 to 5 are the named ones. Next to each save is a
// short description ("Turn 57, 910 jools") so the lists can say what is in a slot without loading it.

SLOT_COUNT :: 6
SCUM_SLOT :: 0

slot_names := [SLOT_COUNT]string{"Scum Slot", "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Slot 5"}

slot_key :: proc(slot: int) -> string {
	keys := [SLOT_COUNT]string{"fhos.slot0", "fhos.slot1", "fhos.slot2", "fhos.slot3", "fhos.slot4", "fhos.slot5"}
	return keys[slot]
}

slot_info_key :: proc(slot: int) -> string {
	keys := [SLOT_COUNT]string{"fhos.slot0.info", "fhos.slot1.info", "fhos.slot2.info", "fhos.slot3.info", "fhos.slot4.info", "fhos.slot5.info"}
	return keys[slot]
}

SETTINGS_KEY :: "fhos.embark"

slot_exists :: proc(s: Storage, slot: int) -> bool {
	return storage_has(s, slot_key(slot))
}

Save_Result :: enum {
	Saved,
	Could_Not_Store,
}

slot_save :: proc(s: Storage, slot: int, u: ^Universe) -> Save_Result {
	if s.write == nil {
		return .Could_Not_Store
	}
	data := universe_to_bytes(u)
	defer delete(data)
	if !s.write(slot_key(slot), data) {
		return .Could_Not_Store
	}
	digits1, digits2: [20]u8
	info := long_join("Turn ", int_text(&digits1, u.turn), ", ", int_text(&digits2, u.avatar.jools), " jools")
	s.write(slot_info_key(slot), transmute([]u8)long_str(&info)) // a missing description is not worth failing over
	return .Saved
}

// What is in a slot, as a short line; empty if nothing is stored or the note is missing.
slot_description :: proc(s: Storage, slot: int) -> Long_Text {
	data, ok := storage_get(s, slot_info_key(slot))
	if !ok {
		return {}
	}
	defer delete(data)
	return long_join(string(data))
}

Load_Result :: struct {
	universe: Universe,
	error:    Load_Error,
	missing:  bool, // nothing in that slot
}

slot_load :: proc(s: Storage, slot: int) -> (result: Load_Result) {
	data, ok := storage_get(s, slot_key(slot))
	if !ok {
		result.missing = true
		return
	}
	defer delete(data)
	result.universe, result.error = universe_from_bytes(data)
	return
}
