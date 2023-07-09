using System;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a single rank zero bound array type.
    /// </summary>
    public readonly partial struct ManagedSZArraySignature
    {

        /// <summary>
        /// Creates a new array signature.
        /// </summary>
        /// <param name="elementTypeData"></param>
        /// <returns></returns>
        internal static ManagedSZArraySignature Create(in ManagedSignatureData elementTypeData) => new ManagedSZArraySignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), elementTypeData, null, null));

    }

}
