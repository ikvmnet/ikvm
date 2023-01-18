using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal class FieldrefConstantReader : RefConstantReader<FieldrefConstantRecord, FieldrefConstantOverride>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public FieldrefConstantReader(ClassReader declaringClass, FieldrefConstantRecord record, FieldrefConstantOverride @override) :
            base(declaringClass, record, @override)
        {

        }

    }

}