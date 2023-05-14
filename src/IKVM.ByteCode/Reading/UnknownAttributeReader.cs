using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal class UnknownAttributeReader : AttributeReader<UnknownAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public UnknownAttributeReader(ClassReader declaringClass, AttributeInfoReader info, UnknownAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
