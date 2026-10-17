.method System.Int32 $this::nestedTry(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Exception
.local [1] System.Exception
.local [2] System.Exception
.local [3] java.lang.Exception
.local [4] System.Int32
.try IL_0000 to IL_0014 catch System.Exception handler IL_0014 to IL_0025
.try IL_0000 to IL_002c catch System.Exception handler IL_002c to IL_0043
.try IL_0000 to IL_004a fault handler IL_004a to IL_0058
.try IL_006b to IL_0086 catch System.Exception handler IL_0086 to IL_009d
.try IL_006b to IL_00a4 fault handler IL_00a4 to IL_00b2
.try IL_00c2 to IL_00dc catch System.Exception handler IL_00dc to IL_00f3
.try IL_00c0 to IL_00fa fault handler IL_00fa to IL_0108
.try IL_0122 to IL_0136 finally handler IL_0136 to IL_0144
// line 49
IL_0000: call System.Void $this::f()
IL_0005: br IL_000a
IL_000a: leave IL_005a
IL_000f: leave IL_0025
IL_0014: ldc.i4.0
IL_0015: call System.Exception IKVM.Runtime.ByteCodeHelper::MapException<System.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_001a: stloc.0
IL_001b: leave IL_005f
IL_0020: leave IL_0025
IL_0025: br.s IL_0025
IL_0027: leave IL_0043
IL_002c: ldc.i4.2
IL_002d: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0032: brtrue IL_0039
IL_0037: rethrow
IL_0039: leave IL_0065
IL_003e: leave IL_0043
IL_0043: br.s IL_0043
IL_0045: leave IL_0058
IL_004a: ldsfld System.Int32 $this::counter
IL_004f: ldc.i4.1
IL_0050: sub
IL_0051: stsfld System.Int32 $this::counter
IL_0056: endfinally
IL_0057: endfinally
IL_0058: br.s IL_0058
IL_005a: br IL_006b
IL_005f: ldloc.0
IL_0060: br IL_00bf
IL_0065: ldnull
IL_0066: br IL_0121
IL_006b: ldsfld System.Int32 $this::counter
IL_0070: ldc.i4.1
IL_0071: add
IL_0072: stsfld System.Int32 $this::counter
IL_0077: br IL_007c
IL_007c: leave IL_00b4
IL_0081: leave IL_009d
IL_0086: ldc.i4.2
IL_0087: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_008c: brtrue IL_0093
IL_0091: rethrow
IL_0093: leave IL_00b9
IL_0098: leave IL_009d
IL_009d: br.s IL_009d
IL_009f: leave IL_00b2
IL_00a4: ldsfld System.Int32 $this::counter
IL_00a9: ldc.i4.1
IL_00aa: sub
IL_00ab: stsfld System.Int32 $this::counter
IL_00b0: endfinally
IL_00b1: endfinally
IL_00b2: br.s IL_00b2
IL_00b4: br IL_0110
IL_00b9: ldnull
IL_00ba: br IL_0121
IL_00bf: stloc.1
IL_00c0: ldloc.1
IL_00c1: stloc.1
IL_00c2: ldloc.1
IL_00c3: stloc.2
IL_00c4: ldsfld System.Int32 $this::counter
IL_00c9: ldc.i4.1
IL_00ca: add
IL_00cb: stsfld System.Int32 $this::counter
IL_00d0: ldloc.2
IL_00d1: call System.Exception java.lang.Throwable::__<unmap>(System.Exception)
IL_00d6: throw
IL_00d7: leave IL_00f3
IL_00dc: ldc.i4.2
IL_00dd: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_00e2: brtrue IL_00e9
IL_00e7: rethrow
IL_00e9: leave IL_010a
IL_00ee: leave IL_00f3
IL_00f3: br.s IL_00f3
IL_00f5: leave IL_0108
IL_00fa: ldsfld System.Int32 $this::counter
IL_00ff: ldc.i4.1
IL_0100: sub
IL_0101: stsfld System.Int32 $this::counter
IL_0106: endfinally
IL_0107: endfinally
IL_0108: br.s IL_0108
IL_010a: ldnull
IL_010b: br IL_0121
IL_0110: ldsfld System.Int32 $this::counter
IL_0115: ldc.i4.1
IL_0116: sub
IL_0117: stsfld System.Int32 $this::counter
IL_011c: br IL_0153
IL_0121: stloc.3
IL_0122: ldloc.3
IL_0123: pop
IL_0124: ldc.i4.m1
IL_0125: stloc.s 4
IL_0127: br IL_012c
IL_012c: leave IL_0146
IL_0131: leave IL_0144
IL_0136: ldsfld System.Int32 $this::counter
IL_013b: ldc.i4.1
IL_013c: sub
IL_013d: stsfld System.Int32 $this::counter
IL_0142: endfinally
IL_0143: endfinally
IL_0144: br.s IL_0144
IL_0146: br IL_014b
IL_014b: br IL_0150
IL_0150: ldloc.s 4
IL_0152: ret
IL_0153: ldarg.0
IL_0154: ret
