namespace IKVM.ByteCode
{

    public sealed class AnnotationDefaultAttributeData : AttributeData<AnnotationDefaultAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal AnnotationDefaultAttributeData(Class declaringClass, AttributeInfo info, AnnotationDefaultAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}