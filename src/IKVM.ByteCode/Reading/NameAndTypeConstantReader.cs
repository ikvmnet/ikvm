using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class NameAndTypeConstantReader : ConstantReader<NameAndTypeConstantRecord, NameAndTypeConstantOverride>
    {

        string name;
        string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public NameAndTypeConstantReader(ClassReader declaringClass, NameAndTypeConstantRecord record, NameAndTypeConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

        /// <summary>
        /// Gets the name of this name and type constant.
        /// </summary>
        public string Name => LazyGet(ref name, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex).Value);

        /// <summary>
        /// Gets the type of this name and type constant.
        /// </summary>
        public string Type => LazyGet(ref type, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.DescriptorIndex).Value);

    }

}
