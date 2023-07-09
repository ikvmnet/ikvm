namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a by-ref type.
    /// </summary>
    public readonly partial struct ManagedByRefSignature
    {

        /// <summary>
        /// Creates a new by-ref signature.
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        internal static ManagedByRefSignature Create(ManagedSignatureData baseType) => new ManagedByRefSignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.ByRef), baseType, null, null));

    }

}
