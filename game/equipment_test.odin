#+build !js
package game

import "core:testing"

first_yard :: proc(u: ^Universe) -> Actor_Id {
	for a, i in u.actors {
		if a.kind == .Shipyard {
			return Actor_Id(i + 1)
		}
	}
	return 0
}

// A shipyard on a planet of the given tech level.
yard_with_tech :: proc(u: ^Universe, tech: int) -> Actor_Id {
	yard := first_yard(u)
	planet_get(u, actor_get(u, yard).planet).tech_level = tech
	return yard
}

// A new unit in the hold.
in_hold :: proc(u: ^Universe, kind: Item_Kind, mark: int = 0) -> Item_Id {
	id := item_add(u, item_new(kind, mark))
	append(&u.avatar.inventory, id)
	return id
}

@(test)
the_ship_starts_with_mark_one_units_installed_and_full :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	life := item_get(&u, u.avatar.equipment[.Life_Support])
	fuel := item_get(&u, u.avatar.equipment[.Fuel_Supply])
	testing.expect_value(t, life.kind, Item_Kind.Life_Support)
	testing.expect_value(t, life.mark, 1)
	testing.expect_value(t, fuel.kind, Item_Kind.Fuel_Supply)
	testing.expect_value(t, fuel.mark, 1)
	testing.expect_value(t, u.avatar.oxygen, Store{current = 250, minimum = 0, maximum = 250})
	testing.expect_value(t, u.avatar.fuel, Store{current = 250, minimum = 0, maximum = 250})
	testing.expect_value(t, u.avatar.equipment[.Accessory_0], Item_Id(0))
	testing.expect_value(t, u.avatar.equipment[.Accessory_1], Item_Id(0))
	testing.expect_value(t, len(u.avatar.inventory), 0)
}

@(test)
slots_take_only_their_own_kind :: proc(t: ^testing.T) {
	testing.expect(t, slot_accepts(.Life_Support, .Life_Support))
	testing.expect(t, !slot_accepts(.Life_Support, .Fuel_Supply))
	testing.expect(t, slot_accepts(.Fuel_Supply, .Fuel_Supply))
	testing.expect(t, !slot_accepts(.Fuel_Supply, .Fuel_Scoop))
	for slot in ([]Equip_Slot{.Accessory_0, .Accessory_1}) {
		testing.expect(t, slot_accepts(slot, .Fuel_Scoop))
		testing.expect(t, slot_accepts(slot, .Atmospheric_Concentrator))
		testing.expect(t, !slot_accepts(slot, .Life_Support))
		testing.expect(t, !slot_accepts(slot, .Scrap))
	}
	testing.expect(t, slot_info[.Life_Support].mandatory && slot_info[.Fuel_Supply].mandatory)
	testing.expect(t, !slot_info[.Accessory_0].mandatory && !slot_info[.Accessory_1].mandatory)
}

@(test)
removing_a_unit_remembers_its_level_and_empties_the_tank :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.oxygen.current = 120
	jools := u.avatar.jools
	old := u.avatar.equipment[.Life_Support]
	testing.expect_value(t, unequip_item(&u, .Life_Support), old)
	testing.expect_value(t, item_get(&u, old).level, 120)
	testing.expect_value(t, u.avatar.oxygen.maximum, 0)
	testing.expect_value(t, u.avatar.oxygen.current, 0)
	testing.expect_value(t, u.avatar.equipment[.Life_Support], Item_Id(0))
	testing.expect_value(t, inventory_count(&u, .Life_Support, 1), 1)
	testing.expect_value(t, u.avatar.jools, jools - 5) // Mark I uninstall fee
	testing.expect_value(t, unequip_item(&u, .Life_Support), Item_Id(0)) // nothing left to remove
}

@(test)
installing_a_unit_fills_the_tank_to_its_own_level_and_charges :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.fuel.current = 40
	old := unequip_item(&u, .Fuel_Supply)
	jools := u.avatar.jools
	testing.expect(t, equip_item(&u, .Fuel_Supply, old))
	testing.expect_value(t, u.avatar.fuel.current, 40) // it remembered
	testing.expect_value(t, u.avatar.fuel.maximum, 250)
	testing.expect_value(t, u.avatar.jools, jools - 10) // Mark I install fee
	testing.expect_value(t, len(u.avatar.inventory), 0) // out of the hold
	testing.expect(t, !equip_item(&u, .Fuel_Supply, in_hold(&u, .Fuel_Supply, 2))) // the slot is taken
	testing.expect(t, !equip_item(&u, .Life_Support, in_hold(&u, .Fuel_Supply, 2))) // and it must fit
}

@(test)
swapping_in_a_new_unit_is_a_refill_and_swapping_back_restores_the_old_level :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	yard := yard_with_tech(&u, 10)
	u.avatar.oxygen.current = 100
	mark_two := in_hold(&u, .Life_Support, 2)
	c := shipyard_change(&u, yard, .Life_Support, mark_two)
	testing.expect_value(t, c.result, Change_Result.Done)
	testing.expect_value(t, u.avatar.oxygen.maximum, 500)
	testing.expect_value(t, u.avatar.oxygen.current, 500) // the new unit was full
	old := c.removed
	testing.expect_value(t, item_get(&u, old).level, 100) // the old one kept what it had

	c = shipyard_change(&u, yard, .Life_Support, old)
	testing.expect_value(t, c.result, Change_Result.Done)
	testing.expect_value(t, u.avatar.oxygen.maximum, 250)
	testing.expect_value(t, u.avatar.oxygen.current, 100)
	testing.expect_value(t, item_get(&u, mark_two).level, 500)
}

@(test)
a_change_costs_the_old_uninstall_fee_plus_the_new_install_fee :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	yard := yard_with_tech(&u, 10)
	mark_three := in_hold(&u, .Fuel_Supply, 3)
	jools := u.avatar.jools
	testing.expect_value(t, change_fee(&u, .Fuel_Supply, mark_three), 5 + 30) // Mark I out, Mark III in
	c := shipyard_change(&u, yard, .Fuel_Supply, mark_three)
	testing.expect_value(t, c.fee, 35)
	testing.expect_value(t, u.avatar.jools, jools - 35)
	testing.expect_value(t, c.installed, mark_three)
}

@(test)
accessories_can_be_installed_and_uninstalled :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	yard := yard_with_tech(&u, 10)
	conc := in_hold(&u, .Atmospheric_Concentrator)
	jools := u.avatar.jools
	c := shipyard_change(&u, yard, .Accessory_1, conc)
	testing.expect_value(t, c.result, Change_Result.Done)
	testing.expect_value(t, c.removed, Item_Id(0))
	testing.expect_value(t, u.avatar.equipment[.Accessory_1], conc)
	testing.expect_value(t, u.avatar.jools, jools - 25)
	testing.expect(t, avatar_has_equipped(&u, .Atmospheric_Concentrator))

	c = shipyard_change(&u, yard, .Accessory_1, 0)
	testing.expect_value(t, c.result, Change_Result.Done)
	testing.expect_value(t, c.removed, conc)
	testing.expect_value(t, u.avatar.jools, jools - 25 - 15)
	testing.expect(t, !avatar_has_equipped(&u, .Atmospheric_Concentrator))
	testing.expect_value(t, inventory_count(&u, .Atmospheric_Concentrator), 1) // back in the hold
}

@(test)
a_low_tech_yard_cannot_install_advanced_gear :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	yard := yard_with_tech(&u, 2)
	mark_three := in_hold(&u, .Fuel_Supply, 3) // needs 3
	jools := u.avatar.jools
	c := shipyard_change(&u, yard, .Fuel_Supply, mark_three)
	testing.expect_value(t, c.result, Change_Result.Insufficient_Tech)
	testing.expect_value(t, u.avatar.jools, jools) // nothing happened
	testing.expect_value(t, item_get(&u, u.avatar.equipment[.Fuel_Supply]).mark, 1)
	testing.expect_value(t, inventory_count(&u, .Fuel_Supply, 3), 1)
}

@(test)
a_low_tech_yard_cannot_even_remove_advanced_gear :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	rich := yard_with_tech(&u, 10)
	scoop := in_hold(&u, .Fuel_Scoop) // tech level 7
	testing.expect_value(t, shipyard_change(&u, rich, .Accessory_0, scoop).result, Change_Result.Done)
	poor := yard_with_tech(&u, 3)
	jools := u.avatar.jools
	c := shipyard_change(&u, poor, .Accessory_0, 0)
	testing.expect_value(t, c.result, Change_Result.Insufficient_Tech)
	testing.expect_value(t, u.avatar.equipment[.Accessory_0], scoop)
	testing.expect_value(t, u.avatar.jools, jools)
}

@(test)
you_must_afford_the_whole_fee_first :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	yard := yard_with_tech(&u, 10)
	mark_two := in_hold(&u, .Fuel_Supply, 2) // 5 out + 20 in = 25
	u.avatar.jools = 24
	testing.expect_value(t, shipyard_change(&u, yard, .Fuel_Supply, mark_two).result, Change_Result.Insufficient_Funds)
	testing.expect_value(t, u.avatar.jools, 24)
	testing.expect_value(t, item_get(&u, u.avatar.equipment[.Fuel_Supply]).mark, 1)
	u.avatar.jools = 25
	testing.expect_value(t, shipyard_change(&u, yard, .Fuel_Supply, mark_two).result, Change_Result.Done)
	testing.expect_value(t, u.avatar.jools, 0)
}

@(test)
mandatory_slots_cannot_be_emptied_and_wrong_items_are_refused :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	yard := yard_with_tech(&u, 10)
	testing.expect_value(t, shipyard_change(&u, yard, .Life_Support, 0).result, Change_Result.Not_Allowed)
	testing.expect_value(t, shipyard_change(&u, yard, .Fuel_Supply, 0).result, Change_Result.Not_Allowed)
	testing.expect_value(t, shipyard_change(&u, yard, .Accessory_0, 0).result, Change_Result.Not_Allowed) // already empty
	testing.expect_value(t, shipyard_change(&u, yard, .Fuel_Supply, in_hold(&u, .Life_Support, 2)).result, Change_Result.Not_Allowed)
	not_mine := item_add(&u, item_new(.Fuel_Supply, 2)) // exists, but is not in the hold
	testing.expect_value(t, shipyard_change(&u, yard, .Fuel_Supply, not_mine).result, Change_Result.Not_Allowed)
	testing.expect(t, u.avatar.oxygen.maximum == 250 && u.avatar.fuel.maximum == 250)
}

@(test)
installable_items_are_the_ones_in_the_hold_that_fit :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	a := in_hold(&u, .Fuel_Supply, 2)
	in_hold(&u, .Life_Support, 2)
	b := in_hold(&u, .Fuel_Supply, 2) // the same kind twice: two entries, they differ in level
	in_hold(&u, .Scrap)
	list := installable_items(&u, .Fuel_Supply)
	testing.expect_value(t, list.count, 2)
	testing.expect(t, list.items[0] == a && list.items[1] == b)
	testing.expect_value(t, installable_items(&u, .Accessory_0).count, 0)
	in_hold(&u, .Fuel_Scoop)
	testing.expect_value(t, installable_items(&u, .Accessory_1).count, 1)
}

@(test)
the_fuel_scoop_needs_installing_and_a_star :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	star: Actor_Id
	for a, i in u.actors {
		if a.kind == .Star {
			star = Actor_Id(i + 1)
			break
		}
	}
	u.avatar.fuel.current = 100
	_, n := offered(&u, star)
	testing.expect_value(t, n, 0) // no scoop
	equip_item(&u, .Accessory_0, item_add(&u, item_new(.Fuel_Scoop)), charge = false)
	list, n2 := offered(&u, star)
	testing.expect_value(t, n2, 1)
	testing.expect_value(t, list[0], Interaction.Use_Fuel_Scoop)
	jools := u.avatar.jools
	testing.expect_value(t, avatar_use_fuel_scoop(&u), 150)
	testing.expect_value(t, u.avatar.fuel.current, 250)
	testing.expect_value(t, u.avatar.jools, jools) // free
	_, n3 := offered(&u, star)
	testing.expect_value(t, n3, 0) // full
}

@(test)
about_one_planet_in_four_has_a_shipyard :: proc(t: ^testing.T) {
	u := generate(2)
	defer universe_destroy(&u)
	yards := count_actors(&u, .Shipyard)
	testing.expect(t, yards > len(u.planets) / 10 && yards < len(u.planets) * 2 / 5)
	cells: map[[2]int]struct{}
	defer delete(cells)
	for p, i in u.planets {
		planet_actor := actor_get(&u, p.actor)
		body := actor_at(&u, planet_actor.interior, map_center(.Planet_Vicinity))
		orbit := actor_get(&u, body).interior
		in_orbit := 0
		clear(&cells)
		for id in map_get(&u, orbit).actors {
			a := actor_get(&u, id)
			testing.expect(t, a.pos not_in cells)
			cells[a.pos] = {}
			if a.kind == .Shipyard {
				in_orbit += 1
				testing.expect(t, a.planet == Planet_Id(i + 1))
			}
		}
		testing.expect(t, in_orbit <= 1)
	}
}

@(test)
a_shipyard_offers_to_be_entered :: proc(t: ^testing.T) {
	u := generate(2)
	defer universe_destroy(&u)
	list, n := offered(&u, first_yard(&u))
	testing.expect_value(t, n, 1)
	testing.expect_value(t, list[0], Interaction.Enter_Shipyard)
	label: Name
	testing.expect_value(t, interaction_label(&u, .Enter_Shipyard, nil, &label), "Enter Shipyard")
}
