.method System.Int32 $this::compares(System.Int64, System.Int64, System.Double, System.Double, System.Single, System.Single)
.maxstack 2
.initlocals true
.local [0] System.Int32
IL_0000: ldc.i4 0
IL_0005: stloc.0
IL_0006: ldarg.0
IL_0007: ldarg.1
IL_0008: bge IL_0015
IL_000d: ldloc.0
IL_000e: ldc.i4 1
IL_0013: add
IL_0014: stloc.0
IL_0015: ldarg.2
IL_0016: conv.r8
IL_0017: ldarg.3
IL_0018: conv.r8
IL_0019: ble.un IL_0026
IL_001e: ldloc.0
IL_001f: ldc.i4 2
IL_0024: add
IL_0025: stloc.0
IL_0026: ldarg.2
IL_0027: conv.r8
IL_0028: ldarg.3
IL_0029: conv.r8
IL_002a: bgt.un IL_0037
IL_002f: ldloc.0
IL_0030: ldc.i4 4
IL_0035: add
IL_0036: stloc.0
IL_0037: ldarg.s 4
IL_0039: conv.r4
IL_003a: ldarg.s 5
IL_003c: conv.r4
IL_003d: bge.un IL_004a
IL_0042: ldloc.0
IL_0043: ldc.i4 8
IL_0048: add
IL_0049: stloc.0
IL_004a: ldarg.s 4
IL_004c: conv.r4
IL_004d: ldarg.s 5
IL_004f: conv.r4
IL_0050: blt.un IL_005d
IL_0055: ldloc.0
IL_0056: ldc.i4 16
IL_005b: add
IL_005c: stloc.0
IL_005d: ldloc.0
IL_005e: ret
