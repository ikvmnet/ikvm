using System;

namespace IKVM.ByteCode.Parser
{

    public static class FieldInfoParser
    {

        /// <summary>
        /// Parses the 'field_info' structure and returns the total length.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        public static int Parse(IFieldInfoListener visitor, ReadOnlySpan<byte> buffer)
        {
            var l = 0;

            ParseAccessFlags(visitor, ref buffer, ref l);
            ParseNameIndex(visitor, ref buffer, ref l);
            ParseDescriptorIndex(visitor, ref buffer, ref l);
            ParseAttributes(visitor, ref buffer, ref l, ParseAttributesCount(visitor, ref buffer, ref l));

            return l;
        }

        /// <summary>
        /// Parses the 'access_flags' field.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseAccessFlags(IFieldInfoListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            visitor.AcceptAccessFlags((AccessFlag)ClassFilePrimitives.ReadU2(ref buffer, ref length));
        }

        /// <summary>
        /// Parses the 'name_index' field.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseNameIndex(IFieldInfoListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            visitor.AcceptNameIndex(new ConstantPoolIndex(ClassFilePrimitives.ReadU2(ref buffer, ref length)));
        }

        /// <summary>
        /// Parses the 'descriptor_index' field.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseDescriptorIndex(IFieldInfoListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            visitor.AcceptDescriptorIndex(new ConstantPoolIndex(ClassFilePrimitives.ReadU2(ref buffer, ref length)));
        }

        /// <summary>
        /// Parses the 'attributes_count' field.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static ushort ParseAttributesCount(IFieldInfoListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
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
        static void ParseAttributes(IFieldInfoListener visitor, ref ReadOnlySpan<byte> buffer, ref int length, ushort count)
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
        static void ParseAttributesItem(IFieldInfoListener visitor, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            visitor.AcceptAttribute(ClassFilePrimitives.ReadU2(ref buffer, ref length));
        }

    }

}