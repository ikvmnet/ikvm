.method System.Int32 $this::whileTrueBreak(System.Int32)
.maxstack 5
.initlocals true
.local [0] System.Int32
IL_0000: ldc.i4 0
IL_0005: stloc.0
IL_0006: ldloc.0
IL_0007: ldarg.0
IL_0008: ble IL_0012
IL_000d: br IL_001f
IL_0012: ldloc.0
IL_0013: ldc.i4 2
IL_0018: add
IL_0019: stloc.0
IL_001a: br IL_0006
IL_001f: ldloc.0
IL_0020: ret
