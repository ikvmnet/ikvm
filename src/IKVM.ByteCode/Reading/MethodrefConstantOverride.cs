namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides data to override a Methodref constant.
    /// </summary>
    internal sealed class MethodrefConstantOverride : RefConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public MethodrefConstantOverride(string className, string name, string type, object value) :
            base(className, name, type, value)
        {

        }

    }

}
