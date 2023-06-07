using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Implements <see cref="IManagedTypeInfo"/> by accessing a <see cref="TypeDefinition"/>.
    /// </summary>
    internal sealed class MetadataTypeInfo : MetadataEntityInfo, IManagedTypeInfo
    {

        readonly MetadataModuleInfo module;
        readonly TypeDefinition type;
        readonly ReadOnlyListMap<MetadataFieldInfo, FieldDefinitionHandle> fields;
        readonly ReadOnlyListMap<MetadataMethodInfo, MethodDefinitionHandle> methods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataTypeInfo(MetadataContext context, MetadataModuleInfo module, TypeDefinition type) :
            base(context)
        {
            this.module = module ?? throw new ArgumentNullException(nameof(module));
            this.type = type;

            fields = new ReadOnlyListMap<MetadataFieldInfo, FieldDefinitionHandle>(new ReadOnlyCollectionList<FieldDefinitionHandle>(type.GetFields()), (f, i) => new MetadataFieldInfo(this, Context.Reader.GetFieldDefinition(f)));
            methods = new ReadOnlyListMap<MetadataMethodInfo, MethodDefinitionHandle>(new ReadOnlyCollectionList<MethodDefinitionHandle>(type.GetMethods()), (f, i) => new MetadataMethodInfo(this, Context.Reader.GetMethodDefinition(f)));
        }

        /// <summary>
        /// Gets the module of the type.
        /// </summary>
        public IManagedModuleInfo Module => module;

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name => Context.Reader.GetString(type.Name);

        /// <summary>
        /// Gets the set of fields.
        /// </summary>
        public IReadOnlyList<IManagedFieldInfo> Fields => fields;

        /// <summary>
        /// Gets the set of methods.
        /// </summary>
        public IReadOnlyList<IManagedMethodInfo> Methods => methods;
    }

}
