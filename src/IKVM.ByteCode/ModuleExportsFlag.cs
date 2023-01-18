using System;

namespace IKVM.ByteCode
{

    [Flags]
    internal enum ModuleExportsFlag : ushort
    {

        Synthetic = 0x1000,
        Mandated = 0x8000,

    }

}