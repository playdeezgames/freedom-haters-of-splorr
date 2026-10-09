#+build !js
package game

import "core:testing"

// An app that has generated a universe and is on the map.
app_on_the_map :: proc(t: ^testing.T, app: ^App) {
	start_generating(app)
	tick_until_generated(t, app)
}

on_screen :: proc(app: ^App, $S: typeid) -> bool {
	_, ok := stack_top(&app.stack)^.(S)
	return ok
}

@(test)
arrow_keys_move_the_ship_and_spend_fuel :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	fuel := u.avatar.fuel.current
	before := ship_pos(u)
	for key in ([]Key{KEY_UP, KEY_RIGHT, KEY_DOWN, KEY_LEFT}) {
		app_key(&app, key)
		if !on_screen(&app, Navigation) { // bumped into something: back out
			app_key(&app, KEY_ESCAPE)
		}
	}
	testing.expect_value(t, u.avatar.fuel.current, fuel - 4)
	_ = before
	testing.expect_value(t, u.turn, 5) // generation starts the clock at 1
}

@(test)
bumping_a_star_system_opens_its_interaction_and_approaching_goes_in :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	dir := park_beside(t, u, star_system_get(u, 1).actor)
	key := [Direction]Key {
		.North = KEY_UP,
		.East  = KEY_RIGHT,
		.South = KEY_DOWN,
		.West  = KEY_LEFT,
	}
	app_key(&app, key[dir])
	testing.expect(t, on_screen(&app, Interaction_Screen))
	// the screen names what you bumped
	testing.expect_value(t, app.text[1][(TEXT_COLUMNS - int(star_system_get(u, 1).name.len)) / 2].char, star_system_get(u, 1).name.buf[0])
	app_key(&app, KEY_ENTER) // Approach
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, map_get(u, ship_map(u)).kind, Map_Kind.Star_System)
	title := map_title(u, ship_map(u))
	testing.expect_value(t, app.text[0][int(title.len) - 1].char, u8('m')) // "... System"
}

@(test)
cancelling_an_interaction_changes_nothing_more :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	dir := park_beside(t, u, star_system_get(u, 1).actor)
	app_key(&app, KEY_UP if dir == .North else KEY_RIGHT if dir == .East else KEY_DOWN if dir == .South else KEY_LEFT)
	testing.expect(t, on_screen(&app, Interaction_Screen))
	before := ship_pos(u)
	app_key(&app, KEY_ESCAPE)
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, ship_pos(u), before)
	testing.expect_value(t, ship_map(u), u.galaxy)
	testing.expect(t, u.avatar.bumped == nil)
}

@(test)
the_border_of_a_system_can_be_left_from_the_map :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	system := star_system_get(u, 1)
	actor_relocate(u, u.avatar.actor, system.interior, {1, 10})
	app_key(&app, KEY_LEFT) // x=0 is the border
	testing.expect(t, on_screen(&app, Interaction_Screen))
	app_key(&app, KEY_ENTER) // Leave Star System
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, ship_map(u), u.galaxy)
}

@(test)
the_view_draws_border_arrows_and_planet_blocks :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	// in a planet's orbit, 5x5 body in the middle, arrows around the edge
	planet := planet_get(u, 1)
	vicinity := actor_get(u, planet.actor).interior
	body := actor_at(u, vicinity, map_center(.Planet_Vicinity))
	orbit := actor_get(u, body).interior
	actor_relocate(u, u.avatar.actor, orbit, {1, 1})
	app_draw(&app)
	// ship at (1,1): view origin is (-9,-9); the border row y=0 is view row 9, x=0 is column 9
	cell :: proc(app: ^App, vx, vy: int) -> Cell {
		return app.text[VIEW_TOP + vy][VIEW_LEFT + vx]
	}
	testing.expect_value(t, cell(&app, 10, 10).char, direction_glyph[.North]) // the ship
	testing.expect_value(t, cell(&app, 9 + 5, 9).char, u8(0x1E)) // top border: ▲
	testing.expect_value(t, cell(&app, 9, 9 + 5).char, u8(0x11)) // left border: ◄
	testing.expect_value(t, cell(&app, 9, 9).char, u8('\\')) // top-left corner
	testing.expect_value(t, cell(&app, 0, 0).char, u8(' ')) // outside the map
	// the 5x5 body: its center is at (5,5), so view (9+5, 9+5)
	testing.expect_value(t, cell(&app, 9 + 5, 9 + 5).char, GLYPH_PLANET_BODY)
	testing.expect_value(t, cell(&app, 9 + 5, 9 + 5).fg, planet_info[planet.type].hue)
	testing.expect_value(t, cell(&app, 9 + 3, 9 + 3).char, GLYPH_PLANET_BODY) // a corner of the footprint
	testing.expect_value(t, cell(&app, 9 + 2, 9 + 2).char, GLYPH_VOID) // just outside it
}

@(test)
with_no_fuel_the_action_menu_offers_distress_and_it_costs_jools :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.fuel.current = 0
	jools := u.avatar.jools
	turn := u.turn
	app_key(&app, KEY_UP)
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, u.turn, turn) // no fuel: no move, no turn
	app_key(&app, KEY_ENTER) // action menu
	testing.expect(t, on_screen(&app, Action_Menu))
	app_key(&app, KEY_ENTER) // Signal Distress, the first entry
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.fuel.current, MARK_I_CAPACITY)
	testing.expect_value(t, u.avatar.jools, jools - MARK_I_CAPACITY * EMERGENCY_FUEL_PRICE)
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
with_fuel_the_action_menu_is_just_cancel :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Action_Menu))
	jools := app.session.universe.avatar.jools
	app_key(&app, KEY_ENTER) // the only entry is Cancel
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, app.session.universe.avatar.jools, jools)
}

@(test)
running_out_of_oxygen_ends_the_game :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.oxygen.current = 1
	app_key(&app, KEY_RIGHT)
	if on_screen(&app, Interaction_Screen) {
		app_key(&app, KEY_ESCAPE)
	}
	app_tick(&app)
	testing.expect(t, on_screen(&app, Game_Over))
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Main_Menu))
	testing.expect_value(t, app.stack.count, 1)
	testing.expect(t, !app.session.in_play)
}

@(test)
an_unaffordable_refuel_is_bankruptcy :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.fuel.current = 0
	u.avatar.jools = u.avatar.jools_minimum + 5
	app_key(&app, KEY_ENTER)
	app_key(&app, KEY_ENTER) // Signal Distress
	app_key(&app, KEY_ENTER) // dismiss the message
	app_tick(&app)
	testing.expect(t, on_screen(&app, Game_Over))
	testing.expect(t, avatar_is_bankrupt(u) && !avatar_is_dead(u))
}

@(test)
the_game_menu_continues_and_declining_to_abandon_keeps_playing :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	app_key(&app, KEY_ESCAPE)
	testing.expect(t, on_screen(&app, Game_Menu))
	app_key(&app, KEY_ESCAPE) // back to the map
	testing.expect(t, on_screen(&app, Navigation))
	app_key(&app, KEY_ESCAPE)
	app_key(&app, KEY_ENTER) // Continue Game
	testing.expect(t, on_screen(&app, Navigation))
	app_key(&app, KEY_ESCAPE)
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_ENTER) // Abandon Game...
	app_key(&app, KEY_ENTER) // ...No
	testing.expect(t, on_screen(&app, Game_Menu))
	testing.expect(t, app.session.in_play)
}

@(test)
the_panel_shows_what_the_ship_has_left :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.oxygen.current = 100 // 40% of 250: yellow
	u.avatar.fuel.current = 25 // 10%: red
	app_draw(&app)
	col := VIEW_SIZE + 1
	testing.expect_value(t, app.text[8][col].char, u8('O')) // "O2: 40%"
	testing.expect_value(t, app.text[8][col + 4].fg, Hue.Yellow)
	testing.expect_value(t, app.text[11][col].char, u8('F')) // "Fuel: 10%"
	testing.expect_value(t, app.text[11][col + 6].fg, Hue.Light_Red)
}
