package game

// The SPLORRPedia screens: a menu, filterable lists, and pages whose links lead to other pages and lists.

// Lists and pages are built from temporary memory that the app frees after every key and frame.
pedia_temp_entries :: proc(u: ^Universe, kind: Pedia_Kind, scope: Pedia_Scope, scope_id: int, filter: string) -> [dynamic]int {
	context.allocator = context.temp_allocator
	return pedia_entries(u, kind, scope, scope_id, filter)
}

// ---- Menu ----

Pedia_Menu :: struct {
	cursor: int,
}

pedia_menu_labels := [?]string{"Factions", "Star Systems", "Planets", "Satellites", "Back"}

pedia_menu_draw :: proc(s: ^Pedia_Menu, tb: ^Text_Buffer, session: ^Session) {
	text_put_centered(tb, 3, "SPLORRPEDIA", .Yellow)
	menu_draw(tb, 8, pedia_menu_labels[:], s.cursor)
}

pedia_menu_key :: proc(s: ^Pedia_Menu, key: Key, session: ^Session) -> Transition {
	switch menu_key(&s.cursor, len(pedia_menu_labels), key) {
	case .Chosen:
		if s.cursor == len(pedia_menu_labels) - 1 {
			return Pop{}
		}
		return Push{Pedia_List{kind = Pedia_Kind(s.cursor)}}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- A list ----

FILTER_CAPACITY :: 24

Pedia_List :: struct {
	kind:       Pedia_Kind,
	scope:      Pedia_Scope,
	scope_id:   int,
	filter:     [FILTER_CAPACITY]u8,
	filter_len: int,
	cursor:     int,
}

pedia_list_filter :: proc(s: ^Pedia_List) -> string {
	return string(s.filter[:s.filter_len])
}

pedia_kind_titles := [Pedia_Kind]string {
	.Faction     = "Factions",
	.Star_System = "Star Systems",
	.Planet      = "Planets",
	.Satellite   = "Satellites",
}

pedia_list_title :: proc(u: ^Universe, s: ^Pedia_List) -> Long_Text {
	switch s.scope {
	case .All:
		return long_join(pedia_kind_titles[s.kind], ":")
	case .Faction:
		return long_join(pedia_kind_titles[s.kind], " in ", name_str(&u.factions[s.scope_id - 1].name), ":")
	case .Star_System:
		return long_join(pedia_kind_titles[s.kind], " in ", name_str(&u.star_systems[s.scope_id - 1].name), ":")
	case .Planet:
		return long_join(pedia_kind_titles[s.kind], " of ", name_str(&u.planets[s.scope_id - 1].name), ":")
	}
	return {}
}

pedia_list_draw :: proc(s: ^Pedia_List, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	title := pedia_list_title(u, s)
	text_put_wrapped(tb, 2, 1, TEXT_COLUMNS - 4, long_str(&title), .Yellow) // two rows if the name is long

	ids := pedia_temp_entries(u, s.kind, s.scope, s.scope_id, pedia_list_filter(s))
	s.cursor = clamp(s.cursor, 0, max(len(ids) - 1, 0))

	c := text_put(tb, 2, 3, "Filter: ", .Light_Gray)
	c = text_put(tb, c, 3, pedia_list_filter(s), .White)
	text_put(tb, c, 3, "_", .Dark_Gray)
	digits: [20]u8
	count_text := long_join("(", int_text(&digits, len(ids)), ")")
	text_put(tb, TEXT_COLUMNS - 2 - count_text.len, 3, long_str(&count_text), .Dark_Gray)

	if len(ids) == 0 {
		text_put(tb, 2, 6, "No matches.", .Dark_Gray)
	}
	// only the rows that fit are drawn; the list scrolls to keep the cursor in view
	visible := min(len(ids), (TEXT_ROWS - 2 - 6) / 2 + 1)
	first := clamp(s.cursor - visible / 2, 0, len(ids) - visible)
	for i in 0 ..< visible {
		row := 6 + i * 2
		name := pedia_name(u, s.kind, ids[first + i])
		if first + i == s.cursor {
			text_put(tb, 2, row, "> ", .Yellow)
			text_put(tb, 4, row, name, .White)
		} else {
			text_put(tb, 4, row, name, .Light_Gray)
		}
	}
	if first > 0 {
		text_put(tb, 2, 5, "\x1e", .Dark_Gray)
	}
	if first + visible < len(ids) {
		text_put(tb, 2, 6 + visible * 2 - 1, "\x1f", .Dark_Gray)
	}
	text_put(tb, 2, 24, "Type: filter  Left/Right: letter", .Dark_Gray)
}

pedia_list_key :: proc(s: ^Pedia_List, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	switch {
	case key >= ' ' && key < 127: // typing narrows the list
		if s.filter_len < FILTER_CAPACITY {
			s.filter[s.filter_len] = u8(key)
			s.filter_len += 1
			s.cursor = 0
		}
		return nil
	case key == KEY_BACKSPACE:
		if s.filter_len > 0 {
			s.filter_len -= 1
			s.cursor = 0
		}
		return nil
	}
	ids := pedia_temp_entries(u, s.kind, s.scope, s.scope_id, pedia_list_filter(s))
	switch key {
	case KEY_LEFT:
		s.cursor = pedia_jump(u, s.kind, ids[:], s.cursor, false)
	case KEY_RIGHT:
		s.cursor = pedia_jump(u, s.kind, ids[:], s.cursor, true)
	case KEY_UP:
		s.cursor = (s.cursor + len(ids) - 1) % max(len(ids), 1)
	case KEY_DOWN:
		s.cursor = (s.cursor + 1) % max(len(ids), 1)
	case KEY_ENTER:
		if len(ids) > 0 {
			return Push{Pedia_Page{kind = s.kind, id = ids[clamp(s.cursor, 0, len(ids) - 1)]}}
		}
	case KEY_ESCAPE:
		return Pop{}
	}
	return nil
}

// ---- A page ----

PAGE_TOP :: 3
PEDIA_TEXT_ROWS :: 11
PEDIA_MENU_TOP :: 15

Pedia_Page :: struct {
	kind:   Pedia_Kind,
	id:     int,
	cursor: int,
	scroll: int,
}

pedia_page_labels :: proc(kind: Pedia_Kind, labels: ^[MAX_LINKS + 1]string) -> (count: int) {
	links, n := pedia_links(kind)
	for i in 0 ..< n {
		labels[i] = pedia_link_names[links[i]]
	}
	labels[n] = "Back"
	return n + 1
}

pedia_page_draw :: proc(s: ^Pedia_Page, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	d: Doc
	title, hue := pedia_page_doc(u, s.kind, s.id, &d)
	text_put_centered(tb, 1, name_str(&title), hue)

	s.scroll = clamp(s.scroll, 0, max(0, d.count - PEDIA_TEXT_ROWS))
	for i in 0 ..< min(PEDIA_TEXT_ROWS, d.count - s.scroll) {
		line := d.lines[s.scroll + i]
		text_put(tb, 2 + line.indent, PAGE_TOP + i, line.text, line.hue)
	}
	if s.scroll > 0 {
		text_put(tb, TEXT_COLUMNS - 2, PAGE_TOP, "\x1e", .Dark_Gray)
	}
	if s.scroll + PEDIA_TEXT_ROWS < d.count {
		text_put(tb, TEXT_COLUMNS - 2, PAGE_TOP + PEDIA_TEXT_ROWS - 1, "\x1f", .Dark_Gray)
	}
	labels: [MAX_LINKS + 1]string
	count := pedia_page_labels(s.kind, &labels)
	menu_draw(tb, PEDIA_MENU_TOP, labels[:count], s.cursor)
	if d.count > PEDIA_TEXT_ROWS {
		text_put(tb, 2, 24, "Left/Right: read", .Dark_Gray)
	}
}

pedia_page_key :: proc(s: ^Pedia_Page, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	labels: [MAX_LINKS + 1]string
	count := pedia_page_labels(s.kind, &labels)
	switch menu_key(&s.cursor, count, key) {
	case .Next:
		s.scroll += PEDIA_TEXT_ROWS
	case .Previous:
		s.scroll -= PEDIA_TEXT_ROWS
	case .Chosen:
		links, n := pedia_links(s.kind)
		if s.cursor >= n {
			return Pop{}
		}
		return pedia_follow(u, s, links[s.cursor])
	case .Cancelled:
		return Pop{}
	case .None:
	}
	return nil // draw keeps the scroll in range
}

// The page's planet and system, whichever kind of page it is.
pedia_follow :: proc(u: ^Universe, s: ^Pedia_Page, link: Pedia_Link) -> Transition {
	faction_of_planet :: proc(u: ^Universe, planet: int) -> int {
		return int(u.planets[planet - 1].faction)
	}
	switch link {
	case .Planets:
		if s.kind == .Faction {
			return Push{Pedia_List{kind = .Planet, scope = .Faction, scope_id = s.id}}
		}
		return Push{Pedia_List{kind = .Planet, scope = .Star_System, scope_id = s.id}}
	case .Satellites:
		if s.kind == .Star_System {
			return Push{Pedia_List{kind = .Satellite, scope = .Star_System, scope_id = s.id}}
		}
		return Push{Pedia_List{kind = .Satellite, scope = .Planet, scope_id = s.id}}
	case .Factions:
		return Push{Pedia_List{kind = .Faction, scope = .Star_System, scope_id = s.id}}
	case .Faction:
		planet := s.id if s.kind == .Planet else int(u.satellites[s.id - 1].planet)
		return Push{Pedia_Page{kind = .Faction, id = faction_of_planet(u, planet)}}
	case .Star_System:
		system: int
		if s.kind == .Planet {
			system = int(u.planets[s.id - 1].star_system)
		} else {
			system = int(u.satellites[s.id - 1].star_system)
		}
		return Push{Pedia_Page{kind = .Star_System, id = system}}
	case .Planet:
		return Push{Pedia_Page{kind = .Planet, id = int(u.satellites[s.id - 1].planet)}}
	}
	return nil
}
