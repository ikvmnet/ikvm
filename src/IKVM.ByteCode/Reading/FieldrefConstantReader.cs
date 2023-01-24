using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal class FieldrefConstantReader : RefConstantReader<FieldrefConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public FieldrefConstantReader(ClassReader declaringClass, ushort index, FieldrefConstantRecord record) :
            base(declaringClass, index, record)
        {

        }

    }

}
