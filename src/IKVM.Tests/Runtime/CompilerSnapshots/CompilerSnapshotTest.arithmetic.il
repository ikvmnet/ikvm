.method System.Int32 $this::arithmetic(System.Int32, System.Int32)
.maxstack 4
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
IL_0017: ldc.i4.7
IL_0018: dup
IL_0019: ldc.i4.m1
IL_001a: bne.un IL_0027
IL_001f: pop
IL_0020: pop
IL_0021: ldc.i4.0
IL_0022: br IL_0028
IL_0027: rem
IL_0028: ldc.i4.2
IL_0029: shl
IL_002a: ldc.i4.1
IL_002b: shr
IL_002c: ldc.i4.1
IL_002d: shr.un
IL_002e: ldarg.0
IL_002f: xor
IL_0030: ldarg.1
IL_0031: ldarg.0
IL_0032: ldc.i4.m1
IL_0033: xor
IL_0034: and
IL_0035: or
IL_0036: ret
