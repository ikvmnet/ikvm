.method System.Int32 $this::stringSwitch(System.String)
.maxstack 3
.initlocals true
.local [0] System.String
.local [1] System.Int32
// line 37
IL_0000: ldarg.0
IL_0001: stloc.0
IL_0002: ldc.i4.m1
IL_0003: stloc.1
IL_0004: ldloc.0
IL_0005: call System.Int32 java.lang.String::instancehelper_hashCode(System.String)
IL_000a: dup
IL_000b: ldc.i4.s 97
IL_000d: bne.un IL_0018
IL_0012: pop
IL_0013: br IL_002c
IL_0018: dup
IL_0019: ldc.i4.s 98
IL_001b: bne.un IL_0026
IL_0020: pop
IL_0021: br IL_0043
IL_0026: pop
IL_0027: br IL_0055
IL_002c: ldloc.0
IL_002d: ldstr "a"
IL_0032: call System.Boolean java.lang.String::instancehelper_equals(System.String, System.Object)
IL_0037: brfalse IL_0055
IL_003c: ldc.i4.0
IL_003d: stloc.1
IL_003e: br IL_0055
IL_0043: ldloc.0
IL_0044: ldstr "b"
IL_0049: call System.Boolean java.lang.String::instancehelper_equals(System.String, System.Object)
IL_004e: brfalse IL_0055
IL_0053: ldc.i4.1
IL_0054: stloc.1
IL_0055: ldloc.1
IL_0056: dup
IL_0057: ldc.i4.0
IL_0058: bne.un IL_0063
IL_005d: pop
IL_005e: br IL_0076
IL_0063: dup
IL_0064: ldc.i4.1
IL_0065: bne.un IL_0070
IL_006a: pop
IL_006b: br IL_0078
IL_0070: pop
IL_0071: br IL_007a
IL_0076: ldc.i4.1
IL_0077: ret
IL_0078: ldc.i4.2
IL_0079: ret
IL_007a: ldc.i4.0
IL_007b: ret
