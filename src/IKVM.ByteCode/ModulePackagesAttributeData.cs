namespace IKVM.ByteCode
{

    public sealed class ModulePackagesAttributeData : AttributeData<ModulePackagesAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal ModulePackagesAttributeData(Class declaringClass, AttributeInfo info, ModulePackagesAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
