using System;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a single rank zero bound array type.
    /// </summary>
    internal readonly partial struct ManagedSZArraySignature
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementTypeData"></param>
        public ManagedSZArraySignature(in ManagedSignatureData elementTypeData)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), elementTypeData, null, null, out data);
        }

    }

}
