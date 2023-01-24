using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class InterfaceMethodrefConstantReader : RefConstantReader<InterfaceMethodrefConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public InterfaceMethodrefConstantReader(ClassReader declaringClass, ushort index, InterfaceMethodrefConstantRecord record) :
            base(declaringClass, index, record)
        {

        }

    }

}