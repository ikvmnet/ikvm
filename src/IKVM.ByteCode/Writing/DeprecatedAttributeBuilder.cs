using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class DeprecatedAttributeBuilder : BuilderBase<DeprecatedAttributeRecord>
    {
        public DeprecatedAttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public override DeprecatedAttributeRecord Build() =>
            new DeprecatedAttributeRecord();
    }
}
