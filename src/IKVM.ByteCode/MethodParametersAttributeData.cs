namespace IKVM.ByteCode
{

    public sealed class MethodParametersAttributeData : AttributeData<MethodParametersAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal MethodParametersAttributeData(Class declaringClass, AttributeInfo info, MethodParametersAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}