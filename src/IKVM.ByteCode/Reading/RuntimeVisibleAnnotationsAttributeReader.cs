using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class RuntimeVisibleAnnotationsAttributeReader : AttributeData<RuntimeVisibleAnnotationsAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeVisibleAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeVisibleAnnotationsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}