using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class NameAndTypeConstantReader : Constant<NameAndTypeConstantRecord>
    {

        Utf8ConstantReader name;
        Utf8ConstantReader descriptor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public NameAndTypeConstantReader(ClassReader declaringClass, NameAndTypeConstantRecord record) :
            base(declaringClass, record)
        {

        }

        public Utf8ConstantReader Name => name ??= DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex);

        public Utf8ConstantReader Descriptor => descriptor ??= DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.DescriptorIndex);

    }

}
