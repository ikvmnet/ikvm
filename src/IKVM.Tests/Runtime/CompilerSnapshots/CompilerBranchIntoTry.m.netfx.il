.method System.Int32 $this::m(System.Int32, System.Int32)
.maxstack 8
.initlocals true
.try IL_0006 to IL_001f catch java.lang.IllegalStateException handler IL_001f to IL_0029
.try IL_0036 to IL_004b catch java.lang.IllegalStateException handler IL_004b to IL_0055
IL_0000: ldarg.0
IL_0001: brfalse IL_0036
IL_0006: ldc.i4 0
IL_000b: call System.Void $this::thrower(System.Int32)
IL_0010: br IL_0015
IL_0015: leave IL_002b
IL_001a: leave IL_0029
IL_001f: leave IL_0030
IL_0024: leave IL_0029
IL_0029: br.s IL_0029
IL_002b: br IL_0036
IL_0030: ldnull
IL_0031: br IL_0068
IL_0036: ldarg.1
IL_0037: call System.Void $this::thrower(System.Int32)
IL_003c: br IL_0041
IL_0041: leave IL_0057
IL_0046: leave IL_0055
IL_004b: leave IL_005c
IL_0050: leave IL_0055
IL_0055: br.s IL_0055
IL_0057: br IL_0062
IL_005c: ldnull
IL_005d: br IL_0068
IL_0062: ldc.i4 1
IL_0067: ret
IL_0068: pop
IL_0069: ldc.i4 -1
IL_006e: ret
