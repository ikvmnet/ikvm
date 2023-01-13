using System;
using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Represents the raw unlinked structure of a method.
    /// </summary>
    class MethodRecord : IClassParserAttributeHandler
    {

        readonly AccessFlag accessFlags;
        readonly ushort nameIndex;
        readonly ushort descriptorIndex;

        AttributeRecord[] attributes;
        int attributeIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="accessFlags"></param>
        /// <param name="nameIndex"></param>
        /// <param name="descriptorIndex"></param>
        public MethodRecord(AccessFlag accessFlags, ushort nameIndex, ushort descriptorIndex)
        {
            this.accessFlags = accessFlags;
            this.nameIndex = nameIndex;
            this.descriptorIndex = descriptorIndex;
        }

        /// <summary>
        /// Resolves the field.
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public Method Resolve(ClassRecord clazz)
        {
            var m = new Method();
            m.AccessFlags = accessFlags;
            m.Name = clazz.ResolveUtf8Constant(nameIndex);
            m.Descriptor = clazz.ResolveUtf8Constant(descriptorIndex);

            foreach (var i in attributes)
                m.Attributes.Add(i.Resolve(clazz));

            return m;
        }

        void IClassParserAttributeHandler.AcceptAttributeCount(int count)
        {
            attributes = new AttributeRecord[count];
            attributeIndex = 0;
        }

        void IClassParserAttributeHandler.AcceptAttribute(ushort nameIndex, in ReadOnlySequence<byte> info)
        {
            var r = new SequenceReader<byte>(info);
            if (r.TryReadExact((int)r.Length, out var buffer) == false)
                throw new ClassReaderException("Invalid buffer length on field attribute.");

            var b = new byte[buffer.Length];
            buffer.CopyTo(b);

            attributes[attributeIndex++] = new AttributeRecord(nameIndex, b);
        }

    }

}
