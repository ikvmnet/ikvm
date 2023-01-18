using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal class SyntheticAttributeReader : AttributeReader<SyntheticAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal SyntheticAttributeReader(ClassReader declaringClass, AttributeInfoReader info, SyntheticAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}