using System.Reflection;
using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Represents a managed event.
    /// </summary>
    internal struct ManagedEventData
    {

        /// <summary>
        /// Gets the name of the managed event.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets the attributes of the event.
        /// </summary>
        public EventAttributes Attributes;

        /// <summary>
        /// Gets the set of custom attributes applied to the event.
        /// </summary>
        public FixedValueList1<ManagedCustomAttributeData> CustomAttributes;

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        public ManagedSignature EventHandlerType;

    }

}
