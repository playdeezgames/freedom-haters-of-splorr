#+build !js
package game

import "core:log"
import "core:testing"

generate :: proc(seed: u64, settings: Embark_Settings = DEFAULT_EMBARK_SETTINGS) -> Universe {
	g := generator_start(seed, settings)
	for generator_step(&g) {
		assert(g.steps_done < 1_000_000, "generation does not finish")
	}
	return generator_finish(&g)
}

count_actors :: proc(u: ^Universe, kind: Actor_Kind) -> (n: int) {
	for a in u.actors {
		if a.kind == kind {
			n += 1
		}
	}
	return
}

squared_distance :: proc(a, b: [2]int) -> int {
	return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y)
}

@(test)
name_candidates_are_pronounceable_and_capitalized :: proc(t: ^testing.T) {
	r := rng_make(1)
	for _ in 0 ..< 500 {
		n := name_candidate(&r)
		s := name_str(&n)
		testing.expect(t, len(s) >= 3 && len(s) <= 24)
		testing.expect(t, s[0] >= 'A' && s[0] <= 'Z')
	}
}

@(test)
unique_names_never_repeat :: proc(t: ^testing.T) {
	r := rng_make(2)
	ng: Name_Generator
	defer name_generator_destroy(&ng)
	seen: map[Name]struct{}
	defer delete(seen)
	for _ in 0 ..< 3000 {
		n := name_unique(&ng, &r)
		testing.expect(t, n not_in seen)
		seen[n] = {}
	}
}

@(test)
generation_is_deterministic_per_seed :: proc(t: ^testing.T) {
	a, b, c := generate(77), generate(77), generate(78)
	defer universe_destroy(&a)
	defer universe_destroy(&b)
	defer universe_destroy(&c)
	testing.expect_value(t, len(a.star_systems), len(b.star_systems))
	testing.expect_value(t, len(a.planets), len(b.planets))
	testing.expect_value(t, len(a.satellites), len(b.satellites))
	testing.expect_value(t, len(a.actors), len(b.actors))
	testing.expect_value(t, a.avatar.jools, b.avatar.jools)
	testing.expect(t, a.avatar.home_planet == b.avatar.home_planet)
	for _, i in a.star_systems {
		testing.expect(t, a.star_systems[i].name == b.star_systems[i].name)
		testing.expect(t, a.star_systems[i].position == b.star_systems[i].position)
	}
	testing.expect(t, a.star_systems[0].name != c.star_systems[0].name || a.star_systems[0].position != c.star_systems[0].position)
}

@(test)
factions_are_sigmo_plus_the_chosen_number :: proc(t: ^testing.T) {
	for count in MIN_FACTION_COUNT ..= MAX_FACTION_COUNT {
		settings := DEFAULT_EMBARK_SETTINGS
		settings.faction_count = count
		u := generate(5, settings)
		defer universe_destroy(&u)
		testing.expect_value(t, len(u.factions), count + 1)
		sigmo := faction_get(&u, SIGMO_FACTION)
		testing.expect_value(t, name_str(&sigmo.name), "SIGMO Federation")
		testing.expect_value(t, sigmo.authority, 100)
		testing.expect_value(t, sigmo.standards, 100)
		testing.expect_value(t, sigmo.conviction, 100)
		testing.expect(t, sigmo.planet_count >= 1)
		for &f, i in u.factions[1:] {
			testing.expect(t, f.authority >= 0 && f.authority <= 100)
			testing.expect(t, f.standards >= 0 && f.standards <= 100)
			testing.expect(t, f.conviction >= 0 && f.conviction <= 100)
			testing.expect(t, card(f.values) >= 1)
			for &other, j in u.factions[1:] {
				if i != j {
					testing.expect(t, f.name != other.name)
				}
			}
		}
	}
}

@(test)
every_planet_has_one_faction_and_counts_add_up :: proc(t: ^testing.T) {
	for seed in 1 ..= 5 {
		u := generate(u64(seed))
		defer universe_destroy(&u)
		total := 0
		for f in u.factions {
			total += f.planet_count
		}
		testing.expect_value(t, total, len(u.planets))
		for p in u.planets {
			testing.expect(t, p.faction != 0 && int(p.faction) <= len(u.factions))
		}
		for f, i in u.factions {
			claimed := 0
			for p in u.planets {
				if p.faction == Faction_Id(i + 1) {
					claimed += 1
				}
			}
			testing.expect_value(t, claimed, f.planet_count)
		}
	}
}

@(test)
stars_keep_their_distance_and_stay_on_the_map :: proc(t: ^testing.T) {
	for density in Galactic_Density {
		settings := DEFAULT_EMBARK_SETTINGS
		settings.density = density
		u := generate(9, settings)
		defer universe_destroy(&u)
		spacing := density_spacing[density].minimum_distance
		testing.expect(t, len(u.star_systems) > 1)
		for a, i in u.star_systems {
			testing.expect(t, map_in_bounds(.Galaxy, a.position))
			for b, j in u.star_systems {
				if i != j {
					testing.expect(t, squared_distance(a.position, b.position) >= spacing * spacing)
				}
			}
		}
	}
}

@(test)
denser_galaxies_hold_more_stars :: proc(t: ^testing.T) {
	dense, sparse := 0, 0
	for seed in 1 ..= 3 {
		s := DEFAULT_EMBARK_SETTINGS
		s.density = .Dense
		a := generate(u64(seed), s)
		s.density = .Sparse
		b := generate(u64(seed), s)
		dense += len(a.star_systems)
		sparse += len(b.star_systems)
		universe_destroy(&a)
		universe_destroy(&b)
	}
	testing.expect(t, dense > sparse)
}

@(test)
age_changes_the_mix_of_stars :: proc(t: ^testing.T) {
	young, old: [Star_Type]int
	for seed in 1 ..= 3 {
		s := DEFAULT_EMBARK_SETTINGS
		s.age = .Young
		a := generate(u64(seed), s)
		s.age = .Old
		b := generate(u64(seed), s)
		for sys in a.star_systems {
			young[sys.star_type] += 1
		}
		for sys in b.star_systems {
			old[sys.star_type] += 1
		}
		universe_destroy(&a)
		universe_destroy(&b)
	}
	testing.expect(t, young[.Blue] > young[.Red])
	testing.expect(t, old[.Red] > old[.Blue])
}

@(test)
star_systems_contain_a_star_and_spaced_planets :: proc(t: ^testing.T) {
	for seed in 1 ..= 4 {
		u := generate(u64(seed))
		defer universe_destroy(&u)
		for sys in u.star_systems {
			testing.expect(t, sys.interior != 0)
			system_map := map_get(&u, sys.interior)
			testing.expect_value(t, system_map.kind, Map_Kind.Star_System)
			testing.expect(t, system_map.owner == sys.actor)
			testing.expect(t, sys.planet_count >= 0 && sys.planet_count <= 12)

			center := map_center(.Star_System)
			vicinity_marker := actor_at(&u, sys.interior, center)
			testing.expect(t, vicinity_marker != 0)
			vm := actor_get(&u, vicinity_marker)
			testing.expect_value(t, vm.kind, Actor_Kind.Star_Vicinity)
			star_map := map_get(&u, vm.interior)
			testing.expect_value(t, star_map.kind, Map_Kind.Star_Vicinity)
			star := actor_get(&u, actor_at(&u, vm.interior, map_center(.Star_Vicinity)))
			testing.expect_value(t, star.kind, Actor_Kind.Star)

			spacing := star_info[sys.star_type].minimum_planet_distance
			planets: [dynamic]Actor
			defer delete(planets)
			for id in system_map.actors {
				if a := actor_get(&u, id)^; a.kind == .Planet_Vicinity {
					append(&planets, a)
					testing.expect(t, squared_distance(a.pos, center) >= spacing * spacing)
					testing.expect(t, !map_is_edge(.Star_System, a.pos))
				}
			}
			testing.expect_value(t, len(planets), sys.planet_count)
			for a, i in planets {
				for b, j in planets {
					if i != j {
						testing.expect(t, squared_distance(a.pos, b.pos) >= spacing * spacing)
					}
				}
			}
		}
	}
}

@(test)
planets_have_a_vicinity_orbit_and_spaced_satellites :: proc(t: ^testing.T) {
	for seed in 1 ..= 4 {
		u := generate(u64(seed))
		defer universe_destroy(&u)
		testing.expect(t, len(u.planets) > 0)
		satellites_seen := 0
		for p, i in u.planets {
			id := Planet_Id(i + 1)
			testing.expect(t, p.tech_level >= 0 && p.tech_level <= 10)
			testing.expect(t, card(p.values) >= 1)
			testing.expect(t, p.satellite_count >= 0 && p.satellite_count <= 4)

			marker := actor_get(&u, p.actor)
			testing.expect_value(t, marker.kind, Actor_Kind.Planet_Vicinity)
			testing.expect(t, marker.planet == id)
			vicinity := map_get(&u, marker.interior)
			testing.expect_value(t, vicinity.kind, Map_Kind.Planet_Vicinity)

			body := actor_get(&u, actor_at(&u, marker.interior, map_center(.Planet_Vicinity)))
			testing.expect_value(t, body.kind, Actor_Kind.Planet)
			testing.expect_value(t, body.size, 3)
			orbit := map_get(&u, body.interior)
			testing.expect_value(t, orbit.kind, Map_Kind.Planet_Orbit)
			orbit_body := actor_get(&u, actor_at(&u, body.interior, map_center(.Planet_Orbit)))
			testing.expect_value(t, orbit_body.kind, Actor_Kind.Planet_Body)
			testing.expect_value(t, orbit_body.size, 5)

			sats: [dynamic]Actor
			defer delete(sats)
			for aid in vicinity.actors {
				if a := actor_get(&u, aid)^; a.kind == .Satellite {
					append(&sats, a)
					testing.expect(t, !map_is_edge(.Planet_Vicinity, a.pos))
					for dy in -1 ..= 1 {
						for dx in -1 ..= 1 {
							testing.expect(t, squared_distance(a.pos, body.pos + {dx, dy}) >= MINIMUM_SATELLITE_DISTANCE * MINIMUM_SATELLITE_DISTANCE)
						}
					}
					sat := satellite_get(&u, a.satellite)
					testing.expect(t, sat.planet == id)
					testing.expect_value(t, sat.tech_level, p.tech_level)
					sat_body := actor_get(&u, actor_at(&u, a.interior, map_center(.Satellite_Orbit)))
					testing.expect_value(t, sat_body.kind, Actor_Kind.Satellite_Body)
					testing.expect_value(t, sat_body.size, 3)
				}
			}
			testing.expect_value(t, len(sats), p.satellite_count)
			for a, i in sats {
				for b, j in sats {
					if i != j {
						testing.expect(t, squared_distance(a.pos, b.pos) >= MINIMUM_SATELLITE_DISTANCE * MINIMUM_SATELLITE_DISTANCE)
					}
				}
			}
			satellites_seen += len(sats)
		}
		testing.expect_value(t, satellites_seen, len(u.satellites))
		sum := 0
		for s in u.star_systems {
			sum += s.satellite_count
		}
		testing.expect_value(t, sum, len(u.satellites))
	}
}

@(test)
all_names_are_unique :: proc(t: ^testing.T) {
	u := generate(3)
	defer universe_destroy(&u)
	seen: map[Name]struct{}
	defer delete(seen)
	for s in u.star_systems {
		testing.expect(t, s.name not_in seen)
		seen[s.name] = {}
	}
	for p in u.planets {
		testing.expect(t, p.name not_in seen)
		seen[p.name] = {}
	}
	for s in u.satellites {
		testing.expect(t, s.name not_in seen)
		seen[s.name] = {}
	}
}

@(test)
the_player_starts_on_the_galaxy_with_full_tanks_and_a_sigmo_home :: proc(t: ^testing.T) {
	for wealth in Starting_Wealth {
		settings := DEFAULT_EMBARK_SETTINGS
		settings.wealth = wealth
		u := generate(12, settings)
		defer universe_destroy(&u)
		ship := actor_get(&u, u.avatar.actor)
		testing.expect_value(t, ship.kind, Actor_Kind.Player_Ship)
		testing.expect(t, ship.map_id == u.galaxy)
		testing.expect_value(t, count_actors(&u, .Player_Ship), 1)
		testing.expect(t, map_in_bounds(.Galaxy, ship.pos))

		profile := wealth_profiles[wealth]
		testing.expect(t, u.avatar.jools >= profile.first && u.avatar.jools <= wealth_max_jools(wealth))
		testing.expect_value(t, (u.avatar.jools - profile.first) % profile.step, 0)
		testing.expect_value(t, u.avatar.jools_minimum, profile.wallet_minimum)
		testing.expect_value(t, u.avatar.fuel.current, MARK_I_CAPACITY)
		testing.expect_value(t, u.avatar.oxygen.current, MARK_I_CAPACITY)
		testing.expect_value(t, u.avatar.faction, SIGMO_FACTION)
		testing.expect(t, planet_get(&u, u.avatar.home_planet).faction == SIGMO_FACTION)
		testing.expect(t, !avatar_is_game_over(&u))
	}
}

@(test)
progress_runs_down_to_done :: proc(t: ^testing.T) {
	g := generator_start(4, DEFAULT_EMBARK_SETTINGS)
	defer generator_destroy(&g)
	label, _ := generator_current(&g)
	testing.expect_value(t, label, "Factions")
	testing.expect_value(t, generator_steps_remaining(&g), 4)
	generator_step(&g)
	label, _ = generator_current(&g)
	testing.expect_value(t, label, "Galaxy")
	generator_step(&g)
	// the galaxy queued a step per star system, plus the player
	testing.expect(t, generator_steps_remaining(&g) > 3)
	subject: Name
	label, subject = generator_current(&g)
	testing.expect_value(t, label, "Star system")
	testing.expect(t, subject.len > 0)
	for generator_step(&g) {}
	testing.expect(t, generator_done(&g))
	label, _ = generator_current(&g)
	testing.expect_value(t, label, "Done!")
	testing.expect(t, !generator_step(&g))
}

@(test)
steps_run_in_the_original_order :: proc(t: ^testing.T) {
	g := generator_start(4, DEFAULT_EMBARK_SETTINGS)
	defer generator_destroy(&g)
	order: [dynamic]string
	defer delete(order)
	for !generator_done(&g) {
		label, _ := generator_current(&g)
		if len(order) == 0 || order[len(order) - 1] != label {
			append(&order, label)
		}
		generator_step(&g)
	}
	want := []string{"Factions", "Galaxy", "Star system", "Planet", "Dividing up the galaxy", "Errands", "Yer ship"}
	testing.expect_value(t, len(order), len(want))
	for w, i in want {
		testing.expect_value(t, order[i], w)
	}
}

@(test)
generation_pins_seed_one :: proc(t: ^testing.T) {
	// Regression pin, not an independent check: a change here means universes from existing seeds changed.
	u := generate(1)
	defer universe_destroy(&u)
	testing.expectf(t, len(u.star_systems) > 0, "no stars")
	log.infof("seed 1: %d stars, %d planets, %d satellites, %d maps, %d actors (%d bytes each), first star %q", len(u.star_systems), len(u.planets), len(u.satellites), len(u.maps), len(u.actors), size_of(Actor), name_str(&u.star_systems[0].name))
}

@(test)
every_planet_orbit_has_one_star_dock :: proc(t: ^testing.T) {
	for seed in 1 ..= 3 {
		u := generate(u64(seed))
		defer universe_destroy(&u)
		docks := count_actors(&u, .Star_Dock)
		testing.expect_value(t, docks, len(u.planets))
		for p, i in u.planets {
			planet_actor := actor_get(&u, p.actor)
			body := actor_at(&u, planet_actor.interior, map_center(.Planet_Vicinity))
			orbit := actor_get(&u, body).interior
			in_orbit := 0
			for id in map_get(&u, orbit).actors {
				a := actor_get(&u, id)
				if a.kind == .Star_Dock {
					in_orbit += 1
					testing.expect(t, a.planet == Planet_Id(i + 1))
					testing.expect(t, a.star_system == p.star_system)
					testing.expect(t, !map_is_edge(.Planet_Orbit, a.pos))
					testing.expect(t, !actor_covers(actor_get(&u, actor_at(&u, orbit, map_center(.Planet_Orbit)))^, a.pos))
				}
			}
			testing.expect_value(t, in_orbit, 1)
		}
	}
}

@(test)
every_system_has_debris_with_scrap_to_find :: proc(t: ^testing.T) {
	for seed in 1 ..= 3 {
		u := generate(u64(seed))
		defer universe_destroy(&u)
		for sys, i in u.star_systems {
			piles := 0
			for id in map_get(&u, sys.interior).actors {
				a := actor_get(&u, id)
				if a.kind == .Debris {
					piles += 1
					testing.expect(t, a.loot >= 4 && a.loot <= 24)
					testing.expect(t, !map_is_edge(.Star_System, a.pos))
					testing.expect(t, a.star_system == Star_System_Id(i + 1))
					testing.expect(t, a.pos != map_center(.Star_System))
				}
			}
			testing.expect(t, piles >= 2 && piles <= 12)
			testing.expect_value(t, sys.scrap, piles)
		}
	}
}

@(test)
debris_never_shares_a_cell_with_anything :: proc(t: ^testing.T) {
	u := generate(5)
	defer universe_destroy(&u)
	for sys in u.star_systems {
		seen: map[[2]int]struct{}
		defer delete(seen)
		for id in map_get(&u, sys.interior).actors {
			a := actor_get(&u, id)
			testing.expect(t, a.pos not_in seen)
			seen[a.pos] = {}
		}
	}
}
