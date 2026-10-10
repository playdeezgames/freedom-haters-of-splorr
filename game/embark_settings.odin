package game

import "base:intrinsics"

// What the player chooses on the Embark screen. The tables below are ported from the VB descriptors
// (GalacticAges, GalacticDensities, StartingWealthLevels, FactionCounts); generation will read them.

Star_Type :: enum {
	Blue,
	Blue_White,
	Yellow,
	Orange,
	Red,
}

Galactic_Age :: enum {
	Young,
	Average,
	Old,
}

Galactic_Density :: enum {
	Dense,
	Average,
	Sparse,
}

Starting_Wealth :: enum {
	Very_Poor,
	Poor,
	Middle,
	Rich,
	Very_Rich,
}

MIN_FACTION_COUNT :: 2
MAX_FACTION_COUNT :: 6

Embark_Settings :: struct {
	age:           Galactic_Age,
	density:       Galactic_Density,
	wealth:        Starting_Wealth,
	faction_count: int,
}

DEFAULT_EMBARK_SETTINGS :: Embark_Settings {
	age           = .Average,
	density       = .Average,
	wealth        = .Middle,
	faction_count = 4,
}

// ---- Galactic age: how likely each star type is ----

galactic_age_names := [Galactic_Age]string {
	.Young   = "Young",
	.Average = "Average",
	.Old     = "Old",
}

// Relative weights for rolling a star's type.
star_type_weights := [Galactic_Age][Star_Type]int {
	.Young   = {.Blue = 5, .Blue_White = 4, .Yellow = 3, .Orange = 2, .Red = 1},
	.Average = {.Blue = 1, .Blue_White = 1, .Yellow = 1, .Orange = 1, .Red = 1},
	.Old     = {.Blue = 1, .Blue_White = 2, .Yellow = 3, .Orange = 4, .Red = 5},
}

// ---- Galactic density: how far apart things must be ----

galactic_density_names := [Galactic_Density]string {
	.Dense   = "Dense",
	.Average = "Average",
	.Sparse  = "Sparse",
}

Density_Spacing :: struct {
	minimum_distance:          int, // between stars
	minimum_wormhole_distance: int, // between wormhole ends
}

density_spacing := [Galactic_Density]Density_Spacing {
	.Dense   = {4, 6},
	.Average = {8, 12},
	.Sparse  = {12, 18},
}

// ---- Starting wealth ----

starting_wealth_names := [Starting_Wealth]string {
	.Very_Poor = "Very Poor",
	.Poor      = "Poor",
	.Middle    = "Middle Class",
	.Rich      = "Rich",
	.Very_Rich = "Very Rich",
}

// Starting jools are `first + step * k` for k in [0, count) chosen uniformly.
// `wallet_minimum` is the balance at which the avatar is bankrupt.
Wealth_Profile :: struct {
	first:          int,
	count:          int,
	step:           int,
	wallet_minimum: int,
}

wealth_profiles := [Starting_Wealth]Wealth_Profile {
	.Very_Poor = {first = 0, count = 1, step = 1, wallet_minimum = -999},
	.Poor      = {first = 450, count = 100, step = 1, wallet_minimum = -999},
	.Middle    = {first = 900, count = 100, step = 2, wallet_minimum = -999},
	.Rich      = {first = 4500, count = 100, step = 10, wallet_minimum = -499},
	.Very_Rich = {first = 9000, count = 100, step = 20, wallet_minimum = 0},
}

// Largest value in the jools roll.
wealth_max_jools :: proc(w: Starting_Wealth) -> int {
	p := wealth_profiles[w]
	return p.first + p.step * (p.count - 1)
}

// ---- Faction count ----

faction_count_names := [MAX_FACTION_COUNT + 1]string{"", "", "Two", "Three", "Four", "Five", "Six"}

// ---- Changing settings ----

// Next/previous enum value, wrapping around. delta is +1 or -1.
cycle :: proc(v: $E, delta: int) -> E where intrinsics.type_is_enum(E) {
	n := len(E)
	return E((int(v) + n + delta) % n)
}

// Faction count stays within [MIN_FACTION_COUNT, MAX_FACTION_COUNT], wrapping around.
cycle_faction_count :: proc(count: int, delta: int) -> int {
	span := MAX_FACTION_COUNT - MIN_FACTION_COUNT + 1
	return MIN_FACTION_COUNT + (count - MIN_FACTION_COUNT + span + delta) % span
}

// ---- Remembering the settings between visits ----
// Four bytes: age, density, wealth, faction count. Anything that doesn't fit the game's choices is ignored.

settings_save :: proc(s: Storage, settings: Embark_Settings) {
	if s.write == nil {
		return
	}
	data := [4]u8{u8(settings.age), u8(settings.density), u8(settings.wealth), u8(settings.faction_count)}
	s.write(SETTINGS_KEY, data[:])
}

settings_load :: proc(s: Storage) -> Embark_Settings {
	data, ok := storage_get(s, SETTINGS_KEY)
	if !ok {
		return DEFAULT_EMBARK_SETTINGS
	}
	defer delete(data)
	if len(data) != 4 || int(data[0]) >= len(Galactic_Age) || int(data[1]) >= len(Galactic_Density) || int(data[2]) >= len(Starting_Wealth) || int(data[3]) < MIN_FACTION_COUNT || int(data[3]) > MAX_FACTION_COUNT {
		return DEFAULT_EMBARK_SETTINGS
	}
	return {age = Galactic_Age(data[0]), density = Galactic_Density(data[1]), wealth = Starting_Wealth(data[2]), faction_count = int(data[3])}
}
