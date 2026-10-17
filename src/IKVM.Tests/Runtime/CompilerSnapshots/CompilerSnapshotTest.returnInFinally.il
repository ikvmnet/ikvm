.method System.Int32 $this::returnInFinally(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Int32
.local [1] System.Exception
.local [2] System.Exception
.try IL_0000 to IL_0016 catch System.Exception handler IL_0016 to IL_0027
// line 54
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: stloc.0
IL_0007: br IL_000c
IL_000c: leave IL_0029
IL_0011: leave IL_0027
IL_0016: ldc.i4.0
IL_0017: call System.Exception IKVM.Runtime.ByteCodeHelper::MapException<System.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_001c: stloc.1
IL_001d: leave IL_002e
IL_0022: leave IL_0027
IL_0027: br.s IL_0027
IL_0029: br IL_0034
IL_002e: ldloc.1
IL_002f: br IL_0040
IL_0034: ldarg.0
IL_0035: ldc.i4.0
IL_0036: ble IL_003e
IL_003b: ldarg.0
IL_003c: neg
IL_003d: ret
IL_003e: ldloc.0
IL_003f: ret
IL_0040: stloc.2
IL_0041: ldarg.0
IL_0042: ldc.i4.0
IL_0043: ble IL_004b
IL_0048: ldarg.0
IL_0049: neg
IL_004a: ret
IL_004b: ldloc.2
IL_004c: call System.Exception java.lang.Throwable::__<unmap>(System.Exception)
IL_0051: throw
