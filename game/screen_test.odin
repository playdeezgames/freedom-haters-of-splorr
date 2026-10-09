#+build !js
package game

import "core:testing"

@(test)
font_has_a_glyph_for_every_character :: proc(t: ^testing.T) {
	testing.expect_value(t, len(font), 256)
	testing.expect(t, font['A'] != 0)
	testing.expect_value(t, font[' '], u64(0))
}

@(test)
render_draws_foreground_and_background :: proc(t: ^testing.T) {
	tb: Text_Buffer
	text_clear(&tb)
	tb[0][0] = {'A', .White, .Blue}
	frame: Frame
	render(&tb, &frame)
	lit, dark: int
	for py in 0 ..< CELL_SIZE {
		for px in 0 ..< CELL_SIZE {
			i := (py * SCREEN_WIDTH + px) * BYTES_PER_PIXEL
			switch {
			case frame[i] == 0xFF && frame[i + 1] == 0xFF && frame[i + 2] == 0xFF:
				lit += 1
			case frame[i] == 0x00 && frame[i + 1] == 0x00 && frame[i + 2] == 0xAA:
				dark += 1
			}
		}
	}
	testing.expect(t, lit > 0)
	testing.expect_value(t, lit + dark, CELL_SIZE * CELL_SIZE)
}

@(test)
text_put_clips_at_the_edge :: proc(t: ^testing.T) {
	tb: Text_Buffer
	text_clear(&tb)
	next := text_put(&tb, TEXT_COLUMNS - 2, 0, "abcd")
	testing.expect_value(t, next, TEXT_COLUMNS + 2)
	testing.expect_value(t, tb[0][TEXT_COLUMNS - 1].char, u8('b'))
}

@(test)
wrapped_text_breaks_at_spaces :: proc(t: ^testing.T) {
	tb: Text_Buffer
	text_clear(&tb)
	rows := text_put_wrapped(&tb, 2, 3, 10, "one two three four")
	testing.expect_value(t, rows, 2)
	testing.expect_value(t, tb[3][2].char, u8('o')) // "one two"
	testing.expect_value(t, tb[3][8].char, u8('o'))
	testing.expect_value(t, tb[4][2].char, u8('t')) // "three four"
	testing.expect_value(t, tb[4][11].char, u8('r'))
	text_clear(&tb)
	testing.expect_value(t, text_put_wrapped(&tb, 0, 0, 4, "abcdefghij"), 3) // a word longer than the line
	testing.expect_value(t, text_put_wrapped(&tb, 0, 0, 10, ""), 0)
}

@(test)
long_menus_scroll_to_keep_the_cursor_in_view :: proc(t: ^testing.T) {
	labels: [30]string
	for i in 0 ..< len(labels) {
		labels[i] = "item"
	}
	tb: Text_Buffer
	text_clear(&tb)
	menu_draw(&tb, 4, labels[:], 0)
	testing.expect_value(t, tb[4][12].char, u8('>')) // cursor on the first row
	testing.expect_value(t, tb[3][12].char, u8(' ')) // nothing above
	testing.expect_value(t, tb[TEXT_ROWS - 2][12].char, u8(0x1F)) // more below
	text_clear(&tb)
	menu_draw(&tb, 4, labels[:], 29)
	cursor_rows := 0
	for row in 0 ..< TEXT_ROWS {
		if tb[row][12].char == '>' {
			cursor_rows += 1
			testing.expect(t, row <= TEXT_ROWS - 2)
		}
	}
	testing.expect_value(t, cursor_rows, 1) // the last entry is on screen
	testing.expect_value(t, tb[3][12].char, u8(0x1E)) // more above
}
