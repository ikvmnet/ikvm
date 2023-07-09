using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed event.
    /// </summary>
    public readonly struct ManagedEvent
    {

        /// <summary>
        /// Gets the name of the managed event.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Gets the attributes of the event.
        /// </summary>
        public readonly EventAttributes Attributes;

        /// <summary>
        /// Gets the set of custom attributes applied to the event.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        public readonly ManagedSignature EventType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="customAttributes"></param>
        /// <param name="attributes"></param>
        /// <param name="eventType"></param>
        public ManagedEvent(string name, EventAttributes attributes, in ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes, ManagedSignature eventType)
        {
            Name = name;
            CustomAttributes = customAttributes;
            Attributes = attributes;
            EventType = eventType;
        }

        /// <inhericdoc />
        public override readonly string ToString() => Name;

    }

}
