.method System.Int32 $this::lookupSwitch(System.Int32)
.maxstack 11
.initlocals true
IL_0000: ldarg.0
IL_0001: dup
IL_0002: ldc.i4 -5
IL_0007: bne.un IL_0012
IL_000c: pop
IL_000d: br IL_0046
IL_0012: dup
IL_0013: ldc.i4 1
IL_0018: bne.un IL_0023
IL_001d: pop
IL_001e: br IL_003a
IL_0023: dup
IL_0024: ldc.i4 1000
IL_0029: bne.un IL_0034
IL_002e: pop
IL_002f: br IL_0040
IL_0034: pop
IL_0035: br IL_004c
IL_003a: ldc.i4 1
IL_003f: ret
IL_0040: ldc.i4 2
IL_0045: ret
IL_0046: ldc.i4 3
IL_004b: ret
IL_004c: ldc.i4 0
IL_0051: ret
