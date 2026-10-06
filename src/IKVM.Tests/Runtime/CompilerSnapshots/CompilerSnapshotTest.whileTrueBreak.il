.method System.Int32 $this::whileTrueBreak(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Int32
IL_0000: ldc.i4.0
IL_0001: stloc.0
IL_0002: ldloc.0
IL_0003: ldarg.0
IL_0004: ble IL_000e
IL_0009: br IL_0017
IL_000e: ldloc.0
IL_000f: ldc.i4.2
IL_0010: add
IL_0011: stloc.0
IL_0012: br IL_0002
IL_0017: ldloc.0
IL_0018: ret
