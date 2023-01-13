namespace IKVM.ByteCode
{

    public class ClassConstant : Constant
    {

        readonly ushort nameIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="nameIndex"></param>
        public ClassConstant(ushort nameIndex)
        {
            this.nameIndex = nameIndex;
        }

        /// <summary>
        /// Gets the index within the constant pool corresponding to the class name.
        /// </summary>
        public ushort NameIndex => nameIndex;

    }

}
