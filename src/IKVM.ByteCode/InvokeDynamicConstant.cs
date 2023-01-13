namespace IKVM.ByteCode
{

    public class InvokeDynamicConstant : Constant
    {

        readonly ushort bootstrapMethodAttrIndex;
        readonly ushort nameAndTypeIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="bootstrapMethodAttrIndex"></param>
        /// <param name="nameAndTypeIndex"></param>
        public InvokeDynamicConstant(ushort bootstrapMethodAttrIndex, ushort nameAndTypeIndex)
        {
            this.bootstrapMethodAttrIndex = bootstrapMethodAttrIndex;
            this.nameAndTypeIndex = nameAndTypeIndex;
        }

        public ushort BootstrapMethodAttrIndex => bootstrapMethodAttrIndex;

        public ushort DescriptorIndex => nameAndTypeIndex;

    }

}