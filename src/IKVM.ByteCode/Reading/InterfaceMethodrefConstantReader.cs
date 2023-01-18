using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class InterfaceMethodrefConstantReader : RefConstantReader<InterfaceMethodrefConstantRecord, InterfaceMethodrefConstantOverride>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public InterfaceMethodrefConstantReader(ClassReader declaringClass, InterfaceMethodrefConstantRecord record, InterfaceMethodrefConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

    }

}