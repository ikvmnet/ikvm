using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal class FieldReader : FieldOrMethodReader<FieldRecord>
    {

        string name;
        string descriptor;
        AttributeReaderCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        internal FieldReader(ClassReader declaringClass, FieldRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the access flags of the field.
        /// </summary>
        public AccessFlag AccessFlags => Record.AccessFlags;

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string Name => LazyGet(ref name, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex).Value);

        /// <summary>
        /// Gets the descriptor of the field.
        /// </summary>
        public string Descriptor => LazyGet(ref descriptor, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.DescriptorIndex).Value);

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        public AttributeReaderCollection Attributes => LazyGet(ref attributes, () => new AttributeReaderCollection(DeclaringClass, Record.Attributes));

    }

}