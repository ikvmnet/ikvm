.method System.Boolean $this::instanceOf(System.Object)
.maxstack 3
.initlocals true
// line 70
IL_0000: ldarg.0
IL_0001: isinst System.String
IL_0006: brfalse IL_0026
IL_000b: ldarg.0
IL_000c: castclass System.String
IL_0011: call System.Int32 java.lang.String::instancehelper_length(System.String)
IL_0016: ldc.i4.0
IL_0017: ble IL_0026
IL_001c: ldc.i4 1
IL_0021: br IL_002b
IL_0026: ldc.i4 0
IL_002b: ret
