package game

import "core:slice"

// The SPLORRPedia's data: every faction, star system, planet and satellite, sorted by name, and the lists
// the pedia builds from them. Like the live game's, it knows the whole galaxy from the start.

Pedia_Kind :: enum {
	Faction,
	Star_System,
	Planet,
	Satellite,
}

// What a list is limited to, besides the typed filter.
Pedia_Scope :: enum {
	All,
	Faction, // a faction's planets
	Star_System, // a star system's planets, satellites or factions
	Planet, // a planet's satellites
}

// Ids (1-based, as ints) of each kind, in name order. Built once, when generation finishes.
Pedia_Index :: struct {
	factions:     [dynamic]int,
	star_systems: [dynamic]int,
	planets:      [dynamic]int,
	satellites:   [dynamic]int,
}

pedia_destroy :: proc(p: ^Pedia_Index) {
	delete(p.factions)
	delete(p.star_systems)
	delete(p.planets)
	delete(p.satellites)
	p^ = {}
}

@(private = "file")
sort_by_name :: proc(ids: []int, u: ^Universe, name_of: proc(u: ^Universe, id: int) -> string) {
	Context :: struct {
		u:       ^Universe,
		name_of: proc(u: ^Universe, id: int) -> string,
	}
	c := Context{u, name_of}
	slice.sort_by_with_data(ids, proc(a, b: int, data: rawptr) -> bool {
			c := (^Context)(data)
			return c.name_of(c.u, a) < c.name_of(c.u, b)
		}, &c)
}

pedia_build :: proc(u: ^Universe) {
	pedia_destroy(&u.pedia)
	for _, i in u.factions {
		append(&u.pedia.factions, i + 1)
	}
	for _, i in u.star_systems {
		append(&u.pedia.star_systems, i + 1)
	}
	for _, i in u.planets {
		append(&u.pedia.planets, i + 1)
	}
	for _, i in u.satellites {
		append(&u.pedia.satellites, i + 1)
	}
	sort_by_name(u.pedia.factions[:], u, proc(u: ^Universe, id: int) -> string {return name_str(&u.factions[id - 1].name)})
	sort_by_name(u.pedia.star_systems[:], u, proc(u: ^Universe, id: int) -> string {return name_str(&u.star_systems[id - 1].name)})
	sort_by_name(u.pedia.planets[:], u, proc(u: ^Universe, id: int) -> string {return name_str(&u.planets[id - 1].name)})
	sort_by_name(u.pedia.satellites[:], u, proc(u: ^Universe, id: int) -> string {return name_str(&u.satellites[id - 1].name)})
}

pedia_name :: proc(u: ^Universe, kind: Pedia_Kind, id: int) -> string {
	switch kind {
	case .Faction:
		return name_str(&u.factions[id - 1].name)
	case .Star_System:
		return name_str(&u.star_systems[id - 1].name)
	case .Planet:
		return name_str(&u.planets[id - 1].name)
	case .Satellite:
		return name_str(&u.satellites[id - 1].name)
	}
	return ""
}

// Case-insensitive "contains"; an empty filter matches everything.
name_matches :: proc(name, filter: string) -> bool {
	if len(filter) == 0 {
		return true
	}
	lower :: proc(c: u8) -> u8 {
		return c + ('a' - 'A') if c >= 'A' && c <= 'Z' else c
	}
	for i in 0 ..= len(name) - len(filter) {
		match := true
		for j in 0 ..< len(filter) {
			if lower(name[i + j]) != lower(filter[j]) {
				match = false
				break
			}
		}
		if match {
			return true
		}
	}
	return false
}

// The ids a pedia list shows: of `kind`, inside `scope`/`scope_id`, whose names contain `filter`, by name.
// The caller frees the result.
pedia_entries :: proc(u: ^Universe, kind: Pedia_Kind, scope: Pedia_Scope, scope_id: int, filter: string) -> (ids: [dynamic]int) {
	all: []int
	switch kind {
	case .Faction:
		all = u.pedia.factions[:]
	case .Star_System:
		all = u.pedia.star_systems[:]
	case .Planet:
		all = u.pedia.planets[:]
	case .Satellite:
		all = u.pedia.satellites[:]
	}
	for id in all {
		in_scope := true
		switch scope {
		case .All:
		case .Faction:
			in_scope = kind == .Planet && int(u.planets[id - 1].faction) == scope_id
		case .Star_System:
			switch kind {
			case .Planet:
				in_scope = int(u.planets[id - 1].star_system) == scope_id
			case .Satellite:
				in_scope = int(u.satellites[id - 1].star_system) == scope_id
			case .Faction:
				in_scope = false
				for p in u.planets {
					if int(p.star_system) == scope_id && int(p.faction) == id {
						in_scope = true
						break
					}
				}
			case .Star_System:
			}
		case .Planet:
			in_scope = kind == .Satellite && int(u.satellites[id - 1].planet) == scope_id
		}
		if in_scope && name_matches(pedia_name(u, kind, id), filter) {
			append(&ids, id)
		}
	}
	return
}

// ---- Moving through a long list ----

@(private = "file")
initial :: proc(u: ^Universe, kind: Pedia_Kind, id: int) -> u8 {
	name := pedia_name(u, kind, id)
	if len(name) == 0 {
		return 0
	}
	c := name[0]
	return c - ('a' - 'A') if c >= 'a' && c <= 'z' else c
}

// Left/Right: the start of the previous / next run of names with the same first letter. Wraps around.
pedia_jump :: proc(u: ^Universe, kind: Pedia_Kind, ids: []int, cursor: int, forward: bool) -> int {
	if len(ids) == 0 {
		return 0
	}
	cursor := clamp(cursor, 0, len(ids) - 1)
	letter := initial(u, kind, ids[cursor])
	start := cursor // the first entry of the current letter
	for start > 0 && initial(u, kind, ids[start - 1]) == letter {
		start -= 1
	}
	if forward {
		i := cursor
		for i < len(ids) && initial(u, kind, ids[i]) == letter {
			i += 1
		}
		return i if i < len(ids) else 0
	}
	if start < cursor {
		return start // back to the top of this letter first
	}
	if start == 0 {
		// wrap to the start of the last letter
		last := len(ids) - 1
		last_letter := initial(u, kind, ids[last])
		for last > 0 && initial(u, kind, ids[last - 1]) == last_letter {
			last -= 1
		}
		return last
	}
	previous := initial(u, kind, ids[start - 1])
	i := start - 1
	for i > 0 && initial(u, kind, ids[i - 1]) == previous {
		i -= 1
	}
	return i
}
