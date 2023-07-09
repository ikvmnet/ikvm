namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a modified type.
    /// </summary>
    public readonly partial struct ManagedModifiedSignature 
    {

        /// <summary>
        /// Creates a new modified type.
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="modifier"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        internal static ManagedModifiedSignature Create(in ManagedSignatureData baseType, in ManagedSignatureData modifier, bool required) => new ManagedModifiedSignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Modified, required), baseType, modifier, null));

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
        public readonly bool Required => data.GetLastCode().Data.Modified_Required!.Value;

    }

}
