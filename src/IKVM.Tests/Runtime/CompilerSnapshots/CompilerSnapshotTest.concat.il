.method System.String $this::concat(System.String, System.Int32, System.Int64, System.Object)
.maxstack 2
.initlocals true
// line 76
IL_0000: newobj instance void java.lang.StringBuilder::.ctor()
IL_0005: ldarg.0
IL_0006: callvirt instance java.lang.StringBuilder java.lang.StringBuilder::append(System.String)
IL_000b: ldarg.1
IL_000c: callvirt instance java.lang.StringBuilder java.lang.StringBuilder::append(System.Int32)
IL_0011: ldarg.2
IL_0012: callvirt instance java.lang.StringBuilder java.lang.StringBuilder::append(System.Int64)
IL_0017: ldarg.3
IL_0018: callvirt instance java.lang.StringBuilder java.lang.StringBuilder::append(System.Object)
IL_001d: callvirt instance System.String java.lang.StringBuilder::toString()
IL_0022: ldnull
IL_0023: pop
IL_0024: ret
