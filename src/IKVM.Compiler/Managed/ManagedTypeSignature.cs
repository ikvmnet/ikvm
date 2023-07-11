namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a type.
    /// </summary>
    internal readonly partial struct ManagedTypeSignature
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeRef"></param>
        public ManagedTypeSignature(in ManagedTypeRef typeRef)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.Type, typeRef), null, null, null, out data);
        }

        /// <summary>
        /// Gets the type reference refered to by this signature.
        /// </summary>
        public readonly ManagedTypeRef TypeRef => data.GetLastCode().Data.Type_Type!.Value;

    }

}
