using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class MethodrefConstantReader : RefConstantReader<MethodrefConstantRecord, MethodrefConstantOverride>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public MethodrefConstantReader(ClassReader declaringClass, MethodrefConstantRecord record, MethodrefConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

    }

}