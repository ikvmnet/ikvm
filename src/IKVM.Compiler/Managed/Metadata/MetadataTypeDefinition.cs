using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Implements <see cref="IManagedTypeDefinition"/> by accessing a <see cref="TypeDefinition"/>.
    /// </summary>
    internal sealed class MetadataTypeDefinition : MetadataEntityDefinition, IManagedTypeDefinition
    {

        readonly MetadataModuleDefinition module;
        readonly TypeDefinition type;
        readonly ReadOnlyListMap<MetadataFieldDefinition, FieldDefinitionHandle> fields;
        readonly ReadOnlyListMap<MetadataMethodDefinition, MethodDefinitionHandle> methods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataTypeDefinition(MetadataContext context, MetadataModuleDefinition module, TypeDefinition type) :
            base(context)
        {
            this.module = module ?? throw new ArgumentNullException(nameof(module));
            this.type = type;

            fields = new ReadOnlyListMap<MetadataFieldDefinition, FieldDefinitionHandle>(new ReadOnlyCollectionList<FieldDefinitionHandle>(type.GetFields()), (f, i) => new MetadataFieldDefinition(this, Context.Reader.GetFieldDefinition(f)));
            methods = new ReadOnlyListMap<MetadataMethodDefinition, MethodDefinitionHandle>(new ReadOnlyCollectionList<MethodDefinitionHandle>(type.GetMethods()), (f, i) => new MetadataMethodDefinition(this, Context.Reader.GetMethodDefinition(f)));
        }

        /// <summary>
        /// Gets the module of the type.
        /// </summary>
        public IManagedModuleDefinition Module => module;

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name => Context.Reader.GetString(type.Name);

        /// <summary>
        /// Gets the set of fields.
        /// </summary>
        public IReadOnlyList<IManagedFieldDefinition> Fields => fields;

        /// <summary>
        /// Gets the set of methods.
        /// </summary>
        public IReadOnlyList<IManagedMethodDefinition> Methods => methods;
    }

}
