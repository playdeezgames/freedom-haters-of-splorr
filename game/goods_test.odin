#+build !js
package game

import "core:log"
import "core:testing"

planet_of_type :: proc(u: ^Universe, type: Planet_Type) -> Planet_Id {
	for p, i in u.planets {
		if p.type == type {
			return Planet_Id(i + 1)
		}
	}
	return 0
}

// A planet whose market has no drift, pressure or opinions, so prices are the base ones.
plain_planet :: proc(u: ^Universe) -> Planet_Id {
	id := Planet_Id(1)
	p := planet_get(u, id)
	p.type = .Arid // nothing special for most goods... then flattened below
	p.tech_level = 5
	p.values = {}
	p.market = {}
	// under a law that bans nothing
	p.faction = 2
	f := faction_get(u, 2)
	f.values, f.authority, f.standards = {}, 0, 100
	return id
}

@(test)
every_good_has_a_name_a_price_and_a_weight :: proc(t: ^testing.T) {
	for good in Good {
		info := good_info[good]
		testing.expect(t, len(info.name) > 0 && info.base_price > 0 && info.weight > 0)
	}
}

@(test)
you_buy_above_the_market_and_sell_below_it :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	for good in Good {
		testing.expect(t, buy_tenths(&u, planet, good) > sell_tenths(&u, planet, good), good_info[good].name)
		testing.expect(t, sell_tenths(&u, planet, good) >= 1)
	}
}

@(test)
planet_traits_move_prices_the_way_they_should :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	p := planet_get(&u, planet)
	// breathable worlds sell oxygen cheaply, others dearly
	p.type = .Terran
	cheap := market_tenths(&u, planet, .Oxygen)
	p.type = .Toxic
	dear := market_tenths(&u, planet, .Oxygen)
	testing.expect(t, cheap < dear)
	// advanced worlds make machinery cheaply
	p.type = .Arid
	p.tech_level = 0
	backward := market_tenths(&u, planet, .Machinery)
	p.tech_level = 10
	advanced := market_tenths(&u, planet, .Machinery)
	testing.expect(t, advanced < backward)
	// order-loving worlds make the slogans; free ones distrust them
	p.values = {.Absolute_Order}
	makes := market_tenths(&u, planet, .Hype)
	p.values = {.Sovereign_Freedom}
	distrusts := market_tenths(&u, planet, .Hype)
	testing.expect(t, makes < distrusts)
	p.values = {.Absolute_Order}
	hunted := market_tenths(&u, planet, .Narcotics)
	p.values = {}
	testing.expect(t, hunted > market_tenths(&u, planet, .Narcotics))
}

@(test)
there_is_money_in_hauling_between_different_worlds :: proc(t: ^testing.T) {
	// the point of the whole system: somewhere buys a good for less than somewhere else pays for it
	u := generate(1)
	defer universe_destroy(&u)
	profitable := 0
	for good in Good {
		cheapest, dearest := 1 << 30, 0
		for _, i in u.planets {
			id := Planet_Id(i + 1)
			cheapest = min(cheapest, buy_tenths(&u, id, good))
			dearest = max(dearest, sell_tenths(&u, id, good))
		}
		if dearest > cheapest {
			profitable += 1
		}
	}
	testing.expect(t, profitable >= len(Good) - 1)
}

@(test)
buying_and_selling_move_jools_cargo_and_price :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	u.avatar.jools = 10000
	before := buy_tenths(&u, planet, .Metal)
	bought, cost := goods_buy(&u, planet, .Metal, 100)
	testing.expect_value(t, bought, 100)
	testing.expect(t, cost > 100 * before / 10) // slippage: the later units cost more than the first
	testing.expect_value(t, u.avatar.jools, 10000 - cost)
	testing.expect_value(t, u.avatar.cargo[.Metal], 100)
	testing.expect(t, buy_tenths(&u, planet, .Metal) > before) // you pushed the price up
	sold, earned := goods_sell(&u, planet, .Metal, 1000)
	testing.expect_value(t, sold, 100)
	testing.expect_value(t, u.avatar.cargo[.Metal], 0)
	testing.expect(t, earned < cost) // the spread and your own pressure: a round trip in one place loses money
	testing.expect_value(t, u.avatar.jools, 10000 - cost + earned)
}

@(test)
you_cannot_buy_your_way_into_bankruptcy :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	u.avatar.jools = 1000
	bought, _ := goods_buy(&u, planet, .Gems, 1000)
	testing.expect(t, bought > 0 && bought < 1000)
	testing.expect(t, u.avatar.jools >= 0)
	u.avatar.jools = 1
	bought, _ = goods_buy(&u, planet, .Gems, 5)
	testing.expect_value(t, bought, 0)
}

@(test)
trades_fade_and_prices_drift_over_time :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	planet_get(&u, planet).market[.Food].pressure = 400
	u.market_turn = 0
	u.turn = 10 * DRIFT_EVERY
	markets_catch_up(&u)
	testing.expect_value(t, u.market_turn, 10 * DRIFT_EVERY)
	testing.expect(t, planet_get(&u, planet).market[.Food].pressure < 400) // a long wait only replays so much
	for _ in 0 ..< 20 {
		u.turn += DRIFT_EVERY
		markets_catch_up(&u)
	}
	testing.expect_value(t, planet_get(&u, planet).market[.Food].pressure, 0)
	u.turn = u.market_turn
	for p in u.planets {
		for state in p.market {
			testing.expect(t, abs(state.drift) <= DRIFT_LIMIT)
		}
	}
	// nothing happens before the next tick is due
	turn := u.market_turn
	u.turn += DRIFT_EVERY - 1
	markets_catch_up(&u)
	testing.expect_value(t, u.market_turn, turn)
}

@(test)
cargo_weight_costs_fuel_on_every_move :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.cargo[.Metal] = 25 // weight 100: 2 extra fuel a move
	testing.expect_value(t, cargo_weight(&u), 100)
	testing.expect_value(t, cargo_fuel_tenths(&u), 20)
	testing.expect_value(t, cargo_fuel_for_move(&u), 2)
	u.avatar.fuel_carry = 0
	u.avatar.cargo = {}
	// smooth: 25 weight is half a fuel a move, so one extra fuel every second move, never a cliff
	u.avatar.cargo[.Gems] = 25
	burned := 0
	for _ in 0 ..< 10 {
		burned += cargo_fuel_for_move(&u)
	}
	testing.expect_value(t, burned, 5)
	u.avatar.fuel_carry = 0
	u.avatar.cargo[.Gems] = 24 // just under: nearly the same, no cliff either side of 25
	burned = 0
	for _ in 0 ..< 50 {
		burned += cargo_fuel_for_move(&u)
	}
	testing.expect_value(t, burned, 24)
	u.avatar.fuel_carry = 0
	u.avatar.cargo[.Gems] = 100
	fuel := u.avatar.fuel.current
	avatar_move(&u, .North)
	testing.expect_value(t, u.avatar.fuel.current, fuel - 1 - 2)
	// never below empty
	u.avatar.fuel.current = 2
	avatar_move(&u, .North)
	testing.expect_value(t, u.avatar.fuel.current, u.avatar.fuel.minimum)
}

@(test)
losing_a_fight_costs_three_quarters_of_the_cargo :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	u.avatar.cargo[.Gems] = 40
	u.avatar.cargo[.Metal] = 3
	c := combat_start(&u, ship_of(&u, false))
	combat_defeat(&u, c)
	testing.expect_value(t, u.avatar.cargo[.Gems], 10)
	testing.expect_value(t, u.avatar.cargo[.Metal], 1)
}

@(test)
trading_goods_through_the_screens :: proc(t: ^testing.T) {
	app: App
	at_the_trader(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.jools = 5000
	press(&app, KEY_DOWN, KEY_ENTER) // Buy, Trade Goods, Leave
	testing.expect(t, on_screen(&app, Market_Screen))
	press(&app, KEY_DOWN, KEY_ENTER) // Metal
	testing.expect(t, on_screen(&app, Good_Trade))
	press(&app, KEY_DOWN, KEY_ENTER) // Buy 10
	testing.expect_value(t, u.avatar.cargo[.Metal], 10)
	testing.expect(t, u.avatar.jools < 5000)
	press(&app, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_DOWN, KEY_ENTER) // Sell All
	testing.expect_value(t, u.avatar.cargo[.Metal], 0)
	press(&app, KEY_DOWN, KEY_ENTER) // Done
	testing.expect(t, on_screen(&app, Market_Screen))
	press(&app, KEY_ESCAPE)
	testing.expect(t, on_screen(&app, Trader))
}

@(test)
a_saved_game_keeps_its_cargo_and_markets :: proc(t: ^testing.T) {
	u := generate(3)
	defer universe_destroy(&u)
	u.avatar.cargo[.Textiles] = 17
	planet_get(&u, 2).market[.Gems] = {drift = 9, pressure = -33}
	data := universe_to_bytes(&u)
	defer delete(data)
	loaded, err := universe_from_bytes(data)
	testing.expect_value(t, err, Load_Error.None)
	defer universe_destroy(&loaded)
	testing.expect_value(t, loaded.avatar.cargo[.Textiles], 17)
	testing.expect_value(t, planet_get(&loaded, 2).market[.Gems], Price_State{drift = 9, pressure = -33})
}

@(test)
slippage_prices_each_unit_after_the_ones_before_it :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	one := quote_tenths(&u, planet, .Gems, 1, .Buy)
	hundred := quote_tenths(&u, planet, .Gems, 100, .Buy)
	testing.expect(t, hundred > 100 * one) // later units are dearer
	// the 100th unit costs about 30% more than the first (0.3% a unit)
	last := unit_tenths(&u, planet, .Gems, 99, .Buy)
	testing.expect(t, last * 100 / one >= 125 && last * 100 / one <= 135)
	// selling mirrors it: later units pay less
	sell_hundred := quote_tenths(&u, planet, .Gems, 100, .Sell)
	testing.expect(t, sell_hundred < 100 * quote_tenths(&u, planet, .Gems, 1, .Sell))
	// and the effect is capped at 50%, so a flood never goes below half price
	testing.expect(t, unit_tenths(&u, planet, .Gems, -10000, .Market) * 2 >= unit_tenths(&u, planet, .Gems, 0, .Market) - 2)
}

@(test)
cheap_goods_keep_their_ten_percent_spread :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	for good in Good {
		market := market_tenths(&u, planet, good)
		buy, sell := buy_tenths(&u, planet, good), sell_tenths(&u, planet, good)
		testing.expect(t, buy >= market && sell <= market)
		// within a tenth of a jool of the stated 10% either way, even for the cheapest goods
		testing.expect(t, abs(buy - market * (100 + SPREAD_PERCENT) / 100) <= 1, good_info[good].name)
		testing.expect(t, abs(sell - market * (100 - SPREAD_PERCENT) / 100) <= 1, good_info[good].name)
	}
	testing.expect_value(t, tenths_text_for(53), "5.3")
	testing.expect_value(t, tenths_text_for(100), "10.0")
	testing.expect_value(t, tenths_text_for(7), "0.7")
}

tenths_text_for :: proc(n: int) -> string {
	@(static) buf: [24]u8
	return tenths_text(&buf, n)
}

@(test)
hauling_within_one_system_no_longer_doubles_your_money :: proc(t: ^testing.T) {
	// the playtest exploit: buy all you can at one planet of a system and sell it at the next, over and over
	for seed in 1 ..= 3 {
		u := generate(u64(seed))
		defer universe_destroy(&u)
		planets: [dynamic]Planet_Id
		defer delete(planets)
		for p, i in u.planets {
			if p.star_system == 1 {
				append(&planets, Planet_Id(i + 1))
			}
		}
		if len(planets) < 2 {
			continue
		}
		u.avatar.jools = 1000
		here := planets[0]
		for hop in 0 ..< 30 {
			best_profit, best_good, best_dest := 0, Good.Gems, here
			for good in Good {
				if good_banned_at(&u, here, good) || good_info[good].weight > 1 {
					continue
				}
				units := goods_max_buy(&u, here, good)
				for dest in planets {
					if dest == here || good_banned_at(&u, dest, good) {
						continue
					}
					profit := quote_sell(&u, dest, good, units) - quote_buy(&u, here, good, units)
					if profit > best_profit {
						best_profit, best_good, best_dest = profit, good, dest
					}
				}
			}
			if best_profit > 0 {
				goods_buy(&u, here, best_good, 1 << 20)
				here = best_dest
				goods_sell(&u, here, best_good, 1 << 20)
			} else {
				here = planets[(hop + 1) % len(planets)]
			}
			u.turn += 12
			markets_catch_up(&u)
		}
		log.infof("seed %d: %d jools after 30 same-system hops", seed, u.avatar.jools)
		// thirty hops used to be hundreds of millions of jools
		testing.expectf(t, u.avatar.jools < 5000, "seed %d: %d jools after 30 hops", seed, u.avatar.jools)
	}
}
