using System;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Represents the raw unlinked structure of an attribute.
    /// </summary>
    class AttributeRecord
    {

        readonly ushort nameIndex;
        readonly ReadOnlyMemory<byte> info;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="nameIndex"></param>
        /// <param name="info"></param>
        public AttributeRecord(ushort nameIndex, ReadOnlyMemory<byte> info)
        {
            this.nameIndex = nameIndex;
            this.info = info;
        }

        /// <summary>
        /// Resolves the attribute.
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public Attribute Resolve(ClassRecord clazz)
        {
            var a = new Attribute();
            a.Name = clazz.ResolveUtf8Constant(nameIndex);
            a.Info = info;
            return a;
        }

    }

}
