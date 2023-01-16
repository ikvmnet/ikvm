using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// A parser to allow a <see cref="ClassRecordReader"/> to Parse a ClassFile structure, as defined in the Java Virtual
    /// Machine Specification(JVMS). This class parses the ClassFile content and calls the appropriate Parse methods
    /// of a given <see cref="ClassRecordReader"/> for each field, method and bytecode instruction encountered.
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
    static class ClassRecordReader
    {

        const uint MAGIC = 0xCAFEBABE;

        /// <summary>
        /// Attempts to read a class record starting at the current position.
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        /// <exception cref="ClassReaderException"></exception>
        public static bool TryReadClass(ref SequenceReader<byte> reader, out ClassRecord clazz)
        {
            clazz = default;

            if (reader.TryReadBigEndian(out uint magic) == false)
                return false;
            if (magic != MAGIC)
                throw new ClassReaderException($"Unexpected magic value '{magic}'.");

            if (reader.TryReadBigEndian(out ushort minorVersion) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort majorVersion) == false)
                return false;

            if (majorVersion > 63)
                throw new UnsupportedClassVersionException(majorVersion, minorVersion);

            if (TryReadConstants(ref reader, out var constants) == false)
                return false;

            if (reader.TryReadBigEndian(out ushort accessFlags) == false)
                return false;

            if (reader.TryReadBigEndian(out ushort thisClass) == false)
                return false;

            if (reader.TryReadBigEndian(out ushort superClass) == false)
                return false;

            if (TryReadInterfaces(ref reader, out var interfaces) == false)
                return false;

            if (TryReadFields(ref reader, out var fields) == false)
                return false;

            if (TryReadMethods(ref reader, out var methods) == false)
                return false;

            if (TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            clazz = new ClassRecord(minorVersion, majorVersion, constants, (AccessFlag)accessFlags, thisClass, superClass, interfaces, fields, methods, attributes);
            return true;
        }

        /// <summary>
        /// Attempts to read the set of constants at the current position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constants"></param>
        /// <returns></returns>
        static bool TryReadConstants(ref SequenceReader<byte> reader, out ConstantRecord[] constants)
        {
            constants = null;

            if (reader.TryReadBigEndian(out ushort constantCount) == false)
                return false;

            constants = new ConstantRecord[constantCount];
            for (int constantPoolIndex = 1; constantPoolIndex < constantCount; constantPoolIndex++)
            {
                if (TryReadConstant(ref reader, out ConstantRecord constant) == false)
                    return false;

                constants[constantPoolIndex] = constant;
            }

            return true;
        }

        /// <summary>
        /// Attempts to read the constant at the current position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        static bool TryReadConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryRead(out byte tag) == false)
                return false;

            switch ((ConstantTag)tag)
            {
                case ConstantTag.Utf8:
                    return TryReadUtf8Constant(ref reader, out constant);
                case ConstantTag.Integer:
                    return TryReadIntegerConstant(ref reader, out constant);
                case ConstantTag.Float:
                    return TryReadFloatConstant(ref reader, out constant);
                case ConstantTag.Long:
                    return TryReadLongConstant(ref reader, out constant);
                case ConstantTag.Double:
                    return TryReadDoubleConstant(ref reader, out constant);
                case ConstantTag.Class:
                    return TryReadClassConstant(ref reader, out constant);
                case ConstantTag.String:
                    return TryReadStringConstant(ref reader, out constant);
                case ConstantTag.Fieldref:
                    return TryReadFieldrefConstant(ref reader, out constant);
                case ConstantTag.Methodref:
                    return TryReadMethodrefConstant(ref reader, out constant);
                case ConstantTag.InterfaceMethodref:
                    return TryReadInterfaceMethodrefConstant(ref reader, out constant);
                case ConstantTag.NameAndType:
                    return TryReadNameAndTypeConstant(ref reader, out constant);
                case ConstantTag.MethodHandle:
                    return TryReadMethodHandleConstant(ref reader, out constant);
                case ConstantTag.MethodType:
                    return TryReadMethodTypeConstant(ref reader, out constant);
                case ConstantTag.Dynamic:
                    return TryReadDynamicConstant(ref reader, out constant);
                case ConstantTag.InvokeDynamic:
                    return TryReadInvokeDynamicConstant(ref reader, out constant);
                case ConstantTag.Module:
                    return TryReadModuleConstant(ref reader, out constant);
                case ConstantTag.Package:
                    return TryReadPackageConstant(ref reader, out constant);
                default:
                    throw new ClassReaderException("Encountered an unknown constant tag.");
            }
        }

        /// <summary>
        /// Parses a UTF8 constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadUtf8Constant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort length) == false)
                return false;
            if (reader.TryReadExact(length, out ReadOnlySequence<byte> value) == false)
                return false;

            var valueBuffer = new byte[value.Length];
            value.CopyTo(valueBuffer);

            constant = new Utf8ConstantRecord(valueBuffer);
            return true;
        }

        /// <summary>
        /// Parses a Integer constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadIntegerConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out int value) == false)
                return false;

            constant = new IntegerConstantRecord(value);
            return true;
        }

        /// <summary>
        /// Parses a Float constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadFloatConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out float value) == false)
                return false;

            constant = new FloatConstantRecord(value);
            return true;
        }

        /// <summary>
        /// Parses a Long constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadLongConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out long value) == false)
                return false;

            constant = new LongConstantRecord(value);
            return true;
        }

        /// <summary>
        /// Parses a Double constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadDoubleConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out double value) == false)
                return false;

            constant = new DoubleConstantRecord(value);
            return true;
        }

        /// <summary>
        /// Parses a Class constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadClassConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;

            constant = new ClassConstantRecord(nameIndex);
            return true;
        }

        /// <summary>
        /// Parses a Class constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadStringConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;

            constant = new StringConstantRecord(nameIndex);
            return true;
        }

        /// <summary>
        /// Parses a Fieldref constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadFieldrefConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new FieldrefConstantRecord(classIndex, nameAndTypeIndex);
            return true;
        }

        /// <summary>
        /// Parses a Methodref constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadMethodrefConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new MethodrefConstantRecord(classIndex, nameAndTypeIndex);
            return true;
        }

        /// <summary>
        /// Parses a InterfaceMethodref constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadInterfaceMethodrefConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new InterfaceMethodrefConstantRecord(classIndex, nameAndTypeIndex);
            return true;
        }

        /// <summary>
        /// Parses a NameAndType constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadNameAndTypeConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                return false;

            constant = new NameAndTypeConstantRecord(nameIndex, descriptorIndex);
            return true;
        }

        /// <summary>
        /// Parses a MethodHandle constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadMethodHandleConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryRead(out ReferenceKind referenceKind) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort referenceIndex) == false)
                return false;

            constant = new MethodHandleConstantRecord(referenceKind, referenceIndex);
            return true;
        }

        /// <summary>
        /// Parses a MethodType constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadMethodTypeConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                return false;

            constant = new MethodTypeConstantRecord(descriptorIndex);
            return true;
        }

        /// <summary>
        /// Parses a Dynamic constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadDynamicConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort bootstrapMethodAttrIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new DynamicConstantRecord(bootstrapMethodAttrIndex, nameAndTypeIndex);
            return true;
        }

        /// <summary>
        /// Parses a InvokeDynamic constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadInvokeDynamicConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort bootstrapMethodAttrIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new InvokeDynamicConstantRecord(bootstrapMethodAttrIndex, nameAndTypeIndex);
            return true;
        }

        /// <summary>
        /// Parses a Module constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadModuleConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;

            constant = new ModuleConstantRecord(nameIndex);
            return true;
        }

        /// <summary>
        /// Parses a Package constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        static bool TryReadPackageConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;

            constant = new PackageConstantRecord(nameIndex);
            return true;
        }

        /// <summary>
        /// Attempts to read the set of interfaces starting from the current position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        static bool TryReadInterfaces(ref SequenceReader<byte> reader, out InterfaceInfoRecord[] interfaces)
        {
            interfaces = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            interfaces = new InterfaceInfoRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadInterface(ref reader, out InterfaceInfoRecord iface) == false)
                    return false;

                interfaces[i] = iface;
            }

            return true;
        }

        /// <summary>
        /// Parses an interface.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="iface"></param>
        static bool TryReadInterface(ref SequenceReader<byte> reader, out InterfaceInfoRecord iface)
        {
            iface = default;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;

            iface = new InterfaceInfoRecord(classIndex);
            return true;
        }

        /// <summary>
        /// Attempts to read the set of fields starting from the current position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        static bool TryReadFields(ref SequenceReader<byte> reader, out FieldInfoRecord[] fields)
        {
            fields = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            fields = new FieldInfoRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadField(ref reader, out FieldInfoRecord field) == false)
                    return false;

                fields[i] = field;
            }

            return true;
        }

        /// <summary>
        /// Parses a field.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constants"></param>
        /// <param name="field"></param>
        static bool TryReadField(ref SequenceReader<byte> reader, out FieldInfoRecord field)
        {
            field = default;

            if (reader.TryReadBigEndian(out ushort accessFlags) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                return false;
            if (TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            field = new FieldInfoRecord((AccessFlag)accessFlags, nameIndex, descriptorIndex, attributes);
            return true;
        }

        /// <summary>
        /// Attempts to read the set of methods starting from the current position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="methods"></param>
        /// <returns></returns>
        static bool TryReadMethods(ref SequenceReader<byte> reader, out MethodInfoRecord[] methods)
        {
            methods = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            methods = new MethodInfoRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadMethod(ref reader, out MethodInfoRecord method) == false)
                    return false;

                methods[i] = method;
            }

            return true;
        }

        /// <summary>
        /// Parses a method.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="method"></param>
        static bool TryReadMethod(ref SequenceReader<byte> reader, out MethodInfoRecord method)
        {
            method = default;

            if (reader.TryReadBigEndian(out ushort accessFlags) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                return false;
            if (TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            method = new MethodInfoRecord((AccessFlag)accessFlags, nameIndex, descriptorIndex, attributes);
            return true;
        }

        /// <summary>
        /// Parses an attributes count followed by a sequence of attributes.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attributes"></param>
        internal static bool TryReadAttributes(ref SequenceReader<byte> reader, out AttributeInfoRecord[] attributes)
        {
            attributes = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            attributes = new AttributeInfoRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadAttribute(ref reader, out var attribute) == false)
                    return false;

                attributes[i] = attribute;
            }

            return true;
        }

        /// <summary>
        /// Parses an attribute.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attribute"></param>
        static bool TryReadAttribute(ref SequenceReader<byte> reader, out AttributeInfoRecord attribute)
        {
            attribute = default;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out uint length) == false)
                return false;
            if (reader.TryReadExact((int)length, out var info) == false)
                return false;

            var infoBuffer = new byte[info.Length];
            info.CopyTo(infoBuffer);

            attribute = new AttributeInfoRecord(nameIndex, infoBuffer);
            return true;
        }

    }

}
