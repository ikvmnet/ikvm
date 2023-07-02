using System;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a manged assembly.
    /// </summary>
    internal class ManagedAssembly
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="publicKey"></param>
        /// <param name="version"></param>
        /// <param name="culture"></param>
        /// <param name="getTypes"></param>
        public ManagedAssembly(IManagedAssemblyContext context, string name, ReadOnlyValueList<ManagedCustomAttribute> customAttributes, IReadOnlyList<byte>? publicKey, Version version, string? culture)
        {
            Context = context;
            Name = name;
            CustomAttributes = customAttributes;
            PublicKey = publicKey;
            Version = version;
            Culture = culture;
        }

        /// <summary>
        /// Gets the context responsible for loading this assembly.
        /// </summary>
        public IManagedAssemblyContext Context { get; }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the set of custom attributes applied to the assembly.
        /// </summary>
        public IReadOnlyList<ManagedCustomAttribute> CustomAttributes { get; }

        /// <summary>
        /// Gets the public key of the assembly.
        /// </summary>
        public IReadOnlyList<byte>? PublicKey { get; }

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// Gets the culture of the assembly.
        /// </summary>
        public string? Culture { get; }

        /// <summary>
        /// Attempts to resolve a type with the specified name from the managed assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public ManagedType? ResolveType(string typeName) => Context.ResolveType(typeName);

        /// <summary>
        /// Provides resolution of types within the assembly.
        /// </summary>
        public IEnumerable<ManagedType> ResolveTypes => Context.ResolveTypes();

    }

}
