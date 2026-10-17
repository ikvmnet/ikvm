.method System.Int32 $this::tryCatchFinally(System.Int32)
.maxstack 2
.initlocals true
.local [0] java.lang.IllegalStateException
.local [1] System.Int32
.local [2] java.lang.RuntimeException
.try IL_0000 to IL_0014 catch java.lang.IllegalStateException handler IL_0014 to IL_001e
.try IL_0000 to IL_0025 catch System.Exception handler IL_0025 to IL_003c
.try IL_0000 to IL_0043 fault handler IL_0043 to IL_0051
.try IL_0076 to IL_0089 fault handler IL_0089 to IL_0097
.try IL_00ad to IL_00c0 finally handler IL_00c0 to IL_00ce
// line 47
IL_0000: call System.Void $this::f()
IL_0005: br IL_000a
IL_000a: leave IL_0053
IL_000f: leave IL_001e
IL_0014: leave IL_0058
IL_0019: leave IL_001e
IL_001e: br.s IL_001e
IL_0020: leave IL_003c
IL_0025: ldc.i4.2
IL_0026: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_002b: brtrue IL_0032
IL_0030: rethrow
IL_0032: leave IL_005e
IL_0037: leave IL_003c
IL_003c: br.s IL_003c
IL_003e: leave IL_0051
IL_0043: ldsfld System.Int32 $this::counter
IL_0048: ldc.i4.1
IL_0049: add
IL_004a: stsfld System.Int32 $this::counter
IL_004f: endfinally
IL_0050: endfinally
IL_0051: br.s IL_0051
IL_0053: br IL_0064
IL_0058: ldnull
IL_0059: br IL_0075
IL_005e: ldnull
IL_005f: br IL_00ac
IL_0064: ldsfld System.Int32 $this::counter
IL_0069: ldc.i4.1
IL_006a: add
IL_006b: stsfld System.Int32 $this::counter
IL_0070: br IL_00dc
IL_0075: stloc.0
IL_0076: ldloc.0
IL_0077: pop
IL_0078: ldc.i4.1
IL_0079: stloc.1
IL_007a: br IL_007f
IL_007f: leave IL_0099
IL_0084: leave IL_0097
IL_0089: ldsfld System.Int32 $this::counter
IL_008e: ldc.i4.1
IL_008f: add
IL_0090: stsfld System.Int32 $this::counter
IL_0095: endfinally
IL_0096: endfinally
IL_0097: br.s IL_0097
IL_0099: br IL_009e
IL_009e: ldsfld System.Int32 $this::counter
IL_00a3: ldc.i4.1
IL_00a4: add
IL_00a5: stsfld System.Int32 $this::counter
IL_00aa: ldloc.1
IL_00ab: ret
IL_00ac: stloc.2
IL_00ad: ldloc.2
IL_00ae: pop
IL_00af: ldc.i4.2
IL_00b0: stloc.1
IL_00b1: br IL_00b6
IL_00b6: leave IL_00d0
IL_00bb: leave IL_00ce
IL_00c0: ldsfld System.Int32 $this::counter
IL_00c5: ldc.i4.1
IL_00c6: add
IL_00c7: stsfld System.Int32 $this::counter
IL_00cc: endfinally
IL_00cd: endfinally
IL_00ce: br.s IL_00ce
IL_00d0: br IL_00d5
IL_00d5: br IL_00da
IL_00da: ldloc.1
IL_00db: ret
IL_00dc: ldarg.0
IL_00dd: ret
