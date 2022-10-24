using System;

namespace IKVM.ByteCode.Parser
{

    public static class MethodParser
    {

        /// <summary>
        /// Parses the 'method_info' structure and returns the total length.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        public static int Parse(IMethodListener visitor, ReadOnlySpan<byte> buffer)
        {
            var l = 0;

            ParseAccessFlags(visitor, ref buffer, ref l);
            ParseNameIndex(visitor, ref buffer, ref l);
            ParseDescriptorIndex(visitor, ref buffer, ref l);
            ParseAttributes(visitor, ref buffer, ref l, ParseAttributesCount(visitor, ref buffer, ref l));

            return l;
        }

        /// <summary>
        /// Parses the 'access_flags' Method.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseAccessFlags(IMethodListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            visitor.AcceptAccessFlags((AccessFlag)ClassFilePrimitives.ReadU2(ref buffer, ref length));
        }

        /// <summary>
        /// Parses the 'name_index' Method.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseNameIndex(IMethodListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            visitor.AcceptNameIndex(new ConstantPoolIndex(ClassFilePrimitives.ReadU2(ref buffer, ref length)));
        }

        /// <summary>
        /// Parses the 'descriptor_index' Method.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseDescriptorIndex(IMethodListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            visitor.AcceptDescriptorIndex(new ConstantPoolIndex(ClassFilePrimitives.ReadU2(ref buffer, ref length)));
        }

        /// <summary>
        /// Parses the 'attributes_count' Method.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static ushort ParseAttributesCount(IMethodListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var interfacesCount = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            visitor.AcceptAttributesCount(interfacesCount);
            return interfacesCount;
        }

        /// <summary>
        /// Parses the 'attributes' array.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="count"></param>
        static void ParseAttributes(IMethodListener visitor, ref ReadOnlySpan<byte> buffer, ref int length, ushort count)
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
        static void ParseAttributesItem(IMethodListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            visitor.AcceptAttribute(ClassFilePrimitives.ReadU2(ref buffer, ref length));
        }

    }

}