using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class MethodBuilder : FieldOrMethodBuilder<MethodRecord>
    {
        internal MethodBuilder(AccessFlag accessFlags, string name, string descriptor, ClassBuilder declaringClass) :
            base(accessFlags, name, descriptor, declaringClass)
        {
        }

        public MethodBuilder WithAttributes(Action<AttributesBuilder> builderAction)
        {
            Attributes ??= new AttributesBuilder(DeclaringClass);
            builderAction.Invoke(Attributes);
            return this;
        }

        public override MethodRecord Build() => new
            (
                AccessFlags: AccessFlags,
                NameIndex: NameIndex,
                DescriptorIndex: DescriptorIndex,
                Attributes: Attributes.Build()
            );
    }
}
