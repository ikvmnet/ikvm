.method System.Int32 $this::breakContinueInTry(System.Int32)
.maxstack 3
.initlocals true
.local [0] System.Int32
.try IL_0002 to IL_0021 fault handler IL_0021 to IL_002f
.try IL_004c to IL_0067 fault handler IL_0067 to IL_0075
.try IL_0092 to IL_00a6 catch System.Exception handler IL_00a6 to IL_00bd
.try IL_0092 to IL_00c4 fault handler IL_00c4 to IL_00d2
// line 51
IL_0000: ldc.i4.0
IL_0001: stloc.0
IL_0002: ldloc.0
IL_0003: ldloc.0
IL_0004: ldc.i4.1
IL_0005: add
IL_0006: stloc.0
IL_0007: ldarg.0
IL_0008: ble IL_0012
IL_000d: br IL_0017
IL_0012: leave IL_0031
IL_0017: leave IL_0036
IL_001c: leave IL_002f
IL_0021: ldsfld System.Int32 $this::counter
IL_0026: ldc.i4.1
IL_0027: add
IL_0028: stsfld System.Int32 $this::counter
IL_002d: endfinally
IL_002e: endfinally
IL_002f: br.s IL_002f
IL_0031: br IL_004c
IL_0036: br IL_003b
IL_003b: ldsfld System.Int32 $this::counter
IL_0040: ldc.i4.1
IL_0041: add
IL_0042: stsfld System.Int32 $this::counter
IL_0047: br IL_0107
IL_004c: ldloc.0
IL_004d: ldc.i4.2
IL_004e: bne.un IL_0058
IL_0053: br IL_005d
IL_0058: leave IL_0077
IL_005d: leave IL_007c
IL_0062: leave IL_0075
IL_0067: ldsfld System.Int32 $this::counter
IL_006c: ldc.i4.1
IL_006d: add
IL_006e: stsfld System.Int32 $this::counter
IL_0073: endfinally
IL_0074: endfinally
IL_0075: br.s IL_0075
IL_0077: br IL_0092
IL_007c: br IL_0081
IL_0081: ldsfld System.Int32 $this::counter
IL_0086: ldc.i4.1
IL_0087: add
IL_0088: stsfld System.Int32 $this::counter
IL_008d: br IL_0002
IL_0092: call System.Void $this::f()
IL_0097: br IL_009c
IL_009c: leave IL_00d4
IL_00a1: leave IL_00bd
IL_00a6: ldc.i4.2
IL_00a7: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_00ac: brtrue IL_00b3
IL_00b1: rethrow
IL_00b3: leave IL_00d9
IL_00b8: leave IL_00bd
IL_00bd: br.s IL_00bd
IL_00bf: leave IL_00d2
IL_00c4: ldsfld System.Int32 $this::counter
IL_00c9: ldc.i4.1
IL_00ca: add
IL_00cb: stsfld System.Int32 $this::counter
IL_00d0: endfinally
IL_00d1: endfinally
IL_00d2: br.s IL_00d2
IL_00d4: br IL_00df
IL_00d9: ldnull
IL_00da: br IL_00f0
IL_00df: ldsfld System.Int32 $this::counter
IL_00e4: ldc.i4.1
IL_00e5: add
IL_00e6: stsfld System.Int32 $this::counter
IL_00eb: br IL_0102
IL_00f0: pop
IL_00f1: ldsfld System.Int32 $this::counter
IL_00f6: ldc.i4.1
IL_00f7: add
IL_00f8: stsfld System.Int32 $this::counter
IL_00fd: br IL_0002
IL_0102: br IL_0002
IL_0107: ldloc.0
IL_0108: ret
