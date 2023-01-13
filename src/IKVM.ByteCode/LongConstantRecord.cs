namespace IKVM.ByteCode
{

    public class LongConstant : Constant
    {

        readonly long value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public LongConstant(long value)
        {
            this.value = value;
        }

    }

}
