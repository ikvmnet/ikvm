using IKVM.ByteCode.Parsing;
using System;

namespace IKVM.ByteCode.Writing
{
    internal class ExceptionsAttributeBuilder : BuilderBase<ExceptionsAttributeRecord>
    {
        public ExceptionsAttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public override ExceptionsAttributeRecord Build()
        {
            throw new NotImplementedException();
        }

        internal void Add(string exceptionClass)
        {
            //classes.Add(classFile.AddClass(exceptionClass));
        }
    }
}
