package game

// Star gates: pick one of your faction's other gates and appear beside it. Wormholes need no screen of their own.

Star_Gate_Screen :: struct {
	gate:   Actor_Id, // the one you are at
	cursor: int,
}

gate_name :: proc(u: ^Universe, gate: Actor_Id) -> Long_Text {
	return long_join(name_str(&planet_get(u, actor_get(u, gate).planet).name), " Star Gate")
}

star_gate_draw :: proc(s: ^Star_Gate_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	text_put_centered(tb, 1, "STAR GATE", .Light_Green)
	text_put(tb, 2, 3, "Destination:", .Light_Gray)
	gates := star_gates_for_avatar(u, s.gate)
	defer delete(gates)
	names := make([]Long_Text, len(gates) + 1, context.temp_allocator)
	labels := make([]string, len(gates) + 1, context.temp_allocator)
	details := make([]string, len(gates) + 1, context.temp_allocator)
	names[0] = long_join("Cancel")
	labels[0] = long_str(&names[0])
	for gate, i in gates {
		names[i + 1] = gate_name(u, gate)
		labels[i + 1] = long_str(&names[i + 1])
		details[i + 1] = name_str(&star_system_get(u, actor_get(u, gate).star_system).name)
	}
	s.cursor = min(s.cursor, len(gates))
	menu_draw(tb, 5, labels, s.cursor, 2, details)
}

star_gate_key :: proc(s: ^Star_Gate_Screen, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	gates := star_gates_for_avatar(u, s.gate)
	defer delete(gates)
	switch menu_key(&s.cursor, len(gates) + 1, key) {
	case .Chosen:
		if s.cursor == 0 || s.cursor > len(gates) {
			return Pop{}
		}
		if avatar_travel_to(u, gates[s.cursor - 1]) == .Blocked {
			return Replace{message_make(.Light_Red, "Destination blocked!")}
		}
		return Pop{}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}
