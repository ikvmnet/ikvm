.method instance void $this::.ctor(System.Int32)
.maxstack 5
.initlocals true
// line 106
IL_0000: ldarg.0
IL_0001: call instance void java.lang.Object::.ctor()
IL_0006: ldarg.1
IL_0007: ldc.i4.0
IL_0008: ble IL_001e
IL_000d: call System.Void java.lang.Throwable::__<suppressFillInStackTrace>()
IL_0012: newobj instance void java.lang.IllegalArgumentException::.ctor()
IL_0017: ldarg.0
IL_0018: call System.Void System.GC::KeepAlive(System.Object)
IL_001d: throw
IL_001e: ldarg.0
IL_001f: call System.Void System.GC::KeepAlive(System.Object)
IL_0024: ret
