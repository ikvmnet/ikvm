namespace IKVM.ByteCode
{

    public sealed class RuntimeInvisibleParameterAnnotationsAttributeData : AttributeData<RuntimeInvisibleParameterAnnotationsAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public RuntimeInvisibleParameterAnnotationsAttributeData(Class declaringClass, AttributeInfo info, RuntimeInvisibleParameterAnnotationsAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
