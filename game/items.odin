package game

// Items are individual records in a pool (decided in PORT_PLAN.md): an unequipped fuel tank remembers how full
// it is, and deliveries will carry a destination and reward. The UI groups identical items into stacks.
// Values are ported from the VB item descriptors.

Item_Id :: distinct int

Item_Kind :: enum {
	Scrap,
	Oxygen_Tank,
	Fuel_Rod,
	Fuel_Scoop,
	Atmospheric_Concentrator,
	Fuel_Supply, // marked: Mark I..V
	Life_Support, // marked: Mark I..V
}

MAX_MARK :: 5

Item :: struct {
	kind:  Item_Kind,
	mark:  int, // 1..MAX_MARK for marked kinds, else 0
	level: int, // what it holds: oxygen in a tank, fuel in a rod, or what a removed supply still has in it
}

Item_Info :: struct {
	name:          string, // marked kinds append " Mark N"
	price:         int, // to buy; marked kinds multiply by the mark
	offer:         int, // what a trader pays for one; 0 means they don't buy it
	tech_level:    int, // a trading post needs at least this; -1 means no requirement; marked kinds use the mark
	install_fee:   int, // marked kinds multiply by the mark
	uninstall_fee: int,
	marked:        bool,
}

item_info := [Item_Kind]Item_Info {
	.Scrap                    = {name = "Scrap", offer = 1, tech_level = -1},
	.Oxygen_Tank              = {name = "Oxygen Tank", price = 5, tech_level = -1},
	.Fuel_Rod                 = {name = "Fuel Rod", price = 20, tech_level = -1},
	.Fuel_Scoop               = {name = "Fuel Scoop", price = 10000, tech_level = 7, install_fee = 100, uninstall_fee = 50},
	.Atmospheric_Concentrator = {name = "AeroSynth Recharger", price = 5000, tech_level = 3, install_fee = 25, uninstall_fee = 15},
	.Fuel_Supply              = {name = "StarLume Fuel", price = 500, tech_level = 0, install_fee = 10, uninstall_fee = 5, marked = true},
	.Life_Support             = {name = "EterniVita", price = 500, tech_level = 0, install_fee = 10, uninstall_fee = 5, marked = true},
}

// What a full tank of a new tank/rod holds, and a supply's capacity per mark.
OXYGEN_TANK_LEVEL :: 100
FUEL_ROD_LEVEL :: 100
CAPACITY_PER_MARK :: 250

item_name :: proc(item: Item) -> Name {
	info := item_info[item.kind]
	if !info.marked {
		return name_make(info.name)
	}
	digits: [20]u8
	return name_join(info.name, " Mark ", int_text(&digits, item.mark))
}

item_price :: proc(item: Item) -> int {
	info := item_info[item.kind]
	return info.price * max(item.mark, 1) if info.marked else info.price
}

item_tech_level :: proc(item: Item) -> int {
	info := item_info[item.kind]
	return item.mark if info.marked else info.tech_level
}

// A brand new item of `kind`, full where that means something.
item_new :: proc(kind: Item_Kind, mark: int = 0) -> Item {
	item := Item{kind = kind, mark = mark}
	#partial switch kind {
	case .Oxygen_Tank:
		item.level = OXYGEN_TANK_LEVEL
	case .Fuel_Rod:
		item.level = FUEL_ROD_LEVEL
	case .Fuel_Supply, .Life_Support:
		assert(mark >= 1 && mark <= MAX_MARK)
		item.level = CAPACITY_PER_MARK * mark
	}
	return item
}

// ---- Stacks: identical items shown together ----

Item_Stack :: struct {
	kind:  Item_Kind,
	mark:  int,
	count: int,
}

MAX_STACKS :: 64

Stacks :: struct {
	stacks: [MAX_STACKS]Item_Stack,
	count:  int,
}

// Items are identical for display when they have the same kind and mark (fullness doesn't split a stack).
inventory_stacks :: proc(u: ^Universe) -> (result: Stacks) {
	for id in u.avatar.inventory {
		item := item_get(u, id)^
		found := false
		for i in 0 ..< result.count {
			s := &result.stacks[i]
			if s.kind == item.kind && s.mark == item.mark {
				s.count += 1
				found = true
				break
			}
		}
		if !found && result.count < MAX_STACKS {
			result.stacks[result.count] = {item.kind, item.mark, 1}
			result.count += 1
		}
	}
	return
}

item_stack_name :: proc(s: Item_Stack) -> Name {
	return item_name(Item{kind = s.kind, mark = s.mark})
}

item_description :: proc(kind: Item_Kind) -> string {
	switch kind {
	case .Scrap:
		return "This item is a pile of junk that was floating around in space."
	case .Oxygen_Tank:
		return "This item can be used to replenish a vessel's oxygen."
	case .Fuel_Rod:
		return "You ram this into yer engine in order to fill it with fuel. No, there is nothing sexual about this. Not at all."
	case .Fuel_Scoop, .Atmospheric_Concentrator, .Fuel_Supply, .Life_Support:
		return ""
	}
	return ""
}
