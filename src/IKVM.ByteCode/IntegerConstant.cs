namespace IKVM.ByteCode
{

    public sealed class IntegerConstant : Constant<IntegerConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        public IntegerConstant(Class clazz, IntegerConstantRecord record) :
            base(clazz, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant. Result is interned.
        /// </summary>
        public int Value => Record.Value;

    }

}