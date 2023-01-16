namespace IKVM.ByteCode
{

    public class RuntimeVisibleParameterAnnotationsAttributeData : AttributeData<RuntimeVisibleParameterAnnotationsAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeVisibleParameterAnnotationsAttributeData(Class declaringClass, AttributeInfo info, RuntimeVisibleParameterAnnotationsAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}