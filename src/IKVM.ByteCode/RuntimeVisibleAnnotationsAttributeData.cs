namespace IKVM.ByteCode
{

    public class RuntimeVisibleAnnotationsAttributeData : AttributeData<RuntimeVisibleAnnotationsAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeVisibleAnnotationsAttributeData(Class declaringClass, AttributeInfo info, RuntimeVisibleAnnotationsAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}