package game

// The kinds of things a universe is made of, ported from the VB type descriptors
// (StarTypes, PlanetTypes, SatelliteTypes, GroupValues). Star_Type itself lives in embark_settings.odin
// because the galactic-age weights are keyed by it.

// ---- Stars ----

Star_Info :: struct {
	name:                  string,
	hue:                   Hue,
	minimum_planet_distance: int, // planets in a system must be at least this far apart (and from the star)
}

star_info := [Star_Type]Star_Info {
	.Blue       = {"Blue Star", .Blue, 6},
	.Blue_White = {"Blue-White Star", .Light_Blue, 8},
	.Yellow     = {"Yellow Star", .Yellow, 10},
	.Orange     = {"Orange Star", .Orange, 12},
	.Red        = {"Red Star", .Red, 14},
}

// Every star rolls its planet count and types the same way.
PLANET_COUNT_DICE :: "2d6" // the VB table 2..12 weighted 1,2,3,4,5,6,5,4,3,2,1 is exactly 2d6

// ---- Planets ----

Planet_Type :: enum {
	Radiated,
	Toxic,
	Volcanic,
	Barren,
	Desert,
	Tundra,
	Arid,
	Ocean,
	Terran,
	Inferno,
	Tropical,
	Grassland,
	Cavernous,
	Gaia,
	Swamp,
}

Planet_Info :: struct {
	name:          string,
	hue:           Hue,
	can_refill_oxygen: bool,
}

planet_info := [Planet_Type]Planet_Info {
	.Radiated  = {"Radiated", .Yellow, false},
	.Toxic     = {"Toxic", .Magenta, false},
	.Volcanic  = {"Volcanic", .Orange, false},
	.Barren    = {"Barren", .Dark_Gray, false},
	.Desert    = {"Desert", .Brown, true},
	.Tundra    = {"Tundra", .White, true},
	.Arid      = {"Arid", .Tan, true},
	.Ocean     = {"Ocean", .Blue, true},
	.Terran    = {"Terran", .Green, true},
	.Inferno   = {"Inferno", .Red, false},
	.Tropical  = {"Tropical", .Light_Blue, true},
	.Grassland = {"Grassland", .Light_Green, true},
	.Cavernous = {"Cavernous", .Light_Gray, false},
	.Gaia      = {"Gaia", .Pink, true},
	.Swamp     = {"Swamp", .Cyan, true},
}

MINIMUM_SATELLITE_DISTANCE :: 5
DEBRIS_COUNT_DICE :: "12d6/6" // piles per star system, 2..12
DEBRIS_LOOT_DICE :: "4d6" // scrap per pile, 4..24
SATELLITE_COUNT_DICE :: "2d3+-2d1" // 0..4 weighted 1,2,3,2,1, as in the VB table
TECH_LEVEL_DICE :: "2d6+-2d1" // 0..10

// ---- Satellites ----

Satellite_Type :: enum {
	Radiated,
	Volcanic,
	Barren,
	Inferno,
	Cavernous,
	Ice,
}

Satellite_Info :: struct {
	name: string,
	hue:  Hue,
}

satellite_info := [Satellite_Type]Satellite_Info {
	.Radiated  = {"Radiated", .Yellow},
	.Volcanic  = {"Volcanic", .Orange},
	.Barren    = {"Barren", .Dark_Gray},
	.Inferno   = {"Inferno", .Red},
	.Cavernous = {"Cavernous", .Light_Gray},
	.Ice       = {"Ice", .White},
}

// ---- Faction and planet values ----

Group_Value :: enum {
	Unity_In_Diversity,
	Relentless_Innovation,
	Sovereign_Freedom,
	Sustainable_Harmony,
	Absolute_Order,
	Collective_Prosperity,
	Exploratory_Spirit,
	Cultural_Preservation,
	Technocratic_Efficiency,
	Martial_Honor,
}

// A set: picking the same value twice only counts once, as in the VB.
Group_Values :: bit_set[Group_Value]

// The long descriptions come with the pedia.
group_value_names := [Group_Value]string {
	.Unity_In_Diversity      = "Unity in Diversity",
	.Relentless_Innovation   = "Relentless Innovation",
	.Sovereign_Freedom       = "Sovereign Freedom",
	.Sustainable_Harmony     = "Sustainable Harmony",
	.Absolute_Order          = "Absolute Order",
	.Collective_Prosperity   = "Collective Prosperity",
	.Exploratory_Spirit      = "Exploratory Spirit",
	.Cultural_Preservation   = "Cultural Preservation",
	.Technocratic_Efficiency = "Technocratic Efficiency",
	.Martial_Honor           = "Martial Honor",
}

VALUE_ATTEMPTS :: 3

// 1 to 3 distinct values.
group_values_roll :: proc(r: ^Rng) -> (values: Group_Values) {
	for _ in 0 ..< VALUE_ATTEMPTS {
		values += {rng_enum(r, Group_Value)}
	}
	return
}
