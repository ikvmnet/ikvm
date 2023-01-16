namespace IKVM.ByteCode
{

    public sealed class InnerClassesAttributeData : AttributeData<InnerClassesAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal InnerClassesAttributeData(Class declaringClass, AttributeInfo info, InnerClassesAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}