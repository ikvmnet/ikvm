using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class LineNumberTableAttributeReader : AttributeReader<LineNumberTableAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal LineNumberTableAttributeReader(ClassReader declaringClass, AttributeInfoReader info, LineNumberTableAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
