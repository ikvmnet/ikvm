namespace IKVM.ByteCode
{

    public class BootstrapMethodsAttributeData : AttributeData<BootstrapMethodsAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal BootstrapMethodsAttributeData(Class declaringClass, AttributeInfo info, BootstrapMethodsAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}