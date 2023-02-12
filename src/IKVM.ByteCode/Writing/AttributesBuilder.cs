using IKVM.ByteCode.Parsing;
using System;
using System.Collections.Generic;

namespace IKVM.ByteCode.Writing
{
    internal class AttributesBuilder : RecordsBuilder<AttributeInfoRecord>
    {
        private AttributeInfoBuilder<CodeAttributeRecord> codeAttribute;

        private AttributeInfoBuilder<DeprecatedAttributeRecord> deprecatedAttribute;

        private AttributeInfoBuilder<ExceptionsAttributeRecord> exceptionsAttribute;

        private AttributeInfoBuilder<InnerClassesAttributeRecord> innerClassesAttribute;

        private AttributeInfoBuilder<SignatureAttributeRecord> signatureAttribute;

        private AttributeInfoBuilder<RuntimeVisibleAnnotationsAttributeRecord> runtimeVisibleAnnotationsAttribute;

        public AttributesBuilder(ClassBuilder declaringClass)
            : base(declaringClass)
        {
        }

        public CodeAttributeBuilder AddCodeAttribute(ushort maxStack, ushort maxLocals, byte[] code)
        {
            var attribute = new CodeAttributeBuilder(maxStack, maxLocals, code, DeclaringClass);

            codeAttribute = new AttributeInfoBuilder<CodeAttributeRecord>(attribute, DeclaringClass);

            return attribute;
        }

        public DeprecatedAttributeBuilder AddDeprecatedAttribute()
        {
            var attribute = new DeprecatedAttributeBuilder(DeclaringClass);

            deprecatedAttribute = new AttributeInfoBuilder<DeprecatedAttributeRecord>(attribute, DeclaringClass);

            return attribute;
        }

        public ExceptionsAttributeBuilder AddExceptionsAttribute()
        {
            var attribute = new ExceptionsAttributeBuilder(DeclaringClass);

            exceptionsAttribute = new AttributeInfoBuilder<ExceptionsAttributeRecord>(attribute, DeclaringClass);

            return attribute;
        }

        public InnerClassesAttributeBuilder AddInnerClassesAttribute()
        {
            var attribute = new InnerClassesAttributeBuilder(DeclaringClass);

            innerClassesAttribute = new AttributeInfoBuilder<InnerClassesAttributeRecord>(attribute, DeclaringClass);

            return attribute;
        }

        public SignatureAttributeBuilder AddSignatureAttribute(string signature)
        {
            var attribute = new SignatureAttributeBuilder(signature, DeclaringClass);

            signatureAttribute = new AttributeInfoBuilder<SignatureAttributeRecord>(attribute, DeclaringClass);

            return attribute;
        }

        public RuntimeVisibleAnnotationsAttributeBuilder AddRuntimeVisibleAnnotationsAttribute()
        {
            var attribute = new RuntimeVisibleAnnotationsAttributeBuilder(DeclaringClass);

            runtimeVisibleAnnotationsAttribute = new AttributeInfoBuilder<RuntimeVisibleAnnotationsAttributeRecord>(attribute, DeclaringClass);

            return attribute;
        }

        public override AttributeInfoRecord[] Build()
        {
            var attributes = new List<AttributeInfoRecord>();

            if (codeAttribute is not null)
            {
                attributes.Add(codeAttribute.Build());
            }

            if (deprecatedAttribute is not null)
            {
                attributes.Add(deprecatedAttribute.Build());
            }

            if (exceptionsAttribute is not null)
            {
                attributes.Add(exceptionsAttribute.Build());
            }

            if (innerClassesAttribute is not null)
            {
                attributes.Add(innerClassesAttribute.Build());
            }

            if (signatureAttribute is not null)
            {
                attributes.Add(signatureAttribute.Build());
            }

            if (runtimeVisibleAnnotationsAttribute is not null)
            {
                attributes.Add(runtimeVisibleAnnotationsAttribute.Build());
            }

            return attributes.ToArray();
        }
    }
}
