#+build !js
package game

import "core:testing"

fixed_seed :: proc() -> u64 {
	return 99
}

// Opens Embark and presses Go, leaving the app on the Generate screen.
start_generating :: proc(app: ^App) {
	app_init(app, fixed_seed)
	app_key(app, KEY_ENTER) // Embark
	app_key(app, KEY_ENTER) // Go
}

// Ticks until the Generate screen hands over, or fails the test if it never does.
tick_until_generated :: proc(t: ^testing.T, app: ^App) -> int {
	for ticks in 1 ..= 100_000 {
		app_tick(app)
		if _, still := stack_top(&app.stack)^.(Generate); !still {
			return ticks
		}
	}
	testing.fail_now(t, "generation never finished")
}

@(test)
go_starts_generation_on_the_first_tick :: proc(t: ^testing.T) {
	app: App
	start_generating(&app)
	defer app_destroy(&app)
	_, on_generate := stack_top(&app.stack)^.(Generate)
	testing.expect(t, on_generate)
	testing.expect(t, !app.session.generating) // nothing runs until the screen is ticked
	app_tick(&app)
	testing.expect(t, app.session.generating || app.session.in_play)
}

@(test)
generation_finishes_on_the_summary_with_a_universe :: proc(t: ^testing.T) {
	app: App
	start_generating(&app)
	defer app_destroy(&app)
	tick_until_generated(t, &app)
	_, on_summary := stack_top(&app.stack)^.(Universe_Summary)
	testing.expect(t, on_summary)
	testing.expect(t, app.session.in_play)
	testing.expect(t, !app.session.generating)
	u := &app.session.universe
	testing.expect(t, len(u.star_systems) > 0)
	testing.expect(t, u.avatar.actor != 0)
	// Embark, then the summary on top of the main menu: Replace swapped Generate out
	testing.expect_value(t, app.stack.count, 3)
}

@(test)
the_seed_comes_from_the_seed_source :: proc(t: ^testing.T) {
	a, b: App
	start_generating(&a)
	start_generating(&b)
	defer app_destroy(&a)
	defer app_destroy(&b)
	tick_until_generated(t, &a)
	tick_until_generated(t, &b)
	testing.expect_value(t, len(a.session.universe.star_systems), len(b.session.universe.star_systems))
	testing.expect(t, a.session.universe.star_systems[0].name == b.session.universe.star_systems[0].name)
	// and it is the same universe a direct run of that seed makes
	direct := generate(99)
	defer universe_destroy(&direct)
	testing.expect(t, direct.star_systems[0].name == a.session.universe.star_systems[0].name)
}

@(test)
generation_takes_several_ticks_but_always_progresses :: proc(t: ^testing.T) {
	app: App
	start_generating(&app)
	defer app_destroy(&app)
	app_tick(&app)
	done_before := app.session.generator.steps_done
	testing.expect(t, done_before >= 1)
	app_tick(&app)
	testing.expect(t, app.session.in_play || app.session.generator.steps_done > done_before)
}

@(test)
escape_cancels_generation_and_returns_to_embark :: proc(t: ^testing.T) {
	app: App
	start_generating(&app)
	defer app_destroy(&app)
	app_tick(&app)
	app_key(&app, KEY_ESCAPE)
	_, on_embark := stack_top(&app.stack)^.(Embark)
	testing.expect(t, on_embark)
	testing.expect(t, !app.session.generating)
	testing.expect(t, !app.session.in_play)
}

@(test)
escape_on_the_summary_discards_the_universe_and_resets_to_the_main_menu :: proc(t: ^testing.T) {
	app: App
	start_generating(&app)
	defer app_destroy(&app)
	tick_until_generated(t, &app)
	app_key(&app, KEY_ESCAPE)
	_, on_menu := stack_top(&app.stack)^.(Main_Menu)
	testing.expect(t, on_menu)
	testing.expect_value(t, app.stack.count, 1)
	testing.expect(t, !app.session.in_play)
}

@(test)
a_second_run_replaces_the_first :: proc(t: ^testing.T) {
	app: App
	start_generating(&app)
	defer app_destroy(&app)
	tick_until_generated(t, &app)
	app_key(&app, KEY_ESCAPE)
	app_key(&app, KEY_ENTER) // Embark again
	app_key(&app, KEY_ENTER) // Go
	tick_until_generated(t, &app)
	testing.expect(t, app.session.in_play)
}

@(test)
generate_screen_shows_progress_and_the_summary_shows_the_universe :: proc(t: ^testing.T) {
	app: App
	start_generating(&app)
	defer app_destroy(&app)
	app_tick(&app)
	// the title row and a lit progress bar cell exist while generating
	testing.expect_value(t, app.text[3][15].char, u8('G'))
	filled, empty := 0, 0
	for cell in app.text[15] {
		if cell.bg == .Green {filled += 1}
		if cell.bg == .Dark_Gray {empty += 1}
	}
	testing.expect_value(t, filled + empty, PROGRESS_BAR_WIDTH)
	tick_until_generated(t, &app)
	// summary: "Star systems" label at column 6 of row 4
	testing.expect_value(t, app.text[4][6].char, u8('S'))
	testing.expect(t, app.text[4][24].char >= '0' && app.text[4][24].char <= '9')
}

@(test)
text_put_int_handles_zero_negative_and_large :: proc(t: ^testing.T) {
	tb: Text_Buffer
	text_clear(&tb)
	next := text_put_int(&tb, 0, 0, 0)
	testing.expect_value(t, next, 1)
	testing.expect_value(t, tb[0][0].char, u8('0'))
	next = text_put_int(&tb, 0, 1, -42)
	testing.expect_value(t, next, 3)
	testing.expect_value(t, tb[1][0].char, u8('-'))
	testing.expect_value(t, tb[1][2].char, u8('2'))
	next = text_put_int(&tb, 0, 2, 1234567)
	testing.expect_value(t, next, 7)
	testing.expect_value(t, tb[2][6].char, u8('7'))
}
