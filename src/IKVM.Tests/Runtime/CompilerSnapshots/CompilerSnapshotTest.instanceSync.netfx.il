.method instance System.Int32 $this::instanceSync()
.maxstack 4
.initlocals true
.local [0] System.Int32
.local [1] $this
// line 62
IL_0000: ldarg.0
IL_0001: dup
IL_0002: ldfld System.Int32 $this::field
IL_0007: stloc.0
IL_0008: stloc.1
IL_0009: ldloc.0
IL_000a: ldloc.1
IL_000b: ldloc.0
IL_000c: ldc.i4 1
IL_0011: add
IL_0012: stfld System.Int32 $this::field
IL_0017: ret
