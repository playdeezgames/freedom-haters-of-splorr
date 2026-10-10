package game

// What each faction forbids, and what happens when a patrol finds you carrying it (new in the port). A faction's
// bans come from its values and Authority, so the pedia can explain them; SIGMO, the fascists, ban the most.
// Infamy is the underworld's score for you: it rises when you are caught, bribe, fight patrols or sell shady
// goods, and a notorious avatar is treated as more of a threat. Nothing here knows about screens.

Goods_Set :: bit_set[Good]

SHADY_GOODS :: Goods_Set{.Narcotics, .Weapons} // selling these gets you talked about

INFAMY_CAUGHT :: 2
INFAMY_BRIBE :: 3
INFAMY_FLEE :: 1
INFAMY_KILL :: 2
INFAMY_PER_SALE_UNITS :: 5 // shady goods sold per point of infamy (rounded up per sale)
INFAMY_WARY :: 20 // patrols start to step up their hostility at this infamy
INFAMY_WANTED :: 60 // and again here
FINE_PER_VALUE :: 2 // a contraband fine is this times what the goods are worth

// The goods a faction bans: a set derived from its traits.
faction_bans :: proc(f: Faction, id: Faction_Id) -> (bans: Goods_Set) {
	if id == SIGMO_FACTION {
		return {.Narcotics, .Weapons, .Gems} // luxuries are unpatriotic
	}
	free := .Sovereign_Freedom in f.values
	if (f.authority >= 45 || .Absolute_Order in f.values || .Sustainable_Harmony in f.values) && !(free && f.authority < 75) {
		bans += {.Narcotics}
	}
	armed := .Martial_Honor in f.values || free
	if (f.authority >= 60 && !armed) || (.Sustainable_Harmony in f.values && .Martial_Honor not_in f.values) {
		bans += {.Weapons}
	}
	if .Collective_Prosperity in f.values && f.standards <= 40 {
		bans += {.Gems} // hoarding
	}
	return
}

good_banned :: proc(u: ^Universe, faction: Faction_Id, good: Good) -> bool {
	return good in faction_bans(faction_get(u, faction)^, faction)
}

// Whether trading a good is against the law at a planet (its faction's law applies).
good_banned_at :: proc(u: ^Universe, planet: Planet_Id, good: Good) -> bool {
	return good_banned(u, planet_get(u, planet).faction, good)
}

// What the avatar carries that `faction` forbids.
contraband_units :: proc(u: ^Universe, faction: Faction_Id) -> (units, value: int) {
	bans := faction_bans(faction_get(u, faction)^, faction)
	for good in bans {
		units += u.avatar.cargo[good]
		value += u.avatar.cargo[good] * good_info[good].base_price
	}
	return
}

// ---- Searches ----

// What it costs to keep your contraband: twice what it is worth.
search_fine :: proc(u: ^Universe, faction: Faction_Id) -> int {
	_, value := contraband_units(u, faction)
	return max(10, FINE_PER_VALUE * value)
}

search_fine_affordable :: proc(u: ^Universe, faction: Faction_Id) -> bool {
	return u.avatar.jools >= search_fine(u, faction)
}

avatar_gain_infamy :: proc(u: ^Universe, amount: int) {
	u.avatar.infamy += amount
}

// Hands over everything the faction bans. Returns how many units went.
avatar_surrender_contraband :: proc(u: ^Universe, ship: Actor_Id) -> (units: int) {
	faction := actor_get(u, ship).faction
	bans := faction_bans(faction_get(u, faction)^, faction)
	for good in bans {
		units += u.avatar.cargo[good]
		u.avatar.cargo[good] = 0
	}
	avatar_gain_infamy(u, INFAMY_CAUGHT)
	ship_calm(u, ship, CALM_AFTER_FINE)
	return
}

// Pays to keep it. Returns the fine.
avatar_bribe :: proc(u: ^Universe, ship: Actor_Id) -> int {
	fine := search_fine(u, actor_get(u, ship).faction)
	u.avatar.jools -= fine
	avatar_gain_infamy(u, INFAMY_BRIBE)
	ship_calm(u, ship, CALM_AFTER_FINE)
	return fine
}

// Selling shady goods earns a name.
infamy_for_sale :: proc(good: Good, sold: int) -> int {
	if good in SHADY_GOODS && sold > 0 {
		return (sold + INFAMY_PER_SALE_UNITS - 1) / INFAMY_PER_SALE_UNITS
	}
	return 0
}
