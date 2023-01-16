using System;

namespace IKVM.ByteCode
{

    public sealed class MethodInfo
    {

        readonly Class declaringClass;
        readonly MethodInfoRecord record;

        string name;
        string descriptor;
        AttributeDataCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        internal MethodInfo(Class declaringClass, MethodInfoRecord record)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.record = record;
        }

        /// <summary>
        /// Gets the access flags of the method.
        /// </summary>
        public AccessFlag AccessFlags => record.AccessFlags;

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string Name => Class.LazyGet(ref name, () => declaringClass.ResolveConstant<Utf8Constant>(record.NameIndex).Value);

        /// <summary>
        /// Gets the descriptor of the method.
        /// </summary>
        public string Descriptor => Class.LazyGet(ref descriptor, () => declaringClass.ResolveConstant<Utf8Constant>(record.DescriptorIndex).Value);

        /// <summary>
        /// Gets the attributes of the method.
        /// </summary>
        public AttributeDataCollection Attributes => Class.LazyGet(ref attributes, () => new AttributeDataCollection(declaringClass, record.Attributes));

    }

}