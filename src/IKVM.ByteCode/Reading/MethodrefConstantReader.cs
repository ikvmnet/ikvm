using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class MethodrefConstantReader : Constant<MethodrefConstantRecord>
    {

        ClassConstantReader declaringClass;
        NameAndTypeConstantReader nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public MethodrefConstantReader(ClassReader declaringClass, MethodrefConstantRecord record) :
            base(declaringClass, record)
        {

        }

        public ClassConstantReader Class => declaringClass ??= DeclaringClass.ResolveConstant<ClassConstantReader>(Record.ClassIndex);

        public NameAndTypeConstantReader NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex);

    }

}