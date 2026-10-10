#+build !js
package game

import "core:testing"

// An app that has generated a universe and is on the map.
app_on_the_map :: proc(t: ^testing.T, app: ^App) {
	start_generating(app)
	tick_until_generated(t, app)
}

// The action menu is Status, Inventory, Equipment, (Signal Distress), Cancel.
open_inventory :: proc(app: ^App) {
	press(app, KEY_ENTER, KEY_DOWN, KEY_ENTER)
}

open_equipment :: proc(app: ^App) {
	press(app, KEY_ENTER, KEY_DOWN, KEY_DOWN, KEY_ENTER)
}

open_pedia :: proc(app: ^App) {
	press(app, KEY_ENTER, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_ENTER)
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
	press(&app, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Signal Distress, after Status, Inventory, Equipment and the pedia
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.fuel.current, MARK_I_CAPACITY)
	testing.expect_value(t, u.avatar.jools, jools - MARK_I_CAPACITY * EMERGENCY_FUEL_PRICE)
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
with_fuel_the_action_menu_has_no_distress :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Action_Menu))
	jools := app.session.universe.avatar.jools
	press(&app, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Status, Inventory, Equipment, SPLORRPedia, then Cancel: no distress while there is fuel
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
	press(&app, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Signal Distress
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
	for _ in 0 ..< 4 {
		app_key(&app, KEY_DOWN)
	}
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

// Into the first planet's orbit, the ship parked beside its star dock; returns the key that bumps it.
park_beside_the_dock :: proc(t: ^testing.T, u: ^Universe) -> Key {
	dock := first_dock(u)
	orbit := actor_get(u, dock).map_id
	// leave the ship wherever it lands in this orbit, then park next to the dock
	actor_relocate(u, u.avatar.actor, orbit, {1, 1})
	dir := park_beside(t, u, dock)
	keys := [Direction]Key {
		.North = KEY_UP,
		.East  = KEY_RIGHT,
		.South = KEY_DOWN,
		.West  = KEY_LEFT,
	}
	return keys[dir]
}

@(test)
refilling_oxygen_at_a_star_dock_charges_jools_and_shows_a_receipt :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	key := park_beside_the_dock(t, u)
	u.avatar.oxygen.current = u.avatar.oxygen.maximum - 25
	jools := u.avatar.jools
	app_key(&app, key)
	testing.expect(t, on_screen(&app, Interaction_Screen))
	app_key(&app, KEY_ENTER) // Refill Oxygen, the only action
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.oxygen.current, u.avatar.oxygen.maximum) // the bump cost 1, so 26 were bought
	testing.expect_value(t, u.avatar.jools, jools - 3)
	testing.expect_value(t, app.text[6][(TEXT_COLUMNS - len("Oxygen Refilled!")) / 2].char, u8('O'))
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
declining_a_star_dock_costs_nothing_more :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	key := park_beside_the_dock(t, u)
	u.avatar.fuel.current = u.avatar.fuel.maximum
	u.avatar.oxygen.current = u.avatar.oxygen.maximum
	jools := u.avatar.jools
	app_key(&app, key)
	// bumping spends 1 oxygen and 1 fuel, so the tanks are 1 short and a refill is on offer; decline it
	testing.expect(t, on_screen(&app, Interaction_Screen))
	app_key(&app, KEY_ESCAPE)
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, u.avatar.jools, jools)
}

@(test)
the_dock_is_drawn_and_described :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	key := park_beside_the_dock(t, u)
	app_draw(&app)
	found := false
	for y in VIEW_TOP ..< VIEW_TOP + VIEW_SIZE {
		for x in VIEW_LEFT ..< VIEW_LEFT + VIEW_SIZE {
			cell := app.text[y][x]
			if cell.char == GLYPH_STAR_DOCK && cell.fg == .Brown {
				found = true
			}
		}
	}
	testing.expect(t, found)
	app_key(&app, key)
	// "<planet> Star Dock" title on row 1, faction on row 4
	title_end := 0
	for cell, i in app.text[1] {
		if cell.char != ' ' {
			title_end = i
		}
	}
	testing.expect_value(t, app.text[1][title_end].char, u8('k')) // ...Dock
	testing.expect_value(t, app.text[4][2].char, u8('F')) // Faction:
}

@(test)
planet_info_says_whether_the_air_is_breathable :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.planets[0].type = .Terran
	planet_actor := actor_get(u, u.planets[0].actor)
	body := actor_at(u, planet_actor.interior, map_center(.Planet_Vicinity))
	u.avatar.bumped = body
	app_draw(&app)
	tb: Text_Buffer
	text_clear(&tb)
	draw_bump_info(&tb, u, u.avatar.bumped, 4)
	air_row := -1
	for row in 0 ..< TEXT_ROWS {
		if tb[row][2].char == 'A' && tb[row][3].char == 'i' && tb[row][4].char == 'r' {
			air_row = row
		}
	}
	testing.expect(t, air_row >= 0)
	testing.expect_value(t, tb[air_row][7].fg, Hue.Light_Green)
	u.planets[0].type = .Toxic
	text_clear(&tb)
	draw_bump_info(&tb, u, u.avatar.bumped, 4)
	testing.expect_value(t, tb[air_row][7].fg, Hue.Light_Red)
}

// ---- debris, salvage and the inventory ----

// The first debris pile in the first system that has one, with the ship on that map beside it.
park_beside_debris :: proc(t: ^testing.T, u: ^Universe) -> (Actor_Id, Key) {
	for a, i in u.actors {
		if a.kind == .Debris && a.map_id != 0 {
			id := Actor_Id(i + 1)
			actor_relocate(u, u.avatar.actor, a.map_id, {1, 1})
			dir := park_beside(t, u, id)
			keys := [Direction]Key {
				.North = KEY_UP,
				.East  = KEY_RIGHT,
				.South = KEY_DOWN,
				.West  = KEY_LEFT,
			}
			return id, keys[dir]
		}
	}
	testing.fail_now(t, "no debris")
}

@(test)
salvaging_debris_fills_the_hold_and_clears_the_pile :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	debris, key := park_beside_debris(t, u)
	loot := actor_get(u, debris).loot
	system := star_system_get(u, actor_get(u, debris).star_system)
	piles := system.scrap
	app_key(&app, key)
	testing.expect(t, on_screen(&app, Interaction_Screen))
	app_key(&app, KEY_ENTER) // Salvage Scrap
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, len(u.avatar.inventory), loot)
	testing.expect_value(t, system.scrap, piles - 1)
	testing.expect_value(t, actor_get(u, debris).map_id, Map_Id(0))
	testing.expect_value(t, actor_at(u, ship_map(u), actor_get(u, debris).pos), Actor_Id(0))
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
the_inventory_lists_stacks_and_describes_them :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	for _ in 0 ..< 3 {
		append(&u.avatar.inventory, item_add(u, item_new(.Scrap)))
	}
	append(&u.avatar.inventory, item_add(u, item_new(.Oxygen_Tank)))
	open_inventory(&app)
	testing.expect(t, on_screen(&app, Inventory_Screen))
	// "Cancel" first, then the stacks in the order they first appear: Scrap (x3), Oxygen Tank (x1)
	app_key(&app, KEY_DOWN)
	app_key(&app, KEY_ENTER) // Scrap
	page, ok := stack_top(&app.stack)^.(Item_Page)
	testing.expect(t, ok)
	testing.expect_value(t, page.kind, Item_Kind.Scrap)
	testing.expect_value(t, page.count, 3)
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Inventory_Screen))
	app_key(&app, KEY_ESCAPE)
	testing.expect(t, on_screen(&app, Action_Menu))
}

@(test)
an_empty_hold_says_so :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	open_inventory(&app)
	testing.expect(t, on_screen(&app, Inventory_Screen))
	testing.expect_value(t, app.text[6][(TEXT_COLUMNS - len("Yer hold is empty.")) / 2].char, u8('Y'))
	app_key(&app, KEY_ENTER) // the only entry is Cancel
	testing.expect(t, on_screen(&app, Action_Menu))
}

@(test)
debris_is_drawn_on_the_system_map :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	debris, _ := park_beside_debris(t, u)
	app_draw(&app)
	// the pile is next to the ship, whose view cell is the center
	d := actor_get(u, debris).pos - ship_pos(u)
	cell := app.text[VIEW_TOP + VIEW_SIZE / 2 + d.y][VIEW_LEFT + VIEW_SIZE / 2 + d.x]
	testing.expect_value(t, cell.char, GLYPH_DEBRIS)
}

// ---- the trading post ----

// Into the first planet's orbit with the ship beside its first trading post; returns the key that bumps it.
park_beside_the_post :: proc(t: ^testing.T, u: ^Universe) -> Key {
	post := first_post(u)
	actor_relocate(u, u.avatar.actor, actor_get(u, post).map_id, {1, 1})
	dir := park_beside(t, u, post)
	keys := [Direction]Key {
		.North = KEY_UP,
		.East  = KEY_RIGHT,
		.South = KEY_DOWN,
		.West  = KEY_LEFT,
	}
	return keys[dir]
}

press :: proc(app: ^App, keys: ..Key) {
	for k in keys {
		app_key(app, k)
	}
}

// At the trader screen of the first post.
at_the_trader :: proc(t: ^testing.T, app: ^App) {
	app_on_the_map(t, app)
	u := &app.session.universe
	key := park_beside_the_post(t, u)
	app_key(app, key)
	testing.expect(t, on_screen(app, Interaction_Screen))
	app_key(app, KEY_ENTER) // Trade
	testing.expect(t, on_screen(app, Trader))
}

@(test)
bumping_a_trading_post_opens_the_trader_and_leaving_goes_back :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	press(&app, KEY_DOWN, KEY_ENTER) // entries: Buy, Leave (no scrap to sell)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
the_trader_offers_sell_only_when_you_have_scrap :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	press(&app, KEY_DOWN)
	testing.expect_value(t, app.stack.count, 4)
	press(&app, KEY_ESCAPE)
	testing.expect(t, on_screen(&app, Navigation))

	append(&u.avatar.inventory, item_add(u, item_new(.Scrap)))
	key := park_beside_the_post(t, u)
	press(&app, key, KEY_ENTER)
	testing.expect(t, on_screen(&app, Trader))
	press(&app, KEY_DOWN, KEY_ENTER) // Buy, Sell, Leave: this is Sell
	testing.expect(t, on_screen(&app, Sell_List))
}

@(test)
buying_one_thing_through_the_screens :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	jools := u.avatar.jools
	press(&app, KEY_ENTER) // Buy
	testing.expect(t, on_screen(&app, Buy_List))
	press(&app, KEY_DOWN, KEY_ENTER) // first item: Oxygen Tank
	testing.expect(t, on_screen(&app, Quantity))
	press(&app, KEY_DOWN, KEY_ENTER) // One
	testing.expect(t, on_screen(&app, Confirm_Trade))
	press(&app, KEY_ENTER) // Yes
	testing.expect(t, on_screen(&app, Buy_List)) // straight back to the list
	testing.expect_value(t, inventory_count(u, .Oxygen_Tank), 1)
	testing.expect_value(t, u.avatar.jools, jools - 5)
}

@(test)
saying_no_buys_nothing :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	jools := u.avatar.jools
	press(&app, KEY_ENTER, KEY_DOWN, KEY_ENTER, KEY_DOWN, KEY_ENTER) // Buy, Oxygen Tank, One
	testing.expect(t, on_screen(&app, Confirm_Trade))
	press(&app, KEY_DOWN, KEY_ENTER) // No
	testing.expect(t, on_screen(&app, Quantity))
	testing.expect_value(t, u.avatar.jools, jools)
	testing.expect_value(t, len(u.avatar.inventory), 0)
}

@(test)
a_specific_number_is_set_with_the_arrows :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	press(&app, KEY_ENTER, KEY_DOWN, KEY_ENTER) // Buy, Oxygen Tank
	press(&app, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_ENTER) // One, Maximum, Specific number...
	n, ok := stack_top(&app.stack)^.(Number_Entry)
	testing.expect(t, ok)
	testing.expect_value(t, n.value, 1)
	press(&app, KEY_UP, KEY_UP, KEY_UP, KEY_UP) // 5
	press(&app, KEY_RIGHT) // 15
	press(&app, KEY_LEFT, KEY_LEFT) // clamps at 1
	n, _ = stack_top(&app.stack)^.(Number_Entry)
	testing.expect_value(t, n.value, 1)
	press(&app, KEY_UP, KEY_UP, KEY_UP, KEY_UP, KEY_ENTER) // 5
	testing.expect(t, on_screen(&app, Confirm_Trade))
	press(&app, KEY_ENTER) // Yes
	testing.expect(t, on_screen(&app, Buy_List)) // three screens popped
	testing.expect_value(t, inventory_count(u, .Oxygen_Tank), 5)
}

@(test)
typed_digits_work_and_are_capped_at_what_you_can_afford :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.jools = 100 // 20 tanks
	press(&app, KEY_ENTER, KEY_DOWN, KEY_ENTER)
	press(&app, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_ENTER)
	press(&app, Key('1'), Key('2'))
	n, _ := stack_top(&app.stack)^.(Number_Entry)
	testing.expect_value(t, n.value, 12) // the first digit replaces the starting 1
	press(&app, KEY_BACKSPACE)
	n, _ = stack_top(&app.stack)^.(Number_Entry)
	testing.expect_value(t, n.value, 1) // backspace drops the last digit
	press(&app, Key('9'), Key('9'))
	n, _ = stack_top(&app.stack)^.(Number_Entry)
	testing.expect_value(t, n.value, 20) // 199 is more than 100 jools buys
}

@(test)
you_cannot_pick_what_you_cannot_afford :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.jools = 3
	press(&app, KEY_ENTER, KEY_DOWN, KEY_ENTER) // Buy, Oxygen Tank (5)
	testing.expect(t, on_screen(&app, Quantity))
	press(&app, KEY_DOWN, KEY_ENTER) // only Cancel exists, so this wraps to it
	testing.expect(t, on_screen(&app, Buy_List))
	testing.expect_value(t, len(u.avatar.inventory), 0)
}

@(test)
selling_scrap_through_the_screens :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	press(&app, KEY_ESCAPE)
	for _ in 0 ..< 10 {
		append(&u.avatar.inventory, item_add(u, item_new(.Scrap)))
	}
	jools := u.avatar.jools
	key := park_beside_the_post(t, u)
	press(&app, key, KEY_ENTER)
	press(&app, KEY_DOWN, KEY_ENTER) // Sell
	testing.expect(t, on_screen(&app, Sell_List))
	press(&app, KEY_DOWN, KEY_ENTER) // Scrap (x10) @1
	testing.expect(t, on_screen(&app, Quantity))
	// Cancel, One, All (10), Half (5), Specific
	press(&app, KEY_DOWN, KEY_DOWN, KEY_ENTER) // All
	testing.expect(t, on_screen(&app, Confirm_Trade))
	press(&app, KEY_ENTER) // Yes
	testing.expect_value(t, inventory_count(u, .Scrap), 0)
	testing.expect_value(t, u.avatar.jools, jools - 0 + 10 - 1 * 0 - 1 * 0) // the walk back cost no jools
	app_tick(&app) // nothing left to list: back to the trader
	testing.expect(t, on_screen(&app, Trader))
}

@(test)
selling_half_keeps_the_rest :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	press(&app, KEY_ESCAPE)
	for _ in 0 ..< 10 {
		append(&u.avatar.inventory, item_add(u, item_new(.Scrap)))
	}
	key := park_beside_the_post(t, u)
	press(&app, key, KEY_ENTER, KEY_DOWN, KEY_ENTER, KEY_DOWN, KEY_ENTER) // Sell, Scrap
	press(&app, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Half
	press(&app, KEY_ENTER) // Yes
	testing.expect_value(t, inventory_count(u, .Scrap), 5)
	testing.expect(t, on_screen(&app, Sell_List))
}

@(test)
using_a_tank_from_the_inventory :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	trade_buy(u, .Oxygen_Tank, 0, 1)
	u.avatar.oxygen.current = 100
	open_inventory(&app)
	press(&app, KEY_DOWN, KEY_ENTER) // Oxygen Tank stack
	page, ok := stack_top(&app.stack)^.(Item_Page)
	testing.expect(t, ok && page.kind == .Oxygen_Tank)
	press(&app, KEY_ENTER) // Use
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.oxygen.current, 200)
	press(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Inventory_Screen))
	// the tank became scrap, so the stack list changed and the cursor must still be valid
	app_draw(&app)
	testing.expect_value(t, inventory_count(u, .Scrap), 1)
}

@(test)
using_a_fuel_rod_from_the_inventory :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	trade_buy(u, .Fuel_Rod, 0, 1)
	u.avatar.fuel.current = 20
	open_inventory(&app)
	press(&app, KEY_DOWN, KEY_ENTER, KEY_ENTER) // Fuel Rod, Use
	testing.expect_value(t, u.avatar.fuel.current, 120)
	testing.expect_value(t, len(u.avatar.inventory), 0)
	press(&app, KEY_ENTER)
	app_draw(&app) // an empty hold after the rod is gone must still draw
	testing.expect(t, on_screen(&app, Inventory_Screen))
}

@(test)
a_tank_saves_you_from_suffocating_and_says_so :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	trade_buy(u, .Oxygen_Tank, 0, 1)
	u.avatar.oxygen.current = 1
	app_key(&app, KEY_RIGHT)
	if on_screen(&app, Interaction_Screen) {
		app_key(&app, KEY_ESCAPE)
	}
	app_tick(&app)
	testing.expect(t, !on_screen(&app, Game_Over))
	testing.expect(t, on_screen(&app, Message)) // "Replenished Oxygen!"
	testing.expect(t, !u.avatar.auto_used.used) // reported once
	testing.expect_value(t, u.avatar.oxygen.current, 100)
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
a_long_item_page_scrolls_and_every_item_page_draws :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	append(&u.avatar.inventory, item_add(u, item_new(.Fuel_Scoop)))
	open_inventory(&app)
	press(&app, KEY_DOWN, KEY_ENTER) // first stack
	page, ok := stack_top(&app.stack)^.(Item_Page)
	testing.expect(t, ok && page.kind == .Fuel_Scoop)
	app_draw(&app)
	testing.expect_value(t, app.text[PAGE_TEXT_TOP][2].char, u8('T')) // "Tap into the Power..."
	testing.expect_value(t, app.text[PAGE_TEXT_TOP + page_window(.Fuel_Scoop) - 1][TEXT_COLUMNS - 2].char, u8(0x1F)) // more below
	press(&app, KEY_DOWN, KEY_DOWN, KEY_DOWN)
	page, _ = stack_top(&app.stack)^.(Item_Page)
	testing.expect_value(t, page.scroll, 3)
	testing.expect_value(t, app.text[PAGE_TEXT_TOP][TEXT_COLUMNS - 2].char, u8(0x1E)) // more above
	for _ in 0 ..< 200 {
		press(&app, KEY_DOWN)
	}
	page, _ = stack_top(&app.stack)^.(Item_Page)
	pl: Page_Lines
	page_lines(nil, item_new(.Fuel_Scoop), &pl)
	testing.expect_value(t, page.scroll, pl.count - page_window(.Fuel_Scoop)) // clamped at the end
	press(&app, KEY_UP, KEY_UP, KEY_ENTER)
	testing.expect(t, on_screen(&app, Inventory_Screen))
}

@(test)
every_kind_of_item_page_can_be_drawn :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	for kind in Item_Kind {
		marks := []int{0}
		if item_info[kind].marked {
			marks = []int{1, 2, 3, 4, 5}
		}
		for mark in marks {
			tb: Text_Buffer
			text_clear(&tb)
			page := Item_Page{kind = kind, mark = mark, count = 1}
			item_page_draw(&page, &tb, &app.session)
			title := 0
			for cell in tb[1] {
				if cell.char != ' ' {title += 1}
			}
			testing.expect(t, title >= len("Scrap")) // a title
		}
	}
}

// ---- the shipyard and the equipment view ----

// At the shipyard screen of a yard on a planet of the given tech level, with `jools` in the wallet.
at_the_shipyard :: proc(t: ^testing.T, app: ^App, tech, jools: int) {
	app_on_the_map(t, app)
	u := &app.session.universe
	yard := yard_with_tech(u, tech)
	u.avatar.jools = jools
	actor_relocate(u, u.avatar.actor, actor_get(u, yard).map_id, {1, 1})
	dir := park_beside(t, u, yard)
	keys := [Direction]Key {
		.North = KEY_UP,
		.East  = KEY_RIGHT,
		.South = KEY_DOWN,
		.West  = KEY_LEFT,
	}
	app_key(app, keys[dir])
	testing.expect(t, on_screen(app, Interaction_Screen))
	app_key(app, KEY_ENTER) // Enter Shipyard
	testing.expect(t, on_screen(app, Shipyard_Screen))
}

@(test)
the_shipyard_lists_every_slot_and_what_is_in_it :: proc(t: ^testing.T) {
	app: App
	at_the_shipyard(t, &app, 5, 1000)
	defer app_destroy(&app)
	app_draw(&app)
	// "Life Support: EterniVita Mark I" is the first entry, at row 7, label column 4
	testing.expect_value(t, app.text[7][4].char, u8('L'))
	testing.expect_value(t, app.text[9][4].char, u8('F')) // Fuel Supply
	testing.expect_value(t, app.text[11][4].char, u8('A')) // Accessory(0): (empty)
	press(&app, KEY_ESCAPE)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
installing_an_accessory_through_the_screens :: proc(t: ^testing.T) {
	app: App
	at_the_shipyard(t, &app, 5, 1000)
	defer app_destroy(&app)
	u := &app.session.universe
	conc := in_hold(u, .Atmospheric_Concentrator)
	press(&app, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Accessory(0)
	testing.expect(t, on_screen(&app, Slot_Items))
	press(&app, KEY_DOWN, KEY_ENTER) // Cancel, then the concentrator
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.equipment[.Accessory_0], conc)
	testing.expect_value(t, u.avatar.jools, 1000 - 25)
	press(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Shipyard_Screen)) // straight back to the slot list
}

@(test)
the_slot_screen_shows_each_items_fee_and_level :: proc(t: ^testing.T) {
	app: App
	at_the_shipyard(t, &app, 5, 1000)
	defer app_destroy(&app)
	u := &app.session.universe
	spare := in_hold(u, .Fuel_Supply, 2)
	item_get(u, spare).level = 120 // a partly used spare
	press(&app, KEY_DOWN, KEY_ENTER) // Fuel Supply
	app_draw(&app)
	// entries: Cancel at row 7, the spare at row 9 with its detail line on row 10
	testing.expect_value(t, app.text[9][4].char, u8('S')) // StarLume Fuel Mark II
	detail := ""
	line: [TEXT_COLUMNS]u8
	for cell, i in app.text[10] {
		line[i] = cell.char
	}
	detail = string(line[:])
	testing.expect(t, len(detail) > 0 && detail[4] == 'H') // "Holds 120/500. Fee 25."
}

@(test)
a_low_tech_yard_refuses_and_says_so :: proc(t: ^testing.T) {
	app: App
	at_the_shipyard(t, &app, 1, 1000)
	defer app_destroy(&app)
	u := &app.session.universe
	in_hold(u, .Fuel_Supply, 3)
	press(&app, KEY_DOWN, KEY_ENTER, KEY_DOWN, KEY_ENTER) // Fuel Supply, the Mark III
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.jools, 1000)
	testing.expect_value(t, app.text[6][(TEXT_COLUMNS - len("Insufficient Tech Level!")) / 2].char, u8('I'))
	testing.expect_value(t, item_get(u, u.avatar.equipment[.Fuel_Supply]).mark, 1)
}

@(test)
a_poor_ship_refuses_and_says_so :: proc(t: ^testing.T) {
	app: App
	at_the_shipyard(t, &app, 5, 5)
	defer app_destroy(&app)
	u := &app.session.universe
	in_hold(u, .Fuel_Supply, 2)
	press(&app, KEY_DOWN, KEY_ENTER, KEY_DOWN, KEY_ENTER)
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.jools, 5)
	testing.expect_value(t, app.text[6][(TEXT_COLUMNS - len("Insufficient funds!")) / 2].char, u8('I'))
}

@(test)
a_long_receipt_wraps_instead_of_running_off_the_screen :: proc(t: ^testing.T) {
	app: App
	at_the_shipyard(t, &app, 9, 1000)
	defer app_destroy(&app)
	u := &app.session.universe
	in_hold(u, .Life_Support, 3)
	press(&app, KEY_ENTER, KEY_DOWN, KEY_ENTER) // Life Support slot, the Mark III
	testing.expect(t, on_screen(&app, Message))
	// "Uninstalled EterniVita Mark I from Life Support." is 48 characters: wrapped at column 2
	testing.expect_value(t, app.text[6][2].char, u8('U'))
	testing.expect(t, app.text[7][2].char != ' ') // its second row
}

@(test)
uninstalling_an_accessory_has_its_own_entry :: proc(t: ^testing.T) {
	app: App
	at_the_shipyard(t, &app, 5, 1000)
	defer app_destroy(&app)
	u := &app.session.universe
	equip_item(u, .Accessory_0, item_add(u, item_new(.Atmospheric_Concentrator)), charge = false)
	press(&app, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Accessory(0)
	press(&app, KEY_DOWN, KEY_ENTER) // Cancel, then Uninstall
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.equipment[.Accessory_0], Item_Id(0))
	testing.expect_value(t, u.avatar.jools, 1000 - 15)
	testing.expect_value(t, inventory_count(u, .Atmospheric_Concentrator), 1)
}

@(test)
mandatory_slots_have_no_uninstall_entry :: proc(t: ^testing.T) {
	app: App
	at_the_shipyard(t, &app, 5, 1000)
	defer app_destroy(&app)
	u := &app.session.universe
	press(&app, KEY_ENTER) // Life Support: nothing in the hold fits, so only Cancel
	testing.expect(t, on_screen(&app, Slot_Items))
	entries: [MAX_SLOT_ENTRIES]Slot_Entry
	testing.expect_value(t, slot_entries(u, .Life_Support, &entries), 1)
	press(&app, KEY_ENTER) // Cancel
	testing.expect(t, on_screen(&app, Shipyard_Screen))
}

@(test)
the_equipment_view_lists_slots_and_opens_item_pages :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	open_equipment(&app)
	testing.expect(t, on_screen(&app, Equipment_Screen))
	press(&app, KEY_DOWN, KEY_ENTER) // Life Support
	page, ok := stack_top(&app.stack)^.(Item_Page)
	testing.expect(t, ok && page.kind == .Life_Support && page.mark == 1)
	app_draw(&app)
	testing.expect_value(t, app.text[2][2].char, u8(' ')) // no "You have" for something installed
	press(&app, KEY_ENTER)
	press(&app, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Accessory(0): empty, nothing opens
	testing.expect(t, on_screen(&app, Equipment_Screen))
	press(&app, KEY_ESCAPE)
	testing.expect(t, on_screen(&app, Action_Menu))
}

@(test)
bumping_a_star_with_a_scoop_offers_free_fuel :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	equip_item(u, .Accessory_0, item_add(u, item_new(.Fuel_Scoop)), charge = false)
	u.avatar.fuel.current = 100
	// into the first system's star vicinity: approach its marker, then bump the star
	system := star_system_get(u, 1)
	actor_relocate(u, u.avatar.actor, system.interior, {1, 1})
	vicinity_marker := actor_at(u, system.interior, map_center(.Star_System))
	dir := park_beside(t, u, vicinity_marker)
	keys := [Direction]Key {
		.North = KEY_UP,
		.East  = KEY_RIGHT,
		.South = KEY_DOWN,
		.West  = KEY_LEFT,
	}
	app_key(&app, keys[dir])
	app_key(&app, KEY_ENTER) // Approach
	star := actor_at(u, ship_map(u), map_center(.Star_Vicinity))
	dir = park_beside(t, u, star)
	app_key(&app, keys[dir])
	testing.expect(t, on_screen(&app, Interaction_Screen))
	app_key(&app, KEY_ENTER) // Use Fuel Scoop
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.fuel.current, 250)
}

// ---- delivery missions ----

direction_key :: proc(dir: Direction) -> Key {
	keys := [Direction]Key {
		.North = KEY_UP,
		.East  = KEY_RIGHT,
		.South = KEY_DOWN,
		.West  = KEY_LEFT,
	}
	return keys[dir]
}

// Flies the ship beside `dock` and bumps it, leaving the app on the interaction screen.
bump_dock :: proc(t: ^testing.T, app: ^App, dock: Actor_Id) {
	u := &app.session.universe
	actor_relocate(u, u.avatar.actor, actor_get(u, dock).map_id, {1, 1})
	dir := park_beside(t, u, dock)
	app_key(app, direction_key(dir))
	testing.expect(t, on_screen(app, Interaction_Screen))
}

// On the interaction screen: moves down to `kind` and chooses it.
pick :: proc(t: ^testing.T, app: ^App, kind: Interaction) {
	u := &app.session.universe
	list, n := interactions_for(u, u.avatar.bumped)
	for i in 0 ..< n {
		if list[i] == kind {
			for _ in 0 ..< i {
				app_key(app, KEY_DOWN)
			}
			app_key(app, KEY_ENTER)
			return
		}
	}
	testing.fail_now(t, "that interaction is not on offer")
}

@(test)
a_dock_with_a_delivery_offers_it_and_one_bound_there_offers_completion :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	dock := home_dock(&u)
	list, n := offered(&u, dock)
	found := false
	for i in 0 ..< n {
		found ||= list[i] == .Delivery_Mission
	}
	testing.expect(t, found)
	label: Name
	testing.expect_value(t, interaction_label(&u, .Delivery_Mission, nil, &label), "Delivery Mission...")
	testing.expect_value(t, interaction_label(&u, .Complete_Delivery, nil, &label), "Complete Delivery...")

	planet := actor_get(&u, dock).planet
	carry(&u, planet, planet, 10) // a delivery bound for this very planet
	list, n = offered(&u, dock)
	found = false
	for i in 0 ..< n {
		found ||= list[i] == .Complete_Delivery
	}
	testing.expect(t, found)
}

@(test)
accepting_a_delivery_through_the_screens :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	dock := home_dock(u)
	offer := actor_get(u, dock).offer
	bump_dock(t, &app, dock)
	pick(t, &app, .Delivery_Mission)
	testing.expect(t, on_screen(&app, Mission_Offer))
	press(&app, KEY_ENTER) // Accept
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, deliveries_carried(u), 1)
	testing.expect_value(t, u.avatar.inventory[0], offer)
	testing.expect(t, actor_get(u, dock).offer != offer)
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
the_offer_screen_describes_the_delivery_and_where_to_find_it :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	dock := home_dock(u)
	m := item_get(u, actor_get(u, dock).offer).mission
	bump_dock(t, &app, dock)
	pick(t, &app, .Delivery_Mission)
	// the rows hold: Item, Destination, System at x,y, Recipient, Reward (some wrap)
	text := ""
	all: [TEXT_ROWS * TEXT_COLUMNS]u8
	for row, r in app.text {
		for cell, c in row {
			all[r * TEXT_COLUMNS + c] = cell.char
		}
	}
	text = string(all[:])
	planet := planet_get(u, m.destination)
	system := star_system_get(u, planet.star_system)
	testing.expect(t, contains(text, "Item: "))
	testing.expect(t, contains(text, "Destination: "))
	testing.expect(t, contains(text, name_str(&system.name)))
	testing.expect(t, contains(text, "Recipient: "))
	testing.expect(t, contains(text, "Jools Reward: "))
}

contains :: proc(haystack, needle: string) -> bool {
	for i in 0 ..= len(haystack) - len(needle) {
		if haystack[i:][:len(needle)] == needle {
			return true
		}
	}
	return false
}

@(test)
with_a_poor_standing_the_offer_cannot_be_accepted :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	dock := away_dock(u)
	mission_accept(u, dock) // one in the hold already: reputation 0 allows only one
	bump_dock(t, &app, dock)
	pick(t, &app, .Delivery_Mission)
	testing.expect(t, on_screen(&app, Mission_Offer))
	all: [TEXT_ROWS * TEXT_COLUMNS]u8
	for row, r in app.text {
		for cell, c in row {
			all[r * TEXT_COLUMNS + c] = cell.char
		}
	}
	testing.expect(t, contains(string(all[:]), "cannot take on more deliveries"))
	press(&app, KEY_ENTER) // the only entry is Cancel
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, deliveries_carried(u), 1)
}

@(test)
a_negative_standing_asks_for_a_deposit_on_screen :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	dock := away_dock(u)
	planet_get(u, actor_get(u, dock).planet).reputation = -5
	offer := actor_get(u, dock).offer
	deposit := deposit_for(u, dock, offer)
	jools := u.avatar.jools
	bump_dock(t, &app, dock)
	pick(t, &app, .Delivery_Mission)
	all: [TEXT_ROWS * TEXT_COLUMNS]u8
	for row, r in app.text {
		for cell, c in row {
			all[r * TEXT_COLUMNS + c] = cell.char
		}
	}
	testing.expect(t, contains(string(all[:]), "deposit")) // the sentence wraps, so look for one word
	press(&app, KEY_ENTER) // Accept
	testing.expect_value(t, u.avatar.jools, jools - deposit)
}

@(test)
completing_a_delivery_shows_a_receipt_and_pays :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	a, b, dock := far_apart(u)
	carry(u, a, b, 40)
	jools := u.avatar.jools
	rep := planet_get(u, b).reputation
	bump_dock(t, &app, dock)
	pick(t, &app, .Complete_Delivery)
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.jools, jools + 40)
	testing.expect_value(t, planet_get(u, b).reputation, rep + 1)
	testing.expect_value(t, deliveries_carried(u), 0)
	testing.expect_value(t, app.text[6][(TEXT_COLUMNS - len("Delivery Complete!")) / 2].char, u8('D'))
	press(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
a_delivery_in_the_hold_has_its_own_page_and_can_be_abandoned :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	a, b, _ := far_apart(u)
	id := carry(u, a, b, 40)
	rep_a, rep_b := planet_get(u, a).reputation, planet_get(u, b).reputation
	open_inventory(&app)
	press(&app, KEY_DOWN, KEY_ENTER) // the delivery
	page, ok := stack_top(&app.stack)^.(Item_Page)
	testing.expect(t, ok && page.kind == .Delivery && page.item == id)
	app_draw(&app)
	title := item_page_title(u, &page)
	testing.expect_value(t, name_str(&title), mission_nouns[item_get(u, id).mission.noun])
	testing.expect_value(t, app.text[2][2].char, u8(' ')) // no "You have" for a one-off
	press(&app, KEY_ENTER) // Abandon Mission
	testing.expect(t, on_screen(&app, Confirm_Abandon_Delivery))
	press(&app, KEY_ENTER) // Cancel is the default: nothing happens
	testing.expect(t, on_screen(&app, Item_Page))
	testing.expect_value(t, deliveries_carried(u), 1)
	press(&app, KEY_ENTER, KEY_DOWN, KEY_ENTER) // Abandon Mission, Confirm
	testing.expect(t, on_screen(&app, Inventory_Screen)) // this screen and the page were popped
	testing.expect_value(t, deliveries_carried(u), 0)
	testing.expect_value(t, planet_get(u, a).reputation, rep_a - 5)
	testing.expect_value(t, planet_get(u, b).reputation, rep_b - 5)
	app_draw(&app) // the emptied hold still draws
}

@(test)
two_deliveries_are_two_inventory_rows :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	a, b, _ := far_apart(u)
	carry(u, a, b, 10)
	carry(u, a, b, 20)
	open_inventory(&app)
	press(&app, KEY_DOWN, KEY_DOWN, KEY_ENTER) // the second delivery, not a stack of two
	page, ok := stack_top(&app.stack)^.(Item_Page)
	testing.expect(t, ok && page.kind == .Delivery)
	testing.expect_value(t, item_get(u, page.item).mission.reward, 20)
}

@(test)
status_shows_reserves_jools_and_standing :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.oxygen.current = 100
	press(&app, KEY_ENTER, KEY_ENTER) // actions, Status (the first entry)
	testing.expect(t, on_screen(&app, Status_Screen))
	all: [TEXT_ROWS * TEXT_COLUMNS]u8
	for row, r in app.text {
		for cell, c in row {
			all[r * TEXT_COLUMNS + c] = cell.char
		}
	}
	text := string(all[:])
	testing.expect(t, contains(text, "O2: (100/250) 40%"))
	testing.expect(t, contains(text, "Fuel: (250/250) 100%"))
	testing.expect(t, contains(text, "Faction: SIGMO Federation"))
	testing.expect(t, contains(text, "Reputation: 100")) // on its own line so it fits
	testing.expect(t, contains(text, "Home Planet: "))
	press(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Action_Menu))
}

// ---- the pedia ----

// The text on screen as one string (rows run together), for finding words.
screen_text :: proc(app: ^App) -> string {
	@(static) all: [TEXT_ROWS * TEXT_COLUMNS]u8
	for row, r in app.text {
		for cell, c in row {
			all[r * TEXT_COLUMNS + c] = cell.char
		}
	}
	return string(all[:])
}

@(test)
the_pedia_opens_from_the_action_menu_with_four_lists :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	open_pedia(&app)
	testing.expect(t, on_screen(&app, Pedia_Menu))
	for kind, i in Pedia_Kind {
		app_key(&app, KEY_ENTER) // the list for the cursor's kind
		list, ok := stack_top(&app.stack)^.(Pedia_List)
		testing.expect(t, ok && list.kind == kind && list.scope == .All)
		app_key(&app, KEY_ESCAPE)
		app_key(&app, KEY_DOWN)
		_ = i
	}
	app_key(&app, KEY_ENTER) // Back
	testing.expect(t, on_screen(&app, Action_Menu))
}

@(test)
typing_narrows_a_list_and_backspace_widens_it :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	open_pedia(&app)
	press(&app, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Planets
	all := pedia_entries(u, .Planet, .All, 0, "")
	defer delete(all)
	name := pedia_name(u, .Planet, all[len(all) / 2])
	for c in transmute([]u8)name[:3] {
		app_key(&app, Key(c))
	}
	list, _ := stack_top(&app.stack)^.(Pedia_List)
	testing.expect_value(t, pedia_list_filter(&list), name[:3])
	narrowed := pedia_entries(u, .Planet, .All, 0, name[:3])
	defer delete(narrowed)
	testing.expect(t, len(narrowed) >= 1 && len(narrowed) < len(all))
	app_draw(&app)
	testing.expect(t, contains(screen_text(&app), name))
	app_key(&app, KEY_BACKSPACE)
	list, _ = stack_top(&app.stack)^.(Pedia_List)
	testing.expect_value(t, list.filter_len, 2)
	for _ in 0 ..< 5 {
		app_key(&app, KEY_BACKSPACE) // more than there is: harmless
	}
	list, _ = stack_top(&app.stack)^.(Pedia_List)
	testing.expect_value(t, list.filter_len, 0)
}

@(test)
a_filter_with_no_match_says_so :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	open_pedia(&app)
	press(&app, KEY_ENTER, Key('#'), Key('#')) // Factions, a filter nothing contains
	app_draw(&app)
	testing.expect(t, contains(screen_text(&app), "No matches."))
	press(&app, KEY_ENTER) // nothing to open
	testing.expect(t, on_screen(&app, Pedia_List))
}

@(test)
left_and_right_jump_between_first_letters :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	open_pedia(&app)
	press(&app, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Planets
	press(&app, KEY_RIGHT)
	list, _ := stack_top(&app.stack)^.(Pedia_List)
	all := pedia_entries(u, .Planet, .All, 0, "")
	defer delete(all)
	testing.expect(t, list.cursor > 0)
	testing.expect(t, pedia_name(u, .Planet, all[list.cursor])[0] != pedia_name(u, .Planet, all[0])[0])
	press(&app, KEY_LEFT)
	list, _ = stack_top(&app.stack)^.(Pedia_List)
	testing.expect_value(t, list.cursor, 0) // back to the top of the first letter
}

@(test)
a_planet_page_shows_its_facts_and_links_go_where_they_say :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	open_pedia(&app)
	press(&app, KEY_DOWN, KEY_DOWN, KEY_ENTER, KEY_ENTER) // Planets, the first one
	page, ok := stack_top(&app.stack)^.(Pedia_Page)
	testing.expect(t, ok && page.kind == .Planet)
	planet := &u.planets[page.id - 1]
	text := screen_text(&app)
	testing.expect(t, contains(text, name_str(&planet.name)))
	testing.expect(t, contains(text, "Planet Type: "))
	testing.expect(t, contains(text, "Tech Level: "))
	testing.expect(t, contains(text, name_str(&u.star_systems[int(planet.star_system) - 1].name)))
	testing.expect(t, contains(text, name_str(&u.factions[int(planet.faction) - 1].name)))

	// menu: Satellites, Faction, Star System, Back
	press(&app, KEY_DOWN, KEY_ENTER) // Faction
	faction_page, is_faction := stack_top(&app.stack)^.(Pedia_Page)
	testing.expect(t, is_faction && faction_page.kind == .Faction && faction_page.id == int(planet.faction))
	press(&app, KEY_ESCAPE)
	press(&app, KEY_DOWN, KEY_ENTER) // Star System (the cursor is still on Faction after coming back)
	system_page, is_system := stack_top(&app.stack)^.(Pedia_Page)
	testing.expect(t, is_system && system_page.kind == .Star_System && system_page.id == int(planet.star_system))
}

@(test)
a_system_page_leads_to_its_planets_satellites_and_factions :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	open_pedia(&app)
	press(&app, KEY_DOWN, KEY_ENTER, KEY_ENTER) // Star Systems, the first one
	page, _ := stack_top(&app.stack)^.(Pedia_Page)
	testing.expect_value(t, page.kind, Pedia_Kind.Star_System)
	text := screen_text(&app)
	testing.expect(t, contains(text, "Position: ("))
	testing.expect(t, contains(text, "Factions Present:"))
	for want, i in ([]Pedia_Kind{.Satellite, .Faction, .Planet}) {
		press(&app, KEY_ENTER) // the link under the cursor
		list, ok := stack_top(&app.stack)^.(Pedia_List)
		testing.expect(t, ok && list.kind == want && list.scope == .Star_System && list.scope_id == page.id)
		press(&app, KEY_ESCAPE, KEY_DOWN)
		_ = i
	}
}

@(test)
a_faction_page_lists_relations_and_values :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	// open SIGMO's page through the list
	open_pedia(&app)
	press(&app, KEY_ENTER) // Factions
	for c in transmute([]u8)string("SIGMO") {
		app_key(&app, Key(c))
	}
	app_key(&app, KEY_ENTER)
	page, ok := stack_top(&app.stack)^.(Pedia_Page)
	testing.expect(t, ok && page.kind == .Faction && Faction_Id(page.id) == SIGMO_FACTION)
	text := screen_text(&app)
	testing.expect(t, contains(text, "SIGMO Federation"))
	testing.expect(t, contains(text, "Authority: Acceptable(100)"))
	testing.expect(t, contains(text, "Reputation: 100")) // you start liked by your own faction
	// every other faction is listed, and each is Hostile to SIGMO; read the relations further down
	d: Doc
	pedia_page_doc(u, .Faction, page.id, &d)
	hostile := 0
	for i in 0 ..< d.count {
		if contains(d.lines[i].text, ": Hostile") {
			hostile += 1
		}
	}
	testing.expect_value(t, hostile, len(u.factions) - 1)
}

@(test)
long_pages_scroll_with_left_and_right :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	// a generated faction has values with long descriptions
	long: int
	for f, i in u.factions {
		if card(f.values) >= 2 {
			long = i + 1
			break
		}
	}
	testing.expect(t, long != 0)
	d: Doc
	pedia_page_doc(u, .Faction, long, &d)
	testing.expect(t, d.count > PEDIA_TEXT_ROWS)
	press(&app, KEY_ENTER, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_ENTER) // actions, pedia
	app_key(&app, KEY_ENTER) // Factions
	name := pedia_name(u, .Faction, long)
	for c in transmute([]u8)name {
		app_key(&app, Key(c))
	}
	app_key(&app, KEY_ENTER)
	page, _ := stack_top(&app.stack)^.(Pedia_Page)
	testing.expect_value(t, page.id, long)
	press(&app, KEY_RIGHT)
	page, _ = stack_top(&app.stack)^.(Pedia_Page)
	testing.expect_value(t, page.scroll, PEDIA_TEXT_ROWS)
	app_draw(&app)
	for _ in 0 ..< 10 {
		app_key(&app, KEY_RIGHT)
	}
	app_draw(&app)
	page, _ = stack_top(&app.stack)^.(Pedia_Page)
	testing.expect_value(t, page.scroll, d.count - PEDIA_TEXT_ROWS) // stops at the end
	press(&app, KEY_LEFT, KEY_LEFT, KEY_LEFT, KEY_LEFT, KEY_LEFT, KEY_LEFT, KEY_LEFT)
	app_draw(&app)
	page, _ = stack_top(&app.stack)^.(Pedia_Page)
	testing.expect_value(t, page.scroll, 0)
}

@(test)
a_satellite_page_names_its_planet_system_and_faction :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	s := &u.satellites[0]
	d: Doc
	title, _ := pedia_page_doc(&u, .Satellite, 1, &d)
	testing.expect_value(t, name_str(&title), name_str(&s.name))
	text := ""
	all: [4096]u8
	n := 0
	for i in 0 ..< d.count {
		for c in transmute([]u8)d.lines[i].text {
			all[n] = c
			n += 1
		}
		all[n] = ' '
		n += 1
	}
	text = string(all[:n])
	planet := &u.planets[int(s.planet) - 1]
	testing.expect(t, contains(text, name_str(&planet.name)))
	testing.expect(t, contains(text, name_str(&u.star_systems[int(s.star_system) - 1].name)))
	testing.expect(t, contains(text, name_str(&u.factions[int(planet.faction) - 1].name)))
}
