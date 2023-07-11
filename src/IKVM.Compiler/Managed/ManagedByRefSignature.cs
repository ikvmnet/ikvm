namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a by-ref type.
    /// </summary>
    internal readonly partial struct ManagedByRefSignature
    {

        /// <summary>
        /// Creates a new by-ref signature
        /// </summary>
        /// <param name="baseType"></param>
        internal ManagedByRefSignature(in ManagedSignatureData baseType)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.ByRef), baseType, null, null, out data);
        }

    }

}
