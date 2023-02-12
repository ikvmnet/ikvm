using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class RuntimeVisibleAnnotationsAttributeBuilder : AttributeBuilder<RuntimeVisibleAnnotationsAttributeRecord>
    {
        public RuntimeVisibleAnnotationsAttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
            Annotations = new AnnotationsBuilder(declaringClass);
        }

        public override string Name => RuntimeVisibleAnnotationsAttributeRecord.Name;

        public AnnotationsBuilder Annotations { get; }

        public override RuntimeVisibleAnnotationsAttributeRecord Build() =>
            new(Annotations.Build());
    }
}
