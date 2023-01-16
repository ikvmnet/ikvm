namespace IKVM.ByteCode
{

    public sealed class LocalVariableTableAttributeData : AttributeData<LocalVariableTableAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal LocalVariableTableAttributeData(Class declaringClass, AttributeInfo info, LocalVariableTableAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
