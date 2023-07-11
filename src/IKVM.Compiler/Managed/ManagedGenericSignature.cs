using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a generic type.
    /// </summary>
    internal readonly partial struct ManagedGenericSignature
    {

        /// <summary>
        /// Creates a new generic signature.
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="genericParameters"></param>
        internal ManagedGenericSignature(in ManagedSignatureData baseType, in FixedValueList4<ManagedSignatureData> genericParameters)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.Generic), baseType, null, genericParameters, out data);
        }

    }

}
