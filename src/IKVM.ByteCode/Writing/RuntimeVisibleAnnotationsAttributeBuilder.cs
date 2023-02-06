using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class RuntimeVisibleAnnotationsAttributeBuilder : BuilderBase<RuntimeVisibleAnnotationsAttributeRecord>
    {
        public RuntimeVisibleAnnotationsAttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public override RuntimeVisibleAnnotationsAttributeRecord Build() =>
            new(new AnnotationRecord[0]);
    }
}
