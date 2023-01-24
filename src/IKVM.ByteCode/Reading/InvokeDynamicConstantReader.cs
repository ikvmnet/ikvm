using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class InvokeDynamicConstantReader : ConstantReader<InvokeDynamicConstantRecord, InvokeDynamicConstantOverride>
    {

        string name;
        string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public InvokeDynamicConstantReader(ClassReader owner, InvokeDynamicConstantRecord record, InvokeDynamicConstantOverride @override) :
            base(owner, record, @override)
        {

        }

        /// <summary>
        /// Gets the index into the BootstrapMethod table that is referenced by this constant.
        /// </summary>
        public ushort BootstrapMethodAttributeIndex => Record.BootstrapMethodAttributeIndex;

        /// <summary>
        /// Gets the name of the InvokeDynamic constant.
        /// </summary>
        public string Name => LazyGet(ref name, () => Override != null ? Override.Name : DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex).Name);

        /// <summary>
        /// Gets the name of the InvokeDynamic constant.
        /// </summary>
        public string Type => LazyGet(ref type, () => Override != null ? Override.Type : DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex).Type);

    }

}
