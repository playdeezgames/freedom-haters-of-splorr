#+build !js
package game

import "core:testing"

@(test)
names_round_trip_and_truncate :: proc(t: ^testing.T) {
	n := name_make("Zhekath")
	testing.expect_value(t, name_str(&n), "Zhekath")
	long := name_make("0123456789012345678901234567890123456789")
	testing.expect_value(t, len(name_str(&long)), NAME_CAPACITY)
	empty: Name
	testing.expect_value(t, name_str(&empty), "")
}

@(test)
ids_start_at_one_so_zero_means_none :: proc(t: ^testing.T) {
	u := universe_make(1)
	defer universe_destroy(&u)
	a := faction_add(&u, {name = name_make("A")})
	b := faction_add(&u, {name = name_make("B")})
	testing.expect_value(t, a, Faction_Id(1))
	testing.expect_value(t, b, Faction_Id(2))
	testing.expect_value(t, name_str(&faction_get(&u, b).name), "B")
	faction_get(&u, a).planet_count = 7
	testing.expect_value(t, faction_get(&u, a).planet_count, 7)
}

@(test)
every_map_kind_has_an_odd_or_known_size :: proc(t: ^testing.T) {
	testing.expect_value(t, map_sizes[.Galaxy], [2]int{63, 63})
	testing.expect_value(t, map_sizes[.Star_System], [2]int{31, 31})
	testing.expect_value(t, map_sizes[.Star_Vicinity], [2]int{15, 15})
	testing.expect_value(t, map_sizes[.Planet_Vicinity], [2]int{15, 15})
	testing.expect_value(t, map_sizes[.Planet_Orbit], [2]int{11, 11})
	testing.expect_value(t, map_sizes[.Satellite_Orbit], [2]int{9, 9})
}

@(test)
edge_is_the_outer_ring :: proc(t: ^testing.T) {
	testing.expect(t, map_is_edge(.Satellite_Orbit, {0, 4}))
	testing.expect(t, map_is_edge(.Satellite_Orbit, {8, 8}))
	testing.expect(t, map_is_edge(.Satellite_Orbit, {4, 0}))
	testing.expect(t, !map_is_edge(.Satellite_Orbit, {1, 1}))
	testing.expect(t, !map_is_edge(.Satellite_Orbit, {4, 4}))
	testing.expect(t, !map_is_edge(.Satellite_Orbit, {9, 4})) // off the map is not an edge
	testing.expect(t, !map_is_edge(.Satellite_Orbit, {-1, 4}))
}

@(test)
footprint_covers_a_square_around_its_center :: proc(t: ^testing.T) {
	planet := Actor{pos = {5, 5}, size = 5}
	testing.expect(t, actor_covers(planet, {5, 5}))
	testing.expect(t, actor_covers(planet, {3, 3}))
	testing.expect(t, actor_covers(planet, {7, 7}))
	testing.expect(t, !actor_covers(planet, {8, 5}))
	testing.expect(t, !actor_covers(planet, {5, 2}))
	satellite := Actor{pos = {4, 4}, size = 3}
	testing.expect(t, actor_covers(satellite, {3, 5}))
	testing.expect(t, !actor_covers(satellite, {2, 4}))
	marker := Actor{pos = {1, 1}, size = 1}
	testing.expect(t, actor_covers(marker, {1, 1}))
	testing.expect(t, !actor_covers(marker, {1, 2}))
}

@(test)
actors_are_found_by_any_cell_of_their_footprint :: proc(t: ^testing.T) {
	u := universe_make(1)
	defer universe_destroy(&u)
	m := map_add(&u, .Planet_Orbit)
	body := actor_add(&u, m, {kind = .Planet_Body, pos = {5, 5}, size = 5})
	ship := actor_add(&u, m, {kind = .Player_Ship, pos = {0, 5}})
	testing.expect_value(t, actor_at(&u, m, {3, 7}), body)
	testing.expect_value(t, actor_at(&u, m, {0, 5}), ship)
	testing.expect_value(t, actor_at(&u, m, {1, 1}), Actor_Id(0))
	testing.expect_value(t, actor_get(&u, ship).size, 1) // default footprint
	testing.expect_value(t, actor_get(&u, ship).map_id, m)
	testing.expect_value(t, len(map_get(&u, m).actors), 2)
}

@(test)
free_cells_exclude_actors_and_the_outside :: proc(t: ^testing.T) {
	u := universe_make(1)
	defer universe_destroy(&u)
	m := map_add(&u, .Satellite_Orbit)
	actor_add(&u, m, {kind = .Satellite_Body, pos = {4, 4}, size = 3})
	testing.expect(t, !cell_is_free(&u, m, {4, 4}))
	testing.expect(t, !cell_is_free(&u, m, {5, 5}))
	testing.expect(t, cell_is_free(&u, m, {6, 6}))
	testing.expect(t, !cell_is_free(&u, m, {9, 0}))
}

@(test)
avatar_dies_without_oxygen_and_goes_bankrupt_at_the_floor :: proc(t: ^testing.T) {
	u := universe_make(1)
	defer universe_destroy(&u)
	u.avatar = {
		jools         = 100,
		jools_minimum = -999,
		oxygen        = {current = 5, minimum = 0, maximum = MARK_I_CAPACITY},
		fuel          = {current = 5, minimum = 0, maximum = MARK_I_CAPACITY},
	}
	testing.expect(t, !avatar_is_game_over(&u))
	u.avatar.oxygen.current = 0
	testing.expect(t, avatar_is_dead(&u))
	testing.expect(t, avatar_is_game_over(&u))
	u.avatar.oxygen.current = 1
	u.avatar.jools = -999
	testing.expect(t, avatar_is_bankrupt(&u))
	testing.expect(t, avatar_is_game_over(&u))
	u.avatar.jools = -998
	testing.expect(t, !avatar_is_game_over(&u))
}

@(test)
type_tables_match_the_original :: proc(t: ^testing.T) {
	refill := 0
	for info in planet_info {
		testing.expect(t, len(info.name) > 0)
		if info.can_refill_oxygen {
			refill += 1
		}
	}
	testing.expect_value(t, len(Planet_Type), 15)
	testing.expect_value(t, refill, 9)
	testing.expect(t, !planet_info[.Inferno].can_refill_oxygen)
	testing.expect(t, planet_info[.Gaia].can_refill_oxygen)
	testing.expect_value(t, len(Satellite_Type), 6)
	testing.expect_value(t, len(Group_Value), 10)
	testing.expect_value(t, star_info[.Blue].minimum_planet_distance, 6)
	testing.expect_value(t, star_info[.Red].minimum_planet_distance, 14)
}

@(test)
rolled_dice_have_the_original_ranges :: proc(t: ^testing.T) {
	check :: proc(t: ^testing.T, text: string, lo, hi: int) {
		dice, ok := dice_parse(text)
		testing.expect(t, ok)
		l, h := dice_bounds(dice)
		testing.expectf(t, l == lo && h == hi, "%q is %d..%d, want %d..%d", text, l, h, lo, hi)
	}
	check(t, PLANET_COUNT_DICE, 2, 12)
	check(t, SATELLITE_COUNT_DICE, 0, 4)
	check(t, TECH_LEVEL_DICE, 0, 10)
}

@(test)
satellite_count_dice_is_weighted_like_the_original :: proc(t: ^testing.T) {
	r := rng_make(4)
	counts: [5]int
	for _ in 0 ..< 9000 {
		counts[dice_roll(&r, SATELLITE_COUNT_DICE)] += 1
	}
	// weights 1,2,3,2,1 out of 9 -> about 1000, 2000, 3000, 2000, 1000
	want := [5]int{1000, 2000, 3000, 2000, 1000}
	for c, i in counts {
		testing.expectf(t, abs(c - want[i]) < 300, "%d satellites: %d, want about %d", i, c, want[i])
	}
}

@(test)
group_values_pick_one_to_three_distinct :: proc(t: ^testing.T) {
	r := rng_make(8)
	for _ in 0 ..< 500 {
		v := group_values_roll(&r)
		n := card(v)
		testing.expect(t, n >= 1 && n <= VALUE_ATTEMPTS)
	}
}

@(test)
rng_enum_covers_every_value :: proc(t: ^testing.T) {
	r := rng_make(6)
	seen: [Planet_Type]bool
	for _ in 0 ..< 2000 {
		seen[rng_enum(&r, Planet_Type)] = true
	}
	for s in seen {
		testing.expect(t, s)
	}
}
