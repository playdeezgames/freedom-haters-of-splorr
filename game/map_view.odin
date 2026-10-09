package game

// Drawing the map the avatar is on: a 21x21 window centered on the ship (as in the VB), with the border
// of non-galaxy maps drawn as arrows pointing out. The ROM font is CP437, so glyphs are CP437 codes.

VIEW_SIZE :: 21
VIEW_LEFT :: 0
VIEW_TOP :: 1

GLYPH_VOID :: u8('.')
GLYPH_STAR :: u8(0x0F) // ☼
GLYPH_PLANET_MARKER :: u8('o')
GLYPH_SATELLITE_MARKER :: u8(0x09) // ○
GLYPH_PLANET_BODY :: u8(0xB2) // ▓
GLYPH_SATELLITE_BODY :: u8(0xB1) // ▒
GLYPH_STAR_DOCK :: u8(0x7F) // ⌂
GLYPH_DEBRIS :: u8('*')

direction_glyph := [Direction]u8 {
	.North = 0x1E, // ▲
	.East  = 0x10, // ►
	.South = 0x1F, // ▼
	.West  = 0x11, // ◄
}

actor_glyph :: proc(u: ^Universe, a: Actor) -> (glyph: u8, hue: Hue) {
	switch a.kind {
	case .Player_Ship:
		return direction_glyph[u.avatar.facing], .White
	case .Star_System, .Star_Vicinity, .Star:
		return GLYPH_STAR, star_info[star_system_get(u, a.star_system).star_type].hue
	case .Planet_Vicinity:
		return GLYPH_PLANET_MARKER, planet_info[planet_get(u, a.planet).type].hue
	case .Planet, .Planet_Body:
		return GLYPH_PLANET_BODY, planet_info[planet_get(u, a.planet).type].hue
	case .Satellite:
		return GLYPH_SATELLITE_MARKER, satellite_info[satellite_get(u, a.satellite).type].hue
	case .Satellite_Body:
		return GLYPH_SATELLITE_BODY, satellite_info[satellite_get(u, a.satellite).type].hue
	case .Star_Dock:
		return GLYPH_STAR_DOCK, .Brown
	case .Debris:
		return GLYPH_DEBRIS, .Light_Gray
	}
	return '?', .Light_Red
}

// The way-out arrows around a map's border; corners get diagonals.
map_edge_glyph :: proc(kind: Map_Kind, p: [2]int) -> u8 {
	size := map_sizes[kind]
	left, right, top, bottom := p.x == 0, p.x == size.x - 1, p.y == 0, p.y == size.y - 1
	switch {
	case top && left, bottom && right:
		return '\\'
	case top && right, bottom && left:
		return '/'
	case top:
		return 0x1E
	case bottom:
		return 0x1F
	case left:
		return 0x11
	case:
		return 0x10
	}
}

map_title :: proc(u: ^Universe, id: Map_Id) -> Name {
	m := map_get(u, id)
	if m.kind == .Galaxy {
		return name_make("Galaxy Map")
	}
	owner := actor_get(u, m.owner)
	switch m.kind {
	case .Galaxy:
	case .Star_System:
		return name_join(name_str(&star_system_get(u, owner.star_system).name), " System")
	case .Star_Vicinity:
		return name_join(name_str(&star_system_get(u, owner.star_system).name), " Vicinity")
	case .Planet_Vicinity:
		return name_join(name_str(&planet_get(u, owner.planet).name), " Vicinity")
	case .Planet_Orbit:
		return name_join(name_str(&planet_get(u, owner.planet).name), " Orbit")
	case .Satellite_Orbit:
		return name_join(name_str(&satellite_get(u, owner.satellite).name), " Orbit")
	}
	return {}
}

// Where the ship is, as a map and a cell.
avatar_place :: proc(u: ^Universe) -> (Map_Id, [2]int) {
	ship := actor_get(u, u.avatar.actor)
	return ship.map_id, ship.pos
}

draw_map_view :: proc(tb: ^Text_Buffer, u: ^Universe) {
	map_id, center := avatar_place(u)
	m := map_get(u, map_id)
	origin := center - VIEW_SIZE / 2 // the world cell shown at the view's top-left

	for vy in 0 ..< VIEW_SIZE {
		for vx in 0 ..< VIEW_SIZE {
			world := origin + {vx, vy}
			cell := &tb[VIEW_TOP + vy][VIEW_LEFT + vx]
			switch {
			case !map_in_bounds(m.kind, world):
				cell^ = {' ', .Black, .Black}
			case m.owner != 0 && map_is_edge(m.kind, world):
				cell^ = {map_edge_glyph(m.kind, world), .Cyan, .Black}
			case:
				cell^ = {GLYPH_VOID, .Dark_Gray, .Black}
			}
		}
	}
	// actors, in the order they joined the map, so the ship (always last to arrive) is on top
	for id in m.actors {
		a := actor_get(u, id)^
		glyph, hue := actor_glyph(u, a)
		half := a.size / 2
		for dy in -half ..= half {
			for dx in -half ..= half {
				v := a.pos + {dx, dy} - origin
				if v.x >= 0 && v.y >= 0 && v.x < VIEW_SIZE && v.y < VIEW_SIZE {
					tb[VIEW_TOP + v.y][VIEW_LEFT + v.x] = {glyph, hue, .Black}
				}
			}
		}
	}
}

// ---- Small drawing helpers shared by the play screens ----

// "Label: value" with the value in its own color; returns the column after it.
put_field :: proc(tb: ^Text_Buffer, col, row: int, label, value: string, hue: Hue = .White) -> int {
	c := text_put(tb, col, row, label, .Light_Gray)
	c = text_put(tb, c, row, ": ", .Light_Gray)
	return text_put(tb, c, row, value, hue)
}

put_field_int :: proc(tb: ^Text_Buffer, col, row: int, label: string, value: int, hue: Hue = .White) -> int {
	c := text_put(tb, col, row, label, .Light_Gray)
	c = text_put(tb, c, row, ": ", .Light_Gray)
	return text_put_int(tb, c, row, value, hue)
}

percent_of :: proc(s: Store) -> int {
	return s.current * 100 / max(s.maximum, 1)
}

// Red under a third, yellow under two thirds, green above: the VB's Hues.ForPercentage.
hue_for_percent :: proc(percent: int) -> Hue {
	switch {
	case percent < 33:
		return .Light_Red
	case percent < 66:
		return .Yellow
	}
	return .Light_Green
}

// What the avatar bumped into, described as the VB's actor descriptions did.
draw_bump_info :: proc(tb: ^Text_Buffer, u: ^Universe, bump: Bump, top_row: int) {
	row := top_row
	next :: proc(row: ^int) -> int {
		r := row^
		row^ += 2
		return r
	}
	switch b in bump {
	case Map_Edge:
		kind := map_get(u, b.map_id).kind
		text_put_centered(tb, 1, "Leave Area", .Yellow)
		text_put(tb, 2, next(&row), "You can depart the", .Light_Gray)
		text_put(tb, 2, next(&row), map_kind_names[kind], .White)
	case Actor_Id:
		a := actor_get(u, b)^
		switch a.kind {
		case .Star_System, .Star_Vicinity, .Star:
			sys := star_system_get(u, a.star_system)
			text_put_centered(tb, 1, name_str(&sys.name), star_info[sys.star_type].hue)
			put_field(tb, 2, next(&row), "Star Type", star_info[sys.star_type].name, star_info[sys.star_type].hue)
			if a.kind == .Star_System {
				c := put_field_int(tb, 2, next(&row), "Position", sys.position.x)
				c = text_put(tb, c, row - 2, ", ", .Light_Gray)
				text_put_int(tb, c, row - 2, sys.position.y, .White)
				put_field_int(tb, 2, next(&row), "Planets", sys.planet_count)
				put_field_int(tb, 2, next(&row), "Satellites", sys.satellite_count)
				put_field_int(tb, 2, next(&row), "Scrap", sys.scrap)
			}
		case .Planet_Vicinity, .Planet, .Planet_Body:
			p := planet_get(u, a.planet)
			text_put_centered(tb, 1, name_str(&p.name), planet_info[p.type].hue)
			put_field(tb, 2, next(&row), "Planet Type", planet_info[p.type].name, planet_info[p.type].hue)
			put_field_int(tb, 2, next(&row), "Tech Level", p.tech_level)
			put_field(tb, 2, next(&row), "Faction", name_str(&faction_get(u, p.faction).name))
			put_field_int(tb, 2, next(&row), "Satellites", p.satellite_count)
			put_field(tb, 2, next(&row), "Star System", name_str(&star_system_get(u, p.star_system).name))
			if planet_info[p.type].can_refill_oxygen {
				put_field(tb, 2, next(&row), "Air", "Breathable", .Light_Green)
			} else {
				put_field(tb, 2, next(&row), "Air", "Unbreathable", .Light_Red)
			}
		case .Star_Dock:
			p := planet_get(u, a.planet)
			c := text_put(tb, (TEXT_COLUMNS - len(name_str(&p.name)) - len(" Star Dock")) / 2, 1, name_str(&p.name), .Brown)
			text_put(tb, c, 1, " Star Dock", .Brown)
			put_field(tb, 2, next(&row), "Faction", name_str(&faction_get(u, p.faction).name))
			put_field_int(tb, 2, next(&row), "Tech Level", p.tech_level)
			c = put_field_int(tb, 2, next(&row), "Oxygen", 1)
			text_put(tb, c, row - 2, " jool per 10", .Light_Gray)
			c = put_field_int(tb, 2, next(&row), "Fuel", 1)
			text_put(tb, c, row - 2, " jool per 3", .Light_Gray)
		case .Satellite, .Satellite_Body:
			s := satellite_get(u, a.satellite)
			text_put_centered(tb, 1, name_str(&s.name), satellite_info[s.type].hue)
			put_field(tb, 2, next(&row), "Satellite Type", satellite_info[s.type].name, satellite_info[s.type].hue)
			put_field_int(tb, 2, next(&row), "Tech Level", s.tech_level)
			put_field(tb, 2, next(&row), "Planet", name_str(&planet_get(u, s.planet).name))
			put_field(tb, 2, next(&row), "Star System", name_str(&star_system_get(u, s.star_system).name))
		case .Debris:
			text_put_centered(tb, 1, "Debris", .Light_Gray)
			text_put(tb, 2, next(&row), "A pile of junk floating", .Light_Gray)
			text_put(tb, 2, next(&row), "around in space.", .Light_Gray)
		case .Player_Ship:
			text_put_centered(tb, 1, "(yer ship)", .White)
		}
	}
}

map_kind_names := [Map_Kind]string {
	.Galaxy          = "Galaxy",
	.Star_System     = "Star System",
	.Star_Vicinity   = "Star Vicinity",
	.Planet_Vicinity = "Planet Vicinity",
	.Planet_Orbit    = "Planet Orbit",
	.Satellite_Orbit = "Satellite Orbit",
}
