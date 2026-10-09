#+build !js
package game

import "core:testing"

faction_with :: proc(a, s, c: int) -> Faction {
	return Faction{authority = a, standards = s, conviction = c}
}

@(test)
relations_use_the_straight_line_distance :: proc(t: ^testing.T) {
	origin := faction_with(0, 0, 0)
	testing.expect_value(t, faction_distance(origin, faction_with(3, 4, 0)), 5.0)
	testing.expect_value(t, faction_distance(origin, faction_with(0, 0, 0)), 0.0)
	d := faction_distance(origin, faction_with(100, 100, 100))
	testing.expect(t, d > 173.2 && d < 173.3) // the README's "about 173"
}

@(test)
the_live_games_misplaced_square_root_is_not_ported :: proc(t: ^testing.T) {
	// two factions 20 apart on each of standards and conviction: the VB's formula gave |0| + 400 + 400 = 800
	// (Hostile); the real distance is about 28 (Neutral)
	a, b := faction_with(50, 50, 50), faction_with(50, 70, 70)
	testing.expect_value(t, relation_between(a, b), Relation.Neutral)
}

@(test)
relation_thresholds_match_the_readme :: proc(t: ^testing.T) {
	zero := faction_with(0, 0, 0)
	testing.expect_value(t, relation_between(zero, faction_with(25, 0, 0)), Relation.Friendly)
	testing.expect_value(t, relation_between(zero, faction_with(26, 0, 0)), Relation.Neutral)
	testing.expect_value(t, relation_between(zero, faction_with(50, 0, 0)), Relation.Neutral)
	testing.expect_value(t, relation_between(zero, faction_with(51, 0, 0)), Relation.Hostile)
	testing.expect_value(t, relation_between(zero, zero), Relation.Friendly)
}

@(test)
relations_are_the_same_both_ways :: proc(t: ^testing.T) {
	a, b := faction_with(10, 90, 40), faction_with(60, 20, 75)
	testing.expect_value(t, relation_between(a, b), relation_between(b, a))
}

@(test)
every_generated_faction_is_hostile_to_sigmo :: proc(t: ^testing.T) {
	// The satire: SIGMO is at the far corner, so everyone else is hostile to the player's faction.
	// Only the factions step is needed, so this can check a lot of seeds.
	hostile, others := 0, 0
	for seed in 1 ..= 1500 {
		settings := DEFAULT_EMBARK_SETTINGS
		settings.faction_count = MAX_FACTION_COUNT
		g := generator_start(u64(seed), settings)
		generator_step(&g) // Step_Factions
		sigmo := g.universe.factions[0]
		for f in g.universe.factions[1:] {
			others += 1
			if relation_between(sigmo, f) == .Hostile {
				hostile += 1
			}
		}
		generator_destroy(&g)
	}
	testing.expect_value(t, hostile, others)
}

@(test)
the_other_factions_get_along_better_with_each_other :: proc(t: ^testing.T) {
	counts: [Relation]int
	for seed in 1 ..= 300 {
		settings := DEFAULT_EMBARK_SETTINGS
		settings.faction_count = MAX_FACTION_COUNT
		g := generator_start(u64(seed), settings)
		generator_step(&g)
		fs := g.universe.factions[1:]
		for a, i in fs {
			for b, j in fs {
				if i < j {
					counts[relation_between(a, b)] += 1
				}
			}
		}
		generator_destroy(&g)
	}
	testing.expect(t, counts[.Friendly] > 0 && counts[.Neutral] > 0) // both happen
	testing.expect(t, counts[.Hostile] * 10 < counts[.Friendly] + counts[.Neutral]) // hostility is rare among them
}

@(test)
trait_levels_use_the_live_games_words :: proc(t: ^testing.T) {
	testing.expect_value(t, trait_level_name(100), "Acceptable")
	testing.expect_value(t, trait_level_name(91), "Acceptable")
	testing.expect_value(t, trait_level_name(90), "Tolerable")
	testing.expect_value(t, trait_level_name(76), "Tolerable")
	testing.expect_value(t, trait_level_name(75), "Unacceptable")
	testing.expect_value(t, trait_level_name(51), "Unacceptable")
	testing.expect_value(t, trait_level_name(50), "Inexcusable")
	testing.expect_value(t, trait_level_name(0), "Inexcusable")
}

@(test)
the_index_lists_everything_in_name_order :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	testing.expect_value(t, len(u.pedia.factions), len(u.factions))
	testing.expect_value(t, len(u.pedia.star_systems), len(u.star_systems))
	testing.expect_value(t, len(u.pedia.planets), len(u.planets))
	testing.expect_value(t, len(u.pedia.satellites), len(u.satellites))
	for kind in Pedia_Kind {
		ids := pedia_entries(&u, kind, .All, 0, "")
		defer delete(ids)
		for i in 1 ..< len(ids) {
			testing.expect(t, pedia_name(&u, kind, ids[i - 1]) <= pedia_name(&u, kind, ids[i]))
		}
	}
}

@(test)
filters_match_anywhere_in_the_name_ignoring_case :: proc(t: ^testing.T) {
	testing.expect(t, name_matches("Zhekath", "")) // an empty filter matches all
	testing.expect(t, name_matches("Zhekath", "kat"))
	testing.expect(t, name_matches("Zhekath", "ZHE"))
	testing.expect(t, name_matches("Zhekath", "zhekath"))
	testing.expect(t, !name_matches("Zhekath", "xyz"))
	testing.expect(t, !name_matches("Zhekath", "zhekathh")) // longer than the name
	testing.expect(t, name_matches("People's Republic", "s r"))
	u := generate(1)
	defer universe_destroy(&u)
	all := pedia_entries(&u, .Planet, .All, 0, "")
	defer delete(all)
	some := pedia_entries(&u, .Planet, .All, 0, "a")
	defer delete(some)
	testing.expect(t, len(some) > 0 && len(some) < len(all))
	for id in some {
		testing.expect(t, name_matches(pedia_name(&u, .Planet, id), "a"))
	}
}

@(test)
scoped_lists_hold_only_their_own :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	// a faction's planets
	for f in 1 ..= len(u.factions) {
		ids := pedia_entries(&u, .Planet, .Faction, f, "")
		defer delete(ids)
		testing.expect_value(t, len(ids), u.factions[f - 1].planet_count)
		for id in ids {
			testing.expect_value(t, int(u.planets[id - 1].faction), f)
		}
	}
	// a system's planets, satellites and the factions present
	for s in 1 ..= len(u.star_systems) {
		planets := pedia_entries(&u, .Planet, .Star_System, s, "")
		defer delete(planets)
		testing.expect_value(t, len(planets), u.star_systems[s - 1].planet_count)
		sats := pedia_entries(&u, .Satellite, .Star_System, s, "")
		defer delete(sats)
		testing.expect_value(t, len(sats), u.star_systems[s - 1].satellite_count)
		factions := pedia_entries(&u, .Faction, .Star_System, s, "")
		defer delete(factions)
		seen: map[int]struct{}
		defer delete(seen)
		for id in planets {
			seen[int(u.planets[id - 1].faction)] = {}
		}
		testing.expect_value(t, len(factions), len(seen)) // each faction once, however many planets it has there
	}
	// a planet's satellites
	for p in 1 ..= len(u.planets) {
		ids := pedia_entries(&u, .Satellite, .Planet, p, "")
		defer delete(ids)
		testing.expect_value(t, len(ids), u.planets[p - 1].satellite_count)
	}
}

@(test)
a_filter_applies_inside_a_scope :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	system := 1
	planets := pedia_entries(&u, .Planet, .Star_System, system, "")
	defer delete(planets)
	testing.expect(t, len(planets) > 0)
	name := pedia_name(&u, .Planet, planets[0])
	one := pedia_entries(&u, .Planet, .Star_System, system, name)
	defer delete(one)
	testing.expect(t, len(one) >= 1)
	elsewhere := pedia_entries(&u, .Planet, .Star_System, system + 1, name)
	defer delete(elsewhere)
	testing.expect_value(t, len(elsewhere), 0)
}

@(test)
letter_jump_moves_between_first_letters :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	ids := pedia_entries(&u, .Planet, .All, 0, "")
	defer delete(ids)
	first_letter :: proc(u: ^Universe, id: int) -> u8 {
		return pedia_name(u, .Planet, id)[0]
	}
	// forward from the top lands on the first name with a different first letter
	next := pedia_jump(&u, .Planet, ids[:], 0, true)
	testing.expect(t, next > 0)
	testing.expect(t, first_letter(&u, ids[next]) != first_letter(&u, ids[0]))
	testing.expect_value(t, first_letter(&u, ids[next - 1]), first_letter(&u, ids[0]))
	// backward from the middle of a letter goes to the top of that letter, then to the previous letter's top
	mid := next + 1 if next + 1 < len(ids) && first_letter(&u, ids[next + 1]) == first_letter(&u, ids[next]) else next
	top := pedia_jump(&u, .Planet, ids[:], mid, false)
	testing.expect(t, top <= mid)
	testing.expect_value(t, first_letter(&u, ids[top]), first_letter(&u, ids[mid]))
	if top > 0 {
		testing.expect(t, first_letter(&u, ids[top - 1]) != first_letter(&u, ids[top]))
	}
	back := pedia_jump(&u, .Planet, ids[:], top, false)
	testing.expect(t, first_letter(&u, ids[back]) != first_letter(&u, ids[top]) || top == 0)
	// it wraps in both directions
	last := len(ids) - 1
	wrapped := pedia_jump(&u, .Planet, ids[:], last, true)
	testing.expect_value(t, wrapped, 0)
	before_first := pedia_jump(&u, .Planet, ids[:], 0, false)
	testing.expect_value(t, first_letter(&u, ids[before_first]), first_letter(&u, ids[last]))
	if before_first > 0 {
		testing.expect(t, first_letter(&u, ids[before_first - 1]) != first_letter(&u, ids[before_first]))
	}
	// nothing to jump over
	testing.expect_value(t, pedia_jump(&u, .Planet, nil, 5, true), 0)
}

@(test)
every_value_has_a_description_the_font_can_draw :: proc(t: ^testing.T) {
	for v in Group_Value {
		d := group_value_descriptions[v]
		testing.expectf(t, len(d) > 20, "%v needs a description", v)
		for c in transmute([]u8)d {
			testing.expectf(t, c >= 32 && c < 127, "%v has the unprintable byte %d", v, c)
		}
	}
}
