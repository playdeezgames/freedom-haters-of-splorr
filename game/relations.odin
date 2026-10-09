package game

import "core:math"

// How factions feel about each other, and the words the pedia uses for a faction's three traits.
//
// The live game (v56) measured the distance with a misplaced square root, which made nearly every pair
// Hostile. This is the distance the README describes: the straight-line distance between two factions in
// (Authority, Standards, Conviction) space, 0 to about 173. SIGMO sits at 100/100/100, the far corner,
// while generated factions cluster around 50 on every axis, so almost everyone is Hostile to SIGMO and the
// rest vary: the joke, kept true by a test.

Relation :: enum {
	Friendly, // 0-25
	Neutral, // 26-50
	Hostile, // 51 and up
}

relation_names := [Relation]string {
	.Friendly = "Friendly",
	.Neutral  = "Neutral",
	.Hostile  = "Hostile",
}

relation_hues := [Relation]Hue {
	.Friendly = .Light_Green,
	.Neutral  = .Yellow,
	.Hostile  = .Light_Red,
}

faction_distance :: proc(a, b: Faction) -> f64 {
	da, ds, dc := f64(b.authority - a.authority), f64(b.standards - a.standards), f64(b.conviction - a.conviction)
	return math.sqrt(da * da + ds * ds + dc * dc)
}

// The distance is rounded to a whole number first, as the VB did.
relation_between :: proc(a, b: Faction) -> Relation {
	switch d := int(math.round(faction_distance(a, b))); {
	case d <= 25:
		return .Friendly
	case d <= 50:
		return .Neutral
	}
	return .Hostile
}

// What the pedia calls a faction's Authority, Standards or Conviction. (Odd, but the live game's words.)
trait_level_name :: proc(value: int) -> string {
	switch {
	case value > 90:
		return "Acceptable"
	case value > 75:
		return "Tolerable"
	case value > 50:
		return "Unacceptable"
	}
	return "Inexcusable"
}
