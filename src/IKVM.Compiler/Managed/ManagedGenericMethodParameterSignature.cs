namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a generic method parameter type.
    /// </summary>
    internal readonly partial struct ManagedGenericMethodParameterSignature
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameter"></param>
        internal ManagedGenericMethodParameterSignature(in ManagedGenericMethodParameterRef parameter)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.GenericMethodParameter, parameter), null, null, null, out data);
        }

    }

}
