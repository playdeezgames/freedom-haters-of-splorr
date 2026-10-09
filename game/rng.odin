package game

// Deterministic random numbers. Universes, saves and tests depend on the exact sequence, so this is
// a fixed algorithm (splitmix64) rather than whatever core:math/rand does in a given Odin release.
// Changing it changes every universe a seed produces.

Rng :: struct {
	state: u64,
}

rng_make :: proc(seed: u64) -> Rng {
	return {state = seed}
}

rng_u64 :: proc(r: ^Rng) -> u64 {
	r.state += 0x9E3779B97F4A7C15
	z := r.state
	z = (z ~ (z >> 30)) * 0xBF58476D1CE4E5B9
	z = (z ~ (z >> 27)) * 0x94D049BB133111EB
	return z ~ (z >> 31)
}

// Uniform in [0, n). n must be positive. Rejects the biased tail so every value is equally likely.
rng_below :: proc(r: ^Rng, n: int) -> int {
	assert(n > 0)
	limit := u64(n)
	threshold := (0 - limit) % limit
	for {
		x := rng_u64(r)
		if x >= threshold {
			return int(x % limit)
		}
	}
}

// Uniform in [lo, hi], both ends included.
rng_range :: proc(r: ^Rng, lo, hi: int) -> int {
	assert(hi >= lo)
	return lo + rng_below(r, hi - lo + 1)
}

// A uniformly chosen element. `items` must not be empty.
rng_pick :: proc(r: ^Rng, items: []$T) -> T {
	return items[rng_below(r, len(items))]
}

// Picks a key of an enum-indexed weight table, e.g. star_type_weights[.Young]. Zero weights are never
// picked; at least one weight must be positive.
rng_weighted :: proc(r: ^Rng, weights: [$E]int) -> E {
	total := 0
	for w in weights {
		total += w
	}
	assert(total > 0)
	roll := rng_below(r, total)
	for w, e in weights {
		roll -= w
		if roll < 0 {
			return e
		}
	}
	unreachable()
}

// ---- Dice ----
//
// Notation, as in the VB: terms joined by '+', each `[-]XdY` optionally followed by `/D` or `*M`.
// A negative count subtracts the roll ("10d11+-10d1" is 10d11 minus 10d1). Each term's roll is scaled
// with integer math, (roll * M) / D, truncating toward zero. Empty text is 0.

MAX_DICE_TERMS :: 4

Dice_Term :: struct {
	count: int, // negative subtracts
	size:  int,
	mult:  int,
	div:   int,
}

Dice :: struct {
	terms: [MAX_DICE_TERMS]Dice_Term,
	count: int,
}

dice_parse :: proc(text: string) -> (dice: Dice, ok: bool) {
	i := 0
	n := len(text)
	for i < n && text[i] == ' ' {
		i += 1
	}
	if i == n {
		return dice, true
	}
	for {
		if dice.count == MAX_DICE_TERMS {
			return {}, false
		}
		term := Dice_Term {
			mult = 1,
			div  = 1,
		}
		sign := 1
		if i < n && text[i] == '-' {
			sign = -1
			i += 1
		}
		count: int
		count, i = parse_digits(text, i) or_return
		term.count = sign * count
		if i >= n || (text[i] != 'd' && text[i] != 'D') {
			return {}, false
		}
		term.size, i = parse_digits(text, i + 1) or_return
		if term.size < 1 {
			return {}, false
		}
		if i < n && (text[i] == '/' || text[i] == '*') {
			op := text[i]
			scale: int
			scale, i = parse_digits(text, i + 1) or_return
			if scale < 1 {
				return {}, false
			}
			if op == '/' {
				term.div = scale
			} else {
				term.mult = scale
			}
		}
		dice.terms[dice.count] = term
		dice.count += 1
		if i == n {
			return dice, true
		}
		if text[i] != '+' {
			return {}, false
		}
		i += 1
	}
}

@(private = "file")
parse_digits :: proc(text: string, start: int) -> (value: int, next: int, ok: bool) {
	i := start
	for i < len(text) && text[i] >= '0' && text[i] <= '9' {
		value = value * 10 + int(text[i] - '0')
		if value > 1_000_000 {
			return 0, i, false
		}
		i += 1
	}
	return value, i, i > start
}

dice_roll_parsed :: proc(r: ^Rng, dice: Dice) -> int {
	total := 0
	for k in 0 ..< dice.count {
		t := dice.terms[k]
		roll := 0
		for _ in 0 ..< abs(t.count) {
			roll += rng_range(r, 1, t.size)
		}
		if t.count < 0 {
			roll = -roll
		}
		total += (roll * t.mult) / t.div
	}
	return total
}

// Smallest and largest possible results of a dice expression.
dice_bounds :: proc(dice: Dice) -> (lowest, highest: int) {
	for k in 0 ..< dice.count {
		t := dice.terms[k]
		lo, hi := abs(t.count), abs(t.count) * t.size
		if t.count < 0 {
			lo, hi = -hi, -lo
		}
		lowest += (lo * t.mult) / t.div
		highest += (hi * t.mult) / t.div
	}
	return
}

// Parses and rolls. Notation is compile-time data in this codebase, so a bad string is a bug and asserts.
dice_roll :: proc(r: ^Rng, text: string) -> int {
	dice, ok := dice_parse(text)
	assert(ok, "bad dice notation")
	return dice_roll_parsed(r, dice)
}
