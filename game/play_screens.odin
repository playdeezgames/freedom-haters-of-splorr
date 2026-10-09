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
	u := &session.universe
	if session.in_play && avatar_is_game_over(u) {
		return Replace{Game_Over{}}
	}
	if session.in_play && u.avatar.auto_used.used {
		report := oxygen_report(u.avatar.auto_used)
		u.avatar.auto_used = {}
		return Push{report}
	}
	return nil
}

// ---- Interaction: what to do about what you bumped into ----

Interaction_Screen :: struct {
	cursor: int,
}

interaction_labels :: proc(u: ^Universe, labels: ^[MAX_INTERACTIONS + 1]string, names: ^[MAX_INTERACTIONS]Name) -> (count: int) {
	list, n := interactions_for(u, u.avatar.bumped)
	for i in 0 ..< n {
		labels[i] = interaction_label(u, list[i], u.avatar.bumped, &names[i])
	}
	labels[n] = "Cancel"
	return n + 1
}

interaction_draw :: proc(s: ^Interaction_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	draw_bump_info(tb, u, u.avatar.bumped, 4)
	labels: [MAX_INTERACTIONS + 1]string
	names: [MAX_INTERACTIONS]Name
	count := interaction_labels(u, &labels, &names)
	menu_draw(tb, 17, labels[:count], s.cursor)
}

interaction_key :: proc(s: ^Interaction_Screen, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	labels: [MAX_INTERACTIONS + 1]string
	names: [MAX_INTERACTIONS]Name
	count := interaction_labels(u, &labels, &names)
	switch menu_key(&s.cursor, count, key) {
	case .Chosen:
		list, actions := interactions_for(u, u.avatar.bumped)
		defer u.avatar.bumped = nil
		if s.cursor >= actions {
			return Pop{}
		}
		digits: [20]u8
		switch list[s.cursor] {
		case .Approach, .Enter_Orbit, .Leave_Area:
			if avatar_interact(u, list[s.cursor]) == .Blocked {
				return Replace{message_make(.Light_Red, "Destination blocked!")}
			}
			return Pop{}
		case .Refill_Oxygen:
			added, cost := avatar_buy_oxygen(u)
			m := message_make(.Orange, "Oxygen Refilled!")
			message_add(&m, .Light_Gray, "You buy ", int_text(&digits, added), " oxygen!")
			message_add(&m, .Light_Gray, "Cost: ", int_text(&digits, cost), " Jools!")
			return Replace{m}
		case .Refuel:
			added, cost := avatar_buy_fuel(u)
			m := message_make(.Orange, "Refueled!")
			message_add(&m, .Light_Gray, "You bought ", int_text(&digits, added), " fuel.")
			message_add(&m, .Light_Gray, "You paid ", int_text(&digits, cost), " Jools.")
			return Replace{m}
		case .Salvage_Scrap:
			found := avatar_salvage(u, u.avatar.bumped.(Actor_Id))
			m := message_make(.Orange, "Salvage!")
			message_add(&m, .Light_Gray, "You find:")
			message_add(&m, .Light_Gray, int_text(&digits, found), " Scrap")
			return Replace{m}
		case .Trade:
			return Replace{Trader{post = u.avatar.bumped.(Actor_Id)}}
		case .Enter_Shipyard:
			return Replace{Shipyard_Screen{yard = u.avatar.bumped.(Actor_Id)}}
		case .Use_Fuel_Scoop:
			added := avatar_use_fuel_scoop(u)
			m := message_make(.Orange, "Fuel Scooped!")
			message_add(&m, .Light_Gray, "You collect ", int_text(&digits, added), " fuel!")
			message_add(&m, .Light_Gray, "No charge!")
			return Replace{m}
		case .Gather_Atmosphere:
			added := avatar_gather_atmosphere(u)
			m := message_make(.Orange, "Atmosphere Gathered!")
			message_add(&m, .Light_Gray, "You collect ", int_text(&digits, added), " oxygen!")
			message_add(&m, .Light_Gray, "No charge!")
			return Replace{m}
		}
		return Pop{}
	case .Cancelled:
		u.avatar.bumped = nil
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- Action menu ----

Action :: enum {
	Inventory,
	Equipment,
	Signal_Distress,
}

Action_Menu :: struct {
	cursor: int,
}

// The actions on offer, in menu order; Cancel is always last and is not listed.
action_list :: proc(u: ^Universe) -> (list: [len(Action)]Action, count: int) {
	list[count] = .Inventory
	count += 1
	list[count] = .Equipment
	count += 1
	if distress_available(u) {
		list[count] = .Signal_Distress
		count += 1
	}
	return
}

action_label :: proc(a: Action) -> string {
	switch a {
	case .Inventory:
		return "Inventory"
	case .Equipment:
		return "Equipment"
	case .Signal_Distress:
		return "Signal Distress"
	}
	return ""
}

action_menu_draw :: proc(s: ^Action_Menu, tb: ^Text_Buffer, session: ^Session) {
	list, n := action_list(&session.universe)
	labels: [len(Action) + 1]string
	for i in 0 ..< n {
		labels[i] = action_label(list[i])
	}
	labels[n] = "Cancel"
	text_put_centered(tb, 3, "ACTIONS", .Yellow)
	menu_draw(tb, 8, labels[:n + 1], s.cursor)
}

action_menu_key :: proc(s: ^Action_Menu, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	list, n := action_list(u)
	switch menu_key(&s.cursor, n + 1, key) {
	case .Chosen:
		if s.cursor >= n {
			return Pop{}
		}
		switch list[s.cursor] {
		case .Inventory:
			return Push{Inventory_Screen{}}
		case .Equipment:
			return Push{Equipment_Screen{}}
		case .Signal_Distress:
			added, price := avatar_signal_distress(u)
			digits: [20]u8
			m := message_make(.Orange, "Emergency Refuel!")
			message_add(&m, .Light_Gray, "Added ", int_text(&digits, added), " fuel!")
			message_add(&m, .Light_Gray, "Price ", int_text(&digits, price), " jools!")
			return Replace{m}
		}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- Inventory ----

Inventory_Screen :: struct {
	cursor: int,
}

inventory_labels :: proc(u: ^Universe, labels: ^[MAX_STACKS + 1]string, names: ^[MAX_STACKS]Name) -> (stacks: Stacks, count: int) {
	stacks = inventory_stacks(u)
	digits: [20]u8
	labels[0] = "Cancel"
	for i in 0 ..< stacks.count {
		st := stacks.stacks[i]
		base := item_stack_name(st)
		names[i] = name_join(name_str(&base), " (x", int_text(&digits, st.count), ")")
		labels[i + 1] = name_str(&names[i])
	}
	return stacks, stacks.count + 1
}

inventory_draw :: proc(s: ^Inventory_Screen, tb: ^Text_Buffer, session: ^Session) {
	labels: [MAX_STACKS + 1]string
	names: [MAX_STACKS]Name
	_, count := inventory_labels(&session.universe, &labels, &names)
	s.cursor = min(s.cursor, count - 1)
	text_put_centered(tb, 1, "INVENTORY", .Yellow)
	if count == 1 {
		text_put_centered(tb, 6, "Yer hold is empty.", .Dark_Gray)
	}
	menu_draw(tb, 4 if count > 1 else 9, labels[:count], s.cursor)
}

inventory_key :: proc(s: ^Inventory_Screen, key: Key, session: ^Session) -> Transition {
	labels: [MAX_STACKS + 1]string
	names: [MAX_STACKS]Name
	stacks, count := inventory_labels(&session.universe, &labels, &names)
	switch menu_key(&s.cursor, count, key) {
	case .Chosen:
		if s.cursor == 0 {
			return Pop{}
		}
		st := stacks.stacks[s.cursor - 1]
		return Push{Item_Page{kind = st.kind, mark = st.mark, count = st.count}}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

Item_Page :: struct {
	kind:   Item_Kind,
	mark:   int,
	count:  int,
	cursor: int,
	scroll: int,
}

item_usable :: proc(kind: Item_Kind) -> bool {
	return kind == .Oxygen_Tank || kind == .Fuel_Rod
}

PAGE_TEXT_TOP :: 4
PAGE_TEXT_WIDTH :: TEXT_COLUMNS - 5
MAX_PAGE_LINES :: 96

Page_Lines :: struct {
	lines: [MAX_PAGE_LINES]string,
	count: int,
	// the composed pieces the lines point into
	intro: Long_Text,
	stats: Item_Stats,
}

// Wraps the description, a gap between paragraphs, then the numbers. `pl` must stay put while `lines` is used.
page_lines :: proc(item: Item, pl: ^Page_Lines) {
	add :: proc(pl: ^Page_Lines, line: string) {
		if pl.count < MAX_PAGE_LINES {
			pl.lines[pl.count] = line
			pl.count += 1
		}
	}
	pl.count = 0
	d := item_description(item, &pl.intro)
	for i in 0 ..< d.count {
		rest := d.paragraphs[i]
		for len(rest) > 0 {
			line: string
			line, rest = text_wrap_next(rest, PAGE_TEXT_WIDTH)
			add(pl, line)
		}
		add(pl, "")
	}
	pl.stats = item_stats(item)
	for i in 0 ..< pl.stats.count {
		add(pl, long_str(&pl.stats.lines[i]))
	}
}

// Rows of text that fit: fewer when there is a Use menu underneath.
page_window :: proc(kind: Item_Kind) -> int {
	return 11 if item_usable(kind) else 18
}

item_page_draw :: proc(s: ^Item_Page, tb: ^Text_Buffer, session: ^Session) {
	item := Item{kind = s.kind, mark = s.mark}
	name := item_name(item)
	text_put_centered(tb, 1, name_str(&name), .Yellow)
	if s.count > 0 {
		put_field_int(tb, 2, 2, "You have", s.count)
	}

	pl: Page_Lines
	page_lines(item, &pl)
	window := page_window(s.kind)
	s.scroll = clamp(s.scroll, 0, max(0, pl.count - window))
	for i in 0 ..< min(window, pl.count - s.scroll) {
		text_put(tb, 2, PAGE_TEXT_TOP + i, pl.lines[s.scroll + i], .Light_Gray)
	}
	if s.scroll > 0 {
		text_put(tb, TEXT_COLUMNS - 2, PAGE_TEXT_TOP, "\x1e", .Dark_Gray)
	}
	if s.scroll + window < pl.count {
		text_put(tb, TEXT_COLUMNS - 2, PAGE_TEXT_TOP + window - 1, "\x1f", .Dark_Gray)
	}
	if item_usable(s.kind) {
		labels := [?]string{"Use", "Back"}
		menu_draw(tb, 17, labels[:], s.cursor)
	} else if pl.count > window {
		text_put_centered(tb, 23, "Up/Down: read   Enter: back", .Dark_Gray)
	} else {
		text_put_centered(tb, 23, "Press Enter", .Dark_Gray)
	}
}

item_page_key :: proc(s: ^Item_Page, key: Key, session: ^Session) -> Transition {
	if !item_usable(s.kind) {
		switch key {
		case KEY_ENTER, KEY_ESCAPE, ' ':
			return Pop{}
		case KEY_UP:
			s.scroll -= 1
		case KEY_DOWN:
			s.scroll += 1
		case KEY_LEFT:
			s.scroll -= page_window(s.kind)
		case KEY_RIGHT:
			s.scroll += page_window(s.kind)
		}
		return nil // draw clamps the scroll
	}
	switch menu_key(&s.cursor, 2, key) {
	case .Chosen:
		if s.cursor == 1 {
			return Pop{}
		}
		u := &session.universe
		digits: [20]u8
		if s.kind == .Oxygen_Tank {
			r := avatar_use_oxygen_tank(u)
			return Replace{oxygen_report(r)}
		}
		r := avatar_use_fuel_rod(u)
		m := message_make(.Orange, "Replenished Fuel!")
		message_add(&m, .Light_Gray, "Added ", int_text(&digits, r.added), " fuel.")
		message_add(&m, .Light_Gray, "Fuel is now ", int_text(&digits, r.percent), "%.")
		return Replace{m}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

oxygen_report :: proc(r: Use_Result) -> Message {
	d1, d2: [20]u8
	m := message_make(.Orange, "Replenished Oxygen!")
	message_add(&m, .Light_Gray, "Added ", int_text(&d1, r.added), " O2.")
	message_add(&m, .Light_Gray, "O2 is now ", int_text(&d2, r.percent), "%.")
	return m
}

// ---- A message to dismiss ----

Message_Line :: struct {
	text: [96]u8,
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
	row := 6
	for i in 0 ..< s.count {
		line := &s.lines[i]
		text := string(line.text[:line.len])
		if len(text) <= TEXT_COLUMNS - 4 {
			text_put_centered(tb, row, text, line.hue)
			row += 2
		} else {
			row += text_put_wrapped(tb, 2, row, TEXT_COLUMNS - 4, text, line.hue) + 1
		}
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
