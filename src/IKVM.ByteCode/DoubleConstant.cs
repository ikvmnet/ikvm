namespace IKVM.ByteCode
{

    public class DoubleConstant : Constant<DoubleConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        public DoubleConstant(Class clazz, DoubleConstantRecord record) :
            base(clazz, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public double Value => Record.Value;

    }

}