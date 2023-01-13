namespace IKVM.ByteCode
{

    public class StringConstant : Constant
    {

        readonly ushort utf8Index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="utf8Index"></param>
        public StringConstant(ushort utf8Index)
        {
            this.utf8Index = utf8Index;
        }

        /// <summary>
        /// Gets the index within the constant pool corresponding to the string value.
        /// </summary>
        public ushort Utf8Index => utf8Index;

    }

}
