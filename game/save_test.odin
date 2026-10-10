#+build !js
package game

import "core:log"
import "core:testing"

// A universe that has had some life in it: stock bought, scrap salvaged, a delivery taken, gear installed.
lived_in :: proc(seed: u64) -> Universe {
	u := generate(seed)
	trade_buy(&u, .Oxygen_Tank, 0, 3)
	trade_buy(&u, .Fuel_Rod, 0, 2)
	for _ in 0 ..< 5 {
		append(&u.avatar.inventory, item_add(&u, item_new(.Scrap)))
	}
	if dock := home_dock(&u); dock != 0 {
		mission_accept(&u, dock)
	}
	equip_item(&u, .Accessory_0, item_add(&u, item_new(.Atmospheric_Concentrator)), charge = false)
	avatar_move(&u, .East)
	avatar_move(&u, .South)
	u.avatar.oxygen.current = 123
	u.avatar.jools = 4321
	return u
}

@(test)
a_saved_universe_loads_back_the_same :: proc(t: ^testing.T) {
	u := lived_in(1)
	defer universe_destroy(&u)
	data := universe_to_bytes(&u)
	defer delete(data)
	loaded, err := universe_from_bytes(data)
	defer universe_destroy(&loaded)
	testing.expect_value(t, err, Load_Error.None)

	// every kind of thing came through
	testing.expect_value(t, len(loaded.factions), len(u.factions))
	testing.expect_value(t, len(loaded.star_systems), len(u.star_systems))
	testing.expect_value(t, len(loaded.planets), len(u.planets))
	testing.expect_value(t, len(loaded.satellites), len(u.satellites))
	testing.expect_value(t, len(loaded.maps), len(u.maps))
	testing.expect_value(t, len(loaded.actors), len(u.actors))
	testing.expect_value(t, len(loaded.items), len(u.items))
	testing.expect_value(t, loaded.turn, u.turn)
	testing.expect_value(t, loaded.rng, u.rng)
	testing.expect_value(t, loaded.galaxy, u.galaxy)
	testing.expect_value(t, loaded.avatar.jools, 4321)
	testing.expect_value(t, loaded.avatar.oxygen.current, 123)
	testing.expect_value(t, loaded.avatar.equipment, u.avatar.equipment)
	testing.expect_value(t, len(loaded.avatar.inventory), len(u.avatar.inventory))
	testing.expect_value(t, name_str(&loaded.star_systems[0].name), name_str(&u.star_systems[0].name))
	testing.expect_value(t, loaded.planets[5].values, u.planets[5].values)
	testing.expect_value(t, loaded.planets[5].reputation, u.planets[5].reputation)
	testing.expect_value(t, loaded.factions[1].authority, u.factions[1].authority)
	testing.expect_value(t, deliveries_carried(&loaded), 1)
	home := home_dock(&loaded)
	testing.expect(t, home != 0 && loaded.actors[int(home) - 1].offer != 0)
	for m, i in u.maps {
		testing.expect_value(t, len(loaded.maps[i].actors), len(m.actors))
	}
	for a, i in u.actors {
		testing.expect_value(t, loaded.actors[i], a)
	}
	for it, i in u.items {
		testing.expect_value(t, loaded.items[i], it)
	}
}

@(test)
saving_again_after_loading_gives_the_same_bytes :: proc(t: ^testing.T) {
	u := lived_in(2)
	defer universe_destroy(&u)
	first := universe_to_bytes(&u)
	defer delete(first)
	loaded, err := universe_from_bytes(first)
	defer universe_destroy(&loaded)
	testing.expect_value(t, err, Load_Error.None)
	second := universe_to_bytes(&loaded)
	defer delete(second)
	testing.expect_value(t, len(second), len(first))
	same := true
	for b, i in first {
		same &&= second[i] == b
	}
	testing.expect(t, same) // nothing was lost or changed on the way
}

@(test)
the_pedia_comes_back_and_transient_state_does_not :: proc(t: ^testing.T) {
	u := lived_in(3)
	defer universe_destroy(&u)
	u.avatar.bumped = Actor_Id(1)
	u.avatar.auto_used = {used = true, added = 50}
	data := universe_to_bytes(&u)
	defer delete(data)
	loaded, _ := universe_from_bytes(data)
	defer universe_destroy(&loaded)
	testing.expect(t, loaded.avatar.bumped == nil)
	testing.expect(t, !loaded.avatar.auto_used.used)
	testing.expect_value(t, len(loaded.pedia.planets), len(loaded.planets)) // rebuilt, not stored
	for i in 1 ..< len(loaded.pedia.planets) {
		testing.expect(t, pedia_name(&loaded, .Planet, loaded.pedia.planets[i - 1]) <= pedia_name(&loaded, .Planet, loaded.pedia.planets[i]))
	}
}

@(test)
a_loaded_game_plays_on_just_like_the_original :: proc(t: ^testing.T) {
	u := lived_in(4)
	defer universe_destroy(&u)
	data := universe_to_bytes(&u)
	defer delete(data)
	loaded, _ := universe_from_bytes(data)
	defer universe_destroy(&loaded)
	// the same moves from the same state give the same results, random numbers included
	for dir in ([]Direction{.North, .West, .West, .South, .East}) {
		a := avatar_move(&u, dir)
		b := avatar_move(&loaded, dir)
		testing.expect_value(t, a, b)
	}
	testing.expect_value(t, actor_get(&u, u.avatar.actor).pos, actor_get(&loaded, loaded.avatar.actor).pos)
	testing.expect_value(t, u.avatar.fuel, loaded.avatar.fuel)
	testing.expect_value(t, rng_u64(&u.rng), rng_u64(&loaded.rng))
}

@(test)
a_save_is_small_enough_for_the_browser :: proc(t: ^testing.T) {
	for density in Galactic_Density {
		settings := DEFAULT_EMBARK_SETTINGS
		settings.density = density
		u := generate(1, settings)
		data := universe_to_bytes(&u)
		log.infof("%v galaxy: %d stars, %d planets, %d satellites, %d actors -> %d bytes", density, len(u.star_systems), len(u.planets), len(u.satellites), len(u.actors), len(data))
		// browsers allow about 5 million characters of local storage for everything; saves are stored as
		// base64 (4/3 the size) and there can be six of them, so keep one well under 600 KB
		testing.expect(t, len(data) * 4 / 3 * 6 < 4_000_000)
		delete(data)
		universe_destroy(&u)
	}
}

@(test)
anything_that_is_not_a_save_is_refused :: proc(t: ^testing.T) {
	_, err := universe_from_bytes(nil)
	testing.expect_value(t, err, Load_Error.Not_A_Save)
	_, err = universe_from_bytes([]u8{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15})
	testing.expect_value(t, err, Load_Error.Not_A_Save)
	_, err = universe_from_bytes([]u8{'F', 'H', 'O'})
	testing.expect_value(t, err, Load_Error.Not_A_Save)
}

@(test)
a_save_from_a_different_build_is_recognized :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	data := universe_to_bytes(&u)
	defer delete(data)
	data[4] += 1 // a different format number
	_, err := universe_from_bytes(data)
	testing.expect_value(t, err, Load_Error.Other_Version)
	data[4] -= 1
	data[7] ~= 0xFF // a different layout hash
	_, err = universe_from_bytes(data)
	testing.expect_value(t, err, Load_Error.Other_Version)
}

@(test)
a_save_that_was_cut_short_is_refused_without_leaking :: proc(t: ^testing.T) {
	u := generate(5)
	defer universe_destroy(&u)
	data := universe_to_bytes(&u)
	defer delete(data)
	// the test runner reports any memory a failed load leaves behind
	for cut in ([]int{HEADER_SIZE, HEADER_SIZE + 1, HEADER_SIZE + 10, len(data) / 4, len(data) / 2, len(data) - 100, len(data) - 1}) {
		_, err := universe_from_bytes(data[:cut])
		testing.expectf(t, err == .Corrupt, "cut at %d of %d gave %v", cut, len(data), err)
	}
	extra := make([]u8, len(data) + 1)
	defer delete(extra)
	copy(extra, data)
	_, err := universe_from_bytes(extra)
	testing.expect_value(t, err, Load_Error.Corrupt) // trailing junk is not a clean save
}

@(test)
damaged_saves_never_crash_the_loader :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	data := universe_to_bytes(&u)
	defer delete(data)
	r := rng_make(12345)
	copy_of := make([]u8, len(data))
	defer delete(copy_of)
	loaded_clean, refused := 0, 0
	for _ in 0 ..< 300 {
		copy(copy_of, data)
		// flip a handful of bytes past the header
		for _ in 0 ..< 1 + rng_below(&r, 6) {
			copy_of[HEADER_SIZE + rng_below(&r, len(data) - HEADER_SIZE)] = u8(rng_below(&r, 256))
		}
		loaded, err := universe_from_bytes(copy_of)
		if err == .None {
			loaded_clean += 1
			testing.expect(t, universe_valid(&loaded))
			universe_destroy(&loaded)
		} else {
			refused += 1
		}
	}
	testing.expect(t, refused > 0) // damage is noticed, at least some of the time
	log.infof("300 damaged saves: %d refused, %d still valid", refused, loaded_clean)
}

@(test)
a_fresh_universe_is_valid_and_a_broken_one_is_not :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	testing.expect(t, universe_valid(&u))
	u.actors[0].map_id = Map_Id(len(u.maps) + 5)
	testing.expect(t, !universe_valid(&u))
	u.actors[0].map_id = 0
	u.avatar.home_planet = Planet_Id(len(u.planets) + 1)
	testing.expect(t, !universe_valid(&u))
}
