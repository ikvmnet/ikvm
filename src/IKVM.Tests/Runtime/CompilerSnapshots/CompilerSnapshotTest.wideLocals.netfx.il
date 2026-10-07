.method System.Int64 $this::wideLocals(System.Int64, System.Double)
.maxstack 2
.initlocals true
.local [0] System.Int64
.local [1] System.Double
IL_0000: ldarg.0
IL_0001: stloc.0
IL_0002: ldarg.1
IL_0003: conv.r8
IL_0004: stloc.1
IL_0005: ldloc.0
IL_0006: ldc.i8 1
IL_000f: add
IL_0010: stloc.0
IL_0011: ldloc.1
IL_0012: ldc.r8 2
IL_001b: mul
IL_001c: conv.r8
IL_001d: stloc.1
IL_001e: ldloc.0
IL_001f: ldloc.1
IL_0020: call System.Int64 IKVM.Runtime.ByteCodeHelper::d2l(System.Double)
IL_0025: add
IL_0026: ret
