namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides data to override an InterfaceMethodref constant.
    /// </summary>
    internal sealed class InterfaceMethodrefConstantOverride : RefConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public InterfaceMethodrefConstantOverride(string className, string name, string type, object value) :
            base(className, name, type, value)
        {

        }

    }

}
