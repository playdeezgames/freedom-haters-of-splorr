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
// Military ships on the galaxy map: one for every two star systems (the VB had a flat 25).
fleet_size :: proc(star_system_count: int) -> int {
	return (star_system_count + 1) / 2
}

// The five hulls the VB drew them in, picked by actor number.
military_hues := [5]Hue{.Dark_Gray, .Light_Gray, .White, .Tan, .Brown}

STAR_GATE_COUNT_DICE :: "1d4/4" // per planet orbit: 0 or 1
SHIPYARD_COUNT_DICE :: "1d4/4" // per planet orbit: 0 or 1, so about one planet in four
TRADING_POST_COUNT_DICE :: "3d6/8" // per planet orbit; the original then takes at least 1
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

// What each value means, ported from the VB (ASCII only for the CP437 font).
group_value_descriptions := [Group_Value]string {
	.Unity_In_Diversity      = "Embracing the strength that comes from the unique contributions of all citizens, fostering a society where differences are celebrated as the foundation of collective progress.",
	.Relentless_Innovation   = "Constantly pushing the boundaries of technology and thought, ensuring that society remains at the forefront of galactic advancement.",
	.Sovereign_Freedom       = "Prioritizing the independence and autonomy of the planet and its citizens, free from external control or influence.",
	.Sustainable_Harmony     = "Balancing technological growth with environmental stewardship, ensuring that the planet's resources are preserved for future generations.",
	.Absolute_Order          = "Maintaining strict adherence to law and order as the cornerstone of a stable and prosperous society, where discipline is key to success.",
	.Collective_Prosperity   = "Ensuring that the wealth and resources of the planet are shared equitably among all citizens, fostering a strong sense of community and mutual support.",
	.Exploratory_Spirit      = "Encouraging exploration and discovery, both within the planet's own borders and beyond, as a means of growth and enlightenment.",
	.Cultural_Preservation   = "Valuing the rich history, traditions, and identity of the planet, ensuring they are protected and passed down through generations.",
	.Technocratic_Efficiency = "Prioritizing logical, data-driven decision-making, where experts and technology guide the path to progress and optimal governance.",
	.Martial_Honor           = "Emphasizing the importance of strength, courage, and military readiness, with a focus on protecting the planet and its values from external threats.",
}

VALUE_ATTEMPTS :: 3

// 1 to 3 distinct values.
group_values_roll :: proc(r: ^Rng) -> (values: Group_Values) {
	for _ in 0 ..< VALUE_ATTEMPTS {
		values += {rng_enum(r, Group_Value)}
	}
	return
}
