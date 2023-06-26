using IKVM.ByteCode.Parsing;
using System.Collections.Generic;

namespace IKVM.ByteCode.Writing
{
    internal class ExceptionsAttributeBuilder : AttributeBuilder<ExceptionsAttributeRecord>
    {
        private readonly List<ushort> exceptionsIndexes = new();

        public ExceptionsAttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public override string Name => ExceptionsAttributeRecord.Name;

        internal void Add(string exceptionClass)
        {
            exceptionsIndexes.Add(DeclaringClass.AddClass(exceptionClass));
        }

        public override ExceptionsAttributeRecord Build() => new
            (
                ExceptionsIndexes: exceptionsIndexes.ToArray()
            );
    }
}
