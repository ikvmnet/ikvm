using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class InterfaceMethodrefConstantReader : Constant<InterfaceMethodrefConstantRecord>
    {

        ClassConstantReader declaringClass;
        NameAndTypeConstantReader nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public InterfaceMethodrefConstantReader(ClassReader owner, InterfaceMethodrefConstantRecord record) :
            base(owner, record)
        {

        }

        public ClassConstantReader Class => declaringClass ??= DeclaringClass.ResolveConstant<ClassConstantReader>(Record.ClassIndex);

        public NameAndTypeConstantReader NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex);

    }

}