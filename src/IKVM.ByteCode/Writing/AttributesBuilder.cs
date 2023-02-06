using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class AttributesBuilder : RecordsBuilder<AttributeInfoRecord>
    {
        private CodeAttributeBuilder codeAttribute;

        private DeprecatedAttributeBuilder deprecatedAttribute;

        private ExceptionsAttributeBuilder exceptionsAttribute;

        private InnerClassesAttributeBuilder innerClassesAttribute;

        private SignatureAttributeBuilder signatureAttribute;

        private RuntimeVisibleAnnotationsAttributeBuilder runtimeVisibleAnnotationsAttribute;

        public AttributesBuilder(ClassBuilder declaringClass)
            : base(declaringClass)
        {
        }

        public AttributesBuilder WithCodeAttribute(ushort maxStack, ushort maxLocals, ReadOnlyMemory<byte> code)
        {
            codeAttribute = new CodeAttributeBuilder(maxStack, maxLocals, code, DeclaringClass);
            return this;
        }

        public AttributesBuilder WithDeprecatedAttribute()
        {
            deprecatedAttribute = new DeprecatedAttributeBuilder(DeclaringClass);
            return this;
        }

        public AttributesBuilder WithExceptionsAttribute()
        {
            exceptionsAttribute = new ExceptionsAttributeBuilder(DeclaringClass);
            return this;
        }

        public AttributesBuilder WithInnerClassesAttribute()
        {
            innerClassesAttribute = new InnerClassesAttributeBuilder(DeclaringClass);
            return this;
        }

        public AttributesBuilder WithSignatureAttribute(string signature)
        {
            signatureAttribute = new SignatureAttributeBuilder(signature, DeclaringClass);
            return this;
        }

        public AttributesBuilder WithRuntimeVisibleAnnotationsAttribute()
        {
            runtimeVisibleAnnotationsAttribute = new RuntimeVisibleAnnotationsAttributeBuilder(DeclaringClass);
            return this;
        }

        public override AttributeInfoRecord[] Build() => new AttributeInfoRecord[0]; 
    }
}
