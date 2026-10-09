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

EMERGENCY_FUEL_PRICE :: 1 // jools per unit; the VB hardcoded this and flagged it as a TODO

Move_Outcome :: enum {
	Moved,
	No_Fuel, // could not even try
	Out_Of_Map, // tried to leave the galaxy's edge: nothing happens, the turn is spent
	Bumped, // see avatar.bumped
}

avatar_do_turn :: proc(u: ^Universe) {
	u.turn += 1
	u.avatar.oxygen.current = max(u.avatar.oxygen.current - 1, u.avatar.oxygen.minimum)
}

avatar_move :: proc(u: ^Universe, dir: Direction) -> Move_Outcome {
	a := &u.avatar
	a.facing = dir
	a.bumped = nil
	if a.fuel.current <= a.fuel.minimum {
		return .No_Fuel
	}
	avatar_do_turn(u)
	a.fuel.current -= 1

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
}

// What can be done about whatever was bumped, if anything. (Cancel is always possible and is not listed.)
interaction_for :: proc(u: ^Universe, bump: Bump) -> (kind: Interaction, ok: bool) {
	switch b in bump {
	case Actor_Id:
		a := actor_get(u, b)
		if a.interior == 0 {
			return
		}
		#partial switch a.kind {
		case .Star_System, .Star_Vicinity, .Planet_Vicinity:
			return .Approach, true
		case .Planet, .Satellite:
			return .Enter_Orbit, true
		}
	case Map_Edge:
		return .Leave_Area, true
	}
	return
}

interaction_label :: proc(u: ^Universe, kind: Interaction, bump: Bump) -> string {
	switch kind {
	case .Approach:
		return "Approach"
	case .Enter_Orbit:
		return "Enter Orbit"
	case .Leave_Area:
		if edge, ok := bump.(Map_Edge); ok {
			switch map_get(u, edge.map_id).kind {
			case .Galaxy:
				return "Leave Galaxy"
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
@(private = "file")
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
	}
	return .Blocked
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
