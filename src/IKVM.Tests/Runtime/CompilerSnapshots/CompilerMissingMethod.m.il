.method System.Int32 $this::m(System.Int32)
.maxstack 2
.initlocals true
IL_0000: ldarg.0
IL_0001: brtrue IL_0008
IL_0006: ldc.i4.1
IL_0007: ret
IL_0008: ldstr "CompilerMissingMethod.missing()V"
IL_000d: newobj instance void java.lang.NoSuchMethodError::.ctor(System.String)
IL_0012: throw
