using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class MethodrefConstantReader : RefConstantReader<MethodrefConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public MethodrefConstantReader(ClassReader declaringClass, ushort index, MethodrefConstantRecord record) :
            base(declaringClass, index, record)
        {

        }

    }

}