using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class CodeAttributeBuilder : AttributeBuilder<CodeAttributeRecord>
    {
        public CodeAttributeBuilder(ushort maxStack, ushort maxLocals, byte[] code, ClassBuilder declaringClass)
            : base(declaringClass)
        {
            MaxStack = maxStack;
            MaxLocals = maxLocals;
            Code = code;

            Attributes = new AttributesBuilder(declaringClass);
        }

        public override string Name => CodeAttributeRecord.Name;

        private ushort MaxStack { get; }

        private ushort MaxLocals { get; }

        private byte[] Code { get; }

        protected AttributesBuilder Attributes { get; set; }

        public override CodeAttributeRecord Build() => new
            (
                MaxStack: MaxStack,
                MaxLocals: MaxLocals,
                Code: Code,
                new ExceptionHandlerRecord[0],
                Attributes.Build()
            );
    }
}
