.method System.Int32 $this::t()
.maxstack 7
.initlocals true
.local [0] System.Int32
.try IL_0000 to IL_001a catch System.Exception handler IL_001a to IL_0035
IL_0000: ldc.i4 5
IL_0005: call System.Void $this::f()
IL_000a: ldc.i4 1
IL_000f: stloc.0
IL_0010: leave IL_0037
IL_0015: leave IL_0035
IL_001a: ldc.i4 2
IL_001f: call java.lang.Exception IKVM.Runtime.ByteCodeHelper::MapException<java.lang.Exception>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0024: brtrue IL_002b
IL_0029: rethrow
IL_002b: leave IL_0039
IL_0030: leave IL_0035
IL_0035: br.s IL_0035
IL_0037: ldloc.0
IL_0038: ret
IL_0039: ldnull
IL_003a: br IL_003f
IL_003f: pop
IL_0040: ldc.i4 -1
IL_0045: ret
