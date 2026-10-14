.method System.Int32 $this::loop(System.Int32)
.maxstack 3
.initlocals true
.local [0] System.Int32
.local [1] System.Int32
IL_0000: ldc.i4 0
IL_0005: stloc.0
IL_0006: ldc.i4 0
IL_000b: stloc.1
IL_000c: ldloc.1
IL_000d: ldarg.0
IL_000e: bge IL_0024
IL_0013: ldloc.0
IL_0014: ldloc.1
IL_0015: add
IL_0016: stloc.0
IL_0017: ldloc.1
IL_0018: ldc.i4 1
IL_001d: add
IL_001e: stloc.1
IL_001f: br IL_000c
IL_0024: ldloc.0
IL_0025: ret
