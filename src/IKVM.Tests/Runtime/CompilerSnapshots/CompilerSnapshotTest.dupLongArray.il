.method System.Int64 $this::dupLongArray(System.Int64[], System.Int32)
.maxstack 5
.initlocals true
.local [0] System.Int32
.local [1] System.Int64[]
.local [2] System.Int64
// line 86
IL_0000: ldarg.0
IL_0001: ldarg.1
IL_0002: stloc.0
IL_0003: stloc.1
IL_0004: ldloc.1
IL_0005: ldloc.0
IL_0006: ldloc.1
IL_0007: ldloc.0
IL_0008: ldelem.i8
IL_0009: stloc.2
IL_000a: stloc.0
IL_000b: stloc.1
IL_000c: ldloc.2
IL_000d: ldloc.1
IL_000e: ldloc.0
IL_000f: ldloc.2
IL_0010: ldc.i8 1
IL_0019: add
IL_001a: stelem.i8
IL_001b: ldarg.0
IL_001c: ldarg.1
IL_001d: stloc.0
IL_001e: stloc.1
IL_001f: ldloc.1
IL_0020: ldloc.0
IL_0021: ldloc.1
IL_0022: ldloc.0
IL_0023: ldelem.i8
IL_0024: ldc.i8 1
IL_002d: sub
IL_002e: stloc.2
IL_002f: stloc.0
IL_0030: stloc.1
IL_0031: ldloc.2
IL_0032: ldloc.1
IL_0033: ldloc.0
IL_0034: ldloc.2
IL_0035: stelem.i8
IL_0036: add
IL_0037: ret
