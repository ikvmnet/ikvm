.method System.Int32 $this::tryCatchUnused(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Int32
.try IL_0000 to IL_0017 catch System.Exception handler IL_0017 to IL_002e
// line 41
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: br IL_000c
IL_000b: ldloc.0
IL_000c: stloc.0
IL_000d: leave IL_0030
IL_0012: leave IL_002e
IL_0017: ldc.i4.2
IL_0018: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_001d: brtrue IL_0024
IL_0022: rethrow
IL_0024: leave IL_0036
IL_0029: leave IL_002e
IL_002e: br.s IL_002e
IL_0030: ldloc.0
IL_0031: br IL_003c
IL_0036: ldnull
IL_0037: br IL_003d
IL_003c: ret
IL_003d: pop
IL_003e: ldc.i4.m1
IL_003f: ret
