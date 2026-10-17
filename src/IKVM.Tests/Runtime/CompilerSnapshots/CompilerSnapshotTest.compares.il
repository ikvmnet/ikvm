.method System.Int32 $this::compares(System.Int64, System.Int64, System.Double, System.Double, System.Single, System.Single)
.maxstack 2
.initlocals true
.local [0] System.Int32
IL_0000: ldc.i4.0
IL_0001: stloc.0
IL_0002: ldarg.0
IL_0003: ldarg.1
IL_0004: bge IL_000d
IL_0009: ldloc.0
IL_000a: ldc.i4.1
IL_000b: add
IL_000c: stloc.0
IL_000d: ldarg.2
IL_000e: conv.r8
IL_000f: ldarg.3
IL_0010: conv.r8
IL_0011: ble.un IL_001a
IL_0016: ldloc.0
IL_0017: ldc.i4.2
IL_0018: add
IL_0019: stloc.0
IL_001a: ldarg.2
IL_001b: conv.r8
IL_001c: ldarg.3
IL_001d: conv.r8
IL_001e: bgt.un IL_0027
IL_0023: ldloc.0
IL_0024: ldc.i4.4
IL_0025: add
IL_0026: stloc.0
IL_0027: ldarg.s 4
IL_0029: conv.r4
IL_002a: ldarg.s 5
IL_002c: conv.r4
IL_002d: bge.un IL_0036
IL_0032: ldloc.0
IL_0033: ldc.i4.8
IL_0034: add
IL_0035: stloc.0
IL_0036: ldarg.s 4
IL_0038: conv.r4
IL_0039: ldarg.s 5
IL_003b: conv.r4
IL_003c: blt.un IL_0046
IL_0041: ldloc.0
IL_0042: ldc.i4.s 16
IL_0044: add
IL_0045: stloc.0
IL_0046: ldloc.0
IL_0047: ret
