using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class InvokeDynamicConstantReader : ConstantReader<InvokeDynamicConstantRecord>
    {

        NameAndTypeConstantReader nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public InvokeDynamicConstantReader(ClassReader owner, ushort index, InvokeDynamicConstantRecord record) :
            base(owner, index, record)
        {

        }

        /// <summary>
        /// Gets the index into the BootstrapMethod table that is referenced by this constant.
        /// </summary>
        public ushort BootstrapMethodAttributeIndex => Record.BootstrapMethodAttributeIndex;

        /// <summary>
        /// Gets the name of the InvokeDynamic constant.
        /// </summary>
        public NameAndTypeConstantReader NameAndType => LazyGet(ref nameAndType, () =>  DeclaringClass.Constants.Get<NameAndTypeConstantReader>(Record.NameAndTypeIndex));

    }

}
