using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class RuntimeInvisibleParameterAnnotationsAttributeReader : AttributeData<RuntimeInvisibleParameterAnnotationsAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public RuntimeInvisibleParameterAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeInvisibleParameterAnnotationsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
