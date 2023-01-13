using System;
using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Represents the raw unlinked structure of a Class.
    /// </summary>
    class ClassRecord : IClassParserHandler
    {

        ushort minorVersion;
        ushort majorVersion;
        public ConstantRecord[] constants;
        AccessFlag accessFlags;
        ushort thisClassConstantIndex;
        ushort superClassConstantIndex;
        InterfaceRecord[] interfaces;
        FieldRecord[] fields;
        MethodRecord[] methods;
        AttributeRecord[] attributes;

        int nextInterfaceIndex;
        int nextFieldIndex;
        int nextMethodIndex;
        int nextAttributeIndex;

        void IClassParserHandler.AcceptVersion(ushort majorVersion, ushort minorVersion)
        {
            this.majorVersion = majorVersion;
            this.minorVersion = minorVersion;
        }

        void IClassParserHandler.AcceptConstantCount(ushort count)
        {
            constants = new ConstantRecord[count];
        }

        void IClassParserHandler.AcceptUtf8Constant(int index, in ReadOnlySequence<byte> value)
        {
            var r = new SequenceReader<byte>(value);
            if (r.TryReadExact((int)r.Length, out var utf8) == false)
                throw new ClassReaderException("Invalud UTF8 constant sequence.");

            var b = new byte[utf8.Length];
            utf8.CopyTo(b);
            constants[index] = new Utf8ConstantRecord(b);
        }

        void IClassParserHandler.AcceptIntegerConstant(int index, int value)
        {
            constants[index] = new IntegerConstantRecord(value);
        }

        void IClassParserHandler.AcceptFloatConstant(int index, float value)
        {
            constants[index] = new FloatConstantRecord(value);
        }

        void IClassParserHandler.AcceptLongConstant(int index, long value)
        {
            constants[index] = new LongConstantRecord(value);
        }

        void IClassParserHandler.AcceptDoubleConstant(int index, double value)
        {
            constants[index] = new DoubleConstantRecord(value);
        }

        void IClassParserHandler.AcceptClassConstant(int index, ushort utf8Index)
        {
            constants[index] = new ClassConstantRecord(utf8Index);
        }

        void IClassParserHandler.AcceptStringConstant(int index, ushort utf8Index)
        {
            constants[index] = new StringConstant(utf8Index);
        }

        void IClassParserHandler.AcceptFieldrefConstant(int index, ushort classIndex, ushort nameAndTypeIndex)
        {
            constants[index] = new FieldrefConstantRecord(classIndex, nameAndTypeIndex);
        }

        void IClassParserHandler.AcceptMethodrefConstant(int index, ushort classIndex, ushort nameAndTypeIndex)
        {
            constants[index] = new MethodrefConstantRecord(classIndex, nameAndTypeIndex);
        }

        void IClassParserHandler.AcceptInterfaceMethodrefConstant(int index, ushort classIndex, ushort nameAndTypeIndex)
        {
            constants[index] = new InterfaceMethodrefConstant(classIndex, nameAndTypeIndex);
        }

        void IClassParserHandler.AcceptNameAndTypeConstant(int index, ushort nameIndex, ushort descriptorIndex)
        {
            constants[index] = new TypeAndNameConstantRecord(nameIndex, descriptorIndex);
        }

        void IClassParserHandler.AcceptMethodHandleConstant(int index, ReferenceKind kind, ushort referenceIndex)
        {
            constants[index] = new MethodHandleConstantRecord(kind, referenceIndex);
        }

        void IClassParserHandler.AcceptMethodTypeConstant(int index, ushort descriptorIndex)
        {
            constants[index] = new MethodTypeConstantRecord(descriptorIndex);
        }

        void IClassParserHandler.AcceptDynamicConstant(int index, ushort bootstrapMethodAttrIndex, ushort nameAndTypeIndex)
        {
            constants[index] = new DynamicConstantRecord(bootstrapMethodAttrIndex, nameAndTypeIndex);
        }

        void IClassParserHandler.AcceptInvokeDynamicConstant(int index, ushort bootstrapMethodAttrIndex, ushort nameAndTypeIndex)
        {
            constants[index] = new InvokeDynamicConstant(bootstrapMethodAttrIndex, nameAndTypeIndex);
        }

        void IClassParserHandler.AcceptModuleConstant(int index, ushort nameIndex)
        {
            constants[index] = new ModuleConstantRecord(nameIndex);
        }

        void IClassParserHandler.AcceptPackageConstant(int index, ushort nameIndex)
        {
            constants[index] = new PackageConstantRecord(nameIndex);
        }

        void IClassParserHandler.AcceptAccessFlags(AccessFlag accessFlags)
        {
            this.accessFlags = accessFlags;
        }

        void IClassParserHandler.AcceptThisClass(ushort thisClass)
        {
            thisClassConstantIndex = thisClass;
        }

        void IClassParserHandler.AcceptSuperClass(ushort superClass)
        {
            superClassConstantIndex = superClass;
        }

        void IClassParserHandler.AcceptInterfaceCount(ushort count)
        {
            interfaces = new InterfaceRecord[count];
            nextInterfaceIndex = 0;
        }

        void IClassParserHandler.AcceptInterface(ushort classIndex)
        {
            try
            {
                interfaces[nextInterfaceIndex++] = new InterfaceRecord(classIndex);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ClassReaderException("Interface outsidie interface count.", e);
            }
        }

        void IClassParserHandler.AcceptFieldCount(ushort count)
        {
            fields = new FieldRecord[count];
            nextFieldIndex = 0;
        }

        void IClassParserHandler.AcceptField(AccessFlag accessFlags, ushort nameIndex, ushort descriptorIndex, out IClassParserAttributeHandler attributes)
        {
            try
            {
                attributes = fields[nextFieldIndex++] = new FieldRecord(accessFlags, nameIndex, descriptorIndex);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ClassReaderException("Field outside field count.", e);
            }
        }

        void IClassParserHandler.AcceptMethodCount(ushort count)
        {
            methods = new MethodRecord[count];
            nextMethodIndex = 0;
        }

        void IClassParserHandler.AcceptMethod(AccessFlag accessFlags, ushort nameIndex, ushort descriptorIndex, out IClassParserAttributeHandler attributes)
        {
            try
            {
                attributes = methods[nextMethodIndex++] = new MethodRecord(accessFlags, nameIndex, descriptorIndex);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ClassReaderException("Method outside method count.", e);
            }
        }

        void IClassParserAttributeHandler.AcceptAttributeCount(int count)
        {
            attributes = new AttributeRecord[count];
            nextAttributeIndex = 0;
        }

        void IClassParserAttributeHandler.AcceptAttribute(ushort nameIndex, in ReadOnlySequence<byte> info)
        {
            var r = new SequenceReader<byte>(info);
            if (r.TryReadExact((int)r.Length, out var buffer) == false)
                throw new ClassReaderException("Invalid buffer length.");

            var b = new byte[buffer.Length];
            buffer.CopyTo(b);

            attributes[nextAttributeIndex++] = new AttributeRecord(nameIndex, b);
        }

        /// <summary>
        /// Resolves the class record.
        /// </summary>
        /// <returns></returns>
        public Class Resolve()
        {
            var c = new Class();
            c.MinorVersion = minorVersion;
            c.MajorVersion = majorVersion;
            c.AccessFlags = accessFlags;
            c.Name = ResolveClassConstantName(thisClassConstantIndex);
            c.SuperName = ResolveClassConstantName(superClassConstantIndex);

            foreach (var i in interfaces)
                c.Interfaces.Add(i.Resolve(this));

            foreach (var i in fields)
                c.Fields.Add(i.Resolve(this));

            foreach (var i in methods)
                c.Methods.Add(i.Resolve(this));

            foreach (var i in attributes)
                c.Attributes.Add(i.Resolve(this));

            return c;
        }

        /// <summary>
        /// Resolves the specified constant pool index of type UTF8 into a string.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ClassReaderException"></exception>
        public string ResolveUtf8Constant(ushort index)
        {
            if (constants[index] is not Utf8ConstantRecord utf8)
                throw new ClassReaderException($"Attempt to resolve UTF8 constant {index} failed.");

            return utf8.ToString();
        }

        /// <summary>
        /// Resolves the specified constant pool index of type Class into its name string.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ClassReaderException"></exception>
        public string ResolveClassConstantName(ushort index)
        {
            if (constants[index] is not ClassConstantRecord constant)
                throw new ClassReaderException($"Attempt to resolve Class constant {index} failed.");
            else
                return ResolveUtf8Constant(constant.NameIndex);
        }

    }

}
