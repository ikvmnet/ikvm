namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides data to override a Fieldref constant.
    /// </summary>
    public sealed class FieldrefConstantOverride : RefConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public FieldrefConstantOverride(string className, string name, string type, object value) :
            base(className, name, type, value)
        {

        }

    }

}
