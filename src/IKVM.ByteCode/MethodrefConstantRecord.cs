namespace IKVM.ByteCode
{

    public class MethodrefConstantRecord : ConstantRecord
    {

        readonly ushort classIndex;
        readonly ushort nameAndTypeIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classIndex"></param>
        /// <param name="nameAndTypeIndex"></param>
        public MethodrefConstantRecord(ushort classIndex, ushort nameAndTypeIndex)
        {
            this.classIndex = classIndex;
            this.nameAndTypeIndex = nameAndTypeIndex;
        }

        public ushort ClassIndex => classIndex;

        public ushort NameAndTypeIndex => nameAndTypeIndex;

    }

}