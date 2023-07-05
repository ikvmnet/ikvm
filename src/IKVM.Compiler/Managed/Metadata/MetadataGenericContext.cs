using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Describes the generic parameters available within a given context.
    /// </summary>
    readonly struct MetadataGenericContext
    {

        /// <summary>
        /// Gets an empty generic context.
        /// </summary>
        public static readonly MetadataGenericContext Empty = new MetadataGenericContext ();

        readonly ReadOnlyFixedValueList<ManagedGenericTypeParameterRef> typeParameters;
        readonly ReadOnlyFixedValueList<ManagedGenericMethodParameterRef>? methodParameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeParameters"></param>
        /// <param name="methodParameters"></param>
        public MetadataGenericContext(in ReadOnlyFixedValueList<ManagedGenericTypeParameterRef> typeParameters, in ReadOnlyFixedValueList<ManagedGenericMethodParameterRef>? methodParameters)
        {
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
