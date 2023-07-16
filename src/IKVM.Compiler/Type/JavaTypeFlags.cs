using System;

namespace IKVM.Compiler.Type
{

    [Flags]
    internal enum JavaTypeFlags : ushort
    {

        None = 0,
        HasIncompleteInterfaceImplementation = 1,
        InternalAccess = 2,
        HasStaticInitializer = 4,
        VerifyError = 8,
        ClassFormatError = 16,
        HasUnsupportedAbstractMethods = 32,
        Anonymous = 64,
        Linked = 128,

    }

}
