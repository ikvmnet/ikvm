using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class FieldBuilder : FieldOrMethodBuilder<FieldRecord>
    {
        internal FieldBuilder(AccessFlag accessFlags, string name, string descriptor, ClassBuilder declaringClass) :
            base(accessFlags, name, descriptor, declaringClass)
        {
        }

        public FieldBuilder WithAttributes(Action<AttributesBuilder> builderAction)
        {
            Attributes ??= new AttributesBuilder(DeclaringClass);
            builderAction(Attributes);
            return this;
        }

        public override FieldRecord Build() => new
            (
                AccessFlags: AccessFlags,
                NameIndex: NameIndex,
                DescriptorIndex: DescriptorIndex,
                Attributes: Attributes.Build()
            );
    }
}
