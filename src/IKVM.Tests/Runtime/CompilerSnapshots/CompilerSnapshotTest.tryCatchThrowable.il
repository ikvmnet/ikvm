.method System.Int32 $this::tryCatchThrowable(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Int32
.try IL_0000 to IL_0017 catch System.Exception handler IL_0017 to IL_0027
// line 43
IL_0000: call System.Void $this::f()
IL_0005: ldarg.0
IL_0006: br IL_000c
IL_000b: ldloc.0
IL_000c: stloc.0
IL_000d: leave IL_0029
IL_0012: leave IL_0027
IL_0017: ldc.i4.2
IL_0018: call System.Exception IKVM.Runtime.ByteCodeHelper::MapException<System.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_001d: leave IL_002f
IL_0022: leave IL_0027
IL_0027: br.s IL_0027
IL_0029: ldloc.0
IL_002a: br IL_0035
IL_002f: ldnull
IL_0030: br IL_0036
IL_0035: ret
IL_0036: pop
IL_0037: ldc.i4.m1
IL_0038: ret
