using System;

namespace IKVM.ByteCode.Parser
{

    /// <summary>
    /// Provides the capability of reading a Java class file.
    /// </summary>
    public static class ClassFileParser
    {

        /// <summary>
        /// Parses the class file structure and returns the total number of bytes processsed.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static int Parse(IClassListener listener, ReadOnlySpan<byte> buffer)
        {
            var l = 0;

            ParseMagic(listener, ref buffer, ref l);
            ParseMinorVersion(listener, ref buffer, ref l);
            ParseMajorVersion(listener, ref buffer, ref l);
            ParseConstantPool(listener, ref buffer, ref l, ParseConstantPoolCount(listener, ref buffer, ref l));
            ParseAccessFlags(listener, ref buffer, ref l);
            ParseThisClass(listener, ref buffer, ref l);
            ParseSuperClass(listener, ref buffer, ref l);
            ParseInterfaces(listener, ref buffer, ref l, ParseInterfacesCount(listener, ref buffer, ref l));
            ParseFields(listener, ref buffer, ref l, ParseFieldsCount(listener, ref buffer, ref l));
            ParseMethods(listener, ref buffer, ref l, ParseMethodsCount(listener, ref buffer, ref l));
            ParseAttributes(listener, ref buffer, ref l, ParseAttributesCount(listener, ref buffer, ref l));

            return l;
        }

        /// <summary>
        /// Parses the 'magic' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseMagic(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            listener.SetMagic(ClassFilePrimitives.ReadU4(ref buffer, ref length));
        }

        /// <summary>
        /// Parses the 'minor_version' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseMinorVersion(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            listener.SetMinorVersion(ClassFilePrimitives.ReadU2(ref buffer, ref length));
        }

        /// <summary>
        /// Parses the 'major_version' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseMajorVersion(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            listener.SetMajorVersion(ClassFilePrimitives.ReadU2(ref buffer, ref length));
        }

        /// <summary>
        /// Parses the 'constant_pool_count' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static ushort ParseConstantPoolCount(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var constantPoolCount = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            listener.SetConstantCount(constantPoolCount);
            return constantPoolCount;
        }

        /// <summary>
        /// Parses the 'constant_pool' array.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="count"></param>
        static void ParseConstantPool(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length, ushort count)
        {
            for (ushort i = 0; i < count; i++)
                ParseConstantPoolItem(listener, ref buffer, ref length, i);
        }

        /// <summary>
        /// Parses an 'constant_pool' array item.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseConstantPoolItem(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length, ushort index)
        {
            var l = ConstantPoolInfoParser.Parse(listener.AddConstant(new ConstantPoolIndex(index)), buffer);
            buffer = buffer.Slice(l);
            length += l;
        }

        /// <summary>
        /// Parses the 'access_flags' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseAccessFlags(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var accessFlags = (AccessFlag)ClassFilePrimitives.ReadU2(ref buffer, ref length);
            listener.SetAccessFlags(accessFlags);
        }

        /// <summary>
        /// Parses the 'this_class' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseThisClass(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var thisClass = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            listener.SetThisClass(new ConstantPoolIndex(thisClass));
        }

        /// <summary>
        /// Parses the 'super_class' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseSuperClass(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var superClass = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            listener.SetSuperClass(new ConstantPoolIndex(superClass));
        }

        /// <summary>
        /// Parses the 'interfaces_count' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static ushort ParseInterfacesCount(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var interfacesCount = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            listener.SetInterfacesCount(interfacesCount);
            return interfacesCount;
        }

        /// <summary>
        /// Parses the 'interfaces' array.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="count"></param>
        static void ParseInterfaces(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length, ushort count)
        {
            for (var i = 0; i < count; i++)
                ParseInterfacesItem(listener, ref buffer, ref length);
        }

        /// <summary>
        /// Parses an 'interfaces' array item.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseInterfacesItem(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var index = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            listener.AddInterface(index);
        }

        /// <summary>
        /// Parses the 'fields_count' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static ushort ParseFieldsCount(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var fieldsCount = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            listener.SetInterfacesCount(fieldsCount);
            return fieldsCount;
        }

        /// <summary>
        /// Parses the 'fields' array.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="count"></param>
        static void ParseFields(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length, ushort count)
        {
            for (var i = 0; i < count; i++)
                ParseFieldsItem(listener, ref buffer, ref length);
        }

        /// <summary>
        /// Parses a 'fields' array item.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseFieldsItem(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var l = FieldInfoParser.Parse(listener.AddField(), buffer);
            buffer = buffer.Slice(l);
            length += l;
        }

        /// <summary>
        /// Parses the 'methods_count' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static ushort ParseMethodsCount(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var methodsCount = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            listener.SetMethodsCount(methodsCount);
            return methodsCount;
        }

        /// <summary>
        /// Parses the 'methods' array.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="count"></param>
        static void ParseMethods(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length, ushort count)
        {
            for (var i = 0; i < count; i++)
                ParseMethodsItem(listener, ref buffer, ref length);
        }

        /// <summary>
        /// Parses a 'methods' array item.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseMethodsItem(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var l = MethodParser.Parse(listener.AddMethod(), buffer);
            buffer = buffer.Slice(l);
            length += l;
        }

        /// <summary>
        /// Parses the 'attributes_count' field.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static ushort ParseAttributesCount(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var attributesCount = ClassFilePrimitives.ReadU2(ref buffer, ref length);
            listener.SetAttributesCount(attributesCount);
            return attributesCount;
        }

        /// <summary>
        /// Parses the 'attributes' array.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <param name="count"></param>
        static void ParseAttributes(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length, ushort count)
        {
            for (var i = 0; i < count; i++)
                ParseAttributesItem(listener, ref buffer, ref length);
        }

        /// <summary>
        /// Parses an 'attributes' array item.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        static void ParseAttributesItem(IClassListener listener, ref ReadOnlySpan<byte> buffer, ref int length)
        {
            var l = AttributeParser.Parse(listener.AddAttribute(), buffer);
            buffer = buffer.Slice(l);
            length += l;
        }

    }

}
