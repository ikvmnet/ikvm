.method System.Object $this::newWithBranch(System.Boolean)
.maxstack 3
.initlocals true
.local [0] System.Int32
IL_0000: ldarg.0
IL_0001: stloc.0
// line 64
IL_0002: ldloc.0
IL_0003: brfalse IL_0012
IL_0008: ldstr "a"
IL_000d: br IL_0017
IL_0012: ldstr "b"
IL_0017: newobj instance void java.lang.StringBuilder::.ctor(System.String)
IL_001c: ldnull
IL_001d: pop
IL_001e: ret
