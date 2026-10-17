.method System.Int32 $this::loop(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Int32
.local [1] System.Int32
IL_0000: ldc.i4.0
IL_0001: stloc.0
IL_0002: ldc.i4.0
IL_0003: stloc.1
IL_0004: ldloc.1
IL_0005: ldarg.0
IL_0006: bge IL_0018
IL_000b: ldloc.0
IL_000c: ldloc.1
IL_000d: add
IL_000e: stloc.0
IL_000f: ldloc.1
IL_0010: ldc.i4.1
IL_0011: add
IL_0012: stloc.1
IL_0013: br IL_0004
IL_0018: ldloc.0
IL_0019: ret
