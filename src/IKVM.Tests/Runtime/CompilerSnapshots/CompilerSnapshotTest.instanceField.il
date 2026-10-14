.method instance System.Int32 $this::instanceField($this)
.maxstack 3
.initlocals true
// line 78
IL_0000: ldarg.0
IL_0001: ldarg.1
IL_0002: ldfld System.Int32 $this::field
IL_0007: ldc.i4.1
IL_0008: add
IL_0009: stfld System.Int32 $this::field
IL_000e: ldarg.0
IL_000f: ldfld System.Int32 $this::field
IL_0014: ret
