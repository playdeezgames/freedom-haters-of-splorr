package game

// Delivery missions ("Errand Boy"), ported from the VB: every star dock holds one delivery to offer, bound for
// another planet of the same faction. Taking them is limited by reputation, and finishing or abandoning one
// changes reputation on the planets, star systems and factions at both ends.

MISSION_REWARD_DICE :: "5d20"
MISSION_REPUTATION_BONUS :: 1
MISSION_REPUTATION_PENALTY :: -5
REPUTATION_PER_EXTRA_DELIVERY :: 25 // you may carry 1 + (worst reputation / 25) deliveries
STARTING_REPUTATION :: 100 // with SIGMO, the home planet and the home system

Mission :: struct {
	origin:      Planet_Id,
	destination: Planet_Id,
	reward:      int, // jools; a deposit, if one was paid, is added so it comes back
	adverb:      u8, // indexes into the word tables below
	adjective:   u8,
	noun:        u8,
	first_name:  u8,
	last_name:   u8,
	job:         u8,
}

mission_adverbs := [?]string{"Swiftly", "Cautiously", "Effortlessly", "Vigorously", "Silently", "Reluctantly", "Rapidly", "Gracefully", "Mysteriously", "Precisely", "Fiercely", "Steadily", "Eagerly", "Curiously", "Quietly", "Boldly", "Patiently", "Carefully", "Relentlessly", "Eerily", "Unexpectedly", "Diligently", "Calmly"}
mission_adjectives := [?]string{"Resilient", "Mysterious", "Luminescent", "Vast", "Agile", "Intricate", "Ancient", "Formidable", "Elegant", "Futuristic", "Stealthy", "Versatile", "Rugged", "Peculiar", "Vibrant", "Nomadic", "Sophisticated", "Expansive", "Grim", "Innovative", "Intrepid", "Surreal", "Stoic"}
mission_nouns := [?]string{"Quantum Batteries", "Bio-enhancement Serums", "Nanobot Swarms", "Cryogenic Stasis Pods", "Plasma Rifles", "Terraforming Modules", "Holographic Projectors", "Fusion Cores", "Genetic Material Samples", "Cybernetic Implants", "Antimatter Containment Units", "Starship Components", "Exoskeleton Suits", "Medical Nanogel", "Encrypted Data Cores", "Alien Artifacts", "Subspace Communication Relays", "Atmospheric Stabilizers", "Portable Shield Generators", "Interstellar Navigation Charts", "Mind Interface Devices", "Graviton Manipulators", "Zero-Point Energy Cells"}
mission_first_names := [?]string{"Gorachan", "Samuli", "David", "Orin", "Lyra", "Jaxon", "Zara", "Talon", "Mira", "Kael", "Nova", "Vera", "Dax", "Seren", "Ryn", "Eris", "Kara", "Thorne", "Xen", "Isla", "Cade", "Nia", "Rook"}
mission_last_names := [?]string{"Valken", "Nex", "Kyre", "Korrin", "Rho", "Aethon", "Draven", "Elara", "Synn", "Voss", "Vael", "Kaelor", "Nyx", "Zhen", "Varek", "Raith", "Thorne", "Arvon", "Solis", "Vire", "Vantros", "Kevar", "Draylen"}
mission_jobs := [?]string{"Starship Engineer", "Quantum Physicist", "Terraforming Specialist", "Cybernetics Surgeon", "Galactic Diplomat", "Stellar Cartographer", "Bioinformatics Analyst", "Interstellar Trader", "Artificial Intelligence Ethicist", "Astrobiologist", "Nanotechnology Architect", "Exo-Law Enforcement Officer", "Gravity Manipulator", "Planetary Governor", "Holographic Artist", "Space Miner", "Drone Operator", "Energy Harvesting Technician", "Virtual Reality Designer", "Time Dilation Theorist", "Cloning Technician", "Subspace Communications Officer", "Genetic Enhancement Specialist"}

// "Swiftly Resilient Quantum Batteries"
mission_item_name :: proc(m: Mission) -> Long_Text {
	return long_join(mission_adverbs[m.adverb], " ", mission_adjectives[m.adjective], " ", mission_nouns[m.noun])
}

// "Gorachan Valken the Starship Engineer"
mission_recipient :: proc(m: Mission) -> Long_Text {
	return long_join(mission_first_names[m.first_name], " ", mission_last_names[m.last_name], " the ", mission_jobs[m.job])
}

// ---- Reputation ----

// Adds `delta` to the planet, its star system and its faction, for both ends of a delivery, each group once
// (the two ends often share a system or a faction).
reputation_change :: proc(u: ^Universe, a, b: Planet_Id, delta: int) {
	planets: [2]Planet_Id = {a, b}
	systems: [2]Star_System_Id
	factions: [2]Faction_Id
	for p, i in planets {
		systems[i] = planet_get(u, p).star_system
		factions[i] = planet_get(u, p).faction
	}
	for i in 0 ..< 2 {
		if i == 1 && planets[1] == planets[0] {
			continue
		}
		planet_get(u, planets[i]).reputation += delta
	}
	for i in 0 ..< 2 {
		if i == 1 && systems[1] == systems[0] {
			continue
		}
		star_system_get(u, systems[i]).reputation += delta
	}
	for i in 0 ..< 2 {
		if i == 1 && factions[1] == factions[0] {
			continue
		}
		faction_get(u, factions[i]).reputation += delta
	}
}

// The lowest of the standings that count at a planet: the planet, its system and its faction.
worst_reputation :: proc(u: ^Universe, planet: Planet_Id) -> int {
	p := planet_get(u, planet)
	return min(p.reputation, star_system_get(u, p.star_system).reputation, faction_get(u, p.faction).reputation)
}

// ---- Offers ----

// Gives the dock a new delivery to offer: to another planet of the same faction. A faction with only one
// planet has nowhere to send it, so that dock offers nothing (the VB crashed here).
mission_generate :: proc(u: ^Universe, dock: Actor_Id) {
	origin := actor_get(u, dock).planet
	faction := planet_get(u, origin).faction
	candidates: [dynamic]Planet_Id
	defer delete(candidates)
	for p, i in u.planets {
		if p.faction == faction && Planet_Id(i + 1) != origin {
			append(&candidates, Planet_Id(i + 1))
		}
	}
	if len(candidates) == 0 {
		actor_get(u, dock).offer = 0
		return
	}
	m := Mission {
		origin      = origin,
		destination = rng_pick(&u.rng, candidates[:]),
		adverb      = u8(rng_below(&u.rng, len(mission_adverbs))),
		adjective   = u8(rng_below(&u.rng, len(mission_adjectives))),
		noun        = u8(rng_below(&u.rng, len(mission_nouns))),
		first_name  = u8(rng_below(&u.rng, len(mission_first_names))),
		last_name   = u8(rng_below(&u.rng, len(mission_last_names))),
		job         = u8(rng_below(&u.rng, len(mission_jobs))),
		reward      = dice_roll(&u.rng, MISSION_REWARD_DICE),
	}
	item := item_new(.Delivery)
	item.mission = m
	actor_get(u, dock).offer = item_add(u, item)
}

deliveries_carried :: proc(u: ^Universe) -> int {
	return inventory_count(u, .Delivery)
}

// With a negative standing you pay a deposit (half the reward, at least 1) and can carry no other deliveries.
needs_deposit :: proc(u: ^Universe, dock: Actor_Id) -> bool {
	return worst_reputation(u, actor_get(u, dock).planet) < 0
}

deposit_for :: proc(u: ^Universe, dock: Actor_Id, offer: Item_Id) -> int {
	if !needs_deposit(u, dock) {
		return 0
	}
	return max(1, item_get(u, offer).mission.reward / 2)
}

// How many deliveries you may be carrying already and still take another.
max_current_deliveries :: proc(u: ^Universe, dock: Actor_Id) -> int {
	if needs_deposit(u, dock) {
		return 0
	}
	return max(0, worst_reputation(u, actor_get(u, dock).planet) / REPUTATION_PER_EXTRA_DELIVERY)
}

can_add_delivery :: proc(u: ^Universe, dock: Actor_Id) -> bool {
	return deliveries_carried(u) <= max_current_deliveries(u, dock)
}

can_accept_mission :: proc(u: ^Universe, dock: Actor_Id) -> bool {
	offer := actor_get(u, dock).offer
	if offer == 0 || !can_add_delivery(u, dock) {
		return false
	}
	deposit := deposit_for(u, dock, offer)
	return deposit == 0 || u.avatar.jools >= deposit
}

// Takes the dock's delivery into the hold (paying any deposit, which is added to the reward so it comes back)
// and gives the dock a fresh one to offer.
mission_accept :: proc(u: ^Universe, dock: Actor_Id) -> bool {
	if !can_accept_mission(u, dock) {
		return false
	}
	offer := actor_get(u, dock).offer
	deposit := deposit_for(u, dock, offer)
	u.avatar.jools -= deposit
	item_get(u, offer).mission.reward += deposit
	append(&u.avatar.inventory, offer)
	mission_generate(u, dock)
	return true
}

// ---- Delivering and abandoning ----

// The deliveries in the hold that are bound for this dock's planet.
deliverable_here :: proc(u: ^Universe, dock: Actor_Id) -> (ids: [MAX_DELIVERABLES]Item_Id, count: int) {
	planet := actor_get(u, dock).planet
	for id in u.avatar.inventory {
		item := item_get(u, id)
		if item.kind == .Delivery && item.mission.destination == planet && count < MAX_DELIVERABLES {
			ids[count] = id
			count += 1
		}
	}
	return
}

MAX_DELIVERABLES :: 16

Completion :: struct {
	ids:        [MAX_DELIVERABLES]Item_Id,
	count:      int,
	jools:      int,
	reputation: int,
}

// Hands over everything bound for here: pays the rewards and raises reputation at both ends.
mission_complete :: proc(u: ^Universe, dock: Actor_Id) -> (done: Completion) {
	ids, count := deliverable_here(u, dock)
	for i in 0 ..< count {
		item := item_get(u, ids[i])^
		u.avatar.jools += item.mission.reward
		reputation_change(u, item.mission.origin, item.mission.destination, MISSION_REPUTATION_BONUS)
		done.jools += item.mission.reward
		done.reputation += MISSION_REPUTATION_BONUS
		for have, k in u.avatar.inventory {
			if have == ids[i] {
				ordered_remove(&u.avatar.inventory, k)
				break
			}
		}
		done.ids[done.count] = ids[i]
		done.count += 1
	}
	return
}

// Gives up a delivery: it is gone and reputation drops at both ends.
mission_abandon :: proc(u: ^Universe, id: Item_Id) {
	item := item_get(u, id)^
	assert(item.kind == .Delivery)
	reputation_change(u, item.mission.origin, item.mission.destination, MISSION_REPUTATION_PENALTY)
	for have, k in u.avatar.inventory {
		if have == id {
			ordered_remove(&u.avatar.inventory, k)
			return
		}
	}
}
