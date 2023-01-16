using System;

namespace IKVM.ByteCode
{

    public sealed class AttributeInfo
    {

        readonly Class clazz;
        readonly AttributeInfoRecord record;

        string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        internal AttributeInfo(Class declaringClass, AttributeInfoRecord record)
        {
            this.clazz = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.record = record;
        }

        /// <summary>
        /// Gets the name of the attribute.
        /// </summary>
        public string Name => name ??= clazz.ResolveConstant<Utf8Constant>(record.NameIndex).Value;

        /// <summary>
        /// Gets the data of the attribute.
        /// </summary>
        public ReadOnlyMemory<byte> Data => record.Data;

    }

}
