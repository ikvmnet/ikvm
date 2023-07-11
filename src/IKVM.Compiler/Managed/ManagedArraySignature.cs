using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an array type.
    /// </summary>
    internal readonly partial struct ManagedArraySignature
    {

        /// <summary>
        /// Creates a new array signature.
        /// </summary>
        /// <param name="elementTypeData"></param>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        internal ManagedArraySignature(in ManagedSignatureData elementTypeData, int rank, in FixedValueList2<int> sizes, in FixedValueList2<int> lowerBounds)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.Array, new ManagedArrayShape(rank, sizes, lowerBounds)), elementTypeData, null, null, out data);
        }

    }

}
