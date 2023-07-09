using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a function pointer type.
    /// </summary>
    public readonly partial struct ManagedFunctionPointerSignature
    {

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        internal static ManagedFunctionPointerSignature Create(ReadOnlyFixedValueList4<ManagedSignatureData> parameterTypes, ManagedSignatureData returnType) => new ManagedFunctionPointerSignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.FunctionPointer), returnType, null, parameterTypes));

    }

}
