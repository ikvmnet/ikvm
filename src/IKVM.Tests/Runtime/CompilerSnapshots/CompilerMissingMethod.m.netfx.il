.method System.Int32 $this::m(System.Int32)
.maxstack 3
.initlocals true
IL_0000: ldarg.0
IL_0001: brtrue IL_000c
IL_0006: ldc.i4 1
IL_000b: ret
IL_000c: ldstr "CompilerMissingMethod.missing()V"
IL_0011: newobj instance void java.lang.NoSuchMethodError::.ctor(System.String)
IL_0016: throw
