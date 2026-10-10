package game

// Trading goods at a post: a list of the goods with their prices, then one good at a time with buy and sell
// buttons. The planet's market is shared by all its posts.

post_planet :: proc(u: ^Universe, post: Actor_Id) -> Planet_Id {
	return actor_get(u, post).planet
}

// ---- The list ----

Market_Screen :: struct {
	post:   Actor_Id,
	cursor: int,
	black:  bool, // a black market: any good, better prices, no law
}

// The header: a trading post's, or the black market's.
draw_market_header :: proc(tb: ^Text_Buffer, u: ^Universe, post: Actor_Id, black: bool) {
	if !black {
		draw_post_header(tb, u, post)
		return
	}
	p := planet_get(u, actor_get(u, post).planet)
	c := text_put(tb, (TEXT_COLUMNS - len(name_str(&p.name)) - len(" Black Market")) / 2, 1, name_str(&p.name), .Magenta)
	text_put(tb, c, 1, " Black Market", .Magenta)
	put_field_int(tb, 2, 3, "Jools", u.avatar.jools)
}

market_draw :: proc(s: ^Market_Screen, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	draw_market_header(tb, u, s.post, s.black)
	text_put(tb, 2, 5, "Buy/Sell (you hold):", .Dark_Gray)
	planet := post_planet(u, s.post)
	texts: [len(Good) + 1]Long_Text
	labels: [len(Good) + 1]string
	texts[0] = long_join("Cancel")
	labels[0] = long_str(&texts[0])
	for good in Good {
		d3: [20]u8
		b1, b2: [24]u8
		i := int(good) + 1
		if s.black {
			texts[i] = long_join(good_info[good].name, " ", tenths_text(&b1, black_buy_tenths(u, planet, good)), "/", tenths_text(&b2, black_sell_tenths(u, planet, good)), " (x", int_text(&d3, u.avatar.cargo[good]), ")")
		} else if good_banned_at(u, planet, good) {
			texts[i] = long_join(good_info[good].name, " (banned) (x", int_text(&d3, u.avatar.cargo[good]), ")")
		} else {
			texts[i] = long_join(good_info[good].name, " ", tenths_text(&b1, buy_tenths(u, planet, good)), "/", tenths_text(&b2, sell_tenths(u, planet, good)), " (x", int_text(&d3, u.avatar.cargo[good]), ")")
		}
		labels[i] = long_str(&texts[i])
	}
	s.cursor = min(s.cursor, len(Good))
	menu_draw(tb, 7, labels[:], s.cursor, 2)
}

market_key :: proc(s: ^Market_Screen, key: Key, session: ^Session) -> Transition {
	switch menu_key(&s.cursor, len(Good) + 1, key) {
	case .Chosen:
		if s.cursor == 0 {
			return Pop{}
		}
		return Push{Good_Trade{post = s.post, good = Good(s.cursor - 1), black = s.black}}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- One good ----

Good_Trade :: struct {
	post:   Actor_Id,
	good:   Good,
	black:  bool,
	cursor: int,
	note:   Long_Text, // what the last button did
}

Good_Button :: enum {
	Buy_One,
	Buy_Ten,
	Buy_Most,
	Sell_One,
	Sell_Ten,
	Sell_All,
	Done,
}

good_button_labels :: proc(u: ^Universe, planet: Planet_Id, good: Good, black: bool, texts: ^[len(Good_Button)]Long_Text) {
	d1, d2: [20]u8
	texts[Good_Button.Buy_One] = long_join("Buy 1")
	texts[Good_Button.Buy_Ten] = long_join("Buy 10")
	texts[Good_Button.Buy_Most] = long_join("Buy As Many As Possible (", int_text(&d1, goods_max_buy(u, planet, good, black)), ")")
	texts[Good_Button.Sell_One] = long_join("Sell 1")
	texts[Good_Button.Sell_Ten] = long_join("Sell 10")
	texts[Good_Button.Sell_All] = long_join("Sell All (", int_text(&d2, u.avatar.cargo[good]), ")")
	texts[Good_Button.Done] = long_join("Done")
}

good_trade_draw :: proc(s: ^Good_Trade, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	planet := post_planet(u, s.post)
	draw_market_header(tb, u, s.post, s.black)
	text_put(tb, 2, 5, good_info[s.good].name, .White)
	banned := !s.black && good_banned_at(u, planet, s.good)
	if banned {
		law := faction_get(u, planet_get(u, planet).faction)
		text_put(tb, 2, 9, "Banned by the law of", .Light_Red)
		text_put(tb, 2, 10, name_str(&law.name), .Light_Red)
		menu_draw(tb, 12, []string{"Done"}, 0, 2)
		return
	}
	buy := black_buy_tenths(u, planet, s.good) if s.black else buy_tenths(u, planet, s.good)
	sell := black_sell_tenths(u, planet, s.good) if s.black else sell_tenths(u, planet, s.good)
	b1, b2: [24]u8
	c := put_field(tb, 2, 6, "Buy", tenths_text(&b1, buy))
	put_field(tb, c + 2, 6, "Sell", tenths_text(&b2, sell))
	c = put_field_int(tb, 2, 7, "Held", u.avatar.cargo[s.good])
	put_field_int(tb, c + 2, 7, "Weighs", good_info[s.good].weight)
	c = put_field_int(tb, 2, 8, "Cargo Weight", cargo_weight(u))
	text_put(tb, c, 8, " (+", .Light_Gray)
	b3: [24]u8
	c = text_put(tb, c + 3, 8, tenths_text(&b3, cargo_fuel_tenths(u)), .White)
	text_put(tb, c, 8, " fuel)", .Light_Gray)
	if s.note.len > 0 {
		text_put(tb, 2, 9, long_str(&s.note), .Orange)
	}
	texts: [len(Good_Button)]Long_Text
	labels: [len(Good_Button)]string
	good_button_labels(u, planet, s.good, s.black, &texts)
	for button in Good_Button {
		labels[button] = long_str(&texts[button])
	}
	menu_draw(tb, 11, labels[:], s.cursor, 2)
}

good_trade_key :: proc(s: ^Good_Trade, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	planet := post_planet(u, s.post)
	if !s.black && good_banned_at(u, planet, s.good) {
		if menu_key(&s.cursor, 1, key) != .None {
			return Pop{}
		}
		return nil
	}
	switch menu_key(&s.cursor, len(Good_Button), key) {
	case .Chosen:
		d1, d2: [20]u8
		button := Good_Button(s.cursor)
		switch button {
		case .Buy_One, .Buy_Ten, .Buy_Most:
			want := 1 if button == .Buy_One else 10 if button == .Buy_Ten else goods_max_buy(u, planet, s.good, s.black)
			bought, cost := goods_buy(u, planet, s.good, want, s.black)
			if bought == 0 {
				s.note = long_join("You can't afford any.")
			} else {
				s.note = long_join("Bought ", int_text(&d1, bought), " for ", int_text(&d2, cost), ".")
			}
		case .Sell_One, .Sell_Ten, .Sell_All:
			want := 1 if button == .Sell_One else 10 if button == .Sell_Ten else u.avatar.cargo[s.good]
			sold, earned := goods_sell(u, planet, s.good, want, s.black)
			if sold == 0 {
				s.note = long_join("You have none to sell.")
			} else {
				s.note = long_join("Sold ", int_text(&d1, sold), " for ", int_text(&d2, earned), ".")
			}
		case .Done:
			return Pop{}
		}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}
