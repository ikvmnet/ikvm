using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class RuntimeVisibleTypeAnnotationsAttributeReader : AttributeData<RuntimeVisibleTypeAnnotationsAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeVisibleTypeAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeVisibleTypeAnnotationsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
