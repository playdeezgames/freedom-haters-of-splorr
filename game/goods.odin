package game

// Trade goods: cargo bought and sold at a planet's market (new in the port; the VB's commodities were never
// finished). Every planet has a market, shared by its trading posts. A good's price is its base price,
// scaled by what the planet is like (its type, tech level and values), by a slow random drift, and by how
// much you have recently bought (dearer) or sold (cheaper). Cargo is heavy: every WEIGHT_PER_FUEL of weight
// adds a fuel to each move. Nothing here knows about screens.

Good :: enum {
	Metal,
	Oxygen,
	Fuel,
	Food,
	Textiles,
	Machinery,
	Medicine,
	Hype,
	Gems,
	Narcotics,
	Weapons,
}

Good_Info :: struct {
	name:       string,
	base_price: int, // jools per unit at a planet with no opinions about it
	weight:     int, // per unit
}

good_info := [Good]Good_Info {
	.Metal     = {"Metal", 8, 4},
	.Oxygen    = {"Oxygen", 6, 2},
	.Fuel      = {"Fuel", 6, 3},
	.Food      = {"Food", 8, 2},
	.Textiles  = {"Textiles", 12, 1},
	.Machinery = {"Machinery", 28, 3},
	.Medicine  = {"Medicine", 36, 1},
	.Hype      = {"Hype", 16, 1},
	.Gems      = {"Gems", 120, 1},
	.Narcotics = {"Narcotics", 80, 1},
	.Weapons   = {"Weapons", 60, 2},
}

WEIGHT_PER_FUEL :: 25
SPREAD_PERCENT :: 10 // you buy this much above the market price and sell this much below it
DRIFT_EVERY :: 50 // turns
DRIFT_STEP :: 5 // percent either way each time
DRIFT_LIMIT :: 25
UNITS_PER_PERCENT :: 5 // units traded to move a price one percent
PRESSURE_LIMIT :: 50 // percent

// What one planet's market has been doing to one good's price, as percentages.
Price_State :: struct {
	drift:    int, // random walk
	pressure: int, // units you have bought (+) or sold (-) lately; fades over time
}

// ---- What the planet is like ----

// A percentage of the base price, from 100 up or down.
@(private = "file")
type_percent :: proc(type: Planet_Type, good: Good) -> int {
	rocky := type == .Barren || type == .Cavernous || type == .Volcanic || type == .Radiated
	lush := type == .Terran || type == .Grassland || type == .Tropical || type == .Gaia || type == .Swamp
	hostile := type == .Barren || type == .Volcanic || type == .Inferno || type == .Toxic || type == .Radiated || type == .Cavernous
	switch good {
	case .Metal:
		return 65 if rocky else 140 if lush else 100
	case .Oxygen:
		return 65 if planet_info[type].can_refill_oxygen else 150
	case .Fuel:
		return 65 if type == .Volcanic || type == .Inferno || type == .Radiated else 135 if type == .Tundra || type == .Ocean else 100
	case .Food:
		return 60 if lush else 160 if hostile else 100
	case .Textiles:
		return 75 if type == .Grassland || type == .Arid || type == .Desert else 130 if hostile else 100
	case .Gems:
		return 60 if type == .Cavernous || type == .Volcanic || type == .Radiated else 130 if type == .Terran || type == .Gaia else 100
	case .Narcotics:
		return 60 if type == .Toxic || type == .Swamp || type == .Tropical else 100
	case .Machinery, .Medicine, .Hype, .Weapons:
		return 100
	}
	return 100
}

// Advanced worlds make machinery, medicine and arms cheaply; they hold little with slogans nobody has heard.
@(private = "file")
tech_percent :: proc(tech: int, good: Good) -> int {
	switch good {
	case .Machinery:
		return 130 - 6 * tech
	case .Medicine:
		return 140 - 7 * tech
	case .Weapons:
		return 120 - 4 * tech
	case .Metal, .Oxygen, .Fuel, .Food, .Textiles, .Hype, .Gems, .Narcotics:
	}
	return 100
}

@(private = "file")
values_percent :: proc(values: Group_Values, good: Good) -> int {
	percent := 100
	switch good {
	case .Hype:
		if .Absolute_Order in values || .Martial_Honor in values {
			percent -= 30 // they make the slogans
		}
		if .Sovereign_Freedom in values || .Unity_In_Diversity in values {
			percent += 40 // and distrust them
		}
	case .Narcotics:
		if .Absolute_Order in values {
			percent += 50 // scarce where it is hunted
		}
		if .Sovereign_Freedom in values {
			percent -= 20
		}
	case .Weapons:
		if .Martial_Honor in values {
			percent -= 30
		}
		if .Sustainable_Harmony in values {
			percent += 50
		}
	case .Medicine:
		if .Collective_Prosperity in values {
			percent -= 20
		}
	case .Machinery:
		if .Technocratic_Efficiency in values {
			percent -= 20
		}
	case .Metal, .Oxygen, .Fuel, .Food, .Textiles, .Gems:
	}
	return percent
}

// The market price of a good at a planet: what a unit is worth there right now, before the spread.
market_price :: proc(u: ^Universe, planet: Planet_Id, good: Good) -> int {
	p := planet_get(u, planet)
	traits := type_percent(p.type, good) * tech_percent(p.tech_level, good) / 100 * values_percent(p.values, good) / 100
	state := p.market[good]
	moved := clamp(state.pressure / UNITS_PER_PERCENT, -PRESSURE_LIMIT, PRESSURE_LIMIT)
	scale := max(100 + state.drift + moved, 10)
	return max(1, good_info[good].base_price * traits * scale / 10000)
}

// What you pay for one unit, and what you are paid for one. The gap between them is the market's cut.
buy_price :: proc(u: ^Universe, planet: Planet_Id, good: Good) -> int {
	price := market_price(u, planet, good)
	return price + (price * SPREAD_PERCENT + 99) / 100
}

sell_price :: proc(u: ^Universe, planet: Planet_Id, good: Good) -> int {
	price := market_price(u, planet, good)
	return max(1, price - (price * SPREAD_PERCENT + 99) / 100)
}

// ---- Cargo ----

cargo_units :: proc(u: ^Universe) -> (n: int) {
	for count in u.avatar.cargo {
		n += count
	}
	return
}

cargo_weight :: proc(u: ^Universe) -> (w: int) {
	for good in Good {
		w += u.avatar.cargo[good] * good_info[good].weight
	}
	return
}

// Extra fuel every move costs because of the load.
cargo_fuel_surcharge :: proc(u: ^Universe) -> int {
	return cargo_weight(u) / WEIGHT_PER_FUEL
}

// How many of a good you can buy right now: what your jools cover, since there is no hold limit.
goods_max_buy :: proc(u: ^Universe, planet: Planet_Id, good: Good, black := false) -> int {
	if !black && good_banned_at(u, planet, good) {
		return 0
	}
	price := black_buy_price(u, planet, good) if black else buy_price(u, planet, good)
	return max(u.avatar.jools, 0) / price // like the trading post's items: all you have, never more
}

// Buys up to `quantity` (never more than the jools allow). Each unit bought nudges the price up.
// Returns how many were bought and what they cost.
goods_buy :: proc(u: ^Universe, planet: Planet_Id, good: Good, quantity: int, black := false) -> (bought, cost: int) {
	if !black && good_banned_at(u, planet, good) {
		return
	}
	bought = min(quantity, goods_max_buy(u, planet, good, black))
	cost = bought * (black_buy_price(u, planet, good) if black else buy_price(u, planet, good))
	u.avatar.jools -= cost
	u.avatar.cargo[good] += bought
	planet_get(u, planet).market[good].pressure += bought
	return
}

// Sells up to `quantity` of what you hold. Each unit sold nudges the price down.
goods_sell :: proc(u: ^Universe, planet: Planet_Id, good: Good, quantity: int, black := false) -> (sold, earned: int) {
	if !black && good_banned_at(u, planet, good) {
		return
	}
	sold = min(quantity, u.avatar.cargo[good])
	earned = sold * (black_sell_price(u, planet, good) if black else sell_price(u, planet, good))
	u.avatar.jools += earned
	u.avatar.cargo[good] -= sold
	planet_get(u, planet).market[good].pressure -= sold
	avatar_gain_infamy(u, infamy_for_sale(good, sold))
	return
}

// ---- Time ----

// Starts a market's drift at a random spot so planets disagree from the first turn.
market_start :: proc(u: ^Universe, planet: Planet_Id) {
	p := planet_get(u, planet)
	for good in Good {
		p.market[good].drift = rng_range(&u.rng, -15, 15)
	}
}

// One drift tick: every price wanders a little and your trades fade.
market_drift :: proc(u: ^Universe) {
	for &p in u.planets {
		for good in Good {
			state := &p.market[good]
			state.drift = clamp(state.drift + rng_range(&u.rng, -DRIFT_STEP, DRIFT_STEP), -DRIFT_LIMIT, DRIFT_LIMIT)
			if state.pressure != 0 {
				fade := max(1, abs(state.pressure) / 4)
				state.pressure -= fade if state.pressure > 0 else -fade
			}
		}
	}
}

// Brings the markets up to date with the turn counter (one drift every DRIFT_EVERY turns).
markets_catch_up :: proc(u: ^Universe) {
	u.market_turn = max(u.market_turn, u.turn - 4 * DRIFT_EVERY) // a loaded game doesn't replay ages
	for u.market_turn + DRIFT_EVERY <= u.turn {
		u.market_turn += DRIFT_EVERY
		market_drift(u)
	}
}
