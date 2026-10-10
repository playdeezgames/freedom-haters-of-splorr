package game

// What a military ship says when it reaches you: a hail from a friendly one, a shakedown from a hostile one.

hail_message :: proc(u: ^Universe, ship: Actor_Id) -> Message {
	a := actor_get(u, ship)
	m := message_make(disposition_hues[ship_disposition(u, a^)], "Military Vessel")
	message_add(&m, .Light_Gray, name_str(&faction_get(u, a.faction).name))
	if ship_disposition(u, a^) == .Friendly {
		message_add(&m, .Light_Gray, "\"Stay safe, citizen.\"")
	} else {
		message_add(&m, .Light_Gray, "\"Keep moving.\"")
	}
	ship_calm(u, ship, CALM_AFTER_HAIL)
	return m
}

Contact_Screen :: struct {
	ship:   Actor_Id,
	cursor: int,
}

Contact_Choice :: enum {
	Pay_Fine,
	Hand_Over_Cargo,
}

// The choices on offer (Resist joins them when there is a way to fight).
contact_choices :: proc(u: ^Universe) -> (list: [len(Contact_Choice)]Contact_Choice, count: int) {
	if fine_amount(u) > 0 {
		list[count] = .Pay_Fine
		count += 1
	}
	if takeable_count(u) > 0 {
		list[count] = .Hand_Over_Cargo
		count += 1
	}
	return
}

contact_draw :: proc(s: ^Contact_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	a := actor_get(u, s.ship)^
	text_put_centered(tb, 1, "HALT!", .Light_Red)
	text_put(tb, 2, 4, "A military vessel has caught you.", .Light_Gray)
	text_put(tb, 2, 6, name_str(&faction_get(u, a.faction).name), .White)
	text_put(tb, 2, 8, "\"You have been selected for a", .Light_Gray)
	text_put(tb, 2, 9, "voluntary contribution.\"", .Light_Gray)
	list, n := contact_choices(u)
	texts: [len(Contact_Choice)]Long_Text
	labels: [len(Contact_Choice)]string
	digits: [20]u8
	for i in 0 ..< n {
		switch list[i] {
		case .Pay_Fine:
			texts[i] = long_join("Pay Fine (", int_text(&digits, fine_amount(u)), " jools)")
		case .Hand_Over_Cargo:
			texts[i] = long_join("Hand Over Cargo (", int_text(&digits, cargo_demanded(u)), " items)")
		}
		labels[i] = long_str(&texts[i])
	}
	menu_draw(tb, 12, labels[:n], s.cursor)
}

contact_key :: proc(s: ^Contact_Screen, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	list, n := contact_choices(u)
	// there is no escaping a shakedown: Escape does nothing
	if menu_key(&s.cursor, n, key) == .Chosen && s.cursor < n {
		digits: [20]u8
		switch list[s.cursor] {
		case .Pay_Fine:
			paid := avatar_pay_fine(u, s.ship)
			m := message_make(.Light_Red, "Fine paid.")
			message_add(&m, .Light_Gray, int_text(&digits, paid), " jools.")
			return Replace{m}
		case .Hand_Over_Cargo:
			taken := avatar_hand_over_cargo(u, s.ship)
			m := message_make(.Light_Red, "Cargo seized.")
			message_add(&m, .Light_Gray, int_text(&digits, taken), " items.")
			return Replace{m}
		}
	}
	return nil
}
