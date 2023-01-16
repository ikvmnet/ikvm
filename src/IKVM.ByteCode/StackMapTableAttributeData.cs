namespace IKVM.ByteCode
{

    public class StackMapTableAttributeData : AttributeData<StackMapTableAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal StackMapTableAttributeData(Class declaringClass, AttributeInfo info, StackMapTableAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}