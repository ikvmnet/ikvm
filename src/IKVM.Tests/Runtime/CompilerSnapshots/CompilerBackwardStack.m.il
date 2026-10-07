.method System.Int32 $this::m(System.Int32)
.maxstack 3
.initlocals true
.local [0] System.Int32
IL_0000: ldc.i4.0
IL_0001: br IL_000e
IL_0006: ldloc.0
IL_0007: ldarg.0
IL_0008: add
IL_0009: ldarg.0
IL_000a: ldc.i4.m1
IL_000b: add
IL_000c: starg.s 0
IL_000e: ldarg.0
IL_000f: brtrue IL_0016
IL_0014: ret
IL_0015: ldloc.0
IL_0016: stloc.0
IL_0017: br IL_0006
