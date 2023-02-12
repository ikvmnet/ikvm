using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class AnnotationDefaultAttributeBuilder : AttributeBuilder<AnnotationDefaultAttributeRecord>
    {
        public AnnotationDefaultAttributeBuilder(ClassBuilder declaringClass)
            : base(declaringClass)
        {
        }

        public override string Name => AnnotationDefaultAttributeRecord.Name;

        public override AnnotationDefaultAttributeRecord Build() => throw new NotImplementedException();
    }
}
