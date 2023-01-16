using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class AnnotationDefaultAttributeReader : AttributeData<AnnotationDefaultAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal AnnotationDefaultAttributeReader(ClassReader declaringClass, AttributeInfoReader info, AnnotationDefaultAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}