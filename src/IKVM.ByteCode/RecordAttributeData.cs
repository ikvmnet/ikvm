namespace IKVM.ByteCode
{

    public sealed class RecordAttributeData : AttributeData<RecordAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public RecordAttributeData(Class declaringClass, AttributeInfo info, RecordAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}