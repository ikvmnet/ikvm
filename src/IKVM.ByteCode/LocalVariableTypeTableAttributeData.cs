namespace IKVM.ByteCode
{

    public sealed class LocalVariableTypeTableAttributeData : AttributeData<LocalVariableTypeTableAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal LocalVariableTypeTableAttributeData(Class declaringClass, AttributeInfo info, LocalVariableTypeTableAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
