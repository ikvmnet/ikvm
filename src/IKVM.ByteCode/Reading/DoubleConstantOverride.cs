namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Represents an override to a Double constant.
    /// </summary>
    internal sealed class DoubleConstantOverride : ConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public DoubleConstantOverride(double value) :
            base(value)
        {

        }

        /// <summary>
        /// Gets the value to be overridden.
        /// </summary>
        public new double Value => (double)base.Value;

    }

}
