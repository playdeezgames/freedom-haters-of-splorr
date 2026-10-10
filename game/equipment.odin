package game

// What is installed on the ship, and the shipyard that changes it. Ported from the VB (ActorExtensions.Equip /
// Unequip, the item descriptors' Equip/Unequip, and the shipyard dialogs). Nothing here knows about screens.
//
// Fuel and life-support units remember their own level: removing one saves how full the tank was and
// empties the ship's tank, and installing one fills the tank to that unit's level. New units are full, so
// swapping in a spare is a refill, and swapping back restores what the old unit held.

Equip_Slot :: enum {
	Life_Support,
	Fuel_Supply,
	Accessory_0,
	Accessory_1,
	Weapon,
	Shield,
	Armour,
}

Slot_Info :: struct {
	name:      string,
	mandatory: bool, // always holds something: it can be swapped but never emptied
}

slot_info := [Equip_Slot]Slot_Info {
	.Life_Support = {"Life Support", true},
	.Fuel_Supply  = {"Fuel Supply", true},
	.Accessory_0  = {"Accessory(0)", false},
	.Accessory_1  = {"Accessory(1)", false},
	.Weapon       = {"Weapon", false},
	.Shield       = {"Shield", false},
	.Armour       = {"Armour", false},
}

slot_accepts :: proc(slot: Equip_Slot, kind: Item_Kind) -> bool {
	switch slot {
	case .Life_Support:
		return kind == .Life_Support
	case .Fuel_Supply:
		return kind == .Fuel_Supply
	case .Accessory_0, .Accessory_1:
		return kind == .Fuel_Scoop || kind == .Atmospheric_Concentrator
	case .Weapon:
		return kind == .Pulse_Laser
	case .Shield:
		return kind == .Deflector_Shield
	case .Armour:
		return kind == .Armour_Plating
	}
	return false
}

avatar_has_equipped :: proc(u: ^Universe, kind: Item_Kind) -> bool {
	for id in u.avatar.equipment {
		if id != 0 && item_get(u, id).kind == kind {
			return true
		}
	}
	return false
}

@(private = "file")
inventory_remove :: proc(u: ^Universe, id: Item_Id) -> bool {
	for have, i in u.avatar.inventory {
		if have == id {
			ordered_remove(&u.avatar.inventory, i)
			return true
		}
	}
	return false
}

// Puts an item in an empty slot it fits. Pays the install fee unless `charge` is false (the ship's first
// units come with it). The item comes out of the hold if it was there.
equip_item :: proc(u: ^Universe, slot: Equip_Slot, id: Item_Id, charge := true) -> bool {
	item := item_get(u, id)
	if u.avatar.equipment[slot] != 0 || !slot_accepts(slot, item.kind) {
		return false
	}
	inventory_remove(u, id)
	u.avatar.equipment[slot] = id
	#partial switch item.kind {
	case .Fuel_Supply:
		u.avatar.fuel.maximum = CAPACITY_PER_MARK * item.mark
		u.avatar.fuel.current = min(item.level, u.avatar.fuel.maximum)
	case .Life_Support:
		u.avatar.oxygen.maximum = CAPACITY_PER_MARK * item.mark
		u.avatar.oxygen.current = min(item.level, u.avatar.oxygen.maximum)
	}
	avatar_refresh_hull(u)
	if charge {
		u.avatar.jools -= item_install_fee(item^)
	}
	return true
}

// Takes what is in a slot back into the hold, remembering a unit's level, and pays the uninstall fee.
unequip_item :: proc(u: ^Universe, slot: Equip_Slot) -> Item_Id {
	id := u.avatar.equipment[slot]
	if id == 0 {
		return 0
	}
	item := item_get(u, id)
	#partial switch item.kind {
	case .Fuel_Supply:
		item.level = u.avatar.fuel.current
		u.avatar.fuel.maximum, u.avatar.fuel.current = 0, 0
	case .Life_Support:
		item.level = u.avatar.oxygen.current
		u.avatar.oxygen.maximum, u.avatar.oxygen.current = 0, 0
	}
	u.avatar.equipment[slot] = 0
	avatar_refresh_hull(u)
	append(&u.avatar.inventory, id)
	u.avatar.jools -= item_uninstall_fee(item^)
	return id
}

// ---- Hull ----

// The ship's own hull plus whatever plating is installed. Installing plating raises the maximum without
// mending anything; removing it can cut the current hull down to the new maximum.
avatar_refresh_hull :: proc(u: ^Universe) {
	maximum := BASE_HULL
	if id := u.avatar.equipment[.Armour]; id != 0 {
		maximum += armour_hull(item_get(u, id).mark)
	}
	u.avatar.hull.maximum = maximum
	u.avatar.hull.current = min(u.avatar.hull.current, maximum)
}

hull_repair_price :: proc(u: ^Universe) -> int {
	return price_of(top_off_amount(u.avatar.hull), HULL_PER_JOOL)
}

Repair_Result :: enum {
	Repaired,
	Nothing_To_Repair,
	Insufficient_Funds,
}

// A shipyard mends the whole hull for jools.
shipyard_repair :: proc(u: ^Universe) -> (result: Repair_Result, mended, cost: int) {
	mended = top_off_amount(u.avatar.hull)
	if mended == 0 {
		return .Nothing_To_Repair, 0, 0
	}
	cost = hull_repair_price(u)
	if u.avatar.jools < cost {
		return .Insufficient_Funds, 0, cost
	}
	u.avatar.hull.current = u.avatar.hull.maximum
	u.avatar.jools -= cost
	return .Repaired, mended, cost
}

// ---- The shipyard ----

MAX_INSTALLABLE :: 32

Installable :: struct {
	items: [MAX_INSTALLABLE]Item_Id,
	count: int,
}

// What in the hold fits this slot (each item separately: they differ in how full they are).
installable_items :: proc(u: ^Universe, slot: Equip_Slot) -> (list: Installable) {
	for id in u.avatar.inventory {
		if slot_accepts(slot, item_get(u, id).kind) && list.count < MAX_INSTALLABLE {
			list.items[list.count] = id
			list.count += 1
		}
	}
	return
}

// Slots where something can still be done: mandatory ones when a spare fits, accessories when something is
// installed or something fits. A slot with nothing to offer is not worth listing in a menu, but the shipyard
// screen lists every slot and lets the slot screen say so.
slot_item_name :: proc(u: ^Universe, slot: Equip_Slot) -> Name {
	id := u.avatar.equipment[slot]
	if id == 0 {
		return name_make("(empty)")
	}
	return item_name(item_get(u, id)^)
}

// The fee to swap what is in `slot` for `new_item` (0 meaning just uninstall).
change_fee :: proc(u: ^Universe, slot: Equip_Slot, new_item: Item_Id) -> (fee: int) {
	if old := u.avatar.equipment[slot]; old != 0 {
		fee += item_uninstall_fee(item_get(u, old)^)
	}
	if new_item != 0 {
		fee += item_install_fee(item_get(u, new_item)^)
	}
	return
}

Change_Result :: enum {
	Done,
	Insufficient_Tech, // the yard's planet is behind either the new unit or the one being removed
	Insufficient_Funds,
	Not_Allowed, // emptying a mandatory slot, or an item that doesn't fit
}

Change :: struct {
	result:      Change_Result,
	removed:     Item_Id,
	installed:   Item_Id,
	fee:         int,
}

// Swaps what is in `slot` for `new_item` (or just empties an accessory slot with 0), as a shipyard does.
shipyard_change :: proc(u: ^Universe, yard: Actor_Id, slot: Equip_Slot, new_item: Item_Id) -> (change: Change) {
	change.fee = change_fee(u, slot, new_item)
	if new_item == 0 && (slot_info[slot].mandatory || u.avatar.equipment[slot] == 0) {
		change.result = .Not_Allowed
		return
	}
	if new_item != 0 {
		in_hold := false
		for id in u.avatar.inventory {
			in_hold ||= id == new_item
		}
		if !in_hold || !slot_accepts(slot, item_get(u, new_item).kind) {
			change.result = .Not_Allowed
			return
		}
	}
	tech := post_tech_level(u, yard)
	if new_item != 0 && item_tech_level(item_get(u, new_item)^) > tech {
		change.result = .Insufficient_Tech
		return
	}
	if old := u.avatar.equipment[slot]; old != 0 && item_tech_level(item_get(u, old)^) > tech {
		change.result = .Insufficient_Tech
		return
	}
	if change.fee > 0 && u.avatar.jools < change.fee {
		change.result = .Insufficient_Funds
		return
	}
	change.removed = unequip_item(u, slot)
	if new_item != 0 {
		equip_item(u, slot, new_item)
		change.installed = new_item
	}
	change.result = .Done
	return
}

// ---- The fuel scoop ----

// Free fuel from a star, if a scoop is installed and there is room in the tank.
avatar_use_fuel_scoop :: proc(u: ^Universe) -> (added: int) {
	added = top_off_amount(u.avatar.fuel)
	u.avatar.fuel.current = u.avatar.fuel.maximum
	return
}
