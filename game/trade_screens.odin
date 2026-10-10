package game

// The trading post: Trader (Buy / Sell) -> a list of items -> how many -> (a number, if "Specific") -> confirm.
// After a confirmed trade the stack pops back to the list it came from, as the VB does.

Trade_Mode :: enum {
	Buy,
	Sell,
}

Trade_Choice :: struct {
	post: Actor_Id,
	mode: Trade_Mode,
	kind: Item_Kind,
	mark: int,
}

// A line of text built from parts, longer than a Name.
Long_Text :: struct {
	buf: [96]u8,
	len: int,
}

long_join :: proc(parts: ..string) -> (t: Long_Text) {
	for part in parts {
		take := min(len(part), len(t.buf) - t.len)
		copy(t.buf[t.len:], part[:take])
		t.len += take
	}
	return
}

long_str :: proc(t: ^Long_Text) -> string {
	return string(t.buf[:t.len])
}

draw_post_header :: proc(tb: ^Text_Buffer, u: ^Universe, post: Actor_Id) {
	p := planet_get(u, actor_get(u, post).planet)
	c := text_put(tb, (TEXT_COLUMNS - len(name_str(&p.name)) - len(" Trading Post")) / 2, 1, name_str(&p.name), .Cyan)
	text_put(tb, c, 1, " Trading Post", .Cyan)
	put_field_int(tb, 2, 3, "Jools", u.avatar.jools)
}

// ---- Trader ----

Trader :: struct {
	post:   Actor_Id,
	cursor: int,
}

Trader_Action :: enum {
	Buy,
	Sell,
	Goods,
}

trader_actions :: proc(u: ^Universe, post: Actor_Id) -> (list: [3]Trader_Action, count: int) {
	if trade_prices(u, post).count > 0 {
		list[count] = .Buy
		count += 1
	}
	if trade_offers(u, post).count > 0 {
		list[count] = .Sell
		count += 1
	}
	list[count] = .Goods
	count += 1
	return
}

trader_draw :: proc(s: ^Trader, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	draw_post_header(tb, u, s.post)
	list, n := trader_actions(u, s.post)
	labels: [4]string
	for i in 0 ..< n {
		labels[i] = "Buy" if list[i] == .Buy else "Sell" if list[i] == .Sell else "Trade Goods"
	}
	labels[n] = "Leave"
	menu_draw(tb, 8, labels[:n + 1], s.cursor)
}

trader_key :: proc(s: ^Trader, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	list, n := trader_actions(u, s.post)
	switch menu_key(&s.cursor, n + 1, key) {
	case .Chosen:
		if s.cursor >= n {
			return Pop{}
		}
		switch list[s.cursor] {
		case .Buy:
			return Push{Buy_List{post = s.post}}
		case .Sell:
			return Push{Sell_List{post = s.post}}
		case .Goods:
			return Push{Market_Screen{post = s.post}}
		}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- Buy list ----

Buy_List :: struct {
	post:   Actor_Id,
	cursor: int,
}

buy_labels :: proc(u: ^Universe, post: Actor_Id, labels: ^[MAX_TRADE_ITEMS + 1]string, names: ^[MAX_TRADE_ITEMS]Name) -> (list: Trade_List, count: int) {
	list = trade_prices(u, post)
	labels[0] = "Cancel"
	for i in 0 ..< list.count {
		it := list.items[i]
		base := item_name(item_new(it.kind, it.mark))
		d1, d2: [20]u8
		names[i] = name_join(name_str(&base), " @", int_text(&d1, trade_unit_price(it.kind, it.mark)), " (x", int_text(&d2, inventory_count(u, it.kind, it.mark)), ")")
		labels[i + 1] = name_str(&names[i])
	}
	return list, list.count + 1
}

buy_list_draw :: proc(s: ^Buy_List, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	draw_post_header(tb, u, s.post)
	text_put(tb, 2, 5, "For sale (you have):", .Dark_Gray)
	labels: [MAX_TRADE_ITEMS + 1]string
	names: [MAX_TRADE_ITEMS]Name
	_, count := buy_labels(u, s.post, &labels, &names)
	s.cursor = min(s.cursor, count - 1)
	menu_draw(tb, 7, labels[:count], s.cursor, 2)
}

buy_list_key :: proc(s: ^Buy_List, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	labels: [MAX_TRADE_ITEMS + 1]string
	names: [MAX_TRADE_ITEMS]Name
	list, count := buy_labels(u, s.post, &labels, &names)
	switch menu_key(&s.cursor, count, key) {
	case .Chosen:
		if s.cursor == 0 {
			return Pop{}
		}
		it := list.items[s.cursor - 1]
		return Push{Quantity{choice = {post = s.post, mode = .Buy, kind = it.kind, mark = it.mark}}}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- Sell list ----

Sell_List :: struct {
	post:   Actor_Id,
	cursor: int,
}

sell_labels :: proc(u: ^Universe, post: Actor_Id, labels: ^[MAX_TRADE_ITEMS + 1]string, names: ^[MAX_TRADE_ITEMS]Name) -> (list: Trade_List, count: int) {
	list = trade_offers(u, post)
	labels[0] = "Cancel"
	for i in 0 ..< list.count {
		it := list.items[i]
		base := item_name(item_new(it.kind, it.mark))
		d1, d2: [20]u8
		names[i] = name_join(name_str(&base), " (x", int_text(&d1, inventory_count(u, it.kind, it.mark)), ") @", int_text(&d2, item_info[it.kind].offer))
		labels[i + 1] = name_str(&names[i])
	}
	return list, list.count + 1
}

sell_list_draw :: proc(s: ^Sell_List, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	draw_post_header(tb, u, s.post)
	text_put(tb, 2, 5, "They will buy:", .Dark_Gray)
	labels: [MAX_TRADE_ITEMS + 1]string
	names: [MAX_TRADE_ITEMS]Name
	_, count := sell_labels(u, s.post, &labels, &names)
	s.cursor = min(s.cursor, count - 1)
	menu_draw(tb, 7, labels[:count], s.cursor, 2)
}

sell_list_key :: proc(s: ^Sell_List, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	labels: [MAX_TRADE_ITEMS + 1]string
	names: [MAX_TRADE_ITEMS]Name
	list, count := sell_labels(u, s.post, &labels, &names)
	switch menu_key(&s.cursor, count, key) {
	case .Chosen:
		if s.cursor == 0 {
			return Pop{}
		}
		it := list.items[s.cursor - 1]
		return Push{Quantity{choice = {post = s.post, mode = .Sell, kind = it.kind, mark = it.mark}}}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// Once the last of something is sold there is nothing left to list: go back to the trader.
sell_list_tick :: proc(s: ^Sell_List, session: ^Session) -> Transition {
	if trade_offers(&session.universe, s.post).count == 0 {
		return Pop{}
	}
	return nil
}

// ---- How many ----

Quantity :: struct {
	choice: Trade_Choice,
	cursor: int,
}

Quantity_Option :: enum {
	Cancel,
	One,
	Maximum, // "Maximum" when buying, "All" when selling
	Half,
	Specific,
}

Quantity_Entry :: struct {
	option: Quantity_Option,
	amount: int,
}

quantity_limit :: proc(u: ^Universe, c: Trade_Choice) -> int {
	if c.mode == .Buy {
		return trade_max_buy(u, c.kind, c.mark)
	}
	return inventory_count(u, c.kind, c.mark)
}

quantity_entries :: proc(u: ^Universe, c: Trade_Choice, entries: ^[len(Quantity_Option)]Quantity_Entry) -> (count: int) {
	limit := quantity_limit(u, c)
	add :: proc(entries: ^[len(Quantity_Option)]Quantity_Entry, count: ^int, option: Quantity_Option, amount: int) {
		entries[count^] = {option, amount}
		count^ += 1
	}
	add(entries, &count, .Cancel, 0)
	if limit > 0 {
		add(entries, &count, .One, 1)
	}
	if limit > 1 {
		add(entries, &count, .Maximum, limit)
	}
	if c.mode == .Sell && limit > 3 {
		add(entries, &count, .Half, limit / 2)
	}
	if limit > 2 {
		add(entries, &count, .Specific, 0)
	}
	return
}

quantity_labels :: proc(c: Trade_Choice, entries: []Quantity_Entry, labels: ^[len(Quantity_Option)]string, names: ^[len(Quantity_Option)]Name) {
	for e, i in entries {
		d: [20]u8
		switch e.option {
		case .Cancel:
			labels[i] = "Cancel"
		case .One:
			labels[i] = "One (1)"
		case .Maximum:
			names[i] = name_join("Maximum (" if c.mode == .Buy else "All (", int_text(&d, e.amount), ")")
			labels[i] = name_str(&names[i])
		case .Half:
			names[i] = name_join("Half (", int_text(&d, e.amount), ")")
			labels[i] = name_str(&names[i])
		case .Specific:
			labels[i] = "Specific number..."
		}
	}
}

quantity_draw :: proc(s: ^Quantity, tb: ^Text_Buffer, session: ^Session) {
	u := &session.universe
	draw_post_header(tb, u, s.choice.post)
	item := item_new(s.choice.kind, s.choice.mark)
	name := item_name(item)
	text_put(tb, 2, 5, "Buy how many?" if s.choice.mode == .Buy else "Sell how many?", .Yellow)
	if s.choice.mode == .Buy {
		c := text_put(tb, 2, 7, name_str(&name), .White)
		c = text_put(tb, c, 7, " @", .Light_Gray)
		text_put_int(tb, c, 7, trade_unit_price(s.choice.kind, s.choice.mark), .White)
	} else {
		put_field_int(tb, 2, 7, name_str(&name), inventory_count(u, s.choice.kind, s.choice.mark), .White)
	}
	entries: [len(Quantity_Option)]Quantity_Entry
	count := quantity_entries(u, s.choice, &entries)
	if count == 1 {
		text_put(tb, 2, 9, "You can't afford any.", .Light_Red)
	}
	labels: [len(Quantity_Option)]string
	names: [len(Quantity_Option)]Name
	quantity_labels(s.choice, entries[:count], &labels, &names)
	menu_draw(tb, 11, labels[:count], s.cursor)
}

quantity_key :: proc(s: ^Quantity, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	entries: [len(Quantity_Option)]Quantity_Entry
	count := quantity_entries(u, s.choice, &entries)
	switch menu_key(&s.cursor, count, key) {
	case .Chosen:
		e := entries[s.cursor]
		switch e.option {
		case .Cancel:
			return Pop{}
		case .One, .Maximum, .Half:
			return Push{Confirm_Trade{choice = s.choice, quantity = e.amount, depth = 2}}
		case .Specific:
			return Push{Number_Entry{choice = s.choice, value = 1, limit = quantity_limit(u, s.choice)}}
		}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}

// ---- A specific number ----
// Up and down change it by 1, left and right by 10, so the touch pad can reach any number;
// typed digits work too.

Number_Entry :: struct {
	choice: Trade_Choice,
	value:  int,
	limit:  int,
	typing: bool, // once digits are being typed they build on each other; the first one replaces the starting 1
}

number_entry_draw :: proc(s: ^Number_Entry, tb: ^Text_Buffer, session: ^Session) {
	draw_post_header(tb, &session.universe, s.choice.post)
	name := item_name(item_new(s.choice.kind, s.choice.mark))
	text_put(tb, 2, 5, "Buy how many?" if s.choice.mode == .Buy else "Sell how many?", .Yellow)
	text_put(tb, 2, 7, name_str(&name), .White)
	text_put_centered(tb, 11, "<   >", .Dark_Gray)
	digits: [20]u8
	text := int_text(&digits, s.value)
	text_put(tb, (TEXT_COLUMNS - len(text)) / 2, 11, text, .Light_Cyan)
	text_put(tb, 2, 14, "Up/Down: 1   Left/Right: 10", .Dark_Gray)
	put_field_int(tb, 2, 16, "At most", s.limit)
	text_put_centered(tb, 22, "Enter: OK", .Dark_Gray)
}

number_entry_key :: proc(s: ^Number_Entry, key: Key, session: ^Session) -> Transition {
	switch {
	case key == KEY_UP:
		s.value += 1
		s.typing = false
	case key == KEY_DOWN:
		s.value -= 1
		s.typing = false
	case key == KEY_RIGHT:
		s.value += 10
		s.typing = false
	case key == KEY_LEFT:
		s.value -= 10
		s.typing = false
	case key == KEY_BACKSPACE:
		s.value /= 10
		s.typing = true
	case key >= '0' && key <= '9':
		s.value = (s.value * 10 if s.typing else 0) + int(key - '0')
		s.typing = true
	case key == KEY_ENTER:
		if s.value >= 1 {
			return Push{Confirm_Trade{choice = s.choice, quantity = s.value, depth = 3}}
		}
	case key == KEY_ESCAPE:
		return Pop{}
	}
	s.value = clamp(s.value, 1, max(s.limit, 1))
	return nil
}

// ---- Confirm ----

Confirm_Trade :: struct {
	choice:   Trade_Choice,
	quantity: int,
	depth:    int, // screens to pop to get back to the list
	cursor:   int,
}

confirm_trade_text :: proc(s: ^Confirm_Trade) -> Long_Text {
	name := item_name(item_new(s.choice.kind, s.choice.mark))
	d1, d2: [20]u8
	if s.choice.mode == .Buy {
		total := trade_unit_price(s.choice.kind, s.choice.mark) * s.quantity
		return long_join("Buy ", int_text(&d1, s.quantity), " ", name_str(&name), " for ", int_text(&d2, total), " Jools?")
	}
	return long_join("Sell ", int_text(&d1, s.quantity), " ", name_str(&name), " for ", int_text(&d2, trade_offer_total(s.choice.kind, s.quantity)), " Jools?")
}

confirm_trade_labels := [?]string{"Yes", "No"}

confirm_trade_draw :: proc(s: ^Confirm_Trade, tb: ^Text_Buffer, session: ^Session) {
	draw_post_header(tb, &session.universe, s.choice.post)
	text := confirm_trade_text(s)
	text_put_wrapped(tb, 2, 6, TEXT_COLUMNS - 4, long_str(&text), .White)
	menu_draw(tb, 12, confirm_trade_labels[:], s.cursor)
}

confirm_trade_key :: proc(s: ^Confirm_Trade, key: Key, session: ^Session) -> Transition {
	u := &session.universe
	switch menu_key(&s.cursor, len(confirm_trade_labels), key) {
	case .Chosen:
		if s.cursor == 0 {
			if s.choice.mode == .Buy {
				trade_buy(u, s.choice.kind, s.choice.mark, s.quantity)
			} else {
				trade_sell(u, s.choice.kind, s.quantity)
			}
			return Pop_Count{s.depth}
		}
		return Pop{}
	case .Cancelled:
		return Pop{}
	case .None, .Previous, .Next:
	}
	return nil
}
