using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class DynamicConstantReader : ConstantReader<DynamicConstantRecord>
    {

        NameAndTypeConstantReader nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public DynamicConstantReader(ClassReader declaringClass, ushort index,  DynamicConstantRecord record) :
            base(declaringClass, index, record)
        {

        }

        public ushort BootstrapMethodAttributeIndex => Record.BootstrapMethodAttributeIndex;

        public NameAndTypeConstantReader NameAndType => LazyGet(ref nameAndType, () => DeclaringClass.Constants.Get<NameAndTypeConstantReader>(Record.NameAndTypeIndex));

        public override bool IsLoadable => DeclaringClass.Version >= new ClassFormatVersion(55, 0);

    }

}
