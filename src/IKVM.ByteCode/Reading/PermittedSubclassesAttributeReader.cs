using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class PermittedSubclassesAttributeReader : AttributeReader<PermittedSubclassesAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        public PermittedSubclassesAttributeReader(ClassReader declaringClass, AttributeInfoReader info, PermittedSubclassesAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
