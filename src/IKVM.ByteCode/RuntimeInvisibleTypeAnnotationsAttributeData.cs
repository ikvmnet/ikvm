namespace IKVM.ByteCode
{

    public sealed class RuntimeInvisibleTypeAnnotationsAttributeData : AttributeData<RuntimeInvisibleTypeAnnotationsAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeInvisibleTypeAnnotationsAttributeData(Class declaringClass, AttributeInfo info, RuntimeInvisibleTypeAnnotationsAttributeDataRecord data) : 
            base(declaringClass, info, data)
        {

        }

    }

}
