.method System.Int32 $this::conversions(System.Double, System.Int64, System.Single, System.Int32)
.maxstack 2
.initlocals true
IL_0000: ldarg.0
IL_0001: conv.r8
IL_0002: call System.Int32 IKVM.Runtime.ByteCodeHelper::d2i(System.Double)
IL_0007: ldarg.1
IL_0008: conv.i4
IL_0009: add
IL_000a: ldarg.2
IL_000b: conv.r4
IL_000c: call System.Int32 IKVM.Runtime.ByteCodeHelper::f2i(System.Single)
IL_0011: add
IL_0012: ldarg.3
IL_0013: conv.i1
IL_0014: add
IL_0015: ldarg.3
IL_0016: conv.u2
IL_0017: add
IL_0018: ldarg.3
IL_0019: conv.i2
IL_001a: add
IL_001b: ldarg.0
IL_001c: conv.r8
IL_001d: call System.Int64 IKVM.Runtime.ByteCodeHelper::d2l(System.Double)
IL_0022: conv.i4
IL_0023: add
IL_0024: ldarg.1
IL_0025: conv.r4
IL_0026: call System.Int32 IKVM.Runtime.ByteCodeHelper::f2i(System.Single)
IL_002b: add
IL_002c: ldarg.2
IL_002d: conv.r4
IL_002e: conv.r8
IL_002f: call System.Int32 IKVM.Runtime.ByteCodeHelper::d2i(System.Double)
IL_0034: add
IL_0035: ret
