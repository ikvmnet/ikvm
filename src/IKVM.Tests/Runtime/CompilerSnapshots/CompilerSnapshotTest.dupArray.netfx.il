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
IL_0010: ldc.i4 1
IL_0015: add
IL_0016: stelem.i4
IL_0017: ldarg.0
IL_0018: ldarg.1
IL_0019: stloc.0
IL_001a: stloc.1
IL_001b: ldloc.1
IL_001c: ldloc.0
IL_001d: ldloc.1
IL_001e: ldloc.0
IL_001f: ldelem.i4
IL_0020: stloc.0
IL_0021: stloc.2
IL_0022: stloc.1
IL_0023: ldloc.0
IL_0024: ldloc.1
IL_0025: ldloc.2
IL_0026: ldloc.0
IL_0027: ldc.i4 1
IL_002c: sub
IL_002d: stelem.i4
IL_002e: add
IL_002f: ret
