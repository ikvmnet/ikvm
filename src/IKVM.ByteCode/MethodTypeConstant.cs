namespace IKVM.ByteCode
{

    public sealed class MethodTypeConstant : Constant<MethodTypeConstantRecord>
    {

        Utf8Constant descriptor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public MethodTypeConstant(Class owner, MethodTypeConstantRecord record) :
            base(owner, record)
        {

        }

        public Utf8Constant Descriptor => descriptor ??= DeclaringClass.ResolveConstant<Utf8Constant>(Record.DescriptorIndex);

    }

}
