using System;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a generic type parameter type.
    /// </summary>
    public readonly partial struct ManagedGenericTypeParameterSignature
    {

        /// <summary>
        /// Creates a new generic method parameter signature.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static ManagedGenericTypeParameterSignature Create(in ManagedGenericTypeParameterRef parameter) => new ManagedGenericTypeParameterSignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.GenericTypeParameter, parameter), null, null, null));

    }

}
