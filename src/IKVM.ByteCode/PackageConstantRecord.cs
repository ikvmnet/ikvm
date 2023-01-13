namespace IKVM.ByteCode
{

    public class PackageConstant : Constant
    {

        readonly ushort nameIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="nameIndex"></param>
        public PackageConstant(ushort nameIndex)
        {
            this.nameIndex = nameIndex;
        }

        public ushort NameIndex => nameIndex;

    }

}
