namespace IKVM.ByteCode
{

    public class TypeAndNameConstantRecord : ConstantRecord
    {

        readonly ushort nameIndex;
        readonly ushort descriptorIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classIndex"></param>
        /// <param name="nameAndTypeIndex"></param>
        public TypeAndNameConstantRecord(ushort classIndex, ushort nameAndTypeIndex)
        {
            this.nameIndex = classIndex;
            this.descriptorIndex = nameAndTypeIndex;
        }

        public ushort NameIndex => nameIndex;

        public ushort DescriptorIndex => descriptorIndex;

    }

}