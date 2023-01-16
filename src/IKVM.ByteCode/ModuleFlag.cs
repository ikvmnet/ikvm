using System;

namespace IKVM.ByteCode
{

    [Flags]
    public enum ModuleFlag : ushort
    {

        Open = 0x0020,
        Synthetic = 0x1000,
        Mandated = 0x8000,

    }

}