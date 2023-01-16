namespace IKVM.ByteCode
{

    public sealed class RuntimeInvisibleAnnotationsAttributeData : AttributeData<RuntimeInvisibleAnnotationsAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeInvisibleAnnotationsAttributeData(Class declaringClass, AttributeInfo info, RuntimeInvisibleAnnotationsAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}