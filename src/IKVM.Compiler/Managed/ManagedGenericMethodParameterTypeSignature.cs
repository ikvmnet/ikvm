namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a type which refers to a generic parameter on an enclosing method.
    /// </summary>
    public sealed class ManagedGenericMethodParameterTypeSignature : ManagedTypeSignature
    {

        readonly ManagedGenericMethodParameterRef parameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameter"></param>
        public ManagedGenericMethodParameterTypeSignature(ManagedGenericMethodParameterRef parameter)
        {
            this.parameter = parameter;
        }

        /// <summary>
        /// Gets a reference to the generic parameter.
        /// </summary>
        public ref readonly ManagedGenericMethodParameterRef Parameter => ref parameter;

    }

}
