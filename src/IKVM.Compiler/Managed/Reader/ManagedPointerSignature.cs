namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Describes a pointer type.
    /// </summary>
    internal readonly partial struct ManagedPointerSignature
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="baseType"></param>
        public ManagedPointerSignature(in ManagedSignatureData baseType)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.Pointer), baseType, null, null, out data);
        }

    }

}