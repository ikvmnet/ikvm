using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class AttributeInfoReader : ReaderBase<AttributeInfoRecord>
    {

        Utf8ConstantReader name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        internal AttributeInfoReader(ClassReader declaringClass, AttributeInfoRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the name of the attribute.
        /// </summary>
        public Utf8ConstantReader Name => name ??= DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.NameIndex);

        /// <summary>
        /// Gets the data of the attribute.
        /// </summary>
        public ReadOnlyMemory<byte> Data => Record.Data;

    }

}
