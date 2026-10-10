package game

// What the player is in the middle of, shared by every screen: a universe being generated, or one being played.

Session :: struct {
	// Where a new universe's seed comes from; the platform provides real entropy. nil means a fixed seed.
	seed_source: proc() -> u64,
	storage:     Storage, // where saves and the embark settings live
	generator:   Generator,
	generating:  bool,
	universe:    Universe,
	in_play:     bool,
}

session_begin_generation :: proc(s: ^Session, settings: Embark_Settings) {
	assert(!s.generating)
	session_end(s)
	seed := u64(1)
	if s.seed_source != nil {
		seed = s.seed_source()
	}
	s.generator = generator_start(seed, settings)
	s.generating = true
}

session_cancel_generation :: proc(s: ^Session) {
	if s.generating {
		generator_destroy(&s.generator)
		s.generating = false
	}
}

session_finish_generation :: proc(s: ^Session) {
	assert(s.generating)
	s.universe = generator_finish(&s.generator)
	s.generating = false
	s.in_play = true
}

// Throws away whatever is in progress or in play.
session_end :: proc(s: ^Session) {
	session_cancel_generation(s)
	if s.in_play {
		universe_destroy(&s.universe)
		s.in_play = false
	}
}
