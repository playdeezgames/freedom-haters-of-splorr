package game

CELL_SIZE :: 8
SCREEN_WIDTH :: TEXT_COLUMNS * CELL_SIZE
SCREEN_HEIGHT :: TEXT_ROWS * CELL_SIZE
BYTES_PER_PIXEL :: 4

Frame :: [SCREEN_WIDTH * SCREEN_HEIGHT * BYTES_PER_PIXEL]u8

// Rasterizes the text buffer into an RGBA frame.
render :: proc(tb: ^Text_Buffer, frame: ^Frame) {
	for row, r in tb {
		for cell, c in row {
			glyph := font[cell.char]
			fg := palette[cell.fg]
			bg := palette[cell.bg]
			for py in 0 ..< CELL_SIZE {
				y := r * CELL_SIZE + py
				for px in 0 ..< CELL_SIZE {
					x := c * CELL_SIZE + px
					color := fg if (glyph >> uint(py * CELL_SIZE + px)) & 1 == 1 else bg
					i := (y * SCREEN_WIDTH + x) * BYTES_PER_PIXEL
					frame[i] = color.r
					frame[i + 1] = color.g
					frame[i + 2] = color.b
					frame[i + 3] = 0xFF
				}
			}
		}
	}
}
