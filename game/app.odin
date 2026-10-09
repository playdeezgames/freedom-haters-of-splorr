package game

// Platform-independent app state. The platform layer feeds it keys and asks it to draw.

App :: struct {
	text:     Text_Buffer,
	last_key: Key,
	keys:     int,
}

app_init :: proc(app: ^App) {
	app^ = {}
	app_draw(app)
}

app_key :: proc(app: ^App, key: Key) {
	app.last_key = key
	app.keys += 1
	app_draw(app)
}

app_draw :: proc(app: ^App) {
	tb := &app.text
	text_clear(tb)
	text_put_centered(tb, 4, "FREEDOM HATERS OF SPLORR!!", .Yellow)
	text_put_centered(tb, 6, "Love FREEDOM or DIE!", .Light_Red)
	text_put_centered(tb, 12, "(odin / wasm toolchain check)", .Dark_Gray)
	text_put(tb, 2, 22, "keys pressed:", .Light_Gray)
	text_put_int(tb, 16, 22, app.keys, .White)
	text_put(tb, 2, 23, "last key code:", .Light_Gray)
	text_put_int(tb, 17, 23, int(app.last_key), .White)
}

text_put_int :: proc(tb: ^Text_Buffer, col, row: int, n: int, fg: Hue = .Light_Gray) {
	buf: [20]u8
	i := len(buf)
	v := n
	neg := v < 0
	if neg {
		v = -v
	}
	for {
		i -= 1
		buf[i] = u8('0' + v % 10)
		v /= 10
		if v == 0 {
			break
		}
	}
	if neg {
		i -= 1
		buf[i] = '-'
	}
	text_put(tb, col, row, string(buf[i:]), fg)
}
