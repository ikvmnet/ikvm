namespace IKVM.ByteCode
{

    public sealed class ModuleAttributeData : AttributeData<ModuleAttributeDataRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public ModuleAttributeData(Class declaringClass, AttributeInfo info, ModuleAttributeDataRecord data) : 
            base(declaringClass, info, data)
        {

        }

    }

}
