.method System.Int32 $this::nestedTry(System.Int32)
.maxstack 33
.initlocals true
.local [0] System.Exception
.local [1] System.Exception
.local [2] System.Exception
.local [3] java.lang.Exception
.local [4] System.Int32
.try IL_0000 to IL_0014 catch System.Exception handler IL_0014 to IL_0029
.try IL_0000 to IL_0030 catch System.Exception handler IL_0030 to IL_004b
.try IL_0000 to IL_0052 fault handler IL_0052 to IL_0064
.try IL_0077 to IL_0096 catch System.Exception handler IL_0096 to IL_00b1
.try IL_0077 to IL_00b8 fault handler IL_00b8 to IL_00ca
.try IL_00da to IL_00f8 catch System.Exception handler IL_00f8 to IL_0113
.try IL_00d8 to IL_011a fault handler IL_011a to IL_012c
.try IL_014a to IL_0162 finally handler IL_0162 to IL_0174
// line 49
IL_0000: call System.Void $this::f()
IL_0005: br IL_000a
IL_000a: leave IL_0066
IL_000f: leave IL_0029
IL_0014: ldc.i4 0
IL_0019: call System.Exception IKVM.Runtime.ByteCodeHelper::MapException<System.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_001e: stloc.0
IL_001f: leave IL_006b
IL_0024: leave IL_0029
IL_0029: br.s IL_0029
IL_002b: leave IL_004b
IL_0030: ldc.i4 2
IL_0035: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_003a: brtrue IL_0041
IL_003f: rethrow
IL_0041: leave IL_0071
IL_0046: leave IL_004b
IL_004b: br.s IL_004b
IL_004d: leave IL_0064
IL_0052: ldsfld System.Int32 $this::counter
IL_0057: ldc.i4 1
IL_005c: sub
IL_005d: stsfld System.Int32 $this::counter
IL_0062: endfinally
IL_0063: endfinally
IL_0064: br.s IL_0064
IL_0066: br IL_0077
IL_006b: ldloc.0
IL_006c: br IL_00d7
IL_0071: ldnull
IL_0072: br IL_0149
IL_0077: ldsfld System.Int32 $this::counter
IL_007c: ldc.i4 1
IL_0081: add
IL_0082: stsfld System.Int32 $this::counter
IL_0087: br IL_008c
IL_008c: leave IL_00cc
IL_0091: leave IL_00b1
IL_0096: ldc.i4 2
IL_009b: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_00a0: brtrue IL_00a7
IL_00a5: rethrow
IL_00a7: leave IL_00d1
IL_00ac: leave IL_00b1
IL_00b1: br.s IL_00b1
IL_00b3: leave IL_00ca
IL_00b8: ldsfld System.Int32 $this::counter
IL_00bd: ldc.i4 1
IL_00c2: sub
IL_00c3: stsfld System.Int32 $this::counter
IL_00c8: endfinally
IL_00c9: endfinally
IL_00ca: br.s IL_00ca
IL_00cc: br IL_0134
IL_00d1: ldnull
IL_00d2: br IL_0149
IL_00d7: stloc.1
IL_00d8: ldloc.1
IL_00d9: stloc.1
IL_00da: ldloc.1
IL_00db: stloc.2
IL_00dc: ldsfld System.Int32 $this::counter
IL_00e1: ldc.i4 1
IL_00e6: add
IL_00e7: stsfld System.Int32 $this::counter
IL_00ec: ldloc.2
IL_00ed: call System.Exception java.lang.Throwable::__<unmap>(System.Exception)
IL_00f2: throw
IL_00f3: leave IL_0113
IL_00f8: ldc.i4 2
IL_00fd: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0102: brtrue IL_0109
IL_0107: rethrow
IL_0109: leave IL_012e
IL_010e: leave IL_0113
IL_0113: br.s IL_0113
IL_0115: leave IL_012c
IL_011a: ldsfld System.Int32 $this::counter
IL_011f: ldc.i4 1
IL_0124: sub
IL_0125: stsfld System.Int32 $this::counter
IL_012a: endfinally
IL_012b: endfinally
IL_012c: br.s IL_012c
IL_012e: ldnull
IL_012f: br IL_0149
IL_0134: ldsfld System.Int32 $this::counter
IL_0139: ldc.i4 1
IL_013e: sub
IL_013f: stsfld System.Int32 $this::counter
IL_0144: br IL_0183
IL_0149: stloc.3
IL_014a: ldloc.3
IL_014b: pop
IL_014c: ldc.i4 -1
IL_0151: stloc.s 4
IL_0153: br IL_0158
IL_0158: leave IL_0176
IL_015d: leave IL_0174
IL_0162: ldsfld System.Int32 $this::counter
IL_0167: ldc.i4 1
IL_016c: sub
IL_016d: stsfld System.Int32 $this::counter
IL_0172: endfinally
IL_0173: endfinally
IL_0174: br.s IL_0174
IL_0176: br IL_017b
IL_017b: br IL_0180
IL_0180: ldloc.s 4
IL_0182: ret
IL_0183: ldarg.0
IL_0184: ret
