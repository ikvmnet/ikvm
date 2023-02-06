using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class RuntimeVisibleAnnotationsAttributeWriter : WriterBase<RuntimeVisibleAnnotationsAttributeRecord>
    {
        public RuntimeVisibleAnnotationsAttributeWriter(ClassWriter declaringClass) :
            base(declaringClass)
        {
        }

        internal override RuntimeVisibleAnnotationsAttributeRecord Record =>
            new(new AnnotationRecord[0]);
    }
}
