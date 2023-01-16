namespace IKVM.ByteCode
{

    public sealed class NestMembersAttributeData : AttributeData<NestMembersAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public NestMembersAttributeData(Class declaringClass, AttributeInfo info, NestMembersAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
