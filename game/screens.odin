package game

import "core:slice"
import "core:time"

// Every screen the player can be on. Per-screen state lives in the screen's struct.

Screen :: union {
	Main_Menu,
	About,
	Embark,
	Generate,
	Navigation,
	Interaction_Screen,
	Action_Menu,
	Inventory_Screen,
	Item_Page,
	Mission_Offer,
	Confirm_Abandon_Delivery,
	Status_Screen,
	Pedia_Menu,
	Pedia_List,
	Pedia_Page,
	Equipment_Screen,
	Shipyard_Screen,
	Slot_Items,
	Trader,
	Buy_List,
	Sell_List,
	Quantity,
	Number_Entry,
	Confirm_Trade,
	Message,
	Game_Menu,
	Confirm_Abandon,
	Game_Over,
}

screen_draw :: proc(screen: ^Screen, tb: ^Text_Buffer, session: ^Session) {
	text_clear(tb)
	switch &s in screen {
	case Main_Menu:
		main_menu_draw(&s, tb)
	case About:
		about_draw(&s, tb)
	case Embark:
		embark_draw(&s, tb)
	case Generate:
		generate_draw(&s, tb, session)
	case Navigation:
		navigation_draw(&s, tb, session)
	case Interaction_Screen:
		interaction_draw(&s, tb, session)
	case Action_Menu:
		action_menu_draw(&s, tb, session)
	case Inventory_Screen:
		inventory_draw(&s, tb, session)
	case Item_Page:
		item_page_draw(&s, tb, session)
	case Mission_Offer:
		mission_offer_draw(&s, tb, session)
	case Confirm_Abandon_Delivery:
		confirm_abandon_delivery_draw(&s, tb, session)
	case Status_Screen:
		status_draw(&s, tb, session)
	case Pedia_Menu:
		pedia_menu_draw(&s, tb, session)
	case Pedia_List:
		pedia_list_draw(&s, tb, session)
	case Pedia_Page:
		pedia_page_draw(&s, tb, session)
	case Equipment_Screen:
		equipment_draw(&s, tb, session)
	case Shipyard_Screen:
		shipyard_draw(&s, tb, session)
	case Slot_Items:
		slot_items_draw(&s, tb, session)
	case Trader:
		trader_draw(&s, tb, session)
	case Buy_List:
		buy_list_draw(&s, tb, session)
	case Sell_List:
		sell_list_draw(&s, tb, session)
	case Quantity:
		quantity_draw(&s, tb, session)
	case Number_Entry:
		number_entry_draw(&s, tb, session)
	case Confirm_Trade:
		confirm_trade_draw(&s, tb, session)
	case Message:
		message_draw(&s, tb, session)
	case Game_Menu:
		game_menu_draw(&s, tb, session)
	case Confirm_Abandon:
		confirm_abandon_draw(&s, tb, session)
	case Game_Over:
		game_over_draw(&s, tb, session)
	}
}

screen_key :: proc(screen: ^Screen, key: Key, session: ^Session) -> Transition {
	switch &s in screen {
	case Main_Menu:
		return main_menu_key(&s, key)
	case About:
		return about_key(&s, key)
	case Embark:
		return embark_key(&s, key)
	case Generate:
		return generate_key(&s, key, session)
	case Navigation:
		return navigation_key(&s, key, session)
	case Interaction_Screen:
		return interaction_key(&s, key, session)
	case Action_Menu:
		return action_menu_key(&s, key, session)
	case Inventory_Screen:
		return inventory_key(&s, key, session)
	case Item_Page:
		return item_page_key(&s, key, session)
	case Mission_Offer:
		return mission_offer_key(&s, key, session)
	case Confirm_Abandon_Delivery:
		return confirm_abandon_delivery_key(&s, key, session)
	case Status_Screen:
		return status_key(&s, key, session)
	case Pedia_Menu:
		return pedia_menu_key(&s, key, session)
	case Pedia_List:
		return pedia_list_key(&s, key, session)
	case Pedia_Page:
		return pedia_page_key(&s, key, session)
	case Equipment_Screen:
		return equipment_key(&s, key, session)
	case Shipyard_Screen:
		return shipyard_key(&s, key, session)
	case Slot_Items:
		return slot_items_key(&s, key, session)
	case Trader:
		return trader_key(&s, key, session)
	case Buy_List:
		return buy_list_key(&s, key, session)
	case Sell_List:
		return sell_list_key(&s, key, session)
	case Quantity:
		return quantity_key(&s, key, session)
	case Number_Entry:
		return number_entry_key(&s, key, session)
	case Confirm_Trade:
		return confirm_trade_key(&s, key, session)
	case Message:
		return message_key(&s, key, session)
	case Game_Menu:
		return game_menu_key(&s, key, session)
	case Confirm_Abandon:
		return confirm_abandon_key(&s, key, session)
	case Game_Over:
		return game_over_key(&s, key, session)
	}
	return nil
}

// Timed work a screen does every frame, whether or not a key was pressed.
screen_tick :: proc(screen: ^Screen, session: ^Session) -> Transition {
	switch &s in screen {
	case Generate:
		return generate_tick(&s, session)
	case Navigation:
		return navigation_tick(&s, session)
	case Sell_List:
		return sell_list_tick(&s, session)
	case Main_Menu, About, Embark, Interaction_Screen, Action_Menu, Inventory_Screen, Item_Page, Mission_Offer, Confirm_Abandon_Delivery, Status_Screen, Pedia_Menu, Pedia_List, Pedia_Page, Equipment_Screen, Shipyard_Screen, Slot_Items, Trader, Buy_List, Quantity, Number_Entry, Confirm_Trade, Message, Game_Menu, Confirm_Abandon, Game_Over:
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

// ---- Generate ----

Generate :: struct {
	settings: Embark_Settings,
	started:  bool,
}

PROGRESS_BAR_WIDTH :: 30

generate_tick :: proc(s: ^Generate, session: ^Session) -> Transition {
	if !s.started {
		session_begin_generation(session, s.settings)
		s.started = true
	}
	if !session.generating {
		return nil
	}
	start := time.tick_now()
	for generator_step(&session.generator) {
		if time.tick_since(start) >= GENERATION_BUDGET {
			break
		}
	}
	if generator_done(&session.generator) {
		session_finish_generation(session)
		return Replace{Navigation{}}
	}
	return nil
}

generate_draw :: proc(s: ^Generate, tb: ^Text_Buffer, session: ^Session) {
	text_put_centered(tb, 3, "GENERATING", .Yellow)
	if !session.generating {
		return
	}
	g := &session.generator
	label, subject := generator_current(g)
	text_put_centered(tb, 9, label, .Light_Gray)
	text_put_centered(tb, 11, name_str(&subject), .Light_Cyan)

	remaining := generator_steps_remaining(g)
	total := g.steps_done + remaining
	filled := PROGRESS_BAR_WIDTH * g.steps_done / max(total, 1)
	left := (TEXT_COLUMNS - PROGRESS_BAR_WIDTH) / 2
	for i in 0 ..< PROGRESS_BAR_WIDTH {
		tb[15][left + i] = {' ', .Black, .Green if i < filled else .Dark_Gray}
	}
	text_put_int(tb, left, 17, g.steps_done, .White)
	text_put(tb, left + 6, 17, "steps done", .Dark_Gray)
	text_put_centered(tb, 22, "Escape cancels", .Dark_Gray)
}

generate_key :: proc(s: ^Generate, key: Key, session: ^Session) -> Transition {
	if key == KEY_ESCAPE {
		session_cancel_generation(session)
		return Pop{}
	}
	return nil
}
