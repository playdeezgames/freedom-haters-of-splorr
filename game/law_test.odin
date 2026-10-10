#+build !js
package game

import "core:testing"

@(test)
sigmo_bans_the_most :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	testing.expect_value(t, faction_bans(faction_get(&u, SIGMO_FACTION)^, SIGMO_FACTION), Goods_Set{.Narcotics, .Weapons, .Gems})
}

@(test)
a_factions_traits_decide_its_bans :: proc(t: ^testing.T) {
	ban :: proc(f: Faction) -> Goods_Set {
		return faction_bans(f, 2)
	}
	testing.expect_value(t, ban({authority = 10, standards = 50}), Goods_Set{})
	testing.expect_value(t, ban({authority = 50}), Goods_Set{.Narcotics})
	testing.expect_value(t, ban({authority = 70}), Goods_Set{.Narcotics, .Weapons})
	// the free and the martial keep their arms, the free their vices
	testing.expect_value(t, ban({authority = 70, values = {.Martial_Honor}}), Goods_Set{.Narcotics})
	testing.expect_value(t, ban({authority = 70, values = {.Sovereign_Freedom}}), Goods_Set{})
	testing.expect_value(t, ban({authority = 80, values = {.Sovereign_Freedom}}), Goods_Set{.Narcotics})
	testing.expect_value(t, ban({authority = 10, values = {.Absolute_Order}}), Goods_Set{.Narcotics})
	testing.expect_value(t, ban({authority = 10, values = {.Sustainable_Harmony}}), Goods_Set{.Narcotics, .Weapons})
	testing.expect_value(t, ban({authority = 10, standards = 30, values = {.Collective_Prosperity}}), Goods_Set{.Gems})
	testing.expect_value(t, ban({authority = 10, standards = 70, values = {.Collective_Prosperity}}), Goods_Set{})
}

@(test)
banned_goods_cannot_be_traded_at_that_factions_planets :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	faction_get(&u, 2).authority = 80 // bans narcotics
	u.avatar.jools = 10000
	testing.expect(t, good_banned_at(&u, planet, .Narcotics))
	testing.expect_value(t, goods_max_buy(&u, planet, .Narcotics), 0)
	bought, _ := goods_buy(&u, planet, .Narcotics, 5)
	testing.expect_value(t, bought, 0)
	u.avatar.cargo[.Narcotics] = 5
	sold, _ := goods_sell(&u, planet, .Narcotics, 5)
	testing.expect_value(t, sold, 0)
	testing.expect_value(t, u.avatar.cargo[.Narcotics], 5)
	bought, _ = goods_buy(&u, planet, .Food, 5) // food is fine
	testing.expect_value(t, bought, 5)
}

@(test)
contraband_depends_on_who_is_asking :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	u.avatar.cargo = {}
	u.avatar.cargo[.Narcotics] = 10
	u.avatar.cargo[.Gems] = 3
	units, value := contraband_units(&u, SIGMO_FACTION)
	testing.expect_value(t, units, 13)
	testing.expect_value(t, value, 10 * good_info[.Narcotics].base_price + 3 * good_info[.Gems].base_price)
	faction_get(&u, 2).authority, faction_get(&u, 2).values = 0, {}
	units, _ = contraband_units(&u, 2)
	testing.expect_value(t, units, 0)
	testing.expect_value(t, search_fine(&u, SIGMO_FACTION), FINE_PER_VALUE * value)
}

@(test)
infamy_makes_patrols_less_friendly :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	stranger := actor_get(&u, ship_of(&u, false))^
	faction_get(&u, stranger.faction).reputation = 75 // great standing: Friendly
	testing.expect_value(t, ship_disposition(&u, stranger), Disposition.Friendly)
	u.avatar.infamy = INFAMY_WARY
	testing.expect_value(t, ship_disposition(&u, stranger), Disposition.Neutral)
	u.avatar.infamy = INFAMY_WANTED
	testing.expect_value(t, ship_disposition(&u, stranger), Disposition.Hostile)
	// your own faction's love can outweigh a bad name
	own := actor_get(&u, ship_of(&u, true))^
	testing.expect_value(t, ship_disposition(&u, own), Disposition.Friendly)
}

@(test)
patrols_search_you_unless_they_are_friends :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	u.avatar.cargo = {}
	u.avatar.cargo[.Narcotics] = 4
	// a stranger who hates you
	stranger := ship_of(&u, false)
	faction_get(&u, actor_get(&u, stranger).faction).authority = 90 // so it bans narcotics too
	bring_alongside(t, &u, stranger)
	_, contact := patrol_contact(&u)
	testing.expect_value(t, contact, Contact.Search)
	// a neutral one searches as well
	faction_get(&u, actor_get(&u, stranger).faction).reputation = 25
	_, contact = patrol_contact(&u)
	testing.expect_value(t, contact, Contact.Search)
	// a friend looks away
	faction_get(&u, actor_get(&u, stranger).faction).reputation = 75
	_, contact = patrol_contact(&u)
	testing.expect_value(t, contact, Contact.Hail)
	// and nothing aboard means no search
	faction_get(&u, actor_get(&u, stranger).faction).reputation = 25
	u.avatar.cargo = {}
	_, contact = patrol_contact(&u)
	testing.expect_value(t, contact, Contact.Hail)
}

@(test)
handing_over_the_goods_keeps_the_rest :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	u.avatar.cargo = {}
	u.avatar.cargo[.Narcotics] = 4
	u.avatar.cargo[.Weapons] = 2
	u.avatar.cargo[.Food] = 9
	ship := ship_of(&u, true)
	actor_get(&u, ship).faction = SIGMO_FACTION
	units := avatar_surrender_contraband(&u, ship)
	testing.expect_value(t, units, 6)
	testing.expect_value(t, u.avatar.cargo[.Narcotics], 0)
	testing.expect_value(t, u.avatar.cargo[.Food], 9)
	testing.expect_value(t, u.avatar.infamy, INFAMY_CAUGHT)
	testing.expect(t, actor_get(&u, ship).calm_until > u.turn)
}

@(test)
a_bribe_keeps_the_goods_and_costs_infamy :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	u.avatar.cargo = {}
	u.avatar.cargo[.Weapons] = 5
	u.avatar.jools = 1000
	ship := ship_of(&u, true)
	actor_get(&u, ship).faction = SIGMO_FACTION
	fine := avatar_bribe(&u, ship)
	testing.expect_value(t, fine, FINE_PER_VALUE * 5 * good_info[.Weapons].base_price)
	testing.expect_value(t, u.avatar.jools, 1000 - fine)
	testing.expect_value(t, u.avatar.cargo[.Weapons], 5)
	testing.expect_value(t, u.avatar.infamy, INFAMY_BRIBE)
}

@(test)
selling_shady_goods_earns_a_name :: proc(t: ^testing.T) {
	u := generate(1)
	defer universe_destroy(&u)
	planet := plain_planet(&u)
	u.avatar.cargo[.Narcotics] = 11
	u.avatar.cargo[.Food] = 50
	goods_sell(&u, planet, .Food, 50)
	testing.expect_value(t, u.avatar.infamy, 0)
	goods_sell(&u, planet, .Narcotics, 11)
	testing.expect_value(t, u.avatar.infamy, 3) // 11 units: three lots of five, rounded up
}

@(test)
fighting_patrols_adds_to_infamy :: proc(t: ^testing.T) {
	u := generate(8)
	defer universe_destroy(&u)
	ship := enemy_with_tech(t, &u, 0)
	c := combat_start(&u, ship)
	combat_victory(&u, c)
	testing.expect_value(t, u.avatar.infamy, INFAMY_KILL)
	u.avatar.infamy = 0
	other := ship_of(&u, false)
	u.avatar.fuel.current = u.avatar.fuel.maximum
	for _ in 0 ..< 60 {
		c2 := combat_start(&u, other)
		u.avatar.hull.current = BASE_HULL
		if combat_round(&u, &c2, .Flee).outcome == .Escaped {
			break
		}
	}
	testing.expect_value(t, u.avatar.infamy, INFAMY_FLEE)
}

@(test)
a_search_through_the_screens :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	u.avatar.cargo[.Narcotics] = 6
	u.avatar.jools = 1000
	ship := enemy_with_tech(t, u, 0)
	faction_get(u, actor_get(u, ship).faction).authority = 90
	u.turn += 1
	app_tick(&app)
	testing.expect(t, on_screen(&app, Search_Screen))
	app_key(&app, KEY_ESCAPE) // no escaping
	testing.expect(t, on_screen(&app, Search_Screen))
	app_key(&app, KEY_ENTER) // hand it over
	testing.expect(t, on_screen(&app, Message))
	testing.expect_value(t, u.avatar.cargo[.Narcotics], 0)
	app_key(&app, KEY_ENTER)
	testing.expect(t, on_screen(&app, Navigation))
}

@(test)
a_poor_smuggler_cannot_bribe :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	u.avatar.cargo = {}
	u.avatar.cargo[.Gems] = 50
	u.avatar.jools = 5
	list, n := search_choices(&u, SIGMO_FACTION)
	for i in 0 ..< n {
		testing.expect(t, list[i] != .Pay_Fine)
	}
}

@(test)
surrendering_in_a_fight_goes_back_to_the_search :: proc(t: ^testing.T) {
	u := generate(6)
	defer universe_destroy(&u)
	u.avatar.cargo = {}
	ship := ship_of(&u, true)
	actor_get(&u, ship).faction = SIGMO_FACTION
	_, is_contact := contact_screen_for(&u, ship).(Contact_Screen)
	testing.expect(t, is_contact)
	u.avatar.cargo[.Narcotics] = 1
	_, is_search := contact_screen_for(&u, ship).(Search_Screen)
	testing.expect(t, is_search)
}

@(test)
the_pedia_lists_a_factions_banned_goods :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	d: Doc
	pedia_page_doc(&app.session.universe, .Faction, int(SIGMO_FACTION), &d)
	found := false
	for i in 0 ..< d.count {
		found ||= d.lines[i].text == " - Narcotics"
	}
	testing.expect(t, found)
}
