.method System.Int32 $this::tryCatchUnused(System.Int32)
.maxstack 7
.initlocals true
.local [0] System.Int32
.try IL_0000 to IL_0017 catch System.Exception handler IL_0017 to IL_0032
// line 41
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: br IL_000c
IL_000b: ldloc.0
IL_000c: stloc.0
IL_000d: leave IL_0034
IL_0012: leave IL_0032
IL_0017: ldc.i4 2
IL_001c: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0021: brtrue IL_0028
IL_0026: rethrow
IL_0028: leave IL_003a
IL_002d: leave IL_0032
IL_0032: br.s IL_0032
IL_0034: ldloc.0
IL_0035: br IL_0040
IL_003a: ldnull
IL_003b: br IL_0041
IL_0040: ret
IL_0041: pop
IL_0042: ldc.i4 -1
IL_0047: ret
