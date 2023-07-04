using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed member.
    /// </summary>
    public abstract class ManagedMember
    {

        readonly string name;
        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="customAttributes"></param>
        public ManagedMember(string name, in ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes)
        {
            this.name = name;
            this.customAttributes = customAttributes;
        }

        /// <summary>
        /// Gets the name of the managed member.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => ref customAttributes;

    }

}
