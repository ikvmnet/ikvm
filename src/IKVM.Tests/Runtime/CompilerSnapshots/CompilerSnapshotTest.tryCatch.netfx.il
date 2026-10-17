.method System.Int32 $this::tryCatch(System.Int32)
.maxstack 7
.initlocals true
.local [0] System.Int32
.local [1] java.lang.RuntimeException
.local [2] java.lang.RuntimeException
.try IL_0000 to IL_0017 catch System.Exception handler IL_0017 to IL_0034
// line 39
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: br IL_000c
IL_000b: ldloc.0
IL_000c: stloc.0
IL_000d: leave IL_0036
IL_0012: leave IL_0034
IL_0017: ldc.i4 0
IL_001c: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0021: dup
IL_0022: brtrue IL_0029
IL_0027: rethrow
IL_0029: stloc.1
IL_002a: leave IL_003c
IL_002f: leave IL_0034
IL_0034: br.s IL_0034
IL_0036: ldloc.0
IL_0037: br IL_0042
IL_003c: ldloc.1
IL_003d: br IL_0043
IL_0042: ret
IL_0043: stloc.2
IL_0044: ldloc.2
IL_0045: call System.Void $this::h(System.Exception)
IL_004a: ldc.i4 -1
IL_004f: ret
