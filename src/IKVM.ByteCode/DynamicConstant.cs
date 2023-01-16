namespace IKVM.ByteCode
{

    public class DynamicConstant : Constant<DynamicConstantRecord>
    {

        NameAndTypeConstant nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        public DynamicConstant(Class clazz, DynamicConstantRecord record) :
            base(clazz, record)
        {

        }

        public ushort BootstrapMethodAttributeIndex => Record.BootstrapMethodAttributeIndex;

        public NameAndTypeConstant NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstant>(Record.NameAndTypeIndex);

    }

}
