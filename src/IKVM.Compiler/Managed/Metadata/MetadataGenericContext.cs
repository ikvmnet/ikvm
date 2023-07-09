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

        readonly ReadOnlyFixedValueList1<ManagedGenericTypeParameterRef> typeParameters;
        readonly ReadOnlyFixedValueList1<ManagedGenericMethodParameterRef>? methodParameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeParameters"></param>
        /// <param name="methodParameters"></param>
        public MetadataGenericContext(in ReadOnlyFixedValueList1<ManagedGenericTypeParameterRef> typeParameters, in ReadOnlyFixedValueList1<ManagedGenericMethodParameterRef>? methodParameters)
        {
            this.typeParameters = typeParameters;
            this.methodParameters = methodParameters;
        }

        /// <summary>
        /// Gets the set of known type parameters.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedGenericTypeParameterRef> TypeParameters => typeParameters;

        /// <summary>
        /// gets the set of known method parameters.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedGenericMethodParameterRef>? MethodParameters => methodParameters;

    }

}
