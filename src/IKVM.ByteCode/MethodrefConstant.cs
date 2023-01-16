namespace IKVM.ByteCode
{

    public sealed class MethodrefConstant : Constant<MethodrefConstantRecord>
    {

        ClassConstant clazz;
        NameAndTypeConstant nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public MethodrefConstant(Class owner, MethodrefConstantRecord record) :
            base(owner, record)
        {

        }

        public ClassConstant Class => clazz ??= DeclaringClass.ResolveConstant<ClassConstant>(Record.ClassIndex);

        public NameAndTypeConstant NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstant>(Record.NameAndTypeIndex);

    }

}