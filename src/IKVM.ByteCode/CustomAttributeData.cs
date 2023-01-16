namespace IKVM.ByteCode
{

    public class CustomAttributeData : AttributeData<CustomAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public CustomAttributeData(Class declaringClass, AttributeInfo info, CustomAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
