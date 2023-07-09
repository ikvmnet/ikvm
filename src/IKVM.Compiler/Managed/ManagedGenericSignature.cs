using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a generic type.
    /// </summary>
    public readonly partial struct ManagedGenericSignature 
    {

        /// <summary>
        /// Creates a new generic signature.
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        internal static ManagedGenericSignature Create(in ManagedSignatureData baseType, ReadOnlyFixedValueList4<ManagedSignatureData> genericParameters) => new ManagedGenericSignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Generic), baseType, null, genericParameters));

    }

}
