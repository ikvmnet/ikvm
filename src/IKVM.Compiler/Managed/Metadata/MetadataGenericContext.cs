using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Describes the generic parameters available within a given context.
    /// </summary>
    readonly struct MetadataGenericContext
    {

        readonly ManagedTypeRef type;
        readonly ReadOnlyFixedValueList<ManagedGenericTypeParameterRef> typeParameters;
        readonly ReadOnlyFixedValueList<ManagedGenericMethodParameterRef>? methodParameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeParameters"></param>
        /// <param name="methodParameters"></param>
        public MetadataGenericContext(ManagedTypeRef type, in ReadOnlyFixedValueList<ManagedGenericTypeParameterRef> typeParameters, in ReadOnlyFixedValueList<ManagedGenericMethodParameterRef>? methodParameters)
        {
            this.type = type;
            this.typeParameters = typeParameters;
            this.methodParameters = methodParameters;
        }

        /// <summary>
        /// Gets the set of known type parameters.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedGenericTypeParameterRef> TypeParameters => typeParameters;

        /// <summary>
        /// gets the set of known method parameters.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedGenericMethodParameterRef>? MethodParameters => methodParameters;

    }

}
