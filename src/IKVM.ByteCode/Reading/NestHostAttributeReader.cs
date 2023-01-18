using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class NestHostAttributeReader : AttributeReader<NestHostAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public NestHostAttributeReader(ClassReader declaringClass, AttributeInfoReader info, NestHostAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}