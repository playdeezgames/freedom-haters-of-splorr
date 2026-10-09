package game

// The screens you see while a universe is in play: navigating, interacting with what you bump into,
// the action and game menus, a plain message box, and game over.

// ---- Navigation ----

Nav_Message :: enum {
	None,
	No_Fuel,
	Out_Of_Fuel,
}

Navigation :: struct {
	message: Nav_Message,
}

navigation_draw :: proc(s: ^Navigation, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	map_id, pos := avatar_place(u)
	title := map_title(u, map_id)
	text_put(tb, 0, 0, name_str(&title), .Yellow)
	draw_map_view(tb, u)

	col := VIEW_SIZE + 1
	text_put(tb, col, 2, "(", .Light_Gray)
	c := text_put_int(tb, col + 1, 2, pos.x, .White)
	c = text_put(tb, c, 2, ",", .Light_Gray)
	c = text_put_int(tb, c, 2, pos.y, .White)
	text_put(tb, c, 2, ")", .Light_Gray)
	put_field_int(tb, col, 4, "Turn", u.turn)
	put_field_int(tb, col, 6, "Jools", u.avatar.jools)

	oxygen_percent := percent_of(u.avatar.oxygen)
	c = put_field_int(tb, col, 8, "O2", oxygen_percent, hue_for_percent(oxygen_percent))
	text_put(tb, c, 8, "%", hue_for_percent(oxygen_percent))
	c = text_put_int(tb, col, 9, u.avatar.oxygen.current, .Light_Gray)
	c = text_put(tb, c, 9, "/", .Dark_Gray)
	text_put_int(tb, c, 9, u.avatar.oxygen.maximum, .Dark_Gray)

	fuel_percent := percent_of(u.avatar.fuel)
	c = put_field_int(tb, col, 11, "Fuel", fuel_percent, hue_for_percent(fuel_percent))
	text_put(tb, c, 11, "%", hue_for_percent(fuel_percent))
	c = text_put_int(tb, col, 12, u.avatar.fuel.current, .Light_Gray)
	c = text_put(tb, c, 12, "/", .Dark_Gray)
	text_put_int(tb, c, 12, u.avatar.fuel.maximum, .Dark_Gray)

	switch s.message {
	case .None:
	case .No_Fuel:
		text_put(tb, col, 15, "NO FUEL!", .Light_Red)
		text_put(tb, col, 17, "Enter: signal", .Light_Gray)
		text_put(tb, col, 18, "distress", .Light_Gray)
	case .Out_Of_Fuel:
		text_put(tb, col, 15, "OUT OF FUEL!", .Light_Red)
		text_put(tb, col, 17, "Enter: signal", .Light_Gray)
		text_put(tb, col, 18, "distress", .Light_Gray)
	}
	text_put(tb, 0, 24, "Arrows:Move Enter:Act Esc:Menu", .Dark_Gray)
}

navigation_key :: proc(s: ^Navigation, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	dir: Direction
	switch key {
	case KEY_UP:
		dir = .North
	case KEY_DOWN:
		dir = .South
	case KEY_LEFT:
		dir = .West
	case KEY_RIGHT:
		dir = .East
	case KEY_ENTER, ' ':
		return Push{Action_Menu{}}
	case KEY_ESCAPE:
		return Push{Game_Menu{}}
	case:
		return nil
	}
	s.message = .None
	outcome := avatar_move(u, dir)
	switch outcome {
	case .No_Fuel:
		s.message = .No_Fuel
	case .Bumped:
		return Push{Interaction_Screen{}}
	case .Moved, .Out_Of_Map:
	}
	if outcome != .No_Fuel && u.avatar.fuel.current <= u.avatar.fuel.minimum {
		s.message = .Out_Of_Fuel
	}
	return nil
}

// Anything that ends the game (running out of oxygen on a move, an emergency refuel you can't afford) is
// noticed here, once the screens above have closed.
navigation_tick :: proc(s: ^Navigation, session: ^Session) -> Transition {
	if session.in_play && avatar_is_game_over(&session.universe) {
		return Replace{Game_Over{}}
	}
	return nil
}

// ---- Interaction: what to do about what you bumped into ----

Interaction_Screen :: struct {
	cursor: int,
}

interaction_labels :: proc(u: ^Universe, labels: ^[2]string) -> (count: int) {
	if kind, ok := interaction_for(u, u.avatar.bumped); ok {
		labels[count] = interaction_label(u, kind, u.avatar.bumped)
		count += 1
	}
	labels[count] = "Cancel"
	count += 1
	return
}

interaction_draw :: proc(s: ^Interaction_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	draw_bump_info(tb, u, u.avatar.bumped, 4)
	labels: [2]string
	count := interaction_labels(u, &labels)
	menu_draw(tb, 18, labels[:count], s.cursor)
}

interaction_key :: proc(s: ^Interaction_Screen, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	labels: [2]string
	count := interaction_labels(u, &labels)
	switch menu_key(&s.cursor, count, key) {
	case .Chosen:
		if kind, ok := interaction_for(u, u.avatar.bumped); ok && s.cursor == 0 {
			if avatar_interact(u, kind) == .Blocked {
				return Replace{message_make(.Light_Red, "Destination blocked!")}
			}
		}
		u.avatar.bumped = nil
		return Pop{}
	case .Cancelled:
		u.avatar.bumped = nil
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- Action menu ----

Action_Menu :: struct {
	cursor: int,
}

action_labels :: proc(u: ^Universe, labels: ^[2]string) -> (count: int) {
	if distress_available(u) {
		labels[count] = "Signal Distress"
		count += 1
	}
	labels[count] = "Cancel"
	count += 1
	return
}

action_menu_draw :: proc(s: ^Action_Menu, tb: ^Text_Buffer, session: ^Session) {
	labels: [2]string
	count := action_labels(&session.universe, &labels)
	text_put_centered(tb, 3, "ACTIONS", .Yellow)
	menu_draw(tb, 8, labels[:count], s.cursor)
}

action_menu_key :: proc(s: ^Action_Menu, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	labels: [2]string
	count := action_labels(u, &labels)
	switch menu_key(&s.cursor, count, key) {
	case .Chosen:
		if labels[s.cursor] == "Signal Distress" {
			added, price := avatar_signal_distress(u)
			digits: [20]u8
			m := message_make(.Orange, "Emergency Refuel!")
			message_add(&m, .Light_Gray, "Added ", int_text(&digits, added), " fuel!")
			message_add(&m, .Light_Gray, "Price ", int_text(&digits, price), " jools!")
			return Replace{m}
		}
		return Pop{}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- A message to dismiss ----

Message_Line :: struct {
	text: [40]u8,
	len:  u8,
	hue:  Hue,
}

Message :: struct {
	lines: [6]Message_Line,
	count: int,
}

int_text :: proc(buf: ^[20]u8, n: int) -> string {
	i := len(buf)
	v := abs(n)
	for {
		i -= 1
		buf[i] = u8('0' + v % 10)
		v /= 10
		if v == 0 {
			break
		}
	}
	if n < 0 {
		i -= 1
		buf[i] = '-'
	}
	return string(buf[i:])
}

message_make :: proc(hue: Hue, parts: ..string) -> (m: Message) {
	message_add(&m, hue, ..parts)
	return
}

message_add :: proc(m: ^Message, hue: Hue, parts: ..string) {
	if m.count == len(m.lines) {
		return
	}
	line := &m.lines[m.count]
	line.hue = hue
	for part in parts {
		take := min(len(part), len(line.text) - int(line.len))
		copy(line.text[line.len:], part[:take])
		line.len += u8(take)
	}
	m.count += 1
}

message_draw :: proc(s: ^Message, tb: ^Text_Buffer, session: ^Session) {
	for i in 0 ..< s.count {
		line := &s.lines[i]
		text_put_centered(tb, 8 + i * 2, string(line.text[:line.len]), line.hue)
	}
	text_put_centered(tb, 22, "Press Enter", .Dark_Gray)
}

message_key :: proc(s: ^Message, key: Key, session: ^Session) -> Transition {
	if key == KEY_ENTER || key == KEY_ESCAPE || key == ' ' {
		return Pop{}
	}
	return nil
}

// ---- Game menu ----

Game_Menu :: struct {
	cursor: int,
}

game_menu_labels := [?]string{"Continue Game", "Abandon Game"}

game_menu_draw :: proc(s: ^Game_Menu, tb: ^Text_Buffer, session: ^Session) {
	text_put_centered(tb, 3, "GAME MENU", .Yellow)
	menu_draw(tb, 8, game_menu_labels[:], s.cursor)
}

game_menu_key :: proc(s: ^Game_Menu, key: Key, session: ^Session) -> Transition {
	switch menu_key(&s.cursor, len(game_menu_labels), key) {
	case .Chosen:
		if s.cursor == 1 {
			return Push{Confirm_Abandon{}}
		}
		return Pop{}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

Confirm_Abandon :: struct {
	cursor: int, // 0 is No, so Enter alone is safe
}

confirm_labels := [?]string{"No", "Yes"}

confirm_abandon_draw :: proc(s: ^Confirm_Abandon, tb: ^Text_Buffer, session: ^Session) {
	text_put_centered(tb, 6, "Are you sure you want to", .Light_Red)
	text_put_centered(tb, 8, "abandon the game?", .Light_Red)
	menu_draw(tb, 13, confirm_labels[:], s.cursor)
}

confirm_abandon_key :: proc(s: ^Confirm_Abandon, key: Key, session: ^Session) -> Transition {
	switch menu_key(&s.cursor, len(confirm_labels), key) {
	case .Chosen:
		if s.cursor == 1 {
			session_end(session)
			return Reset{Main_Menu{}}
		}
		return Pop{}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- Game over ----

Game_Over :: struct {}

game_over_draw :: proc(s: ^Game_Over, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	if avatar_is_dead(u) {
		text_put_centered(tb, 9, "Yer Dead!", .Light_Red)
	} else {
		text_put_centered(tb, 9, "Yer Bankrupt!", .Light_Red)
	}
	put_field_int(tb, 12, 13, "Turns", u.turn)
	put_field_int(tb, 12, 15, "Jools", u.avatar.jools)
	text_put_centered(tb, 22, "Press Enter", .Dark_Gray)
}

game_over_key :: proc(s: ^Game_Over, key: Key, session: ^Session) -> Transition {
	if key == KEY_ENTER || key == KEY_ESCAPE {
		session_end(session)
		return Reset{Main_Menu{}}
	}
	return nil
}
