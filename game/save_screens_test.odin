#+build !js
package game

import "core:testing"

// What the top Message screen says, line by line, joined with '/'.
message_text :: proc(app: ^App) -> string {
	m, ok := stack_top(&app.stack)^.(Message)
	if !ok {
		return ""
	}
	out: [dynamic]u8
	context.allocator = context.temp_allocator
	for i in 0 ..< m.count {
		if i > 0 {
			append(&out, '/')
		}
		line := m.lines[i]
		append(&out, ..line.text[:line.len])
	}
	return string(out[:])
}

// On the map with the game menu open.
open_game_menu :: proc(app: ^App) {
	press(app, KEY_ESCAPE)
}

GAME_MENU_SCUM_SAVE :: 1
GAME_MENU_SAVE :: 2
GAME_MENU_SCUM_LOAD :: 3

choose_row :: proc(app: ^App, index: int) {
	for _ in 0 ..< index {
		app_key(app, KEY_DOWN)
	}
	app_key(app, KEY_ENTER)
}

@(test)
scum_save_then_scum_load_restores_the_game :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	defer memory_storage_reset()
	u := &app.session.universe
	u.avatar.jools = 777
	open_game_menu(&app)
	choose_row(&app, GAME_MENU_SCUM_SAVE)
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, message_text(&app), "Game Saved!")
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))

	u.avatar.jools = 5
	open_game_menu(&app)
	choose_row(&app, GAME_MENU_SCUM_LOAD)
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, app.stack.count, 2) // main menu under the map, as after generating
	testing.expect_value(t, app.session.universe.avatar.jools, 777)
	testing.expect(t, app.session.in_play)
}

@(test)
scum_load_without_a_save_says_so :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	defer memory_storage_reset()
	open_game_menu(&app)
	choose_row(&app, GAME_MENU_SCUM_LOAD)
	testing.expect_value(t, message_text(&app), "No Scum Slot!")
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Game_Menu))
	testing.expect(t, app.session.in_play)
}

@(test)
numbered_slots_show_their_description_and_warn_before_overwriting :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	defer memory_storage_reset()
	u := &app.session.universe
	u.avatar.jools = 910
	open_game_menu(&app)
	choose_row(&app, GAME_MENU_SAVE)
	testing.expect(t, on_screen(&app, Save_Screen))
	// Cancel is first, then Slots 1-5
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_DOWN) // Slot 2
	app_key(&app, KEY_ENTER)
	testing.expect_value(t, message_text(&app), "Game Saved!")
	testing.expect(t, slot_exists(app.session.storage, 2))
	testing.expect(t, !slot_exists(app.session.storage, 1))
	desc := slot_description(app.session.storage, 2)
	testing.expect_value(t, long_str(&desc), "Turn 1, 910 jools")
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))

	open_game_menu(&app)
	choose_row(&app, GAME_MENU_SAVE)
	labels, details: [MAX_SLOT_ROWS]Long_Text
	slot_row_texts(&app.session, save_slots[:], false, &labels, &details)
	testing.expect_value(t, long_str(&labels[1]), "Slot 1")
	testing.expect_value(t, long_str(&labels[2]), "Slot 2 (will overwrite)")
	testing.expect_value(t, long_str(&details[2]), "Turn 1, 910 jools")
	app_key(&app, KEY_ESCAPE) // cancel out of the list
	testing.expect(t, on_screen(&app, Game_Menu))
}

@(test)
the_load_list_offers_only_slots_that_exist :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	defer memory_storage_reset()
	u := &app.session.universe
	testing.expect_value(t, slot_save(app.session.storage, 3, u), Save_Result.Saved)
	testing.expect_value(t, slot_save(app.session.storage, 0, u), Save_Result.Saved)
	slots, count := loadable_slots(&app.session)
	testing.expect_value(t, count, 2)
	testing.expect_value(t, slots[0], 0)
	testing.expect_value(t, slots[1], 3)

	// back on the main menu, Scum Load now exists, and Load opens the list
	app_key(&app, KEY_ESCAPE)
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_ENTER) // Abandon Game
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_ENTER) // Yes
	testing.expect(t, on_screen(&app, Main_Menu))
	items, n := main_menu_items(&app.session)
	testing.expect_value(t, n, 4)
	testing.expect_value(t, items[1], Main_Menu_Choice.Scum_Load)
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_DOWN) // Load
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Load_Screen))
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_DOWN) // Slot 3
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect(t, app.session.in_play)
}

@(test)
load_says_so_when_nothing_is_saved :: proc(t: ^testing.T) {
	app: App
	memory_storage_reset()
	defer memory_storage_reset()
	app_init(&app, fixed_seed, memory_storage())
	defer app_destroy(&app)
	items, n := main_menu_items(&app.session)
	testing.expect_value(t, n, 3) // no Scum Load
	testing.expect_value(t, items[1], Main_Menu_Choice.Load)
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_ENTER)
	testing.expect_value(t, message_text(&app), "No Saves Exist!")
}

@(test)
damaged_and_foreign_slots_give_a_message_not_a_crash :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	defer memory_storage_reset()
	s := app.session.storage
	testing.expect_value(t, slot_save(s, 1, &app.session.universe), Save_Result.Saved)
	data, _ := storage_get(s, slot_key(1))
	defer delete(data)

	junk := []u8{1, 2, 3}
	testing.expect(t, s.write(slot_key(1), junk))
	r := load_slot(&app.session, 1, replacing = 0)
	pt, is_pop := r.(Pop_Then)
	testing.expect(t, is_pop)
	m, is_message := pt.screen.(Message)
	testing.expect(t, is_message)
	testing.expect_value(t, m.count, 1)

	other := make([]u8, len(data))
	defer delete(other)
	copy(other, data)
	other[4] += 1 // the format byte
	testing.expect(t, s.write(slot_key(1), other))
	r = load_slot(&app.session, 1, replacing = 0)
	pt = r.(Pop_Then)
	m = pt.screen.(Message)
	testing.expect_value(t, m.count, 2)
	testing.expect(t, app.session.in_play) // the running game is untouched
}

@(test)
a_failing_write_reports_could_not_save :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	defer memory_storage_reset()
	memory_write_fails = true
	open_game_menu(&app)
	choose_row(&app, GAME_MENU_SCUM_SAVE)
	testing.expect_value(t, message_text(&app), "Could not save!/The browser would not store it.")
	testing.expect(t, !slot_exists(app.session.storage, SCUM_SLOT))
}

@(test)
embark_settings_survive_a_new_session :: proc(t: ^testing.T) {
	memory_storage_reset()
	defer memory_storage_reset()
	app: App
	app_init(&app, fixed_seed, memory_storage())
	app_key(&app, KEY_ENTER) // Embark
	before := app.stack.items[app.stack.count - 1].(Embark).settings
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_RIGHT) // Age
	after := app.stack.items[app.stack.count - 1].(Embark).settings
	testing.expect(t, before != after)
	app_destroy(&app)

	app2: App
	app_init(&app2, fixed_seed, memory_storage())
	defer app_destroy(&app2)
	app_key(&app2, KEY_ENTER)
	testing.expect_value(t, app2.stack.items[app2.stack.count - 1].(Embark).settings, after)
}
