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
	Pulse_Laser, // marked: the weapon slot
	Deflector_Shield, // marked: the shield slot
	Armour_Plating, // marked: the armour slot
	Package, // a sealed package for the underworld quest
	Delivery, // something to take to another planet; carries a Mission
}

MAX_MARK :: 5

Item :: struct {
	kind:    Item_Kind,
	mark:    int, // 1..MAX_MARK for marked kinds, else 0
	level:   int, // what it holds: oxygen in a tank, fuel in a rod, or what a removed supply still has in it
	mission: Mission, // deliveries only
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
	.Pulse_Laser              = {name = "Pulse Laser", price = 300, install_fee = 20, uninstall_fee = 10, marked = true},
	.Deflector_Shield         = {name = "Deflector Shield", price = 250, install_fee = 15, uninstall_fee = 8, marked = true},
	.Armour_Plating           = {name = "Armour Plating", price = 200, install_fee = 25, uninstall_fee = 12, marked = true},
	.Package                  = {name = "Sealed Package", tech_level = -1},
	.Delivery                 = {name = "Delivery", tech_level = -1},
}

// What the combat gear does (new in the port; the VB had no combat). Each is per mark.
BASE_HULL :: 100
HULL_PER_ARMOUR_MARK :: 50
weapon_damage :: proc(mark: int) -> int {return 8 + 6 * mark} // per Fire
shield_capacity :: proc(mark: int) -> int {return 20 * mark} // damage it soaks up each fight
armour_hull :: proc(mark: int) -> int {return HULL_PER_ARMOUR_MARK * mark}
HULL_PER_JOOL :: 2 // shipyard repairs

// What a full tank of a new tank/rod holds, and a supply's capacity per mark.
OXYGEN_TANK_LEVEL :: 100
FUEL_ROD_LEVEL :: 100
CAPACITY_PER_MARK :: 250

item_name :: proc(item: Item) -> Name {
	info := item_info[item.kind]
	if !info.marked {
		return name_make(info.name)
	}
	return name_join(info.name, " Mark ", mark_numerals[item.mark])
}

// The VB names marks with roman numerals.
mark_numerals := [MAX_MARK + 1]string{"", "I", "II", "III", "IV", "V"}

item_price :: proc(item: Item) -> int {
	info := item_info[item.kind]
	return info.price * max(item.mark, 1) if info.marked else info.price
}

// The VB's life support units need 1, 3, 5, 7 and 9; fuel supplies need their mark; the rest are fixed.
life_support_tech_levels := [MAX_MARK + 1]int{0, 1, 3, 5, 7, 9}

item_tech_level :: proc(item: Item) -> int {
	#partial switch item.kind {
	case .Life_Support:
		return life_support_tech_levels[item.mark]
	case .Fuel_Supply, .Armour_Plating:
		return item.mark
	case .Pulse_Laser:
		return 2 * item.mark
	case .Deflector_Shield:
		return 2 * item.mark - 1
	}
	return item_info[item.kind].tech_level
}

item_install_fee :: proc(item: Item) -> int {
	info := item_info[item.kind]
	return info.install_fee * max(item.mark, 1) if info.marked else info.install_fee
}

item_uninstall_fee :: proc(item: Item) -> int {
	info := item_info[item.kind]
	return info.uninstall_fee * max(item.mark, 1) if info.marked else info.uninstall_fee
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
	item:  Item_Id, // deliveries are each their own stack; this is which one
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
			if item.kind == .Delivery {
				break // every delivery is different
			}
			s := &result.stacks[i]
			if s.kind == item.kind && s.mark == item.mark {
				s.count += 1
				found = true
				break
			}
		}
		if !found && result.count < MAX_STACKS {
			result.stacks[result.count] = {item.kind, item.mark, 1, id if item.kind == .Delivery else 0}
			result.count += 1
		}
	}
	return
}

item_stack_name :: proc(u: ^Universe, s: Item_Stack) -> Name {
	if s.kind == .Delivery {
		return name_make(mission_nouns[item_get(u, s.item).mission.noun])
	}
	return item_name(Item{kind = s.kind, mark = s.mark})
}

// ---- Descriptions, ported from the VB. The font is CP437, so dashes and apostrophes are plain ASCII. ----

MAX_PARAGRAPHS :: 8

Description :: struct {
	paragraphs: [MAX_PARAGRAPHS]string,
	count:      int,
}

@(private = "file")
description_of :: proc(texts: ..string) -> (d: Description) {
	for t in texts {
		d.paragraphs[d.count] = t
		d.count += 1
	}
	return
}

// The paragraphs for an item. Marked kinds start with a line naming the mark, composed into `intro`.
item_description :: proc(item: Item, intro: ^Long_Text) -> Description {
	switch item.kind {
	case .Scrap:
		return description_of("This item is a pile of junk that was floating around in space.")
	case .Oxygen_Tank:
		return description_of("This item can be used to replenish a vessel's oxygen.")
	case .Fuel_Rod:
		return description_of("You ram this into yer engine in order to fill it with fuel. No, there is nothing sexual about this. Not at all.")
	case .Fuel_Scoop:
		return description_of(
			"Tap into the Power of the Stars with the SolarForge Extractor by HeliosDrive Industries",
			"Why settle for conventional fuel sources when you can harness the raw, untamed power of a star? The SolarForge Extractor is your gateway to limitless energy, revolutionizing the way you refuel in the vast expanse of space.",
			"Brought to you by HeliosDrive Industries, the pioneers of stellar energy technology, the SolarForge Extractor is designed for the boldest explorers and the most advanced fleets. This state-of-the-art device captures and condenses stellar energy directly from a star's core, transforming it into a stable, high-density fuel ready for storage in your fuel systems.",
			"Compact, efficient, and incredibly powerful, the SolarForge Extractor allows you to refuel your vessels with ease, no matter where your adventures take you. Whether you're on the fringes of the galaxy or orbiting a distant sun, the SolarForge Extractor ensures you never run out of the energy you need to keep moving forward.",
			"With HeliosDrive Industries, you're not just exploring the stars - you're harnessing them. Equip your fleet with the SolarForge Extractor and experience the true power of the cosmos.",
		)
	case .Atmospheric_Concentrator:
		return description_of(
			"Discover the Future of Planetary Exploration with the AeroSynth Recharger by StarBreathe Technologies",
			"Imagine landing on a new world, breathing in the untouched air, and knowing that your life support system will never run out of fresh, breathable atmosphere. With the AeroSynth Recharger, this is no longer a dream - it's your new reality.",
			"The AeroSynth Recharger is a cutting-edge device engineered by the brilliant minds at StarBreathe Technologies. Designed for explorers, colonists, and spacefarers, the AeroSynth Recharger effortlessly extracts and refines atmospheric elements from any planet, converting them into life-sustaining air for your entire crew.",
			"Compact yet powerful, the AeroSynth Recharger seamlessly integrates with your existing life support systems, recharging them with the perfect blend of gases tailored to human needs. Whether you're on a long-term mission or a short reconnaissance, the AeroSynth Recharger ensures that every breath you take is fresh, clean, and revitalizing.",
			"With StarBreathe Technologies, exploration knows no bounds. Trust the AeroSynth Recharger to keep you breathing easy, wherever your journey takes you.",
		)
	case .Fuel_Supply:
		intro^ = long_join("This is the StarLume Fuel Storage Solution System Mark ", mark_numerals[item.mark], " from Celestial Energy Solutions.")
		return description_of(
			long_str(intro),
			"Embark on interstellar journeys with StarLume Fuel Storage Solution System by Celestial Energy Solutions, the foremost name in propulsion innovation.",
			"Crafted from rare celestial metals and refined through cutting-edge fusion technology, StarLume Fuel Storage Solution System guarantees unmatched efficiency and reliability for your spacecraft.",
			"Whether you're charting new frontiers or navigating through asteroid belts, trust Celestial Energy Solutions to propel you farther and faster than ever before.",
			"Reach for the stars with StarLume Fuel Storage Solution System - where limitless possibilities await beyond every horizon.",
		)
	case .Pulse_Laser:
		intro^ = long_join("This is the Pulse Laser Mark ", mark_numerals[item.mark], " from Liberty Arms.")
		return description_of(
			long_str(intro),
			"Freedom is a right, and rights must be defended. Liberty Arms makes sure yours can be.",
			"Fires a concentrated pulse of light at anything that disagrees with you. Higher marks disagree harder.",
			"Liberty Arms is not responsible for who you disagree with.",
		)
	case .Deflector_Shield:
		intro^ = long_join("This is the Deflector Shield Mark ", mark_numerals[item.mark], " from SafeSpace Ltd.")
		return description_of(
			long_str(intro),
			"Nobody ever got hurt by something that bounced off.",
			"Soaks up a quantity of incoming damage in every encounter, then recharges once things calm down. Higher marks soak up more.",
			"SafeSpace Ltd. recommends not testing it.",
		)
	case .Armour_Plating:
		intro^ = long_join("This is the Armour Plating Mark ", mark_numerals[item.mark], " from IronHull Fabrication.")
		return description_of(
			long_str(intro),
			"More hull is more ship. It is that simple.",
			"Each mark bolts more metal on and raises the damage your ship can take before it comes apart. It does not repair anything: see a shipyard.",
			"IronHull Fabrication accepts no liability for weight.",
		)
	case .Package:
		return description_of("A sealed package. It is warm. You were told not to ask what is in it, and you are not going to.")
	case .Delivery:
		return description_of("A thing to be delivered.")
	case .Life_Support:
		intro^ = long_join("This is the EterniVita Mark ", mark_numerals[item.mark], " from NexGen Dynamics.")
		return description_of(
			long_str(intro),
			"Step into the future with EterniVita, the pinnacle of life support technology.",
			"Engineered to ensure uninterrupted vitality and resilience, EterniVita redefines safety and peace of mind in the most challenging environments.",
			"With its cutting-edge biostasis chambers and adaptive AI monitoring, EterniVita stands as the ultimate safeguard for explorers, colonists, and spacefarers alike.",
			"Embrace limitless possibilities with EterniVita - where every breath guarantees a secure tomorrow, today.",
		)
	}
	return {}
}

// The lines of numbers that follow the description, as the VB listed them: tech level, then capacity.
MAX_STATS :: 3

Item_Stats :: struct {
	lines: [MAX_STATS]Long_Text,
	count: int,
}

item_stats :: proc(item: Item) -> (stats: Item_Stats) {
	add :: proc(stats: ^Item_Stats, parts: ..string) {
		stats.lines[stats.count] = long_join(..parts)
		stats.count += 1
	}
	d1: [20]u8
	if tech := item_tech_level(item); tech >= 0 && item.kind != .Scrap && item.kind != .Oxygen_Tank && item.kind != .Fuel_Rod {
		add(&stats, "Tech Level: ", int_text(&d1, tech))
	}
	d2: [20]u8
	#partial switch item.kind {
	case .Fuel_Supply:
		add(&stats, "Maximum Fuel: ", int_text(&d2, CAPACITY_PER_MARK * item.mark))
	case .Life_Support:
		add(&stats, "Maximum Oxygen: ", int_text(&d2, CAPACITY_PER_MARK * item.mark))
	case .Pulse_Laser:
		add(&stats, "Damage: ", int_text(&d2, weapon_damage(item.mark)))
	case .Deflector_Shield:
		add(&stats, "Absorbs: ", int_text(&d2, shield_capacity(item.mark)))
	case .Armour_Plating:
		add(&stats, "Extra Hull: ", int_text(&d2, armour_hull(item.mark)))
	case .Scrap:
		add(&stats, "Sells for: ", int_text(&d2, item_info[.Scrap].offer))
	}
	return
}
