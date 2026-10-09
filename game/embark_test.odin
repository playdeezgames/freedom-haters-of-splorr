#+build !js
package game

import "core:testing"

@(test)
defaults_match_the_original :: proc(t: ^testing.T) {
	d := DEFAULT_EMBARK_SETTINGS
	testing.expect_value(t, d.age, Galactic_Age.Average)
	testing.expect_value(t, d.density, Galactic_Density.Average)
	testing.expect_value(t, d.wealth, Starting_Wealth.Middle)
	testing.expect_value(t, d.faction_count, 4)
}

@(test)
wealth_rolls_match_the_original_ranges :: proc(t: ^testing.T) {
	testing.expect_value(t, wealth_profiles[.Very_Poor].first, 0)
	testing.expect_value(t, wealth_max_jools(.Very_Poor), 0)
	testing.expect_value(t, wealth_profiles[.Poor].first, 450)
	testing.expect_value(t, wealth_max_jools(.Poor), 549)
	testing.expect_value(t, wealth_profiles[.Middle].first, 900)
	testing.expect_value(t, wealth_max_jools(.Middle), 1098)
	testing.expect_value(t, wealth_profiles[.Rich].first, 4500)
	testing.expect_value(t, wealth_max_jools(.Rich), 5490)
	testing.expect_value(t, wealth_profiles[.Very_Rich].first, 9000)
	testing.expect_value(t, wealth_max_jools(.Very_Rich), 10980)
}

@(test)
bankruptcy_floor_rises_with_wealth :: proc(t: ^testing.T) {
	testing.expect_value(t, wealth_profiles[.Very_Poor].wallet_minimum, -999)
	testing.expect_value(t, wealth_profiles[.Rich].wallet_minimum, -499)
	testing.expect_value(t, wealth_profiles[.Very_Rich].wallet_minimum, 0)
	for w in Starting_Wealth {
		testing.expect(t, wealth_profiles[w].first >= wealth_profiles[w].wallet_minimum)
	}
}

@(test)
age_shifts_star_weights :: proc(t: ^testing.T) {
	testing.expect_value(t, star_type_weights[.Young][.Blue], 5)
	testing.expect_value(t, star_type_weights[.Young][.Red], 1)
	testing.expect_value(t, star_type_weights[.Old][.Blue], 1)
	testing.expect_value(t, star_type_weights[.Old][.Red], 5)
	for st in Star_Type {
		testing.expect_value(t, star_type_weights[.Average][st], 1)
	}
}

@(test)
density_spacing_grows_as_the_galaxy_thins :: proc(t: ^testing.T) {
	testing.expect_value(t, density_spacing[.Dense], Density_Spacing{4, 6})
	testing.expect_value(t, density_spacing[.Average], Density_Spacing{8, 12})
	testing.expect_value(t, density_spacing[.Sparse], Density_Spacing{12, 18})
}

@(test)
cycle_wraps_both_ways :: proc(t: ^testing.T) {
	testing.expect_value(t, cycle(Galactic_Age.Old, 1), Galactic_Age.Young)
	testing.expect_value(t, cycle(Galactic_Age.Young, -1), Galactic_Age.Old)
	testing.expect_value(t, cycle(Starting_Wealth.Poor, 1), Starting_Wealth.Middle)
	testing.expect_value(t, cycle_faction_count(6, 1), 2)
	testing.expect_value(t, cycle_faction_count(2, -1), 6)
	testing.expect_value(t, cycle_faction_count(4, 1), 5)
}

@(test)
every_faction_count_has_a_name :: proc(t: ^testing.T) {
	for n in MIN_FACTION_COUNT ..= MAX_FACTION_COUNT {
		testing.expect(t, len(faction_count_names[n]) > 0)
	}
}

embark_press :: proc(app: ^App, keys: ..Key) {
	for k in keys {
		app_key(app, k)
	}
}

@(test)
embark_rows_change_their_settings :: proc(t: ^testing.T) {
	app: App
	app_init(&app)
	app_key(&app, KEY_ENTER) // open Embark, cursor on Go
	embark := &stack_top(&app.stack)^.(Embark)

	embark_press(&app, KEY_DOWN, KEY_RIGHT) // Age: Average -> Old
	testing.expect_value(t, embark.settings.age, Galactic_Age.Old)
	embark_press(&app, KEY_LEFT, KEY_LEFT) // Old -> Average -> Young
	testing.expect_value(t, embark.settings.age, Galactic_Age.Young)

	embark_press(&app, KEY_DOWN, KEY_RIGHT) // Density: Average -> Sparse
	testing.expect_value(t, embark.settings.density, Galactic_Density.Sparse)

	embark_press(&app, KEY_DOWN, KEY_ENTER) // Wealth: Enter steps forward, Middle -> Rich
	testing.expect_value(t, embark.settings.wealth, Starting_Wealth.Rich)

	embark_press(&app, KEY_DOWN, KEY_RIGHT, KEY_RIGHT) // Factions: 4 -> 6
	testing.expect_value(t, embark.settings.faction_count, 6)
	embark_press(&app, KEY_RIGHT) // wraps to 2
	testing.expect_value(t, embark.settings.faction_count, 2)
}

@(test)
go_carries_the_settings_to_generate :: proc(t: ^testing.T) {
	app: App
	app_init(&app)
	app_key(&app, KEY_ENTER)
	embark_press(&app, KEY_DOWN, KEY_RIGHT, KEY_UP, KEY_ENTER) // Age -> Old, back to Go, Go
	generate, ok := stack_top(&app.stack)^.(Generate)
	testing.expect(t, ok)
	testing.expect_value(t, generate.settings.age, Galactic_Age.Old)
	testing.expect_value(t, generate.settings.faction_count, 4)
	app_key(&app, KEY_ESCAPE)
	_, back_on_embark := stack_top(&app.stack)^.(Embark)
	testing.expect(t, back_on_embark)
}

@(test)
embark_settings_survive_going_to_generate_and_back :: proc(t: ^testing.T) {
	app: App
	app_init(&app)
	app_key(&app, KEY_ENTER)
	embark_press(&app, KEY_DOWN, KEY_RIGHT, KEY_UP, KEY_ENTER, KEY_ESCAPE)
	embark := stack_top(&app.stack)^.(Embark)
	testing.expect_value(t, embark.settings.age, Galactic_Age.Old)
}

@(test)
embark_draws_every_row :: proc(t: ^testing.T) {
	embark := embark_new()
	tb: Text_Buffer
	text_clear(&tb)
	embark_draw(&embark, &tb)
	// the Faction Count row shows its value ("Four") at column 24
	y := 8 + int(Embark_Row.Factions) * 2
	testing.expect_value(t, tb[y][24].char, u8('F'))
}
