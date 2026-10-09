package game

import "core:slice"

// Every screen the player can be on. Per-screen state lives in the screen's struct.

Screen :: union {
	Main_Menu,
	About,
	Embark,
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
			return Push{Embark{}}
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

// ---- Embark (placeholder until universe generation is ported) ----

Embark :: struct {
	cursor: int,
}

embark_labels := [?]string{"Go", "Cancel"}

embark_draw :: proc(s: ^Embark, tb: ^Text_Buffer) {
	text_put_centered(tb, 3, "EMBARK", .Yellow)
	text_put_centered(tb, 6, "(settings coming soon)", .Dark_Gray)
	menu_draw(tb, 13, embark_labels[:], s.cursor)
}

embark_key :: proc(s: ^Embark, key: Key) -> Transition {
	switch menu_key(&s.cursor, len(embark_labels), key) {
	case .Chosen:
		if s.cursor == 1 {
			return Pop{}
		}
	case .Cancelled:
		return Pop{}
	case .None:
	}
	return nil
}
