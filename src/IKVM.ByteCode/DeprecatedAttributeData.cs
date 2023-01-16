namespace IKVM.ByteCode
{

    public class DeprecatedAttributeData : AttributeData<DeprecatedAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal DeprecatedAttributeData(Class declaringClass, AttributeInfo info, DeprecatedAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
