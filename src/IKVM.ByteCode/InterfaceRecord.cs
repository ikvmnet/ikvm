namespace IKVM.ByteCode
{

    class InterfaceRecord
    {

        readonly ushort classIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classIndex"></param>
        public InterfaceRecord(ushort classIndex)
        {
            this.classIndex = classIndex;
        }

        /// <summary>
        /// Resolves the interface.
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public Interface Resolve(ClassRecord clazz)
        {
            var i = new Interface();
            i.Name = clazz.ResolveClassConstantName(classIndex);
            return i;
        }

    }

}
