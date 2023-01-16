namespace IKVM.ByteCode
{

    public sealed class LineNumberTableAttributeData : AttributeData<LineNumberTableAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal LineNumberTableAttributeData(Class declaringClass, AttributeInfo info, LineNumberTableAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
