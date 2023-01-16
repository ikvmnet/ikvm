using System;
using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class FieldReader
    {

        readonly ClassReader declaringClass;
        readonly FieldRecord record;

        string name;
        string descriptor;
        AttributeReaderCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        internal FieldReader(ClassReader declaringClass, FieldRecord record)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.record = record;
        }

        /// <summary>
        /// Gets the access flags of the field.
        /// </summary>
        public AccessFlag AccessFlags => record.AccessFlags;

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string Name => ClassReader.LazyGet(ref name, () => declaringClass.ResolveConstant<Utf8ConstantReader>(record.NameIndex).Value);

        /// <summary>
        /// Gets the descriptor of the field.
        /// </summary>
        public string Descriptor => ClassReader.LazyGet(ref descriptor, () => declaringClass.ResolveConstant<Utf8ConstantReader>(record.DescriptorIndex).Value);

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        public AttributeReaderCollection Attributes => ClassReader.LazyGet(ref attributes, () => new AttributeReaderCollection(declaringClass, record.Attributes));

    }

}