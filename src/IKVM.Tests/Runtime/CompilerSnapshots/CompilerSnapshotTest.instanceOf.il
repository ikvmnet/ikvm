.method System.Boolean $this::instanceOf(System.Object)
.maxstack 2
.initlocals true
// line 70
IL_0000: ldarg.0
IL_0001: isinst System.String
IL_0006: brfalse IL_0022
IL_000b: ldarg.0
IL_000c: castclass System.String
IL_0011: call System.Int32 java.lang.String::instancehelper_length(System.String)
IL_0016: ldc.i4.0
IL_0017: ble IL_0022
IL_001c: ldc.i4.1
IL_001d: br IL_0023
IL_0022: ldc.i4.0
IL_0023: ret
