package game

// Screens replace the VB `IState.Run` chain: the stack holds the screens the player has navigated through,
// only the top one is drawn and receives keys, and its on_key answers with a Transition.

MAX_SCREENS :: 16

Push :: struct {
	screen: Screen,
}
Replace :: struct {
	screen: Screen,
}
Pop :: struct {}
// Throws away the whole stack and starts over from `screen`.
Reset :: struct {
	screen: Screen,
}

// nil means "stay on this screen".
Transition :: union {
	Push,
	Replace,
	Pop,
	Reset,
}

Screen_Stack :: struct {
	items: [MAX_SCREENS]Screen,
	count: int,
}

stack_push :: proc(stack: ^Screen_Stack, screen: Screen) -> bool {
	if stack.count >= MAX_SCREENS {
		return false
	}
	stack.items[stack.count] = screen
	stack.count += 1
	return true
}

stack_pop :: proc(stack: ^Screen_Stack) {
	if stack.count > 0 {
		stack.count -= 1
		stack.items[stack.count] = nil
	}
}

stack_top :: proc(stack: ^Screen_Stack) -> ^Screen {
	if stack.count == 0 {
		return nil
	}
	return &stack.items[stack.count - 1]
}

stack_apply :: proc(stack: ^Screen_Stack, transition: Transition) {
	switch t in transition {
	case Push:
		stack_push(stack, t.screen)
	case Replace:
		stack_pop(stack)
		stack_push(stack, t.screen)
	case Pop:
		stack_pop(stack)
	case Reset:
		for stack.count > 0 {
			stack_pop(stack)
		}
		stack_push(stack, t.screen)
	}
}
