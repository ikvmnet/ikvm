.method System.Int32 $this::manyArgsWithTry(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)
.maxstack 2
.initlocals true
.try IL_0006 to IL_0024 catch System.Exception handler IL_0024 to IL_003b
IL_0000: ldarga.s 3
IL_0002: pop
IL_0003: ldarga.s 4
IL_0005: pop
// line 98
IL_0006: ldarg.s 4
IL_0008: ldc.i4.1
IL_0009: add
IL_000a: starg.s 4
IL_000c: ldarg.s 4
IL_000e: call System.Int32 $this::g(System.Int32)
IL_0013: starg.s 3
IL_0015: br IL_001a
IL_001a: leave IL_003d
IL_001f: leave IL_003b
IL_0024: ldc.i4.2
IL_0025: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_002a: brtrue IL_0031
IL_002f: rethrow
IL_0031: leave IL_0042
IL_0036: leave IL_003b
IL_003b: br.s IL_003b
IL_003d: br IL_0048
IL_0042: ldnull
IL_0043: br IL_004d
IL_0048: br IL_0051
IL_004d: pop
IL_004e: ldc.i4.m1
IL_004f: starg.s 0
IL_0051: ldarg.0
IL_0052: ldarg.1
IL_0053: add
IL_0054: ldarg.2
IL_0055: add
IL_0056: ldarg.3
IL_0057: add
IL_0058: ldarg.s 4
IL_005a: add
IL_005b: ret
