.method System.Int32 $this::tryFinally(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Int32
.try IL_0000 to IL_0016 finally handler IL_0016 to IL_0024
// line 45
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: stloc.0
IL_0007: br IL_000c
IL_000c: leave IL_0026
IL_0011: leave IL_0024
IL_0016: ldsfld System.Int32 $this::counter
IL_001b: ldc.i4.1
IL_001c: add
IL_001d: stsfld System.Int32 $this::counter
IL_0022: endfinally
IL_0023: endfinally
IL_0024: br.s IL_0024
IL_0026: br IL_002b
IL_002b: br IL_0030
IL_0030: ldloc.0
IL_0031: ret
