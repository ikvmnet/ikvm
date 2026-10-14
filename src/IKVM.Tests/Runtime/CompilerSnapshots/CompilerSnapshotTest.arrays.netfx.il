.method System.Object $this::arrays(System.Int32)
.maxstack 4
.initlocals true
.local [0] System.Int32[]
.local [1] System.Int64[]
.local [2] System.String[]
.local [3] System.Int32[]
.local [4] System.Int32
.local [5] System.Int32[][]
// line 66
IL_0000: ldarg.0
IL_0001: newarr System.Int32
IL_0006: stloc.0
IL_0007: ldarg.0
IL_0008: newarr System.Int64
IL_000d: stloc.1
IL_000e: ldarg.0
IL_000f: newarr System.String
IL_0014: stloc.2
IL_0015: ldarg.0
IL_0016: ldarg.0
IL_0017: ldc.i4 2
IL_001c: newarr System.Int32
IL_0021: stloc.3
IL_0022: stloc.s 4
IL_0024: ldloc.3
IL_0025: ldc.i4 1
IL_002a: ldloc.s 4
IL_002c: stelem.i4
IL_002d: stloc.s 4
IL_002f: ldloc.3
IL_0030: ldc.i4 0
IL_0035: ldloc.s 4
IL_0037: stelem.i4
IL_0038: ldtoken System.Int32[][]
IL_003d: ldloc.3
IL_003e: call System.Object IKVM.Runtime.ByteCodeHelper::multianewarray(System.RuntimeTypeHandle, System.Int32[])
IL_0043: castclass System.Int32[][]
IL_0048: stloc.s 5
IL_004a: ldloc.0
IL_004b: ldc.i4 0
IL_0050: ldloc.s 5
IL_0052: ldlen
IL_0053: stelem.i4
IL_0054: ldloc.1
IL_0055: ldc.i4 0
IL_005a: ldloc.0
IL_005b: ldc.i4 0
IL_0060: ldelem.i4
IL_0061: conv.i8
IL_0062: stelem.i8
IL_0063: ldloc.2
IL_0064: ldc.i4 0
IL_0069: ldstr "x"
IL_006e: stelem.ref
IL_006f: ldloc.s 5
IL_0071: ldc.i4 0
IL_0076: ldelem.ref
IL_0077: ldc.i4 0
IL_007c: ldloc.0
IL_007d: ldlen
IL_007e: stelem.i4
IL_007f: ldloc.2
IL_0080: ret
