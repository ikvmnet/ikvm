using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Writing
{
    internal class InterfaceWriter : WriterBase<InterfaceRecord>
    {
        private readonly InterfaceRecord _record;

        public InterfaceWriter(string name, ClassWriter declaringClass) :
            base(declaringClass)
        {
            _record = new InterfaceRecord(declaringClass.AddClass(name));
        }

        internal override InterfaceRecord Record => _record;
    }
}
