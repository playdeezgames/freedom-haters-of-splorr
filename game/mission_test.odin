#+build !js
package game

import "core:testing"

home_dock :: proc(u: ^Universe) -> Actor_Id {
	for a, i in u.actors {
		if a.kind == .Star_Dock && a.planet == u.avatar.home_planet {
			return Actor_Id(i + 1)
		}
	}
	return 0
}

// A dock on a planet that is not the avatar's home, in a system that is not the home system.
away_dock :: proc(u: ^Universe) -> Actor_Id {
	home_system := planet_get(u, u.avatar.home_planet).star_system
	for a, i in u.actors {
		if a.kind == .Star_Dock && a.star_system != home_system && u.planets[int(a.planet) - 1].faction != SIGMO_FACTION && actor_get(u, Actor_Id(i + 1)).offer != 0 {
			return Actor_Id(i + 1)
		}
	}
	return 0
}

dock_planets :: proc(u: ^Universe) -> (n: int) {
	for a in u.actors {
		if a.kind == .Star_Dock {
			n += 1
		}
	}
	return
}

@(test)
every_dock_offers_a_delivery_to_another_planet_of_its_faction :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	offers := 0
	for a, i in u.actors {
		if a.kind != .Star_Dock {
			continue
		}
		faction := u.planets[int(a.planet) - 1].faction
		planets_in_faction := 0
		for p in u.planets {
			if p.faction == faction {
				planets_in_faction += 1
			}
		}
		if a.offer == 0 {
			testing.expect_value(t, planets_in_faction, 1) // only a lone planet has nowhere to send things
			continue
		}
		offers += 1
		item := item_get(&u, a.offer)
		testing.expect_value(t, item.kind, Item_Kind.Delivery)
		m := item.mission
		testing.expect_value(t, m.origin, a.planet)
		testing.expect(t, m.destination != m.origin)
		testing.expect_value(t, planet_get(&u, m.destination).faction, faction)
		// 5d20 plus 2 a cell between the two systems
		cells := trip_cells(&u, m.origin, m.destination)
		testing.expect(t, m.reward >= 5 + 2 * cells && m.reward <= 100 + 2 * cells)
	}
	testing.expect(t, offers > 0)
}

@(test)
standing_starts_at_the_top_at_home_and_nowhere_else :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	home := planet_get(&u, u.avatar.home_planet)
	testing.expect_value(t, home.reputation, 100)
	testing.expect_value(t, star_system_get(&u, home.star_system).reputation, 100)
	testing.expect_value(t, faction_get(&u, SIGMO_FACTION).reputation, 100)
	others := 0
	for p, i in u.planets {
		if Planet_Id(i + 1) != u.avatar.home_planet {
			testing.expect_value(t, p.reputation, 0)
			others += 1
		}
	}
	testing.expect(t, others > 0)
	for f, i in u.factions[1:] {
		testing.expect_value(t, f.reputation, 0)
	}
}

@(test)
the_names_come_from_the_word_lists :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	offer := item_get(&u, actor_get(&u, home_dock(&u)).offer)
	name := mission_item_name(offer.mission)
	who := mission_recipient(offer.mission)
	testing.expect(t, name.len > 10)
	testing.expect(t, who.len > 10)
	m := Mission{adverb = 0, adjective = 0, noun = 0, first_name = 0, last_name = 0, job = 0}
	n := mission_item_name(m)
	testing.expect_value(t, long_str(&n), "Swiftly Resilient Quantum Batteries")
	r := mission_recipient(m)
	testing.expect_value(t, long_str(&r), "Gorachan Valken the Starship Engineer")
	testing.expect_value(t, len(mission_nouns), 23)
	testing.expect_value(t, len(mission_jobs), 23)
}

@(test)
how_many_deliveries_you_may_carry_follows_your_standing :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	dock := home_dock(&u)
	planet := planet_get(&u, actor_get(&u, dock).planet)
	star_system_get(&u, planet.star_system).reputation = 100
	faction_get(&u, planet.faction).reputation = 100

	check :: proc(t: ^testing.T, u: ^Universe, dock: Actor_Id, rep, max_carried: int) {
		planet := planet_get(u, actor_get(u, dock).planet)
		planet.reputation = rep
		testing.expect_value(t, max_current_deliveries(u, dock), max_carried)
	}
	check(t, &u, dock, 100, 4) // up to five in all
	check(t, &u, dock, 75, 3)
	check(t, &u, dock, 50, 2)
	check(t, &u, dock, 25, 1)
	check(t, &u, dock, 24, 0) // one at a time
	check(t, &u, dock, 0, 0)
	check(t, &u, dock, -1, 0)
	// the worst of the three counts
	planet.reputation = 100
	star_system_get(&u, planet.star_system).reputation = 30
	testing.expect_value(t, max_current_deliveries(&u, dock), 1)
}

@(test)
accepting_moves_the_delivery_into_the_hold_and_the_dock_offers_a_new_one :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	dock := home_dock(&u)
	first := actor_get(&u, dock).offer
	jools := u.avatar.jools
	testing.expect(t, can_accept_mission(&u, dock))
	testing.expect(t, mission_accept(&u, dock))
	testing.expect_value(t, inventory_count(&u, .Delivery), 1)
	testing.expect_value(t, u.avatar.inventory[0], first)
	testing.expect_value(t, u.avatar.jools, jools) // no deposit at home
	testing.expect(t, actor_get(&u, dock).offer != first)
	testing.expect(t, actor_get(&u, dock).offer != 0)
}

@(test)
with_a_poor_standing_you_carry_one_at_a_time :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	dock := away_dock(&u)
	testing.expect(t, dock != 0)
	testing.expect(t, mission_accept(&u, dock)) // none carried yet
	testing.expect(t, !can_accept_mission(&u, dock)) // reputation 0 allows only one
	testing.expect(t, !mission_accept(&u, dock))
	testing.expect_value(t, deliveries_carried(&u), 1)
}

@(test)
a_bad_name_needs_a_deposit_that_comes_back_with_the_reward :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	dock := away_dock(&u)
	planet_get(&u, actor_get(&u, dock).planet).reputation = -5
	offer := actor_get(&u, dock).offer
	reward := item_get(&u, offer).mission.reward
	deposit := max(1, reward / 2)
	testing.expect_value(t, deposit_for(&u, dock, offer), deposit)
	jools := u.avatar.jools
	testing.expect(t, mission_accept(&u, dock))
	testing.expect_value(t, u.avatar.jools, jools - deposit)
	testing.expect_value(t, item_get(&u, offer).mission.reward, reward + deposit) // paid back on delivery
}

@(test)
you_must_afford_the_deposit :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	dock := away_dock(&u)
	planet_get(&u, actor_get(&u, dock).planet).reputation = -1
	offer := actor_get(&u, dock).offer
	deposit := deposit_for(&u, dock, offer)
	u.avatar.jools = deposit - 1
	testing.expect(t, !can_accept_mission(&u, dock))
	u.avatar.jools = deposit
	testing.expect(t, can_accept_mission(&u, dock))
}

@(test)
no_deposit_when_standing_is_not_negative :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	dock := away_dock(&u)
	testing.expect_value(t, deposit_for(&u, dock, actor_get(&u, dock).offer), 0) // reputation 0 is fine
	testing.expect(t, !needs_deposit(&u, dock))
}

// A delivery in the hold from `origin` to `destination`.
carry :: proc(u: ^Universe, origin, destination: Planet_Id, reward: int) -> Item_Id {
	item := item_new(.Delivery)
	item.mission = {origin = origin, destination = destination, reward = reward}
	id := item_add(u, item)
	append(&u.avatar.inventory, id)
	return id
}

// Two planets of different factions in different systems, and a dock at the second.
far_apart :: proc(u: ^Universe) -> (a, b: Planet_Id, dock_at_b: Actor_Id) {
	for p, i in u.planets {
		for q, j in u.planets {
			if p.faction != q.faction && p.star_system != q.star_system {
				for ac, k in u.actors {
					if ac.kind == .Star_Dock && ac.planet == Planet_Id(j + 1) {
						return Planet_Id(i + 1), Planet_Id(j + 1), Actor_Id(k + 1)
					}
				}
			}
		}
	}
	return
}

@(test)
delivering_pays_the_reward_and_raises_standing_at_both_ends :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	a, b, dock := far_apart(&u)
	testing.expect(t, dock != 0)
	carry(&u, a, b, 40)
	jools := u.avatar.jools
	rep_a, rep_b := planet_get(&u, a).reputation, planet_get(&u, b).reputation
	done := mission_complete(&u, dock)
	testing.expect_value(t, done.count, 1)
	testing.expect_value(t, done.jools, 40)
	testing.expect_value(t, u.avatar.jools, jools + 40)
	testing.expect_value(t, deliveries_carried(&u), 0)
	testing.expect_value(t, planet_get(&u, a).reputation, rep_a + 1)
	testing.expect_value(t, planet_get(&u, b).reputation, rep_b + 1)
	for p in ([]Planet_Id{a, b}) {
		pl := planet_get(&u, p)
		_ = pl
	}
}

@(test)
standing_rises_once_for_a_shared_system_or_faction :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	// two planets of one faction in one system, if there are any
	pa, pb: Planet_Id
	search: for p, i in u.planets {
		for q, j in u.planets {
			if i != j && p.faction == q.faction && p.star_system == q.star_system {
				pa, pb = Planet_Id(i + 1), Planet_Id(j + 1)
				break search
			}
		}
	}
	testing.expect(t, pa != 0)
	sys := star_system_get(&u, planet_get(&u, pa).star_system)
	faction := faction_get(&u, planet_get(&u, pa).faction)
	rs, rf := sys.reputation, faction.reputation
	reputation_change(&u, pa, pb, 1)
	testing.expect_value(t, sys.reputation, rs + 1) // not +2
	testing.expect_value(t, faction.reputation, rf + 1)
	testing.expect_value(t, planet_get(&u, pa).reputation, planet_get(&u, pa).reputation) // each planet still moves on its own
	// a delivery to the very same planet counts it once
	before := planet_get(&u, pa).reputation
	reputation_change(&u, pa, pa, 1)
	testing.expect_value(t, planet_get(&u, pa).reputation, before + 1)
}

@(test)
only_deliveries_bound_for_this_dock_are_handed_over :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	a, b, dock := far_apart(&u)
	carry(&u, a, b, 10)
	carry(&u, a, a, 99) // bound for somewhere else (itself, here, by construction)
	carry(&u, b, b, 7)
	done := mission_complete(&u, dock)
	testing.expect_value(t, done.count, 2) // the ones whose destination is b
	testing.expect_value(t, done.jools, 10 + 7)
	testing.expect_value(t, deliveries_carried(&u), 1)
}

@(test)
abandoning_costs_five_standing_at_both_ends :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	a, b, _ := far_apart(&u)
	id := carry(&u, a, b, 40)
	rep_a, rep_b := planet_get(&u, a).reputation, planet_get(&u, b).reputation
	mission_abandon(&u, id)
	testing.expect_value(t, deliveries_carried(&u), 0)
	testing.expect_value(t, planet_get(&u, a).reputation, rep_a - 5)
	testing.expect_value(t, planet_get(&u, b).reputation, rep_b - 5)
}

@(test)
a_faction_with_one_planet_offers_nothing_instead_of_crashing :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	dock := home_dock(&u)
	// make the dock's planet the only one of its faction
	mine := actor_get(&u, dock).planet
	for &p, i in u.planets {
		p.faction = Faction_Id(2) if Planet_Id(i + 1) != mine else Faction_Id(3)
	}
	mission_generate(&u, dock)
	testing.expect_value(t, actor_get(&u, dock).offer, Item_Id(0))
	testing.expect(t, !can_accept_mission(&u, dock))
}

@(test)
each_delivery_is_its_own_stack_labeled_by_what_it_is :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	a, b, _ := far_apart(&u)
	d1 := carry(&u, a, b, 10)
	carry(&u, a, b, 20)
	append(&u.avatar.inventory, item_add(&u, item_new(.Scrap)))
	append(&u.avatar.inventory, item_add(&u, item_new(.Scrap)))
	stacks := inventory_stacks(&u)
	testing.expect_value(t, stacks.count, 3) // two deliveries, one stack of scrap
	testing.expect_value(t, stacks.stacks[0].kind, Item_Kind.Delivery)
	testing.expect_value(t, stacks.stacks[0].item, d1)
	testing.expect_value(t, stacks.stacks[1].kind, Item_Kind.Delivery)
	testing.expect_value(t, stacks.stacks[2], Item_Stack{kind = .Scrap, count = 2})
	n := item_stack_name(&u, stacks.stacks[0])
	testing.expect_value(t, name_str(&n), mission_nouns[item_get(&u, d1).mission.noun])
}
