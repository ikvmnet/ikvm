using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class RecordAttributeReader : AttributeReader<RecordAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public RecordAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RecordAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}