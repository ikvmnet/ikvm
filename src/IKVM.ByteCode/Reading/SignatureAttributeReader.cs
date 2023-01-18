using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class SignatureAttributeReader : AttributeReader<SignatureAttributeRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal SignatureAttributeReader(ClassReader declaringClass, AttributeInfoReader info, SignatureAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

    }

}
