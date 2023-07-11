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
        public static readonly MetadataGenericContext Empty = new MetadataGenericContext();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeParameters"></param>
        /// <param name="methodParameters"></param>
        public MetadataGenericContext(in FixedValueList1<ManagedGenericTypeParameterRef> typeParameters, in FixedValueList1<ManagedGenericMethodParameterRef>? methodParameters)
        {
            TypeParameters = typeParameters;
            MethodParameters = methodParameters;
        }

        /// <summary>
        /// Gets the set of known type parameters.
        /// </summary>
        public readonly FixedValueList1<ManagedGenericTypeParameterRef> TypeParameters;

        /// <summary>
        /// gets the set of known method parameters.
        /// </summary>
        public readonly FixedValueList1<ManagedGenericMethodParameterRef>? MethodParameters;

    }

}
