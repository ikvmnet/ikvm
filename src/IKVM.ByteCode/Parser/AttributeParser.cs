using System;

namespace IKVM.ByteCode.Parser
{

    public static class AttributeParser
    {

        /// <summary>
        /// Parses the 'attribute_info' structure and returns the total length.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        public static int Parse(IAttributeListener visitor, ReadOnlySpan<byte> buffer)
        {
            var l = 0;

            ParseNameIndex(visitor, ref buffer, ref l);
            ParseAttribute(visitor, ref buffer, ref l, ParseAttributeLength(visitor, ref buffer, ref l));

            return l;
        }

        /// <summary>
        /// Parses the 'attribute_length' Method.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static ushort ParseAttributeLength(IAttributeListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var interfacesCount = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            visitor.AcceptAttributeLength(interfacesCount);
            return interfacesCount;
        }

        /// <summary>
        /// Parses the 'attributes' array.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="count"></param>
        static void ParseAttributes(IAttributeListener visitor, ref ReadOnlySpan<byte> buffer, ref int length, ushort count)
        {
            for (var i = 0; i < count; i++)
                ParseAttributesItem(visitor, ref buffer, ref length);
        }

        /// <summary>
        /// Parses an 'attributes' array item.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseAttributesItem(IAttributeListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            visitor.AcceptAttribute(ClassFilePrimitives.ReadU2(ref buffer, ref length));
        }

    }

}