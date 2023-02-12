using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class MethodBuilder : FieldOrMethodBuilder<MethodRecord>
    {
        internal MethodBuilder(AccessFlag accessFlags, string name, string descriptor, ClassBuilder declaringClass) :
            base(accessFlags, name, descriptor, declaringClass)
        {
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
