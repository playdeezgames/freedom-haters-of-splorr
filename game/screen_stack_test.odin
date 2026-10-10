#+build !js
package game

import "core:testing"

@(test)
push_pop_and_top :: proc(t: ^testing.T) {
	stack: Screen_Stack
	testing.expect(t, stack_top(&stack) == nil)
	testing.expect(t, stack_push(&stack, Main_Menu{}))
	testing.expect(t, stack_push(&stack, About{}))
	_, on_about := stack_top(&stack)^.(About)
	testing.expect(t, on_about)
	stack_pop(&stack)
	_, on_menu := stack_top(&stack)^.(Main_Menu)
	testing.expect(t, on_menu)
	stack_pop(&stack)
	stack_pop(&stack) // popping an empty stack is harmless
	testing.expect_value(t, stack.count, 0)
}

@(test)
push_fails_when_full :: proc(t: ^testing.T) {
	stack: Screen_Stack
	for _ in 0 ..< MAX_SCREENS {
		testing.expect(t, stack_push(&stack, About{}))
	}
	testing.expect(t, !stack_push(&stack, About{}))
	testing.expect_value(t, stack.count, MAX_SCREENS)
}

@(test)
replace_swaps_the_top :: proc(t: ^testing.T) {
	stack: Screen_Stack
	stack_push(&stack, Main_Menu{})
	stack_push(&stack, About{})
	stack_apply(&stack, Replace{embark_new()})
	testing.expect_value(t, stack.count, 2)
	_, ok := stack_top(&stack)^.(Embark)
	testing.expect(t, ok)
}

@(test)
menu_wraps_both_ways :: proc(t: ^testing.T) {
	cursor := 0
	testing.expect_value(t, menu_key(&cursor, 3, KEY_UP), Menu_Result.None)
	testing.expect_value(t, cursor, 2)
	menu_key(&cursor, 3, KEY_DOWN)
	testing.expect_value(t, cursor, 0)
	testing.expect_value(t, menu_key(&cursor, 3, KEY_ENTER), Menu_Result.Chosen)
	testing.expect_value(t, menu_key(&cursor, 3, KEY_ESCAPE), Menu_Result.Cancelled)
	testing.expect_value(t, menu_key(&cursor, 0, KEY_ENTER), Menu_Result.None)
}

@(test)
main_menu_navigates_to_embark_and_back :: proc(t: ^testing.T) {
	app: App
	app_init(&app)
	app_key(&app, KEY_ENTER) // Embark is the first choice
	_, on_embark := stack_top(&app.stack)^.(Embark)
	testing.expect(t, on_embark)
	app_key(&app, KEY_ESCAPE)
	_, on_menu := stack_top(&app.stack)^.(Main_Menu)
	testing.expect(t, on_menu)
}

@(test)
about_pops_and_root_escape_stays :: proc(t: ^testing.T) {
	app: App
	app_init(&app)
	app_key(&app, KEY_DOWN) // Load
	app_key(&app, KEY_DOWN) // About
	app_key(&app, KEY_ENTER)
	_, on_about := stack_top(&app.stack)^.(About)
	testing.expect(t, on_about)
	app_key(&app, KEY_ENTER)
	testing.expect_value(t, app.stack.count, 1)
	app_key(&app, KEY_ESCAPE)
	testing.expect_value(t, app.stack.count, 1)
}

@(test)
embark_cancel_pops :: proc(t: ^testing.T) {
	app: App
	app_init(&app)
	app_key(&app, KEY_ENTER)
	app_key(&app, KEY_UP) // wraps from Go to Cancel
	app_key(&app, KEY_ENTER)
	testing.expect_value(t, app.stack.count, 1)
}
