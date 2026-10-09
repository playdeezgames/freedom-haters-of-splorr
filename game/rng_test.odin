#+build !js
package game

import "core:testing"

@(test)
sequence_is_pinned :: proc(t: ^testing.T) {
	// Values computed independently (splitmix64, seed 1). If this fails, every saved seed changed meaning.
	r := rng_make(1)
	testing.expect_value(t, rng_u64(&r), u64(0x910a2dec89025cc1))
	testing.expect_value(t, rng_u64(&r), u64(0xbeeb8da1658eec67))
	testing.expect_value(t, rng_u64(&r), u64(0xf893a2eefb32555e))
}

@(test)
same_seed_same_numbers_different_seed_differs :: proc(t: ^testing.T) {
	a, b, c := rng_make(42), rng_make(42), rng_make(43)
	same, differ := true, false
	for _ in 0 ..< 100 {
		x, y, z := rng_range(&a, 0, 1000), rng_range(&b, 0, 1000), rng_range(&c, 0, 1000)
		same &&= x == y
		differ ||= x != z
	}
	testing.expect(t, same)
	testing.expect(t, differ)
}

@(test)
range_includes_both_ends_and_stays_inside :: proc(t: ^testing.T) {
	r := rng_make(7)
	seen: [6]bool
	for _ in 0 ..< 1000 {
		v := rng_range(&r, 1, 6)
		testing.expect(t, v >= 1 && v <= 6)
		seen[v - 1] = true
	}
	for s in seen {
		testing.expect(t, s)
	}
	testing.expect_value(t, rng_range(&r, 5, 5), 5)
	testing.expect_value(t, rng_range(&r, -3, -3), -3)
	for _ in 0 ..< 200 {
		v := rng_range(&r, -2, 2)
		testing.expect(t, v >= -2 && v <= 2)
	}
}

@(test)
pick_returns_members :: proc(t: ^testing.T) {
	r := rng_make(3)
	items := []string{"a", "b", "c"}
	counts: map[string]int
	defer delete(counts)
	for _ in 0 ..< 300 {
		counts[rng_pick(&r, items)] += 1
	}
	testing.expect_value(t, len(counts), 3)
}

@(test)
weighted_follows_the_weights :: proc(t: ^testing.T) {
	r := rng_make(11)
	table := [Galactic_Age]int {
		.Young   = 1,
		.Average = 0,
		.Old     = 3,
	}
	counts: [Galactic_Age]int
	for _ in 0 ..< 4000 {
		counts[rng_weighted(&r, table)] += 1
	}
	testing.expect_value(t, counts[.Average], 0)
	// expect about 1000 / 3000; allow generous slack
	testing.expect(t, counts[.Young] > 800 && counts[.Young] < 1200)
	testing.expect(t, counts[.Old] > 2800 && counts[.Old] < 3200)
}

@(test)
dice_parse_accepts_the_notation_in_use :: proc(t: ^testing.T) {
	for text in ([]string{"3d4", "4d6", "5d20", "12d6/6", "1d4/4", "3d6/8", "2d6+-2d1", "10d11+-10d1", "2D6", "1d6*10"}) {
		_, ok := dice_parse(text)
		testing.expectf(t, ok, "%q should parse", text)
	}
}

@(test)
dice_parse_rejects_nonsense :: proc(t: ^testing.T) {
	for text in ([]string{"d6", "3d", "3x4", "3d0", "3d6/0", "3d6/", "abc", "3d6+", "3d6 4", "1d1+1d1+1d1+1d1+1d1"}) {
		_, ok := dice_parse(text)
		testing.expectf(t, !ok, "%q should not parse", text)
	}
}

@(test)
empty_dice_is_zero :: proc(t: ^testing.T) {
	r := rng_make(1)
	testing.expect_value(t, dice_roll(&r, ""), 0)
	testing.expect_value(t, dice_roll(&r, "  "), 0)
}

@(test)
dice_bounds_match_the_original_expressions :: proc(t: ^testing.T) {
	check :: proc(t: ^testing.T, text: string, want_lo, want_hi: int) {
		dice, ok := dice_parse(text)
		testing.expect(t, ok)
		lo, hi := dice_bounds(dice)
		testing.expectf(t, lo == want_lo && hi == want_hi, "%q: got %d..%d want %d..%d", text, lo, hi, want_lo, want_hi)
	}
	check(t, "3d4", 3, 12)
	check(t, "12d6/6", 2, 12) // debris per star system
	check(t, "1d4/4", 0, 1) // shipyards / star gates per planet orbit
	check(t, "3d6/8", 0, 2) // trading posts (the original then takes at least 1)
	check(t, "2d6+-2d1", 0, 10) // tech level
	check(t, "10d11+-10d1", 0, 100) // faction Authority/Standards/Conviction
	check(t, "1d6*10", 10, 60)
}

@(test)
rolls_stay_inside_their_bounds_and_hit_both_ends :: proc(t: ^testing.T) {
	r := rng_make(99)
	for text in ([]string{"3d4", "2d6+-2d1", "1d4/4"}) {
		dice, _ := dice_parse(text)
		lo, hi := dice_bounds(dice)
		hit_lo, hit_hi := false, false
		for _ in 0 ..< 20000 {
			v := dice_roll_parsed(&r, dice)
			testing.expectf(t, v >= lo && v <= hi, "%q rolled %d outside %d..%d", text, v, lo, hi)
			hit_lo ||= v == lo
			hit_hi ||= v == hi
		}
		testing.expectf(t, hit_lo && hit_hi, "%q never reached both %d and %d", text, lo, hi)
	}
}

@(test)
big_pools_stay_inside_their_bounds :: proc(t: ^testing.T) {
	// 12d6/6 can't be expected to reach its extremes (6^-12), but must never leave its bounds
	r := rng_make(21)
	dice, _ := dice_parse("12d6/6")
	lo, hi := dice_bounds(dice)
	for _ in 0 ..< 20000 {
		v := dice_roll_parsed(&r, dice)
		testing.expect(t, v >= lo && v <= hi)
	}
}

@(test)
negative_terms_subtract :: proc(t: ^testing.T) {
	r := rng_make(5)
	// 10d1 always rolls 10, so 10d1+-10d1 is exactly 0 and 3d1+-1d1 is exactly 2
	testing.expect_value(t, dice_roll(&r, "10d1+-10d1"), 0)
	testing.expect_value(t, dice_roll(&r, "3d1+-1d1"), 2)
}
