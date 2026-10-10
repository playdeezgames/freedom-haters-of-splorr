package game

// Saving and loading from the menus: the Scum Slot for quick saves, and slots 1 to 5.

// Writes the game to `slot` and says how it went, popping `pops` screens first so a good save lands back on the map.
save_to_slot :: proc(session: ^Session, slot: int, pops: int) -> Transition {
	switch slot_save(session.storage, slot, &session.universe) {
	case .Saved:
		return Pop_Then{pops, message_make(.Light_Green, "Game Saved!")}
	case .Could_Not_Store:
		return Pop_Then{pops, message_make(.Light_Red, "Could not save!", "The browser would not store it.")}
	}
	return nil
}

// Replaces the current game with the one in `slot`, or says why it can't. `replacing` is how many screens to
// pop for a message when it fails (0 pushes the message on top).
load_slot :: proc(session: ^Session, slot: int, replacing: int) -> Transition {
	result := slot_load(session.storage, slot)
	switch {
	case result.missing:
		return Pop_Then{replacing, message_make(.Light_Red, "No Scum Slot!" if slot == SCUM_SLOT else "Nothing is saved there!")}
	case result.error == .Other_Version:
		return Pop_Then{replacing, message_make(.Light_Red, "That save is from another", "version of the game.")}
	case result.error != .None:
		return Pop_Then{replacing, message_make(.Light_Red, "That save is damaged.")}
	}
	session_end(session)
	session.universe = result.universe
	session.in_play = true
	return Reset{screen = Main_Menu{}, on_top = Navigation{}}
}

// The Load list, or a message if there is nothing to load.
open_load_screen :: proc(session: ^Session) -> Transition {
	for slot in 0 ..< SLOT_COUNT {
		if slot_exists(session.storage, slot) {
			return Push{Load_Screen{}}
		}
	}
	return Push{message_make(.Light_Red, "No Saves Exist!")}
}

// ---- Save ----

Save_Screen :: struct {
	cursor: int,
}

MAX_SLOT_ROWS :: SLOT_COUNT + 1

slot_row_texts :: proc(session: ^Session, slots: []int, include_scum: bool, labels, details: ^[MAX_SLOT_ROWS]Long_Text) {
	for slot, i in slots {
		exists := slot_exists(session.storage, slot)
		labels[i + 1] = long_join(slot_names[slot], " (will overwrite)" if exists else "")
		if exists {
			details[i + 1] = slot_description(session.storage, slot)
		}
	}
	labels[0] = long_join("Cancel")
}

save_slots := [?]int{1, 2, 3, 4, 5}

save_screen_draw :: proc(s: ^Save_Screen, tb: ^Text_Buffer, session: ^Session) {
	text_put_centered(tb, 3, "SAVE GAME", .Yellow)
	label_texts, detail_texts: [MAX_SLOT_ROWS]Long_Text
	slot_row_texts(session, save_slots[:], false, &label_texts, &detail_texts)
	labels, details: [MAX_SLOT_ROWS]string
	for i in 0 ..< len(save_slots) + 1 {
		labels[i] = long_str(&label_texts[i])
		details[i] = long_str(&detail_texts[i])
	}
	menu_draw(tb, 6, labels[:len(save_slots) + 1], s.cursor, 2, details[:len(save_slots) + 1])
}

save_screen_key :: proc(s: ^Save_Screen, key: Key, session: ^Session) -> Transition {
	switch menu_key(&s.cursor, len(save_slots) + 1, key) {
	case .Chosen:
		if s.cursor == 0 {
			return Pop{}
		}
		return save_to_slot(session, save_slots[s.cursor - 1], pops = 2) // this screen and the game menu
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- Load ----

Load_Screen :: struct {
	cursor: int,
}

// The slots that have something in them, scum slot first.
loadable_slots :: proc(session: ^Session) -> (slots: [SLOT_COUNT]int, count: int) {
	for slot in 0 ..< SLOT_COUNT {
		if slot_exists(session.storage, slot) {
			slots[count] = slot
			count += 1
		}
	}
	return
}

load_screen_draw :: proc(s: ^Load_Screen, tb: ^Text_Buffer, session: ^Session) {
	text_put_centered(tb, 3, "LOAD GAME", .Yellow)
	slots, count := loadable_slots(session)
	label_texts, detail_texts: [MAX_SLOT_ROWS]Long_Text
	label_texts[0] = long_join("Cancel")
	for i in 0 ..< count {
		label_texts[i + 1] = long_join(slot_names[slots[i]])
		detail_texts[i + 1] = slot_description(session.storage, slots[i])
	}
	labels, details: [MAX_SLOT_ROWS]string
	for i in 0 ..< count + 1 {
		labels[i] = long_str(&label_texts[i])
		details[i] = long_str(&detail_texts[i])
	}
	s.cursor = min(s.cursor, count)
	menu_draw(tb, 6, labels[:count + 1], s.cursor, 2, details[:count + 1])
}

load_screen_key :: proc(s: ^Load_Screen, key: Key, session: ^Session) -> Transition {
	slots, count := loadable_slots(session)
	switch menu_key(&s.cursor, count + 1, key) {
	case .Chosen:
		if s.cursor == 0 || s.cursor > count {
			return Pop{}
		}
		return load_slot(session, slots[s.cursor - 1], replacing = 1) // a failure replaces this list
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}
