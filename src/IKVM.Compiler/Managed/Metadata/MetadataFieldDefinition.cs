using System;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    internal sealed class MetadataFieldDefinition : MetadataMemberDefinition, IManagedFieldDefinition
    {

        readonly MetadataTypeDefinition declaringType;
        readonly FieldDefinition field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="field"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataFieldDefinition(MetadataTypeDefinition declaringType, FieldDefinition field) :
            base(declaringType)
        {
            this.declaringType = this.declaringType ?? throw new ArgumentNullException(nameof(MetadataFieldDefinition.declaringType));
            this.field = field;
        }

        public override string Name => Context.Reader.GetString(field.Name);

    }

}
