using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class SignatureAttributeBuilder : AttributeBuilder<SignatureAttributeRecord>
    {
        public SignatureAttributeBuilder(string signature, ClassBuilder declaringClass) :
            base(declaringClass)
        {
            SignatureIndex = declaringClass.AddUtf8(signature);
        }

        public override string Name => SignatureAttributeRecord.Name;

        private ushort SignatureIndex { get; }

        public override SignatureAttributeRecord Build() => new
            (
                SignatureIndex: SignatureIndex
            );
    }
}
