using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class NameAndTypeConstantReader : ConstantReader<NameAndTypeConstantRecord>
    {

        Utf8ConstantReader name;
        Utf8ConstantReader type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public NameAndTypeConstantReader(ClassReader declaringClass, ushort index, NameAndTypeConstantRecord record) :
            base(declaringClass, index, record)
        {

        }

        /// <summary>
        /// Gets the name of this name and type constant.
        /// </summary>
        public Utf8ConstantReader Name => LazyGet(ref name, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.NameIndex));

        /// <summary>
        /// Gets the type of this name and type constant.
        /// </summary>
        public Utf8ConstantReader Type => LazyGet(ref type, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.DescriptorIndex));

    }

}
