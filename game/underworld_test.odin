#+build !js
package game

import "core:testing"

dock_of :: proc(u: ^Universe, planet: Planet_Id) -> Actor_Id {
	for a, i in u.actors {
		if a.kind == .Star_Dock && a.planet == planet {
			return Actor_Id(i + 1)
		}
	}
	return 0
}

rough_planet_ids :: proc(u: ^Universe) -> (ids: [dynamic]Planet_Id) {
	for _, i in u.planets {
		if planet_is_rough(u, Planet_Id(i + 1)) {
			append(&ids, Planet_Id(i + 1))
		}
	}
	return
}

// Runs the quest up to `stage` using the rules directly. Returns the fixer's and contact's docks.
play_quest_to :: proc(u: ^Universe, stage: Quest_Stage) -> (fixer_dock, contact_dock: Actor_Id) {
	rough := rough_planet_ids(u)
	defer delete(rough)
	start := dock_of(u, rough[0])
	if stage == .None {
		return
	}
	quest_advance(u, start)
	fixer_dock = dock_of(u, u.avatar.quest.fixer)
	if stage == .Lead {
		return
	}
	quest_advance(u, fixer_dock)
	contact_dock = dock_of(u, u.avatar.quest.contact)
	if stage == .Package {
		return
	}
	quest_advance(u, contact_dock)
	if stage == .Goods {
		return
	}
	u.avatar.cargo[.Narcotics] = GOODS_REQUIRED
	quest_advance(u, contact_dock)
	if stage == .Prove {
		return
	}
	quest_note_fight(u)
	if stage == .Proven {
		return
	}
	quest_advance(u, contact_dock)
	return
}

@(test)
rough_planets_exist_but_sigmo_is_never_one :: proc(t: ^testing.T) {
	for seed in 1 ..= 5 {
		u := generate(u64(seed))
		defer universe_destroy(&u)
		rough := rough_planet_ids(&u)
		defer delete(rough)
		testing.expect(t, len(rough) >= 2, "the quest needs two rough planets")
		testing.expect(t, len(rough) < len(u.planets))
		for id in rough {
			testing.expect(t, planet_get(&u, id).faction != SIGMO_FACTION)
		}
	}
}

@(test)
the_offer_follows_the_quest :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	rough := rough_planet_ids(&u)
	defer delete(rough)
	first := dock_of(&u, rough[0])
	label, ok := underworld_offer(&u, first)
	testing.expect(t, ok)
	testing.expect_value(t, label, "Ask Around")
	home := dock_of(&u, u.avatar.home_planet)
	_, ok = underworld_offer(&u, home) // SIGMO's home is not a rough place
	testing.expect(t, !ok)

	fixer, contact := play_quest_to(&u, .Lead)
	_ = contact
	label, ok = underworld_offer(&u, fixer)
	testing.expect_value(t, label, "Meet The Fixer")
	testing.expect(t, ok)
	_, ok = underworld_offer(&u, first)
	testing.expect(t, !ok || first == fixer)
}

@(test)
the_whole_quest_connects_you :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	rough := rough_planet_ids(&u)
	defer delete(rough)
	start := dock_of(&u, rough[0])
	jools := u.avatar.jools

	testing.expect_value(t, quest_advance(&u, start), Quest_Result.Rumor)
	testing.expect_value(t, u.avatar.quest.stage, Quest_Stage.Lead)
	testing.expect(t, planet_is_rough(&u, u.avatar.quest.fixer))
	testing.expect(t, u.avatar.quest.fixer != rough[0])

	fixer := dock_of(&u, u.avatar.quest.fixer)
	testing.expect_value(t, quest_advance(&u, fixer), Quest_Result.Package_Given)
	testing.expect(t, package_in_hold(&u) != 0)
	testing.expect(t, planet_is_rough(&u, u.avatar.quest.contact))
	testing.expect(t, u.avatar.quest.contact != u.avatar.quest.fixer)

	contact := dock_of(&u, u.avatar.quest.contact)
	testing.expect_value(t, quest_advance(&u, contact), Quest_Result.Package_Delivered)
	testing.expect_value(t, package_in_hold(&u), Item_Id(0))
	testing.expect_value(t, u.avatar.infamy, INFAMY_PACKAGE)

	u.avatar.cargo[.Narcotics] = 4
	testing.expect_value(t, quest_advance(&u, contact), Quest_Result.Goods_Short)
	u.avatar.cargo[.Weapons] = 8
	testing.expect_value(t, quest_advance(&u, contact), Quest_Result.Goods_Delivered)
	testing.expect_value(t, u.avatar.cargo[.Narcotics], 0) // narcotics first
	testing.expect_value(t, u.avatar.cargo[.Weapons], 2)
	testing.expect_value(t, u.avatar.quest.stage, Quest_Stage.Prove)

	testing.expect_value(t, quest_advance(&u, contact), Quest_Result.Not_Proven)
	quest_note_fight(&u)
	testing.expect_value(t, u.avatar.quest.stage, Quest_Stage.Proven)
	infamy := u.avatar.infamy
	testing.expect_value(t, quest_advance(&u, contact), Quest_Result.Connected)
	testing.expect_value(t, u.avatar.quest.stage, Quest_Stage.Connected)
	testing.expect_value(t, u.avatar.infamy, infamy + INFAMY_CONNECTED)
	testing.expect_value(t, u.avatar.jools, jools + CONNECTION_GIFT)
	_, ok := underworld_offer(&u, contact)
	testing.expect(t, !ok)
}

@(test)
a_fight_only_counts_while_you_are_proving_yourself :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	quest_note_fight(&u)
	testing.expect_value(t, u.avatar.quest.stage, Quest_Stage.None)
	play_quest_to(&u, .Prove)
	ship := ship_of(&u, false)
	c := combat_start(&u, ship)
	combat_victory(&u, c)
	testing.expect_value(t, u.avatar.quest.stage, Quest_Stage.Proven)
}

@(test)
connecting_opens_a_black_market_on_every_rough_planet :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	testing.expect_value(t, count_actors(&u, .Black_Market), 0)
	play_quest_to(&u, .Connected)
	rough := rough_planet_ids(&u)
	defer delete(rough)
	testing.expect_value(t, count_actors(&u, .Black_Market), len(rough))
	for id in rough {
		orbit := planet_orbit(&u, id)
		found := 0
		for a in map_get(&u, orbit).actors {
			if actor_get(&u, a).kind == .Black_Market {
				found += 1
			}
		}
		testing.expect_value(t, found, 1)
	}
}

@(test)
the_package_is_never_taken_by_patrols :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	clear(&u.avatar.inventory)
	play_quest_to(&u, .Package)
	testing.expect(t, package_in_hold(&u) != 0)
	testing.expect_value(t, takeable_count(&u), 0)
	c := combat_start(&u, ship_of(&u, false))
	combat_defeat(&u, c)
	testing.expect(t, package_in_hold(&u) != 0)
}

@(test)
black_markets_pay_better_and_ignore_the_law :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	faction_get(&u, 2).authority = 80 // bans narcotics
	testing.expect(t, good_banned_at(&u, planet, .Narcotics))
	for good in Good {
		testing.expect(t, black_sell_tenths(&u, planet, good) > sell_tenths(&u, planet, good))
		testing.expect(t, black_buy_tenths(&u, planet, good) <= buy_tenths(&u, planet, good))
	}
	u.avatar.jools = 10000
	bought, _ := goods_buy(&u, planet, .Narcotics, 5)
	testing.expect_value(t, bought, 0) // the law
	bought, _ = goods_buy(&u, planet, .Narcotics, 5, black = true)
	testing.expect_value(t, bought, 5) // the shadows
	sold, _ := goods_sell(&u, planet, .Narcotics, 5, black = true)
	testing.expect_value(t, sold, 5)
	testing.expect(t, u.avatar.infamy > 0)
	// a bad name pays more
	base := black_sell_tenths(&u, planet, .Gems)
	u.avatar.infamy = 1000
	testing.expect(t, black_sell_tenths(&u, planet, .Gems) > base)
}

@(test)
connected_rough_docks_offer_shady_deliveries :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	play_quest_to(&u, .Connected)
	rough := rough_planet_ids(&u)
	defer delete(rough)
	dock := dock_of(&u, rough[0])
	offer := item_get(&u, actor_get(&u, dock).offer)
	testing.expect(t, offer.mission.criminal)
	testing.expect(t, planet_is_rough(&u, offer.mission.destination))
	testing.expect(t, offer.mission.destination != rough[0])
	name := mission_item_name(offer.mission)
	testing.expect(t, long_str(&name)[:6] == "Shady ")
	// ordinary docks still offer ordinary errands
	home := dock_of(&u, u.avatar.home_planet)
	if home != 0 {
		testing.expect(t, !item_get(&u, actor_get(&u, home).offer).mission.criminal)
	}
}

@(test)
a_shady_delivery_pays_in_infamy_not_reputation_and_is_contraband :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	play_quest_to(&u, .Connected)
	rough := rough_planet_ids(&u)
	defer delete(rough)
	origin, dest := rough[0], rough[1]
	item := item_new(.Delivery)
	item.mission = {origin = origin, destination = dest, reward = 150, criminal = true}
	id := item_add(&u, item)
	append(&u.avatar.inventory, id)
	units, value := contraband_units(&u, SIGMO_FACTION)
	testing.expect(t, units >= 1 && value >= 150)

	infamy, jools := u.avatar.infamy, u.avatar.jools
	rep := planet_get(&u, dest).reputation
	done := mission_complete(&u, dock_of(&u, dest))
	testing.expect_value(t, done.count, 1)
	testing.expect_value(t, u.avatar.jools, jools + 150)
	testing.expect_value(t, u.avatar.infamy, infamy + INFAMY_SHADY_DELIVERY)
	testing.expect_value(t, planet_get(&u, dest).reputation, rep)

	// and a patrol can take one
	id2 := item_add(&u, item)
	append(&u.avatar.inventory, id2)
	ship := ship_of(&u, true)
	actor_get(&u, ship).faction = SIGMO_FACTION
	before := len(u.avatar.inventory)
	avatar_surrender_contraband(&u, ship)
	testing.expect_value(t, len(u.avatar.inventory), before - 1)
}

@(test)
the_underworld_through_the_screens :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	rough := rough_planet_ids(u)
	defer delete(rough)
	dock := dock_of(u, rough[0])
	actor_relocate(u, u.avatar.actor, actor_get(u, dock).map_id, {1, 1})
	dir := park_beside(t, u, dock)
	keys := [Direction]Key{.North = KEY_UP, .East = KEY_RIGHT, .South = KEY_DOWN, .West = KEY_LEFT}
	app_key(&app, keys[dir])
	testing.expect(t, on_screen(&app, Interaction_Screen))
	list, n := interactions_for(u, u.avatar.bumped)
	index := -1
	for i in 0 ..< n {
		if list[i] == .Underworld_Contact {
			index = i
		}
	}
	testing.expect(t, index >= 0)
	for _ in 0 ..< index {
		app_key(&app, KEY_DOWN)
	}
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.quest.stage, Quest_Stage.Lead)
}

@(test)
the_black_market_opens_a_market_screen :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	play_quest_to(u, .Connected)
	market := first_of_kind_on(u, .Black_Market)
	actor_relocate(u, u.avatar.actor, actor_get(u, market).map_id, {1, 1})
	dir := park_beside(t, u, market)
	keys := [Direction]Key{.North = KEY_UP, .East = KEY_RIGHT, .South = KEY_DOWN, .West = KEY_LEFT}
	app_key(&app, keys[dir])
	testing.expect(t, on_screen(&app, Interaction_Screen))
	app_key(&app, KEY_ENTER) // Trade In The Shadows
	screen, ok := stack_top(&app.stack)^.(Market_Screen)
	testing.expect(t, ok && screen.black)
}

@(test)
a_saved_game_keeps_its_quest_and_black_markets :: proc(t: ^testing.T) {
	u := generate(2)
	defer universe_destroy(&u)
	play_quest_to(&u, .Connected)
	u.avatar.infamy = 33
	data := universe_to_bytes(&u)
	defer delete(data)
	loaded, err := universe_from_bytes(data)
	testing.expect_value(t, err, Load_Error.None)
	defer universe_destroy(&loaded)
	testing.expect_value(t, loaded.avatar.quest, u.avatar.quest)
	testing.expect_value(t, loaded.avatar.infamy, 33)
	testing.expect_value(t, count_actors(&loaded, .Black_Market), count_actors(&u, .Black_Market))
}

@(test)
the_messages_fit_the_screen :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	with_longest_names(u)
	u.avatar.quest.fixer, u.avatar.quest.contact = 1, 2
	for result in Quest_Result {
		fits(t, "quest message", underworld_message(u, result), &app)
	}
	play_quest_to(u, .Connected)
	post := first_post(u)
	fits(t, "black market", Market_Screen{post = post, black = true}, &app)
	for good in Good {
		fits(t, "black good", Good_Trade{post = post, good = good, black = true, note = long_join("Bought 99999 for 99999999.")}, &app)
	}
}
