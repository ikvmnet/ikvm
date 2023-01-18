using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class StackMapTableAttributeReader : AttributeReader<StackMapTableAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal StackMapTableAttributeReader(ClassReader declaringClass, AttributeInfoReader info, StackMapTableAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}