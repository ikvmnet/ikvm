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
IL_0017: ldc.i4.2
IL_0018: newarr System.Int32
IL_001d: stloc.3
IL_001e: stloc.s 4
IL_0020: ldloc.3
IL_0021: ldc.i4.1
IL_0022: ldloc.s 4
IL_0024: stelem.i4
IL_0025: stloc.s 4
IL_0027: ldloc.3
IL_0028: ldc.i4.0
IL_0029: ldloc.s 4
IL_002b: stelem.i4
IL_002c: ldtoken System.Int32[][]
IL_0031: ldloc.3
IL_0032: call System.Object IKVM.Runtime.ByteCodeHelper::multianewarray(System.RuntimeTypeHandle, System.Int32[])
IL_0037: castclass System.Int32[][]
IL_003c: stloc.s 5
IL_003e: ldloc.0
IL_003f: ldc.i4.0
IL_0040: ldloc.s 5
IL_0042: ldlen
IL_0043: stelem.i4
IL_0044: ldloc.1
IL_0045: ldc.i4.0
IL_0046: ldloc.0
IL_0047: ldc.i4.0
IL_0048: ldelem.i4
IL_0049: conv.i8
IL_004a: stelem.i8
IL_004b: ldloc.2
IL_004c: ldc.i4.0
IL_004d: ldstr "x"
IL_0052: stelem.ref
IL_0053: ldloc.s 5
IL_0055: ldc.i4.0
IL_0056: ldelem.ref
IL_0057: ldc.i4.0
IL_0058: ldloc.0
IL_0059: ldlen
IL_005a: stelem.i4
IL_005b: ldloc.2
IL_005c: ret
