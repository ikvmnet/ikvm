.method System.Int32 $this::syncReturn(System.Object, System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Object
.local [1] System.Int32
.try IL_0008 to IL_0036 fault handler IL_0036 to IL_003e
// line 58
IL_0000: ldarg.0
IL_0001: dup
IL_0002: stloc.0
IL_0003: call System.Void System.Threading.Monitor::Enter(System.Object)
IL_0008: ldarg.1
IL_0009: ldc.i4.0
IL_000a: ble IL_001c
IL_000f: ldarg.1
IL_0010: ldloc.0
IL_0011: call System.Void System.Threading.Monitor::Exit(System.Object)
IL_0016: stloc.1
IL_0017: leave IL_0040
IL_001c: call System.Void $this::f()
IL_0021: ldloc.0
IL_0022: call System.Void System.Threading.Monitor::Exit(System.Object)
IL_0027: br IL_002c
IL_002c: leave IL_0042
IL_0031: leave IL_003e
IL_0036: ldloc.0
IL_0037: call System.Void System.Threading.Monitor::Exit(System.Object)
IL_003c: endfinally
IL_003d: endfinally
IL_003e: br.s IL_003e
IL_0040: ldloc.1
IL_0041: ret
IL_0042: br IL_0047
IL_0047: br IL_004c
IL_004c: ldc.i4.0
IL_004d: ret
