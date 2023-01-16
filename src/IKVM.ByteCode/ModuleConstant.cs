namespace IKVM.ByteCode
{

    public sealed class ModuleConstant : Constant<ModuleConstantRecord>
    {

        Utf8Constant name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public ModuleConstant(Class owner, ModuleConstantRecord record) :
            base(owner, record)
        {

        }

        public Utf8Constant Name => name ??= DeclaringClass.ResolveConstant<Utf8Constant>(Record.NameIndex);

    }

}
