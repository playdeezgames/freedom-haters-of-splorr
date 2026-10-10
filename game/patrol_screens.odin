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
	Resist,
}

// The choices on offer. Resist needs a weapon.
contact_choices :: proc(u: ^Universe) -> (list: [len(Contact_Choice)]Contact_Choice, count: int) {
	if fine_amount(u) > 0 {
		list[count] = .Pay_Fine
		count += 1
	}
	if takeable_count(u) > 0 {
		list[count] = .Hand_Over_Cargo
		count += 1
	}
	if avatar_fire_damage(u) > 0 {
		list[count] = .Resist // with nothing to shoot, resisting is only a way to lose
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
		case .Resist:
			texts[i] = long_join("Resist")
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
		case .Resist:
			return Replace{Combat_Screen{combat = combat_start(u, s.ship)}}
		case .Hand_Over_Cargo:
			taken := avatar_hand_over_cargo(u, s.ship)
			m := message_make(.Light_Red, "Cargo seized.")
			message_add(&m, .Light_Gray, int_text(&digits, taken), " items.")
			return Replace{m}
		}
	}
	return nil
}

// ---- Combat ----

Combat_Screen :: struct {
	combat: Combat,
	cursor: int,
	log:    [3]Long_Text, // what the last round did
	logged: int,
}

Combat_Choice :: enum {
	Fire,
	Evade,
	Flee,
	Surrender,
}

combat_choices :: proc(u: ^Universe) -> (list: [len(Combat_Choice)]Combat_Choice, count: int) {
	if avatar_fire_damage(u) > 0 {
		list[count] = .Fire
		count += 1
	}
	list[count] = .Evade
	count += 1
	if can_flee(u) {
		list[count] = .Flee
		count += 1
	}
	list[count] = .Surrender
	count += 1
	return
}

combat_draw :: proc(s: ^Combat_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	c := &s.combat
	ship := actor_get(u, c.ship)^
	text_put_centered(tb, 1, "COMBAT", .Light_Red)
	text_put(tb, 2, 3, name_str(&faction_get(u, ship.faction).name), .White)
	col := put_field_int(tb, 2, 5, "Enemy Hull", c.enemy_hull, hue_for_percent(c.enemy_hull * 100 / max(c.enemy_max, 1)))
	text_put_int(tb, text_put(tb, col, 5, "/", .Dark_Gray), 5, c.enemy_max, .Dark_Gray)
	col = put_field_int(tb, 2, 7, "Yer Hull", u.avatar.hull.current, hue_for_percent(percent_of(u.avatar.hull)))
	text_put_int(tb, text_put(tb, col, 7, "/", .Dark_Gray), 7, u.avatar.hull.maximum, .Dark_Gray)
	if shield_mark(u) > 0 {
		put_field_int(tb, 2, 8, "Shield", c.shield, .Light_Cyan)
	} else {
		put_field(tb, 2, 8, "Shield", "none", .Dark_Gray)
	}
	if weapon_mark(u) > 0 {
		weapon := item_name(item_get(u, u.avatar.equipment[.Weapon])^)
		put_field(tb, 2, 9, "Weapon", name_str(&weapon), .Light_Green)
	} else {
		put_field(tb, 2, 9, "Weapon", "none", .Dark_Gray)
	}
	for i in 0 ..< s.logged {
		text_put(tb, 2, 11 + i, long_str(&s.log[i]), .Light_Gray)
	}
	list, n := combat_choices(u)
	texts: [len(Combat_Choice)]Long_Text
	labels: [len(Combat_Choice)]string
	digits: [20]u8
	for i in 0 ..< n {
		switch list[i] {
		case .Fire:
			texts[i] = long_join("Fire (", int_text(&digits, avatar_fire_damage(u)), " damage)")
		case .Evade:
			texts[i] = long_join("Evade")
		case .Flee:
			texts[i] = long_join("Flee (", int_text(&digits, FLEE_FUEL), " fuel)")
		case .Surrender:
			texts[i] = long_join("Surrender")
		}
		labels[i] = long_str(&texts[i])
	}
	menu_draw(tb, 15, labels[:n], s.cursor)
}

@(private = "file")
log_round :: proc(s: ^Combat_Screen, r: Round) {
	d1, d2: [20]u8
	s.logged = 0
	add :: proc(s: ^Combat_Screen, t: Long_Text) {
		s.log[s.logged] = t
		s.logged += 1
	}
	switch {
	case r.fled && r.outcome != .Escaped:
		add(s, long_join("You try to flee. No luck."))
	case r.evaded:
		add(s, long_join("You evade."))
	case r.dealt > 0:
		add(s, long_join("You hit for ", int_text(&d1, r.dealt), "."))
	}
	if r.outcome == .Continues || r.outcome == .Lost {
		add(s, long_join("It hits for ", int_text(&d1, r.taken + r.absorbed), "."))
		if r.absorbed > 0 {
			add(s, long_join("The shield soaks ", int_text(&d2, r.absorbed), "."))
		}
	}
}

combat_key :: proc(s: ^Combat_Screen, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	list, n := combat_choices(u)
	s.cursor = min(s.cursor, n - 1)
	// there is no walking out of a fight: Escape does nothing
	if menu_key(&s.cursor, n, key) != .Chosen || s.cursor >= n {
		return nil
	}
	action: Combat_Action
	switch list[s.cursor] {
	case .Fire:
		action = .Fire
	case .Evade:
		action = .Evade
	case .Flee:
		action = .Flee
	case .Surrender:
		return Replace{contact_screen_for(u, s.combat.ship)}
	}
	round := combat_round(u, &s.combat, action)
	digits: [20]u8
	switch round.outcome {
	case .Continues:
		log_round(s, round)
	case .Won:
		v := combat_victory(u, s.combat)
		m := message_make(.Light_Green, "Victory!")
		message_add(&m, .Light_Gray, "The ship comes apart.")
		message_add(&m, .Light_Gray, "Wreckage: ", int_text(&digits, v.loot), " scrap.")
		d3: [20]u8
		message_add(&m, .Light_Gray, "Salvaged: ", int_text(&d3, v.parts), " ship parts.")
		if v.hold_units > 0 {
			d4: [20]u8
			message_add(&m, .Light_Gray, "From its hold: ", int_text(&d4, v.hold_units), " ", good_info[v.hold_good].name)
		}
		message_add(&m, .Light_Red, "Its faction will remember.")
		return Replace{m}
	case .Lost:
		d := combat_defeat(u, s.combat)
		d2: [20]u8
		m := message_make(.Light_Red, "Defeated!")
		message_add(&m, .Light_Gray, "You are robbed and towed home.")
		message_add(&m, .Light_Gray, "Jools lost: ", int_text(&digits, d.jools_lost))
		message_add(&m, .Light_Gray, "Items lost: ", int_text(&d2, d.items_lost))
		return Replace{m}
	case .Escaped:
		return Replace{message_make(.Light_Green, "You get away!", "For now.")}
	}
	return nil
}

// ---- Searches ----

// Back to what the ship wants of you: a search if you carry what it forbids, else the shakedown.
contact_screen_for :: proc(u: ^Universe, ship: Actor_Id) -> Screen {
	if units, _ := contraband_units(u, actor_get(u, ship).faction); units > 0 {
		return Search_Screen{ship = ship}
	}
	return Contact_Screen{ship = ship}
}

Search_Screen :: struct {
	ship:   Actor_Id,
	cursor: int,
}

Search_Choice :: enum {
	Surrender_Contraband,
	Pay_Fine,
	Resist,
}

search_choices :: proc(u: ^Universe, faction: Faction_Id) -> (list: [len(Search_Choice)]Search_Choice, count: int) {
	list[count] = .Surrender_Contraband
	count += 1
	if search_fine_affordable(u, faction) {
		list[count] = .Pay_Fine
		count += 1
	}
	if avatar_fire_damage(u) > 0 {
		list[count] = .Resist
		count += 1
	}
	return
}

search_draw :: proc(s: ^Search_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	faction := actor_get(u, s.ship).faction
	text_put_centered(tb, 1, "SEARCH!", .Light_Red)
	text_put(tb, 2, 4, name_str(&faction_get(u, faction).name), .White)
	text_put(tb, 2, 6, "\"Your hold will be inspected.\"", .Light_Gray)
	units, _ := contraband_units(u, faction)
	digits: [20]u8
	text_put(tb, 2, 8, long_str_of_parts(&digits, "Banned goods aboard: ", units), .Light_Red)
	list, n := search_choices(u, faction)
	texts: [len(Search_Choice)]Long_Text
	labels: [len(Search_Choice)]string
	d2: [20]u8
	for i in 0 ..< n {
		switch list[i] {
		case .Surrender_Contraband:
			texts[i] = long_join("Hand Over The Goods")
		case .Pay_Fine:
			texts[i] = long_join("Pay The Fine (", int_text(&d2, search_fine(u, faction)), " jools)")
		case .Resist:
			texts[i] = long_join("Resist")
		}
		labels[i] = long_str(&texts[i])
	}
	menu_draw(tb, 12, labels[:n], s.cursor)
}

search_key :: proc(s: ^Search_Screen, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	faction := actor_get(u, s.ship).faction
	list, n := search_choices(u, faction)
	if menu_key(&s.cursor, n, key) != .Chosen || s.cursor >= n {
		return nil // no escaping a search
	}
	digits: [20]u8
	switch list[s.cursor] {
	case .Surrender_Contraband:
		units := avatar_surrender_contraband(u, s.ship)
		m := message_make(.Light_Red, "Contraband seized.")
		message_add(&m, .Light_Gray, int_text(&digits, units), " units.")
		message_add(&m, .Light_Gray, "They have your name now.")
		return Replace{m}
	case .Pay_Fine:
		fine := avatar_bribe(u, s.ship)
		m := message_make(.Light_Red, "Fine paid.")
		message_add(&m, .Light_Gray, int_text(&digits, fine), " jools.")
		message_add(&m, .Light_Gray, "They look the other way.")
		return Replace{m}
	case .Resist:
		return Replace{Combat_Screen{combat = combat_start(u, s.ship)}}
	}
	return nil
}

@(private = "file")
long_str_of_parts :: proc(digits: ^[20]u8, label: string, n: int) -> string {
	@(static) buf: [48]u8
	text := int_text(digits, n)
	copy(buf[:], label)
	copy(buf[len(label):], text)
	return string(buf[:len(label) + len(text)])
}
