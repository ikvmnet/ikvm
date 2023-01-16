using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class DynamicConstantReader : Constant<DynamicConstantRecord>
    {

        NameAndTypeConstantReader nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public DynamicConstantReader(ClassReader declaringClass, DynamicConstantRecord record) :
            base(declaringClass, record)
        {

        }

        public ushort BootstrapMethodAttributeIndex => Record.BootstrapMethodAttributeIndex;

        public NameAndTypeConstantReader NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex);

    }

}
