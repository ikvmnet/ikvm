using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    public sealed class ManagedInterface
    {

        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;
        readonly ManagedTypeRef type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="customAttributes"></param>
        /// <param name="type"></param>
        public ManagedInterface(in ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes, ManagedTypeRef type)
        {
            this.customAttributes = customAttributes;
            this.type = type;
        }

        /// <summary>
        /// Gets the set of custom attributes applied to the interface.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => ref customAttributes;

        /// <summary>
        /// Gets the type of the implemented interface.
        /// </summary>
        public ManagedTypeRef Type => type;

    }

}
