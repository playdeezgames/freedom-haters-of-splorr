package game

// The rules of flying around, ported from the VB (ActorExtensions.Move/DoTurn, the Approach / Enter Orbit /
// Leave interactions, and the emergency refuel). Nothing here knows about screens.
//
// Every attempted move spends a turn, 1 oxygen and 1 fuel, even if it goes nowhere. With no fuel the ship
// cannot move at all. Running into an actor opens that actor's interaction; running into the border of a map
// (other than the galaxy) opens "leave". Going into an actor's interior costs a turn and some oxygen;
// leaving a map is free.

direction_delta := [Direction][2]int {
	.North = {0, -1},
	.East  = {1, 0},
	.South = {0, 1},
	.West  = {-1, 0},
}

EMERGENCY_FUEL_PRICE :: 1 // jools per unit: 3x the dock price, on purpose (the VB hardcoded it with a TODO)

Move_Outcome :: enum {
	Moved,
	No_Fuel, // could not even try
	Out_Of_Map, // tried to leave the galaxy's edge: nothing happens, the turn is spent
	Bumped, // see avatar.bumped
}

avatar_do_turn :: proc(u: ^Universe) {
	u.turn += 1
	u.avatar.oxygen.current = max(u.avatar.oxygen.current - 1, u.avatar.oxygen.minimum)
	avatar_auto_use_tank(u)
}

avatar_move :: proc(u: ^Universe, dir: Direction) -> Move_Outcome {
	a := &u.avatar
	a.facing = dir
	a.bumped = nil
	if a.fuel.current <= a.fuel.minimum {
		return .No_Fuel
	}
	avatar_do_turn(u)
	a.fuel.current = max(a.fuel.current - 1 - cargo_fuel_surcharge(u), a.fuel.minimum)

	ship := actor_get(u, a.actor)
	m := map_get(u, ship.map_id)
	next := ship.pos + direction_delta[dir]
	if !map_in_bounds(m.kind, next) {
		return .Out_Of_Map
	}
	if m.owner != 0 && map_is_edge(m.kind, next) {
		a.bumped = Map_Edge{ship.map_id}
		return .Bumped
	}
	if hit := actor_at(u, ship.map_id, next); hit != 0 {
		a.bumped = hit
		return .Bumped
	}
	ship.pos = next
	return .Moved
}

// ---- Interactions ----

Interaction :: enum {
	Approach, // into a star system, a star's vicinity or a planet's vicinity
	Enter_Orbit, // into a planet's or a satellite's orbit
	Leave_Area, // out of the map you are on
	Refill_Oxygen, // at a star dock, for jools
	Refuel, // at a star dock, for jools
	Gather_Atmosphere, // from a breathable planet, free
	Salvage_Scrap, // from a pile of debris, free
	Trade, // at a trading post
	Enter_Shipyard, // at a shipyard
	Use_Fuel_Scoop, // at a star, with a fuel scoop installed
	Delivery_Mission, // at a star dock that has a delivery on offer
	Complete_Delivery, // at the star dock of a planet you are carrying a delivery to
	Enter_Wormhole, // at either end of a wormhole
	Enter_Star_Gate, // at a star gate
	Underworld_Contact, // at a star dock where the quest has something to do
	Trade_Black, // at a black market
}

MAX_INTERACTIONS :: 6

OXYGEN_PER_JOOL :: 10 // star dock prices, from the VB's refill dialogs
FUEL_PER_JOOL :: 3

// How much a store is short of full.
top_off_amount :: proc(s: Store) -> int {
	return s.maximum - s.current
}

// Whole jools, rounded up.
price_of :: proc(units, per_jool: int) -> int {
	return (units + per_jool - 1) / per_jool
}

oxygen_price :: proc(u: ^Universe) -> int {
	return price_of(top_off_amount(u.avatar.oxygen), OXYGEN_PER_JOOL)
}

fuel_price :: proc(u: ^Universe) -> int {
	return price_of(top_off_amount(u.avatar.fuel), FUEL_PER_JOOL)
}

// What can be done about whatever was bumped. (Cancel is always possible and is not listed.)
interactions_for :: proc(u: ^Universe, bump: Bump) -> (list: [MAX_INTERACTIONS]Interaction, count: int) {
	add :: proc(list: ^[MAX_INTERACTIONS]Interaction, count: ^int, kind: Interaction) {
		list[count^] = kind
		count^ += 1
	}
	switch b in bump {
	case Actor_Id:
		a := actor_get(u, b)
		#partial switch a.kind {
		case .Star_System, .Star_Vicinity, .Planet_Vicinity:
			if a.interior != 0 {
				add(&list, &count, .Approach)
			}
		case .Planet, .Satellite:
			if a.interior != 0 {
				add(&list, &count, .Enter_Orbit)
			}
		case .Debris:
			add(&list, &count, .Salvage_Scrap)
		case .Wormhole:
			if a.target != 0 {
				add(&list, &count, .Enter_Wormhole)
			}
		case .Star_Gate:
			add(&list, &count, .Enter_Star_Gate)
		case .Trading_Post:
			add(&list, &count, .Trade)
		case .Shipyard:
			add(&list, &count, .Enter_Shipyard)
		case .Star:
			if avatar_has_equipped(u, .Fuel_Scoop) && top_off_amount(u.avatar.fuel) > 0 {
				add(&list, &count, .Use_Fuel_Scoop)
			}
		case .Planet_Body:
			if avatar_has_equipped(u, .Atmospheric_Concentrator) && planet_info[planet_get(u, a.planet).type].can_refill_oxygen && top_off_amount(u.avatar.oxygen) > 0 {
				add(&list, &count, .Gather_Atmosphere)
			}
		case .Star_Dock:
			if top_off_amount(u.avatar.oxygen) > 0 {
				add(&list, &count, .Refill_Oxygen)
			}
			if top_off_amount(u.avatar.fuel) > 0 {
				add(&list, &count, .Refuel)
			}
			if a.offer != 0 {
				add(&list, &count, .Delivery_Mission)
			}
			if _, deliverable := deliverable_here(u, b); deliverable > 0 {
				add(&list, &count, .Complete_Delivery)
			}
			if _, offered := underworld_offer(u, b); offered {
				add(&list, &count, .Underworld_Contact)
			}
		case .Black_Market:
			add(&list, &count, .Trade_Black)
		}
	case Map_Edge:
		add(&list, &count, .Leave_Area)
	}
	return
}

// The first thing on offer, if anything.
interaction_for :: proc(u: ^Universe, bump: Bump) -> (kind: Interaction, ok: bool) {
	list, count := interactions_for(u, bump)
	if count > 0 {
		return list[0], true
	}
	return
}

// Menu text for an interaction. Prices change, so the text is built into `buf`.
interaction_label :: proc(u: ^Universe, kind: Interaction, bump: Bump, buf: ^Name) -> string {
	digits: [20]u8
	switch kind {
	case .Approach:
		return "Approach"
	case .Enter_Orbit:
		return "Enter Orbit"
	case .Gather_Atmosphere:
		return "Gather Atmosphere"
	case .Salvage_Scrap:
		return "Salvage Scrap"
	case .Trade:
		return "Trade"
	case .Enter_Shipyard:
		return "Enter Shipyard"
	case .Use_Fuel_Scoop:
		return "Use Fuel Scoop"
	case .Delivery_Mission:
		return "Delivery Mission..."
	case .Complete_Delivery:
		return "Complete Delivery..."
	case .Enter_Wormhole:
		return "Enter Wormhole"
	case .Trade_Black:
		return "Trade In The Shadows"
	case .Underworld_Contact:
		if dock, ok := bump.(Actor_Id); ok {
			label, _ := underworld_offer(u, dock)
			return label
		}
		return "..."
	case .Enter_Star_Gate:
		return "Enter Star Gate..."
	case .Refill_Oxygen:
		buf^ = name_join("Refill Oxygen (", int_text(&digits, oxygen_price(u)), " jools)")
		return name_str(buf)
	case .Refuel:
		buf^ = name_join("Refuel (", int_text(&digits, fuel_price(u)), " jools)")
		return name_str(buf)
	case .Leave_Area:
		if edge, ok := bump.(Map_Edge); ok {
			switch map_get(u, edge.map_id).kind {
			case .Galaxy:
				return "Leave Galaxy"
			case .Nexus:
				return "Leave Nexus"
			case .Star_System:
				return "Leave Star System"
			case .Star_Vicinity:
				return "Leave Star Vicinity"
			case .Planet_Vicinity:
				return "Leave Planet Vicinity"
			case .Planet_Orbit, .Satellite_Orbit:
				return "Leave Orbit"
			}
		}
		return "Leave"
	}
	return ""
}

Interaction_Result :: enum {
	Done,
	Blocked, // nowhere to go
}

// Moves an actor to `pos` on `to`, keeping the map's actor lists straight.
actor_relocate :: proc(u: ^Universe, id: Actor_Id, to: Map_Id, pos: [2]int) {
	a := actor_get(u, id)
	if a.map_id != to {
		from := map_get(u, a.map_id)
		for other, i in from.actors {
			if other == id {
				ordered_remove(&from.actors, i)
				break
			}
		}
		append(&map_get(u, to).actors, id)
		a.map_id = to
	}
	a.pos = pos
}

// A cell the avatar can stand on: on the map, nothing there, and not the border it would leave by.
cell_is_open :: proc(u: ^Universe, on: Map_Id, p: [2]int) -> bool {
	m := map_get(u, on)
	if m.owner != 0 && map_is_edge(m.kind, p) {
		return false
	}
	return cell_is_free(u, on, p)
}

// Just inside the border: where you arrive when you go into a map.
@(private = "file")
open_cells_inside_the_border :: proc(u: ^Universe, on: Map_Id) -> (cells: [dynamic][2]int) {
	size := map_sizes[map_get(u, on).kind]
	for y in 1 ..< size.y - 1 {
		for x in 1 ..< size.x - 1 {
			if min(x, y, size.x - 1 - x, size.y - 1 - y) == 1 && cell_is_open(u, on, {x, y}) {
				append(&cells, [2]int{x, y})
			}
		}
	}
	return
}

// Just outside an actor's footprint, sharing an edge with it: where you appear when you leave a map.
open_cells_beside :: proc(u: ^Universe, owner: Actor_Id) -> (cells: [dynamic][2]int) {
	o := actor_get(u, owner)^
	half := o.size / 2
	add :: proc(u: ^Universe, cells: ^[dynamic][2]int, on: Map_Id, p: [2]int) {
		if cell_is_open(u, on, p) {
			append(cells, p)
		}
	}
	for i in -half ..= half {
		add(u, &cells, o.map_id, o.pos + {i, -half - 1})
		add(u, &cells, o.map_id, o.pos + {i, half + 1})
		add(u, &cells, o.map_id, o.pos + {-half - 1, i})
		add(u, &cells, o.map_id, o.pos + {half + 1, i})
	}
	return
}

avatar_set_star_system :: proc(u: ^Universe, id: Star_System_Id) {
	if id != 0 && id != u.avatar.star_system {
		star_system_get(u, id).visit_count += 1
	}
	u.avatar.star_system = id
}

// Perform the interaction offered by `avatar.bumped`.
avatar_interact :: proc(u: ^Universe, kind: Interaction) -> Interaction_Result {
	defer u.avatar.bumped = nil
	switch kind {
	case .Approach, .Enter_Orbit:
		target := u.avatar.bumped.(Actor_Id) or_else 0
		if target == 0 {
			return .Blocked
		}
		t := actor_get(u, target)^
		spots := open_cells_inside_the_border(u, t.interior)
		defer delete(spots)
		if len(spots) == 0 {
			return .Blocked
		}
		avatar_do_turn(u)
		actor_relocate(u, u.avatar.actor, t.interior, rng_pick(&u.rng, spots[:]))
		if map_get(u, t.interior).kind == .Star_System {
			avatar_set_star_system(u, t.star_system)
		}
		return .Done
	case .Leave_Area:
		edge, ok := u.avatar.bumped.(Map_Edge)
		if !ok {
			return .Blocked
		}
		owner := map_get(u, edge.map_id).owner
		if owner == 0 {
			return .Blocked
		}
		spots := open_cells_beside(u, owner)
		defer delete(spots)
		if len(spots) == 0 {
			return .Blocked
		}
		parent := actor_get(u, owner).map_id
		actor_relocate(u, u.avatar.actor, parent, rng_pick(&u.rng, spots[:]))
		if map_get(u, parent).kind == .Galaxy {
			avatar_set_star_system(u, 0)
		}
		return .Done
	case .Enter_Wormhole:
		wormhole, ok := u.avatar.bumped.(Actor_Id)
		if !ok {
			return .Blocked
		}
		return avatar_travel_to(u, actor_get(u, wormhole).target)
	case .Refill_Oxygen, .Refuel, .Gather_Atmosphere, .Salvage_Scrap, .Trade, .Enter_Shipyard, .Use_Fuel_Scoop, .Delivery_Mission, .Complete_Delivery, .Enter_Star_Gate, .Underworld_Contact, .Trade_Black:
		// these are transactions, not moves: see avatar_buy_oxygen and friends
		return .Blocked
	}
	return .Blocked
}

// ---- Buying air and fuel ----

// Each returns how much was added and what it cost (jools can go below zero: that is bankruptcy's job).
avatar_buy_oxygen :: proc(u: ^Universe) -> (added, cost: int) {
	added = top_off_amount(u.avatar.oxygen)
	cost = oxygen_price(u)
	u.avatar.oxygen.current = u.avatar.oxygen.maximum
	u.avatar.jools -= cost
	return
}

avatar_buy_fuel :: proc(u: ^Universe) -> (added, cost: int) {
	added = top_off_amount(u.avatar.fuel)
	cost = fuel_price(u)
	u.avatar.fuel.current = u.avatar.fuel.maximum
	u.avatar.jools -= cost
	return
}

// Free, and only worth doing next to a breathable planet.
avatar_gather_atmosphere :: proc(u: ^Universe) -> (added: int) {
	added = top_off_amount(u.avatar.oxygen)
	u.avatar.oxygen.current = u.avatar.oxygen.maximum
	return
}

// Takes everything in a pile of debris (free, no turn) and removes the pile. Returns how many scrap items came.
avatar_salvage :: proc(u: ^Universe, debris: Actor_Id) -> (found: int) {
	d := actor_get(u, debris)
	assert(d.kind == .Debris && d.map_id != 0)
	found = d.loot
	for _ in 0 ..< found {
		append(&u.avatar.inventory, item_add(u, item_new(.Scrap)))
	}
	if d.star_system != 0 {
		star_system_get(u, d.star_system).scrap -= 1
	}
	actor_remove(u, debris)
	return
}

// ---- Emergency refuel ----

distress_available :: proc(u: ^Universe) -> bool {
	return u.avatar.fuel.current <= u.avatar.fuel.minimum
}

// Fills the tank and charges for it. Returns what was added and what it cost.
avatar_signal_distress :: proc(u: ^Universe) -> (added, price: int) {
	f := &u.avatar.fuel
	added = f.maximum - f.current
	price = added * EMERGENCY_FUEL_PRICE
	f.current = f.maximum
	u.avatar.jools -= price
	return
}

// ---- Wormholes and star gates ----

// Arrives on a random open cell beside `destination` (a wormhole's far end or a star gate), in whatever system
// that is in. Free: the move that bumped into the way in already cost a turn. Blocked if there is no room.
avatar_travel_to :: proc(u: ^Universe, destination: Actor_Id) -> Interaction_Result {
	if destination == 0 {
		return .Blocked
	}
	spots := open_cells_beside(u, destination)
	defer delete(spots)
	if len(spots) == 0 {
		return .Blocked
	}
	there := actor_get(u, destination)
	actor_relocate(u, u.avatar.actor, there.map_id, rng_pick(&u.rng, spots[:]))
	avatar_set_star_system(u, there.star_system)
	return .Done
}

// The star gates the avatar can go to: its own faction's, other than the one it is standing at. (The live game
// lets you use any gate to reach your own faction's gates, and so does this.)
star_gates_for_avatar :: proc(u: ^Universe, here: Actor_Id) -> (gates: [dynamic]Actor_Id) {
	for a, i in u.actors {
		if a.kind != .Star_Gate || a.map_id == 0 || Actor_Id(i + 1) == here {
			continue
		}
		if planet_get(u, a.planet).faction == u.avatar.faction {
			append(&gates, Actor_Id(i + 1))
		}
	}
	return
}
