.method instance System.Int64 $this::dupLongField()
.maxstack 4
.initlocals true
.local [0] System.Int64
.local [1] $this
// line 90
IL_0000: ldarg.0
IL_0001: dup
IL_0002: ldfld System.Int64 $this::longField
IL_0007: stloc.0
IL_0008: stloc.1
IL_0009: ldloc.0
IL_000a: ldloc.1
IL_000b: ldloc.0
IL_000c: ldc.i8 1
IL_0015: add
IL_0016: stfld System.Int64 $this::longField
IL_001b: ret
