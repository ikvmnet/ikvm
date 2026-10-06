.method System.Int32 $this::manyArgsWithTry(System.Int32, System.Int32, System.Int32, System.Int32, System.Int32)
.maxstack 7
.initlocals true
.try IL_0006 to IL_0028 catch System.Exception handler IL_0028 to IL_0043
IL_0000: ldarga.s 3
IL_0002: pop
IL_0003: ldarga.s 4
IL_0005: pop
// line 98
IL_0006: ldarg.s 4
IL_0008: ldc.i4 1
IL_000d: add
IL_000e: starg.s 4
IL_0010: ldarg.s 4
IL_0012: call System.Int32 $this::g(System.Int32)
IL_0017: starg.s 3
IL_0019: br IL_001e
IL_001e: leave IL_0045
IL_0023: leave IL_0043
IL_0028: ldc.i4 2
IL_002d: call java.lang.RuntimeException IKVM.Runtime.ByteCodeHelper::MapException<java.lang.RuntimeException>(System.Exception, IKVM.Runtime.ByteCodeHelper/MapFlags)
IL_0032: brtrue IL_0039
IL_0037: rethrow
IL_0039: leave IL_004a
IL_003e: leave IL_0043
IL_0043: br.s IL_0043
IL_0045: br IL_0050
IL_004a: ldnull
IL_004b: br IL_0055
IL_0050: br IL_005d
IL_0055: pop
IL_0056: ldc.i4 -1
IL_005b: starg.s 0
IL_005d: ldarg.0
IL_005e: ldarg.1
IL_005f: add
IL_0060: ldarg.2
IL_0061: add
IL_0062: ldarg.3
IL_0063: add
IL_0064: ldarg.s 4
IL_0066: add
IL_0067: ret
