namespace IKVM.ByteCode
{

    public sealed class InterfaceMethodrefConstant : Constant<InterfaceMethodrefConstantRecord>
    {

        ClassConstant clazz;
        NameAndTypeConstant nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public InterfaceMethodrefConstant(Class owner, InterfaceMethodrefConstantRecord record) :
            base(owner, record)
        {

        }

        public ClassConstant Class => clazz ??= DeclaringClass.ResolveConstant<ClassConstant>(Record.ClassIndex);

        public NameAndTypeConstant NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstant>(Record.NameAndTypeIndex);

    }

}