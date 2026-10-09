#+build !js
package game

import "core:testing"

opposite :: proc(d: Direction) -> Direction {
	return Direction((int(d) + 2) % 4)
}

// Puts the ship on a free neighbor of `target` and returns the direction that moves it into `target`.
park_beside :: proc(t: ^testing.T, u: ^Universe, target: Actor_Id) -> Direction {
	a := actor_get(u, target)^
	half := a.size / 2
	for dir in Direction {
		// stand on the side opposite `dir`, then walk `dir` into the footprint
		spot := a.pos - direction_delta[dir] * (half + 1)
		if cell_is_open(u, a.map_id, spot) {
			actor_relocate(u, u.avatar.actor, a.map_id, spot)
			return dir
		}
	}
	testing.fail_now(t, "no free cell beside the target")
}

ship_map :: proc(u: ^Universe) -> Map_Id {
	return actor_get(u, u.avatar.actor).map_id
}

ship_pos :: proc(u: ^Universe) -> [2]int {
	return actor_get(u, u.avatar.actor).pos
}

// Walks the ship into the star system marker `i` and approaches it.
fly_into_system :: proc(t: ^testing.T, u: ^Universe, i: int) -> Star_System_Id {
	id := Star_System_Id(i)
	dir := park_beside(t, u, star_system_get(u, id).actor)
	testing.expect_value(t, avatar_move(u, dir), Move_Outcome.Bumped)
	kind, ok := interaction_for(u, u.avatar.bumped)
	testing.expect(t, ok)
	testing.expect_value(t, kind, Interaction.Approach)
	testing.expect_value(t, avatar_interact(u, kind), Interaction_Result.Done)
	return id
}

@(test)
a_move_spends_a_turn_oxygen_and_fuel :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	start := ship_pos(&u)
	turn, oxygen, fuel := u.turn, u.avatar.oxygen.current, u.avatar.fuel.current
	moved := false
	for dir in Direction {
		if cell_is_open(&u, u.galaxy, start + direction_delta[dir]) {
			testing.expect_value(t, avatar_move(&u, dir), Move_Outcome.Moved)
			testing.expect_value(t, ship_pos(&u), start + direction_delta[dir])
			testing.expect_value(t, u.avatar.facing, dir)
			moved = true
			break
		}
	}
	testing.expect(t, moved)
	testing.expect_value(t, u.turn, turn + 1)
	testing.expect_value(t, u.avatar.oxygen.current, oxygen - 1)
	testing.expect_value(t, u.avatar.fuel.current, fuel - 1)
}

@(test)
no_fuel_means_no_move_and_no_cost :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.fuel.current = 0
	start, turn, oxygen := ship_pos(&u), u.turn, u.avatar.oxygen.current
	testing.expect_value(t, avatar_move(&u, .East), Move_Outcome.No_Fuel)
	testing.expect_value(t, ship_pos(&u), start)
	testing.expect_value(t, u.turn, turn)
	testing.expect_value(t, u.avatar.oxygen.current, oxygen)
	testing.expect_value(t, u.avatar.facing, Direction.East) // it still turns to face that way
}

@(test)
the_galaxy_edge_swallows_a_turn :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	actor_relocate(&u, u.avatar.actor, u.galaxy, {0, 5})
	// clear the way so it is the edge, not a star, that stops the ship
	turn, fuel := u.turn, u.avatar.fuel.current
	testing.expect_value(t, avatar_move(&u, .West), Move_Outcome.Out_Of_Map)
	testing.expect_value(t, ship_pos(&u), [2]int{0, 5})
	testing.expect_value(t, u.turn, turn + 1)
	testing.expect_value(t, u.avatar.fuel.current, fuel - 1)
	testing.expect(t, u.avatar.bumped == nil)
}

@(test)
bumping_a_star_system_offers_to_approach_it :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	sys := star_system_get(&u, 1)
	marker := sys.actor
	dir := park_beside(t, &u, marker)
	before := ship_pos(&u)
	turn := u.turn
	testing.expect_value(t, avatar_move(&u, dir), Move_Outcome.Bumped)
	testing.expect_value(t, ship_pos(&u), before) // the ship does not move
	testing.expect_value(t, u.turn, turn + 1) // but the attempt costs
	bumped, ok := u.avatar.bumped.(Actor_Id)
	testing.expect(t, ok)
	testing.expect_value(t, bumped, marker)
}

@(test)
approaching_a_system_arrives_just_inside_its_border :: proc(t: ^testing.T) {
	for seed in 1 ..= 5 {
		u := generate(u64(seed))
		defer universe_destroy(&u)
		turn, oxygen, fuel := u.turn, u.avatar.oxygen.current, u.avatar.fuel.current
		id := fly_into_system(t, &u, 1)
		sys := star_system_get(&u, id)
		testing.expect_value(t, ship_map(&u), sys.interior)
		p := ship_pos(&u)
		size := map_sizes[.Star_System]
		testing.expect_value(t, min(p.x, p.y, size.x - 1 - p.x, size.y - 1 - p.y), 1)
		testing.expect_value(t, sys.visit_count, 1)
		testing.expect_value(t, u.avatar.star_system, id)
		// the bump cost one turn and one fuel; going in cost one more turn and oxygen, no fuel
		testing.expect_value(t, u.turn, turn + 2)
		testing.expect_value(t, u.avatar.oxygen.current, oxygen - 2)
		testing.expect_value(t, u.avatar.fuel.current, fuel - 1)
		testing.expect(t, u.avatar.bumped == nil)
	}
}

@(test)
the_border_offers_to_leave_and_leaving_is_free :: proc(t: ^testing.T) {
	u := generate(2)
	defer universe_destroy(&u)
	id := fly_into_system(t, &u, 1)
	marker := star_system_get(&u, id).actor
	// walk to the nearest border cell and bump it
	size := map_sizes[.Star_System]
	p := ship_pos(&u)
	toward := Direction.North
	switch {
	case p.x == 1:
		toward = .West
	case p.y == 1:
		toward = .North
	case p.x == size.x - 2:
		toward = .East
	case:
		toward = .South
	}
	testing.expect_value(t, avatar_move(&u, toward), Move_Outcome.Bumped)
	edge, is_edge := u.avatar.bumped.(Map_Edge)
	testing.expect(t, is_edge)
	testing.expect(t, edge.map_id == star_system_get(&u, id).interior)
	kind, ok := interaction_for(&u, u.avatar.bumped)
	testing.expect(t, ok)
	testing.expect_value(t, kind, Interaction.Leave_Area)
	label: Name
	testing.expect_value(t, interaction_label(&u, kind, u.avatar.bumped, &label), "Leave Star System")

	turn, oxygen := u.turn, u.avatar.oxygen.current
	testing.expect_value(t, avatar_interact(&u, kind), Interaction_Result.Done)
	testing.expect_value(t, u.turn, turn) // leaving is free
	testing.expect_value(t, u.avatar.oxygen.current, oxygen)
	testing.expect_value(t, ship_map(&u), u.galaxy)
	m := actor_get(&u, marker)
	d := ship_pos(&u) - m.pos
	testing.expect_value(t, abs(d.x) + abs(d.y), 1) // right beside the star system
	testing.expect_value(t, u.avatar.star_system, Star_System_Id(0))
}

@(test)
star_systems_nest_down_to_a_satellite_and_back_out :: proc(t: ^testing.T) {
	u := generate(3)
	defer universe_destroy(&u)
	fly_into_system(t, &u, 1)

	// find a planet with a satellite in the first system that has one
	planet_id: Planet_Id
	for p, i in u.planets {
		if p.satellite_count > 0 {
			planet_id = Planet_Id(i + 1)
			break
		}
	}
	testing.expect(t, planet_id != 0)
	system := planet_get(&u, planet_id).star_system
	// jump straight to the right system by relocating the ship to its map
	actor_relocate(&u, u.avatar.actor, star_system_get(&u, system).interior, {1, 1})
	marker := planet_get(&u, planet_id).actor

	// approach the planet's vicinity
	dir := park_beside(t, &u, marker)
	testing.expect_value(t, avatar_move(&u, dir), Move_Outcome.Bumped)
	kind, _ := interaction_for(&u, u.avatar.bumped)
	testing.expect_value(t, kind, Interaction.Approach)
	avatar_interact(&u, kind)
	vicinity := actor_get(&u, marker).interior
	testing.expect_value(t, ship_map(&u), vicinity)

	// bump the planet itself (3x3): orbit
	body: Actor_Id
	for id in map_get(&u, vicinity).actors {
		if actor_get(&u, id).kind == .Planet {
			body = id
		}
	}
	dir = park_beside(t, &u, body)
	testing.expect_value(t, avatar_move(&u, dir), Move_Outcome.Bumped)
	kind, _ = interaction_for(&u, u.avatar.bumped)
	testing.expect_value(t, kind, Interaction.Enter_Orbit)
	avatar_interact(&u, kind)
	testing.expect_value(t, map_get(&u, ship_map(&u)).kind, Map_Kind.Planet_Orbit)

	// the planet's body in orbit is 5x5 and has nothing to offer
	orbit := ship_map(&u)
	orbit_body := actor_at(&u, orbit, map_center(.Planet_Orbit))
	_, offers := interaction_for(&u, orbit_body)
	testing.expect(t, !offers)

	// leave: back in the vicinity, beside (not on) the planet
	actor_relocate(&u, u.avatar.actor, orbit, {1, 1})
	dir = .North
	testing.expect_value(t, avatar_move(&u, dir), Move_Outcome.Bumped) // (1,0) is the border
	kind, _ = interaction_for(&u, u.avatar.bumped)
	label: Name
	testing.expect_value(t, interaction_label(&u, kind, u.avatar.bumped, &label), "Leave Orbit")
	testing.expect_value(t, avatar_interact(&u, kind), Interaction_Result.Done)
	testing.expect_value(t, ship_map(&u), vicinity)
	planet_actor := actor_get(&u, body)
	testing.expect(t, !actor_covers(planet_actor^, ship_pos(&u)))
	d := ship_pos(&u) - planet_actor.pos
	testing.expect(t, max(abs(d.x), abs(d.y)) == 2) // the ring just outside the 3x3
}

@(test)
satellites_and_stars_behave :: proc(t: ^testing.T) {
	u := generate(3)
	defer universe_destroy(&u)
	for a, i in u.actors {
		id := Actor_Id(i + 1)
		kind, offers := interaction_for(&u, id)
		#partial switch a.kind {
		case .Star_System, .Star_Vicinity, .Planet_Vicinity:
			testing.expect(t, offers && kind == .Approach)
		case .Planet, .Satellite:
			testing.expect(t, offers && kind == .Enter_Orbit)
		case .Star, .Planet_Body, .Satellite_Body, .Player_Ship, .Star_Dock:
			// nothing on offer with full tanks and no equipment
			testing.expect(t, !offers)
		}
	}
}

@(test)
arriving_never_lands_on_something_or_on_the_border :: proc(t: ^testing.T) {
	u := generate(4)
	defer universe_destroy(&u)
	for s in u.star_systems {
		for _ in 0 ..< 20 {
			actor_relocate(&u, u.avatar.actor, u.galaxy, {0, 0})
			u.avatar.bumped = s.actor
			testing.expect_value(t, avatar_interact(&u, .Approach), Interaction_Result.Done)
			p := ship_pos(&u)
			m := ship_map(&u)
			testing.expect(t, m == s.interior)
			size := map_sizes[map_get(&u, m).kind]
			testing.expect_value(t, min(p.x, p.y, size.x - 1 - p.x, size.y - 1 - p.y), 1)
			for other in map_get(&u, m).actors {
				if other != u.avatar.actor {
					testing.expect(t, !actor_covers(actor_get(&u, other)^, p))
				}
			}
		}
	}
}

@(test)
running_out_of_oxygen_is_game_over :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.oxygen.current = 2
	testing.expect(t, !avatar_is_game_over(&u))
	avatar_move(&u, .East)
	testing.expect(t, !avatar_is_game_over(&u))
	avatar_move(&u, .East)
	testing.expect(t, avatar_is_dead(&u))
	testing.expect(t, avatar_is_game_over(&u))
	avatar_do_turn(&u)
	testing.expect_value(t, u.avatar.oxygen.current, 0) // never goes negative
}

@(test)
distress_refuels_for_a_price_only_when_empty :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	testing.expect(t, !distress_available(&u))
	u.avatar.fuel.current = 0
	testing.expect(t, distress_available(&u))
	jools := u.avatar.jools
	added, price := avatar_signal_distress(&u)
	testing.expect_value(t, added, MARK_I_CAPACITY)
	testing.expect_value(t, price, MARK_I_CAPACITY * EMERGENCY_FUEL_PRICE)
	testing.expect_value(t, u.avatar.fuel.current, MARK_I_CAPACITY)
	testing.expect_value(t, u.avatar.jools, jools - price)
	testing.expect(t, !distress_available(&u))
}

@(test)
distress_can_bankrupt_you :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.fuel.current = 0
	u.avatar.jools = u.avatar.jools_minimum + 10
	avatar_signal_distress(&u)
	testing.expect(t, avatar_is_bankrupt(&u))
	testing.expect(t, avatar_is_game_over(&u))
}

@(test)
relocating_keeps_the_map_actor_lists_straight :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	system := star_system_get(&u, 1)
	galaxy_count := len(map_get(&u, u.galaxy).actors)
	system_count := len(map_get(&u, system.interior).actors)
	actor_relocate(&u, u.avatar.actor, system.interior, {1, 1})
	testing.expect_value(t, len(map_get(&u, u.galaxy).actors), galaxy_count - 1)
	testing.expect_value(t, len(map_get(&u, system.interior).actors), system_count + 1)
	actor_relocate(&u, u.avatar.actor, system.interior, {2, 2}) // same map: just moves
	testing.expect_value(t, len(map_get(&u, system.interior).actors), system_count + 1)
	testing.expect_value(t, actor_at(&u, system.interior, {2, 2}), u.avatar.actor)
}

// A star dock to test against, with the ship parked on the cell beside it.
first_dock :: proc(u: ^Universe) -> Actor_Id {
	for a, i in u.actors {
		if a.kind == .Star_Dock {
			return Actor_Id(i + 1)
		}
	}
	return 0
}

offered :: proc(u: ^Universe, target: Actor_Id) -> (list: [MAX_INTERACTIONS]Interaction, count: int) {
	return interactions_for(u, target)
}

@(test)
a_star_dock_offers_only_what_the_ship_needs :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	dock := first_dock(&u)
	_, none := offered(&u, dock)
	testing.expect_value(t, none, 0) // full tanks

	u.avatar.oxygen.current = 100
	list, n := offered(&u, dock)
	testing.expect_value(t, n, 1)
	testing.expect_value(t, list[0], Interaction.Refill_Oxygen)

	u.avatar.oxygen.current = u.avatar.oxygen.maximum
	u.avatar.fuel.current = 100
	list, n = offered(&u, dock)
	testing.expect_value(t, n, 1)
	testing.expect_value(t, list[0], Interaction.Refuel)

	u.avatar.oxygen.current = 100
	list, n = offered(&u, dock)
	testing.expect_value(t, n, 2)
	testing.expect_value(t, list[0], Interaction.Refill_Oxygen)
	testing.expect_value(t, list[1], Interaction.Refuel)
}

@(test)
prices_round_up_to_whole_jools :: proc(t: ^testing.T) {
	testing.expect_value(t, price_of(1, OXYGEN_PER_JOOL), 1)
	testing.expect_value(t, price_of(10, OXYGEN_PER_JOOL), 1)
	testing.expect_value(t, price_of(11, OXYGEN_PER_JOOL), 2)
	testing.expect_value(t, price_of(250, OXYGEN_PER_JOOL), 25)
	testing.expect_value(t, price_of(250, FUEL_PER_JOOL), 84)
	testing.expect_value(t, price_of(0, FUEL_PER_JOOL), 0)
}

@(test)
buying_oxygen_fills_the_tank_and_charges :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.oxygen.current = u.avatar.oxygen.maximum - 25
	jools := u.avatar.jools
	label: Name
	testing.expect_value(t, interaction_label(&u, .Refill_Oxygen, nil, &label), "Refill Oxygen (3 jools)")
	added, cost := avatar_buy_oxygen(&u)
	testing.expect_value(t, added, 25)
	testing.expect_value(t, cost, 3)
	testing.expect_value(t, u.avatar.oxygen.current, u.avatar.oxygen.maximum)
	testing.expect_value(t, u.avatar.jools, jools - 3)
}

@(test)
buying_fuel_fills_the_tank_and_charges :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.fuel.current = 0
	jools := u.avatar.jools
	label: Name
	testing.expect_value(t, interaction_label(&u, .Refuel, nil, &label), "Refuel (84 jools)")
	added, cost := avatar_buy_fuel(&u)
	testing.expect_value(t, added, 250)
	testing.expect_value(t, cost, 84)
	testing.expect_value(t, u.avatar.fuel.current, 250)
	testing.expect_value(t, u.avatar.jools, jools - 84)
}

@(test)
buying_what_you_cannot_afford_is_bankruptcy :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.fuel.current = 0
	u.avatar.jools = u.avatar.jools_minimum + 10
	avatar_buy_fuel(&u)
	testing.expect(t, avatar_is_bankrupt(&u))
}

// The body of a planet of the given type, as the ship would bump it in orbit.
orbit_body_of :: proc(u: ^Universe, type: Planet_Type) -> Actor_Id {
	for &p in u.planets {
		if p.type == type {
			planet_actor := actor_get(u, p.actor)
			vicinity_planet := actor_at(u, planet_actor.interior, map_center(.Planet_Vicinity))
			orbit := actor_get(u, vicinity_planet).interior
			return actor_at(u, orbit, map_center(.Planet_Orbit))
		}
	}
	return 0
}

@(test)
a_planet_only_gives_air_to_a_ship_with_a_concentrator :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.planets[0].type = .Terran // breathable
	body := actor_get(&u, orbit_body_of(&u, .Terran))
	testing.expect(t, body != nil)
	id := orbit_body_of(&u, .Terran)
	u.avatar.oxygen.current = 100

	_, n := offered(&u, id)
	testing.expect_value(t, n, 0) // no concentrator

	u.avatar.accessories += {.Atmospheric_Concentrator}
	list, n2 := offered(&u, id)
	testing.expect_value(t, n2, 1)
	testing.expect_value(t, list[0], Interaction.Gather_Atmosphere)

	u.avatar.oxygen.current = u.avatar.oxygen.maximum // nothing to fill
	_, n3 := offered(&u, id)
	testing.expect_value(t, n3, 0)
}

@(test)
only_breathable_planets_can_be_drawn_from :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.accessories += {.Atmospheric_Concentrator}
	u.avatar.oxygen.current = 100
	breathable, unbreathable := 0, 0
	for ptype in Planet_Type {
		u.planets[0].type = ptype
		id := orbit_body_of(&u, ptype)
		_, n := offered(&u, id)
		if planet_info[ptype].can_refill_oxygen {
			testing.expectf(t, n == 1, "%v should be breathable", ptype)
			breathable += 1
		} else {
			testing.expectf(t, n == 0, "%v should not be breathable", ptype)
			unbreathable += 1
		}
	}
	testing.expect_value(t, breathable, 9)
	testing.expect_value(t, unbreathable, 6)
}

@(test)
gathering_atmosphere_is_free :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.oxygen.current = 40
	jools := u.avatar.jools
	added := avatar_gather_atmosphere(&u)
	testing.expect_value(t, added, u.avatar.oxygen.maximum - 40)
	testing.expect_value(t, u.avatar.oxygen.current, u.avatar.oxygen.maximum)
	testing.expect_value(t, u.avatar.jools, jools)
}
