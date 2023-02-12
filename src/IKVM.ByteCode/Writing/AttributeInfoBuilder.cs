using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class AttributeInfoBuilder<TAttribute> : BuilderBase<AttributeInfoRecord>
        where TAttribute : AttributeRecord
    {
        private readonly AttributeBuilder<TAttribute> _attribute;

        public AttributeInfoBuilder(AttributeBuilder<TAttribute> attribute, ClassBuilder declaringClass) :
            base(declaringClass)
        {
            _attribute = attribute;

            NameIndex = declaringClass.AddUtf8(attribute.Name);
        }

        private ushort NameIndex { get; }

        public override AttributeInfoRecord Build()
        {
            var record = _attribute.Build();

            var buffer = new byte[record.GetSize()];
            var writer = new ClassFormatWriter(buffer);

            record.TryWrite(ref writer);

            return new
            (
                NameIndex: NameIndex,
                Data: buffer
            );
        }
    }
}
