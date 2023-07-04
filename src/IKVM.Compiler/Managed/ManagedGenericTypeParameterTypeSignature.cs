namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a type which refers to a generic parameter on an enclosing type.
    /// </summary>
    public sealed class ManagedGenericTypeParameterTypeSignature : ManagedTypeSignature
    {

        readonly ManagedGenericTypeParameterRef parameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameter"></param>
        public ManagedGenericTypeParameterTypeSignature(ManagedGenericTypeParameterRef parameter)
        {
            this.parameter = parameter;
        }

        /// <summary>
        /// Gets the type parameter of the generic type.
        /// </summary>
        public ref readonly ManagedGenericTypeParameterRef Parameter => ref parameter;

    }

}
