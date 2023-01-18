namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Abstract class for an override applied to a constant.
    /// </summary>
    internal abstract class ConstantOverride
    {

        readonly object value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        protected ConstantOverride(object value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets an anonymous value made available to constants.
        /// </summary>
        public object Value => value;

    }

}
