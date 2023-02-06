using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class InnerClassesAttributeBuilder : BuilderBase<InnerClassesAttributeRecord>
    {
        public InnerClassesAttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public void Add(string inner, string outer, string name, AccessFlag accessFlags)
        {
        }

        public override InnerClassesAttributeRecord Build()
        {
            throw new NotImplementedException();
        }
    }
}
