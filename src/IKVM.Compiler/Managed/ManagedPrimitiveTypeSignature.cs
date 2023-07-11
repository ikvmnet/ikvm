namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a primtive type.
    /// </summary>
    internal readonly partial struct ManagedPrimitiveTypeSignature
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ManagedPrimitiveTypeSignature(ManagedPrimitiveTypeCode code)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, code), null, null, null, out data);
        }

        /// <summary>
        /// Gets the underlying primitive type code.
        /// </summary>
        public readonly ManagedPrimitiveTypeCode TypeCode => (ManagedPrimitiveTypeCode)data.GetLastCode().Data.Primitive_TypeCode!;

    }

}
