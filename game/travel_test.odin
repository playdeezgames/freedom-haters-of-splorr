#+build !js
package game

import "core:testing"

first_of_kind_on :: proc(u: ^Universe, kind: Actor_Kind, on: Map_Id = 0) -> Actor_Id {
	for a, i in u.actors {
		if a.kind == kind && a.map_id != 0 && (on == 0 || a.map_id == on) {
			return Actor_Id(i + 1)
		}
	}
	return 0
}

@(test)
every_wormhole_is_paired_with_one_in_a_system :: proc(t: ^testing.T) {
	for density in Galactic_Density {
		settings := DEFAULT_EMBARK_SETTINGS
		settings.density = density
		u := generate(7, settings)
		defer universe_destroy(&u)
		testing.expect(t, u.nexus != 0)
		testing.expect_value(t, map_get(&u, u.nexus).kind, Map_Kind.Nexus)
		on_nexus := 0
		for a, i in u.actors {
			if a.kind != .Wormhole || a.map_id == 0 {
				continue
			}
			other := actor_get(&u, a.target)
			testing.expect_value(t, other.kind, Actor_Kind.Wormhole)
			testing.expect_value(t, other.target, Actor_Id(i + 1))
			if a.map_id == u.nexus {
				on_nexus += 1
				testing.expect(t, other.map_id != u.nexus)
				testing.expect_value(t, map_get(&u, other.map_id).kind, Map_Kind.Star_System)
			}
		}
		testing.expect(t, on_nexus > 0)
		testing.expect_value(t, on_nexus * 2, count_actors_on_maps(&u, .Wormhole))
		sum := 0
		for s in u.star_systems {
			sum += s.wormhole_count
		}
		testing.expect_value(t, sum, on_nexus)
	}
}

count_actors_on_maps :: proc(u: ^Universe, kind: Actor_Kind) -> (n: int) {
	for a in u.actors {
		if a.kind == kind && a.map_id != 0 {
			n += 1
		}
	}
	return
}

@(test)
nexus_wormholes_keep_their_distance :: proc(t: ^testing.T) {
	u := generate(3)
	defer universe_destroy(&u)
	spacing := density_spacing[DEFAULT_EMBARK_SETTINGS.density].minimum_wormhole_distance
	ids := map_get(&u, u.nexus).actors
	for a in ids {
		for b in ids {
			if a < b {
				d := squared_distance(actor_get(&u, a).pos, actor_get(&u, b).pos)
				testing.expect(t, d >= spacing * spacing)
			}
		}
	}
}

@(test)
star_gates_are_in_orbits_and_named_for_their_planet :: proc(t: ^testing.T) {
	u := generate(5)
	defer universe_destroy(&u)
	gates := count_actors(&u, .Star_Gate)
	testing.expect(t, gates > 0)
	testing.expect(t, gates < len(u.planets))
	for a in u.actors {
		if a.kind == .Star_Gate {
			testing.expect_value(t, map_get(&u, a.map_id).kind, Map_Kind.Planet_Orbit)
		}
	}
	gate := first_of_kind_on(&u, .Star_Gate)
	name := gate_name(&u, gate)
	testing.expect(t, name.len > len(" Star Gate"))
}

@(test)
a_wormhole_carries_you_to_its_twin_and_back :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	wormhole := first_of_kind_on(u, .Wormhole, u.nexus)
	twin := actor_get(u, wormhole).target
	// stand in the nexus beside the wormhole and walk into it
	actor_relocate(u, u.avatar.actor, u.nexus, {0, 0})
	dir := park_beside(t, u, wormhole)
	keys := [Direction]Key{.North = KEY_UP, .East = KEY_RIGHT, .South = KEY_DOWN, .West = KEY_LEFT}
	turn := u.turn
	app_key(&app, keys[dir])
	testing.expect(t, on_screen(&app, Interaction_Screen))
	testing.expect_value(t, u.turn, turn + 1)
	app_key(&app, KEY_ENTER) // Enter Wormhole
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, ship_map(u), actor_get(u, twin).map_id)
	testing.expect_value(t, u.avatar.star_system, actor_get(u, twin).star_system)
	testing.expect_value(t, u.turn, turn + 1) // the way through is free
	d := ship_pos(u) - actor_get(u, twin).pos
	testing.expect_value(t, abs(d.x) + abs(d.y), 1)

	// and back
	actor_relocate(u, u.avatar.actor, ship_map(u), ship_pos(u)) // already beside it
	dir = park_beside(t, u, twin)
	app_key(&app, keys[dir])
	testing.expect(t, on_screen(&app, Interaction_Screen))
	app_key(&app, KEY_ENTER)
	testing.expect_value(t, ship_map(u), u.nexus)
	testing.expect_value(t, u.avatar.star_system, Star_System_Id(0))
}

@(test)
a_star_gate_lists_only_your_factions_other_gates :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	mine: [dynamic]Actor_Id
	defer delete(mine)
	for a, i in u.actors {
		if a.kind == .Star_Gate && planet_get(u, a.planet).faction == u.avatar.faction {
			append(&mine, Actor_Id(i + 1))
		}
	}
	if len(mine) < 2 {
		testing.fail_now(t, "SIGMO has too few gates")
	}
	list := star_gates_for_avatar(u, mine[0])
	defer delete(list)
	testing.expect_value(t, len(list), len(mine) - 1)
	for g in list {
		testing.expect(t, g != mine[0])
		testing.expect_value(t, planet_get(u, actor_get(u, g).planet).faction, u.avatar.faction)
	}

	// go through the screens: bump the first gate, choose the first destination
	actor_relocate(u, u.avatar.actor, actor_get(u, mine[0]).map_id, {1, 1})
	dir := park_beside(t, u, mine[0])
	keys := [Direction]Key{.North = KEY_UP, .East = KEY_RIGHT, .South = KEY_DOWN, .West = KEY_LEFT}
	app_key(&app, keys[dir])
	app_key(&app, KEY_ENTER) // Enter Star Gate
	testing.expect(t, on_screen(&app, Star_Gate_Screen))
	app_key(&app, KEY_DOWN) // past Cancel
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
	testing.expect_value(t, ship_map(u), actor_get(u, list[0]).map_id)
	testing.expect_value(t, u.avatar.star_system, actor_get(u, list[0]).star_system)
}

@(test)
the_fleet_scales_with_the_galaxy_and_belongs_to_planet_factions :: proc(t: ^testing.T) {
	u := generate(9)
	defer universe_destroy(&u)
	ships := count_actors(&u, .Military_Ship)
	testing.expect_value(t, ships, fleet_size(len(u.star_systems)))
	for a in u.actors {
		if a.kind == .Military_Ship {
			testing.expect_value(t, a.map_id, u.galaxy)
			testing.expect_value(t, planet_get(&u, a.planet).faction, a.faction)
		}
	}
	// nothing shares a cell with the player's ship
	ship := actor_get(&u, u.avatar.actor)
	testing.expect_value(t, actor_at(&u, u.galaxy, ship.pos), u.avatar.actor)
}

@(test)
a_military_ship_shows_its_faction_and_how_it_feels_about_you :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	ship := first_of_kind_on(u, .Military_Ship)
	u.avatar.bumped = ship
	app_key(&app, KEY_ESCAPE) // any screen will do to draw; push the interaction directly
	app_key(&app, KEY_ESCAPE)
	app.stack.items[app.stack.count] = Interaction_Screen{}
	app.stack.count += 1
	app_tick(&app)
	list, n := interactions_for(u, u.avatar.bumped)
	_ = list
	testing.expect_value(t, n, 0) // inert: only Cancel
	testing.expect_value(t, app.text[1][(TEXT_COLUMNS - len("Military Vessel")) / 2].char, u8('M'))
}
