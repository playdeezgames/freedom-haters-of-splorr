package game

// Shared cursor-menu behavior for screens that just choose from a list.

Menu_Result :: enum {
	None,
	Chosen,
	Cancelled,
	Previous, // Left: for rows whose value can be changed
	Next, // Right
}

// Up/Down move the cursor (wrapping), Enter chooses it, Escape cancels.
menu_key :: proc(cursor: ^int, count: int, key: Key) -> Menu_Result {
	switch key {
	case KEY_UP:
		if count > 0 {
			cursor^ = (cursor^ + count - 1) % count
		}
	case KEY_DOWN:
		if count > 0 {
			cursor^ = (cursor^ + 1) % count
		}
	case KEY_ENTER:
		if count > 0 {
			return .Chosen
		}
	case KEY_ESCAPE:
		return .Cancelled
	case KEY_LEFT:
		return .Previous
	case KEY_RIGHT:
		return .Next
	}
	return .None
}

// Items are drawn every other row: the 8x8 font has no line gap, so adjacent rows touch.
menu_draw :: proc(tb: ^Text_Buffer, top_row: int, labels: []string, cursor: int) {
	for label, i in labels {
		if i == cursor {
			text_put(tb, 12, top_row + i * 2, "> ", .Yellow)
			text_put(tb, 14, top_row + i * 2, label, .White)
		} else {
			text_put(tb, 14, top_row + i * 2, label, .Light_Gray)
		}
	}
}
