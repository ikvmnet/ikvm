namespace IKVM.ByteCode
{

    public sealed class LongConstant : Constant<LongConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        public LongConstant(Class clazz, LongConstantRecord record) :
            base(clazz, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public long Value => Record.Value;

    }

}