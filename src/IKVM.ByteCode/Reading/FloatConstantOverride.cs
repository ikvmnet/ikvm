namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Represents an override to a Float constant.
    /// </summary>
    public sealed class FloatConstantOverride : ConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public FloatConstantOverride(float value) : 
            base(value)
        {

        }

        /// <summary>
        /// Gets the value to be overridden.
        /// </summary>
        public new float Value => (float)base.Value;

    }

}
