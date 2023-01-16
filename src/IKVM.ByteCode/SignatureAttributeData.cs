namespace IKVM.ByteCode
{

    public class SignatureAttributeData : AttributeData<SignatureAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal SignatureAttributeData(Class declaringClass, AttributeInfo info, SignatureAttributeDataRecord data) : 
            base(declaringClass, info, data)
        {

        }

    }

}
