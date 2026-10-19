using System;

namespace IKVM.CoreLib.Linking
{

    [Flags]
    internal enum OpCodeFlags : byte
    {

        None = 0,
        FixedArg = 1,
        CannotThrow = 2

    }

}
