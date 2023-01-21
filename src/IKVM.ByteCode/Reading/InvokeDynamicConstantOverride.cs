namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides data to override an InvokeDynamic constant.
    /// </summary>
    internal class InvokeDynamicConstantOverride : ConstantOverride
    {

        readonly string name;
        readonly string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public InvokeDynamicConstantOverride(string name, string type, object value) :
            base(value)
        {
            this.name = name;
            this.type = type;
        }

        public string Name => name;

        public string Type => type;

    }

}