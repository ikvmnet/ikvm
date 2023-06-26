using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class AnnotationDefaultAttributeBuilder : AttributeBuilder<AnnotationDefaultAttributeRecord>
    {
        private readonly ElementValueValueBuilder _value;

        public AnnotationDefaultAttributeBuilder(ElementValueValueBuilder value, ClassBuilder declaringClass)
            : base(declaringClass)
        {
            _value = value;
        }

        public override string Name => AnnotationDefaultAttributeRecord.Name;

        public override AnnotationDefaultAttributeRecord Build() => new
            (
                DefaultValue: _value.Build()
            );
    }
}
