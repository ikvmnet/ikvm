using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class MethodReader
    {

        readonly ClassReader declaringClass;
        readonly MethodRecord record;

        string name;
        string descriptor;
        AttributeReaderCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        internal MethodReader(ClassReader declaringClass, MethodRecord record)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.record = record;
        }

        /// <summary>
        /// Gets the underlying record being read.
        /// </summary>
        public MethodRecord Record => record;

        /// <summary>
        /// Gets the access flags of the method.
        /// </summary>
        public AccessFlag AccessFlags => record.AccessFlags;

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string Name => ClassReader.LazyGet(ref name, () => declaringClass.ResolveConstant<Utf8ConstantReader>(record.NameIndex).Value);

        /// <summary>
        /// Gets the descriptor of the method.
        /// </summary>
        public string Descriptor => ClassReader.LazyGet(ref descriptor, () => declaringClass.ResolveConstant<Utf8ConstantReader>(record.DescriptorIndex).Value);

        /// <summary>
        /// Gets the attributes of the method.
        /// </summary>
        public AttributeReaderCollection Attributes => ClassReader.LazyGet(ref attributes, () => new AttributeReaderCollection(declaringClass, record.Attributes));

    }

}