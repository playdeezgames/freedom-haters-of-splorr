package game

// Fighting a military ship, in rounds: Fire, Evade, Flee or Surrender. New in the port (the VB had no combat);
// the numbers here are the tuning knobs. Nothing here knows about screens.
//
// A ship's strength comes from its home planet's tech level. Your shield soaks damage first (and starts every
// fight full); the rest wears down the hull. Lose, and you are robbed and towed home; win, and the ship leaves
// debris behind and its faction remembers.

ENEMY_HULL_BASE :: 60
ENEMY_HULL_PER_TECH :: 10
ENEMY_DAMAGE_BASE :: 6
ENEMY_DAMAGE_PER_TECH :: 2
ENEMY_DAMAGE_DICE :: "1d4"
FLEE_FUEL :: 10
FLEE_ODDS_IN :: 2 // escapes one time in this many
UNARMED_DAMAGE :: 0

LOOT_DICE :: "6d6" // scrap in the wreck
KILL_REPUTATION_LOSS :: 5
ENEMY_OF_ENEMY_GAIN :: 1
CALM_AFTER_FLEEING :: 15
CALM_AFTER_DEFEAT :: 200
JOOLS_KEPT_IN_DEFEAT :: 4 // you keep 1/4
SLIVER :: 10 // oxygen and fuel left after being robbed

Combat_Action :: enum {
	Fire,
	Evade,
	Flee,
}

Combat_Outcome :: enum {
	Continues,
	Won,
	Lost,
	Escaped,
}

Combat :: struct {
	ship:         Actor_Id,
	enemy_hull:   int,
	enemy_max:    int,
	enemy_damage: int, // base damage per attack; each attack adds a die
	shield:       int, // what is left of the shield this fight
}

// What happened in a round, for the log.
Round :: struct {
	outcome:  Combat_Outcome,
	dealt:    int, // by you
	taken:    int, // hull you lost
	absorbed: int, // damage the shield soaked
	evaded:   bool,
	fled:     bool, // you tried and it failed, or it worked
}

enemy_tech :: proc(u: ^Universe, ship: Actor) -> int {
	return planet_get(u, ship.planet).tech_level
}

weapon_mark :: proc(u: ^Universe) -> int {
	if id := u.avatar.equipment[.Weapon]; id != 0 {
		return item_get(u, id).mark
	}
	return 0
}

shield_mark :: proc(u: ^Universe) -> int {
	if id := u.avatar.equipment[.Shield]; id != 0 {
		return item_get(u, id).mark
	}
	return 0
}

// Damage per Fire: 0 without a weapon.
avatar_fire_damage :: proc(u: ^Universe) -> int {
	mark := weapon_mark(u)
	return weapon_damage(mark) if mark > 0 else UNARMED_DAMAGE
}

can_flee :: proc(u: ^Universe) -> bool {
	return u.avatar.fuel.current - FLEE_FUEL > u.avatar.fuel.minimum
}

combat_start :: proc(u: ^Universe, ship: Actor_Id) -> Combat {
	tech := enemy_tech(u, actor_get(u, ship)^)
	hull := ENEMY_HULL_BASE + ENEMY_HULL_PER_TECH * tech
	return {
		ship = ship,
		enemy_hull = hull,
		enemy_max = hull,
		enemy_damage = ENEMY_DAMAGE_BASE + ENEMY_DAMAGE_PER_TECH * tech,
		shield = shield_capacity(shield_mark(u)) if shield_mark(u) > 0 else 0,
	}
}

// The enemy's attack: soaked by the shield first, the rest takes the hull. `divisor` 2 is you evading.
@(private = "file")
enemy_attacks :: proc(u: ^Universe, c: ^Combat, r: ^Round, divisor: int) {
	damage := c.enemy_damage + dice_roll(&u.rng, ENEMY_DAMAGE_DICE)
	damage = (damage + divisor - 1) / divisor
	r.absorbed = min(c.shield, damage)
	c.shield -= r.absorbed
	r.taken = damage - r.absorbed
	u.avatar.hull.current = max(u.avatar.hull.current - r.taken, 0)
}

// Plays one round and says how it went. Fire needs a weapon, Flee needs fuel: otherwise nothing happens.
combat_round :: proc(u: ^Universe, c: ^Combat, action: Combat_Action) -> (r: Round) {
	switch action {
	case .Fire:
		r.dealt = avatar_fire_damage(u)
		c.enemy_hull = max(c.enemy_hull - r.dealt, 0)
		if c.enemy_hull == 0 {
			r.outcome = .Won
			return
		}
		enemy_attacks(u, c, &r, 1)
	case .Evade:
		r.evaded = true
		enemy_attacks(u, c, &r, 2)
	case .Flee:
		r.fled = true
		u.avatar.fuel.current -= FLEE_FUEL
		if rng_below(&u.rng, FLEE_ODDS_IN) == 0 {
			r.outcome = .Escaped
			avatar_gain_infamy(u, INFAMY_FLEE)
			quest_note_fight(u)
			ship_calm(u, c.ship, CALM_AFTER_FLEEING)
			return
		}
		enemy_attacks(u, c, &r, 1)
	}
	if u.avatar.hull.current <= 0 {
		r.outcome = .Lost
	}
	return
}

// ---- After the fight ----

Victory :: struct {
	loot:       int,
	reputation: int, // lost with the ship's faction and its home planet
}

// The ship is gone, leaving a pile of scrap where it was. Its faction and home planet think less of you;
// factions that are hostile to it think slightly more.
combat_victory :: proc(u: ^Universe, c: Combat) -> (v: Victory) {
	ship := actor_get(u, c.ship)^
	v.loot = dice_roll(&u.rng, LOOT_DICE)
	v.reputation = KILL_REPUTATION_LOSS
	avatar_gain_infamy(u, INFAMY_KILL)
	quest_note_fight(u)
	actor_remove(u, c.ship)
	actor_add(u, u.galaxy, {kind = .Debris, pos = ship.pos, loot = v.loot})
	theirs := faction_get(u, ship.faction)
	theirs.reputation -= KILL_REPUTATION_LOSS
	planet_get(u, ship.planet).reputation -= KILL_REPUTATION_LOSS
	for &other, i in u.factions {
		if Faction_Id(i + 1) != ship.faction && relation_between(other, theirs^) == .Hostile {
			other.reputation += ENEMY_OF_ENEMY_GAIN
		}
	}
	return
}

Defeat :: struct {
	jools_lost: int,
	items_lost: int,
}

// Robbed and towed home: most jools and cargo gone, tanks nearly empty, the hull a wreck, and the ship
// is set down beside the star dock of the home planet.
combat_defeat :: proc(u: ^Universe, c: Combat) -> (d: Defeat) {
	a := &u.avatar
	kept := max(a.jools / JOOLS_KEPT_IN_DEFEAT, min(a.jools, a.jools_minimum + 1))
	d.jools_lost = a.jools - kept
	a.jools = kept
	d.items_lost = takeable_count(u) * 3 / 4
	for _ in 0 ..< d.items_lost {
		for {
			i := rng_below(&u.rng, len(a.inventory))
			if !item_is_kept(item_get(u, a.inventory[i]).kind) {
				ordered_remove(&a.inventory, i)
				break
			}
		}
	}
	for good in Good {
		a.cargo[good] -= a.cargo[good] * 3 / 4
	}
	a.oxygen.current = min(a.oxygen.current, max(SLIVER, a.oxygen.minimum + 1))
	a.fuel.current = min(a.fuel.current, max(SLIVER, a.fuel.minimum + 1))
	a.hull.current = max(1, a.hull.maximum / 10)
	ship_calm(u, c.ship, CALM_AFTER_DEFEAT)
	for actor, i in u.actors {
		if actor.kind == .Star_Dock && actor.planet == a.home_planet {
			avatar_travel_to(u, Actor_Id(i + 1))
			break
		}
	}
	return
}
