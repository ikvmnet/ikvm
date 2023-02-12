using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class ConstantValueAttributeBuilder : AttributeBuilder<ConstantValueAttributeRecord>
    {
        public ConstantValueAttributeBuilder(int value, ClassBuilder declaringClass)
            : this(declaringClass.AddConstant(value), declaringClass)
        {
        }

        public ConstantValueAttributeBuilder(long value, ClassBuilder declaringClass)
            : this(declaringClass.AddConstant(value), declaringClass)
        {
        }

        public ConstantValueAttributeBuilder(float value, ClassBuilder declaringClass)
            : this(declaringClass.AddConstant(value), declaringClass)
        {
        }

        public ConstantValueAttributeBuilder(double value, ClassBuilder declaringClass)
            : this(declaringClass.AddConstant(value), declaringClass)
        {
        }

        public ConstantValueAttributeBuilder(string value, ClassBuilder declaringClass)
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
