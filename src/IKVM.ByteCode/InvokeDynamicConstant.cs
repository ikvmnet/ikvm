namespace IKVM.ByteCode
{

    public sealed class InvokeDynamicConstant : Constant<InvokeDynamicConstantRecord>
    {

        NameAndTypeConstant nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public InvokeDynamicConstant(Class owner, InvokeDynamicConstantRecord record) :
            base(owner, record)
        {

        }

        public ushort BootstrapMethodAttributeIndex => Record.BootstrapMethodAttributeIndex;

        public NameAndTypeConstant NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstant>(Record.NameAndTypeIndex);

    }

}
