.method System.Int32 $this::tryCatchThrowable(System.Int32)
.maxstack 7
.initlocals true
.local [0] System.Int32
.try IL_0000 to IL_0017 catch System.Exception handler IL_0017 to IL_002b
// line 43
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: br IL_000c
IL_000b: ldloc.0
IL_000c: stloc.0
IL_000d: leave IL_002d
IL_0012: leave IL_002b
IL_0017: ldc.i4 2
IL_001c: call System.Exception IKVM.Runtime.ByteCodeHelper::MapException<System.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0021: leave IL_0033
IL_0026: leave IL_002b
IL_002b: br.s IL_002b
IL_002d: ldloc.0
IL_002e: br IL_0039
IL_0033: ldnull
IL_0034: br IL_003a
IL_0039: ret
IL_003a: pop
IL_003b: ldc.i4 -1
IL_0040: ret
