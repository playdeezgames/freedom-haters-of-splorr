package game

// Military ships on the galaxy map. Each turn the avatar takes, every ship takes one step: a ship that is
// hostile and has you in sight steps toward you, any other wanders, drifting back toward its home system if it
// strays. A hostile ship that ends its step next to you makes contact; a friendlier one just hails you.
// Nothing here knows about screens. See PORT_PLAN.md ("Decisions (patrols and combat)").

SIGHT_RANGE :: 6 // cells
LEASH :: 12 // how far a ship wanders from its home system before turning back
CALM_AFTER_FINE :: 100 // turns a ship leaves you alone once it has had its way
CALM_AFTER_HAIL :: 30
RESPAWN_EVERY :: 100 // turns between replacements for ships that have been destroyed
RESPAWN_DISTANCE :: 10 // new ships appear at least this far from the avatar

Disposition :: enum {
	Friendly,
	Neutral,
	Hostile,
}

disposition_names := [Disposition]string {
	.Friendly = "Friendly",
	.Neutral  = "Neutral",
	.Hostile  = "Hostile",
}

disposition_hues := [Disposition]Hue {
	.Friendly = .Light_Green,
	.Neutral  = .Yellow,
	.Hostile  = .Light_Red,
}

// The faction's relation to yours, moved a step by how the avatar stands with it: good standing (25+) calms a
// ship a step, great standing (75+) another, and bad standing (-25 or less) angers it a step.
ship_disposition :: proc(u: ^Universe, ship: Actor) -> Disposition {
	mine := faction_get(u, u.avatar.faction)
	theirs := faction_get(u, ship.faction)
	score := int(relation_between(theirs^, mine^)) // Friendly 0, Neutral 1, Hostile 2
	standing := theirs.reputation
	if standing >= 25 {
		score -= 1
	}
	if standing >= 75 {
		score -= 1
	}
	if standing <= -25 {
		score += 1
	}
	if u.avatar.infamy >= INFAMY_WARY {
		score += 1
	}
	if u.avatar.infamy >= INFAMY_WANTED {
		score += 1
	}
	return Disposition(clamp(score, 0, 2))
}

@(private = "file")
squared :: proc(a, b: [2]int) -> int {
	d := a - b
	return d.x * d.x + d.y * d.y
}

@(private = "file")
sign :: proc(n: int) -> int {
	return -1 if n < 0 else 1 if n > 0 else 0
}

// The cell a ship would like to go to, nearer to `goal`, or its own cell if boxed in.
@(private = "file")
step_toward :: proc(u: ^Universe, ship: Actor, goal: [2]int) -> [2]int {
	d := goal - ship.pos
	first, second: [2]int
	if abs(d.x) >= abs(d.y) {
		first, second = {sign(d.x), 0}, {0, sign(d.y)}
	} else {
		first, second = {0, sign(d.y)}, {sign(d.x), 0}
	}
	for step in ([3][2]int{first, second, {sign(d.x), sign(d.y)}}) {
		if step != {0, 0} && cell_is_free(u, u.galaxy, ship.pos + step) {
			return ship.pos + step
		}
	}
	return ship.pos
}

@(private = "file")
wander :: proc(u: ^Universe, ship: Actor) -> [2]int {
	home := star_system_get(u, planet_get(u, ship.planet).star_system).position
	if squared(ship.pos, home) > LEASH * LEASH {
		return step_toward(u, ship, home)
	}
	if rng_below(&u.rng, 3) == 0 {
		return ship.pos // loiter
	}
	step := direction_delta[rng_enum(&u.rng, Direction)]
	if cell_is_free(u, u.galaxy, ship.pos + step) {
		return ship.pos + step
	}
	return ship.pos
}

// Whether the ship is after the avatar this turn.
ship_is_chasing :: proc(u: ^Universe, ship: Actor) -> bool {
	avatar := actor_get(u, u.avatar.actor)
	return avatar.map_id == u.galaxy && u.turn >= ship.calm_until && ship_disposition(u, ship) == .Hostile && squared(ship.pos, avatar.pos) <= SIGHT_RANGE * SIGHT_RANGE
}

// A new ship for a random planet's faction, somewhere open and out of the avatar's sight.
@(private = "file")
respawn_ship :: proc(u: ^Universe) {
	size := map_sizes[.Galaxy]
	avatar := actor_get(u, u.avatar.actor)^
	home := Planet_Id(rng_range(&u.rng, 1, len(u.planets)))
	for _ in 0 ..< MAX_PLACEMENT_TRIES {
		pos := [2]int{rng_range(&u.rng, 0, size.x - 1), rng_range(&u.rng, 0, size.y - 1)}
		if avatar.map_id == u.galaxy && squared(pos, avatar.pos) < RESPAWN_DISTANCE * RESPAWN_DISTANCE {
			continue
		}
		if cell_is_free(u, u.galaxy, pos) {
			actor_add(u, u.galaxy, {kind = .Military_Ship, pos = pos, planet = home, faction = planet_get(u, home).faction})
			return
		}
	}
}

// Moves every military ship one step, and now and then replaces a lost one.
patrol_step :: proc(u: ^Universe) {
	if u.turn % RESPAWN_EVERY == 0 {
		ships := 0
		for a in map_get(u, u.galaxy).actors {
			if actor_get(u, a).kind == .Military_Ship {
				ships += 1
			}
		}
		if ships < fleet_size(len(u.star_systems)) {
			respawn_ship(u)
		}
	}
	avatar := actor_get(u, u.avatar.actor)^
	for id in map_get(u, u.galaxy).actors {
		ship := actor_get(u, id)
		if ship.kind != .Military_Ship {
			continue
		}
		next: [2]int
		if ship_is_chasing(u, ship^) {
			next = step_toward(u, ship^, avatar.pos)
		} else {
			next = wander(u, ship^)
		}
		ship.pos = next
	}
}

// Brings the ships up to date with the avatar's turn counter (a move is one turn; so is going into orbit).
patrol_catch_up :: proc(u: ^Universe) {
	// a loaded or odd counter should not make the ships run a long time at once
	u.patrol_turn = max(u.patrol_turn, u.turn - 20)
	for u.patrol_turn < u.turn {
		u.patrol_turn += 1
		patrol_step(u)
	}
}

Contact :: enum {
	None,
	Hail, // a ship that means no harm says something
	Search, // a ship that is not friendly found contraband aboard
	Shakedown, // a hostile ship has caught you
	Nothing_To_Take, // a hostile ship has caught you and you have nothing it wants
}

// Whether a ship next to the avatar (diagonals count) wants a word, and which ship.
patrol_contact :: proc(u: ^Universe) -> (ship: Actor_Id, kind: Contact) {
	avatar := actor_get(u, u.avatar.actor)^
	if avatar.map_id != u.galaxy {
		return
	}
	for id in map_get(u, u.galaxy).actors {
		other := actor_get(u, id)^
		if other.kind != .Military_Ship || u.turn < other.calm_until {
			continue
		}
		d := other.pos - avatar.pos
		if max(abs(d.x), abs(d.y)) > 1 {
			continue
		}
		disposition := ship_disposition(u, other)
		if disposition == .Friendly {
			return id, .Hail
		}
		if units, _ := contraband_units(u, other.faction); units > 0 {
			return id, .Search
		}
		if disposition != .Hostile {
			return id, .Hail
		}
		if fine_amount(u) == 0 && takeable_count(u) == 0 {
			return id, .Nothing_To_Take
		}
		return id, .Shakedown
	}
	return
}

// ---- What a shakedown takes ----

// A tenth of the jools, at least 10, and nothing if paying would leave the avatar bankrupt.
fine_amount :: proc(u: ^Universe) -> int {
	fine := max(10, u.avatar.jools / 10)
	if u.avatar.jools - fine <= u.avatar.jools_minimum {
		return 0
	}
	return fine
}

// Everything in the hold that is not a delivery (those are not theirs to take).
takeable_count :: proc(u: ^Universe) -> (n: int) {
	for id in u.avatar.inventory {
		if !item_is_kept(item_get(u, id).kind) {
			n += 1
		}
	}
	return
}

// Half the cargo, rounded up.
cargo_demanded :: proc(u: ^Universe) -> int {
	return (takeable_count(u) + 1) / 2
}

ship_calm :: proc(u: ^Universe, ship: Actor_Id, turns: int) {
	actor_get(u, ship).calm_until = u.turn + turns
}

avatar_pay_fine :: proc(u: ^Universe, ship: Actor_Id) -> int {
	fine := fine_amount(u)
	u.avatar.jools -= fine
	ship_calm(u, ship, CALM_AFTER_FINE)
	return fine
}

// Takes `cargo_demanded` random items that are not deliveries. Returns how many went.
avatar_hand_over_cargo :: proc(u: ^Universe, ship: Actor_Id) -> (taken: int) {
	taken = cargo_demanded(u)
	for _ in 0 ..< taken {
		for {
			i := rng_below(&u.rng, len(u.avatar.inventory))
			if !item_is_kept(item_get(u, u.avatar.inventory[i]).kind) {
				ordered_remove(&u.avatar.inventory, i)
				break
			}
		}
	}
	ship_calm(u, ship, CALM_AFTER_FINE)
	return
}
