package game

// The typed game state. Ids are 1-based indices into the universe's arrays so that the zero value
// means "none"; the VB's string-keyed entities, statistics and yokes become plain fields.
//
// Places nest: every map belongs to the actor whose interior it is, and stepping into that actor
// moves you onto the map. Maps are sparse (see PORT_PLAN.md): a map is its dimensions and the actors
// on it, and every other cell is empty void.

Faction_Id :: distinct int
Star_System_Id :: distinct int
Planet_Id :: distinct int
Satellite_Id :: distinct int
Map_Id :: distinct int
Actor_Id :: distinct int

// Fixed-size, copyable text so names need no allocation and survive being saved.
NAME_CAPACITY :: 32

Name :: struct {
	buf: [NAME_CAPACITY]u8,
	len: u8,
}

// Truncates to NAME_CAPACITY bytes.
name_make :: proc(s: string) -> (n: Name) {
	n.len = u8(min(len(s), NAME_CAPACITY))
	copy(n.buf[:], s[:n.len])
	return
}

// Concatenates the parts, truncating to NAME_CAPACITY bytes.
name_join :: proc(parts: ..string) -> (n: Name) {
	for part in parts {
		room := NAME_CAPACITY - int(n.len)
		take := min(len(part), room)
		copy(n.buf[n.len:], part[:take])
		n.len += u8(take)
	}
	return
}

// The result points into `n`; don't keep it past a copy or move of the Name.
name_str :: proc(n: ^Name) -> string {
	return string(n.buf[:n.len])
}

// ---- Groups of things ----

Faction :: struct {
	name:            Name,
	authority:       int, // 0 chaotic .. 100 lawful
	standards:       int, // 0 good .. 100 evil
	conviction:      int, // fanaticism
	minimum_planets: int, // planets guaranteed when the galaxy is divided up
	planet_count:    int,
	values:          Group_Values,
	reputation:      int, // the avatar's standing with this faction
}

Star_System :: struct {
	name:            Name,
	star_type:       Star_Type,
	position:        [2]int, // on the galaxy map
	actor:           Actor_Id, // its marker on the galaxy map
	interior:        Map_Id, // the system map
	planet_count:    int,
	satellite_count: int,
	visit_count:     int,
	wormhole_count:  int,
	scrap:           int, // debris piles still to be salvaged
	reputation:      int, // the avatar's standing in this system
}

Planet :: struct {
	name:            Name,
	type:            Planet_Type,
	tech_level:      int,
	values:          Group_Values,
	star_system:     Star_System_Id,
	faction:         Faction_Id,
	actor:           Actor_Id, // its marker on the system map
	satellite_count: int,
	reputation:      int, // the avatar's standing on this planet
	market:          [Good]Price_State,
}

Satellite :: struct {
	name:        Name,
	type:        Satellite_Type,
	tech_level:  int,
	planet:      Planet_Id,
	star_system: Star_System_Id,
}

// ---- Maps and actors ----

Map_Kind :: enum {
	Galaxy,
	Nexus, // the space between: reached through wormholes, with no way out but them
	Star_System,
	Star_Vicinity,
	Planet_Vicinity,
	Planet_Orbit,
	Satellite_Orbit,
}

// width, height
map_sizes := [Map_Kind][2]int {
	.Galaxy          = {63, 63},
	.Nexus           = {63, 63},
	.Star_System     = {31, 31},
	.Star_Vicinity   = {15, 15},
	.Planet_Vicinity = {15, 15},
	.Planet_Orbit    = {11, 11},
	.Satellite_Orbit = {9, 9},
}

Map :: struct {
	kind:   Map_Kind,
	owner:  Actor_Id, // the actor whose interior this is; none for the galaxy
	actors: [dynamic]Actor_Id,
}

Actor_Kind :: enum {
	Player_Ship,
	Star_System, // on the galaxy map
	Star_Vicinity, // on a system map, at the center
	Star, // on a star vicinity map, at the center
	Planet_Vicinity, // on a system map
	Planet, // on a planet vicinity map, at the center, 3x3
	Planet_Body, // on a planet orbit map, at the center, 5x5
	Star_Dock, // on a planet orbit map, one per planet
	Shipyard, // on a planet orbit map, on about one planet in four
	Trading_Post, // on a planet orbit map, one or two per planet
	Satellite, // on a planet vicinity map
	Satellite_Body, // on a satellite orbit map, at the center, 3x3
	Debris, // on a star system map: a pile of scrap
	Wormhole, // on the nexus and on star system maps; each end's `target` is the other
	Star_Gate, // on a planet orbit map: a way to the avatar's faction's other gates
	Military_Ship, // on the galaxy map: belongs to `faction`, home is `planet`
}

Actor :: struct {
	kind:        Actor_Kind,
	map_id:      Map_Id, // the map it is on
	pos:         [2]int, // center cell
	size:        int, // odd footprint width and height: 1, 3 or 5
	interior:    Map_Id, // where stepping into it leads; none if nowhere
	star_system: Star_System_Id,
	planet:      Planet_Id,
	satellite:   Satellite_Id,
	loot:        int, // debris: how much scrap is in the pile
	offer:       Item_Id, // star dock: the delivery mission it is offering, if any
	target:      Actor_Id, // wormhole: the other end
	faction:     Faction_Id, // military ship: whose it is
	calm_until:  int, // military ship: leaves you alone until this turn
}

actor_covers :: proc(a: Actor, p: [2]int) -> bool {
	half := a.size / 2
	return abs(p.x - a.pos.x) <= half && abs(p.y - a.pos.y) <= half
}

map_in_bounds :: proc(kind: Map_Kind, p: [2]int) -> bool {
	size := map_sizes[kind]
	return p.x >= 0 && p.y >= 0 && p.x < size.x && p.y < size.y
}

// The outermost ring of cells. Reaching it is how you leave a map for the one above.
map_is_edge :: proc(kind: Map_Kind, p: [2]int) -> bool {
	size := map_sizes[kind]
	return map_in_bounds(kind, p) && (p.x == 0 || p.y == 0 || p.x == size.x - 1 || p.y == size.y - 1)
}

// ---- The avatar (the player) ----

Store :: struct {
	current, minimum, maximum: int,
}

// What the avatar ran into on its last move: an actor, or the border of a map (the way out of it).
Map_Edge :: struct {
	map_id: Map_Id,
}
Bump :: union {
	Actor_Id,
	Map_Edge,
}

Direction :: enum {
	North,
	East,
	South,
	West,
}

Avatar :: struct {
	inventory:     [dynamic]Item_Id,
	actor:         Actor_Id,
	equipment:     [Equip_Slot]Item_Id, // what is installed; 0 means empty
	auto_used:     Use_Result `save:"-"`, // set when an oxygen tank was used automatically; the map screen reports it and clears it
	facing:        Direction,
	bumped:        Bump `save:"-"`,
	star_system:   Star_System_Id, // the system the avatar is in; none in the galaxy
	faction:       Faction_Id, // the SIGMO Federation
	home_planet:   Planet_Id,
	jools:         int,
	jools_minimum: int, // bankrupt at or below this
	fuel:          Store,
	oxygen:        Store, // dead when it runs out
	hull:          Store, // what combat wears down; mended at a shipyard
	cargo:         [Good]int, // units of trade goods in the hold
	infamy:        int, // standing with the underworld; see law.odin
}

// A Mark I life support and fuel supply, which is what the ship starts with (250 per Mark).
MARK_I_CAPACITY :: 250

// ---- The universe ----

Universe :: struct {
	rng:          Rng,
	turn:         int,
	patrol_turn:  int, // the last turn the military ships moved for
	market_turn:  int, // the last turn the markets drifted for
	factions:     [dynamic]Faction,
	star_systems: [dynamic]Star_System,
	planets:      [dynamic]Planet,
	satellites:   [dynamic]Satellite,
	maps:         [dynamic]Map,
	actors:       [dynamic]Actor,
	items:        [dynamic]Item,
	pedia:        Pedia_Index `save:"-"`, // derived: rebuilt after loading
	galaxy:       Map_Id,
	nexus:        Map_Id,
	avatar:       Avatar,
}

universe_make :: proc(seed: u64) -> Universe {
	return {rng = rng_make(seed), turn = 1}
}

universe_destroy :: proc(u: ^Universe) {
	for &m in u.maps {
		delete(m.actors)
	}
	delete(u.factions)
	delete(u.star_systems)
	delete(u.planets)
	delete(u.satellites)
	delete(u.maps)
	delete(u.actors)
	delete(u.items)
	delete(u.avatar.inventory)
	pedia_destroy(&u.pedia)
	u^ = {}
}

@(private = "file")
pool_add :: proc(pool: ^[dynamic]$T, item: T, $Id: typeid) -> Id {
	append(pool, item)
	return Id(len(pool))
}

@(private = "file")
pool_get :: proc(pool: [dynamic]$T, id: $Id) -> ^T {
	assert(id > 0 && int(id) <= len(pool), "bad id")
	return &pool[int(id) - 1]
}

// The result points into the universe's storage; it is invalidated by adding more of the same kind.
faction_get :: proc(u: ^Universe, id: Faction_Id) -> ^Faction {return pool_get(u.factions, id)}
star_system_get :: proc(u: ^Universe, id: Star_System_Id) -> ^Star_System {return pool_get(u.star_systems, id)}
planet_get :: proc(u: ^Universe, id: Planet_Id) -> ^Planet {return pool_get(u.planets, id)}
satellite_get :: proc(u: ^Universe, id: Satellite_Id) -> ^Satellite {return pool_get(u.satellites, id)}
map_get :: proc(u: ^Universe, id: Map_Id) -> ^Map {return pool_get(u.maps, id)}
actor_get :: proc(u: ^Universe, id: Actor_Id) -> ^Actor {return pool_get(u.actors, id)}
item_get :: proc(u: ^Universe, id: Item_Id) -> ^Item {return pool_get(u.items, id)}

item_add :: proc(u: ^Universe, item: Item) -> Item_Id {return pool_add(&u.items, item, Item_Id)}
faction_add :: proc(u: ^Universe, f: Faction) -> Faction_Id {return pool_add(&u.factions, f, Faction_Id)}
star_system_add :: proc(u: ^Universe, s: Star_System) -> Star_System_Id {return pool_add(&u.star_systems, s, Star_System_Id)}
planet_add :: proc(u: ^Universe, p: Planet) -> Planet_Id {return pool_add(&u.planets, p, Planet_Id)}
satellite_add :: proc(u: ^Universe, s: Satellite) -> Satellite_Id {return pool_add(&u.satellites, s, Satellite_Id)}

map_add :: proc(u: ^Universe, kind: Map_Kind, owner: Actor_Id = 0) -> Map_Id {
	return pool_add(&u.maps, Map{kind = kind, owner = owner}, Map_Id)
}

// Creates `a` on map `on` and links it in. `a.map_id` is set here; the caller fills in the rest.
actor_add :: proc(u: ^Universe, on: Map_Id, a: Actor) -> Actor_Id {
	a := a
	a.map_id = on
	if a.size == 0 {
		a.size = 1
	}
	id := pool_add(&u.actors, a, Actor_Id)
	append(&map_get(u, on).actors, id)
	return id
}

// Takes an actor off its map (debris being salvaged). The id stays valid but the actor is on no map.
actor_remove :: proc(u: ^Universe, id: Actor_Id) {
	a := actor_get(u, id)
	if a.map_id == 0 {
		return
	}
	on := map_get(u, a.map_id)
	for other, i in on.actors {
		if other == id {
			ordered_remove(&on.actors, i)
			break
		}
	}
	a.map_id = 0
}

// The actor covering cell `p` of map `on`, or none (0).
actor_at :: proc(u: ^Universe, on: Map_Id, p: [2]int) -> Actor_Id {
	for id in map_get(u, on).actors {
		if actor_covers(actor_get(u, id)^, p) {
			return id
		}
	}
	return 0
}

// A cell is free if it is on the map and nothing covers it.
cell_is_free :: proc(u: ^Universe, on: Map_Id, p: [2]int) -> bool {
	return map_in_bounds(map_get(u, on).kind, p) && actor_at(u, on, p) == 0
}

avatar_is_dead :: proc(u: ^Universe) -> bool {
	return u.avatar.oxygen.current <= u.avatar.oxygen.minimum
}

avatar_is_bankrupt :: proc(u: ^Universe) -> bool {
	return u.avatar.jools <= u.avatar.jools_minimum
}

avatar_is_game_over :: proc(u: ^Universe) -> bool {
	return avatar_is_dead(u) || avatar_is_bankrupt(u)
}
