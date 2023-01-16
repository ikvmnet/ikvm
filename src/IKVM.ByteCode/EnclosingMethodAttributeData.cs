namespace IKVM.ByteCode
{

    public sealed class EnclosingMethodAttributeData : AttributeData<EnclosingMethodAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal EnclosingMethodAttributeData(Class declaringClass, AttributeInfo info, EnclosingMethodAttributeDataRecord data) : 
            base(declaringClass, info, data)
        {

        }

    }

}
