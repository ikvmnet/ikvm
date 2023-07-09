namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a generic method parameter type.
    /// </summary>
    public readonly partial struct ManagedGenericMethodParameterSignature
    {

        /// <summary>
        /// Creates a new generic method parameter signature.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static ManagedGenericMethodParameterSignature Create(in ManagedGenericMethodParameterRef parameter) => new ManagedGenericMethodParameterSignature(new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.GenericMethodParameter, parameter), null, null, null));

    }

}
