using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class RuntimeInvisibleTypeAnnotationsAttributeReader : AttributeData<RuntimeInvisibleTypeAnnotationsAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal RuntimeInvisibleTypeAnnotationsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, RuntimeInvisibleTypeAnnotationsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
