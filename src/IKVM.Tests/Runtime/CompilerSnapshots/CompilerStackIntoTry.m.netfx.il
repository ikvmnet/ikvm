.method System.Int32 $this::m(System.Int32)
.maxstack 8
.initlocals true
.local [0] System.Int32
.try IL_0006 to IL_001f catch System.Exception handler IL_001f to IL_003a
IL_0000: ldc.i4 5
IL_0005: stloc.0
IL_0006: ldloc.0
IL_0007: call System.Void $this::f()
IL_000c: ldarg.0
IL_000d: add
IL_000e: br IL_0014
IL_0013: ldloc.0
IL_0014: stloc.0
IL_0015: leave IL_003c
IL_001a: leave IL_003a
IL_001f: ldc.i4 2
IL_0024: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0029: brtrue IL_0030
IL_002e: rethrow
IL_0030: leave IL_0042
IL_0035: leave IL_003a
IL_003a: br.s IL_003a
IL_003c: ldloc.0
IL_003d: br IL_0048
IL_0042: ldnull
IL_0043: br IL_0049
IL_0048: ret
IL_0049: pop
IL_004a: ldc.i4 -1
IL_004f: ret
