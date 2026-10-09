#+build !js
package game

import "core:testing"

first_post :: proc(u: ^Universe) -> Actor_Id {
	for a, i in u.actors {
		if a.kind == .Trading_Post {
			return Actor_Id(i + 1)
		}
	}
	return 0
}

// A post on a planet of the given tech level.
post_with_tech :: proc(u: ^Universe, tech: int) -> Actor_Id {
	post := first_post(u)
	planet_get(u, actor_get(u, post).planet).tech_level = tech
	return post
}

has :: proc(list: Trade_List, kind: Item_Kind, mark: int = 0) -> bool {
	for i in 0 ..< list.count {
		if list.items[i] == {kind, mark} {
			return true
		}
	}
	return false
}

@(test)
a_low_tech_post_sells_only_tanks_and_rods :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	list := trade_prices(&u, post_with_tech(&u, 0))
	testing.expect_value(t, list.count, 2)
	testing.expect(t, has(list, .Oxygen_Tank) && has(list, .Fuel_Rod))
}

@(test)
tech_level_unlocks_equipment :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)

	list := trade_prices(&u, post_with_tech(&u, 2))
	testing.expect(t, !has(list, .Atmospheric_Concentrator))
	testing.expect(t, has(list, .Life_Support, 1) && has(list, .Fuel_Supply, 2))
	testing.expect(t, !has(list, .Life_Support, 2)) // life support Mark II needs tech level 3
	testing.expect(t, !has(list, .Fuel_Supply, 3))
	testing.expect_value(t, list.count, 2 + 1 + 2)

	list = trade_prices(&u, post_with_tech(&u, 3))
	testing.expect(t, has(list, .Atmospheric_Concentrator))
	testing.expect(t, !has(list, .Fuel_Scoop))
	testing.expect(t, has(list, .Life_Support, 2) && !has(list, .Life_Support, 3))
	testing.expect_value(t, list.count, 2 + 1 + 2 + 3)

	list = trade_prices(&u, post_with_tech(&u, 7))
	testing.expect(t, has(list, .Fuel_Scoop))
	testing.expect(t, has(list, .Life_Support, 4) && !has(list, .Life_Support, 5)) // Mark V life support needs 9
	testing.expect(t, has(list, .Fuel_Supply, 5))
	testing.expect_value(t, list.count, 2 + 1 + 1 + 4 + 5)
	list = trade_prices(&u, post_with_tech(&u, 9))
	testing.expect(t, has(list, .Life_Support, 5))
	testing.expect_value(t, list.count, 14) // everything
}

@(test)
buying_spends_jools_and_stocks_the_hold :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.jools = 100
	testing.expect_value(t, trade_max_buy(&u, .Oxygen_Tank), 20)
	testing.expect_value(t, trade_buy(&u, .Oxygen_Tank, 0, 3), 3)
	testing.expect_value(t, u.avatar.jools, 85)
	testing.expect_value(t, inventory_count(&u, .Oxygen_Tank), 3)
	for id in u.avatar.inventory {
		testing.expect_value(t, item_get(&u, id).level, 100) // bought full
	}
}

@(test)
you_cannot_buy_more_than_you_can_afford :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.jools = 47
	testing.expect_value(t, trade_buy(&u, .Fuel_Rod, 0, 10), 2) // 20 each
	testing.expect_value(t, u.avatar.jools, 7)
	testing.expect_value(t, trade_max_buy(&u, .Fuel_Rod), 0)
	testing.expect_value(t, trade_buy(&u, .Fuel_Rod, 0, 1), 0)
	u.avatar.jools = -50
	testing.expect_value(t, trade_max_buy(&u, .Oxygen_Tank), 0)
	testing.expect_value(t, trade_buy(&u, .Oxygen_Tank, 0, 5), 0)
	testing.expect_value(t, u.avatar.jools, -50)
}

@(test)
spending_exactly_everything_is_allowed :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.jools = 40
	testing.expect_value(t, trade_buy(&u, .Fuel_Rod, 0, 2), 2)
	testing.expect_value(t, u.avatar.jools, 0)
	testing.expect(t, !avatar_is_bankrupt(&u)) // the floor is below zero for most wealth levels
}

@(test)
marked_items_cost_by_mark :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.jools = 5000
	testing.expect_value(t, trade_unit_price(.Fuel_Supply, 3), 1500)
	testing.expect_value(t, trade_buy(&u, .Fuel_Supply, 3, 1), 1)
	testing.expect_value(t, u.avatar.jools, 3500)
	testing.expect_value(t, inventory_count(&u, .Fuel_Supply, 3), 1)
	testing.expect_value(t, inventory_count(&u, .Fuel_Supply, 2), 0)
	id := u.avatar.inventory[0]
	testing.expect_value(t, item_get(&u, id).level, 750)
}

@(test)
the_post_buys_only_scrap_and_only_if_you_have_some :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	post := first_post(&u)
	testing.expect_value(t, trade_offers(&u, post).count, 0)
	trade_buy(&u, .Oxygen_Tank, 0, 1)
	testing.expect_value(t, trade_offers(&u, post).count, 0) // they don't buy tanks
	for _ in 0 ..< 5 {
		append(&u.avatar.inventory, item_add(&u, item_new(.Scrap)))
	}
	offers := trade_offers(&u, post)
	testing.expect_value(t, offers.count, 1)
	testing.expect_value(t, offers.items[0].kind, Item_Kind.Scrap)
}

@(test)
selling_scrap_pays_one_jool_each :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	for _ in 0 ..< 10 {
		append(&u.avatar.inventory, item_add(&u, item_new(.Scrap)))
	}
	trade_buy(&u, .Fuel_Rod, 0, 1)
	jools := u.avatar.jools
	testing.expect_value(t, trade_offer_total(.Scrap, 7), 7)
	testing.expect_value(t, trade_sell(&u, .Scrap, 7), 7)
	testing.expect_value(t, u.avatar.jools, jools + 7)
	testing.expect_value(t, inventory_count(&u, .Scrap), 3)
	testing.expect_value(t, inventory_count(&u, .Fuel_Rod), 1) // only scrap went
	testing.expect_value(t, trade_sell(&u, .Scrap, 99), 3) // you can't sell what you don't have
	testing.expect_value(t, inventory_count(&u, .Scrap), 0)
}

@(test)
an_oxygen_tank_adds_a_hundred_and_leaves_scrap :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	trade_buy(&u, .Oxygen_Tank, 0, 2)
	u.avatar.oxygen.current = 100
	r := avatar_use_oxygen_tank(&u)
	testing.expect(t, r.used)
	testing.expect_value(t, r.added, 100)
	testing.expect_value(t, u.avatar.oxygen.current, 200)
	testing.expect_value(t, r.percent, 80)
	testing.expect_value(t, inventory_count(&u, .Oxygen_Tank), 1)
	testing.expect_value(t, inventory_count(&u, .Scrap), 1)
}

@(test)
a_tank_cannot_overfill_and_reports_what_it_really_added :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	trade_buy(&u, .Oxygen_Tank, 0, 1)
	u.avatar.oxygen.current = 200
	r := avatar_use_oxygen_tank(&u)
	testing.expect_value(t, r.added, 50)
	testing.expect_value(t, u.avatar.oxygen.current, u.avatar.oxygen.maximum)
	testing.expect_value(t, r.percent, 100)
}

@(test)
a_fuel_rod_adds_a_hundred_and_leaves_nothing :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	trade_buy(&u, .Fuel_Rod, 0, 1)
	u.avatar.fuel.current = 10
	r := avatar_use_fuel_rod(&u)
	testing.expect(t, r.used)
	testing.expect_value(t, r.added, 100)
	testing.expect_value(t, u.avatar.fuel.current, 110)
	testing.expect_value(t, len(u.avatar.inventory), 0)
	testing.expect(t, !avatar_use_fuel_rod(&u).used) // none left
	testing.expect(t, !avatar_use_oxygen_tank(&u).used)
}

@(test)
a_tank_is_used_automatically_instead_of_dying :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	trade_buy(&u, .Oxygen_Tank, 0, 1)
	u.avatar.oxygen.current = 1
	avatar_do_turn(&u) // would take the last oxygen
	testing.expect(t, !avatar_is_dead(&u))
	testing.expect_value(t, u.avatar.oxygen.current, 100)
	testing.expect(t, u.avatar.auto_used.used)
	testing.expect_value(t, u.avatar.auto_used.added, 100)
	testing.expect_value(t, inventory_count(&u, .Oxygen_Tank), 0)
	testing.expect_value(t, inventory_count(&u, .Scrap), 1)
}

@(test)
without_a_tank_the_last_breath_is_the_last :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.oxygen.current = 1
	avatar_do_turn(&u)
	testing.expect(t, avatar_is_dead(&u))
	testing.expect(t, !u.avatar.auto_used.used)
}

@(test)
every_planet_orbit_has_a_trading_post_and_a_dock :: proc(t: ^testing.T) {
	u := generate(2)
	defer universe_destroy(&u)
	two := 0
	for p, i in u.planets {
		planet_actor := actor_get(&u, p.actor)
		body := actor_at(&u, planet_actor.interior, map_center(.Planet_Vicinity))
		orbit := actor_get(&u, body).interior
		posts := 0
		cells: map[[2]int]struct{}
		defer delete(cells)
		for id in map_get(&u, orbit).actors {
			a := actor_get(&u, id)
			testing.expect(t, a.pos not_in cells) // nothing shares a cell
			cells[a.pos] = {}
			if a.kind == .Trading_Post {
				posts += 1
				testing.expect(t, a.planet == Planet_Id(i + 1))
				testing.expect(t, !map_is_edge(.Planet_Orbit, a.pos))
			}
		}
		testing.expect(t, posts == 1 || posts == 2)
		if posts == 2 {
			two += 1
		}
	}
	testing.expect(t, two < len(u.planets) / 4) // two is the rare case (about 5%)
}
