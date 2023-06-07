using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class ModuleConstantReader : ConstantReader<ModuleConstantRecord>
    {

        Utf8ConstantReader name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public ModuleConstantReader(ClassReader owner, ushort index, ModuleConstantRecord record) :
            base(owner, index, record)
        {

        }

        /// <summary>
        /// Gets the name of this module.
        /// </summary>
        public Utf8ConstantReader Name => LazyGet(ref name, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.NameIndex));

    }

}
