.method System.Int32 $this::arrayOps(System.Byte[], System.Char[], System.Int16[], System.Boolean[], System.Single[], System.Double[])
.maxstack 5
.initlocals true
// line 68
IL_0000: ldarg.0
IL_0001: ldc.i4 0
IL_0006: ldc.i4 1
IL_000b: stelem.i1
IL_000c: ldarg.1
IL_000d: ldc.i4 0
IL_0012: ldc.i4 99
IL_0017: stelem.i2
IL_0018: ldarg.2
IL_0019: ldc.i4 0
IL_001e: ldc.i4 2
IL_0023: stelem.i2
IL_0024: ldarg.3
IL_0025: ldc.i4 0
IL_002a: ldc.i4 1
IL_002f: stelem.i1
IL_0030: ldarg.s 4
IL_0032: ldc.i4 0
IL_0037: ldc.r4 1.5
IL_003c: stelem.r4
IL_003d: ldarg.s 5
IL_003f: ldc.i4 0
IL_0044: ldc.r8 2.5
IL_004d: stelem.r8
IL_004e: ldarg.0
IL_004f: ldc.i4 0
IL_0054: ldelem.i1
IL_0055: ldarg.1
IL_0056: ldc.i4 0
IL_005b: ldelem.u2
IL_005c: add
IL_005d: ldarg.2
IL_005e: ldc.i4 0
IL_0063: ldelem.i2
IL_0064: add
IL_0065: ldarg.3
IL_0066: ldc.i4 0
IL_006b: ldelem.i1
IL_006c: brfalse IL_007b
IL_0071: ldc.i4 1
IL_0076: br IL_0080
IL_007b: ldc.i4 0
IL_0080: add
IL_0081: ldarg.s 4
IL_0083: ldc.i4 0
IL_0088: ldelem.r4
IL_0089: call System.Int32 IKVM.Runtime.ByteCodeHelper::f2i(System.Single)
IL_008e: add
IL_008f: ldarg.s 5
IL_0091: ldc.i4 0
IL_0096: ldelem.r8
IL_0097: call System.Int32 IKVM.Runtime.ByteCodeHelper::d2i(System.Double)
IL_009c: add
IL_009d: ret
