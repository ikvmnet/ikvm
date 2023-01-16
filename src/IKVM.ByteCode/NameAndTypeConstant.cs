namespace IKVM.ByteCode
{

    public sealed class NameAndTypeConstant : Constant<NameAndTypeConstantRecord>
    {

        Utf8Constant name;
        Utf8Constant descriptor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        public NameAndTypeConstant(Class clazz, NameAndTypeConstantRecord record) :
            base(clazz, record)
        {

        }

        public Utf8Constant Name => name ??= DeclaringClass.ResolveConstant<Utf8Constant>(Record.NameIndex);

        public Utf8Constant Descriptor => descriptor ??= DeclaringClass.ResolveConstant<Utf8Constant>(Record.DescriptorIndex);

    }

}
