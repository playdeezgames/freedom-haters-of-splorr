package game

// Keys arrive from the platform as an i32. Printable ASCII is passed as its character code;
// everything else uses the small values below. web/game.js must map to these same numbers.

Key :: distinct i32

KEY_NONE :: Key(0)
KEY_UP :: Key(1)
KEY_DOWN :: Key(2)
KEY_LEFT :: Key(3)
KEY_RIGHT :: Key(4)
KEY_ENTER :: Key(5)
KEY_ESCAPE :: Key(6)
KEY_TAB :: Key(7)
KEY_BACKSPACE :: Key(8)
