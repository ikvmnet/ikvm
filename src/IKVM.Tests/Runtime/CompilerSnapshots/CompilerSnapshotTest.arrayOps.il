.method System.Int32 $this::arrayOps(System.Byte[], System.Char[], System.Int16[], System.Boolean[], System.Single[], System.Double[])
.maxstack 3
.initlocals true
// line 68
IL_0000: ldarg.0
IL_0001: ldc.i4.0
IL_0002: ldc.i4.1
IL_0003: stelem.i1
IL_0004: ldarg.1
IL_0005: ldc.i4.0
IL_0006: ldc.i4.s 99
IL_0008: stelem.i2
IL_0009: ldarg.2
IL_000a: ldc.i4.0
IL_000b: ldc.i4.2
IL_000c: stelem.i2
IL_000d: ldarg.3
IL_000e: ldc.i4.0
IL_000f: ldc.i4.1
IL_0010: stelem.i1
IL_0011: ldarg.s 4
IL_0013: ldc.i4.0
IL_0014: ldc.r4 1.5
IL_0019: stelem.r4
IL_001a: ldarg.s 5
IL_001c: ldc.i4.0
IL_001d: ldc.r8 2.5
IL_0026: stelem.r8
IL_0027: ldarg.0
IL_0028: ldc.i4.0
IL_0029: ldelem.i1
IL_002a: ldarg.1
IL_002b: ldc.i4.0
IL_002c: ldelem.u2
IL_002d: add
IL_002e: ldarg.2
IL_002f: ldc.i4.0
IL_0030: ldelem.i2
IL_0031: add
IL_0032: ldarg.3
IL_0033: ldc.i4.0
IL_0034: ldelem.i1
IL_0035: brfalse IL_0040
IL_003a: ldc.i4.1
IL_003b: br IL_0041
IL_0040: ldc.i4.0
IL_0041: add
IL_0042: ldarg.s 4
IL_0044: ldc.i4.0
IL_0045: ldelem.r4
IL_0046: call System.Int32 IKVM.Runtime.ByteCodeHelper::f2i(System.Single)
IL_004b: add
IL_004c: ldarg.s 5
IL_004e: ldc.i4.0
IL_004f: ldelem.r8
IL_0050: call System.Int32 IKVM.Runtime.ByteCodeHelper::d2i(System.Double)
IL_0055: add
IL_0056: ret
