namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Describes a type.
    /// </summary>
    internal readonly partial struct ManagedTypeSignature
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public ManagedTypeSignature(ManagedType type)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.Type, type), null, null, null, out data);
        }

        /// <summary>
        /// Gets the type reference refered to by this signature.
        /// </summary>
        public readonly ManagedType Type => data.GetLastCode().Data.Type!;

    }

}
