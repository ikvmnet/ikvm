namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a type.
    /// </summary>
    public readonly partial struct ManagedTypeSignature
    {

        /// <summary>
        /// Creates a new <see cref="ManagedTypeSignature"/>.
        /// </summary>
        /// <param name="typeRef"></param>
        /// <returns></returns>
        internal static ManagedTypeSignature Create(in ManagedTypeRef typeRef) => new ManagedTypeSignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, typeRef), null, null, null));

        /// <summary>
        /// Gets the type reference refered to by this signature.
        /// </summary>
        public readonly ManagedTypeRef TypeRef => data.GetLastCode().Data.Type_Type!.Value;

    }

}
