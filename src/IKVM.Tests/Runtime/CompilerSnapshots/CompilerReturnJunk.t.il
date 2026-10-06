.method System.Int32 $this::t()
.maxstack 2
.initlocals true
.local [0] System.Int32
.try IL_0000 to IL_0012 catch System.Exception handler IL_0012 to IL_0029
IL_0000: ldc.i4.5
IL_0001: call System.Void $this::f()
IL_0006: ldc.i4.1
IL_0007: stloc.0
IL_0008: leave IL_002b
IL_000d: leave IL_0029
IL_0012: ldc.i4.2
IL_0013: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0018: brtrue IL_001f
IL_001d: rethrow
IL_001f: leave IL_002d
IL_0024: leave IL_0029
IL_0029: br.s IL_0029
IL_002b: ldloc.0
IL_002c: ret
IL_002d: ldnull
IL_002e: br IL_0033
IL_0033: pop
IL_0034: ldc.i4.m1
IL_0035: ret
