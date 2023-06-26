using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal abstract class ElementValueValueBuilder : BuilderBase<ElementValueRecord>
    {
        protected ElementValueValueBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }
    }
}
