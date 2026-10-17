.method System.Int32 $this::tryFinally(System.Int32)
.maxstack 5
.initlocals true
.local [0] System.Int32
.try IL_0000 to IL_0016 finally handler IL_0016 to IL_0028
// line 45
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: stloc.0
IL_0007: br IL_000c
IL_000c: leave IL_002a
IL_0011: leave IL_0028
IL_0016: ldsfld System.Int32 $this::counter
IL_001b: ldc.i4 1
IL_0020: add
IL_0021: stsfld System.Int32 $this::counter
IL_0026: endfinally
IL_0027: endfinally
IL_0028: br.s IL_0028
IL_002a: br IL_002f
IL_002f: br IL_0034
IL_0034: ldloc.0
IL_0035: ret
