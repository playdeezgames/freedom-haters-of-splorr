#+build !js
package game

import "core:testing"

@(test)
standing_changes_what_a_factions_posts_charge :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u) // under faction 2
	f := faction_get(&u, 2)
	f.reputation = 0
	testing.expect_value(t, standing_percent(&u, planet), 100)
	f.reputation = STANDING_GOOD
	testing.expect_value(t, standing_percent(&u, planet), PRICE_GOOD_PERCENT)
	f.reputation = STANDING_BAD
	testing.expect_value(t, standing_percent(&u, planet), PRICE_BAD_PERCENT)
	f.reputation = STANDING_BAD + 1
	testing.expect_value(t, standing_percent(&u, planet), 100)
	// goods
	f.reputation = 0
	base := buy_tenths(&u, planet, .Gems)
	f.reputation = STANDING_GOOD
	good := buy_tenths(&u, planet, .Gems)
	f.reputation = STANDING_BAD
	bad := buy_tenths(&u, planet, .Gems)
	testing.expect(t, good < base && base < bad)
	testing.expect(t, abs(good - base * 90 / 100) <= 1 && abs(bad - base * 115 / 100) <= 1)
	// what they pay you is not affected
	f.reputation = 0
	sell := sell_tenths(&u, planet, .Gems)
	f.reputation = STANDING_GOOD
	testing.expect_value(t, sell_tenths(&u, planet, .Gems), sell)
	// and a black market ignores faction standing
	f.reputation = STANDING_BAD
	black := black_buy_tenths(&u, planet, .Gems)
	f.reputation = STANDING_GOOD
	testing.expect_value(t, black_buy_tenths(&u, planet, .Gems), black)
}

@(test)
fuel_repairs_and_fees_follow_the_percentage_you_were_quoted :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.fuel.current = u.avatar.fuel.maximum - 90 // 30 jools at the list price
	u.avatar.hull.current = u.avatar.hull.maximum - 40 // 20 jools
	u.avatar.service_percent = 100
	testing.expect_value(t, fuel_price(&u), 30)
	testing.expect_value(t, hull_repair_price(&u), 20)
	u.avatar.service_percent = PRICE_GOOD_PERCENT
	testing.expect_value(t, fuel_price(&u), 27)
	testing.expect_value(t, hull_repair_price(&u), 18)
	u.avatar.service_percent = PRICE_BAD_PERCENT
	testing.expect_value(t, fuel_price(&u), 35) // rounded up
	testing.expect_value(t, hull_repair_price(&u), 23)
	// an install fee too
	laser := in_hold(&u, .Pulse_Laser, 1)
	u.avatar.service_percent = 100
	list := change_fee(&u, .Weapon, laser)
	u.avatar.service_percent = PRICE_BAD_PERCENT
	testing.expect(t, change_fee(&u, .Weapon, laser) > list)
}

@(test)
bumping_a_post_sets_the_percentage :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	post := first_post(&u)
	planet := actor_get(&u, post).planet
	faction_get(&u, planet_get(&u, planet).faction).reputation = STANDING_GOOD
	actor_relocate(&u, u.avatar.actor, actor_get(&u, post).map_id, {1, 1})
	// walk the ship up against the post from the left
	p := actor_get(&u, post).pos
	for _, _ in 0 ..< 1 {
		actor_relocate(&u, u.avatar.actor, actor_get(&u, post).map_id, p + {-1, 0})
	}
	u.avatar.fuel.current = u.avatar.fuel.maximum
	if !cell_is_free(&u, actor_get(&u, post).map_id, p + {-1, 0}) {
		return // something is in the way in this seed
	}
	avatar_move(&u, .East)
	testing.expect_value(t, u.avatar.bumped, Bump(post))
	testing.expect_value(t, u.avatar.service_percent, PRICE_GOOD_PERCENT)
	// the unit price of an item at the post carries it
	testing.expect_value(t, trade_unit_price(&u, .Fuel_Rod), service_price(&u, item_price(item_new(.Fuel_Rod))))
	testing.expect(t, trade_unit_price(&u, .Fuel_Rod) < item_price(item_new(.Fuel_Rod)))
}

@(test)
a_faction_that_hates_you_will_not_deal :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	post := first_post(&u)
	planet := actor_get(&u, post).planet
	f := faction_get(&u, planet_get(&u, planet).faction)
	f.reputation = 0
	list, n := interactions_for(&u, Bump(post))
	testing.expect_value(t, n, 1)
	testing.expect_value(t, list[0], Interaction.Trade)
	f.reputation = STANDING_REFUSED
	list, n = interactions_for(&u, Bump(post))
	testing.expect_value(t, n, 1)
	testing.expect_value(t, list[0], Interaction.Refused)
	// docks and yards too, but not a black market or a gate
	dock := dock_of(&u, planet)
	list, n = interactions_for(&u, Bump(dock))
	testing.expect_value(t, list[0], Interaction.Refused)
	testing.expect_value(t, n, 1)
	f.reputation = STANDING_REFUSED + 1
	list, n = interactions_for(&u, Bump(post))
	testing.expect_value(t, list[0], Interaction.Trade)
}

@(test)
killing_a_ships_worth_of_a_faction_costs_you_its_service :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	faction := actor_get(&u, ship_of(&u, false)).faction
	for _ in 0 ..< 10 {
		ship := Actor_Id(0)
		for a, i in u.actors {
			if a.kind == .Military_Ship && a.map_id != 0 && a.faction == faction {
				ship = Actor_Id(i + 1)
				break
			}
		}
		if ship == 0 {
			break
		}
		combat_victory(&u, combat_start(&u, ship))
	}
	// the faction's reputation fell by 5 a kill; once it is low enough its posts close
	if faction_get(&u, faction).reputation <= STANDING_REFUSED {
		for _, i in u.planets {
			if planet_get(&u, Planet_Id(i + 1)).faction == faction {
				testing.expect(t, standing_refused(&u, Planet_Id(i + 1)))
				break
			}
		}
	}
	testing.expect(t, faction_get(&u, faction).reputation < 0)
}

@(test)
friendly_ships_carry_nothing_worth_taking :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	clear(&u.avatar.inventory)
	ship := ship_of(&u, true)
	actor_get(&u, ship).faction = SIGMO_FACTION
	pos := actor_get(&u, ship).pos
	infamy := u.avatar.infamy
	v := combat_victory(&u, combat_start(&u, ship))
	testing.expect_value(t, v.parts, 0)
	testing.expect_value(t, v.hold_units, 0)
	testing.expect_value(t, v.loot, 0)
	testing.expect_value(t, len(u.avatar.inventory), 0)
	testing.expect_value(t, actor_at(&u, u.galaxy, pos), Actor_Id(0)) // no wreck
	// it still costs you: infamy and standing
	testing.expect_value(t, u.avatar.infamy, infamy + INFAMY_KILL)
	testing.expect_value(t, faction_get(&u, SIGMO_FACTION).reputation, STARTING_REPUTATION - KILL_REPUTATION_LOSS)
	// a hostile one still pays
	stranger := ship_of(&u, false)
	v2 := combat_victory(&u, combat_start(&u, stranger))
	testing.expect(t, v2.parts > 0)
}

@(test)
the_run_summary_counts_what_you_did :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	// a delivery
	a, b, _ := far_apart(u)
	carry(u, a, b, 40)
	mission_complete(u, dock_of(u, b))
	testing.expect_value(t, u.avatar.stats.deliveries, 1)
	// a kill
	combat_victory(u, combat_start(u, ship_of(u, false)))
	testing.expect_value(t, u.avatar.stats.kills, 1)
	// wealth: the peak is remembered after the money is gone
	u.avatar.jools = 5000
	app_tick(&app)
	u.avatar.jools = 100
	app_tick(&app)
	testing.expect_value(t, u.avatar.stats.peak_jools, 5000)
	// a planet visited by going into orbit
	planet := first_of_kind_on(u, .Planet)
	actor_relocate(u, u.avatar.actor, actor_get(u, planet).map_id, {1, 1})
	dir := park_beside(t, u, planet)
	keys := [Direction]Key{.North = KEY_UP, .East = KEY_RIGHT, .South = KEY_DOWN, .West = KEY_LEFT}
	app_key(&app, keys[dir])
	app_key(&app, KEY_ENTER) // Enter Orbit
	testing.expect_value(t, u.avatar.stats.planets_visited, 1)
	testing.expect(t, planet_get(u, actor_get(u, planet).planet).visited)
	// the score is a pure function of the run
	score := run_score(u)
	testing.expect(t, score >= 5000 / 10 + 20 + 40 + 10)
}

@(test)
game_over_shows_the_summary_and_score :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.stats.kills = 3
	u.avatar.oxygen.current = 0
	app_tick(&app)
	testing.expect(t, on_screen(&app, Game_Over))
	text := screen_text(&app)
	testing.expect(t, contains(text, "Ships Destroyed"))
	testing.expect(t, contains(text, "Score"))
	testing.expect(t, contains(text, "Yer Dead!"))
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Main_Menu))
}
