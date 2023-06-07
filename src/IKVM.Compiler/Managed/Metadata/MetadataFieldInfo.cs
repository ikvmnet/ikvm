using System;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    internal sealed class MetadataFieldInfo : MetadataMemberInfo, IManagedFieldInfo
    {

        readonly MetadataTypeInfo declaringType;
        readonly FieldDefinition field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="field"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataFieldInfo(MetadataTypeInfo declaringType, FieldDefinition field) :
            base(declaringType)
        {
            this.declaringType = this.declaringType ?? throw new ArgumentNullException(nameof(MetadataFieldInfo.declaringType));
            this.field = field;
        }

        public override string Name => Context.Reader.GetString(field.Name);

    }

}
