namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a primtive type.
    /// </summary>
    public readonly partial struct ManagedPrimitiveTypeSignature
    {

        /// <summary>
        /// Creates a new primitive type signature.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static ManagedPrimitiveTypeSignature Create(ManagedPrimitiveTypeCode code) => new ManagedPrimitiveTypeSignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, code), null, null, null));

        /// <summary>
        /// Gets the underlying primitive type code.
        /// </summary>
        public readonly ManagedPrimitiveTypeCode TypeCode => (ManagedPrimitiveTypeCode)data.GetLastCode().Data.Primitive_TypeCode!;

    }

}
