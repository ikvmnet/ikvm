using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class MethodTypeConstantReader : Constant<MethodTypeConstantRecord>
    {

        Utf8ConstantReader descriptor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public MethodTypeConstantReader(ClassReader owner, MethodTypeConstantRecord record) :
            base(owner, record)
        {

        }

        public Utf8ConstantReader Descriptor => descriptor ??= DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.DescriptorIndex);

    }

}
