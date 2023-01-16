namespace IKVM.ByteCode
{

    public class SourceDebugExtensionAttributeData : AttributeData<SourceDebugExtensionAttributeDataRecord>
    {

        /// <summary>
        /// Initalizes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal SourceDebugExtensionAttributeData(Class declaringClass, AttributeInfo info, SourceDebugExtensionAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
