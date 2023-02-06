using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class CodeAttributeBuilder : BuilderBase<CodeAttributeRecord>
    {
        public CodeAttributeBuilder(ushort maxStack, ushort maxLocals, ReadOnlyMemory<byte> code, ClassBuilder declaringClass)
            : base(declaringClass)
        {
            MaxStack = maxStack;
            MaxLocals = maxLocals;
            Code = code;

            Attributes = new AttributesBuilder(declaringClass);
        }

        private ushort MaxStack { get; }

        private ushort MaxLocals { get; }

        private ReadOnlyMemory<byte> Code { get; }

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
