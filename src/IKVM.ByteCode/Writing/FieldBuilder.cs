using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class FieldBuilder : FieldOrMethodBuilder<FieldRecord>
    {
        internal FieldBuilder(AccessFlag accessFlags, string name, string descriptor, ClassBuilder declaringClass) :
            base(accessFlags, name, descriptor, declaringClass)
        {
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
