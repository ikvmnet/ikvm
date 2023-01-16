namespace IKVM.ByteCode
{

    public sealed class PermittedSubclassesAttributeData : AttributeData<PermittedSubclassesAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public PermittedSubclassesAttributeData(Class declaringClass, AttributeInfo info, PermittedSubclassesAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
