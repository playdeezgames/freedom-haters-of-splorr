package game

// Buying, selling and using items, ported from the VB trader models. Nothing here knows about screens.
//
// A trading post sells a fixed list of items, limited by its planet's tech level, at the items' fixed
// prices, and buys only scrap. Buying more than you can afford is not possible; spending exactly
// everything you have is.

MAX_TRADE_ITEMS :: 40

Trade_Item :: struct {
	kind: Item_Kind,
	mark: int,
}

Trade_List :: struct {
	items: [MAX_TRADE_ITEMS]Trade_Item,
	count: int,
}

@(private = "file")
trade_list_add :: proc(list: ^Trade_List, kind: Item_Kind, mark: int = 0) {
	list.items[list.count] = {kind, mark}
	list.count += 1
}

post_tech_level :: proc(u: ^Universe, post: Actor_Id) -> int {
	return planet_get(u, actor_get(u, post).planet).tech_level
}

// What the post sells: tank and rod always, the rest once its planet is advanced enough.
trade_prices :: proc(u: ^Universe, post: Actor_Id) -> (list: Trade_List) {
	tech := post_tech_level(u, post)
	trade_list_add(&list, .Oxygen_Tank)
	trade_list_add(&list, .Fuel_Rod)
	if tech >= item_info[.Atmospheric_Concentrator].tech_level {
		trade_list_add(&list, .Atmospheric_Concentrator)
	}
	if tech >= item_info[.Fuel_Scoop].tech_level {
		trade_list_add(&list, .Fuel_Scoop)
	}
	for mark in 1 ..= MAX_MARK {
		if item_tech_level(item_new(.Life_Support, mark)) <= tech {
			trade_list_add(&list, .Life_Support, mark)
		}
		if item_tech_level(item_new(.Fuel_Supply, mark)) <= tech {
			trade_list_add(&list, .Fuel_Supply, mark)
		}
		for kind in ([]Item_Kind{.Pulse_Laser, .Deflector_Shield, .Armour_Plating}) {
			if item_tech_level(item_new(kind, mark)) <= tech {
				trade_list_add(&list, kind, mark)
			}
		}
	}
	return
}

inventory_count :: proc(u: ^Universe, kind: Item_Kind, mark: int = 0) -> (n: int) {
	for id in u.avatar.inventory {
		item := item_get(u, id)
		if item.kind == kind && item.mark == mark {
			n += 1
		}
	}
	return
}

trade_unit_price :: proc(u: ^Universe, kind: Item_Kind, mark: int = 0) -> int {
	return service_price(u, item_price(item_new(kind, mark)))
}

// The most of an item you can afford.
trade_max_buy :: proc(u: ^Universe, kind: Item_Kind, mark: int = 0) -> int {
	price := trade_unit_price(u, kind, mark)
	if price <= 0 || u.avatar.jools < price {
		return 0
	}
	return u.avatar.jools / price
}

// Buys up to `quantity`, never more than you can afford. Returns how many were bought.
trade_buy :: proc(u: ^Universe, kind: Item_Kind, mark, quantity: int) -> int {
	quantity := min(quantity, trade_max_buy(u, kind, mark))
	price := trade_unit_price(u, kind, mark)
	for _ in 0 ..< quantity {
		append(&u.avatar.inventory, item_add(u, item_new(kind, mark)))
		u.avatar.jools -= price
	}
	return max(quantity, 0)
}

// What the post buys from you right now: only scrap, and only if you have some.
trade_offers :: proc(u: ^Universe, post: Actor_Id) -> (list: Trade_List) {
	for kind in Item_Kind {
		if item_info[kind].offer > 0 && inventory_count(u, kind) > 0 {
			trade_list_add(&list, kind)
		}
	}
	return
}

trade_offer_total :: proc(kind: Item_Kind, quantity: int) -> int {
	return item_info[kind].offer * quantity
}

// Sells up to `quantity` and returns how many were sold.
trade_sell :: proc(u: ^Universe, kind: Item_Kind, quantity: int) -> int {
	sold := 0
	i := 0
	for i < len(u.avatar.inventory) && sold < quantity {
		if item_get(u, u.avatar.inventory[i]).kind == kind {
			ordered_remove(&u.avatar.inventory, i)
			u.avatar.jools += item_info[kind].offer
			sold += 1
		} else {
			i += 1
		}
	}
	return sold
}

// ---- Using items ----

Use_Result :: struct {
	used:   bool,
	added:  int,
	percent: int, // how full the tank is afterwards
}

// Removes and returns the first item of `kind` from the hold, if any.
@(private = "file")
take_item :: proc(u: ^Universe, kind: Item_Kind) -> (item: Item, ok: bool) {
	for id, i in u.avatar.inventory {
		if item_get(u, id).kind == kind {
			item = item_get(u, id)^
			ordered_remove(&u.avatar.inventory, i)
			return item, true
		}
	}
	return
}

// Pours an oxygen tank into the ship. The empty tank is junk: it becomes scrap.
avatar_use_oxygen_tank :: proc(u: ^Universe) -> (result: Use_Result) {
	tank, ok := take_item(u, .Oxygen_Tank)
	if !ok {
		return
	}
	o := &u.avatar.oxygen
	before := o.current
	o.current = min(o.current + tank.level, o.maximum)
	append(&u.avatar.inventory, item_add(u, item_new(.Scrap)))
	return {used = true, added = o.current - before, percent = percent_of(o^)}
}

// Pours a fuel rod into the engine. Nothing is left over.
avatar_use_fuel_rod :: proc(u: ^Universe) -> (result: Use_Result) {
	rod, ok := take_item(u, .Fuel_Rod)
	if !ok {
		return
	}
	f := &u.avatar.fuel
	before := f.current
	f.current = min(f.current + rod.level, f.maximum)
	return {used = true, added = f.current - before, percent = percent_of(f^)}
}

// The ship refuses to die while there is a tank to use: when a turn would take the last of the oxygen,
// one is used automatically. Returns whether that happened.
avatar_auto_use_tank :: proc(u: ^Universe) -> bool {
	if u.avatar.oxygen.current > u.avatar.oxygen.minimum {
		return false
	}
	result := avatar_use_oxygen_tank(u)
	if result.used {
		u.avatar.auto_used = result
	}
	return result.used
}
