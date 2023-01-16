namespace IKVM.ByteCode
{

    public class SyntheticAttributeData : AttributeData<SyntheticAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal SyntheticAttributeData(Class declaringClass, AttributeInfo info, SyntheticAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}