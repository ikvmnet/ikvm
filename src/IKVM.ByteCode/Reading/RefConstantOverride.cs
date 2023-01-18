namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides data to override a Fieldref, Methodref or InterfaceMethodref constant.
    /// </summary>
    public abstract class RefConstantOverride : ConstantOverride
    {

        readonly string className;
        readonly string name;
        readonly string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        protected RefConstantOverride(string className, string name, string type, object value) :
            base(value)
        {
            this.className = className;
            this.name = name;
            this.type = type;
        }

        /// <summary>
        /// Gets the overridden class name.
        /// </summary>
        public string ClassName => className;

        /// <summary>
        /// Gets the overridden name.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the overridden type.
        /// </summary>
        public string Type => type;

    }

}