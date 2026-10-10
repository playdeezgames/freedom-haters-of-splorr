package game

// What a pedia page says. This is the one place the pedia reads the galaxy's facts: pages are built from the
// universe here and nowhere else, so that a pedia that is out of date or biased (a planned idea, see
// PORT_PLAN.md) can later stand between the truth and the page without touching the screens.

MAX_DOC_LINES :: 160
MAX_DOC_STORE :: 48

Doc_Line :: struct {
	text:   string,
	hue:    Hue,
	indent: int,
}

Doc :: struct {
	lines:  [MAX_DOC_LINES]Doc_Line,
	count:  int,
	store:  [MAX_DOC_STORE]Long_Text, // composed lines the Doc_Lines point into
	stored: int,
}

DOC_WIDTH :: TEXT_COLUMNS - 4

doc_wrap :: proc(d: ^Doc, hue: Hue, indent: int, text: string) {
	rest := text
	for len(rest) > 0 && d.count < MAX_DOC_LINES {
		line: string
		line, rest = text_wrap_next(rest, DOC_WIDTH - indent)
		d.lines[d.count] = {line, hue, indent}
		d.count += 1
	}
}

// A line built from parts (wrapped if it is long).
doc_add :: proc(d: ^Doc, hue: Hue, parts: ..string) {
	if d.stored == MAX_DOC_STORE {
		return
	}
	d.store[d.stored] = long_join(..parts)
	doc_wrap(d, hue, 0, long_str(&d.store[d.stored]))
	d.stored += 1
}

// Fixed text, wrapped and indented.
doc_text :: proc(d: ^Doc, hue: Hue, indent: int, text: string) {
	doc_wrap(d, hue, indent, text)
}

doc_blank :: proc(d: ^Doc) {
	if d.count < MAX_DOC_LINES {
		d.lines[d.count] = {"", .Black, 0}
		d.count += 1
	}
}

doc_reputation :: proc(d: ^Doc, reputation: int) {
	if reputation != 0 { // the live game shows it only once you have one with that group
		digits: [20]u8
		doc_add(d, .Light_Gray, "Reputation: ", int_text(&digits, reputation))
	}
}

doc_values :: proc(d: ^Doc, values: Group_Values) {
	if card(values) == 0 {
		return
	}
	doc_blank(d)
	doc_add(d, .White, "Values:")
	for v in Group_Value {
		if v in values {
			doc_add(d, .Light_Gray, " - ", group_value_names[v], ":")
			doc_text(d, .Light_Gray, 4, group_value_descriptions[v])
		}
	}
}

doc_trait :: proc(d: ^Doc, label: string, value: int) {
	d1: [20]u8
	doc_add(d, .Light_Gray, label, ": ", trait_level_name(value), "(", int_text(&d1, value), ")")
}

// Fills `d` with the page for `id` of `kind` and returns its title and the title's color.
pedia_page_doc :: proc(u: ^Universe, kind: Pedia_Kind, id: int, d: ^Doc) -> (title: Name, hue: Hue) {
	d1, d2: [20]u8
	switch kind {
	case .Faction:
		f := &u.factions[id - 1]
		title, hue = f.name, .Yellow
		doc_reputation(d, f.reputation)
		doc_trait(d, "Authority", f.authority)
		doc_trait(d, "Standards", f.standards)
		doc_trait(d, "Conviction", f.conviction)
		doc_add(d, .Light_Gray, "Planets: ", int_text(&d1, f.planet_count))
		doc_blank(d)
		doc_add(d, .White, "Banned Goods:")
		bans := faction_bans(f^, Faction_Id(id))
		if card(bans) == 0 {
			doc_add(d, .Light_Gray, " - none")
		}
		for good in bans {
			doc_add(d, .Light_Red, " - ", good_info[good].name)
		}
		doc_blank(d)
		doc_add(d, .White, "Other Faction Relationships:")
		for other_id in u.pedia.factions {
			if other_id == id {
				continue
			}
			other := &u.factions[other_id - 1]
			relation := relation_between(f^, other^)
			doc_add(d, relation_hues[relation], " - ", name_str(&other.name), ": ", relation_names[relation])
		}
		doc_values(d, f.values)
	case .Star_System:
		s := &u.star_systems[id - 1]
		title, hue = s.name, star_info[s.star_type].hue
		doc_reputation(d, s.reputation)
		doc_add(d, .Light_Gray, "Type: ", star_info[s.star_type].name)
		doc_add(d, .Light_Gray, "Position: (", int_text(&d1, s.position.x), ",", int_text(&d2, s.position.y), ")")
		doc_add(d, .Light_Gray, "Planet Count: ", int_text(&d1, s.planet_count))
		doc_add(d, .Light_Gray, "Satellite Count: ", int_text(&d2, s.satellite_count))
		doc_add(d, .Light_Gray, "Wormholes: ", int_text(&d1, s.wormhole_count))
		doc_blank(d)
		doc_add(d, .White, "Factions Present:")
		present := pedia_entries(u, .Faction, .Star_System, id, "")
		defer delete(present)
		for fid in present {
			doc_add(d, .Light_Gray, " - ", name_str(&u.factions[fid - 1].name))
		}
		doc_blank(d)
		doc_add(d, .Light_Gray, "Scrap: ", int_text(&d2, s.scrap))
		doc_add(d, .Light_Gray, "Shipyards: ", int_text(&d1, system_count(u, Star_System_Id(id), .Shipyard)))
		doc_add(d, .Light_Gray, "Trading Posts: ", int_text(&d2, system_count(u, Star_System_Id(id), .Trading_Post)))
		doc_add(d, .Light_Gray, "Star Gates: ", int_text(&d1, system_count(u, Star_System_Id(id), .Star_Gate)))
		doc_add(d, .Light_Gray, "Visit Count: ", int_text(&d2, s.visit_count))
	case .Planet:
		p := &u.planets[id - 1]
		title, hue = p.name, planet_info[p.type].hue
		doc_reputation(d, p.reputation)
		doc_add(d, .Light_Gray, "Planet Type: ", planet_info[p.type].name)
		doc_add(d, .Light_Gray, "Tech Level: ", int_text(&d1, p.tech_level))
		doc_add(d, .Light_Gray, "Star System: ", name_str(&u.star_systems[int(p.star_system) - 1].name))
		doc_add(d, .Light_Gray, "Satellite Count: ", int_text(&d2, p.satellite_count))
		doc_add(d, .Light_Gray, "Faction: ", name_str(&u.factions[int(p.faction) - 1].name))
		doc_values(d, p.values)
	case .Satellite:
		s := &u.satellites[id - 1]
		planet := &u.planets[int(s.planet) - 1]
		title, hue = s.name, satellite_info[s.type].hue
		doc_add(d, .Light_Gray, "Type: ", satellite_info[s.type].name)
		doc_add(d, .Light_Gray, "Tech Level: ", int_text(&d1, s.tech_level))
		doc_add(d, .Light_Gray, "Planet: ", name_str(&planet.name))
		doc_add(d, .Light_Gray, "Star System: ", name_str(&u.star_systems[int(s.star_system) - 1].name))
		doc_add(d, .Light_Gray, "Faction: ", name_str(&u.factions[int(planet.faction) - 1].name))
	}
	return
}

// Where a page can lead, besides back.
Pedia_Link :: enum {
	Planets, // the planets of this faction or system
	Satellites, // the satellites of this system or planet
	Factions, // the factions present in this system
	Faction, // this planet's or satellite's faction
	Star_System,
	Planet,
}

pedia_link_names := [Pedia_Link]string {
	.Planets     = "Planets",
	.Satellites  = "Satellites",
	.Factions    = "Factions",
	.Faction     = "Faction",
	.Star_System = "Star System",
	.Planet      = "Planet",
}

MAX_LINKS :: 4

pedia_links :: proc(kind: Pedia_Kind) -> (links: [MAX_LINKS]Pedia_Link, count: int) {
	switch kind {
	case .Faction:
		links[0] = .Planets
		count = 1
	case .Star_System:
		links[0], links[1], links[2] = .Satellites, .Factions, .Planets
		count = 3
	case .Planet:
		links[0], links[1], links[2] = .Satellites, .Faction, .Star_System
		count = 3
	case .Satellite:
		links[0], links[1], links[2] = .Faction, .Star_System, .Planet
		count = 3
	}
	return
}

// How many of a kind of station a star system has.
system_count :: proc(u: ^Universe, system: Star_System_Id, kind: Actor_Kind) -> (n: int) {
	for a in u.actors {
		if a.kind == kind && a.map_id != 0 && a.star_system == system {
			n += 1
		}
	}
	return
}
