using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class CodeAttributeWriter : WriterBase<CodeAttributeRecord>
    {
        public CodeAttributeWriter(ClassWriter declaringClass)
            : base(declaringClass)
        {
        }

        public ushort MaxStack { get; set; }

        public ushort MaxLocals { get; set; }

        public ReadOnlyMemory<byte> Code { get; set; }

        internal override CodeAttributeRecord Record => new
            (
                MaxStack: MaxStack,
                MaxLocals: MaxLocals,
                Code: Code,
                new ExceptionHandlerRecord[0],
                new AttributeInfoRecord[0]
            );
    }
}
