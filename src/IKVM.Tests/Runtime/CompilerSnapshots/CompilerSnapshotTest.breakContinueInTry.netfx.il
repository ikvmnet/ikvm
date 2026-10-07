.method System.Int32 $this::breakContinueInTry(System.Int32)
.maxstack 26
.initlocals true
.local [0] System.Int32
.try IL_0006 to IL_0029 fault handler IL_0029 to IL_003b
.try IL_005c to IL_007b fault handler IL_007b to IL_008d
.try IL_00ae to IL_00c2 catch System.Exception handler IL_00c2 to IL_00dd
.try IL_00ae to IL_00e4 fault handler IL_00e4 to IL_00f6
// line 51
IL_0000: ldc.i4 0
IL_0005: stloc.0
IL_0006: ldloc.0
IL_0007: ldloc.0
IL_0008: ldc.i4 1
IL_000d: add
IL_000e: stloc.0
IL_000f: ldarg.0
IL_0010: ble IL_001a
IL_0015: br IL_001f
IL_001a: leave IL_003d
IL_001f: leave IL_0042
IL_0024: leave IL_003b
IL_0029: ldsfld System.Int32 $this::counter
IL_002e: ldc.i4 1
IL_0033: add
IL_0034: stsfld System.Int32 $this::counter
IL_0039: endfinally
IL_003a: endfinally
IL_003b: br.s IL_003b
IL_003d: br IL_005c
IL_0042: br IL_0047
IL_0047: ldsfld System.Int32 $this::counter
IL_004c: ldc.i4 1
IL_0051: add
IL_0052: stsfld System.Int32 $this::counter
IL_0057: br IL_0133
IL_005c: ldloc.0
IL_005d: ldc.i4 2
IL_0062: bne.un IL_006c
IL_0067: br IL_0071
IL_006c: leave IL_008f
IL_0071: leave IL_0094
IL_0076: leave IL_008d
IL_007b: ldsfld System.Int32 $this::counter
IL_0080: ldc.i4 1
IL_0085: add
IL_0086: stsfld System.Int32 $this::counter
IL_008b: endfinally
IL_008c: endfinally
IL_008d: br.s IL_008d
IL_008f: br IL_00ae
IL_0094: br IL_0099
IL_0099: ldsfld System.Int32 $this::counter
IL_009e: ldc.i4 1
IL_00a3: add
IL_00a4: stsfld System.Int32 $this::counter
IL_00a9: br IL_0006
IL_00ae: call System.Void $this::f()
IL_00b3: br IL_00b8
IL_00b8: leave IL_00f8
IL_00bd: leave IL_00dd
IL_00c2: ldc.i4 2
IL_00c7: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_00cc: brtrue IL_00d3
IL_00d1: rethrow
IL_00d3: leave IL_00fd
IL_00d8: leave IL_00dd
IL_00dd: br.s IL_00dd
IL_00df: leave IL_00f6
IL_00e4: ldsfld System.Int32 $this::counter
IL_00e9: ldc.i4 1
IL_00ee: add
IL_00ef: stsfld System.Int32 $this::counter
IL_00f4: endfinally
IL_00f5: endfinally
IL_00f6: br.s IL_00f6
IL_00f8: br IL_0103
IL_00fd: ldnull
IL_00fe: br IL_0118
IL_0103: ldsfld System.Int32 $this::counter
IL_0108: ldc.i4 1
IL_010d: add
IL_010e: stsfld System.Int32 $this::counter
IL_0113: br IL_012e
IL_0118: pop
IL_0119: ldsfld System.Int32 $this::counter
IL_011e: ldc.i4 1
IL_0123: add
IL_0124: stsfld System.Int32 $this::counter
IL_0129: br IL_0006
IL_012e: br IL_0006
IL_0133: ldloc.0
IL_0134: ret
