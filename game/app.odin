package game

// Platform-independent app state. The platform layer feeds it keys and asks it to draw.

App :: struct {
	text:  Text_Buffer,
	stack: Screen_Stack,
}

app_init :: proc(app: ^App) {
	app^ = {}
	stack_push(&app.stack, Main_Menu{})
	app_draw(app)
}

app_key :: proc(app: ^App, key: Key) {
	if top := stack_top(&app.stack); top != nil {
		stack_apply(&app.stack, screen_key(top, key))
	}
	app_draw(app)
}

app_draw :: proc(app: ^App) {
	if top := stack_top(&app.stack); top != nil {
		screen_draw(top, &app.text)
	} else {
		text_clear(&app.text)
		text_put_centered(&app.text, 12, "Thanks for playing!", .Yellow)
	}
}
