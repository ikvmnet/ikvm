.method System.Int32 $this::staticSync(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Object
.local [1] System.Int32
.try IL_0011 to IL_0022 finally handler IL_0022 to IL_002a
IL_0000: ldtoken $this
IL_0005: call java.lang.Class ikvm.runtime.Util::getClassFromTypeHandle(System.RuntimeTypeHandle)
IL_000a: dup
IL_000b: stloc.0
IL_000c: call System.Void System.Threading.Monitor::Enter(System.Object)
// line 60
IL_0011: call System.Void $this::f()
IL_0016: ldarg.0
IL_0017: stloc.1
IL_0018: leave IL_002c
IL_001d: leave IL_002a
IL_0022: ldloc.0
IL_0023: call System.Void System.Threading.Monitor::Exit(System.Object)
IL_0028: endfinally
IL_0029: endfinally
IL_002a: br.s IL_002a
IL_002c: ldloc.1
IL_002d: ret
