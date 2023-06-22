using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Implements <see cref="IManagedType"/> by accessing a <see cref="TypeDefinition"/>.
    /// </summary>
    internal sealed class MetadataType : MetadataBase, IManagedType, IMetadataGenericTypeContext
    {

        readonly TypeDefinitionHandle handle;
        readonly ReadOnlyListMap<CustomAttributeHandle, MetadataCustomAttribute> customAttributes;
        readonly ReadOnlyListMap<FieldDefinitionHandle, MetadataField> fields;
        readonly ReadOnlyListMap<MethodDefinitionHandle, MetadataMethod> methods;
        readonly ReadOnlyListMap<TypeDefinitionHandle, MetadataType> nestedTypes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="handle"></param>
        internal MetadataType(MetadataContext context, TypeDefinitionHandle handle) :
            base(context)
        {
            this.handle = handle;

            customAttributes = Data.GetCustomAttributes().Map((a, i) => new MetadataCustomAttribute(this, a));
            fields = Data.GetFields().Map((f, i) => new MetadataField(this, f));
            methods = Data.GetMethods().Map((f, i) => new MetadataMethod(this, f));
            nestedTypes = Data.GetNestedTypes().Map((t, i) => Context.ResolveType(t));
        }

        TypeDefinition Data => Context.Reader.GetTypeDefinition(handle);

        /// <inheritdoc />
        public IManagedType? DeclaringType => Data.GetDeclaringType() is { IsNil: false } t ? Context.ResolveType(t) : null;

        /// <inheritdoc />
        public string Name => Context.Reader.GetString(Data.Name);

        /// <inheritdoc />
        public TypeAttributes Attributes => Data.Attributes;

        /// <inheritdoc />
        public IReadOnlyList<IManagedCustomAttribute> CustomAttributes => customAttributes;

        /// <inheritdoc />
        public IReadOnlyList<IManagedField> Fields => fields;

        /// <inheritdoc />
        public IReadOnlyList<IManagedMethod> Methods => methods;

        /// <inheritdoc />
        public IReadOnlyList<IManagedType> NestedTypes => nestedTypes;

    }

}
