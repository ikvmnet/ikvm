namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a pointer type.
    /// </summary>
    public readonly partial struct ManagedPointerSignature
    {

        /// <summary>
        /// Creates a new pointer signature.
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        internal static ManagedPointerSignature Create(ManagedSignatureData baseType) => new ManagedPointerSignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Pointer), baseType, null, null));

    }

}