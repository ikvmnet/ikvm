using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ModuleConstantReader : ConstantReader<ModuleConstantRecord, ModuleConstantOverride>
    {

        string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public ModuleConstantReader(ClassReader owner, ModuleConstantRecord record, ModuleConstantOverride @override = null) :
            base(owner, record, @override)
        {

        }

        /// <summary>
        /// Gets the name of this module.
        /// </summary>
        public string Name => LazyGet(ref name, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex).Value);

    }

}
