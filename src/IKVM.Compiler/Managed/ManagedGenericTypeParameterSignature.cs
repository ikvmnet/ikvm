namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a generic type parameter type.
    /// </summary>
    internal partial struct ManagedGenericTypeParameterSignature
    {

        /// <summary>
        /// Creates a new generic method parameter signature.
        /// </summary>
        /// <param name="parameter"></param>
        internal ManagedGenericTypeParameterSignature(in ManagedGenericTypeParameterRef parameter)
        {
            ManagedSignatureData.Write(new ManagedSignatureCodeData(ManagedSignatureKind.GenericTypeParameter, parameter), null, null, null, out data);
        }

    }

}
