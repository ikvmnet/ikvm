using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a primitive type.
    /// </summary>
    public sealed class ManagedFunctionPointerSignature : ManagedTypeSignature
    {

        readonly ReadOnlyFixedValueList<ManagedTypeSignature> parameterTypes;
        readonly ManagedTypeSignature returnType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        public ManagedFunctionPointerSignature(in ReadOnlyFixedValueList<ManagedTypeSignature> parameterTypes, ManagedTypeSignature returnType)
        {
            this.parameterTypes = parameterTypes;
            this.returnType = returnType;
        }

        /// <summary>
        /// Gets the types of the method parameters.
        /// </summary>
        public ReadOnlyFixedValueList<ManagedTypeSignature> ParameterTypes => parameterTypes;

        /// <summary>
        /// Gets the type of the return value.
        /// </summary>
        public ManagedTypeSignature ReturnType => returnType;

    }

}
