.method instance System.Int32 $this::instanceField($this)
.maxstack 3
.initlocals true
// line 78
IL_0000: ldarg.0
IL_0001: ldarg.1
IL_0002: ldfld System.Int32 $this::field
IL_0007: ldc.i4 1
IL_000c: add
IL_000d: stfld System.Int32 $this::field
IL_0012: ldarg.0
IL_0013: ldfld System.Int32 $this::field
IL_0018: ret
