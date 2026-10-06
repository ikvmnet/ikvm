.method System.Void $this::interfaceCall(java.util.List)
.maxstack 2
.initlocals true
// line 92
IL_0000: ldarg.0
IL_0001: ldstr "x"
IL_0006: callvirt instance System.Boolean java.util.List::add(System.Object)
IL_000b: pop
IL_000c: ldarg.0
IL_000d: callvirt instance System.Int32 java.util.List::size()
IL_0012: pop
IL_0013: leave.s IL_0015
IL_0015: ret
