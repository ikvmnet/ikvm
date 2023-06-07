using System;
using IKVM.ByteCode.Syntax;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Provides utilities for working with managed types.
    /// </summary>
    static class ManagedTypeUtil
    {

        /// <summary>
        /// Returns <c>true</c> if the specified managed type is a compiled Java type.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsJavaType(IManagedTypeInfo info)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Java type name for a managed type.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static JavaClassName GetName(IManagedTypeInfo info)
        {
            throw new NotImplementedException();
        }

    }

}
