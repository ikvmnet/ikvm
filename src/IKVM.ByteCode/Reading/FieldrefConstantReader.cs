using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class FieldrefConstantReader : Constant<FieldrefConstantRecord>
    {

        ClassConstantReader clazz;
        NameAndTypeConstantReader nameAndType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public FieldrefConstantReader(ClassReader declaringClass, FieldrefConstantRecord record) :
            base(declaringClass, record)
        {

        }

        public ClassConstantReader Class => clazz ??= DeclaringClass.ResolveConstant<ClassConstantReader>(Record.ClassIndex);

        public NameAndTypeConstantReader NameAndType => nameAndType ??= DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex);

    }

}