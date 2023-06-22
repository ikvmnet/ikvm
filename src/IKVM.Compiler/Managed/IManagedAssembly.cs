using System;
using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a manged assembly.
    /// </summary>
    internal interface IManagedAssembly
    {

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the public key of the assembly.
        /// </summary>
        IReadOnlyList<byte>? PublicKey { get; }

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gets the culture of the assembly.
        /// </summary>
        string? Culture { get; }

        /// <summary>
        /// Gets the types of the assembly.
        /// </summary>
        IReadOnlyList<IManagedType> Types { get; }

    }

}
