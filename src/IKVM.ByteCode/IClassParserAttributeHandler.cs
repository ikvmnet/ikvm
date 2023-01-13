using System.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Interfaces that accepts the parsing of attributes.
    /// </summary>
    public interface IClassParserAttributeHandler
    {

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters an attribute count.
        /// </summary>
        /// <param name="count"></param>
        void AcceptAttributeCount(int count);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters an attribute.
        /// </summary>
        /// <param name="nameIndex"></param>
        /// <param name="info"></param>
        void AcceptAttribute(ushort nameIndex, in ReadOnlySequence<byte> info);

    }

}
