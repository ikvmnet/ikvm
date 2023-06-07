namespace IKVM.ByteCode.Parsing
{

    public record struct ClassRecord(ushort MinorVersion, ushort MajorVersion, ConstantRecord[] Constants, AccessFlag AccessFlags, ushort ThisClassIndex, ushort SuperClassIndex, InterfaceRecord[] Interfaces, FieldRecord[] Fields, MethodRecord[] Methods, AttributeInfoRecord[] Attributes)
    {

        const uint MAGIC = 0xCAFEBABE;

        /// <summary>
        /// Attempts to read a class record starting at the current position.
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static bool TryRead(ref ClassFormatReader reader, out ClassRecord clazz)
        {
            clazz = default;

            if (reader.TryReadU4(out uint magic) == false)
                return false;
            if (magic != MAGIC)
                throw new ByteCodeException($"Unexpected magic value '{magic}'.");

            if (reader.TryReadU2(out ushort minorVersion) == false)
                return false;
            if (reader.TryReadU2(out ushort majorVersion) == false)
                return false;

            if (majorVersion > 63)
                throw new UnsupportedClassVersionException(new ClassFormatVersion(majorVersion, minorVersion));

            if (TryReadConstants(ref reader, out var constants) == false)
                return false;

            if (reader.TryReadU2(out ushort accessFlags) == false)
                return false;

            if (reader.TryReadU2(out ushort thisClass) == false)
                return false;

            if (reader.TryReadU2(out ushort superClass) == false)
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
        static bool TryReadConstants(ref ClassFormatReader reader, out ConstantRecord[] constants)
        {
            constants = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            constants = new ConstantRecord[count];
            for (int i = 1; i < count; i++)
            {
                if (ConstantRecord.TryRead(ref reader, out var constant, out var skip) == false)
                    return false;

                constants[i] = constant;
                i += skip;
            }

            return true;
        }

        /// <summary>
        /// Attempts to read the set of interfaces starting from the current position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        static bool TryReadInterfaces(ref ClassFormatReader reader, out InterfaceRecord[] interfaces)
        {
            interfaces = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            interfaces = new InterfaceRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (InterfaceRecord.TryRead(ref reader, out InterfaceRecord iface) == false)
                    return false;

                interfaces[i] = iface;
            }

            return true;
        }

        /// <summary>
        /// Attempts to read the set of fields starting from the current position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        static bool TryReadFields(ref ClassFormatReader reader, out FieldRecord[] fields)
        {
            fields = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            fields = new FieldRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (FieldRecord.TryRead(ref reader, out FieldRecord field) == false)
                    return false;

                fields[i] = field;
            }

            return true;
        }

        /// <summary>
        /// Attempts to read the set of methods starting from the current position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="methods"></param>
        /// <returns></returns>
        static bool TryReadMethods(ref ClassFormatReader reader, out MethodRecord[] methods)
        {
            methods = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            methods = new MethodRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (MethodRecord.TryRead(ref reader, out MethodRecord method) == false)
                    return false;

                methods[i] = method;
            }

            return true;
        }

        /// <summary>
        /// Parses an attributes count followed by a sequence of attributes.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attributes"></param>
        internal static bool TryReadAttributes(ref ClassFormatReader reader, out AttributeInfoRecord[] attributes)
        {
            attributes = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            attributes = new AttributeInfoRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (AttributeInfoRecord.TryReadAttribute(ref reader, out var attribute) == false)
                    return false;

                attributes[i] = attribute;
            }

            return true;
        }

    }

}
