namespace IKVM.ByteCode
{

    public class DoubleConstant : Constant
    {

        readonly double value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public DoubleConstant(double value)
        {
            this.value = value;
        }

        public double Value => value;

    }

}
