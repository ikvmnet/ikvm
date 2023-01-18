namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Represents an override to a Long constant.
    /// </summary>
    public class LongConstantOverride : ConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public LongConstantOverride(long value) :
            base(value)
        {

        }

        /// <summary>
        /// Gets the value to be overridden in.
        /// </summary>
        public new long Value => (long)base.Value;

    }

}
