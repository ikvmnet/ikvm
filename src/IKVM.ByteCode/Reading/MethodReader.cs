using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class MethodReader : FieldOrMethodReader<MethodRecord>
    {

        Utf8ConstantReader name;
        Utf8ConstantReader descriptor;
        AttributeReaderCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        internal MethodReader(ClassReader declaringClass, MethodRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the access flags of the method.
        /// </summary>
        public AccessFlag AccessFlags => Record.AccessFlags;

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public Utf8ConstantReader Name => LazyGet(ref name, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex));

        /// <summary>
        /// Gets the descriptor of the method.
        /// </summary>
        public Utf8ConstantReader Descriptor => LazyGet(ref descriptor, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.DescriptorIndex));

        /// <summary>
        /// Gets the attributes of the method.
        /// </summary>
        public AttributeReaderCollection Attributes => LazyGet(ref attributes, () => new AttributeReaderCollection(DeclaringClass, Record.Attributes));

    }

}