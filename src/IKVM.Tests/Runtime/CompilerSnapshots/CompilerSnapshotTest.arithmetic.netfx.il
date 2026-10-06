.method System.Int32 $this::arithmetic(System.Int32, System.Int32)
.maxstack 10
.initlocals true
// line 17
IL_0000: ldarg.0
IL_0001: ldarg.1
IL_0002: add
IL_0003: ldarg.0
IL_0004: ldarg.1
IL_0005: sub
IL_0006: mul
IL_0007: ldarg.1
IL_0008: dup
IL_0009: ldc.i4.m1
IL_000a: bne.un IL_0016
IL_000f: pop
IL_0010: neg
IL_0011: br IL_0017
IL_0016: div
IL_0017: ldc.i4 7
IL_001c: dup
IL_001d: ldc.i4.m1
IL_001e: bne.un IL_002b
IL_0023: pop
IL_0024: pop
IL_0025: ldc.i4.0
IL_0026: br IL_002c
IL_002b: rem
IL_002c: ldc.i4 2
IL_0031: shl
IL_0032: ldc.i4 1
IL_0037: shr
IL_0038: ldc.i4 1
IL_003d: shr.un
IL_003e: ldarg.0
IL_003f: xor
IL_0040: ldarg.1
IL_0041: ldarg.0
IL_0042: ldc.i4 -1
IL_0047: xor
IL_0048: and
IL_0049: or
IL_004a: ret
