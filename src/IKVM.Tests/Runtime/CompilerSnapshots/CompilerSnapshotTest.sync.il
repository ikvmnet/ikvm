.method System.Void $this::sync(System.Object)
.maxstack 2
.initlocals true
.local [0] System.Object
.try IL_0008 to IL_001c finally handler IL_001c to IL_0024
// line 56
IL_0000: ldarg.0
IL_0001: dup
IL_0002: stloc.0
IL_0003: call System.Void System.Threading.Monitor::Enter(System.Object)
IL_0008: call System.Void $this::f()
IL_000d: br IL_0012
IL_0012: leave IL_0026
IL_0017: leave IL_0024
IL_001c: ldloc.0
IL_001d: call System.Void System.Threading.Monitor::Exit(System.Object)
IL_0022: endfinally
IL_0023: endfinally
IL_0024: br.s IL_0024
IL_0026: br IL_002b
IL_002b: br IL_0030
IL_0030: ret
