#+build !js
package game

import "core:testing"

// A Storage that keeps its values in memory. Thread-local so parallel tests don't see each other's saves; call
// memory_storage_reset at the start of a test that uses it.

@(thread_local)
memory_values: map[string][]u8

@(thread_local)
memory_write_fails: bool

memory_storage_reset :: proc() {
	for key, value in memory_values {
		delete(value)
		delete(key)
	}
	delete(memory_values)
	memory_values = nil
	memory_write_fails = false
}

memory_storage :: proc() -> Storage {
	return {
		size = proc(key: string) -> int {
			value, ok := memory_values[key]
			return len(value) if ok else -1
		},
		read = proc(key: string, into: []u8) -> bool {
			value, ok := memory_values[key]
			if !ok || len(value) != len(into) {
				return false
			}
			copy(into, value)
			return true
		},
		write = proc(key: string, data: []u8) -> bool {
			if memory_write_fails {
				return false
			}
			if old_key, old_value := delete_key(&memory_values, key); len(old_key) > 0 {
				delete(old_value)
				delete(old_key)
			}
			memory_values[clone_key(key)] = clone_bytes(data)
			return true
		},
		remove = proc(key: string) {
			if old_key, old_value := delete_key(&memory_values, key); len(old_key) > 0 {
				delete(old_value)
				delete(old_key)
			}
		},
	}
}

@(private = "file")
clone_key :: proc(key: string) -> string {
	out := make([]u8, len(key))
	copy(out, key)
	return string(out)
}

@(private = "file")
clone_bytes :: proc(data: []u8) -> []u8 {
	out := make([]u8, len(data))
	copy(out, data)
	return out
}

@(test)
memory_storage_stores_replaces_and_removes :: proc(t: ^testing.T) {
	memory_storage_reset()
	defer memory_storage_reset()
	s := memory_storage()
	testing.expect(t, !storage_has(s, "a"))
	testing.expect(t, s.write("a", {1, 2, 3}))
	testing.expect(t, s.write("a", {4, 5}))
	data, ok := storage_get(s, "a")
	testing.expect(t, ok)
	testing.expect_value(t, len(data), 2)
	delete(data)
	s.remove("a")
	testing.expect(t, !storage_has(s, "a"))
}
