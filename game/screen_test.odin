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
