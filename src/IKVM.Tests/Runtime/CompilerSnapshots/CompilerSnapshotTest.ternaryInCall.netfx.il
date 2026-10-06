.method System.Int32 $this::ternaryInCall(System.Boolean, System.Int32)
.maxstack 3
.initlocals true
.local [0] System.Int32
IL_0000: ldarg.0
IL_0001: stloc.0
// line 82
IL_0002: ldloc.0
IL_0003: brfalse IL_000e
IL_0008: ldarg.1
IL_0009: br IL_0010
IL_000e: ldarg.1
IL_000f: neg
IL_0010: call System.Int32 $this::g(System.Int32)
IL_0015: ldarg.1
IL_0016: call System.Int32 $this::g(System.Int32)
IL_001b: add
IL_001c: ret
