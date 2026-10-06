.method System.Int32 $this::returnFromLoopWithFinally(System.Int32)
.maxstack 12
.initlocals true
.local [0] System.Int32
.local [1] System.Int32
.try IL_000d to IL_002e fault handler IL_002e to IL_0040
// line 96
IL_0000: ldc.i4 0
IL_0005: stloc.0
IL_0006: ldloc.0
IL_0007: ldarg.0
IL_0008: bge IL_0080
IL_000d: ldloc.0
IL_000e: ldc.i4 3
IL_0013: bne.un IL_001f
IL_0018: ldloc.0
IL_0019: stloc.1
IL_001a: br IL_0024
IL_001f: leave IL_0042
IL_0024: leave IL_0047
IL_0029: leave IL_0040
IL_002e: ldsfld System.Int32 $this::counter
IL_0033: ldc.i4 1
IL_0038: add
IL_0039: stsfld System.Int32 $this::counter
IL_003e: endfinally
IL_003f: endfinally
IL_0040: br.s IL_0040
IL_0042: br IL_005e
IL_0047: br IL_004c
IL_004c: ldsfld System.Int32 $this::counter
IL_0051: ldc.i4 1
IL_0056: add
IL_0057: stsfld System.Int32 $this::counter
IL_005c: ldloc.1
IL_005d: ret
IL_005e: ldsfld System.Int32 $this::counter
IL_0063: ldc.i4 1
IL_0068: add
IL_0069: stsfld System.Int32 $this::counter
IL_006e: br IL_0073
IL_0073: ldloc.0
IL_0074: ldc.i4 1
IL_0079: add
IL_007a: stloc.0
IL_007b: br IL_0006
IL_0080: ldc.i4 -1
IL_0085: ret
