using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ModuleConstantReader : Constant<ModuleConstantRecord>
    {

        Utf8ConstantReader name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public ModuleConstantReader(ClassReader owner, ModuleConstantRecord record) :
            base(owner, record)
        {

        }

        public Utf8ConstantReader Name => name ??= DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex);

    }

}
