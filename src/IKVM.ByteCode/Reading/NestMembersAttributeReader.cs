using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class NestMembersAttributeReader : AttributeReader<NestMembersAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public NestMembersAttributeReader(ClassReader declaringClass, AttributeInfoReader info, NestMembersAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
