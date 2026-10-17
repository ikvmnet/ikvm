.method System.Int32 $this::returnFromLoopWithFinally(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Int32
.local [1] System.Int32
.try IL_0009 to IL_0026 fault handler IL_0026 to IL_0034
// line 96
IL_0000: ldc.i4.0
IL_0001: stloc.0
IL_0002: ldloc.0
IL_0003: ldarg.0
IL_0004: bge IL_0068
IL_0009: ldloc.0
IL_000a: ldc.i4.3
IL_000b: bne.un IL_0017
IL_0010: ldloc.0
IL_0011: stloc.1
IL_0012: br IL_001c
IL_0017: leave IL_0036
IL_001c: leave IL_003b
IL_0021: leave IL_0034
IL_0026: ldsfld System.Int32 $this::counter
IL_002b: ldc.i4.1
IL_002c: add
IL_002d: stsfld System.Int32 $this::counter
IL_0032: endfinally
IL_0033: endfinally
IL_0034: br.s IL_0034
IL_0036: br IL_004e
IL_003b: br IL_0040
IL_0040: ldsfld System.Int32 $this::counter
IL_0045: ldc.i4.1
IL_0046: add
IL_0047: stsfld System.Int32 $this::counter
IL_004c: ldloc.1
IL_004d: ret
IL_004e: ldsfld System.Int32 $this::counter
IL_0053: ldc.i4.1
IL_0054: add
IL_0055: stsfld System.Int32 $this::counter
IL_005a: br IL_005f
IL_005f: ldloc.0
IL_0060: ldc.i4.1
IL_0061: add
IL_0062: stloc.0
IL_0063: br IL_0002
IL_0068: ldc.i4.m1
IL_0069: ret
