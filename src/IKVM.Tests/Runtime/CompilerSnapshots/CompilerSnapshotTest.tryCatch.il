.method System.Int32 $this::tryCatch(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Int32
.local [1] java.lang.RuntimeException
.local [2] java.lang.RuntimeException
.try IL_0000 to IL_0017 catch System.Exception handler IL_0017 to IL_0030
// line 39
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: br IL_000c
IL_000b: ldloc.0
IL_000c: stloc.0
IL_000d: leave IL_0032
IL_0012: leave IL_0030
IL_0017: ldc.i4.0
IL_0018: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_001d: dup
IL_001e: brtrue IL_0025
IL_0023: rethrow
IL_0025: stloc.1
IL_0026: leave IL_0038
IL_002b: leave IL_0030
IL_0030: br.s IL_0030
IL_0032: ldloc.0
IL_0033: br IL_003e
IL_0038: ldloc.1
IL_0039: br IL_003f
IL_003e: ret
IL_003f: stloc.2
IL_0040: ldloc.2
IL_0041: call System.Void $this::h(System.Exception)
IL_0046: ldc.i4.m1
IL_0047: ret
