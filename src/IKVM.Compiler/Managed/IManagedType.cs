using System.Collections.Generic;
using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a managed type.
    /// </summary>
    internal interface IManagedType
    {

        /// <summary>
        /// Gets the parent type of this type.
        /// </summary>
        IManagedType? DeclaringType { get; }

        /// <summary>
        /// Gets the name of the managed type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the attributes for the type.
        /// </summary>
        TypeAttributes Attributes { get; }

        /// <summary>
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        IReadOnlyList<IManagedCustomAttribute> CustomAttributes { get; }

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        IReadOnlyList<IManagedField> Fields { get; }

        /// <summary>
        /// Gets the set of methods declared on the managed type.
        /// </summary>
        IReadOnlyList<IManagedMethod> Methods { get; }

        /// <summary>
        /// Gets the set of nested types within the managed type.
        /// </summary>
        IReadOnlyList<IManagedType> NestedTypes { get; }

    }

}
