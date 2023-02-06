using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class SignatureAttributeBuilder : BuilderBase<SignatureAttributeRecord>
    {
        public SignatureAttributeBuilder(string signature, ClassBuilder declaringClass) :
            base(declaringClass)
        {
            SignatureIndex = declaringClass.AddUtf8(signature);
        }

        private ushort SignatureIndex { get; }

        public override SignatureAttributeRecord Build() => new
            (
                SignatureIndex: SignatureIndex
            );
    }
}
