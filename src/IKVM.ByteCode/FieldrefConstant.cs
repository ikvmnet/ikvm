namespace IKVM.ByteCode
{

    public sealed class FieldrefConstant : Constant<FieldrefConstantRecord>
    {

        ClassConstant clazz;
        NameAndTypeConstant nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public FieldrefConstant(Class owner, FieldrefConstantRecord record) :
            base(owner, record)
        {

        }

        public ClassConstant Class => clazz ??= DeclaringClass.ResolveConstant<ClassConstant>(Record.ClassIndex);

        public NameAndTypeConstant NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstant>(Record.NameAndTypeIndex);

    }

}