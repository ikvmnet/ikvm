using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a function pointer type.
    /// </summary>
    internal readonly partial struct ManagedFunctionPointerSignature
    {

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        internal ManagedFunctionPointerSignature(in FixedValueList4<ManagedSignatureData> parameterTypes, in ManagedSignatureData returnType)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.FunctionPointer), returnType, null, parameterTypes, out data);
        }

    }

}
