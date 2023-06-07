using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class EnclosingMethodAttributeReader : AttributeReader<EnclosingMethodAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal EnclosingMethodAttributeReader(ClassReader declaringClass, AttributeInfoReader info, EnclosingMethodAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
