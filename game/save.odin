package game

import "base:runtime"
import "core:reflect"

// Saving a universe to bytes and loading it back.
//
// The serializer walks the Universe's type information, so a field added to any struct is saved without
// touching this file. Integers are written as varints (most are small), which keeps a whole galaxy to a
// fraction of its in-memory size. Fields tagged `save:"-"` are left out: derived data (the pedia index) and
// transient state (what the ship just bumped into).
//
// The header carries a hash of the data's layout. A save written by a build whose structs differ is refused
// rather than misread. (There is no migration yet; that can wait until there is something to keep.)

SAVE_MAGIC :: [4]u8{'F', 'H', 'O', 'S'}
SAVE_FORMAT :: u8(1)
HEADER_SIZE :: 4 + 1 + 8

Load_Error :: enum {
	None,
	Not_A_Save,
	Other_Version, // written by a build with different data
	Corrupt, // cut short, or not a universe once read
}

// ---- Writing ----

@(private = "file")
Writer :: struct {
	buf: [dynamic]u8,
}

@(private = "file")
write_varint :: proc(w: ^Writer, v: u64) {
	v := v
	for v >= 0x80 {
		append(&w.buf, u8(v) | 0x80)
		v >>= 7
	}
	append(&w.buf, u8(v))
}

@(private = "file")
zigzag :: proc(v: i64) -> u64 {
	return u64(v << 1) ~ u64(v >> 63)
}

@(private = "file")
unzigzag :: proc(v: u64) -> i64 {
	return i64(v >> 1) ~ -i64(v & 1)
}

@(private = "file")
read_unsigned_at :: proc(p: rawptr, size: int) -> u64 {
	switch size {
	case 1:
		return u64((^u8)(p)^)
	case 2:
		return u64((^u16)(p)^)
	case 4:
		return u64((^u32)(p)^)
	case 8:
		return (^u64)(p)^
	}
	panic("unsupported integer size")
}

@(private = "file")
read_signed_at :: proc(p: rawptr, size: int) -> i64 {
	switch size {
	case 1:
		return i64((^i8)(p)^)
	case 2:
		return i64((^i16)(p)^)
	case 4:
		return i64((^i32)(p)^)
	case 8:
		return (^i64)(p)^
	}
	panic("unsupported integer size")
}

@(private = "file")
write_unsigned_at :: proc(p: rawptr, size: int, v: u64) {
	switch size {
	case 1:
		(^u8)(p)^ = u8(v)
	case 2:
		(^u16)(p)^ = u16(v)
	case 4:
		(^u32)(p)^ = u32(v)
	case 8:
		(^u64)(p)^ = v
	case:
		panic("unsupported integer size")
	}
}

@(private = "file")
skipped :: proc(tag: string) -> bool {
	return reflect.struct_tag_get(reflect.Struct_Tag(tag), "save") == "-"
}

@(private = "file")
write_value :: proc(w: ^Writer, p: rawptr, ti: ^runtime.Type_Info) {
	base := runtime.type_info_base(ti)
	#partial switch info in base.variant {
	case runtime.Type_Info_Integer:
		if info.signed {
			write_varint(w, zigzag(read_signed_at(p, base.size)))
		} else {
			write_varint(w, read_unsigned_at(p, base.size))
		}
	case runtime.Type_Info_Boolean:
		append(&w.buf, 1 if read_unsigned_at(p, base.size) != 0 else 0)
	case runtime.Type_Info_Enum:
		write_value(w, p, info.base)
	case runtime.Type_Info_Bit_Set:
		write_varint(w, read_unsigned_at(p, base.size))
	case runtime.Type_Info_Float:
		for i in 0 ..< base.size {
			append(&w.buf, (^u8)(uintptr(p) + uintptr(i))^)
		}
	case runtime.Type_Info_Array:
		if info.elem_size == 1 { // byte arrays (names) go in one piece
			for i in 0 ..< info.count {
				append(&w.buf, (^u8)(uintptr(p) + uintptr(i))^)
			}
			return
		}
		for i in 0 ..< info.count {
			write_value(w, rawptr(uintptr(p) + uintptr(i * info.elem_size)), info.elem)
		}
	case runtime.Type_Info_Enumerated_Array:
		for i in 0 ..< info.count {
			write_value(w, rawptr(uintptr(p) + uintptr(i * info.elem_size)), info.elem)
		}
	case runtime.Type_Info_Dynamic_Array:
		raw := (^runtime.Raw_Dynamic_Array)(p)
		write_varint(w, u64(raw.len))
		for i in 0 ..< raw.len {
			write_value(w, rawptr(uintptr(raw.data) + uintptr(i * info.elem_size)), info.elem)
		}
	case runtime.Type_Info_Struct:
		for i in 0 ..< int(info.field_count) {
			if skipped(info.tags[i]) {
				continue
			}
			write_value(w, rawptr(uintptr(p) + info.offsets[i]), info.types[i])
		}
	case runtime.Type_Info_Union:
		tag := read_unsigned_at(rawptr(uintptr(p) + info.tag_offset), info.tag_type.size)
		write_varint(w, tag)
		if tag != 0 {
			write_value(w, p, info.variants[tag - 1])
		}
	case:
		panic("this kind of data can't be saved")
	}
}

// ---- Reading ----

@(private = "file")
Reader :: struct {
	data: []u8,
	pos:  int,
	ok:   bool,
}

@(private = "file")
read_varint :: proc(r: ^Reader) -> (v: u64) {
	shift: uint
	for {
		if r.pos >= len(r.data) || shift > 63 {
			r.ok = false
			return 0
		}
		b := r.data[r.pos]
		r.pos += 1
		v |= u64(b & 0x7F) << shift
		if b < 0x80 {
			return
		}
		shift += 7
	}
}

@(private = "file")
read_value :: proc(r: ^Reader, p: rawptr, ti: ^runtime.Type_Info) {
	if !r.ok {
		return
	}
	base := runtime.type_info_base(ti)
	#partial switch info in base.variant {
	case runtime.Type_Info_Integer:
		if info.signed {
			v := unzigzag(read_varint(r))
			write_unsigned_at(p, base.size, u64(v))
		} else {
			write_unsigned_at(p, base.size, read_varint(r))
		}
	case runtime.Type_Info_Boolean:
		if r.pos >= len(r.data) {
			r.ok = false
			return
		}
		write_unsigned_at(p, base.size, u64(r.data[r.pos] != 0))
		r.pos += 1
	case runtime.Type_Info_Enum:
		read_value(r, p, info.base)
		// enums index tables all over the game, so only the values the enum declares are accepted
		member := false
		value := runtime.Type_Info_Enum_Value(read_signed_at(p, base.size) if reflect.is_signed(info.base) else i64(read_unsigned_at(p, base.size)))
		for v in info.values {
			member ||= v == value
		}
		if !member {
			r.ok = false
		}
	case runtime.Type_Info_Bit_Set:
		write_unsigned_at(p, base.size, read_varint(r))
	case runtime.Type_Info_Float:
		if r.pos + base.size > len(r.data) {
			r.ok = false
			return
		}
		for i in 0 ..< base.size {
			(^u8)(uintptr(p) + uintptr(i))^ = r.data[r.pos + i]
		}
		r.pos += base.size
	case runtime.Type_Info_Array:
		if info.elem_size == 1 {
			if r.pos + info.count > len(r.data) {
				r.ok = false
				return
			}
			for i in 0 ..< info.count {
				(^u8)(uintptr(p) + uintptr(i))^ = r.data[r.pos + i]
			}
			r.pos += info.count
			return
		}
		for i in 0 ..< info.count {
			read_value(r, rawptr(uintptr(p) + uintptr(i * info.elem_size)), info.elem)
		}
	case runtime.Type_Info_Enumerated_Array:
		for i in 0 ..< info.count {
			read_value(r, rawptr(uintptr(p) + uintptr(i * info.elem_size)), info.elem)
		}
	case runtime.Type_Info_Dynamic_Array:
		n := read_varint(r)
		// every element takes at least a byte, so a count beyond what is left is a lie
		if !r.ok || n > u64(len(r.data) - r.pos) {
			r.ok = false
			return
		}
		raw := (^runtime.Raw_Dynamic_Array)(p)
		raw.allocator = context.allocator
		if n > 0 {
			if !runtime.__dynamic_array_resize(raw, info.elem_size, info.elem.align, int(n)) {
				r.ok = false
				return
			}
		}
		for i in 0 ..< int(n) {
			read_value(r, rawptr(uintptr(raw.data) + uintptr(i * info.elem_size)), info.elem)
		}
	case runtime.Type_Info_Struct:
		for i in 0 ..< int(info.field_count) {
			if skipped(info.tags[i]) {
				continue
			}
			read_value(r, rawptr(uintptr(p) + info.offsets[i]), info.types[i])
		}
	case runtime.Type_Info_Union:
		tag := read_varint(r)
		if !r.ok || tag > u64(len(info.variants)) {
			r.ok = false
			return
		}
		write_unsigned_at(rawptr(uintptr(p) + info.tag_offset), info.tag_type.size, tag)
		if tag != 0 {
			read_value(r, p, info.variants[tag - 1])
		}
	case:
		panic("this kind of data can't be loaded")
	}
}

// ---- A hash of the layout, so a save from different data is recognized ----

@(private = "file")
mix :: proc(h: ^u64, v: u64) {
	h^ = (h^ ~ v) * 0x100000001b3
}

@(private = "file")
mix_string :: proc(h: ^u64, s: string) {
	for c in transmute([]u8)s {
		mix(h, u64(c))
	}
	mix(h, 0xFF)
}

@(private = "file")
layout :: proc(h: ^u64, ti: ^runtime.Type_Info) {
	base := runtime.type_info_base(ti)
	mix(h, u64(base.size))
	#partial switch info in base.variant {
	case runtime.Type_Info_Integer:
		mix(h, 1 + u64(info.signed))
	case runtime.Type_Info_Boolean:
		mix(h, 3)
	case runtime.Type_Info_Float:
		mix(h, 4)
	case runtime.Type_Info_Enum:
		mix(h, 5)
		for name in info.names {
			mix_string(h, name)
		}
	case runtime.Type_Info_Bit_Set:
		mix(h, 6)
		mix(h, u64(info.lower))
		mix(h, u64(info.upper))
	case runtime.Type_Info_Array:
		mix(h, 7)
		mix(h, u64(info.count))
		layout(h, info.elem)
	case runtime.Type_Info_Enumerated_Array:
		mix(h, 8)
		mix(h, u64(info.count))
		layout(h, info.elem)
	case runtime.Type_Info_Dynamic_Array:
		mix(h, 9)
		layout(h, info.elem)
	case runtime.Type_Info_Struct:
		mix(h, 10)
		for i in 0 ..< int(info.field_count) {
			if skipped(info.tags[i]) {
				continue
			}
			mix_string(h, info.names[i])
			layout(h, info.types[i])
		}
	case runtime.Type_Info_Union:
		mix(h, 11)
		for v in info.variants {
			layout(h, v)
		}
	case:
		mix(h, 99)
	}
}

universe_layout_hash :: proc() -> u64 {
	h: u64 = 0xcbf29ce484222325
	layout(&h, type_info_of(Universe))
	return h
}

// ---- Saving and loading a universe ----

// The universe as bytes. The caller frees the result.
universe_to_bytes :: proc(u: ^Universe) -> []u8 {
	w := Writer{}
	for c in SAVE_MAGIC {
		append(&w.buf, c)
	}
	append(&w.buf, SAVE_FORMAT)
	hash := universe_layout_hash()
	for i in 0 ..< 8 {
		append(&w.buf, u8(hash >> uint(i * 8)))
	}
	write_value(&w, u, type_info_of(Universe))
	return w.buf[:]
}

// Rebuilds a universe from bytes. On failure the error says why and nothing is leaked.
universe_from_bytes :: proc(data: []u8) -> (u: Universe, err: Load_Error) {
	if len(data) < HEADER_SIZE {
		return {}, .Not_A_Save
	}
	for c, i in SAVE_MAGIC {
		if data[i] != c {
			return {}, .Not_A_Save
		}
	}
	if data[4] != SAVE_FORMAT {
		return {}, .Other_Version
	}
	hash: u64
	for i in 0 ..< 8 {
		hash |= u64(data[5 + i]) << uint(i * 8)
	}
	if hash != universe_layout_hash() {
		return {}, .Other_Version
	}
	r := Reader{data = data, pos = HEADER_SIZE, ok = true}
	read_value(&r, &u, type_info_of(Universe))
	if !r.ok || r.pos != len(data) || !universe_valid(&u) {
		universe_destroy(&u)
		return {}, .Corrupt
	}
	pedia_build(&u)
	return u, .None
}

// ---- Checking what was read ----

// Every id in range and every link pointing somewhere real, so a damaged save is refused instead of crashing later.
universe_valid :: proc(u: ^Universe) -> bool {
	in_range :: proc(id: int, count: int) -> bool {
		return id >= 0 && id <= count // 0 means none
	}
	good_name :: proc(n: Name) -> bool {
		return int(n.len) <= NAME_CAPACITY
	}
	nf, ns, np, nm, na, ni, nsat := len(u.factions), len(u.star_systems), len(u.planets), len(u.maps), len(u.actors), len(u.items), len(u.satellites)
	if nm == 0 || na == 0 || !in_range(int(u.galaxy), nm) || u.galaxy == 0 || !in_range(int(u.nexus), nm) {
		return false
	}
	for f in u.factions {
		if !good_name(f.name) {
			return false
		}
	}
	for s in u.star_systems {
		if !in_range(int(s.actor), na) || !in_range(int(s.interior), nm) || !good_name(s.name) {
			return false
		}
	}
	for p in u.planets {
		if !in_range(int(p.star_system), ns) || !in_range(int(p.faction), nf) || !in_range(int(p.actor), na) || !good_name(p.name) {
			return false
		}
	}
	for s in u.satellites {
		if !in_range(int(s.planet), np) || !in_range(int(s.star_system), ns) || !good_name(s.name) {
			return false
		}
	}
	for m in u.maps {
		if !in_range(int(m.owner), na) || int(m.kind) < 0 || int(m.kind) >= len(Map_Kind) {
			return false
		}
		for id in m.actors {
			if id == 0 || int(id) > na {
				return false
			}
		}
	}
	for a in u.actors {
		if !in_range(int(a.map_id), nm) || !in_range(int(a.interior), nm) || !in_range(int(a.star_system), ns) || !in_range(int(a.planet), np) || !in_range(int(a.satellite), nsat) || !in_range(int(a.offer), ni) || !in_range(int(a.target), na) || (a.size != 1 && a.size != 3 && a.size != 5) {
			return false
		}
		if int(a.kind) < 0 || int(a.kind) >= len(Actor_Kind) {
			return false
		}
	}
	for it in u.items {
		m := it.mission
		if !in_range(int(m.origin), np) || !in_range(int(m.destination), np) || int(it.kind) < 0 || int(it.kind) >= len(Item_Kind) {
			return false
		}
		if it.mark < 0 || it.mark > MAX_MARK || (item_info[it.kind].marked && it.mark < 1) {
			return false
		}
		// the word tables a delivery's name is built from
		if int(m.adverb) >= len(mission_adverbs) || int(m.adjective) >= len(mission_adjectives) || int(m.noun) >= len(mission_nouns) || int(m.first_name) >= len(mission_first_names) || int(m.last_name) >= len(mission_last_names) || int(m.job) >= len(mission_jobs) {
			return false
		}
		if it.kind == .Delivery && (m.destination == 0 || m.origin == 0) {
			return false
		}
	}
	a := u.avatar
	if a.actor == 0 || int(a.actor) > na || !in_range(int(a.faction), nf) || !in_range(int(a.home_planet), np) || !in_range(int(a.star_system), ns) {
		return false
	}
	for id in a.inventory {
		if id == 0 || int(id) > ni {
			return false
		}
	}
	for id in a.equipment {
		if !in_range(int(id), ni) {
			return false
		}
	}
	return true
}
