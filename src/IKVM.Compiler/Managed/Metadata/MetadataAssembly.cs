using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Describes an assembly.
    /// </summary>
    internal sealed class MetadataAssembly : MetadataBase, IManagedAssembly
    {

        readonly AssemblyDefinition assembly;
        readonly ReadOnlyListMap<TypeDefinitionHandle, MetadataType> types;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataAssembly(MetadataContext context, AssemblyDefinition assembly) :
            base(context)
        {
            this.assembly = assembly;

            types = Context.Reader.TypeDefinitions.Map((t, i) => Context.ResolveType(t));
        }

        /// <inheritdoc />
        public string Name => Context.Reader.GetString(assembly.Name);

        /// <inheritdoc />
        public IReadOnlyList<byte> PublicKey => Context.Reader.GetBlobBytes(assembly.PublicKey);

        /// <inheritdoc />
        public Version Version => assembly.Version;

        /// <inheritdoc />
        public string Culture => Context.Reader.GetString(assembly.Culture);

        /// <inheritdoc />
        public IReadOnlyList<IManagedType> Types => types;

    }

}
