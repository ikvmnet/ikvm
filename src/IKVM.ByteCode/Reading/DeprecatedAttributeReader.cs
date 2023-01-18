using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class DeprecatedAttributeReader : AttributeReader<DeprecatedAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal DeprecatedAttributeReader(ClassReader declaringClass, AttributeInfoReader info, DeprecatedAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
