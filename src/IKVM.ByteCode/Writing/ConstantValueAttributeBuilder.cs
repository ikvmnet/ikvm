using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class ConstantValueAttributeBuilder : AttributeBuilder<ConstantValueAttributeRecord>
    {
        public ConstantValueAttributeBuilder(object value, ClassBuilder declaringClass)
            : this(declaringClass.AddConstant(value), declaringClass)
        {
        }

        private ConstantValueAttributeBuilder(ushort valueIndex, ClassBuilder declaringClass)
            : base(declaringClass)
        {
            ValueIndex = valueIndex;
        }

        public override string Name => ConstantValueAttributeRecord.Name;

        private ushort ValueIndex { get; }

        public override ConstantValueAttributeRecord Build() => new
            (
                ValueIndex: ValueIndex
            );
    }
}
