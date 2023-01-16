using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ExceptionsAttributeReader : AttributeData<ExceptionsAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal ExceptionsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, ExceptionsAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
