#+build !js
package game

import "core:testing"

// A hostile ship beside the avatar with a home planet of the given tech level.
enemy_with_tech :: proc(t: ^testing.T, u: ^Universe, tech: int) -> Actor_Id {
	ship := ship_of(u, false)
	planet_get(u, actor_get(u, ship).planet).tech_level = tech
	bring_alongside(t, u, ship)
	return ship
}

@(test)
a_ships_strength_comes_from_its_home_planet :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	weak := combat_start(&u, enemy_with_tech(t, &u, 0))
	strong := combat_start(&u, enemy_with_tech(t, &u, 10))
	testing.expect_value(t, weak.enemy_hull, ENEMY_HULL_BASE)
	testing.expect_value(t, strong.enemy_hull, ENEMY_HULL_BASE + 10 * ENEMY_HULL_PER_TECH)
	testing.expect(t, strong.enemy_damage > weak.enemy_damage)
	testing.expect_value(t, weak.shield, 0) // no shield installed
}

@(test)
firing_wears_the_enemy_down_and_it_hits_back :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	equip_item(&u, .Weapon, in_hold(&u, .Pulse_Laser, 1), charge = false)
	c := combat_start(&u, enemy_with_tech(t, &u, 0))
	r := combat_round(&u, &c, .Fire)
	testing.expect_value(t, r.dealt, weapon_damage(1))
	testing.expect_value(t, c.enemy_hull, ENEMY_HULL_BASE - weapon_damage(1))
	testing.expect(t, r.taken >= ENEMY_DAMAGE_BASE + 1 && r.taken <= ENEMY_DAMAGE_BASE + 4)
	testing.expect_value(t, u.avatar.hull.current, BASE_HULL - r.taken)
	testing.expect_value(t, r.outcome, Combat_Outcome.Continues)
}

@(test)
the_killing_blow_gets_no_reply :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	equip_item(&u, .Weapon, in_hold(&u, .Pulse_Laser, 1), charge = false)
	c := combat_start(&u, enemy_with_tech(t, &u, 0))
	c.enemy_hull = 5
	r := combat_round(&u, &c, .Fire)
	testing.expect_value(t, r.outcome, Combat_Outcome.Won)
	testing.expect_value(t, r.taken, 0)
	testing.expect_value(t, u.avatar.hull.current, BASE_HULL)
}

@(test)
without_a_weapon_you_can_only_take_it :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	testing.expect_value(t, avatar_fire_damage(&u), 0)
	list, n := combat_choices(&u)
	for i in 0 ..< n {
		testing.expect(t, list[i] != .Fire)
	}
}

@(test)
a_shield_soaks_before_the_hull :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	equip_item(&u, .Shield, in_hold(&u, .Deflector_Shield, 2), charge = false)
	c := combat_start(&u, enemy_with_tech(t, &u, 5))
	testing.expect_value(t, c.shield, shield_capacity(2))
	r := combat_round(&u, &c, .Evade)
	testing.expect(t, r.absorbed > 0)
	testing.expect_value(t, r.taken, 0) // 40 of shield covers a halved hit at tech 5
	testing.expect_value(t, c.shield, shield_capacity(2) - r.absorbed)
	c.shield = 3
	r = combat_round(&u, &c, .Evade)
	testing.expect_value(t, r.absorbed, 3)
	testing.expect(t, r.taken > 0)
	testing.expect_value(t, u.avatar.hull.current, BASE_HULL - r.taken)
}

@(test)
evading_halves_the_damage :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	c := combat_start(&u, enemy_with_tech(t, &u, 4))
	full, half := 0, 0
	for _ in 0 ..< 30 {
		u.avatar.hull.current = BASE_HULL
		c2 := c
		r := combat_round(&u, &c2, .Evade)
		half = max(half, r.taken)
	}
	for _ in 0 ..< 30 {
		u.avatar.hull.current = BASE_HULL
		c2 := c
		u.avatar.fuel.current = u.avatar.fuel.maximum
		equip_item(&u, .Weapon, in_hold(&u, .Pulse_Laser, 1), charge = false)
		r := combat_round(&u, &c2, .Fire)
		full = max(full, r.taken)
		unequip_item(&u, .Weapon)
	}
	testing.expect(t, half <= (c.enemy_damage + 4 + 1) / 2)
	testing.expect(t, full > half)
}

@(test)
fleeing_costs_fuel_and_sometimes_works :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	ship := enemy_with_tech(t, &u, 2)
	escaped, caught := 0, 0
	for _ in 0 ..< 60 {
		u.avatar.hull.current = BASE_HULL
		u.avatar.fuel.current = u.avatar.fuel.maximum
		c := combat_start(&u, ship)
		r := combat_round(&u, &c, .Flee)
		testing.expect_value(t, u.avatar.fuel.current, u.avatar.fuel.maximum - FLEE_FUEL)
		if r.outcome == .Escaped {
			escaped += 1
			testing.expect_value(t, r.taken, 0)
		} else {
			caught += 1
			testing.expect(t, r.taken > 0)
		}
	}
	testing.expect(t, escaped > 0 && caught > 0)
	testing.expect(t, actor_get(&u, ship).calm_until > u.turn)
	u.avatar.fuel.current = FLEE_FUEL
	testing.expect(t, !can_flee(&u))
}

@(test)
winning_leaves_wreckage_and_a_grudge :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	ship := enemy_with_tech(t, &u, 3)
	a := actor_get(&u, ship)^
	theirs := faction_get(&u, a.faction)
	theirs.reputation = 10
	planet_get(&u, a.planet).reputation = 7
	before := make([]int, len(u.factions))
	defer delete(before)
	for f, i in u.factions {
		before[i] = f.reputation
	}
	c := combat_start(&u, ship)
	v := combat_victory(&u, c)
	testing.expect(t, v.loot >= 6 && v.loot <= 36)
	testing.expect_value(t, actor_get(&u, ship).map_id, Map_Id(0)) // gone
	wreck := actor_at(&u, u.galaxy, a.pos)
	testing.expect(t, wreck != 0)
	testing.expect_value(t, actor_get(&u, wreck).kind, Actor_Kind.Debris)
	testing.expect_value(t, actor_get(&u, wreck).loot, v.loot)
	testing.expect_value(t, theirs.reputation, 10 - KILL_REPUTATION_LOSS)
	testing.expect_value(t, planet_get(&u, a.planet).reputation, 7 - KILL_REPUTATION_LOSS)
	for f, i in u.factions {
		id := Faction_Id(i + 1)
		if id == a.faction {
			continue
		}
		hostile := relation_between(f, theirs^) == .Hostile
		testing.expect_value(t, f.reputation, before[i] + (ENEMY_OF_ENEMY_GAIN if hostile else 0))
	}
	// and it can be salvaged like any other pile
	got := avatar_salvage(&u, wreck)
	testing.expect_value(t, got, v.loot)
}

@(test)
losing_robs_you_and_tows_you_home :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	clear(&u.avatar.inventory)
	for _ in 0 ..< 8 {
		in_hold(&u, .Scrap)
	}
	a, b, _ := far_apart(&u)
	delivery := carry(&u, a, b, 40)
	u.avatar.jools = 1000
	ship := enemy_with_tech(t, &u, 3)
	c := combat_start(&u, ship)
	u.avatar.hull.current = 0
	d := combat_defeat(&u, c)
	testing.expect_value(t, u.avatar.jools, 250)
	testing.expect_value(t, d.jools_lost, 750)
	testing.expect_value(t, d.items_lost, 6)
	testing.expect_value(t, len(u.avatar.inventory), 3) // 2 scrap and the delivery
	kept_delivery := false
	for id in u.avatar.inventory {
		kept_delivery ||= id == delivery
	}
	testing.expect(t, kept_delivery)
	testing.expect_value(t, u.avatar.oxygen.current, SLIVER)
	testing.expect_value(t, u.avatar.fuel.current, SLIVER)
	testing.expect_value(t, u.avatar.hull.current, BASE_HULL / 10)
	// beside the home planet's star dock, in that system
	home := planet_get(&u, u.avatar.home_planet)
	me := actor_get(&u, u.avatar.actor)
	testing.expect_value(t, map_get(&u, me.map_id).kind, Map_Kind.Planet_Orbit)
	testing.expect_value(t, u.avatar.star_system, home.star_system)
	dock := Actor_Id(0)
	for id in map_get(&u, me.map_id).actors {
		if actor_get(&u, id).kind == .Star_Dock {
			dock = id
		}
	}
	testing.expect_value(t, actor_get(&u, dock).planet, u.avatar.home_planet)
	delta := me.pos - actor_get(&u, dock).pos
	testing.expect_value(t, abs(delta.x) + abs(delta.y), 1)
	testing.expect(t, actor_get(&u, ship).calm_until >= u.turn + CALM_AFTER_DEFEAT)
}

@(test)
a_poor_loser_keeps_enough_to_stay_solvent :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	u.avatar.jools = u.avatar.jools_minimum + 3
	c := combat_start(&u, enemy_with_tech(t, &u, 1))
	combat_defeat(&u, c)
	testing.expect(t, !avatar_is_bankrupt(&u))
}

@(test)
resisting_opens_combat_and_surrender_goes_back_to_the_shakedown :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.jools = 1000
	ship := enemy_with_tech(t, u, 0)
	u.turn += 1
	app_tick(&app)
	testing.expect(t, on_screen(&app, Contact_Screen))
	// Pay Fine, Hand Over Cargo (only if there is any), Resist: the last
	list, n := contact_choices(u)
	testing.expect_value(t, list[n - 1], Contact_Choice.Resist)
	for _ in 0 ..< n - 1 {
		app_key(&app, KEY_DOWN)
	}
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Combat_Screen))
	app_key(&app, KEY_ESCAPE) // no walking out
	testing.expect(t, on_screen(&app, Combat_Screen))
	// Evade, then Surrender
	app_key(&app, KEY_ENTER) // the first choice is Evade without a weapon
	testing.expect(t, on_screen(&app, Combat_Screen))
	testing.expect(t, u.avatar.hull.current < BASE_HULL)
	cs := stack_top(&app.stack)^.(Combat_Screen)
	_, count := combat_choices(u)
	for _ in 0 ..< count - 1 {
		app_key(&app, KEY_DOWN)
	}
	_ = cs
	app_key(&app, KEY_ENTER) // Surrender
	testing.expect(t, on_screen(&app, Contact_Screen))
	_ = ship
}

@(test)
fighting_to_victory_through_the_screens :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	equip_item(&u^, .Weapon, in_hold(u, .Pulse_Laser, 5), charge = false)
	ship := enemy_with_tech(t, u, 0)
	u.turn += 1
	app_tick(&app)
	testing.expect(t, on_screen(&app, Contact_Screen))
	list, n := contact_choices(u)
	testing.expect_value(t, list[n - 1], Contact_Choice.Resist)
	for _ in 0 ..< n - 1 {
		app_key(&app, KEY_DOWN)
	}
	app_key(&app, KEY_ENTER)
	for round in 0 ..< 10 {
		if !on_screen(&app, Combat_Screen) {
			break
		}
		app_key(&app, KEY_ENTER) // Fire, the first choice with a weapon
		_ = round
	}
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, actor_get(u, ship).map_id, Map_Id(0))
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
destroyed_ships_are_replaced_over_time :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	want := count_actors_on_maps(&u, .Military_Ship)
	actor_remove(&u, ship_of(&u, false))
	testing.expect_value(t, count_actors_on_maps(&u, .Military_Ship), want - 1)
	u.turn = RESPAWN_EVERY - 1
	patrol_step(&u)
	testing.expect_value(t, count_actors_on_maps(&u, .Military_Ship), want - 1) // not yet
	u.turn = RESPAWN_EVERY
	patrol_step(&u)
	testing.expect_value(t, count_actors_on_maps(&u, .Military_Ship), want)
	u.turn = 2 * RESPAWN_EVERY
	patrol_step(&u)
	testing.expect_value(t, count_actors_on_maps(&u, .Military_Ship), want) // never over the fleet size
}
