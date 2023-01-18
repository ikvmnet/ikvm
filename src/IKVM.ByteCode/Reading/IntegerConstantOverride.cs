namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Represents an override to an Integer constant.
    /// </summary>
    public class IntegerConstantOverride : ConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public IntegerConstantOverride(int value) : 
            base(value)
        {

        }

        /// <summary>
        /// Gets the value to be overridden.
        /// </summary>
        public new int Value => (int)base.Value;

    }

}
