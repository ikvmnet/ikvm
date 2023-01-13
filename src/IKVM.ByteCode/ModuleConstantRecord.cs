namespace IKVM.ByteCode
{

    public class ModuleConstant : Constant
    {

        readonly ushort nameIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="nameIndex"></param>
        public ModuleConstant(ushort nameIndex)
        {
            this.nameIndex = nameIndex;
        }

        public ushort NameIndex => nameIndex;

    }

}
