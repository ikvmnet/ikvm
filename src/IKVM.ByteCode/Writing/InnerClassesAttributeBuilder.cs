using IKVM.ByteCode.Parsing;
using System.Collections.Generic;

namespace IKVM.ByteCode.Writing
{
    internal class InnerClassesAttributeBuilder : AttributeBuilder<InnerClassesAttributeRecord>
    {
        private readonly List<InnerClassesAttributeItemRecord> items = new();

        public InnerClassesAttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public override string Name => InnerClassesAttributeRecord.Name;

        public void Add(string inner, string outer, string name, AccessFlag accessFlags)
        {
            items.Add(new InnerClassesAttributeItemRecord(
                InnerClassIndex: DeclaringClass.AddClass(inner),
                OuterClassIndex: DeclaringClass.AddClass(outer),
                InnerNameIndex: DeclaringClass.AddUtf8(name),
                InnerClassAccessFlags: accessFlags
            ));
        }

        public override InnerClassesAttributeRecord Build() => new
            (
                Items: items.ToArray()
            );
    }
}
