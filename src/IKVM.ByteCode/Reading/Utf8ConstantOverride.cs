namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Represents an override to a UTF8 constant.
    /// </summary>
    public sealed class Utf8ConstantOverride : ConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public Utf8ConstantOverride(string value) :
            base(value)
        {

        }

        /// <summary>
        /// Gets the anonymous value attached to the override.
        /// </summary>
        public new string Value => (string)base.Value;

    }

}
