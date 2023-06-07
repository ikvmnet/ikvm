using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public class SignatureAttributeReader : AttributeReader<SignatureAttributeRecord>
    {

        string value;

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

        /// <summary>
        /// Gets the signature value.
        /// </summary>
        public string Value => LazyGet(ref value, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.SignatureIndex).Value);

    }

}
