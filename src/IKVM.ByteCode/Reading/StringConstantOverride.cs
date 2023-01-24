namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Represents an override to a String constant.
    /// </summary>
    internal sealed class StringConstantOverride : ConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public StringConstantOverride(object value) :
            base(value)
        {

        }

    }

}
