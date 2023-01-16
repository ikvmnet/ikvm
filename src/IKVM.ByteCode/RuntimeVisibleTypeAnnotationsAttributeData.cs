namespace IKVM.ByteCode
{

    public class RuntimeVisibleTypeAnnotationsAttributeData : AttributeData<RuntimeVisibleTypeAnnotationsAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeVisibleTypeAnnotationsAttributeData(Class declaringClass, AttributeInfo info, RuntimeVisibleTypeAnnotationsAttributeDataRecord data) : 
            base(declaringClass, info, data)
        {

        }

    }

}
