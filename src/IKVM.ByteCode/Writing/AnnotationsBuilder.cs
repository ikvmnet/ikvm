using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class AnnotationsBuilder : RecordsBuilder<AnnotationRecord>
    {
        public AnnotationsBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public override AnnotationRecord[] Build()
        {
            throw new NotImplementedException();
        }
    }
}
