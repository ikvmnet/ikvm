using System;
using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// A parser to allow a <see cref="Classparser"/> to Parse a ClassFile structure, as defined in the Java Virtual
    /// Machine Specification(JVMS). This class parses the ClassFile content and calls the appropriate Parse methods
    /// of a given <see cref="Classparser"/> for each field, method and bytecode instruction encountered.
    /// </summary>
    /// <remarks>
    /// The structure of a Java Class File is described by the Java Virtual Machine specification.
    /// 
    /// ClassFile {
    ///     u4 magic;
    ///     u2 minor_version;
    ///     u2 major_version;
    ///     u2 constant_pool_count;
    ///     cp_info constant_pool[constant_pool_count - 1];
    ///     u2 access_flags;
    ///     u2 this_class;
    ///     u2 super_class;
    ///     u2 interfaces_count;
    ///     u2 interfaces[interfaces_count];
    ///     u2 fields_count;
    ///     field_info fields[fields_count];
    ///     u2 methods_count;
    ///     method_info methods[methods_count];
    ///     u2 attributes_count;
    ///     attribute_info attributes[attributes_count];
    /// }
    /// 
    /// </remarks>
    public readonly ref struct ClassParser
    {

        const int MAX_STACK_BUFFER_SIZE = 128;
        const uint MAGIC = 0xCAFEBABE;

        readonly ReadOnlySequence<byte> buffer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="buffer"></param>
        public ClassParser(ReadOnlyMemory<byte> buffer) :
            this(new ReadOnlySequence<byte>(buffer))
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="buffer"></param>
        public ClassParser(ReadOnlySequence<byte> buffer)
        {
            this.buffer = buffer;
        }

        /// <summary>
        /// Parsers the class file given by the initialized buffer and invokes the appropriate method on the
        /// <see cref="IClassParserHandler"/> for each element.
        /// </summary>
        /// <param name="parser"></param>
        /// <exception cref="ClassParserException"></exception>
        /// <exception cref="UnsupportedClassVersionException"></exception>
        public void Parse(IClassParserHandler parser)
        {
            var reader = new SequenceReader<byte>(buffer);

            if (reader.TryReadBigEndian(out uint magic) == false)
                throw new ClassParserException(reader);
            if (magic != MAGIC)
                throw new ClassParserException(reader, $"Unexpected magic value '{magic}'.");

            if (reader.TryReadBigEndian(out ushort minorVersion) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort majorVersion) == false)
                throw new ClassParserException(reader);

            if (majorVersion > 63)
                throw new UnsupportedClassVersionException(reader, majorVersion, minorVersion);

            parser.AcceptVersion(majorVersion, minorVersion);

            if (reader.TryReadBigEndian(out ushort constantCount) == false)
                throw new ClassParserException(reader);

            parser.AcceptConstantCount(constantCount);

            for (int constantPoolIndex = 1; constantPoolIndex < constantCount - 1; constantPoolIndex++)
                ParseConstant(parser, ref reader, constantPoolIndex);

            if (reader.TryReadBigEndian(out ushort accessFlags) == false)
                throw new ClassParserException(reader);

            parser.AcceptAccessFlags((AccessFlag)accessFlags);

            if (reader.TryReadBigEndian(out ushort thisClass) == false)
                throw new ClassParserException(reader);

            parser.AcceptThisClass(thisClass);

            if (reader.TryReadBigEndian(out ushort superClass) == false)
                throw new ClassParserException(reader);

            parser.AcceptSuperClass(superClass);

            if (reader.TryReadBigEndian(out ushort interfaceCount) == false)
                throw new ClassParserException(reader);

            parser.AcceptInterfaceCount(interfaceCount);

            for (int interfaceIndex = 0; interfaceIndex < interfaceCount; interfaceIndex++)
                ParseInterface(parser, ref reader, interfaceIndex);

            if (reader.TryReadBigEndian(out ushort fieldCount) == false)
                throw new ClassParserException(reader);

            parser.AcceptFieldCount(fieldCount);

            for (int fieldIndex = 0; fieldIndex < fieldCount; fieldIndex++)
                ParseField(parser, ref reader, fieldIndex);

            if (reader.TryReadBigEndian(out ushort methodCount) == false)
                throw new ClassParserException(reader);

            parser.AcceptMethodCount(methodCount);

            for (int methodIndex = 0; methodIndex < methodCount; methodIndex++)
                ParseMethod(parser, ref reader, methodIndex);

            if (reader.TryReadBigEndian(out ushort attributeCount) == false)
                throw new ClassParserException(reader);

            parser.AcceptAttributeCount(attributeCount);

            for (int attributeIndex = 0; attributeIndex < attributeCount; attributeIndex++)
                ParseAttribute(parser, ref reader, attributeIndex);
        }

        /// <summary>
        /// Parses the reader starting at the beginning of the constnat pool (cp_info) structure.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="constantPoolIndex"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int constantPoolIndex)
        {
            if (reader.TryRead(out byte tag) == false)
                throw new ClassParserException(reader);

            switch ((ConstantTag)tag)
            {
                case ConstantTag.Utf8:
                    ParseUtf8Constant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Integer:
                    ParseIntegerConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Float:
                    ParseFloatConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Long:
                    ParseLongConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Double:
                    ParseDoubleConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Class:
                    ParseClassConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.String:
                    ParseStringConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Fieldref:
                    ParseFieldrefConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Methodref:
                    ParseMethodrefConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.InterfaceMethodref:
                    ParseInterfaceMethodrefConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.NameAndType:
                    ParseNameAndTypeConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.MethodHandle:
                    ParseMethodHandleConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.MethodType:
                    ParseMethodTypeConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Dynamic:
                    ParseDynamicConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.InvokeDynamic:
                    ParseInvokeDynamicConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Module:
                    ParseModuleConstant(parser, ref reader, constantPoolIndex);
                    break;
                case ConstantTag.Package:
                    ParsePackageConstant(parser, ref reader, constantPoolIndex);
                    break;
                default:
                    throw new ClassParserException(reader, "Encountered an unknown constant tag.");
            }
        }

        /// <summary>
        /// Parses a UTF8 constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseUtf8Constant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort length) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadExact(length, out ReadOnlySequence<byte> value) == false)
                throw new ClassParserException(reader);

            parser.AcceptUtf8Constant(index, value);
        }

        /// <summary>
        /// Parses a Integer constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseIntegerConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out int value) == false)
                throw new ClassParserException(reader);

            parser.AcceptIntegerConstant(index, value);
        }

        /// <summary>
        /// Parses a Float constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseFloatConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out float value) == false)
                throw new ClassParserException(reader);

            parser.AcceptFloatConstant(index, value);
        }

        /// <summary>
        /// Parses a Long constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseLongConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out long value) == false)
                throw new ClassParserException(reader);

            parser.AcceptLongConstant(index, value);
        }

        /// <summary>
        /// Parses a Double constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseDoubleConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out double value) == false)
                throw new ClassParserException(reader);

            parser.AcceptDoubleConstant(index, value);
        }

        /// <summary>
        /// Parses a Class constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseClassConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort value) == false)
                throw new ClassParserException(reader);

            parser.AcceptClassConstant(index, value);
        }

        /// <summary>
        /// Parses a Class constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseStringConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort value) == false)
                throw new ClassParserException(reader);

            parser.AcceptStringConstant(index, value);
        }

        /// <summary>
        /// Parses a Fieldref constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseFieldrefConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptFieldrefConstant(index, classIndex, nameAndTypeIndex);
        }

        /// <summary>
        /// Parses a Methodref constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseMethodrefConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptMethodrefConstant(index, classIndex, nameAndTypeIndex);
        }

        /// <summary>
        /// Parses a InterfaceMethodref constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseInterfaceMethodrefConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptInterfaceMethodrefConstant(index, classIndex, nameAndTypeIndex);
        }

        /// <summary>
        /// Parses a NameAndType constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseNameAndTypeConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptNameAndTypeConstant(index, nameIndex, descriptorIndex);
        }

        /// <summary>
        /// Parses a MethodHandle constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseMethodHandleConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryRead(out ReferenceKind referenceKind) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort referenceIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptMethodHandleConstant(index, referenceKind, referenceIndex);
        }

        /// <summary>
        /// Parses a MethodType constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseMethodTypeConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptMethodTypeConstant(index, descriptorIndex);
        }

        /// <summary>
        /// Parses a Dynamic constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseDynamicConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort bootstrapMethodAttrIndex) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptDynamicConstant(index, bootstrapMethodAttrIndex, nameAndTypeIndex);
        }

        /// <summary>
        /// Parses a InvokeDynamic constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseInvokeDynamicConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort bootstrapMethodAttrIndex) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptInvokeDynamicConstant(index, bootstrapMethodAttrIndex, nameAndTypeIndex);
        }

        /// <summary>
        /// Parses a Module constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseModuleConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptModuleConstant(index, nameIndex);
        }

        /// <summary>
        /// Parses a Package constant in the constant pool.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParsePackageConstant(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptPackageConstant(index, nameIndex);
        }

        /// <summary>
        /// Parses an interface.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        void ParseInterface(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptInterface(classIndex);
        }

        /// <summary>
        /// Parses a field.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        void ParseField(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort accessFlags) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptField((AccessFlag)accessFlags, nameIndex, descriptorIndex, out var attributeHandler);
            ParseAttributes(attributeHandler, ref reader);
        }

        /// <summary>
        /// Parses a method.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        void ParseMethod(IClassParserHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort accessFlags) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                throw new ClassParserException(reader);

            parser.AcceptMethod((AccessFlag)accessFlags, nameIndex, descriptorIndex, out var attributeHandler);
            ParseAttributes(attributeHandler, ref reader);
        }

        /// <summary>
        /// Parses an attributes count followed by a sequence of attributes.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <exception cref="ClassParserException"></exception>
        void ParseAttributes(IClassParserAttributeHandler parser, ref SequenceReader<byte> reader)
        {
            if (reader.TryReadBigEndian(out ushort count) == false)
                throw new ClassParserException(reader);

            parser.AcceptAttributeCount(count);

            for (int i = 0; i < count; i++)
                ParseAttribute(parser, ref reader, i);
        }

        /// <summary>
        /// Parses an attribute.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        unsafe void ParseAttribute(IClassParserAttributeHandler parser, ref SequenceReader<byte> reader, int index)
        {
            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadBigEndian(out uint length) == false)
                throw new ClassParserException(reader);
            if (reader.TryReadExact((int)length, out var info) == false)
                throw new ClassParserException(reader);

            parser.AcceptAttribute(nameIndex, info);
        }

    }

}
