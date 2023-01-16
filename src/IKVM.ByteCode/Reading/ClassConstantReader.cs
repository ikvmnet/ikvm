using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ClassConstantReader : Constant<ClassConstantRecord>
    {

        Utf8ConstantReader name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ClassConstantReader(ClassReader declaringClass, ClassConstantRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public Utf8ConstantReader Name => name ??= DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex);

    }

}