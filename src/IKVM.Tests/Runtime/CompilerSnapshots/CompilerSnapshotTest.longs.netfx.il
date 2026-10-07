.method System.Int64 $this::longs(System.Int64, System.Int64, System.Int32)
.maxstack 13
.initlocals true
// line 19
IL_0000: ldarg.0
IL_0001: ldarg.1
IL_0002: mul
IL_0003: ldarg.0
IL_0004: ldarg.1
IL_0005: dup
IL_0006: ldc.i4.m1
IL_0007: conv.i8
IL_0008: bne.un IL_0014
IL_000d: pop
IL_000e: neg
IL_000f: br IL_0015
IL_0014: div
IL_0015: add
IL_0016: ldarg.0
IL_0017: ldarg.1
IL_0018: dup
IL_0019: ldc.i4.m1
IL_001a: conv.i8
IL_001b: bne.un IL_0029
IL_0020: pop
IL_0021: pop
IL_0022: ldc.i4.0
IL_0023: conv.i8
IL_0024: br IL_002a
IL_0029: rem
IL_002a: sub
IL_002b: ldarg.2
IL_002c: ldc.i4 63
IL_0031: and
IL_0032: shl
IL_0033: ldarg.2
IL_0034: ldc.i4 63
IL_0039: and
IL_003a: shr
IL_003b: ldarg.2
IL_003c: ldc.i4 63
IL_0041: and
IL_0042: shr.un
IL_0043: ldarg.0
IL_0044: ldarg.1
IL_0045: ldarg.0
IL_0046: ldc.i8 -1
IL_004f: xor
IL_0050: and
IL_0051: or
IL_0052: xor
IL_0053: ret
