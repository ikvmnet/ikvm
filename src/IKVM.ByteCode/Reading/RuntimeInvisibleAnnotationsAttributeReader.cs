using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class RuntimeInvisibleAnnotationsAttributeReader : AttributeData<RuntimeInvisibleAnnotationsAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeInvisibleAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeInvisibleAnnotationsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}