namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a modified type.
    /// </summary>
    internal readonly partial struct ManagedModifiedSignature 
    {

        /// <summary>
        /// Creates a new modified type.
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="modifier"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        internal ManagedModifiedSignature(in ManagedSignatureData baseType, in ManagedSignatureData modifier, bool required)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.Modified, required), baseType, modifier, null, out data);
        }

        /// <summary>
        /// Gets the base type that was modified
        /// </summary>
        public readonly ManagedSignature BaseType => default;

        /// <summary>
        /// Gets the modifier type.
        /// </summary>
        public readonly ManagedSignature Modifier => default;

        /// <summary>
        /// Gets whether the modifier is required.
        /// </summary>
        public readonly bool Required => data.GetLastCode().Data.Data.Modified_Required;

    }

}
