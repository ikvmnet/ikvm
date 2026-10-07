.method System.Int32 $this::staticField()
.maxstack 2
.initlocals true
IL_0000: ldsfld System.Int32 $this::counter
IL_0005: ldc.i4 2
IL_000a: add
IL_000b: stsfld System.Int32 $this::counter
IL_0010: ldsfld System.Int32 $this::counter
IL_0015: ret
