package game

// Builds a universe a step at a time so the browser can draw progress between steps. This follows the
// VB Initializer: a queue of steps where a step may enqueue more, and a second "final" queue that only
// runs when the first is empty. Factions -> galaxy -> (every star system, then every planet)
// -> divide the planets among factions -> the player.
//
// Placement uses rejection sampling, as in the VB: try random cells until one is far enough from
// everything already placed, and stop after MAX_PLACEMENT_TRIES misses in a row.

MAX_PLACEMENT_TRIES :: 5000

Step_Factions :: struct {}
Step_Galaxy :: struct {}
Step_Star_System :: struct {
	id: Star_System_Id,
}
Step_Planet :: struct {
	id: Planet_Id,
}
Step_Factionize :: struct {}
Step_Avatar :: struct {}

Gen_Step :: union {
	Step_Factions,
	Step_Galaxy,
	Step_Star_System,
	Step_Planet,
	Step_Factionize,
	Step_Avatar,
}

Generator :: struct {
	universe:   Universe,
	settings:   Embark_Settings,
	names:      Name_Generator,
	steps:      [dynamic]Gen_Step,
	final:      [dynamic]Gen_Step,
	steps_next: int,
	final_next: int,
	steps_done: int,
}

generator_start :: proc(seed: u64, settings: Embark_Settings) -> (g: Generator) {
	g.universe = universe_make(seed)
	g.settings = settings
	append(&g.final, Step_Factions{}, Step_Galaxy{}, Step_Factionize{})
	return
}

// Hands over the finished universe and frees everything else.
generator_finish :: proc(g: ^Generator) -> Universe {
	u := g.universe
	g.universe = {}
	generator_destroy(g)
	return u
}

generator_destroy :: proc(g: ^Generator) {
	universe_destroy(&g.universe)
	name_generator_destroy(&g.names)
	delete(g.steps)
	delete(g.final)
	g^ = {}
}

generator_steps_remaining :: proc(g: ^Generator) -> int {
	return (len(g.steps) - g.steps_next) + (len(g.final) - g.final_next)
}

generator_done :: proc(g: ^Generator) -> bool {
	return generator_steps_remaining(g) == 0
}

@(private = "file")
generator_peek :: proc(g: ^Generator) -> (step: Gen_Step, ok: bool) {
	if g.steps_next < len(g.steps) {
		return g.steps[g.steps_next], true
	}
	if g.final_next < len(g.final) {
		return g.final[g.final_next], true
	}
	return nil, false
}

// What the next step is about, for a progress line: "Star system" + the system's name.
generator_current :: proc(g: ^Generator) -> (label: string, subject: Name) {
	step, ok := generator_peek(g)
	if !ok {
		return "Done!", {}
	}
	switch s in step {
	case Step_Factions:
		return "Factions", {}
	case Step_Galaxy:
		return "Galaxy", {}
	case Step_Star_System:
		return "Star system", star_system_get(&g.universe, s.id).name
	case Step_Planet:
		return "Planet", planet_get(&g.universe, s.id).name
	case Step_Factionize:
		return "Dividing up the galaxy", {}
	case Step_Avatar:
		return "Yer ship", {}
	}
	return "", {}
}

// Runs one step. Returns false when there was nothing left to run.
generator_step :: proc(g: ^Generator) -> bool {
	step, ok := generator_peek(g)
	if !ok {
		return false
	}
	if g.steps_next < len(g.steps) {
		g.steps_next += 1
	} else {
		g.final_next += 1
	}
	g.steps_done += 1
	switch s in step {
	case Step_Factions:
		step_factions(g)
	case Step_Galaxy:
		step_galaxy(g)
	case Step_Star_System:
		step_star_system(g, s.id)
	case Step_Planet:
		step_planet(g, s.id)
	case Step_Factionize:
		step_factionize(g)
	case Step_Avatar:
		step_avatar(g)
	}
	return true
}

// ---- Placement ----

// A random cell in [lo, hi] at least min_distance (straight line) from every taken cell, if one turns up.
find_spot :: proc(r: ^Rng, taken: [][2]int, lo, hi: [2]int, min_distance: int) -> (pos: [2]int, ok: bool) {
	for _ in 0 ..< MAX_PLACEMENT_TRIES {
		pos = {rng_range(r, lo.x, hi.x), rng_range(r, lo.y, hi.y)}
		far_enough := true
		for t in taken {
			dx, dy := pos.x - t.x, pos.y - t.y
			if dx * dx + dy * dy < min_distance * min_distance {
				far_enough = false
				break
			}
		}
		if far_enough {
			return pos, true
		}
	}
	return {}, false
}

map_center :: proc(kind: Map_Kind) -> [2]int {
	return map_sizes[kind] / 2
}

// ---- Factions ----

faction_first_parts := [?]string{"People's", "Socialist", "Merchant's", "Democratic", "Invincible", "Academics'", "Farmers'", "Philosophers'", "Altruistic", "United"}
faction_second_parts := [?]string{"Republic", "League", "Monarchy", "Alliance", "State", "Principality", "Federation", "Sultanate", "Commonwealth", "Meritocracy"}

SIGMO_FACTION :: Faction_Id(1)

@(private = "file")
step_factions :: proc(g: ^Generator) {
	u := &g.universe
	faction_add(u, {name = name_make("SIGMO Federation"), authority = 100, standards = 100, conviction = 100, minimum_planets = 1})
	made: [MAX_FACTION_COUNT]Name
	for i in 0 ..< g.settings.faction_count {
		name: Name
		for {
			name = name_join(rng_pick(&u.rng, faction_first_parts[:]), " ", rng_pick(&u.rng, faction_second_parts[:]))
			if !contains_name(made[:i], name) {
				break
			}
		}
		made[i] = name
		faction_add(
			u,
			{
				name = name,
				authority = dice_roll(&u.rng, "10d11+-10d1"),
				standards = dice_roll(&u.rng, "10d11+-10d1"),
				conviction = dice_roll(&u.rng, "10d11+-10d1"),
				values = group_values_roll(&u.rng),
			},
		)
	}
}

@(private = "file")
contains_name :: proc(names: []Name, name: Name) -> bool {
	for n in names {
		if n == name {
			return true
		}
	}
	return false
}

// ---- Galaxy ----

@(private = "file")
step_galaxy :: proc(g: ^Generator) {
	u := &g.universe
	u.galaxy = map_add(u, .Galaxy)
	size := map_sizes[.Galaxy]
	spacing := density_spacing[g.settings.density].minimum_distance
	stars: [dynamic][2]int
	defer delete(stars)
	for {
		pos, ok := find_spot(&u.rng, stars[:], {0, 0}, size - 1, spacing)
		if !ok {
			break
		}
		append(&stars, pos)
		add_star_system(g, pos)
	}
	append(&g.final, Step_Avatar{})
}

@(private = "file")
add_star_system :: proc(g: ^Generator, pos: [2]int) {
	u := &g.universe
	star_type := rng_weighted(&u.rng, star_type_weights[g.settings.age])
	id := star_system_add(u, {name = name_unique(&g.names, &u.rng), star_type = star_type, position = pos})
	marker := actor_add(u, u.galaxy, {kind = .Star_System, pos = pos, star_system = id})
	star_system_get(u, id).actor = marker
	append(&g.steps, Step_Star_System{id})
}

// ---- Star systems ----

@(private = "file")
step_star_system :: proc(g: ^Generator, id: Star_System_Id) {
	u := &g.universe
	marker := star_system_get(u, id).actor
	star_type := star_system_get(u, id).star_type

	system_map := map_add(u, .Star_System, marker)
	actor_get(u, marker).interior = system_map
	star_system_get(u, id).interior = system_map
	center := map_center(.Star_System)

	// the star, one map down from the center of the system
	vicinity_marker := actor_add(u, system_map, {kind = .Star_Vicinity, pos = center, star_system = id})
	vicinity_map := map_add(u, .Star_Vicinity, vicinity_marker)
	actor_get(u, vicinity_marker).interior = vicinity_map
	actor_add(u, vicinity_map, {kind = .Star, pos = map_center(.Star_Vicinity), star_system = id})

	// planets, kept away from the star and each other
	size := map_sizes[.Star_System]
	spacing := star_info[star_type].minimum_planet_distance
	max_planets := dice_roll(&u.rng, PLANET_COUNT_DICE)
	taken: [dynamic][2]int
	defer delete(taken)
	append(&taken, center)
	planet_count := 0
	for planet_count < max_planets {
		pos, ok := find_spot(&u.rng, taken[:], {1, 1}, size - 2, spacing)
		if !ok {
			break
		}
		append(&taken, pos)
		add_planet(g, id, system_map, pos)
		planet_count += 1
	}
	star_system_get(u, id).planet_count = planet_count
	add_debris(g, id, system_map)
}

// Piles of scrap drifting about the system, on any open cell inside the border.
@(private = "file")
add_debris :: proc(g: ^Generator, system: Star_System_Id, system_map: Map_Id) {
	u := &g.universe
	size := map_sizes[.Star_System]
	for _ in 0 ..< dice_roll(&u.rng, DEBRIS_COUNT_DICE) {
		for _ in 0 ..< MAX_PLACEMENT_TRIES {
			pos := [2]int{rng_range(&u.rng, 1, size.x - 2), rng_range(&u.rng, 1, size.y - 2)}
			if cell_is_free(u, system_map, pos) {
				actor_add(u, system_map, {kind = .Debris, pos = pos, star_system = system, loot = dice_roll(&u.rng, DEBRIS_LOOT_DICE)})
				star_system_get(u, system).scrap += 1
				break
			}
		}
	}
}

@(private = "file")
add_planet :: proc(g: ^Generator, system: Star_System_Id, system_map: Map_Id, pos: [2]int) {
	u := &g.universe
	type := rng_enum(&u.rng, Planet_Type)
	name := name_unique(&g.names, &u.rng)
	values := group_values_roll(&u.rng)
	tech_level := dice_roll(&u.rng, TECH_LEVEL_DICE)
	id := planet_add(u, {name = name, type = type, tech_level = tech_level, values = values, star_system = system})
	marker := actor_add(u, system_map, {kind = .Planet_Vicinity, pos = pos, star_system = system, planet = id})
	planet_get(u, id).actor = marker
	append(&g.steps, Step_Planet{id})
}

// ---- Planets and their satellites ----

@(private = "file")
step_planet :: proc(g: ^Generator, id: Planet_Id) {
	u := &g.universe
	marker := planet_get(u, id).actor
	system := planet_get(u, id).star_system

	vicinity := map_add(u, .Planet_Vicinity, marker)
	actor_get(u, marker).interior = vicinity
	center := map_center(.Planet_Vicinity)

	// the planet: 3x3 here, 5x5 once you are in orbit around it
	body := actor_add(u, vicinity, {kind = .Planet, pos = center, size = 3, star_system = system, planet = id})
	orbit := map_add(u, .Planet_Orbit, body)
	actor_get(u, body).interior = orbit
	actor_add(u, orbit, {kind = .Planet_Body, pos = map_center(.Planet_Orbit), size = 5, star_system = system, planet = id})
	add_star_dock(g, id, orbit)
	for _ in 0 ..< max(1, dice_roll(&u.rng, TRADING_POST_COUNT_DICE)) {
		add_trading_post(g, id, orbit)
	}

	// satellites, kept away from the planet's 3x3 block and each other
	size := map_sizes[.Planet_Vicinity]
	max_satellites := dice_roll(&u.rng, SATELLITE_COUNT_DICE)
	taken: [dynamic][2]int
	defer delete(taken)
	for dy in -1 ..= 1 {
		for dx in -1 ..= 1 {
			append(&taken, center + {dx, dy})
		}
	}
	satellite_count := 0
	for satellite_count < max_satellites {
		pos, ok := find_spot(&u.rng, taken[:], {1, 1}, size - 2, MINIMUM_SATELLITE_DISTANCE)
		if !ok {
			break
		}
		append(&taken, pos)
		add_satellite(g, id, vicinity, pos)
		satellite_count += 1
	}
	planet_get(u, id).satellite_count = satellite_count
	star_system_get(u, system).satellite_count += satellite_count
}

// A trading post: at least one in every planet's orbit, sometimes two.
@(private = "file")
add_trading_post :: proc(g: ^Generator, planet: Planet_Id, orbit: Map_Id) {
	u := &g.universe
	size := map_sizes[.Planet_Orbit]
	for _ in 0 ..< MAX_PLACEMENT_TRIES {
		pos := [2]int{rng_range(&u.rng, 1, size.x - 2), rng_range(&u.rng, 1, size.y - 2)}
		if cell_is_free(u, orbit, pos) {
			actor_add(u, orbit, {kind = .Trading_Post, pos = pos, star_system = planet_get(u, planet).star_system, planet = planet})
			return
		}
	}
	panic("no room for a trading post")
}

// One Star Dock in every planet's orbit, on any open cell inside the border.
@(private = "file")
add_star_dock :: proc(g: ^Generator, planet: Planet_Id, orbit: Map_Id) {
	u := &g.universe
	size := map_sizes[.Planet_Orbit]
	for _ in 0 ..< MAX_PLACEMENT_TRIES {
		pos := [2]int{rng_range(&u.rng, 1, size.x - 2), rng_range(&u.rng, 1, size.y - 2)}
		if cell_is_free(u, orbit, pos) {
			actor_add(u, orbit, {kind = .Star_Dock, pos = pos, star_system = planet_get(u, planet).star_system, planet = planet})
			return
		}
	}
	panic("no room for a star dock")
}

@(private = "file")
add_satellite :: proc(g: ^Generator, planet: Planet_Id, vicinity: Map_Id, pos: [2]int) {
	u := &g.universe
	system := planet_get(u, planet).star_system
	type := rng_enum(&u.rng, Satellite_Type)
	name := name_unique(&g.names, &u.rng)
	id := satellite_add(u, {name = name, type = type, tech_level = planet_get(u, planet).tech_level, planet = planet, star_system = system})
	marker := actor_add(u, vicinity, {kind = .Satellite, pos = pos, star_system = system, planet = planet, satellite = id})
	orbit := map_add(u, .Satellite_Orbit, marker)
	actor_get(u, marker).interior = orbit
	actor_add(u, orbit, {kind = .Satellite_Body, pos = map_center(.Satellite_Orbit), size = 3, star_system = system, planet = planet, satellite = id})
}

// ---- Dividing up the galaxy ----

@(private = "file")
step_factionize :: proc(g: ^Generator) {
	u := &g.universe
	unclaimed: [dynamic]Planet_Id
	defer delete(unclaimed)
	for _, i in u.planets {
		append(&unclaimed, Planet_Id(i + 1))
	}
	// each faction first takes the planets it is guaranteed
	for _, i in u.factions {
		faction := Faction_Id(i + 1)
		for _ in 0 ..< faction_get(u, faction).minimum_planets {
			if len(unclaimed) == 0 {
				break
			}
			k := rng_below(&u.rng, len(unclaimed))
			claim_planet(u, unclaimed[k], faction)
			unordered_remove(&unclaimed, k)
		}
	}
	// whatever is left goes to a random faction
	for planet in unclaimed {
		claim_planet(u, planet, Faction_Id(rng_below(&u.rng, len(u.factions)) + 1))
	}
}

@(private = "file")
claim_planet :: proc(u: ^Universe, planet: Planet_Id, faction: Faction_Id) {
	planet_get(u, planet).faction = faction
	faction_get(u, faction).planet_count += 1
}

// ---- The player ----

@(private = "file")
step_avatar :: proc(g: ^Generator) {
	u := &g.universe

	// the ship starts on a random empty cell of the galaxy
	size := map_sizes[.Galaxy]
	pos: [2]int
	for _ in 0 ..< MAX_PLACEMENT_TRIES {
		pos = {rng_range(&u.rng, 0, size.x - 1), rng_range(&u.rng, 0, size.y - 1)}
		if cell_is_free(u, u.galaxy, pos) {
			break
		}
	}
	ship := actor_add(u, u.galaxy, {kind = .Player_Ship, pos = pos})

	// home is one of SIGMO's planets
	home: [dynamic]Planet_Id
	defer delete(home)
	for p, i in u.planets {
		if p.faction == SIGMO_FACTION {
			append(&home, Planet_Id(i + 1))
		}
	}
	assert(len(home) > 0, "SIGMO has no planets")

	profile := wealth_profiles[g.settings.wealth]
	u.avatar = {
		actor         = ship,
		faction       = SIGMO_FACTION,
		home_planet   = rng_pick(&u.rng, home[:]),
		jools         = profile.first + profile.step * rng_below(&u.rng, profile.count),
		jools_minimum = profile.wallet_minimum,
		fuel          = {current = MARK_I_CAPACITY, maximum = MARK_I_CAPACITY},
		oxygen        = {current = MARK_I_CAPACITY, maximum = MARK_I_CAPACITY},
	}
}
