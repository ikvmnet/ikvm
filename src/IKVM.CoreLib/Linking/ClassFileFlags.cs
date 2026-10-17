using System;

namespace IKVM.CoreLib.Linking
{

    [Flags]
    internal enum ClassFileFlags : ushort
    {

        MASK_DEPRECATED = 0x100,
        MASK_INTERNAL = 0x200,
        CALLERSENSITIVE = 0x400,
        LAMBDAFORM_COMPILED = 0x800,
        LAMBDAFORM_HIDDEN = 0x1000,
        FORCEINLINE = 0x2000,
        HAS_ASSERTIONS = 0x4000,
        MODULE_INITIALIZER = 0x8000,

    }

}
