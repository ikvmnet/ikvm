using System;

namespace IKVM.ByteCode
{

    [Flags]
    internal enum ModuleRequiresFlag : ushort
    {

        Transitive = 0x0020,
        StaticPhase = 0x0040,
        Synthetic = 0x1000,
        Mandated = 0x8000,

    }

}