#+build !js
package game

import "core:testing"

// A military ship of some faction other than the avatar's, or of the avatar's own.
ship_of :: proc(u: ^Universe, own_faction: bool) -> Actor_Id {
	for a, i in u.actors {
		if a.kind == .Military_Ship && (a.faction == u.avatar.faction) == own_faction {
			return Actor_Id(i + 1)
		}
	}
	return 0
}

// Puts a ship on the open galaxy cell just east of the avatar (or another side if that is taken).
bring_alongside :: proc(t: ^testing.T, u: ^Universe, ship: Actor_Id) {
	me := actor_get(u, u.avatar.actor)^
	for step in ([4][2]int{{1, 0}, {-1, 0}, {0, 1}, {0, -1}}) {
		if cell_is_free(u, u.galaxy, me.pos + step) {
			actor_relocate(u, ship, u.galaxy, me.pos + step)
			return
		}
	}
	testing.fail_now(t, "no room beside the ship")
}

@(test)
standing_and_relations_decide_who_is_hostile :: proc(t: ^testing.T) {
	u := generate(2)
	defer universe_destroy(&u)
	stranger := actor_get(&u, ship_of(&u, false))
	testing.expect_value(t, ship_disposition(&u, stranger^), Disposition.Hostile) // every faction hates SIGMO
	faction := faction_get(&u, stranger.faction)
	faction.reputation = 25
	testing.expect_value(t, ship_disposition(&u, stranger^), Disposition.Neutral)
	faction.reputation = 75
	testing.expect_value(t, ship_disposition(&u, stranger^), Disposition.Friendly)
	faction.reputation = -25
	testing.expect_value(t, ship_disposition(&u, stranger^), Disposition.Hostile) // can't get worse
	own := actor_get(&u, ship_of(&u, true))
	testing.expect_value(t, ship_disposition(&u, own^), Disposition.Friendly)
}

@(test)
ships_wander_without_stacking_or_leaving_the_galaxy :: proc(t: ^testing.T) {
	u := generate(4)
	defer universe_destroy(&u)
	ships := count_actors(&u, .Military_Ship)
	for _ in 0 ..< 200 {
		patrol_step(&u)
	}
	seen := make(map[[2]int]bool)
	defer delete(seen)
	for id in map_get(&u, u.galaxy).actors {
		a := actor_get(&u, id)^
		testing.expect(t, map_in_bounds(.Galaxy, a.pos))
		testing.expect(t, !seen[a.pos], "two actors share a cell")
		seen[a.pos] = true
	}
	testing.expect_value(t, count_actors(&u, .Military_Ship), ships)
}

@(test)
a_hostile_ship_in_sight_closes_in :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	ship := ship_of(&u, false)
	me := actor_get(&u, u.avatar.actor)
	// three cells away on a free row
	spot := me.pos + {3, 0}
	if !cell_is_free(&u, u.galaxy, spot) {
		spot = me.pos + {-3, 0}
	}
	testing.expect(t, cell_is_free(&u, u.galaxy, spot))
	actor_relocate(&u, ship, u.galaxy, spot)
	start := squared(actor_get(&u, ship).pos, me.pos)
	patrol_step(&u)
	testing.expect(t, squared(actor_get(&u, ship).pos, me.pos) < start)
}

squared :: proc(a, b: [2]int) -> int {
	d := a - b
	return d.x * d.x + d.y * d.y
}

@(test)
a_calm_ship_ignores_you :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	ship := ship_of(&u, false)
	bring_alongside(t, &u, ship)
	_, contact := patrol_contact(&u)
	testing.expect_value(t, contact, Contact.Shakedown)
	ship_calm(&u, ship, 10)
	_, contact = patrol_contact(&u)
	testing.expect_value(t, contact, Contact.None)
	u.turn += 10
	_, contact = patrol_contact(&u)
	testing.expect_value(t, contact, Contact.Shakedown)
}

@(test)
friendly_ships_hail_instead :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	ship := ship_of(&u, true)
	bring_alongside(t, &u, ship)
	found, contact := patrol_contact(&u)
	testing.expect_value(t, contact, Contact.Hail)
	testing.expect_value(t, found, ship)
}

@(test)
the_fine_is_a_quarter_and_never_bankrupts :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	u.avatar.jools = 1000
	testing.expect_value(t, fine_amount(&u), 250)
	u.avatar.jools = 20
	testing.expect_value(t, fine_amount(&u), 10)
	u.avatar.jools = u.avatar.jools_minimum + 5
	testing.expect_value(t, fine_amount(&u), 0)
}

@(test)
a_shakedown_takes_half_the_cargo_but_not_deliveries :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	clear(&u.avatar.inventory)
	for _ in 0 ..< 5 {
		in_hold(&u, .Scrap)
	}
	a, b, _ := far_apart(&u)
	delivery := carry(&u, a, b, 50)
	ship := ship_of(&u, false)
	testing.expect_value(t, cargo_demanded(&u), 3)
	taken := avatar_hand_over_cargo(&u, ship)
	testing.expect_value(t, taken, 3)
	testing.expect_value(t, len(u.avatar.inventory), 3) // 2 scrap and the delivery
	found := false
	for id in u.avatar.inventory {
		found ||= id == delivery
	}
	testing.expect(t, found)
	testing.expect(t, actor_get(&u, ship).calm_until > u.turn)
}

@(test)
a_ship_that_catches_you_opens_the_shakedown_and_paying_ends_it :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	ship := ship_of(u, false)
	bring_alongside(t, u, ship)
	u.avatar.jools = 1000
	u.turn += 1 // a turn passes, so the ships get their say
	app_tick(&app)
	testing.expect(t, on_screen(&app, Contact_Screen))
	app_key(&app, KEY_ESCAPE) // no way out
	testing.expect(t, on_screen(&app, Contact_Screen))
	app_key(&app, KEY_ENTER) // Pay Fine, the first choice
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.jools, 750)
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
	u.turn += 1
	app_tick(&app)
	testing.expect(t, on_screen(&app, Navigation)) // it leaves you be for a while
}

@(test)
nothing_to_take_means_a_warning_only :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	ship := ship_of(u, false)
	bring_alongside(t, u, ship)
	u.avatar.jools = u.avatar.jools_minimum + 5
	clear(&u.avatar.inventory)
	u.turn += 1
	app_tick(&app)
	testing.expect(t, on_screen(&app, Message))
	testing.expect(t, actor_get(u, ship).calm_until > u.turn)
}
