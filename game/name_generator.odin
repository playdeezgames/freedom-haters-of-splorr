package game

// Pronounceable names for star systems, planets and satellites: 3d4 alternating consonant and vowel
// pieces ("Zhekath"), never the same name twice in one universe.

Name_Generator :: struct {
	used: map[Name]struct{},
}

name_generator_destroy :: proc(ng: ^Name_Generator) {
	delete(ng.used)
	ng^ = {}
}

vowel_pieces := [?]string{"a", "e", "i", "o", "u"}
consonant_pieces := [?]string{"b", "ch", "d", "f", "g", "k", "l", "m", "n", "p", "r", "s", "sh", "t", "th", "v", "z", "zh"}

name_candidate :: proc(r: ^Rng) -> (n: Name) {
	is_vowel := rng_below(r, 2) == 1
	for _ in 0 ..< dice_roll(r, "3d4") {
		piece := rng_pick(r, vowel_pieces[:] if is_vowel else consonant_pieces[:])
		n = name_join(name_str(&n), piece)
		is_vowel = !is_vowel
	}
	if n.len > 0 && n.buf[0] >= 'a' && n.buf[0] <= 'z' {
		n.buf[0] -= 'a' - 'A'
	}
	return
}

name_unique :: proc(ng: ^Name_Generator, r: ^Rng) -> Name {
	for {
		n := name_candidate(r)
		if n not_in ng.used {
			ng.used[n] = {}
			return n
		}
	}
}
