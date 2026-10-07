.method System.Int32 $this::m(System.Int32, System.Int32)
.maxstack 2
.initlocals true
.try IL_0006 to IL_001b catch java.lang.IllegalStateException handler IL_001b to IL_0025
.try IL_0032 to IL_0047 catch java.lang.IllegalStateException handler IL_0047 to IL_0051
IL_0000: ldarg.0
IL_0001: brfalse IL_0032
IL_0006: ldc.i4.0
IL_0007: call System.Void $this::thrower(System.Int32)
IL_000c: br IL_0011
IL_0011: leave IL_0027
IL_0016: leave IL_0025
IL_001b: leave IL_002c
IL_0020: leave IL_0025
IL_0025: br.s IL_0025
IL_0027: br IL_0032
IL_002c: ldnull
IL_002d: br IL_0060
IL_0032: ldarg.1
IL_0033: call System.Void $this::thrower(System.Int32)
IL_0038: br IL_003d
IL_003d: leave IL_0053
IL_0042: leave IL_0051
IL_0047: leave IL_0058
IL_004c: leave IL_0051
IL_0051: br.s IL_0051
IL_0053: br IL_005e
IL_0058: ldnull
IL_0059: br IL_0060
IL_005e: ldc.i4.1
IL_005f: ret
IL_0060: pop
IL_0061: ldc.i4.m1
IL_0062: ret
