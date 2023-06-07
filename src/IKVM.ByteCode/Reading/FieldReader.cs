using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public class FieldReader : FieldOrMethodReader<FieldRecord>
    {

        Utf8ConstantReader name;
        Utf8ConstantReader descriptor;
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
        public override AccessFlag AccessFlags => Record.AccessFlags;

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public override Utf8ConstantReader Name => LazyGet(ref name, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.NameIndex));

        /// <summary>
        /// Gets the descriptor of the field.
        /// </summary>
        public override Utf8ConstantReader Descriptor => LazyGet(ref descriptor, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.DescriptorIndex));

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        public override AttributeReaderCollection Attributes => LazyGet(ref attributes, () => new AttributeReaderCollection(DeclaringClass, Record.Attributes));

    }

}