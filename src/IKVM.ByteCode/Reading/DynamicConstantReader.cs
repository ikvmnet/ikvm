using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class DynamicConstantReader : ConstantReader<DynamicConstantRecord, DynamicConstantOverride>
    {

        string name;
        string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public DynamicConstantReader(ClassReader declaringClass, DynamicConstantRecord record, DynamicConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

        public ushort BootstrapMethodAttributeIndex => Record.BootstrapMethodAttributeIndex;

        public string Name => LazyGet(ref name, () => DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex).Name);

        public string Type => LazyGet(ref type, () => DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex).Type);

        public override bool IsLoadable => DeclaringClass.MajorVersion >= 55;

    }

}
