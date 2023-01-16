namespace IKVM.ByteCode
{

    public sealed class FloatConstant : Constant<FloatConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        public FloatConstant(Class clazz, FloatConstantRecord record) :
            base(clazz, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public float Value => Record.Value;

    }

}