namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides an overload to a Module constant.
    /// </summary>
    internal sealed class ModuleConstantOverride : ConstantOverride
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public ModuleConstantOverride(object value) :
            base(value)
        {

        }

    }

}