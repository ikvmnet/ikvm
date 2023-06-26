using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class ElementValueAnnotationBuilder : ElementValueValueBuilder
    {
        public ElementValueAnnotationBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public override ElementValueRecord Build() => throw new NotImplementedException();
    }
}
