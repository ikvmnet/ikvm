using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class InvokeDynamicConstantReader : Constant<InvokeDynamicConstantRecord>
    {

        NameAndTypeConstantReader nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public InvokeDynamicConstantReader(ClassReader owner, InvokeDynamicConstantRecord record) :
            base(owner, record)
        {

        }

        public ushort BootstrapMethodAttributeIndex => Record.BootstrapMethodAttributeIndex;

        public NameAndTypeConstantReader NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex);

    }

}
