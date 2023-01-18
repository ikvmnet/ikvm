using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class MethodTypeConstantReader : ConstantReader<MethodTypeConstantRecord, MethodTypeConstantOverride>
    {

        string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public MethodTypeConstantReader(ClassReader owner, MethodTypeConstantRecord record, MethodTypeConstantOverride @override = null) :
            base(owner, record, @override)
        {

        }

        /// <summary>
        /// Gets the type of this MethodType constant.
        /// </summary>
        public string Type => LazyGet(ref type, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.DescriptorIndex).Value);

        /// <summary>
        /// Returns <c>true</c> if this constant is loadable.
        /// </summary>
        public override bool IsLoadable => DeclaringClass.MajorVersion >= 51;

    }

}
