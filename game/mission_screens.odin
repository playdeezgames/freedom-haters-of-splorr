package game

// The mission screens: a star dock's offer, the receipt for a completed delivery, abandoning one, and the
// Status screen that shows where you stand (reputation drives how many deliveries you may carry).

MAX_MISSION_LINES :: 5

// Where a delivery is going, as lines of text. The destination's star system and galaxy position stand in for
// the pedia lookup that the live game uses to find a planet by name.
mission_lines :: proc(u: ^Universe, m: Mission, lines: ^[MAX_MISSION_LINES]Long_Text) -> int {
	planet := planet_get(u, m.destination)
	system := star_system_get(u, planet.star_system)
	d1, d2, d3: [20]u8
	item := mission_item_name(m)
	who := mission_recipient(m)
	lines[0] = long_join("Item: ", long_str(&item))
	lines[1] = long_join("Destination: ", name_str(&planet.name))
	lines[2] = long_join("System: ", name_str(&system.name), " at ", int_text(&d1, system.position.x), ",", int_text(&d2, system.position.y))
	lines[3] = long_join("Recipient: ", long_str(&who))
	lines[4] = long_join("Jools Reward: ", int_text(&d3, m.reward))
	return MAX_MISSION_LINES
}

// ---- A dock's offer ----

Mission_Offer :: struct {
	dock:   Actor_Id,
	cursor: int,
}

mission_offer_labels :: proc(u: ^Universe, dock: Actor_Id, labels: ^[2]string) -> (count: int) {
	if can_accept_mission(u, dock) {
		labels[count] = "Accept"
		count += 1
	}
	labels[count] = "Cancel"
	count += 1
	return
}

mission_offer_draw :: proc(s: ^Mission_Offer, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	planet := planet_get(u, actor_get(u, s.dock).planet)
	text_put(tb, 2, 1, "Delivery from:", .Orange)
	text_put(tb, 2, 2, name_str(&planet.name), .Orange)

	offer := actor_get(u, s.dock).offer
	row := 4
	if offer != 0 {
		lines: [MAX_MISSION_LINES]Long_Text
		n := mission_lines(u, item_get(u, offer).mission, &lines)
		for i in 0 ..< n {
			row += text_put_wrapped(tb, 2, row, TEXT_COLUMNS - 4, long_str(&lines[i]), .Light_Gray)
		}
		row += 1
		if !can_add_delivery(u, s.dock) {
			row += text_put_wrapped(tb, 2, row, TEXT_COLUMNS - 4, "Based on yer current reputation, you cannot take on more deliveries.", .Yellow) + 1
		}
		if needs_deposit(u, s.dock) {
			d: [20]u8
			text := long_join("Based on yer current reputation, you must pay a deposit of ", int_text(&d, deposit_for(u, s.dock, offer)), ".")
			row += text_put_wrapped(tb, 2, row, TEXT_COLUMNS - 4, long_str(&text), .Yellow) + 1
		}
	}
	labels: [2]string
	count := mission_offer_labels(u, s.dock, &labels)
	menu_draw(tb, max(row + 1, 17), labels[:count], s.cursor)
}

mission_offer_key :: proc(s: ^Mission_Offer, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	labels: [2]string
	count := mission_offer_labels(u, s.dock, &labels)
	switch menu_key(&s.cursor, count, key) {
	case .Chosen:
		if labels[s.cursor] == "Accept" {
			offer := actor_get(u, s.dock).offer
			m := item_get(u, offer).mission
			if mission_accept(u, s.dock) {
				return Replace{accepted_message(u, m)}
			}
		}
		return Pop{}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

accepted_message :: proc(u: ^Universe, m: Mission) -> Message {
	msg := message_make(.Orange, "Delivery accepted!")
	lines: [MAX_MISSION_LINES]Long_Text
	mission_lines(u, m, &lines)
	for i in 0 ..< 3 { // the item, where it goes, and which system
		message_add(&msg, .Light_Gray, long_str(&lines[i]))
	}
	return msg
}

// ---- The receipt ----

completion_message :: proc(u: ^Universe, done: Completion) -> Message {
	m := message_make(.Orange, "Delivery Complete!")
	d1, d2: [20]u8
	for i in 0 ..< done.count {
		item := item_get(u, done.ids[i])
		noun := mission_nouns[item.mission.noun]
		message_add(&m, .Light_Gray, noun, " (Jools: +", int_text(&d1, item.mission.reward), ", Reputation: +", int_text(&d2, MISSION_REPUTATION_BONUS), ")")
	}
	message_add(&m, .Light_Gray, "Total Jools: +", int_text(&d1, done.jools))
	message_add(&m, .Light_Gray, "Total Reputation: +", int_text(&d2, done.reputation))
	return m
}

// ---- Abandoning ----

Confirm_Abandon_Delivery :: struct {
	item:   Item_Id,
	cursor: int, // 0 is Cancel, so Enter alone is safe
}

abandon_labels := [?]string{"Cancel", "Confirm"}

confirm_abandon_delivery_draw :: proc(s: ^Confirm_Abandon_Delivery, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	text_put_centered(tb, 1, "Really abandon delivery?", .Yellow)
	item := item_get(u, s.item)
	name := mission_item_name(item.mission)
	text_put_wrapped(tb, 2, 4, TEXT_COLUMNS - 4, long_str(&name), .Orange)
	d: [20]u8
	text := long_join("Consider this carefully. This will result in a reputation penalty of ", int_text(&d, MISSION_REPUTATION_PENALTY), ".")
	text_put_wrapped(tb, 2, 8, TEXT_COLUMNS - 4, long_str(&text), .Light_Red)
	menu_draw(tb, 14, abandon_labels[:], s.cursor)
}

confirm_abandon_delivery_key :: proc(s: ^Confirm_Abandon_Delivery, key: Key, session: ^Session) -> Transition {
	switch menu_key(&s.cursor, len(abandon_labels), key) {
	case .Chosen:
		if s.cursor == 1 {
			mission_abandon(&session.universe, s.item)
			return Pop_Count{2} // this screen and the item's page
		}
		return Pop{}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- Status ----

Status_Screen :: struct {}

status_draw :: proc(s: ^Status_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	text_put_centered(tb, 1, "STATUS", .Yellow)
	row := 4
	for entry in ([]struct {
		label: string,
		store: Store,
	}{{"O2", u.avatar.oxygen}, {"Fuel", u.avatar.fuel}, {"Hull", u.avatar.hull}}) {
		pct := percent_of(entry.store)
		hue := hue_for_percent(pct)
		c := text_put(tb, 2, row, entry.label, hue)
		c = text_put(tb, c, row, ": (", hue)
		c = text_put_int(tb, c, row, entry.store.current, hue)
		c = text_put(tb, c, row, "/", hue)
		c = text_put_int(tb, c, row, entry.store.maximum, hue)
		c = text_put(tb, c, row, ") ", hue)
		c = text_put_int(tb, c, row, pct, hue)
		text_put(tb, c, row, "%", hue)
		row += 2
	}
	put_field_int(tb, 2, row, "Jools", u.avatar.jools)
	row += 2
	put_field_int(tb, 2, row, "Turn", u.turn)
	row += 2
	cargo := put_field_int(tb, 2, row, "Cargo", cargo_weight(u))
	text_put(tb, cargo, row, " (+", .Light_Gray)
	cargo = text_put_int(tb, cargo + 3, row, cargo_fuel_surcharge(u), .White)
	text_put(tb, cargo, row, " fuel/move)", .Light_Gray)
	row += 3
	faction := faction_get(u, u.avatar.faction)
	put_field(tb, 2, row, "Faction", name_str(&faction.name))
	put_field_int(tb, 4, row + 1, "Reputation", faction.reputation)
	row += 3
	home := planet_get(u, u.avatar.home_planet)
	put_field(tb, 2, row, "Home Planet", name_str(&home.name))
	put_field_int(tb, 4, row + 1, "Reputation", home.reputation)
	text_put_centered(tb, 22, "Press Enter", .Dark_Gray)
}

status_key :: proc(s: ^Status_Screen, key: Key, session: ^Session) -> Transition {
	if key == KEY_ENTER || key == KEY_ESCAPE || key == ' ' {
		return Pop{}
	}
	return nil
}
