.method System.Int32 $this::tryCatchFinally(System.Int32)
.maxstack 22
.initlocals true
.local [0] java.lang.IllegalStateException
.local [1] System.Int32
.local [2] java.lang.RuntimeException
.try IL_0000 to IL_0014 catch java.lang.IllegalStateException handler IL_0014 to IL_001e
.try IL_0000 to IL_0025 catch System.Exception handler IL_0025 to IL_0040
.try IL_0000 to IL_0047 fault handler IL_0047 to IL_0059
.try IL_0082 to IL_0099 fault handler IL_0099 to IL_00ab
.try IL_00c5 to IL_00dc finally handler IL_00dc to IL_00ee
// line 47
IL_0000: call System.Void $this::f()
IL_0005: br IL_000a
IL_000a: leave IL_005b
IL_000f: leave IL_001e
IL_0014: leave IL_0060
IL_0019: leave IL_001e
IL_001e: br.s IL_001e
IL_0020: leave IL_0040
IL_0025: ldc.i4 2
IL_002a: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_002f: brtrue IL_0036
IL_0034: rethrow
IL_0036: leave IL_0066
IL_003b: leave IL_0040
IL_0040: br.s IL_0040
IL_0042: leave IL_0059
IL_0047: ldsfld System.Int32 $this::counter
IL_004c: ldc.i4 1
IL_0051: add
IL_0052: stsfld System.Int32 $this::counter
IL_0057: endfinally
IL_0058: endfinally
IL_0059: br.s IL_0059
IL_005b: br IL_006c
IL_0060: ldnull
IL_0061: br IL_0081
IL_0066: ldnull
IL_0067: br IL_00c4
IL_006c: ldsfld System.Int32 $this::counter
IL_0071: ldc.i4 1
IL_0076: add
IL_0077: stsfld System.Int32 $this::counter
IL_007c: br IL_00fc
IL_0081: stloc.0
IL_0082: ldloc.0
IL_0083: pop
IL_0084: ldc.i4 1
IL_0089: stloc.1
IL_008a: br IL_008f
IL_008f: leave IL_00ad
IL_0094: leave IL_00ab
IL_0099: ldsfld System.Int32 $this::counter
IL_009e: ldc.i4 1
IL_00a3: add
IL_00a4: stsfld System.Int32 $this::counter
IL_00a9: endfinally
IL_00aa: endfinally
IL_00ab: br.s IL_00ab
IL_00ad: br IL_00b2
IL_00b2: ldsfld System.Int32 $this::counter
IL_00b7: ldc.i4 1
IL_00bc: add
IL_00bd: stsfld System.Int32 $this::counter
IL_00c2: ldloc.1
IL_00c3: ret
IL_00c4: stloc.2
IL_00c5: ldloc.2
IL_00c6: pop
IL_00c7: ldc.i4 2
IL_00cc: stloc.1
IL_00cd: br IL_00d2
IL_00d2: leave IL_00f0
IL_00d7: leave IL_00ee
IL_00dc: ldsfld System.Int32 $this::counter
IL_00e1: ldc.i4 1
IL_00e6: add
IL_00e7: stsfld System.Int32 $this::counter
IL_00ec: endfinally
IL_00ed: endfinally
IL_00ee: br.s IL_00ee
IL_00f0: br IL_00f5
IL_00f5: br IL_00fa
IL_00fa: ldloc.1
IL_00fb: ret
IL_00fc: ldarg.0
IL_00fd: ret
