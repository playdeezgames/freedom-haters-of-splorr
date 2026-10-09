package game

// The whole UI is a 40x25 grid of colored characters, drawn with the 8x8 font.

TEXT_COLUMNS :: 40
TEXT_ROWS :: 25

Hue :: enum u8 {
	Black,
	Blue,
	Green,
	Cyan,
	Red,
	Magenta,
	Brown,
	Light_Gray,
	Dark_Gray,
	Light_Blue,
	Light_Green,
	Light_Cyan,
	Light_Red,
	Light_Magenta,
	Yellow,
	White,
	// Not CGA: the original planet types are told apart by these three.
	Orange,
	Pink,
	Tan,
}

// CGA palette plus Orange, Pink and Tan
palette := [Hue][3]u8 {
	.Black         = {0x00, 0x00, 0x00},
	.Blue          = {0x00, 0x00, 0xAA},
	.Green         = {0x00, 0xAA, 0x00},
	.Cyan          = {0x00, 0xAA, 0xAA},
	.Red           = {0xAA, 0x00, 0x00},
	.Magenta       = {0xAA, 0x00, 0xAA},
	.Brown         = {0xAA, 0x55, 0x00},
	.Light_Gray    = {0xAA, 0xAA, 0xAA},
	.Dark_Gray     = {0x55, 0x55, 0x55},
	.Light_Blue    = {0x55, 0x55, 0xFF},
	.Light_Green   = {0x55, 0xFF, 0x55},
	.Light_Cyan    = {0x55, 0xFF, 0xFF},
	.Light_Red     = {0xFF, 0x55, 0x55},
	.Light_Magenta = {0xFF, 0x55, 0xFF},
	.Yellow        = {0xFF, 0xFF, 0x55},
	.White         = {0xFF, 0xFF, 0xFF},
	.Orange        = {0xFF, 0xAA, 0x00},
	.Pink          = {0xFF, 0x88, 0xD0},
	.Tan           = {0xD7, 0xAF, 0x87},
}

Cell :: struct {
	char: u8,
	fg:   Hue,
	bg:   Hue,
}

Text_Buffer :: [TEXT_ROWS][TEXT_COLUMNS]Cell

text_clear :: proc(tb: ^Text_Buffer, bg: Hue = .Black) {
	for &row in tb {
		for &cell in row {
			cell = {' ', .Light_Gray, bg}
		}
	}
}

// Writes `s` starting at (col,row); clips at the right edge. Returns the column after the last character.
text_put :: proc(tb: ^Text_Buffer, col, row: int, s: string, fg: Hue = .Light_Gray, bg: Hue = .Black) -> int {
	if row < 0 || row >= TEXT_ROWS {
		return col
	}
	c := col
	for i in 0 ..< len(s) {
		if c >= 0 && c < TEXT_COLUMNS {
			tb[row][c] = {s[i], fg, bg}
		}
		c += 1
	}
	return c
}

text_put_centered :: proc(tb: ^Text_Buffer, row: int, s: string, fg: Hue = .Light_Gray, bg: Hue = .Black) {
	text_put(tb, (TEXT_COLUMNS - len(s)) / 2, row, s, fg, bg)
}

// Writes a whole number (negatives get a leading '-') and returns the column after it.
text_put_int :: proc(tb: ^Text_Buffer, col, row: int, n: int, fg: Hue = .Light_Gray, bg: Hue = .Black) -> int {
	buf: [20]u8
	i := len(buf)
	v := abs(n)
	for {
		i -= 1
		buf[i] = u8('0' + v % 10)
		v /= 10
		if v == 0 {
			break
		}
	}
	if n < 0 {
		i -= 1
		buf[i] = '-'
	}
	return text_put(tb, col, row, string(buf[i:]), fg, bg)
}

// The first line of `s` wrapped to `width` columns (breaking at a space), and what is left.
text_wrap_next :: proc(s: string, width: int) -> (line, rest: string) {
	if len(s) <= width {
		return s, ""
	}
	take := width
	for take > 0 && s[take] != ' ' {
		take -= 1
	}
	if take == 0 { // one word longer than the line
		take = width
	}
	rest = s[take:]
	for len(rest) > 0 && rest[0] == ' ' {
		rest = rest[1:]
	}
	return s[:take], rest
}

// Writes `s` wrapped to `width` columns starting at (col,row). Returns the rows used.
text_put_wrapped :: proc(tb: ^Text_Buffer, col, row, width: int, s: string, fg: Hue = .Light_Gray) -> int {
	rows := 0
	rest := s
	for len(rest) > 0 {
		line: string
		line, rest = text_wrap_next(rest, width)
		text_put(tb, col, row + rows, line, fg)
		rows += 1
	}
	return rows
}
