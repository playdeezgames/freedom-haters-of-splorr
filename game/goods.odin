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

WEIGHT_PER_FUEL :: 50 // weight per fuel of extra burn on every move
SPREAD_PERCENT :: 10 // you buy this much above the market price and sell this much below it
DRIFT_EVERY :: 50 // turns
DRIFT_STEP :: 5 // percent either way each time
DRIFT_LIMIT :: 15

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

// ---- Prices ----
//
// Prices are in tenths of a jool so cheap goods keep their real spread, and a bulk trade is priced a unit at a
// time: every unit bought pushes the price up and every unit sold pushes it down (slippage), so a flood of
// goods ends well away from where it started. Only the total is rounded, once, to whole jools.

Price_Mode :: enum {
	Market, // the market price itself
	Buy, // from a trading post: a little over the market price
	Sell, // to a trading post: a little under it
	Black_Buy, // from a black market
	Black_Sell, // to a black market
}

TRAIT_COMPRESS :: 40 // percent of a planet's price opinions that survives (they were too wide)
TRAIT_FLOOR :: 70
TRAIT_CEILING :: 150
PERMILLE_PER_UNIT :: 4 // each unit bought or sold moves the price 0.4%
PRESSURE_PERMILLE_LIMIT :: 500
MAX_TRADE_UNITS :: 10000

// A good's price at a planet before pressure, drift and the spread, in tenths of a jool per unit.
@(private = "file")
base_tenths :: proc(p: ^Planet, good: Good) -> int {
	raw := type_percent(p.type, good) * tech_percent(p.tech_level, good) / 100 * values_percent(p.values, good) / 100
	traits := clamp(100 + (raw - 100) * TRAIT_COMPRESS / 100, TRAIT_FLOOR, TRAIT_CEILING)
	return good_info[good].base_price * 10 * traits / 100
}

@(private = "file")
mode_percent :: proc(u: ^Universe, mode: Price_Mode) -> int {
	switch mode {
	case .Market:
		return 100
	case .Buy:
		return 100 + SPREAD_PERCENT
	case .Sell:
		return 100 - SPREAD_PERCENT
	case .Black_Buy:
		return BLACK_BUY_PERCENT
	case .Black_Sell:
		return BLACK_SELL_BASE_PERCENT + min(u.avatar.infamy / BLACK_SELL_INFAMY_STEP, BLACK_SELL_INFAMY_LIMIT)
	}
	return 100
}

// The price of one unit when `extra` more units have already been bought (positive) or sold (negative)
// in this trade, in tenths of a jool.
unit_tenths :: proc(u: ^Universe, planet: Planet_Id, good: Good, extra: int, mode: Price_Mode) -> int {
	p := planet_get(u, planet)
	state := p.market[good]
	moved := clamp((state.pressure + extra) * PERMILLE_PER_UNIT, -PRESSURE_PERMILLE_LIMIT, PRESSURE_PERMILLE_LIMIT)
	scale := max(1000 + state.drift * 10 + moved, 100)
	market := base_tenths(p, good) * scale / 1000
	return max(1, market * mode_percent(u, mode) / 100)
}

// The market price of a good now, before the spread (tenths of a jool).
market_tenths :: proc(u: ^Universe, planet: Planet_Id, good: Good) -> int {
	return unit_tenths(u, planet, good, 0, .Market)
}

buy_tenths :: proc(u: ^Universe, planet: Planet_Id, good: Good) -> int {
	return unit_tenths(u, planet, good, 0, .Buy)
}

sell_tenths :: proc(u: ^Universe, planet: Planet_Id, good: Good) -> int {
	return unit_tenths(u, planet, good, 0, .Sell)
}

black_buy_tenths :: proc(u: ^Universe, planet: Planet_Id, good: Good) -> int {
	return unit_tenths(u, planet, good, 0, .Black_Buy)
}

black_sell_tenths :: proc(u: ^Universe, planet: Planet_Id, good: Good) -> int {
	return unit_tenths(u, planet, good, 0, .Black_Sell)
}

// The total for `units`, in tenths: each one priced after the ones before it.
quote_tenths :: proc(u: ^Universe, planet: Planet_Id, good: Good, units: int, mode: Price_Mode) -> (total: int) {
	buying := mode == .Buy || mode == .Black_Buy
	for i in 0 ..< units {
		total += unit_tenths(u, planet, good, i if buying else -i, mode)
	}
	return
}

// What buying costs in whole jools (rounded up) and what selling pays (rounded down).
quote_buy :: proc(u: ^Universe, planet: Planet_Id, good: Good, units: int, black := false) -> int {
	return (quote_tenths(u, planet, good, units, .Black_Buy if black else .Buy) + 9) / 10
}

quote_sell :: proc(u: ^Universe, planet: Planet_Id, good: Good, units: int, black := false) -> int {
	return quote_tenths(u, planet, good, units, .Black_Sell if black else .Sell) / 10
}

// "5.3" for 53 tenths.
tenths_text :: proc(buf: ^[24]u8, tenths: int) -> string {
	whole, frac := tenths / 10, tenths % 10
	digits: [20]u8
	text := int_text(&digits, whole)
	n := copy(buf[:], text)
	buf[n] = '.'
	buf[n + 1] = u8('0' + frac)
	return string(buf[:n + 2])
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

// The extra fuel a move costs because of the load, in tenths per move (weight/50, so 25 weight is 0.5).
cargo_fuel_tenths :: proc(u: ^Universe) -> int {
	return cargo_weight(u) * 10 / WEIGHT_PER_FUEL
}

// Whole fuel to burn for this move: the fractional part builds up in `fuel_carry` and comes due later.
cargo_fuel_for_move :: proc(u: ^Universe) -> int {
	u.avatar.fuel_carry += cargo_weight(u)
	extra := u.avatar.fuel_carry / WEIGHT_PER_FUEL
	u.avatar.fuel_carry %= WEIGHT_PER_FUEL
	return extra
}

// How many of a good you can buy right now: what your jools cover, since there is no hold limit.
goods_max_buy :: proc(u: ^Universe, planet: Planet_Id, good: Good, black := false) -> int {
	if !black && good_banned_at(u, planet, good) {
		return 0
	}
	jools := max(u.avatar.jools, 0)
	mode := Price_Mode.Black_Buy if black else .Buy
	total, units := 0, 0
	for units < MAX_TRADE_UNITS {
		total += unit_tenths(u, planet, good, units, mode)
		if (total + 9) / 10 > jools {
			break
		}
		units += 1
	}
	return units
}

// Buys up to `quantity` (never more than the jools allow). Returns how many were bought and what they cost.
goods_buy :: proc(u: ^Universe, planet: Planet_Id, good: Good, quantity: int, black := false) -> (bought, cost: int) {
	if !black && good_banned_at(u, planet, good) {
		return
	}
	bought = min(quantity, goods_max_buy(u, planet, good, black))
	cost = quote_buy(u, planet, good, bought, black)
	u.avatar.jools -= cost
	u.avatar.cargo[good] += bought
	planet_get(u, planet).market[good].pressure += bought
	return
}

// Sells up to `quantity` of what you hold.
goods_sell :: proc(u: ^Universe, planet: Planet_Id, good: Good, quantity: int, black := false) -> (sold, earned: int) {
	if !black && good_banned_at(u, planet, good) {
		return
	}
	sold = min(quantity, u.avatar.cargo[good], MAX_TRADE_UNITS)
	earned = quote_sell(u, planet, good, sold, black)
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
		p.market[good].drift = rng_range(&u.rng, -10, 10)
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
