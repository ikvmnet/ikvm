.method System.Int32 $this::m(System.Int32)
.maxstack 5
.initlocals true
.local [0] System.Int32
IL_0000: ldc.i4 0
IL_0005: br IL_0016
IL_000a: ldloc.0
IL_000b: ldarg.0
IL_000c: add
IL_000d: ldarg.0
IL_000e: ldc.i4 -1
IL_0013: add
IL_0014: starg.s 0
IL_0016: ldarg.0
IL_0017: brtrue IL_001e
IL_001c: ret
IL_001d: ldloc.0
IL_001e: stloc.0
IL_001f: br IL_000a
