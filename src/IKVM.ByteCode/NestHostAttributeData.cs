namespace IKVM.ByteCode
{

    public sealed class NestHostAttributeData : AttributeData<NestHostAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public NestHostAttributeData(Class declaringClass, AttributeInfo info, NestHostAttributeDataRecord data) : 
            base(declaringClass, info, data)
        {

        }

    }

}