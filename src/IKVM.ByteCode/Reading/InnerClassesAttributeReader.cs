using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class InnerClassesAttributeReader : AttributeData<InnerClassesAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal InnerClassesAttributeReader(ClassReader declaringClass, AttributeInfoReader info, InnerClassesAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}