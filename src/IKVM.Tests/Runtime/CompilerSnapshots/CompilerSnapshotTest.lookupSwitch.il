.method System.Int32 $this::lookupSwitch(System.Int32)
.maxstack 3
.initlocals true
IL_0000: ldarg.0
IL_0001: dup
IL_0002: ldc.i4.s -5
IL_0004: bne.un IL_000f
IL_0009: pop
IL_000a: br IL_0037
IL_000f: dup
IL_0010: ldc.i4.1
IL_0011: bne.un IL_001c
IL_0016: pop
IL_0017: br IL_0033
IL_001c: dup
IL_001d: ldc.i4 1000
IL_0022: bne.un IL_002d
IL_0027: pop
IL_0028: br IL_0035
IL_002d: pop
IL_002e: br IL_0039
IL_0033: ldc.i4.1
IL_0034: ret
IL_0035: ldc.i4.2
IL_0036: ret
IL_0037: ldc.i4.3
IL_0038: ret
IL_0039: ldc.i4.0
IL_003a: ret
