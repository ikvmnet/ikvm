.method System.Int32 $this::dupStaticField()
.maxstack 3
.initlocals true
IL_0000: ldsfld System.Int32 $this::counter
IL_0005: dup
IL_0006: ldc.i4.1
IL_0007: add
IL_0008: stsfld System.Int32 $this::counter
IL_000d: ret
