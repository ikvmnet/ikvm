namespace IKVM.ByteCode
{

    public class IntegerConstantRecord : ConstantRecord
    {

        readonly int value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public IntegerConstantRecord(int value)
        {
            this.value = value;
        }

    }

}
