using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed event.
    /// </summary>
    public readonly struct ManagedEvent
    {

        readonly string name;
        readonly EventAttributes attributes;
        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;
        readonly ManagedTypeSignature eventType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="customAttributes"></param>
        /// <param name="attributes"></param>
        /// <param name="eventType"></param>
        public ManagedEvent(string name, EventAttributes attributes, in ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes, ManagedTypeSignature eventType)
        {
            this.name = name;
            this.customAttributes = customAttributes;
            this.attributes = attributes;
            this.eventType = eventType;
        }

        /// <summary>
        /// Gets the name of the managed event.
        /// </summary>
        public readonly string Name => name;

        /// <summary>
        /// Gets the attributes of the event.
        /// </summary>
        public readonly EventAttributes Attributes => attributes;

        /// <summary>
        /// Gets the set of custom attributes applied to the event.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => customAttributes;

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        public readonly ManagedTypeSignature EventType => eventType;

        /// <inhericdoc />
        public override readonly string ToString() => name;

    }

}
