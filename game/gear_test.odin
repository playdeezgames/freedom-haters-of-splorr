#+build !js
package game

import "core:testing"

first_yard_of :: proc(u: ^Universe) -> Actor_Id {
	return first_of_kind_on(u, .Shipyard)
}

@(test)
the_ship_starts_with_a_full_base_hull :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	testing.expect_value(t, u.avatar.hull, Store{current = BASE_HULL, maximum = BASE_HULL})
}

@(test)
plating_raises_the_maximum_without_mending :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.hull.current = 60
	plating := in_hold(&u, .Armour_Plating, 2)
	testing.expect(t, equip_item(&u, .Armour, plating))
	testing.expect_value(t, u.avatar.hull.maximum, BASE_HULL + armour_hull(2))
	testing.expect_value(t, u.avatar.hull.current, 60)
	u.avatar.hull.current = BASE_HULL + armour_hull(2)
	unequip_item(&u, .Armour)
	testing.expect_value(t, u.avatar.hull.maximum, BASE_HULL)
	testing.expect_value(t, u.avatar.hull.current, BASE_HULL) // cut down to fit
}

@(test)
each_slot_takes_only_its_own_gear :: proc(t: ^testing.T) {
	for kind in Item_Kind {
		fits_somewhere := 0
		for slot in Equip_Slot {
			if slot_accepts(slot, kind) {
				fits_somewhere += 1
			}
		}
		installable := kind == .Fuel_Scoop || kind == .Atmospheric_Concentrator || kind == .Fuel_Supply || kind == .Life_Support || kind == .Pulse_Laser || kind == .Deflector_Shield || kind == .Armour_Plating
		testing.expect_value(t, fits_somewhere > 0, installable)
	}
	testing.expect(t, slot_accepts(.Weapon, .Pulse_Laser) && !slot_accepts(.Weapon, .Deflector_Shield))
	testing.expect(t, slot_accepts(.Shield, .Deflector_Shield) && !slot_accepts(.Accessory_0, .Pulse_Laser))
	testing.expect(t, slot_accepts(.Armour, .Armour_Plating) && !slot_accepts(.Armour, .Pulse_Laser))
}

@(test)
combat_gear_numbers_grow_with_the_mark :: proc(t: ^testing.T) {
	for mark in 1 ..< MAX_MARK {
		testing.expect(t, weapon_damage(mark + 1) > weapon_damage(mark))
		testing.expect(t, shield_capacity(mark + 1) > shield_capacity(mark))
		testing.expect(t, armour_hull(mark + 1) > armour_hull(mark))
	}
	stats := item_stats(item_new(.Pulse_Laser, 2))
	testing.expect_value(t, long_str(&stats.lines[0]), "Tech Level: 4")
	testing.expect_value(t, long_str(&stats.lines[1]), "Damage: 20")
}

@(test)
a_shipyard_repairs_the_hull_for_jools :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	result, mended, cost := shipyard_repair(&u)
	testing.expect_value(t, result, Repair_Result.Nothing_To_Repair)
	u.avatar.hull.current = 40
	u.avatar.jools = 1000
	result, mended, cost = shipyard_repair(&u)
	testing.expect_value(t, result, Repair_Result.Repaired)
	testing.expect_value(t, mended, 60)
	testing.expect_value(t, cost, 30)
	testing.expect_value(t, u.avatar.jools, 970)
	testing.expect_value(t, u.avatar.hull.current, BASE_HULL)
	u.avatar.hull.current = 40
	u.avatar.jools = 10
	result, _, _ = shipyard_repair(&u)
	testing.expect_value(t, result, Repair_Result.Insufficient_Funds)
	testing.expect_value(t, u.avatar.hull.current, 40)
}

@(test)
repairing_through_the_shipyard_screen :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	yard := first_yard_of(u)
	u.avatar.hull.current = 50
	u.avatar.jools = 500
	app.stack.items[app.stack.count] = Shipyard_Screen{yard = yard, cursor = len(Equip_Slot)}
	app.stack.count += 1
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.hull.current, BASE_HULL)
	testing.expect_value(t, u.avatar.jools, 475)
}

@(test)
a_saved_game_keeps_its_hull_and_gear :: proc(t: ^testing.T) {
	u := generate(3)
	defer universe_destroy(&u)
	equip_item(&u, .Armour, in_hold(&u, .Armour_Plating, 3))
	equip_item(&u, .Weapon, in_hold(&u, .Pulse_Laser, 2))
	u.avatar.hull.current = 77
	data := universe_to_bytes(&u)
	defer delete(data)
	loaded, err := universe_from_bytes(data)
	testing.expect_value(t, err, Load_Error.None)
	defer universe_destroy(&loaded)
	testing.expect_value(t, loaded.avatar.hull, u.avatar.hull)
	testing.expect_value(t, item_get(&loaded, loaded.avatar.equipment[.Weapon]).kind, Item_Kind.Pulse_Laser)
}
