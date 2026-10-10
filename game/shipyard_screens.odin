package game

// The ship's equipment: a read-only view from the action menu, and the shipyard where it is changed.
// One slot list (decided in PORT_PLAN.md) replaces the VB's separate Change / Install / Uninstall menus.

slot_label :: proc(u: ^Universe, slot: Equip_Slot) -> Long_Text {
	name := slot_item_name(u, slot)
	return long_join(slot_info[slot].name, ": ", name_str(&name))
}

// ---- Equipment (view only) ----

Equipment_Screen :: struct {
	cursor: int,
}

equipment_draw :: proc(s: ^Equipment_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	text_put_centered(tb, 1, "EQUIPMENT", .Yellow)
	texts: [len(Equip_Slot)]Long_Text
	labels: [len(Equip_Slot) + 1]string
	labels[0] = "Back"
	for slot in Equip_Slot {
		texts[slot] = slot_label(u, slot)
		labels[int(slot) + 1] = long_str(&texts[slot])
	}
	menu_draw(tb, 4, labels[:], s.cursor, 2)
}

equipment_key :: proc(s: ^Equipment_Screen, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	switch menu_key(&s.cursor, len(Equip_Slot) + 1, key) {
	case .Chosen:
		if s.cursor == 0 {
			return Pop{}
		}
		if id := u.avatar.equipment[Equip_Slot(s.cursor - 1)]; id != 0 {
			item := item_get(u, id)
			return Push{Item_Page{kind = item.kind, mark = item.mark}}
		}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- Shipyard ----

draw_yard_header :: proc(tb: ^Text_Buffer, u: ^Universe, yard: Actor_Id) {
	p := planet_get(u, actor_get(u, yard).planet)
	c := text_put(tb, (TEXT_COLUMNS - len(name_str(&p.name)) - len(" Shipyard")) / 2, 1, name_str(&p.name), .Orange)
	text_put(tb, c, 1, " Shipyard", .Orange)
	put_field_int(tb, 2, 3, "Jools", u.avatar.jools)
}

Shipyard_Screen :: struct {
	yard:   Actor_Id,
	cursor: int,
}

shipyard_draw :: proc(s: ^Shipyard_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	draw_yard_header(tb, u, s.yard)
	text_put(tb, 2, 5, "Pick a slot to change:", .Dark_Gray)
	texts: [len(Equip_Slot) + 1]Long_Text
	labels: [len(Equip_Slot) + 2]string
	digits: [20]u8
	for slot in Equip_Slot {
		texts[slot] = slot_label(u, slot)
		labels[slot] = long_str(&texts[slot])
	}
	if top_off_amount(u.avatar.hull) > 0 {
		texts[len(Equip_Slot)] = long_join("Repair Hull (", int_text(&digits, hull_repair_price(u)), " jools)")
	} else {
		texts[len(Equip_Slot)] = long_join("Repair Hull (sound)")
	}
	labels[len(Equip_Slot)] = long_str(&texts[len(Equip_Slot)])
	labels[len(Equip_Slot) + 1] = "Leave"
	menu_draw(tb, 7, labels[:], s.cursor, 2)
}

shipyard_key :: proc(s: ^Shipyard_Screen, key: Key, session: ^Session) -> Transition {
	switch menu_key(&s.cursor, len(Equip_Slot) + 2, key) {
	case .Chosen:
		if s.cursor == len(Equip_Slot) + 1 {
			return Pop{}
		}
		if s.cursor == len(Equip_Slot) {
			digits: [20]u8
			result, mended, cost := shipyard_repair(&session.universe)
			switch result {
			case .Nothing_To_Repair:
				return Push{message_make(.Light_Gray, "The hull is sound.")}
			case .Insufficient_Funds:
				return Push{message_make(.Light_Red, "Insufficient funds!")}
			case .Repaired:
				m := message_make(.Orange, "Hull Repaired!")
				message_add(&m, .Light_Gray, "You mend ", int_text(&digits, mended), " hull.")
				d2: [20]u8
				message_add(&m, .Light_Gray, "You paid ", int_text(&d2, cost), " Jools.")
				return Push{m}
			}
		}
		return Push{Slot_Items{yard = s.yard, slot = Equip_Slot(s.cursor)}}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- One slot: what can go in it ----

Slot_Items :: struct {
	yard:   Actor_Id,
	slot:   Equip_Slot,
	cursor: int,
}

Slot_Entry_Kind :: enum {
	Cancel,
	Uninstall,
	Install,
}

Slot_Entry :: struct {
	kind: Slot_Entry_Kind,
	item: Item_Id,
}

MAX_SLOT_ENTRIES :: MAX_INSTALLABLE + 2

slot_entries :: proc(u: ^Universe, slot: Equip_Slot, entries: ^[MAX_SLOT_ENTRIES]Slot_Entry) -> (count: int) {
	entries[count] = {.Cancel, 0}
	count += 1
	if !slot_info[slot].mandatory && u.avatar.equipment[slot] != 0 {
		entries[count] = {.Uninstall, 0}
		count += 1
	}
	list := installable_items(u, slot)
	for i in 0 ..< list.count {
		entries[count] = {.Install, list.items[i]}
		count += 1
	}
	return
}

// The text of every entry: a label and the line under it.
slot_entry_texts :: proc(u: ^Universe, slot: Equip_Slot, entries: []Slot_Entry, labels, details: ^[MAX_SLOT_ENTRIES]Long_Text) {
	for e, i in entries {
		d1, d2, d3: [20]u8
		switch e.kind {
		case .Cancel:
			labels[i] = long_join("Cancel")
		case .Uninstall:
			item := item_get(u, u.avatar.equipment[slot])
			name := item_name(item^)
			labels[i] = long_join("Uninstall ", name_str(&name))
			details[i] = long_join("Fee ", int_text(&d1, item_uninstall_fee(item^)))
		case .Install:
			item := item_get(u, e.item)
			name := item_name(item^)
			labels[i] = long_join(name_str(&name))
			fee := change_fee(u, slot, e.item)
			if item.kind == .Fuel_Supply || item.kind == .Life_Support {
				details[i] = long_join("Holds ", int_text(&d1, item.level), "/", int_text(&d2, CAPACITY_PER_MARK * item.mark), ". Fee ", int_text(&d3, fee))
			} else {
				details[i] = long_join("Fee ", int_text(&d1, fee))
			}
		}
	}
}

slot_items_draw :: proc(s: ^Slot_Items, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	draw_yard_header(tb, u, s.yard)
	text := slot_label(u, s.slot)
	text_put(tb, 2, 5, long_str(&text), .White)

	entries: [MAX_SLOT_ENTRIES]Slot_Entry
	count := slot_entries(u, s.slot, &entries)
	s.cursor = min(s.cursor, count - 1)
	label_texts, detail_texts: [MAX_SLOT_ENTRIES]Long_Text
	slot_entry_texts(u, s.slot, entries[:count], &label_texts, &detail_texts)
	labels, details: [MAX_SLOT_ENTRIES]string
	for i in 0 ..< count {
		labels[i] = long_str(&label_texts[i])
		details[i] = long_str(&detail_texts[i])
	}
	if count == 1 {
		text_put(tb, 2, 9, "Nothing in yer hold fits.", .Dark_Gray)
	}
	menu_draw(tb, 7, labels[:count], s.cursor, 2, details[:count])
}

slot_items_key :: proc(s: ^Slot_Items, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	entries: [MAX_SLOT_ENTRIES]Slot_Entry
	count := slot_entries(u, s.slot, &entries)
	switch menu_key(&s.cursor, count, key) {
	case .Chosen:
		e := entries[s.cursor]
		if e.kind == .Cancel {
			return Pop{}
		}
		change := shipyard_change(u, s.yard, s.slot, e.item if e.kind == .Install else 0)
		return Replace{change_message(u, s.slot, change)}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

change_message :: proc(u: ^Universe, slot: Equip_Slot, change: Change) -> Message {
	digits: [20]u8
	switch change.result {
	case .Insufficient_Tech:
		return message_make(.Light_Red, "Insufficient Tech Level!")
	case .Insufficient_Funds:
		return message_make(.Light_Red, "Insufficient funds!")
	case .Not_Allowed:
		return message_make(.Light_Red, "That can't be done.")
	case .Done:
	}
	m: Message
	if change.removed != 0 {
		name := item_name(item_get(u, change.removed)^)
		message_add(&m, .Light_Gray, "Uninstalled ", name_str(&name), " from ", slot_info[slot].name, ".")
	}
	if change.installed != 0 {
		name := item_name(item_get(u, change.installed)^)
		message_add(&m, .Light_Gray, "Installed ", name_str(&name), " on ", slot_info[slot].name, ".")
	}
	if change.fee > 0 {
		message_add(&m, .Light_Gray, "Paid Fees: ", int_text(&digits, change.fee))
	}
	message_add(&m, .Light_Gray, "Jools: ", int_text(&digits, u.avatar.jools))
	return m
}
