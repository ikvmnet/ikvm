using IKVM.ByteCode.Parsing;
using System.Collections.Generic;

namespace IKVM.ByteCode.Writing
{
    internal class MethodParametersAttributeBuilder : AttributeBuilder<MethodParametersAttributeRecord>
    {
        private readonly List<MethodParametersAttributeParameterRecord> parameters = new();

        public MethodParametersAttributeBuilder(ClassBuilder declaringClass) :
            base(declaringClass)
        {
        }

        public override string Name => MethodParametersAttributeRecord.Name;

        public void Add(string name, AccessFlag accessFlag)
        {
            parameters.Add(new MethodParametersAttributeParameterRecord(NameIndex: DeclaringClass.AddUtf8(name), accessFlag));
        }

        public override MethodParametersAttributeRecord Build() => new
            (
                Parameters: parameters.ToArray()
            );
    }
}
