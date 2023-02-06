using IKVM.ByteCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKVM.ByteCode.Writing
{
    internal class FieldWriter : FieldOrMethodWriter<FieldRecord>
    {
        public FieldWriter(ClassWriter declaringClass) :
            base(declaringClass)
        {
        }

        internal override FieldRecord Record => throw new NotImplementedException();
    }
}
