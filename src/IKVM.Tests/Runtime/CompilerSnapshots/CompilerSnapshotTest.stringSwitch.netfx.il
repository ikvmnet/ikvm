.method System.Int32 $this::stringSwitch(System.String)
.maxstack 15
.initlocals true
.local [0] System.String
.local [1] System.Int32
// line 37
IL_0000: ldarg.0
IL_0001: stloc.0
IL_0002: ldc.i4 -1
IL_0007: stloc.1
IL_0008: ldloc.0
IL_0009: call System.Int32 java.lang.String::instancehelper_hashCode(System.String)
IL_000e: dup
IL_000f: ldc.i4 97
IL_0014: bne.un IL_001f
IL_0019: pop
IL_001a: br IL_0036
IL_001f: dup
IL_0020: ldc.i4 98
IL_0025: bne.un IL_0030
IL_002a: pop
IL_002b: br IL_0051
IL_0030: pop
IL_0031: br IL_0067
IL_0036: ldloc.0
IL_0037: ldstr "a"
IL_003c: call System.Boolean java.lang.String::instancehelper_equals(System.String, System.Object)
IL_0041: brfalse IL_0067
IL_0046: ldc.i4 0
IL_004b: stloc.1
IL_004c: br IL_0067
IL_0051: ldloc.0
IL_0052: ldstr "b"
IL_0057: call System.Boolean java.lang.String::instancehelper_equals(System.String, System.Object)
IL_005c: brfalse IL_0067
IL_0061: ldc.i4 1
IL_0066: stloc.1
IL_0067: ldloc.1
IL_0068: dup
IL_0069: ldc.i4 0
IL_006e: bne.un IL_0079
IL_0073: pop
IL_0074: br IL_0090
IL_0079: dup
IL_007a: ldc.i4 1
IL_007f: bne.un IL_008a
IL_0084: pop
IL_0085: br IL_0096
IL_008a: pop
IL_008b: br IL_009c
IL_0090: ldc.i4 1
IL_0095: ret
IL_0096: ldc.i4 2
IL_009b: ret
IL_009c: ldc.i4 0
IL_00a1: ret
