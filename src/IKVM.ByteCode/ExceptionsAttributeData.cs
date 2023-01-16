namespace IKVM.ByteCode
{

    public sealed class ExceptionsAttributeData : AttributeData<ExceptionsAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal ExceptionsAttributeData(Class declaringClass, AttributeInfo info, ExceptionsAttributeDataRecord data) : 
            base(declaringClass, info, data)
        {

        }

    }

}
