package game

import "core:time"

// Platform-independent app state. The platform layer feeds it keys, ticks it once per frame, and asks it to draw.

App :: struct {
	text:    Text_Buffer,
	stack:   Screen_Stack,
	session: Session,
}

app_init :: proc(app: ^App, seed_source: proc() -> u64 = nil, storage: Storage = {}) {
	app^ = {}
	app.session.seed_source = seed_source
	app.session.storage = storage
	stack_push(&app.stack, Main_Menu{})
	app_draw(app)
}

app_destroy :: proc(app: ^App) {
	session_end(&app.session)
}

app_key :: proc(app: ^App, key: Key) {
	if top := stack_top(&app.stack); top != nil {
		stack_apply(&app.stack, screen_key(top, key, &app.session))
	}
	app_draw(app)
	free_all(context.temp_allocator) // screens build their lists in it
}

// Called once per frame, after keys: lets the current screen do timed work such as generating a universe.
app_tick :: proc(app: ^App) {
	if top := stack_top(&app.stack); top != nil {
		stack_apply(&app.stack, screen_tick(top, &app.session))
	}
	app_draw(app)
	free_all(context.temp_allocator)
}

app_draw :: proc(app: ^App) {
	if top := stack_top(&app.stack); top != nil {
		screen_draw(top, &app.text, &app.session)
	} else {
		text_clear(&app.text)
		text_put_centered(&app.text, 12, "Thanks for playing!", .Yellow)
	}
}

// Generation work done per frame; the browser has ~16ms per frame and rendering needs a few.
GENERATION_BUDGET :: 10 * time.Millisecond
