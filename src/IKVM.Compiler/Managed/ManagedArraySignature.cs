using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an array type.
    /// </summary>
    public readonly partial struct ManagedArraySignature
    {

        /// <summary>
        /// Creates a new array signature.
        /// </summary>
        /// <param name="elementTypeData"></param>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        internal static ManagedArraySignature Create(in ManagedSignatureData elementTypeData, int rank, in ReadOnlyFixedValueList2<int> sizes, in ReadOnlyFixedValueList2<int> lowerBounds) => new ManagedArraySignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Array, new ManagedArrayShape(rank, sizes, lowerBounds)), elementTypeData, null, null));

    }

}
