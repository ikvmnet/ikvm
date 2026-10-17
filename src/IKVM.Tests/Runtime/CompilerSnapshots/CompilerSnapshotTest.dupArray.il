.method System.Int32 $this::dupArray(System.Int32[], System.Int32)
.maxstack 6
.initlocals true
.local [0] System.Int32
.local [1] System.Int32[]
.local [2] System.Int32
// line 84
IL_0000: ldarg.0
IL_0001: ldarg.1
IL_0002: stloc.0
IL_0003: stloc.1
IL_0004: ldloc.1
IL_0005: ldloc.0
IL_0006: ldloc.1
IL_0007: ldloc.0
IL_0008: ldelem.i4
IL_0009: stloc.0
IL_000a: stloc.2
IL_000b: stloc.1
IL_000c: ldloc.0
IL_000d: ldloc.1
IL_000e: ldloc.2
IL_000f: ldloc.0
IL_0010: ldc.i4.1
IL_0011: add
IL_0012: stelem.i4
IL_0013: ldarg.0
IL_0014: ldarg.1
IL_0015: stloc.0
IL_0016: stloc.1
IL_0017: ldloc.1
IL_0018: ldloc.0
IL_0019: ldloc.1
IL_001a: ldloc.0
IL_001b: ldelem.i4
IL_001c: stloc.0
IL_001d: stloc.2
IL_001e: stloc.1
IL_001f: ldloc.0
IL_0020: ldloc.1
IL_0021: ldloc.2
IL_0022: ldloc.0
IL_0023: ldc.i4.1
IL_0024: sub
IL_0025: stelem.i4
IL_0026: add
IL_0027: ret
