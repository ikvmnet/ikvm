.method System.Void $this::throwNew()
.maxstack 2
.initlocals true
// line 72
IL_0000: ldstr "x"
IL_0005: call System.Void java.lang.Throwable::__<suppressFillInStackTrace>()
IL_000a: newobj instance void java.lang.IllegalStateException::.ctor(System.String)
IL_000f: throw
