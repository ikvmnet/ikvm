using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class AttributeInfoReader
    {

        readonly ClassReader declaringClass;
        readonly AttributeInfoRecord record;

        string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        internal AttributeInfoReader(ClassReader declaringClass, AttributeInfoRecord record)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.record = record;
        }

        /// <summary>
        /// Gets the declaring class of the attribute.
        /// </summary>
        public ClassReader DeclaringClass => declaringClass;

        /// <summary>
        /// Gets the underlying record of the attribute info.
        /// </summary>
        public AttributeInfoRecord Record => record;

        /// <summary>
        /// Gets the name of the attribute.
        /// </summary>
        public string Name => name ??= declaringClass.ResolveConstant<Utf8ConstantReader>(record.NameIndex).Value;

        /// <summary>
        /// Gets the data of the attribute.
        /// </summary>
        public ReadOnlyMemory<byte> Data => record.Data;

    }

}
