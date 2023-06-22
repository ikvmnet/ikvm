using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Represents a field from metadata.
    /// </summary>
    internal sealed class MetadataField : MetadataMember, IManagedField
    {

        readonly FieldDefinitionHandle handle;
        readonly ReadOnlyListMap<MetadataCustomAttribute, CustomAttributeHandle> customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="handle"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataField(MetadataType declaringType, FieldDefinitionHandle handle) :
            base(declaringType)
        {
            this.handle = handle;

            customAttributes = new ReadOnlyListMap<MetadataCustomAttribute, CustomAttributeHandle>(new ReadOnlyCollectionList<CustomAttributeHandle>(Data.GetCustomAttributes()), (a, i) => new MetadataCustomAttribute(this, a));
        }

        FieldDefinition Data => Context.Reader.GetFieldDefinition(handle);

        /// <inheritdoc />
        public override string Name => Context.Reader.GetString(Data.Name);

        /// <inheritdoc />
        public ManagedTypeSignature FieldType => Data.DecodeSignature(new MetadataSignatureTypeProvider(Context), declaringType);

        /// <inheritdoc />
        public FieldAttributes Attributes => Data.Attributes;

        /// <inheritdoc />
        public override IReadOnlyList<IManagedCustomAttribute> CustomAttributes => customAttributes;

    }

}
