namespace IKVM.ByteCode
{

    public sealed class ModuleMainClassAttributeData : AttributeData<ModuleMainClassAttributeDataRecord>
    {

        /// <summary>
        /// initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public ModuleMainClassAttributeData(Class declaringClass, AttributeInfo info, ModuleMainClassAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
