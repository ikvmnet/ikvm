using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class DeprecatedAttributeBuilder : AttributeBuilder<DeprecatedAttributeRecord>
    {
        public DeprecatedAttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public override string Name => DeprecatedAttributeRecord.Name;

        public override DeprecatedAttributeRecord Build() =>
            new DeprecatedAttributeRecord();
    }
}
