.method System.Int32 $this::m(System.Int32)
.maxstack 2
.initlocals true
.local [0] System.Int32
.try IL_0002 to IL_001b catch System.Exception handler IL_001b to IL_0032
IL_0000: ldc.i4.5
IL_0001: stloc.0
IL_0002: ldloc.0
IL_0003: call System.Void $this::f()
IL_0008: ldarg.0
IL_0009: add
IL_000a: br IL_0010
IL_000f: ldloc.0
IL_0010: stloc.0
IL_0011: leave IL_0034
IL_0016: leave IL_0032
IL_001b: ldc.i4.2
IL_001c: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0021: brtrue IL_0028
IL_0026: rethrow
IL_0028: leave IL_003a
IL_002d: leave IL_0032
IL_0032: br.s IL_0032
IL_0034: ldloc.0
IL_0035: br IL_0040
IL_003a: ldnull
IL_003b: br IL_0041
IL_0040: ret
IL_0041: pop
IL_0042: ldc.i4.m1
IL_0043: ret
