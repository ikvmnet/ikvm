using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class InterfaceBuilder : BuilderBase<InterfaceRecord>
    {
        public InterfaceBuilder(string name, ClassBuilder declaringClass) :
            base(declaringClass)
        {
            ClassIndex = declaringClass.AddClass(name);
        }

        private ushort ClassIndex { get; }

        public override InterfaceRecord Build() => new
            (
                ClassIndex: ClassIndex
            );
    }
}
