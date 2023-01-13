namespace IKVM.ByteCode
{

    public class FloatConstant : Constant
    {

        readonly float value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public FloatConstant(float value)
        {
            this.value = value;
        }

    }

}
