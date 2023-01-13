namespace IKVM.ByteCode
{

    public class MethodTypeConstant : Constant
    {

        readonly ushort descriptorIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="descriptorIndex"></param>
        public MethodTypeConstant( ushort descriptorIndex)
        {
            this.descriptorIndex = descriptorIndex;
        }

        public ushort NameAndTypeIndex => descriptorIndex;

    }

}