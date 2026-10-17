.method System.Int32 $this::returnInFinally(System.Int32)
.maxstack 10
.initlocals true
.local [0] System.Int32
.local [1] System.Exception
.local [2] System.Exception
.try IL_0000 to IL_0016 catch System.Exception handler IL_0016 to IL_002b
// line 54
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: stloc.0
IL_0007: br IL_000c
IL_000c: leave IL_002d
IL_0011: leave IL_002b
IL_0016: ldc.i4 0
IL_001b: call System.Exception IKVM.Runtime.ByteCodeHelper::MapException<System.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0020: stloc.1
IL_0021: leave IL_0032
IL_0026: leave IL_002b
IL_002b: br.s IL_002b
IL_002d: br IL_0038
IL_0032: ldloc.1
IL_0033: br IL_0044
IL_0038: ldarg.0
IL_0039: ldc.i4.0
IL_003a: ble IL_0042
IL_003f: ldarg.0
IL_0040: neg
IL_0041: ret
IL_0042: ldloc.0
IL_0043: ret
IL_0044: stloc.2
IL_0045: ldarg.0
IL_0046: ldc.i4.0
IL_0047: ble IL_004f
IL_004c: ldarg.0
IL_004d: neg
IL_004e: ret
IL_004f: ldloc.2
IL_0050: call System.Exception java.lang.Throwable::__<unmap>(System.Exception)
IL_0055: throw
