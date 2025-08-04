using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides access to manifest resources, which are XML files that describe application dependencies.
    /// </summary>
    public readonly struct ManifestResourceData(string Name, ImmutableArray<byte> Data, ResourceAttributes Attributes)
    {

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        public readonly string Name = Name;

        /// <summary>
        /// Gets the data of the resource.
        /// </summary>
        public readonly ImmutableArray<byte> Data = Data;

        /// <summary>
        /// Gets the resource attributes.
        /// </summary>
        public readonly ResourceAttributes Attributes = Attributes;

    }

}
