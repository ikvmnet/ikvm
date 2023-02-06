using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKVM.ByteCode.Writing
{
    internal class AttributeWriterCollection
    {
        private readonly ClassWriter declaringClass;

        private CodeAttributeWriter codeAttribute;

        private InnerClassesAttributeWriter innerClassesAttribute;

        private ExceptionsAttributeWriter exceptionsAttribute;

        public AttributeWriterCollection(ClassWriter declaringClass)
        {
            this.declaringClass = declaringClass;
        }

        public DeprecatedAttributeWriter DeprecatedAttribute { get; set; }

        public SignatureAttributeWriter AddSignatureAttribute(string signature) =>
            new SignatureAttributeWriter(signature, declaringClass);

        public CodeAttributeWriter AddCodeAttribute() =>
            codeAttribute ??= new CodeAttributeWriter(this.declaringClass);

        public InnerClassesAttributeWriter AddInnerClassesAttribute() =>
            innerClassesAttribute ??= new InnerClassesAttributeWriter();

        public ExceptionsAttributeWriter AddExceptionsAttribute() =>
            exceptionsAttribute ??= new ExceptionsAttributeWriter();

        public RuntimeVisibleAnnotationsAttributeWriter AddRuntimeVisibleAnnotationsAttribute() =>
            new RuntimeVisibleAnnotationsAttributeWriter(declaringClass);
    }
}
