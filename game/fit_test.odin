#+build !js
package game

import "core:fmt"
import "core:testing"

// Gives everything the longest names the generators can make (24 characters), so a screen that only fits
// short names shows up as clipped text.
with_longest_names :: proc(u: ^Universe) {
	long :: proc(c: u8) -> Name {
		n: Name
		n.len = 24
		for i in 0 ..< 24 {
			n.buf[i] = c
		}
		return n
	}
	for &f, i in u.factions {
		f.name = long(u8('F' + i % 3))
	}
	for &s in u.star_systems {
		s.name = long('S')
	}
	for &p in u.planets {
		p.name = long('P')
	}
	for &s in u.satellites {
		s.name = long('M')
	}
}

// Draws a screen and fails the test if any text fell off the edge.
fits :: proc(t: ^testing.T, what: string, screen: Screen, app: ^App) {
	before := text_clipped
	s := screen
	tb: Text_Buffer
	text_clear(&tb)
	screen_draw(&s, &tb, &app.session)
	testing.expectf(t, text_clipped == before, "%s: %d characters ran off the screen", what, text_clipped - before)
	text_clipped = before
}

@(test)
screens_fit_the_forty_columns_with_the_longest_names :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	with_longest_names(u)

	dock := home_dock(u)
	post := first_post(u)
	yard := first_yard(u)
	star: Actor_Id
	for a, i in u.actors {
		if a.kind == .Star {
			star = Actor_Id(i + 1)
			break
		}
	}
	// something of every kind in the hold, and an offer to look at
	for kind in Item_Kind {
		if kind == .Delivery {
			continue
		}
		if item_info[kind].marked {
			for mark in 1 ..= MAX_MARK {
				in_hold(u, kind, mark)
			}
		} else {
			in_hold(u, kind)
		}
	}
	a, b, _ := far_apart(u)
	delivery := carry(u, a, b, 100)
	u.avatar.jools = 123456

	fits(t, "status", Status_Screen{}, &app)
	fits(t, "pedia menu", Pedia_Menu{}, &app)
	for kind in Pedia_Kind {
		fits(t, "pedia list", Pedia_List{kind = kind}, &app)
		fits(t, "pedia filtered list", Pedia_List{kind = kind, filter = {0 = 'P'}, filter_len = 1}, &app)
		count := len(u.factions) if kind == .Faction else len(u.star_systems) if kind == .Star_System else len(u.planets) if kind == .Planet else len(u.satellites)
		for id in 1 ..= min(count, 40) {
			fits(t, "pedia page", Pedia_Page{kind = kind, id = id}, &app)
		}
	}
	fits(t, "faction's planets", Pedia_List{kind = .Planet, scope = .Faction, scope_id = 2}, &app)
	fits(t, "system's factions", Pedia_List{kind = .Faction, scope = .Star_System, scope_id = 1}, &app)
	fits(t, "planet's satellites", Pedia_List{kind = .Satellite, scope = .Planet, scope_id = 1}, &app)
	fits(t, "inventory", Inventory_Screen{}, &app)
	fits(t, "equipment", Equipment_Screen{}, &app)
	fits(t, "action menu", Action_Menu{}, &app)
	fits(t, "mission offer", Mission_Offer{dock = dock}, &app)
	fits(t, "confirm abandon", Confirm_Abandon_Delivery{item = delivery}, &app)
	fits(t, "trader", Trader{post = post}, &app)
	fits(t, "buy list", Buy_List{post = post}, &app)
	fits(t, "sell list", Sell_List{post = post}, &app)
	fits(t, "shipyard", Shipyard_Screen{yard = yard}, &app)
	for slot in Equip_Slot {
		fits(t, "slot items", Slot_Items{yard = yard, slot = slot}, &app)
	}
	post_tech := post_tech_level(u, post)
	_ = post_tech
	for kind in Item_Kind {
		marks := []int{0}
		if item_info[kind].marked {
			marks = []int{1, 2, 3, 4, 5}
		}
		for mark in marks {
			id := delivery if kind == .Delivery else 0
			fits(t, "item page", Item_Page{kind = kind, mark = mark, count = 99, item = id}, &app)
			choice := Trade_Choice{post = post, mode = .Buy, kind = kind, mark = mark}
			fits(t, "quantity", Quantity{choice = choice}, &app)
			fits(t, "number", Number_Entry{choice = choice, value = 123456, limit = 123456}, &app)
			fits(t, "confirm buy", Confirm_Trade{choice = choice, quantity = 123456}, &app)
			choice.mode = .Sell
			fits(t, "confirm sell", Confirm_Trade{choice = choice, quantity = 123456}, &app)
		}
	}
	// what you can bump into, in every kind of place
	for a, i in u.actors {
		if a.map_id == 0 {
			continue
		}
		u.avatar.bumped = Actor_Id(i + 1)
		fits(t, fmt.tprintf("interaction with a %v", a.kind), Interaction_Screen{}, &app)
	}
	u.avatar.bumped = Map_Edge{u.galaxy}
	fits(t, "leave", Interaction_Screen{}, &app)
	_ = star
	fits(t, "game over", Game_Over{}, &app)
	u.avatar.jools = 123456789
	u.turn = 123456789
	for slot in 0 ..< SLOT_COUNT {
		testing.expect_value(t, slot_save(app.session.storage, slot, u), Save_Result.Saved)
	}
	fits(t, "game menu", Game_Menu{}, &app)
	fits(t, "save", Save_Screen{}, &app)
	fits(t, "load", Load_Screen{}, &app)
	fits(t, "main menu with saves", Main_Menu{}, &app)
	fits(t, "navigation", Navigation{message = .Out_Of_Fuel}, &app)
}

@(test)
messages_with_the_longest_lines_fit :: proc(t: ^testing.T) {
	app: App
	app_on_the_map(t, &app)
	defer app_destroy(&app)
	u := &app.session.universe
	with_longest_names(u)
	a, b, dock := far_apart(u)
	carry(u, a, b, 100)
	carry(u, a, b, 100)
	done := mission_complete(u, dock)
	fits(t, "completion receipt", completion_message(u, done), &app)
	m := item_get(u, u.actors[int(dock) - 1].offer).mission
	_ = m
	fits(t, "accepted", accepted_message(u, Mission{origin = a, destination = b, reward = 100}), &app)
	yard_change := Change{result = .Done, removed = u.avatar.equipment[.Life_Support], installed = u.avatar.equipment[.Fuel_Supply], fee = 99999}
	fits(t, "shipyard receipt", change_message(u, .Life_Support, yard_change), &app)
}
