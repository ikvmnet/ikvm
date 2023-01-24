namespace IKVM.ByteCode.Reading
{

    internal sealed class DynamicConstantOverride : ConstantOverride
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public DynamicConstantOverride(object value) :
            base(value)
        {

        }

    }

}