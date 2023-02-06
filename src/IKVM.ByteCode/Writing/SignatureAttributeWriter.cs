using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class SignatureAttributeWriter : WriterBase<SignatureAttributeRecord>
    {
        private readonly SignatureAttributeRecord record;

        public SignatureAttributeWriter(string signature, ClassWriter declaringClass) :
            base(declaringClass)
        {
            record = new SignatureAttributeRecord(declaringClass.AddUtf8(signature));
        }

        internal override SignatureAttributeRecord Record => record;
    }
}
