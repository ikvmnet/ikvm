.method System.Int32 $this::staticField()
.maxstack 2
.initlocals true
IL_0000: ldsfld System.Int32 $this::counter
IL_0005: ldc.i4.2
IL_0006: add
IL_0007: stsfld System.Int32 $this::counter
IL_000c: ldsfld System.Int32 $this::counter
IL_0011: ret
