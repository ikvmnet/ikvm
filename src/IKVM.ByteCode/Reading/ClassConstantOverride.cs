namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Represents an override to a Class constant.
    /// </summary>
    public class ClassConstantOverride : ConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public ClassConstantOverride(object value) :
            base(value)
        {

        }

    }

}
