package game

// The criminal underworld (new in the port). Rough planets (chaotic or low-Standards factions) have fixers at their
// star docks. A four-step contact quest, started by asking around, gets you "connected": then black markets
// appear on every rough planet (they trade any good at better prices, banned or not) and rough docks offer shady
// deliveries. Nothing here knows about screens.
//
//   Ask Around (a rough dock) -> Lead: meet the fixer on the named rough planet and take a sealed package
//   -> Package: hand it to the contact on another rough planet
//   -> Goods: bring the contact GOODS_REQUIRED units of Narcotics or Weapons
//   -> Prove: win or flee a fight with a patrol ship
//   -> Proven: report back to the contact -> Connected

GOODS_REQUIRED :: 10
INFAMY_PACKAGE :: 3
INFAMY_GOODS :: 5
INFAMY_CONNECTED :: 10
CONNECTION_GIFT :: 100 // jools, for joining
SHADY_REWARD_FACTOR :: 3
INFAMY_SHADY_DELIVERY :: 2
BLACK_BUY_PERCENT :: 105 // you pay a little over the market price
BLACK_SELL_BASE_PERCENT :: 115 // and are paid a premium...
BLACK_SELL_INFAMY_STEP :: 4 // ...which grows with infamy, one percent per this much
BLACK_SELL_INFAMY_LIMIT :: 20

Quest_Stage :: enum {
	None,
	Lead,
	Package,
	Goods,
	Prove,
	Proven,
	Connected,
}

Quest :: struct {
	stage:   Quest_Stage,
	fixer:   Planet_Id, // where the lead sends you
	contact: Planet_Id, // where the package, the goods and the report go
}

// A planet is rough if its faction is lawless or wicked enough. SIGMO (100 and 100) never is.
planet_is_rough :: proc(u: ^Universe, planet: Planet_Id) -> bool {
	f := faction_get(u, planet_get(u, planet).faction)
	return f.standards <= 42 || f.authority <= 35
}

@(private = "file")
other_rough_planet :: proc(u: ^Universe, not: Planet_Id) -> Planet_Id {
	candidates: [dynamic]Planet_Id
	defer delete(candidates)
	for _, i in u.planets {
		id := Planet_Id(i + 1)
		if id != not && planet_is_rough(u, id) {
			append(&candidates, id)
		}
	}
	if len(candidates) == 0 {
		return not
	}
	return rng_pick(&u.rng, candidates[:])
}

// The orbit map of a planet.
planet_orbit :: proc(u: ^Universe, planet: Planet_Id) -> Map_Id {
	vicinity := actor_get(u, planet_get(u, planet).actor).interior
	body := actor_at(u, vicinity, map_center(.Planet_Vicinity))
	return actor_get(u, body).interior
}

// ---- The package ----

package_in_hold :: proc(u: ^Universe) -> Item_Id {
	for id in u.avatar.inventory {
		if item_get(u, id).kind == .Package {
			return id
		}
	}
	return 0
}

// Items a patrol never takes and a defeat never costs: quest things.
item_is_kept :: proc(kind: Item_Kind) -> bool {
	return kind == .Delivery || kind == .Package
}

shady_units :: proc(u: ^Universe) -> int {
	return u.avatar.cargo[.Narcotics] + u.avatar.cargo[.Weapons]
}

// ---- The interaction at a star dock ----

// The label of what the underworld offers at this dock, if anything.
underworld_offer :: proc(u: ^Universe, dock: Actor_Id) -> (label: string, ok: bool) {
	planet := actor_get(u, dock).planet
	q := u.avatar.quest
	switch q.stage {
	case .None:
		return "Ask Around", planet_is_rough(u, planet)
	case .Lead:
		return "Meet The Fixer", planet == q.fixer
	case .Package:
		return "Hand Over The Package", planet == q.contact && package_in_hold(u) != 0
	case .Goods:
		return "Deliver Contraband", planet == q.contact
	case .Prove, .Proven:
		return "Report In", planet == q.contact
	case .Connected:
	}
	return "", false
}

Quest_Result :: enum {
	Rumor,
	Package_Given,
	Package_Delivered,
	Goods_Delivered,
	Goods_Short,
	Not_Proven,
	Connected,
}

// Does what the offer says. The dock's planet is where it happens.
quest_advance :: proc(u: ^Universe, dock: Actor_Id) -> Quest_Result {
	planet := actor_get(u, dock).planet
	q := &u.avatar.quest
	switch q.stage {
	case .None:
		q.stage = .Lead
		q.fixer = other_rough_planet(u, planet)
		return .Rumor
	case .Lead:
		q.stage = .Package
		q.contact = other_rough_planet(u, q.fixer)
		append(&u.avatar.inventory, item_add(u, item_new(.Package)))
		return .Package_Given
	case .Package:
		id := package_in_hold(u)
		for have, i in u.avatar.inventory {
			if have == id {
				ordered_remove(&u.avatar.inventory, i)
				break
			}
		}
		q.stage = .Goods
		avatar_gain_infamy(u, INFAMY_PACKAGE)
		return .Package_Delivered
	case .Goods:
		if shady_units(u) < GOODS_REQUIRED {
			return .Goods_Short
		}
		left := GOODS_REQUIRED
		for good in ([]Good{.Narcotics, .Weapons}) {
			take := min(left, u.avatar.cargo[good])
			u.avatar.cargo[good] -= take
			left -= take
		}
		q.stage = .Prove
		avatar_gain_infamy(u, INFAMY_GOODS)
		return .Goods_Delivered
	case .Prove:
		return .Not_Proven
	case .Proven:
		underworld_connect(u)
		return .Connected
	case .Connected:
	}
	return .Not_Proven
}

// Called when a fight with a patrol is won or escaped: the contact wants to know you can take the heat.
quest_note_fight :: proc(u: ^Universe) {
	if u.avatar.quest.stage == .Prove {
		u.avatar.quest.stage = .Proven
	}
}

// Joins the underworld: a reward, black markets on every rough planet, shady deliveries at their docks.
underworld_connect :: proc(u: ^Universe) {
	u.avatar.quest.stage = .Connected
	avatar_gain_infamy(u, INFAMY_CONNECTED)
	u.avatar.jools += CONNECTION_GIFT
	for _, i in u.planets {
		planet := Planet_Id(i + 1)
		if !planet_is_rough(u, planet) {
			continue
		}
		orbit := planet_orbit(u, planet)
		size := map_sizes[.Planet_Orbit]
		for _ in 0 ..< MAX_PLACEMENT_TRIES {
			pos := [2]int{rng_range(&u.rng, 1, size.x - 2), rng_range(&u.rng, 1, size.y - 2)}
			if cell_is_free(u, orbit, pos) {
				actor_add(u, orbit, {kind = .Black_Market, pos = pos, star_system = planet_get(u, planet).star_system, planet = planet})
				break
			}
		}
	}
	for a, i in u.actors {
		if a.kind == .Star_Dock && planet_is_rough(u, a.planet) {
			mission_generate(u, Actor_Id(i + 1))
		}
	}
}

// What the contact says, for the message screen.
underworld_message :: proc(u: ^Universe, result: Quest_Result) -> Message {
	digits: [20]u8
	q := u.avatar.quest
	switch result {
	case .Rumor:
		m := message_make(.Magenta, "A man in a long coat", "finds you at the dock.", "")
		message_add(&m, .Light_Gray, "\"Ask for the fixer on")
		message_add(&m, .White, name_str(&planet_get(u, q.fixer).name))
		message_add(&m, .Light_Gray, "in ", name_str(&star_system_get(u, planet_get(u, q.fixer).star_system).name), ".\"")
		return m
	case .Package_Given:
		m := message_make(.Magenta, "The fixer slides you a package.", "")
		message_add(&m, .Light_Gray, "\"Take this to my friend on")
		message_add(&m, .White, name_str(&planet_get(u, q.contact).name))
		message_add(&m, .Light_Gray, "in ", name_str(&star_system_get(u, planet_get(u, q.contact).star_system).name), ".\"")
		return m
	case .Package_Delivered:
		m := message_make(.Magenta, "The contact weighs the package", "in one hand.", "")
		message_add(&m, .Light_Gray, "\"Now bring me ", int_text(&digits, GOODS_REQUIRED), " units of")
		message_add(&m, .Light_Gray, "something that is not legal.\"")
		message_add(&m, .Light_Gray, "(Narcotics or Weapons.)")
		return m
	case .Goods_Short:
		m := message_make(.Magenta, "\"That is not enough.\"", "")
		message_add(&m, .Light_Gray, "Bring ", int_text(&digits, GOODS_REQUIRED), " units of")
		message_add(&m, .Light_Gray, "Narcotics or Weapons.")
		return m
	case .Goods_Delivered:
		return message_make(.Magenta, "\"Nice.\"", "", "\"Now show me you can take heat.\"", "Beat or outrun a patrol ship,", "then come back.")
	case .Not_Proven:
		return message_make(.Magenta, "\"Not yet.\"", "", "Beat or outrun a patrol ship,", "then come back.")
	case .Connected:
		m := message_make(.Light_Green, "You are connected.", "")
		message_add(&m, .Light_Gray, "\"You are one of us.\"")
		message_add(&m, .Light_Gray, "Black markets are open on")
		message_add(&m, .Light_Gray, "the rough worlds.")
		message_add(&m, .Light_Gray, "Reward: ", int_text(&digits, CONNECTION_GIFT), " jools.")
		return m
	}
	return {}
}
