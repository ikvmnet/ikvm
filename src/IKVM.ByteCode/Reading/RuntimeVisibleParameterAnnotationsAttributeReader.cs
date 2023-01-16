using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class RuntimeVisibleParameterAnnotationsAttributeReader : AttributeData<RuntimeVisibleParameterAnnotationsAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeVisibleParameterAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeVisibleParameterAnnotationsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}