package game

import "core:slice"

// Every screen the player can be on. Per-screen state lives in the screen's struct.

Screen :: union {
	Main_Menu,
	About,
	Embark,
	Generate,
}

screen_draw :: proc(screen: ^Screen, tb: ^Text_Buffer) {
	text_clear(tb)
	switch &s in screen {
	case Main_Menu:
		main_menu_draw(&s, tb)
	case About:
		about_draw(&s, tb)
	case Embark:
		embark_draw(&s, tb)
	case Generate:
		generate_draw(&s, tb)
	}
}

screen_key :: proc(screen: ^Screen, key: Key) -> Transition {
	switch &s in screen {
	case Main_Menu:
		return main_menu_key(&s, key)
	case About:
		return about_key(&s, key)
	case Embark:
		return embark_key(&s, key)
	case Generate:
		return generate_key(&s, key)
	}
	return nil
}

// ---- Main menu ----

Main_Menu_Choice :: enum {
	Embark,
	About,
}

main_menu_labels := [Main_Menu_Choice]string {
	.Embark = "Embark",
	.About  = "About",
}

Main_Menu :: struct {
	cursor: int,
}

main_menu_draw :: proc(s: ^Main_Menu, tb: ^Text_Buffer) {
	text_put_centered(tb, 3, "FREEDOM HATERS", .Yellow)
	text_put_centered(tb, 5, "OF SPLORR!!", .Yellow)
	text_put_centered(tb, 8, "Love FREEDOM or DIE!", .Light_Red)
	menu_draw(tb, 11, slice.enumerated_array(&main_menu_labels), s.cursor)
}

main_menu_key :: proc(s: ^Main_Menu, key: Key) -> Transition {
	if menu_key(&s.cursor, len(main_menu_labels), key) == .Chosen {
		switch Main_Menu_Choice(s.cursor) {
		case .Embark:
			return Push{embark_new()}
		case .About:
			return Push{About{}}
		}
	}
	return nil
}

// ---- About ----

About :: struct {}

about_draw :: proc(s: ^About, tb: ^Text_Buffer) {
	text_put_centered(tb, 3, "ABOUT", .Yellow)
	text_put_centered(tb, 7, "Freedom Haters of SPLORR!!", .White)
	text_put_centered(tb, 10, "Dev. Rule #1:", .Light_Gray)
	text_put_centered(tb, 12, "Love FREEDOM or DIE!", .Light_Red)
	text_put_centered(tb, 15, "#satire", .Dark_Gray)
	text_put_centered(tb, 22, "Press Enter", .Dark_Gray)
}

about_key :: proc(s: ^About, key: Key) -> Transition {
	if key == KEY_ENTER || key == KEY_ESCAPE {
		return Pop{}
	}
	return nil
}

// ---- Embark ----

Embark_Row :: enum {
	Go,
	Age,
	Density,
	Wealth,
	Factions,
	Cancel,
}

Embark :: struct {
	cursor:   int,
	settings: Embark_Settings,
}

embark_new :: proc() -> Embark {
	return {settings = DEFAULT_EMBARK_SETTINGS}
}

embark_draw :: proc(s: ^Embark, tb: ^Text_Buffer) {
	text_put_centered(tb, 3, "EMBARK", .Yellow)
	text_put_centered(tb, 5, "Left/Right changes a setting", .Dark_Gray)
	for row in Embark_Row {
		y := 8 + int(row) * 2
		selected := int(row) == s.cursor
		text_put(tb, 4, y, "> " if selected else "  ", .Yellow)
		label_hue := Hue.White if selected else Hue.Light_Gray
		switch row {
		case .Go:
			text_put(tb, 6, y, "Go", label_hue)
		case .Cancel:
			text_put(tb, 6, y, "Cancel", label_hue)
		case .Age:
			embark_draw_setting(tb, y, "Galactic Age", galactic_age_names[s.settings.age], selected)
		case .Density:
			embark_draw_setting(tb, y, "Galactic Density", galactic_density_names[s.settings.density], selected)
		case .Wealth:
			embark_draw_setting(tb, y, "Starting Wealth", starting_wealth_names[s.settings.wealth], selected)
		case .Factions:
			embark_draw_setting(tb, y, "Faction Count", faction_count_names[s.settings.faction_count], selected)
		}
	}
}

embark_draw_setting :: proc(tb: ^Text_Buffer, y: int, label, value: string, selected: bool) {
	text_put(tb, 6, y, label, .White if selected else .Light_Gray)
	text_put(tb, 24, y, value, .Light_Cyan if selected else .Cyan)
}

embark_key :: proc(s: ^Embark, key: Key) -> Transition {
	result := menu_key(&s.cursor, len(Embark_Row), key)
	row := Embark_Row(s.cursor)
	if result == .Cancelled {
		return Pop{}
	}
	delta := 0
	switch result {
	case .Next:
		delta = 1
	case .Previous:
		delta = -1
	case .Chosen:
		// Enter also steps a setting forward, so the touch pad needs no Left/Right.
		delta = 1
		switch row {
		case .Go:
			return Push{Generate{settings = s.settings}}
		case .Cancel:
			return Pop{}
		case .Age, .Density, .Wealth, .Factions:
		}
	case .None, .Cancelled:
	}
	if delta != 0 {
		switch row {
		case .Age:
			s.settings.age = cycle(s.settings.age, delta)
		case .Density:
			s.settings.density = cycle(s.settings.density, delta)
		case .Wealth:
			s.settings.wealth = cycle(s.settings.wealth, delta)
		case .Factions:
			s.settings.faction_count = cycle_faction_count(s.settings.faction_count, delta)
		case .Go, .Cancel:
		}
	}
	return nil
}

// ---- Generate (placeholder until universe generation is ported) ----

Generate :: struct {
	settings: Embark_Settings,
}

generate_draw :: proc(s: ^Generate, tb: ^Text_Buffer) {
	text_put_centered(tb, 3, "GENERATING", .Yellow)
	text_put_centered(tb, 6, "(universe generation coming soon)", .Dark_Gray)
	text_put(tb, 4, 10, "Age:", .Light_Gray)
	text_put(tb, 16, 10, galactic_age_names[s.settings.age], .Cyan)
	text_put(tb, 4, 12, "Density:", .Light_Gray)
	text_put(tb, 16, 12, galactic_density_names[s.settings.density], .Cyan)
	text_put(tb, 4, 14, "Wealth:", .Light_Gray)
	text_put(tb, 16, 14, starting_wealth_names[s.settings.wealth], .Cyan)
	text_put(tb, 4, 16, "Factions:", .Light_Gray)
	text_put(tb, 16, 16, faction_count_names[s.settings.faction_count], .Cyan)
	text_put_centered(tb, 22, "Press Escape", .Dark_Gray)
}

generate_key :: proc(s: ^Generate, key: Key) -> Transition {
	if key == KEY_ESCAPE || key == KEY_ENTER {
		return Pop{}
	}
	return nil
}
