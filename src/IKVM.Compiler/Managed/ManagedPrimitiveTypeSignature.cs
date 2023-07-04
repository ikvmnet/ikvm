namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a primitive type.
    /// </summary>
    public sealed class ManagedPrimitiveTypeSignature : ManagedTypeSignature
    {

        readonly ManagedPrimitiveType primitiveType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="primitiveType"></param>
        public ManagedPrimitiveTypeSignature(ManagedPrimitiveType primitiveType)
        {
            this.primitiveType = primitiveType;
        }

        /// <summary>
        /// Gets the type refered to by the pointer.
        /// </summary>
        public ManagedPrimitiveType PrimitiveType => primitiveType;

    }

}
